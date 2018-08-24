
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System;
using DbLogger.Core.Domain;
using DbLogger.Core.Context;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using DbLogger.Core.Application.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DbLogger.Core.Application
{


    /// <summary>
    /// 
    /// </summary>
    public class AppLogService : IAppLogService
    {
        #region Fileds


        private readonly DbSet<AppLog> _appLogs;
        private readonly ILoggerUnitOfWork _uow;


        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        public AppLogService(ILoggerUnitOfWork uow)
        {
            _uow = uow;
            _appLogs = _uow.Set<AppLog>();
        }


        #endregion

        #region Public Methods




        /// <summary>
        /// 
        /// </summary>
        public async Task CreateAsync(AppLogInput input)
        {
            var appLog = BindToDomainModel(input);

            await _appLogs.AddAsync(appLog);
            await _uow.SaveChangesAsync();
        }



        /// <summary>
        /// 
        /// </summary>
        public  void Create(AppLogInput input)
        {
            var appLog = BindToDomainModel(input);

             _appLogs.Add(appLog);
             _uow.SaveChanges();
        }




        /// <summary>
        /// 
        /// </summary>
        public async Task<AppLogOutput> GetAsync(long id)
        {
            var appLog = await _appLogs.FirstOrDefaultAsync(a=>a.Id==id);
            if (appLog==null)
            {
                throw new Exception("Log Not Found !");
            }

            return BindToOutputModel(appLog);
        }




        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AppLogOutput> Search(int page, int recordsPerPage, string term,string applicationName, LogLevel? logLevel, out int pageSize, out int totalItemCount)
        {
            var queryable = _appLogs.AsQueryable();

            #region By term

            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(a => a.Message.Contains(term)|| a.Logger.Contains(term));
            }

            #endregion

            #region By applicationName

            if (!string.IsNullOrEmpty(applicationName))
            {
                queryable = queryable.Where(a => a.ApplicationName== applicationName);
            }

            #endregion

            #region By logLevel

            if (logLevel.HasValue)
            {
                queryable = queryable.Where(a => a.LogLevel == logLevel);
            }

            #endregion

            #region sortOrder

            queryable = queryable.OrderByDescending(b => b.CreateDateTime);

            #endregion

            #region Take


            totalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)totalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;


            var skiped = (page - 1) * recordsPerPage;
            var appLogs = queryable.Skip(skiped).Take(recordsPerPage).ToList();


            #endregion

            return appLogs.Select(appLog=> BindToOutputModel(appLog));
        }








        /// <summary>
        /// 
        /// </summary>
        public async Task<int> CountAsync(LogLevel? logLevel = null)
        {
            var queryable = _appLogs.AsQueryable();

            #region By logLevel

            if (logLevel.HasValue)
            {
                queryable = queryable.Where(a=>a.LogLevel== logLevel);
            }

            #endregion

            return await queryable.CountAsync();
        }


        #endregion

        #region Private Methods


        /// <summary>
        /// 
        /// </summary>
        private AppLogOutput BindToOutputModel(AppLog appLog)
        {
            return new AppLogOutput
            {
                Id=appLog.Id,
                Logger=appLog.Logger,
                LogLevel=appLog.LogLevel,
                Message=appLog.Message,
                CreateDateTime=appLog.CreateDateTime.ToString(),
                Url=appLog.Url,
                ApplicationName = appLog.ApplicationName,

            };
        }



        /// <summary>
        /// 
        /// </summary>
        private AppLog BindToDomainModel(AppLogInput appLog)
        {
            return new AppLog
            {
                Logger = appLog.Logger,
                LogLevel = appLog.LogLevel,
                Message = appLog.Message,
                Url = appLog.Url,
                ApplicationName = appLog.ApplicationName,
                
            };
        }





        /// <summary>
        /// 
        /// </summary>
        public async Task<IEnumerable<SelectListItem>> GetApplicationNameSelectListItemsAsync(string applicationName = "")
        {
            return await _appLogs.GroupBy(a => a.ApplicationName).Select(app => new SelectListItem
            {
                Text=app.Key,
                Value=app.Key,
                Selected=app.Key==applicationName

            }).ToListAsync();
        }

     


        #endregion


    }
}