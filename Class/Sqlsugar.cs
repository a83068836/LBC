using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBC.Class
{
    public class Sqlsugar
    {
        //private static string server = "server=1.15.7.225;Database=lcode;Uid=lcode;Pwd=YPS85ptpYtNTbFRH;";
        private static string connStr = @"DataSource=Settings\lcode.sqlite";
        //static string dbName = System.IO.Path.Combine(Environment.CurrentDirectory, "Settings\\lcode.sqlite");
        //static string connStr = new SqliteConnectionStringBuilder()
        //{
        //    DataSource = dbName,
        //    Mode = SqliteOpenMode.ReadWriteCreate,
        //    Password = "Aa102264841314520."
        //}.ToString();
        public static SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = DbType.Sqlite,
            ConnectionString = connStr,
            IsAutoCloseConnection = true
        });
    }
    public class TokenInfo
    {
        public string UnionId { get; set; }
        public string MachineCode { get; set; }
        public DateTime LoginTime { get; set; }

        public TokenInfo(string unionId, string machineCode, DateTime loginTime)
        {
            UnionId = unionId;
            MachineCode = machineCode;
            LoginTime = loginTime;
        }
    }
    public class CheckUserAuthentication
    {
        public static TokenInfo GetSavedToken()
        {
            try
            {
                // 从注册表中读取令牌信息
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\LcodeApp");
                if (key != null)
                {
                    string unionId = key.GetValue("UnionId") as string;
                    string machineCode = key.GetValue("MachineCode") as string;
                    DateTime loginTime = Convert.ToDateTime(key.GetValue("LoginTime"));

                    return new TokenInfo(unionId, machineCode, loginTime);
                }
            }
            catch (Exception ex)
            {
                // 处理异常情况
            }

            return null;
        }
        public static void SaveToken(TokenInfo token)
        {
            try
            {
                // 将令牌信息保存到注册表
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\LcodeApp");
                if (key != null)
                {
                    key.SetValue("UnionId", token.UnionId);
                    key.SetValue("MachineCode", token.MachineCode);
                    key.SetValue("LoginTime", token.LoginTime);
                }
            }
            catch (Exception ex)
            {
                // 处理异常情况
            }
        }

        public static void RemoveToken()
        {
            try
            {
                // 从注册表中移除令牌信息
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\LcodeApp", true);
                key?.DeleteValue("UnionId");
                key?.DeleteValue("MachineCode");
                key?.DeleteValue("LoginTime");
            }
            catch (Exception ex)
            {
                // 处理异常情况
            }
        }
    }
}
