

using DbLogger.Core;

var builder = WebApplication.CreateBuilder(args);

//add DbLogger Service
builder.Services.AddDbLogger(options =>
{
    options.logLevel = LogLevel.Error;
    options.Path = "DbLogs";//use this in url to show logs on view
    options.ApplicationName = "DbLogger.Core.Example";
});



 builder.Services.AddControllersWithViews();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseStaticFiles();
app.UseRouting();

//add DbLogger middleware
app.UseDbLogger();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
