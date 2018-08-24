using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbLogger.Core.Application.Dto
{

    /// <summary>
    /// 
    /// </summary>
    public class AppLogOutput
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }
        public string ApplicationName { get; set; }

        public string CreateDateTime { get; set; }
    }
}
