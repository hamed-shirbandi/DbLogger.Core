using Microsoft.Extensions.Logging;
using System;

namespace DbLogger.Core.Domain
{

    /// <summary>
    /// 
    /// </summary>
    public class AppLog : BaseEntity
    {
        
        public string Url { get; set; }

        public LogLevel LogLevel { get; set; }


        public string Logger { get; set; }

        public string Message { get; set; }
        public string ApplicationName { get; set; }

    }
}