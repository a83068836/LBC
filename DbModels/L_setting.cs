using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_setting")]
    public class L_setting
    {
        /// <summary>
        /// 1 
        ///</summary>
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName= "folderPath")]
         public string folderPath { get; set; }
    }
}
