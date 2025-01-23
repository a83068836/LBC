using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_OAuth")]
    public class L_OAuth
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
         [SugarColumn(ColumnName="openid"    )]
         public string openid { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="nickname"    )]
         public string nickname { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="sex"    )]
         public string sex { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="province"    )]
         public string province { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="city"    )]
         public string city { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="country"    )]
         public string country { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="headimgurl"    )]
         public string headimgurl { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="privilege"    )]
         public string privilege { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="unionid"    )]
         public string unionid { get; set; }
    }
}
