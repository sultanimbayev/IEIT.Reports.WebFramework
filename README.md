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
