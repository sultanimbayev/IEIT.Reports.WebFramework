# IEIT.Reports.WebFramework

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

Далее, создайте класс в любом месте вашего проекта (можно даже в другом проекте, лишь бы он был связан с проектом в котором находится контроллер). Затем добавьте следующие пространства имен в свой код:

```C#
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
```

Добавьте атрибут Report, добавьте конструктор принимающий объект NameValueCollection и реализуйте интерфейс IReport в своем классе. Пример:

```C#
[Report]
public class MyFileReport: IReport
{
    public MyFileReport(NameValueCollection queryParams)
    {
    }
    public void GenerateFiles(string inDir)
    {
            var path = Path.Combine(inDir, "newFile.txt");
            File.WriteAllText(path, "Hello there!");
    }

}
```

Все, теперь вы можете выгрузить свой файл по ссылке `api/Files/DownloadForm/MyFile

queryParams - это параметры запроса выгружаемого файла методом GET
