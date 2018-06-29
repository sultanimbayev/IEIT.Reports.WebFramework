# IEIT.Reports.WebFramework
[Read in english](README-Eng.md)

Фреймворк для выгрузки файлов в ASP.NET MVC.

## Установить

Установка через менеджер пакетов NuGet:

```
PM> Install-Package IEIT.Reports.WebFramework.Api -Version 1.3.0
```

## Как использовать

После установки у вас появится файл `ReportExportController.cs` в папке `~\Controllers\`. В котором вы найдете константу:

```C#
private const string API_ROUTE_BASE = "api/Files/DownloadForm/";
```

Это путь выгрузке ваших файлов. Запомните его чтобы вводить в браузер для проверки и выгрузки ваших файлов. 

Далле, вам нужно будет создать репозитории и обработчик.
Репозиторий и обработчик это обычные классы реализующие определенные интерфейсы.
Репозиторий хранит всю информацию о том, как создать файл, а обработчик используя эту информацию, создает файл.
Создание репозитория, пример:

```C#
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
public class MyFileRepo : IRepository
{
    public string Content { get; set; }

    public void Init(NameValueCollection queryParams)
    {
        var name = queryParams["name"] ?? "всем";
        Content = $"Привет {name}!";
    }
}
```

Нужно чтобы класс репозитория имел два аттрибута: `RepositoryFor` и `HasHandler`, а также чтобы был реализован
интерфейс `IRepository`.

C помощью аттрибута `RepositoryFor` вы устанавливаете путь по которому вы сможете скачивать файл. В данном примере это *MyFile*.
Соответственно для скачивания этого файла будет путь *api/Files/DownloadForm/MyFile*

Аттрибутом `HasHandler` мы задаем обработчик (его создание описано ниже).

Вы можете не реализовывать интерфейс `IRepository`, но тогда вам нужно будет создать конструктор который принимает объект `NameValueCollection` как параметр. (Я рекомендую использовать интерфейс во избежание непонимания).

Вы можете создать класс репозитория в любом месте вашего решения. Можно даже в другом проекте, лишь бы он был связан с проектом в котором находится контроллер, и нужно чтобы там был установлен NuGet пакет *IEIT.Reports.WebFramework.Core* той же версии что и *IEIT.Reports.WebFramework.Api*.

`queryParams` - это параметры запроса выгружаемого файла методом GET

Обработчик:

```C#
public class MyFileHandler : IHandler
{
    MyFileRepo repo;
    public void InitializeRepo(object repository)
    {
        repo = (MyFileRepo)repository;
    }

    public void GenerateFiles(string inDir)
    {
        var filePath = Path.Combine(inDir, "file.txt");
        File.WriteAllText(filePath, repo.Content);
    }

}
```

Обработчик должен реализвывать интерфейс `IHandler`
Этот интерфейс теребут два метода (которые показаны в примере).

В методе `InitializeRepo` желательно привести объект репозитория в определенный тип, и сохранить его
для получения информации из него.

`GenerateFiles` - метод в котором мы создаем файлы которые будут переданы клиенту запросившего файл.

`inDir` это путь к папке где следует создать выгружаемые файлы

Теперь вы можете выгрузить свой файл по ссылке `/api/Files/DownloadForm/MyFile?name=Sultan`

Обработчик, как и Репозиторий, может быт помещен в любом месте вашего решения. 
Это нормально иметь Репозиторий и Обработчик в разных проектах решения, 
в таком случае вам нужно чтобы был установлен NuGet пакет *IEIT.Reports.WebFramework.Core* в этих проектах.

### Аттрибут ReturnsZip

Если по указанному пути, `inDir`, вы создадите один файл, то этот файл будет возвращен клиенту.
Но, если вы создадите там несколько файлов, тогда клиенту будет передан .zip архив с этими файлами.
Вы можете использовать данный аттрибут, если хотите задать имя архива.

Например:

```C#
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
[ReturnsZip("Имя архива такой, какой я указал.zip")]
public class MyFileRepo : IRepository
{
    //...
}
```


Можно не указывать имя архива. Тогда, имя архива будет указано как указано в константе `DEFAULT_ZIP_NAME`
в конторллере `ReportExportController. По умолчанию:

