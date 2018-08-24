using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DbLogger.Core.Domain
{
    public class BaseEntity
    {
        #region Ctor

        public BaseEntity()
        {
            CreateDateTime = DateTime.Now;
        }

        #endregion

        #region Properties

        public long Id { get; set; }
        public DateTime CreateDateTime { get; set; }

  

        #endregion

        #region Nav Prop



        #endregion

    }
}
