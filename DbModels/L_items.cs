using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 字段说明
    ///</summary>
    [SugarTable("L_items")]
    public class L_items
    {
        /// <summary>
        /// 1 
        ///</summary>
         [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        /// 类型gom，gee996 
        ///</summary>
         [SugarColumn(ColumnName="type"    )]
         public int type { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="name"    )]
         public string name { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="displayName"    )]
         public string displayName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="geshi"    )]
         public string geshi { get; set; }
    }
}