```C#
private const string DEFAULT_ZIP_NAME = "Выходные данные.zip";
```

> Если вы установили аттрибут `ReturnsZip`, то при выгрузке будет возвращен  архив
> вне зависимости от того, сколько файлов вы сгенерировали в папке `inDir`. Даже если
> вы создали там один файл, он будет сжат в архив и передан клиенту запросившего файл.

### Аттрибут DisplayName

Если вам нужно указать и передать наименование отчета, 
то вы можете использовать аттрибут DisplayName.

Например:

```C#
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
[DisplayName(DisplayLanguage.Russian, "Отчет о данных пользователя")]
public class MyFileRepo : IRepository
{
    //...
}
```

Не забудьте включить пространства имен:

```C#
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Enum;
```


Чтобы получить указанные названия, используйте класс `RepositoryResolver` который
находится в пространстве `IEIT.Reports.WebFramework.Api.Resolvers`
Например, чтобы получить название отчета на примере выше:

```C#
var name = RepositoryResolver.GetDisplayName("MyFile", DisplayLanguage.Russian);
Console.WriteLine($"Название отчета: {name}");
```


Можно задавать несколько аттрибутов `DisplayName` с  разными значениями `DisplayLanguage`, то есть на разных языках.
Затем, указывать соответсвующий язык при получении наименования.


Вы также можете получить названия всех отчетов с именами их классов:
```C#
var names = RepositoryResolver.GetDisplayNames(DisplayLanguage.Russian);
foreach(var pair in names)
{
	Console.WriteLine($"Название: {pair.Value}, класс: {pair.Key}");
}
```


### TemplateResolver

##### Настройка

Если ваш проект содержит файл конфигурационный файл *(App.config)*, то после установки данного пакета, 
должна появиться следующая строка строка в секции `appSettings`:
```
<add key="WebFramework.TemplatesPath" value="..\..\TemplateFiles" />
```
Если же строка не появилась, то добавьте его вручную в соответствующую секцию.

Значение *value* данной настройки содержит путь, где будут храниться ваши файлы-шаблоны. Этот путь относителе к
папке с бинарными файлами. Обычно это папка *bin* в ASP.NET или *bin/Debug* в консольных приложениях.
Измените данное значение на то, где будет храниться ваша папка с шаблонами.

Затем, создайте эту папку и перенесите туда необходимые вам файлы.

#### Использование

Допустим, что вы поместили файл *readme.txt* в папку с шаблонами.
Тогда, вы можете получить полный путь к этому файлу с помощью следующих строк:

```C#
var path = TemplateResolver.ResolveFilePath("readme.txt");
Console.WriteLine(path);
```

либо проще:

```C#
var path = TemplateResolver.ResolveFilePath("readme");
Console.WriteLine(path);
```

Все методы находятся в классе TemplateResolver.
Не забудьте добавить пространство имен:
```C#
using IEIT.Reports.WebFramework.Core.Resolvers;
```

Можно не указывать некоторые расширения файлов, они автоматический подставятся.
На данный момент можно не указывать названия следующих расширении:
```
txt|pdf|rtf|ppt|pptx|xls|xlsx|doc|docx
```

Вы спокойно можете хранить свои шаблоны в подпапках. Получить к ним полный путь можно будет так:

```C#
var path = TemplateResolver.ResolveFilePath("подпапка\\readme");
Console.WriteLine(path);
```

Если же вы по какой-то причине захотели получить путь к папке шаблонов,
то это можно слелать через метод `TemplateResolver.GetTemplatesDir()`


