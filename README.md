# IEIT.Reports.WebFramework

[Read the english version](README-eng.md)

Фреймворк для выгрузки файлов в ASP.NET MVC.

## Установить

Установка через менеджер пакетов NuGet:

```
PM> Install-Package IEIT.Reports.WebFramework.Api
```

## Как использовать

(для версии 2.x.x)

После установки у вас появится файл `ReportExportController.cs` в папке `~\Controllers\`. В котором вы найдете константу:

```C#
private const string API_ROUTE_BASE = "api/Files/DownloadForm/";
```

Это путь выгрузке ваших файлов. Запомните его чтобы вводить в браузер для проверки и выгрузки ваших файлов. 

Далее, создайте класс в любом месте вашего решения (можно даже в другом проекте, лишь бы он был связан с проектом в котором находится контроллер, и нужно чтобы там был установлен пакет IEIT.Reports.WebFramework.Core).
Затем добавьте следующие пространства имен в свой код:

```C#
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
```

Сделайте следующие шаги для создание выгружаемого файла:
 1. Добавьте атрибут `Report` к созданному классу.
 2. Добавьте конструктор принимающий объект `NameValueCollection`.
 3. Реализуйте интерфейс `IReport` в своем классе. 

 Пример:

```C#
[Report]
public class MyFileReport: IReport
{
    public string Name { get; set; }

    public MyFileReport(NameValueCollection queryParams)
    {
        Name = queryParams["name"] ?? "there";
    }
    public void GenerateFiles(string inDir)
    {
        var path = Path.Combine(inDir, "newFile.txt");
        File.WriteAllText(path, $"Hello {Name}!");
    }

}
```

Все, теперь вы можете выгрузить свой файл по ссылке `/api/Files/DownloadForm/MyFile?name=Sultan`

`queryParams` - это параметры запроса выгружаемого файла методом GET

`inDir` - путь к папке где следует создать выгружаемые файлы


### Аттрибут ReturnsZip

Если по указанному пути, `inDir`, вы создадите один файл, то этот файл будет возвращен клиенту.
Но, если вы создадите там несколько файлов, тогда клиенту будет передан .zip архив с этими файлами.
Вы можете использовать данный аттрибут, если хотите задать имя архива.

Например:

```C#
[Report]
[ReturnsZip("Имя архива такой, какой я указал")]
public class MyFileReport: IReport
{
    //...
}
```


Можно не указывать имя архива. Тогда, имя архива будет указано как "Выходные данные".

> Если вы установили аттрибут `ReturnsZip`, то при выгрузке будет возвращен  архив
> вне зависимости от того, сколько файлов вы сгенерировали в папке `inDir`. Даже если
> вы создали там один файл, он будет сжат в архив и передан клиенту запросившего файл.

### Аттрибут DisplayName

Если вам нужно указать и передать наименование отчета, 
то вы можете использовать аттрибут DisplayName.

Например:

```C#
[Report]
[DisplayName(DisplayLanguage.Russian, "Отчет о данных пользователя")]
public class MyFileReport: IReport
{
    //...
}
```

Не забудьте включить пространства имен:

```C#
using IEIT.Reports.WebFramework.Api.Resolvers;
using IEIT.Reports.WebFramework.Core.Enum;
```


Чтобы получить указанные названия, используйте класс `LangUtils` который
находится в пространстве `IEIT.Reports.WebFramework.Api.Resolvers`
Например, чтобы получить название отчета на примере выше:

```C#
var name = LangUtils.GetDisplayName("MyFile", DisplayLanguage.Russian);
Console.WriteLine($"Название отчета: {name}");
```


Можно задавать несколько аттрибутов `DisplayName` с  разными значениями `DisplayLanguage`, то есть на разных языках.
Затем, указывать соответсвующий язык при получении наименования.


Вы также можете получить названия всех отчетов с именами их классов:
```C#
var names = LangUtils.GetDisplayNames(DisplayLanguage.Russian);
foreach(var pair in names)
{
	Console.WriteLine($"Название: {pair.Value}, класс: {pair.Key}");
}
```