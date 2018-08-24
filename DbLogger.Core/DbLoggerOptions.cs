using Microsoft.Extensions.Logging;

namespace DbLogger.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class DbLoggerOptions
    {
        /// <summary>
        /// Specifies the path to view the logs.
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// Determines whether log statements should be logged based on the name of the logger
        /// </summary>
        public LogLevel logLevel { get; set; }



        /// <summary>
        /// Specifies Application Name To Filter Logs By Applications
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
