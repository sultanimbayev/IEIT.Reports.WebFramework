# IEIT.Reports.WebFramework

Framework for file downloading in ASP.NET MVC.

## Installing

Install using NuGet package manager:

```
PM> Install-Package IEIT.Reports.WebFramework.Api
```

## How to use

(for versions 2.x.x)

After installation, `ReportExportController.cs` will be created in `~\Controllers\` directory. In which you will find the following constant:

```C#
private const string API_ROUTE_BASE = "api/Files/DownloadForm/";
```

It is the url for downloading. Remember it to download and check your files that you make downloadable. 

Next, create a class at any place in your solution. 
If you place this class in different project than the controller is, then you must add a reference to a project where your new class is, and install the IEIT.Reports.WebFramework.Core package in it.
Then, add the namespaces:

```C#
using IEIT.Reports.WebFramework.Core.Attributes;
using IEIT.Reports.WebFramework.Core.Interfaces;
```

Do the following steps to create a new downloadable file:
 1. Add the `Report` attribute to the class.
 2. Make a constructor that takes `NameValueCollection` object.
 3. Implement the `IReport` interface in your class. 

Example:

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

Voila! Now you can download your file via the `/api/Files/DownloadForm/MyFile?name=Sultan` url.

`queryParams` - GET Parameters of the request

`inDir` - directory path where your files that are to be downloaded, should be created.


### ReturnsZip attribute

If you create one file in the `inDir` directory, then, this file is sent to the client.
But, if you create there two or more files it will send a .zip archive containig that files.
Use ReturnsZip attribute to define the name of the archive.

Example:
```C#
[Report]
[ReturnsZip("My archive")]
public class MyFileReport: IReport
{
    //...
}
```

If you don't define the name of an archive it the name will be "Выходные данные" as default.

> If you have placed the `ReturnsZip` attribute, then it does not matter that do you create one file or more, it 
> will send all files in the archive anyway.

### DisplayName attribute

If you want to declare and share the display names of the reports you create, then, you can use this attribute.

Example:
```C#
[Report]
[DisplayName(DisplayLanguage.English, "Download my files")]
public class MyFileReport: IReport
{
    //...
}
```

Don't forget to include the namespaces:

```C#
using IEIT.Reports.WebFramework.Api.Resolvers;
using IEIT.Reports.WebFramework.Core.Enum;
```

Use the `LangUtils` class to get the names you have assigned.
`LangUtils` is placed in the `IEIT.Reports.WebFramework.Api.Resolvers` namespace.

For example, if you want to get the display name of the report in the example above:
```C#
var name = LangUtils.GetDisplayName("MyFile", DisplayLanguage.English);
Console.WriteLine($"Name of the report is: {name}");
```

You can include multiple `DisplayName` attributes on a class with different value of `DisplayLanguage`.
It means that you can make display name of the report in several languages.

You can also, take the display names of all report classes with their corresponding class name:
```C#
var names = LangUtils.GetDisplayNames(DisplayLanguage.English);
foreach(var pair in names)
{
	Console.WriteLine($"Display name: {pair.Value}, class name: {pair.Key}");
}
```