﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LBC.Class
{

    /// <summary>
    /// IniFiles 的摘要说明。
    /// 示例文件路径：C:\file.ini
    /// [Server]            //[*] 表示缓存区
    /// name=localhost      //name 表示主键，localhost 表示值
    /// </summary>
    public class IniFiles
    {
        public string path;
        [DllImport("kernel32")] //返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")] //返回取得字符串缓冲区的长度
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// 保存ini文件的路径
        /// 调用示例：var ini = IniFiles("C:\file.ini");
        /// </summary>
        /// <param name="INIPath"></param>
        public IniFiles(string iniPath)
        {
            this.path = iniPath;
        }
        /// <summary>
        /// 写Ini文件
        /// 调用示例：ini.IniWritevalue("Server","name","localhost");
        /// </summary>
        /// <param name="Section">[缓冲区]</param>
        /// <param name="Key">键</param>
        /// <param name="value">值</param>
        public void IniWritevalue(string Section, string Key, string value)
        {
            WritePrivateProfileString(Section, Key, value, this.path);
        }
        /// <summary>
        /// 读Ini文件
        /// 调用示例：ini.IniWritevalue("Server","name");
        /// </summary>
        /// <param name="Section">[缓冲区]</param>
        /// <param name="Key">键</param>
        /// <returns>值</returns>
        public string IniReadvalue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);

            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }

    }
}
