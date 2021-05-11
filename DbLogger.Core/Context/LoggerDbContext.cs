﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using DbLogger.Core.Domain;

namespace DbLogger.Core.Context
{

    /// <summary>
    /// 
    /// </summary>
    public class LoggerDbContext : DbContext, ILoggerUnitOfWork
    {
        #region Fields

        private readonly IConfiguration _configuration;


        #endregion

        #region Ctor


        /// <summary>
        /// 
        /// </summary>
        public LoggerDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Properties


        /// <summary>
        /// 
        /// </summary>
        public DbSet<AppLog> AppLogs { get; set; }


        #endregion

        #region protected Methods



        /// <summary>
        /// 
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var useInMemoryDatabase = _configuration["UseInMemoryDatabase"];
            if (!string.IsNullOrEmpty(useInMemoryDatabase) && useInMemoryDatabase.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: "Add_writes_to_database");
            }
            else
            {
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DbLoggerConnection"]);
            }
        }





        /// <summary>
        /// 
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }



        #endregion

        #region Public Methods



        #region IUnitOfWork Implementations



        /// <summary>
        /// 
        /// </summary>
        public void Migrate()
        {
            this.Database.Migrate();
        }




        /// <summary>
        /// 
        /// </summary>
        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            base.Set<TEntity>().AddRange(entities);
        }




        /// <summary>
        /// 
        /// </summary>
        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            base.Set<TEntity>().RemoveRange(entities);
        }



        /// <summary>
        /// 
        /// </summary>
        public void MarkAsModified<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry(entity).State = EntityState.Modified; // Or use ---> this.Update(entity);
        }



        /// <summary>
        /// 
        /// </summary>
        public void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            base.Entry<TEntity>(entity).State = EntityState.Deleted;

        }




        /// <summary>
        /// 
        /// </summary>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }




        /// <summary>
        /// 
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }



        #endregion


        #endregion

    }
}
