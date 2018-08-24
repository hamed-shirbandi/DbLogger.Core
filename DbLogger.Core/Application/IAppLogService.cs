using DbLogger.Core.Application.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbLogger.Core.Application
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppLogService
    {
        Task CreateAsync(AppLogInput input);
        void Create(AppLogInput input);
        Task<AppLogOutput> GetAsync(long id);
        IEnumerable<AppLogOutput> Search(int page, int recordsPerPage, string term,string applicationName, LogLevel? logLevel, out int pageSize, out int totalItemCount);
        Task<int> CountAsync(LogLevel? logLevel = null);
        Task<IEnumerable<SelectListItem>> GetApplicationNameSelectListItemsAsync(string applicationName="");
    }
}