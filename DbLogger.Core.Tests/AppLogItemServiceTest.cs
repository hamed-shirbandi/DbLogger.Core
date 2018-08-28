using DbLogger.Core.Application;
using DbLogger.Core.Application.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbLogger.Core.Tests
{
    [TestClass]
    public class AppLogItemServiceTest : TestsBase
    {
        public AppLogItemServiceTest()
        {

        }



        [TestMethod]
        public void Can_Create_Log()
        {
            RunScopedService<IAppLogService>(ServiceProvider,async appLogItemService =>
            {
                //Arrange
                var model = new AppLogInput
                {
                    Url = "/test",
                    LogLevel = LogLevel.Error,
                    Logger = "Test Logger",
                    Message = "Test Message",
                    ApplicationName = "Logging.Tests",
                };


                //Act
                await appLogItemService.CreateAsync(model);
                var appLogItem = await appLogItemService.GetAsync(1);

                //Assert
                Assert.AreEqual(model.ApplicationName, appLogItem.ApplicationName);

            });
        }





        [TestMethod]
        public void Test_Search_By_ApplicationName()
        {
            RunScopedService<IAppLogService>(ServiceProvider, async appLogItemService =>
            {
                //Arrange
                string applicationName = "Logging.Tests";

                //Act
                var logs = appLogItemService.Search(page: 1, recordsPerPage: 20, term: "", applicationName: applicationName, logLevel: null, pageSize: out int pageSize, totalItemCount: out int totalItemCount);
                var appLogItem = await appLogItemService.GetAsync(1);

                //Assert
                Assert.AreEqual(logs.Count(), 1);

            });
        }





        [TestMethod]
        public void Test_Search_By_LogLevel()
        {
            RunScopedService<IAppLogService>(ServiceProvider, async appLogItemService =>
            {
                //Arrange
                string applicationName = "Logging.Tests";
                var logLevel = LogLevel.Error;

                //Act
                var logs = appLogItemService.Search(page: 1, recordsPerPage: 20, term: "", applicationName: applicationName, logLevel: logLevel, pageSize: out int pageSize, totalItemCount: out int totalItemCount);
                var appLogItem = await appLogItemService.GetAsync(1);

                //Assert
                Assert.AreEqual(logs.Count(), 1);

            });
        }




    }
}
