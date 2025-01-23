using LBC.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextEditLib.Class;
using WebFirst.Entities;

namespace LBC.Class
{
    public class Global
    {
        public List<L_items> Globals_L_items { get; set;}
        public List<L_shuxing> Globals_L_shuxing { get; set; }
        public List<RegexList> RegexLists  { get; set; }
        public OAuth_Token oAuth_Token { get; set; } = new OAuth_Token();

        public L_OAuth l_OAuth { get; set; }=new L_OAuth();
        public string appid { get; set; }
        public string SecretId { get; set; }
        public string SecretKey { get; set; }
        public string buketName { get; set; }
        public string region { get; set; } = "ap-nanjing";
        public string servermac { get; set; }
        public string mac { get; set; }
        public static string GetMacAddress()
        {
            string macAddresses = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    StringBuilder macAddress = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        macAddress.Append(bytes[i].ToString("X2")); // Convert each byte to hexadecimal string
                        if (i != bytes.Length - 1)
                            macAddress.Append(":");
                    }

                    if (macAddress.Length > 0)
                    {
                        macAddresses += /*adapter.Name + ": " + */macAddress.ToString();
                    }
                }
            }
            macAddresses = macAddresses + ":" + GetRandom();
            return macAddresses.Trim();
        }
        private static string GetRandom()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] randomChars = new char[2];

            for (int i = 0; i < 2; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomChars);
        }
        public Global() 
        {
            var a = Sqlsugar.db.Queryable<L_bucketClient>().Where(o => o.Type == 1).ToList();
            if (a.Count > 0)
            {
                appid = a[0].appid;
                SecretId = a[0].SecretId; 
                SecretKey = a[0].SecretKey;
                buketName = a[0].buketName;
                region = a[0].region;
                servermac = a[0].servermac;
                mac = GetMacAddress();
            }
            Globals_L_items = Sqlsugar.db.Queryable<L_items>().Where(o => o.type == 1).ToList();

            // 执行关联查询，并获取关联表信息的数量
            try
            {
                Globals_L_shuxing = Sqlsugar.db.Queryable<L_shuxing>().ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
            

            RegexLists = new List<RegexList>();
            foreach (var item in Globals_L_items)
            {
                if (item.geshi.ToLower().IndexOf("<") == 0 && item.geshi.ToLower().IndexOf(">") == item.geshi.Length - "<".Length)
                {
                    int start = item.geshi.ToLower().IndexOf("<") + "<".Length;
                    int end = item.geshi.ToLower().IndexOf(":");
                    if (end > 0)
                    {
                        int leng = end - start;
                        string key = item.geshi.ToLower().Substring(start, leng);
                        int count = CountOccurrences(item.geshi.ToLower(), ":");
                        string reg = string.Empty;
                        reg += "<";
                        reg += key;
                        for (int i = 0; i < count; i++)
                        {
                            reg += ":" + @"([^:>]*)";
                        }
                        if (item.geshi.ToLower().IndexOf("@") > 0)
                        {
                            reg += @"/@(?:([^:>]+))?>";
                        }
                        else
                        {
                            reg += @"(?:([^:>]+))?>";
                        }
                        RegexLists.Add(new RegexList() { id = 1, key = key, Reg = reg });
                    }
                }
                else
                {
                    int start = 0;
                    int end = item.geshi.ToLower().IndexOf(" ");
                    if (end > 0) 
                    {
                        int leng = end - start;
                        string key = item.geshi.ToLower().Substring(start, leng);
                        int count = CountSpaces(item.geshi.ToLower());
                        string reg = string.Empty;
                        reg += @"(^|\s+)";
                        reg += key;
						for (int i = 0; i < count; i++)
						{
							reg += @"\s+([\d]+|\S+)";
						}
						reg += @"(\s+|\t|\n|$)";
                        RegexLists.Add(new RegexList() { id = 1, key = key, Reg = reg });
                    }
                }
            }

        }
        private async void InitializeGlobalsAsync()
        {
            await Task.Run(async () =>
            {
                Globals_L_items = await Sqlsugar.db.Queryable<L_items>().Where(o => o.type == 1).ToListAsync();

                // 执行关联查询，并获取关联表信息的数量
                Globals_L_shuxing = await Sqlsugar.db.Queryable<L_shuxing>().ToListAsync();
            });
        }
		/// <summary>
		/// 获取某字符串出现的次数
		/// </summary>
		/// <param name="targetString"></param>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public static int CountOccurrences(string targetString, string searchString)
        {
            int count = 0;
            int i = 0;

            while ((i = targetString.IndexOf(searchString, i, StringComparison.Ordinal)) != -1)
            {
                i += searchString.Length;
                count++;
            }

            return count;
        }
        /// <summary>
        /// 查找空格出现的次数并且将连续的多个空格算作一个空格时,结尾空格不算
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static int CountSpaces(string targetString)
        {
            int count = 0;
            bool isSpace = false;

            for (int i = 0; i < targetString.Length; i++)
            {
                if (targetString[i] == ' ')
                {
                    if (!isSpace)
                    {
						if (i != targetString.Length - 1) // 排除结尾空格
						{
							count++;
							isSpace = true;
						}
					}
                }
                else
                {
                    isSpace = false;
                }
            }

            return count;
        }
    }
    public class Automaticloading
    {

        public static async Task<string> AutoLoading(string filename)
        {
            string ret = "";
            string path = "";
            IntPtr int1 = IntPtr.Zero;
            Process[] ps = Process.GetProcessesByName("M2Server");
            if (ps.Length == 0)
            {
                ps = Process.GetProcessesByName("M2Server-x64");
            }
            await Task.Run(() =>
            {
                foreach (Process p in ps)
                {
                    path = p.MainModule.FileName.ToString();
                    if (filename.ToLower().Contains(Path.GetDirectoryName(path).ToLower()))
                    {
                        int1 = WinAPI.WindowsAPI.GetWnd(p.Id, "TFrmMain", "");
                        StringBuilder s = new StringBuilder(512);
                        int aaaa = WinAPI.WindowsAPI.GetWindowText(int1, s, s.Capacity);
                        if (File.Exists(System.Windows.Forms.Application.StartupPath + "Settings\\Automatic.dll"))
                        {
                            string str = File.ReadAllText(System.Windows.Forms.Application.StartupPath + "Settings\\Automatic.dll");
                            JArray json = (JArray)JsonConvert.DeserializeObject(str);
                            for (int i = 0; i < json.Count; i++)
                            {
                                string Header = json[i]["name"].ToString();
                                if (Header == "Gom" && s.ToString().ToLower().Contains("[商业版]"))
                                {
                                    NewMethod(int1, json, i);
                                    ret = "Gom";
                                    break;
                                }
                                if (Header == "Gee" && (s.ToString().ToUpper().IndexOf("无限版") >= 0 || s.ToString().ToUpper().IndexOf("服务器验证成功") >= 0 || s.ToString().ToUpper().IndexOf("★★全功能测试版-限制10人★★") >= 0))
                                {
                                    NewMethod(int1, json, i);
                                    ret = "Gee";
                                    break;
                                }
                                if (s.ToString().ToUpper().IndexOf("996引擎") >= 0)
                                {
                                    NewMethod(int1, json, i);
                                    ret = "996";
                                    break;
                                }
                            }
                        }
                    }
                }
            });
            return ret;
        }

        private static void NewMethod(IntPtr int1, JArray json, int i)
        {
            foreach (var item in json[i]["list"].Where(item => (bool)item["ischecked"]))
            {
                string Content = item["name"].ToString();
                int item1 = int.Parse(item["item1"].ToString());
                int item2 = int.Parse(item["item2"].ToString());
                int item3 = int.Parse(item["item3"].ToString());
                AutoNpc(int1, item1, item2, item3);
            }
        }

        public static void AutoNpc(IntPtr int1, int item1, int item2, int item3)
        {
            const int WM_COMMAND = 0x0111;
            if (int1 != (IntPtr)0)
            {
                //Thread.Sleep(100);
                IntPtr hmenu = WinAPI.WindowsAPI.GetMenu(int1);
                if (hmenu != IntPtr.Zero)
                {
                    hmenu = WinAPI.WindowsAPI.GetSubMenu(hmenu, item1);

                    if (hmenu != IntPtr.Zero)
                    {
                        hmenu = WinAPI.WindowsAPI.GetSubMenu(hmenu, item2);
                        if (hmenu != IntPtr.Zero)
                        {
                            hmenu = WinAPI.WindowsAPI.GetMenuItemID(hmenu, item3);
                            if (hmenu != IntPtr.Zero)
                            {
                                IntPtr hmenu1 = hmenu;
                                WinAPI.WindowsAPI.SendMessage(int1, WM_COMMAND, hmenu, null);
                            }
                        }
                    }

                }
            }
        }
    }

    public class Timers
    {
        public static DateTime ConvertTimestampToDateTime(long timestamp)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dateTimeOffset.LocalDateTime;
        }
        public static long ConvertDateTimeToTimestamp(DateTime dateTime)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }

    }
    public class MachineCode
    {

        static MachineCode machineCode;

        public static string GetMachineCodeString()
        {
            string machineCodeString = string.Empty;
            if (machineCode == null)
            {
                machineCode = new MachineCode();
            }
            machineCodeString = "PC." + machineCode.GetCpuInfo() + "." +
                                machineCode.GetHDid() + "." +
                                machineCode.GetMoAddress();
            return machineCodeString;
        }

        ///   <summary> 
        ///   获取cpu序列号     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetCpuInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo.ToString();
        }

        ///   <summary> 
        ///   获取硬盘ID     
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetHDid()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return HDid.ToString();
        }

        ///   <summary> 
        ///   获取网卡硬件地址 
        ///   </summary> 
        ///   <returns> string </returns> 
        public string GetMoAddress()
        {
            string MoAddress = "";
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MoAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return MoAddress.ToString();
        }
    }
    public class ShortcutUtility
    {
		public static void CreateShortcutWithIcon(string shortcutPath, string targetPath, string description)
		{
			Shortcut sc = new Shortcut();
			sc.Path = targetPath;
			// sc.Arguments = "启动参数";
			// sc.WorkingDirectory = "启动文件的文件夹";
			sc.Description = description;
			sc.Save(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + shortcutPath);
		}
	}
	public class FileHelpers
	{
        public static string ConvertToCOSPath(string originalPath)
        {
            // 去除盘符和文件名
            string[] parts = originalPath.Split('\\');
            string cosPath = string.Join("/", parts[1..]);

            // 根据需要转换为小写
            //cosPath = cosPath.ToLower();

            return cosPath;
        }
        public static string[] GetAllFilePaths(string[] files)
		{
			List<string> filePaths = new List<string>();

			foreach (string file in files)
			{
				if (File.Exists(file))
				{
					filePaths.Add(file);
				}
				else if (Directory.Exists(file))
				{
					string[] subFiles = Directory.GetFiles(file, "*", SearchOption.AllDirectories);
					filePaths.AddRange(subFiles);
				}
			}

			return filePaths.ToArray();
		}
		public static string[] GetApkFiles(string folderPath)
		{
			List<string> apkFilePaths = new List<string>();

			try
			{
				// 获取当前文件夹中的所有.apk文件
				string[] apkFiles = Directory.GetFiles(folderPath, "*.pak");

				// 添加当前文件夹中的.apk文件路径到列表
				apkFilePaths.AddRange(apkFiles);

				// 获取当前文件夹中的所有子文件夹
				string[] subdirectories = Directory.GetDirectories(folderPath);

				// 递归遍历子文件夹
				foreach (string subdirectory in subdirectories)
				{
					// 递归调用GetApkFiles方法，并将结果添加到列表
					apkFilePaths.AddRange(GetApkFiles(subdirectory));
				}
			}
			catch (Exception e)
			{
				// 处理异常
				Console.WriteLine(e.Message);
			}

			return apkFilePaths.ToArray();
		}
		#region 获取指定目录中的匹配文件
		/// <summary>
		/// 获取指定目录中的匹配文件
		/// </summary>
		/// <param name="dir">要搜索的目录</param>
		/// <param name="regexPattern">文件名模式（正则）。null表示忽略模式匹配，返回所有文件</param>
		/// <param name="recurse">是否搜索子目录</param>
		/// <param name="throwEx">是否抛异常</param>
		/// <returns></returns>
		public static string[] GetFiles1(string dir, string regexPattern = null, bool recurse = false, bool throwEx = false)
		{
			List<string> lst = new List<string>();

			try
			{
				foreach (string item in Directory.GetFileSystemEntries(dir))
				{
					try
					{
						bool isFile = (File.GetAttributes(item) & FileAttributes.Directory) != FileAttributes.Directory;

						string a = Path.GetExtension(item);
						if (isFile && (regexPattern == null || Regex.IsMatch(Path.GetFileName(item), regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)) && Path.GetExtension(item).ToLower() == ".txt")
						{ lst.Add(item); }

						//递归
						if (recurse && !isFile) { lst.AddRange(GetFiles1(item, regexPattern, true)); }
					}
					catch { if (throwEx) { throw; } }
				}
			}
			catch { if (throwEx) { throw; } }

			return lst.ToArray();
		}
		#endregion

		public static void WriteTextToFile(string filePath, string content, Encoding encoding)
		{
			int a =1; int b = 1;
			string[] ss=content.Split('\n');
			//File.WriteAllText(filePath, content, encoding);
			using (StreamWriter writer = new StreamWriter(filePath, false, encoding))
			{
				writer.Write(content);
				writer.Flush();
				writer.Close();
			}
		}
        #region 打开文件夹
        public static string OpenFolderBrowserDialogA(string diglogpath)
        {
            string result1 = string.Empty;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            string defaultfilePath = Application.UserAppDataRegistry.GetValue(diglogpath) as string;
            if (defaultfilePath != "")
            {
                dialog.SelectedPath = defaultfilePath;
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                result1 = dialog.SelectedPath;
                Application.UserAppDataRegistry.SetValue(diglogpath, dialog.SelectedPath); ;
            }
            return result1;
        }
        #endregion
    }


}
