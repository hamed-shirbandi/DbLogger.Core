using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbLogger.Core.Tests
{
    public class TestsBase
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public TestsBase()
        {
            ServiceProvider = GetServiceProvider();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                                    .AddInMemoryCollection(new[]
                                    {
                                        new KeyValuePair<string,string>("UseInMemoryDatabase", "true"),
                                    })
                                    .Build();

            services.AddSingleton<IConfiguration>(provider => configuration);

            services.AddDbLogger(options =>
            {
                options.logLevel = LogLevel.Error;
                options.Path = "logs";
                options.ApplicationName = "DbLogger.Core.Tests";
            });

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }





      

        /// <summary>
        ///
        /// </summary>
        protected static void RunScopedService<S>(IServiceProvider serviceProvider, Action<S> callback)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<S>();
                callback(context);
                if (context is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
        

    }
}