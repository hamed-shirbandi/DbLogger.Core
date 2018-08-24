# What is this ?

Log error/event into Database to be used in ASP.NET Core 2.1 projects

By using this library you will have the following features in your project:
- Automatically logging all of the events such as viewing an item, deleting an item, logging in, etc.
- Automatically logging all of the errors and events for each request based on LogLevel value
- Automatically adding  a log table to your database.
- Manually log anything in the application code.
- Url to see logs, with search and filtering capabilities.
- ...
# Install via NuGet

To install DbLogger.Core, run the following command in the Package Manager Console
```code
pm> Install-Package DbLogger.Core
```
You can also view the [package page](https://www.nuget.org/packages/DbLogger.Core) on NuGet.

# How to use ?


1- install package from nuget

2- add required services to Startup class as below :

```code
           services.AddDbLogger(options =>
             {
                 options.logLevel = LogLevel.Error; // Determines whether log statements should be logged 
                 options.Path = "DbLogs";//Specifies the path to view the logs in browser
                 options.ApplicationName = "DbLogger.Core.Example"; //Specifies Application Name To Filter Logs By Applications
             });
```
if you have more than 1 application that uses your Database, you can filter errors by their ApplicationName. so you must determine options.ApplicationNamevalue in all of them
 and On the other hand, if you need to collect all of your project's Logs, you can set a same connectionString by DbLoggerConnection in their appsettings.json
 
3- add middleware to Startup class as below :

```code
   app.UseDbLogger();
```
4- add following settings to appsettings.json

```code
    "ConnectionStrings": {
    "DbLoggerConnection": "Server=.;Database=TestDB;Trusted_Connection=True;MultipleActiveResultSets=true",
  }
```
5- To view list of logs, enter the url defined at step 3 in browser. like : www.site.com/DbLogs

6- after you lunch the app, a table add to your DB that defined by DbLoggerConnection in appsettings.json (AppLogs table)

# Screenshots

![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/1.jpg)
![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/2.jpg)
![alt text](https://github.com/hamed-shirbandi/DbLogger.Core/blob/master/DbLogger.Core.Example/wwwroot/images/3.png)
