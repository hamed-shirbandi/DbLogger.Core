using System;
using Microsoft.Extensions.Logging;
using DbLogger.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DbLogger.Core.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace DbLogger.Core
{

    /// <summary>
    /// 
    /// </summary>
    public static class DbLoggerExtensions
    {


        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection AddDbLogger(this IServiceCollection services, Action<DbLoggerOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddScoped<ILoggerUnitOfWork, LoggerDbContext>();
            services.AddScoped<IAppLogService, AppLogService>();
            services.Configure(setupAction);
            return services;
        }

        



        /// <summary>
        /// 
        /// </summary>
        public static IApplicationBuilder UseDbLogger(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //init database - add log table to application db
                var context = serviceScope.ServiceProvider.GetRequiredService<LoggerDbContext>();
                context.Database.Migrate();
            }


            //set logger provider
            var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var provider = app.ApplicationServices.GetRequiredService<DbLoggerProvider>();
            factory.AddProvider(provider);


            //set routes 
            var options = app.ApplicationServices.GetRequiredService<IOptions<DbLoggerOptions>>();
            app.UseMvc(routes =>
            {
                routes.MapRoute("dbLoggerDefaultRoute", options.Value.Path, new { controller = "AppLogItems", action = "Index" });
                routes.MapRoute("dbLoggerDetailsRoute", options.Value.Path + "/{id}", new { controller = "AppLogItems", action = "Details", id = 0 });
            });

            //ignore route - prevent direct access to controller
            return app.UseMiddleware<IgnoreRoutesMiddleware>(); 
        }

    }
}