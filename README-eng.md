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
