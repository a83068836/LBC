using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 属性
    ///</summary>
    [SugarTable("L_shuxing")]
    public class L_shuxing
    {
        /// <summary>
        /// 1 
        ///</summary>
         [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        /// tid 
        ///</summary>
        [SugarColumn(ColumnName = "tid")]
        public int tid { get; set; }
        /// <summary>
        /// 名字 
        ///</summary>
        [SugarColumn(ColumnName="title"    )]
         public string title { get; set; }
        /// <summary>
        /// 说明 
        ///</summary>
         [SugarColumn(ColumnName="shuoming"    )]
         public string shuoming { get; set; }
    }
}
