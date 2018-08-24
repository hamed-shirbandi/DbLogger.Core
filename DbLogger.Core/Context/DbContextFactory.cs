using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace DbLogger.Core.Context
{
    public class CustomHostingEnvironment : IHostingEnvironment
    {
        public string EnvironmentName { get; set; } = "DbLogger.Core";
        public string ApplicationName { get; set; } = "DbLogger.Core";
        public string WebRootPath { get; set; } = Path.Combine(AppContext.BaseDirectory.Split(new[] { "bin" }, StringSplitOptions.RemoveEmptyEntries).First(), "wwwroot");
        public IFileProvider WebRootFileProvider { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory.Split(new[] { "bin" }, StringSplitOptions.RemoveEmptyEntries).First();
        public IFileProvider ContentRootFileProvider { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }


    /// <summary>
    /// Only used by EF Tooling
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<LoggerDbContext>
    {

        public LoggerDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddScoped<IHostingEnvironment, CustomHostingEnvironment>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            var serviceProvider = services.BuildServiceProvider();


            var hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(hostingEnvironment.ContentRootPath)
                                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                                .Build();

            return new LoggerDbContext(configuration);
        }
    }
}
