using DbLogger.Core.Application;
using DbLogger.Core.Application.Dto;
using DbLogger.Core.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace DbLogger.Core
{


    /// <summary>
    /// 
    /// </summary>
    public class DbLoggerProvider : ILoggerProvider
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly DbLoggerOptions _options;

        /// <summary>
        /// 
        /// </summary>
        public DbLoggerProvider(IServiceProvider serviceProvider, IOptions<DbLoggerOptions> options)
        {
            _serviceProvider = serviceProvider;
            _options = options != null ? options.Value : throw new ArgumentNullException(nameof(options));
        }





        /// <summary>
        /// 
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomDbLogger(_serviceProvider, categoryName, _options);
        }




        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }



    /// <summary>
    /// 
    /// </summary>
    public class CustomDbLogger : ILogger
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly DbLoggerOptions _options;
        private readonly string _loggerName;


        /// <summary>
        /// 
        /// </summary>
        public CustomDbLogger(IServiceProvider serviceProvider, string loggerName, DbLoggerOptions options)
        {
            _loggerName = loggerName;
            _options = options;
            _filter = GetFilter();
            _serviceProvider = serviceProvider;
        }



  
        
        /// <summary>
        /// 
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }




        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter(_loggerName, logLevel);
        }




        /// <summary>
        /// 
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (exception != null)
            {
                message = $"{message}{Environment.NewLine}{exception}";
            }

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var appLogItem = new AppLogInput
            {
                Url = httpContextAccessor?.HttpContext != null ? httpContextAccessor.HttpContext.Request.Path.ToString() : string.Empty,
                LogLevel = logLevel,
                Logger = _loggerName,
                Message = message,
                ApplicationName=_options.ApplicationName,
            };


            try
            {

                using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<ILoggerUnitOfWork>();
                    var appLogItemsService = serviceScope.ServiceProvider.GetRequiredService<IAppLogService>();

                    //create log
                    appLogItemsService.Create(appLogItem);
                    context.SaveChanges();

                }

            }
            catch
            {
                // don't throw exceptions from logger
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private Func<string, LogLevel, bool> GetFilter()
        {
            return delegate (string loggerName, LogLevel logLevel)
            {
                return logLevel >= _options.logLevel;
            };
        }





        /// <summary>
        /// 
        /// </summary>

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }



}