using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_Authorization")]
    public class L_Authorization
    {
        /// <summary>
        /// 1 
        ///</summary>
         [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="UserID"    )]
         public int UserID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="AccessToken"    )]
         public string AccessToken { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="RefreshToken"    )]
         public string RefreshToken { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="ExpiresIn"    )]
         public int ExpiresIn { get; set; }
    }
}
