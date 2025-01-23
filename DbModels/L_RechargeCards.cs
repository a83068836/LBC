using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_RechargeCards")]
    public class L_RechargeCards
    {
        /// <summary>
        /// 1 
        ///</summary>
         [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int Id { get; set; }
        /// <summary>
        /// 卡号（主键） 
        ///</summary>
         [SugarColumn(ColumnName="CardID"    )]
         public string CardID { get; set; }
        /// <summary>
        /// 充值金额 
        ///</summary>
         [SugarColumn(ColumnName="Amount"    )]
         public decimal? Amount { get; set; }
        /// <summary>
        /// 是否已使用 
        ///</summary>
         [SugarColumn(ColumnName="IsUsed"    )]
         public int? IsUsed { get; set; }
        /// <summary>
        /// 使用日期 
        ///</summary>
         [SugarColumn(ColumnName="UsageDate"    )]
         public int? UsageDate { get; set; }
    }
}
