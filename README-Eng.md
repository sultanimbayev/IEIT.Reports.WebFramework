# IEIT.Reports.WebFramework

Framework for file downloading in ASP.NET MVC.

## Installing

Install using NuGet package manager:

```
PM> Install-Package IEIT.Reports.WebFramework.Api -Version 1.2.1
```

## How to use

After installation, `ReportExportController.cs` will be created in `~\Controllers\` directory. In which you will find the following constant:

```C#
private const string API_ROUTE_BASE = "api/Files/DownloadForm/";
```

It is the url for downloading. Remember it to download and check your files that you make downloadable. 

To make downloadable file, you need to make a *Repository* and a *Handler* for it.
*Repository* and *Handler* are both classes that implement some interfaces.
*Repository* is for getting and storing the information about new file that is going to be created. *Handler* makes that file using information *Repository* provides.
An example of *Repository* class:

```C#
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
public class MyFileRepo : IRepository
{
    public string Content { get; set; }

    public void Init(NameValueCollection queryParams)
    {
        var name = queryParams["name"] ?? "there";
        Content = $"Hello {name}!";
    }
}
```

*Repository* class have to have `RepositoryFor` and`HasHandler` attribures, and Implement `IRepository` interface.

`RepositoryFor` attribure helps you to define the final part of path that you use to download file. 
In the example above, it is defined as *MyFile*. So, the *uri* to download this file will be as *api/Files/DownloadForm/MyFile*.

We reference to the handler that serves instances of the repositories with help of `HasHandler` attribute. 
(How to create *Handlers* is described below)

You may not to implement `IRepository`, but then, you will need to create a constructor 
that takes the only argument of `NameValueCollection` type. (I recommend to use an interface to avoid unclarity).

Create repository in any place of your solution. 
Yes, you can place it in another project. If you go so, you must reference 
that project to the project where the controller is. 
And also you will need to add IEIT.Reports.WebFramework.Core* package 
at the same version as *IEIT.Reports.WebFramework.Api* from NuGet.

Вы можете создать класс репозитория в любом месте вашего решения (можно даже в другом проекте, лишь бы он был связан с проектом в котором находится контроллер, и нужно чтобы там был установлен пакет *IEIT.Reports.WebFramework.Core* той же версии что и *IEIT.Reports.WebFramework.Api*).

`queryParams` - GET Parameters of the request

Making a handler:

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

*Handler* must implement `IHandler` interface.
That interface requres to methods to be implemented (that are shown in the example above).

In recommends to cast repository to the custom type and store it in some variable in the `InitializeRepo` method.

`GenerateFiles` is the method where we create the file/files that are to be sent to the client that requested them.

`inDir` contains the path where file/files should be created.

Now you can download this file using the next url `/api/Files/DownloadForm/MyFile?name=Sultan`

*Handlers* are like *Repositories* can be created in any place of your solution.  You can store *Repository* and *Handler* in different projects, the only thing you need is the `IEIT.Reports.WebFramework.Core` package installed in that projects.

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