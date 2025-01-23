using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_Users")]
    public class L_Users
    {
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="UserID" ,IsPrimaryKey = true ,IsIdentity = true  )]
         public int UserID { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="UserName"    )]
         public string UserName { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Email"    )]
         public string Email { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="PhoneNumber"    )]
         public string PhoneNumber { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Password"    )]
         public string Password { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="Point"    )]
         public int? Point { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="VIPExpiration"    )]
         public int? VIPExpiration { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="RegistrationTime"    )]
         public int? RegistrationTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="LastLoginTime"    )]
         public int? LastLoginTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
         [SugarColumn(ColumnName="TotalUsageTime"    )]
         public int? TotalUsageTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnName = "MachineCode")]
        public string MachineCode { get; set; }
    }
}
