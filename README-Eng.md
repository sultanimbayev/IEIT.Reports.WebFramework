# IEIT.Reports.WebFramework

Framework for file downloading in ASP.NET MVC.

## Installing

Install using NuGet package manager:

```
PM> Install-Package IEIT.Reports.WebFramework.Api -Version 1.3.0
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
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
[ReturnsZip("My archive.zip")]
public class MyFileRepo : IRepository
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
[RepositoryFor("MyFile")]
[HasHandler("MyFileHandler")]
[DisplayName(DisplayLanguage.English, "Download my files")]
public class MyFileRepo : IRepository
{
    //...
}
```

Don't forget to include the namespaces:

```C#
using IEIT.Reports.WebFramework.Api.Resolvers;
using IEIT.Reports.WebFramework.Core.Enum;
```

Use the `RepositoryResolver` class to get the names you have assigned.
`RepositoryResolver` is placed in the `IEIT.Reports.WebFramework.Api.Resolvers` namespace.

For example, if you want to get the display name of the report in the example above:
```C#
var name = RepositoryResolver.GetDisplayName("MyFile", DisplayLanguage.English);
Console.WriteLine($"Name of the report is: {name}");
```

You can include multiple `DisplayName` attributes on a class with different value of `DisplayLanguage`.
It means that you can make display name of the report in several languages.

You can also, take the display names of all report classes with their corresponding class name:
```C#
var names = RepositoryResolver.GetDisplayNames(DisplayLanguage.English);
foreach(var pair in names)
{
	Console.WriteLine($"Display name: {pair.Value}, class name: {pair.Key}");
}
```

### TemplateResolver

##### Configuring

If your project contains the *(App.config)* cofiguration file, then after installation of the package the following line have to appear in the `appSettings` section:
```
<add key="WebFramework.TemplatesPath" value="..\..\TemplateFiles" />
```
If not, add it manually.

The *value* of this configuration is pointing to the directory of your template files. 
This path is relative to the binary files of your program.
Usualy it's *bin* folder in ASP.NET or *bin/Debug* in colnsole apps.
Change this value as you like or leave it as is.

Then, you should create that folder and place files you need.

##### Usage

Let's assume that you placed *readme.txt* file in the template files directory.
Then, you can get full path to that file using the following line of code:

```C#
var path = TemplateResolver.ResolveFilePath("readme.txt");
Console.WriteLine(path);
```

or you can do it even simpler:

```C#
var path = TemplateResolver.ResolveFilePath("readme");
Console.WriteLine(path);
```

All methods you need are placed in `TemplateResolver` (static) class.
Don't forget to add namespaces:
```C#
using IEIT.Reports.WebFramework.Core.Resolvers;
```

You can skip writing extensions of files when pointing to the file you want to get full path of.
In the current version, you can miss the following file extensions:
```
txt|pdf|rtf|ppt|pptx|xls|xlsx|doc|docx
```

Feel free to create subdirectories in the directory where you store your template files. 
You can access file's path in subdirectories with the following code:

```C#
var path = TemplateResolver.ResolveFilePath("subdirectory\\readme");
Console.WriteLine(path);
```

You can get path to the root directory with templates using `TemplateResolver.GetTemplatesDir()` method.
