using Microsoft.Extensions.Logging;

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
