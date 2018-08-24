using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DbLogger.Core.Example.Models;
using DbLogger.Core.Application;
using DbLogger.Core.Application.Dto;
using Microsoft.Extensions.Logging;

namespace DbLogger.Core.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppLogService _appLogService;

        public HomeController(IAppLogService appLogService)
        {
            _appLogService = appLogService;
        }


        public async Task<IActionResult> Index()
        {
            var log = new AppLogInput
            {
                ApplicationName = "DbLogger.Core.Example",
                LogLevel= LogLevel.Information,
                Message="I See Home => Index",
            };
            await _appLogService.CreateAsync(log);

            return View();
        }




        public IActionResult Error()
        {
            throw new Exception("This is Test Exception From Home Controller");
        }
    }
}
