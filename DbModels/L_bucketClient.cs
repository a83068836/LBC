using SqlSugar;

namespace WebFirst.Entities
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("L_bucketClient")]
    public class L_bucketClient
    {
        /// <summary>
        /// 1 
        ///</summary>
        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "Type")]
        public int Type { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "SecretId")]
        public string SecretId { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "SecretKey")]
        public string SecretKey { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "appid")]
        public string appid { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "buketName")]
        public string buketName { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "region")]
        public string region { get; set; }

        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "servermac")]

        public string servermac { get; set; }
        /// <summary>
        /// 类型 
        ///</summary>
        [SugarColumn(ColumnName = "mac")]
        public string mac { get; set; }
    }
}
