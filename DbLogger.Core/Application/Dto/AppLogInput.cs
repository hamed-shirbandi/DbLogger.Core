using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbLogger.Core.Application.Dto
{

    /// <summary>
    /// 
    /// </summary>
    public class AppLogInput
    {

        public string Url { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }
        public string ApplicationName { get; set; }

    }
}
