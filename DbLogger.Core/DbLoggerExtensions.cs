using System;
using Microsoft.Extensions.Logging;
using DbLogger.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DbLogger.Core.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using DbLogger.Core.Application.Dto;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using DbLogger.Core.Controllers;

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

            services.AddHttpContextAccessor();
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


            //set logger provider
            var factory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var options = app.ApplicationServices.GetRequiredService<IOptions<DbLoggerOptions>>();
            factory.AddProvider(new DbLoggerProvider(app.ApplicationServices, options));


            //migrate db
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ILoggerUnitOfWork>();
                context.Migrate();

            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("dbLoggerDefaultRoute", options.Value.Path, new { controller = "AppLogs", action = "Index" });
                endpoints.MapControllerRoute("dbLoggerDetailsRoute", options.Value.Path + "/{id}", new { controller = "AppLogs", action = "Details", id = 0 });
            });


            //ignore route - prevent direct access to controller
            return app.UseMiddleware<IgnoreRoutesMiddleware>();
        }




        /// <summary>
        /// 
        /// </summary>
        public static void ConfigureDbLoggerPlugin(this ApplicationPartManager applicationPartManager)
        {

            var dBLoggerAssembly = typeof(AppLogsController).Assembly;
            var dBLoggerPartFactory = ApplicationPartFactory.GetApplicationPartFactory(dBLoggerAssembly);

            foreach (var part in dBLoggerPartFactory.GetApplicationParts(dBLoggerAssembly))
            {
                applicationPartManager.ApplicationParts.Add(part);
            }

            var dBLoggerRelatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(dBLoggerAssembly, throwOnError: true);
            foreach (var assembly in dBLoggerRelatedAssemblies)
            {
                dBLoggerPartFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var part in dBLoggerPartFactory.GetApplicationParts(assembly))
                {
                    applicationPartManager.ApplicationParts.Add(part);
                }
            }


        }

    }
}