using Microsoft.AspNetCore.Mvc;
using DbLogger.Core.Context;
using Microsoft.Extensions.Logging;
using DbLogger.Core.Application;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;

namespace DbLogger.Core.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class AppLogsController : Controller
    {
        #region Fields

        private readonly ILoggerUnitOfWork _uow;
        private readonly IAppLogService _appLogItemService;
        private readonly DbLoggerOptions _options;

        private int recordsPerPage;
        private int pageSize;
        private int totalItemCount;

        #endregion

        #region Ctor



        /// <summary>
        /// 
        /// </summary>
        public AppLogsController(ILoggerUnitOfWork uow, IAppLogService appLogItemService, IOptions<DbLoggerOptions> options)
        {
            _uow = uow;
            _appLogItemService = appLogItemService;
            _options = options.Value;

            pageSize = 0;
            recordsPerPage = 8;
            totalItemCount = 0;
        }

        #endregion

        #region Public Methods



        /// <summary>
        /// 
        /// </summary>
        public IActionResult Index(int page = 1, string term = "", LogLevel? logLevel = null)
        {
            var appLogItems = _appLogItemService.Search(page: page, recordsPerPage: recordsPerPage, term: term, applicationName: _options.ApplicationName, logLevel: logLevel, pageSize: out pageSize, totalItemCount: out totalItemCount);

            #region ViewBags

            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = totalItemCount;


            #endregion

            return View(appLogItems);

        }







        /// <summary>
        /// 
        /// </summary>
        public async Task<IActionResult> Details(long id)
        {
            var appLogItem = await _appLogItemService.GetAsync(id);
            return View(appLogItem);
        }



        #endregion


        #region Private Methods





        #endregion

    }


}
