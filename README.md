# IEIT.Reports.WebFramework

[Read the english version](README-eng.md)

Фреймворк для выгрузки файлов в ASP.NET MVC.

## Установить

Установка через менеджер пакетов NuGet:

```
PM> Install-Package IEIT.Reports.WebFramework.Api
```

## Как использовать

Cоздайте класс в любом месте вашего web проекта и добавьте следующие пространства имен:

```C#
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
```

Затем:
 1. Добавьте атрибут `Report` к созданному классу.
 2. Реализуйте интерфейс `IReport` в своем классе. 

 Пример:

```C#
[Report]
public class MyFile : IReport
{
    public void GenerateFiles(NameValueCollection queryParams, string inDir)
    {
        var name = queryParams["name"] ?? "there";
        var path = Path.Combine(inDir, "newFile.txt");
        File.WriteAllText(path, $"Hello {name}!");
    }
}
```

Теперь вы можете выгрузить свой файл по ссылке `/api/Files/DownloadForm/MyFile?name=Sultan`

`queryParams` - это параметры запроса выгружаемого файла методом GET

`inDir` - путь к папке где следует создать выгружаемые файлы

> Если использовать аттрибут `[Report]` без параметров, то название класса под аттрибутом будет определять
> конечный вид ссылки на выгрузку. Если вы хотите переопределить это название, то передайте это значение как
> параметр в аттрибут `[Report]`. Например `[Report("NotMyFile")]`

Чтобы такой класс создать в другом проекте нужно добавить связь (Reference) с проектом в котором находится контроллер и установить 
туда пакет IEIT.Reports.WebFramework.Core.

### Как это работает?

После установки у вас появится файл `ReportExportController.cs` в папке `~\Controllers\`. 
В котором вы найдете константу:

> `~` это домашняя папка веб проекта

```C#
private const string API_ROUTE_BASE = "api/Files/DownloadForm/";
```

Это путь выгрузке ваших файлов. Запомните его чтобы вводить в браузер для проверки и выгрузки ваших файлов. 
Вся логика находится в этом контроллере, но детали скрыты в коде библиотеки, которые вы можете посмотреть тут на GitHub.

### Аттрибут ReturnsZip

Если по указанному пути, `inDir`, вы создадите не один файл а несколько, тогда клиенту будет 
передан .zip архив с этими файлами. Вы можете использовать данный аттрибут, если хотите 
задать имя архива.

Например:

```C#
[Report]
[ReturnsZip("Имя архива такой, какой я указал")]
public class MyFileReport: IReport
{
    public void GenerateFiles(NameValueCollection queryParams, string inDir)
    {
        //...
    }
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