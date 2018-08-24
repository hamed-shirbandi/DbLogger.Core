# What is this ?

Log error/event into Database to be used in ASP.NET Core 2.1 projects

By using this library you will have the following features in your project:
- Automatically logging all of the errors and events for each request based on LogLevel.
- Automatically adding  a log table to your database.
- Manually log anything in the application code.
- Url to see logs, with search and filtering capabilities.
- ...
# Install via NuGet

To install DbLogger.Core, run the following command in the Package Manager Console.
```code
pm> Install-Package DbLogger.Core
```
You can also view the [package page](https://www.nuget.org/packages/DbLogger.Core) on NuGet.

# How to use ?


1- install package from nuget.

2- add required services to Startup class as below :

```code
           services.AddDbLogger(options =>
             {
                 options.logLevel = LogLevel.Error; // Determines whether log statements should be logged 
                 options.Path = "DbLogs";//Specifies the path to view the logs in browser
                 options.ApplicationName = "DbLogger.Core.Example"; //Specifies Application Name To Filter Logs By Applications
             });
```

 
3- add middleware to Startup class as below :

```code
   app.UseDbLogger();
```
4- add following settings to appsettings.json:

```code
    "ConnectionStrings": {
    "DbLoggerConnection": "Server=.;Database=TestDB;Trusted_Connection=True;MultipleActiveResultSets=true",
  }
```
5- To view list of logs, enter the url defined at step 3 in browser. like : www.site.com/DbLogs

6- after you lunch the app, a table add to your DB that defined by DbLoggerConnection in appsettings.json (AppLogs table).

# Important points
- if you have more than 1 application that uses your Database, you can filter errors by their ApplicationName. so you must determine options.ApplicationName value in all of them.

 - if you need to collect all of your project's Logs in one database, you can set a same connectionString by DbLoggerConnection in their appsettings.json.
 
 - if you need to manually log something in the application code, so you can use IAppLogService like bellow :
 ```code
 public class HomeController : Controller
    {
        private readonly IAppLogService _appLogService;

        public HomeController(IAppLogService appLogService)
        {
            _appLogService = appLogService;
        }


        public async Task<IActionResult> Index()
        {
            var log = new AppLogInput
            {
                ApplicationName = "DbLogger.Core.Example",
                LogLevel= LogLevel.Information,
                Message="I See Home => Index",
            };
            await _appLogService.CreateAsync(log);

            return View();
        }


    }
```
 
# Screenshots

![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/11.png)

![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/22.png)


![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/3.png)
