using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbLogger.Core
{


    /// <summary>
    /// 
    /// </summary>
    public class IgnoreRoutesMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly DbLoggerOptions _options;


        /// <summary>
        /// 
        /// </summary>
        public IgnoreRoutesMiddleware(RequestDelegate next,IOptions<DbLoggerOptions> options)
        {
            _options = options.Value;
            _next = next;
        }



        /// <summary>
        /// 
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue &&context.Request.Path.Value.ToLower()== "/applogs")
            {
                context.Response.StatusCode = 404;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
