using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LBC.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using LBC.View.Patch;
using System.IO;
using SqlSugar;
using System.Threading;
using System.Windows.Documents;
using System.Reflection.Metadata;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows;
using LBC.ViewModels;
using LBC.Class;
using UnitComboLib.Command;
using LBC.View.Patch;
using SharpCompress.Common;
using System.Data;
using System.Windows.Media.Imaging;
using HandyControl.Controls;
using System.Diagnostics;

namespace LBC.ViewModel
{
    public class PatchViewModel : ViewModelBases
    {
        public static int patchid = 0;

        private Visibility _progressVisibility =Visibility.Collapsed;
        /// <summary>
        /// ProgressVisibility
        /// </summary>
        public Visibility ProgressVisibility
        {
            get { return _progressVisibility; }
            set
            {
                _progressVisibility = value;
                this.RaisePropertyChanged("ProgressVisibility");
            }
        }

        private int _ProgressOne;
        /// <summary>
        /// _ProgressOne
        /// </summary>
        public int ProgressOne
        {
            get { return _ProgressOne; }
            set
            {
                _ProgressOne = value;
                this.RaisePropertyChanged("ProgressOne");
            }
        }

        private int _ProgressCount;
        /// <summary>
        /// _ProgressCount
        /// </summary>
        public int ProgressCount
        {
            get { return _ProgressCount; }
            set
            {
                _ProgressCount = value;
                this.RaisePropertyChanged("ProgressCount");
            }
        }

        private int _ProgressOneMax;
        /// <summary>
        /// _ProgressOne
        /// </summary>
        public int ProgressOneMax
        {
            get { return _ProgressOneMax; }
            set
            {
                _ProgressOneMax = value;
                this.RaisePropertyChanged("ProgressOneMax");
            }
        }

        private int _ProgressCountMax;
        /// <summary>
        /// _ProgressCount
        /// </summary>
        public int ProgressCountMax
        {
            get { return _ProgressCountMax; }
            set
            {
                _ProgressCountMax = value;
                this.RaisePropertyChanged("ProgressCountMax");
            }
        }
        public static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @"H:\GomPak\MirServer\Mud2\DB\HeroDB.MDB";
        public SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.Access,
            ConnectionString = connectionString,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
        public  PatchViewModel() 
        {
            try
            { 
                LoadPathViewModel(); 
            }
            catch (Exception ex)
            { 
            
            }
            
            //try
            //{
            //    Task.Run(async () => await LoadPathViewModel()).GetAwaiter().GetResult();
            //}
            //catch (AggregateException ae)
            //{
            //    foreach (var innerException in ae.Flatten().InnerExceptions)
            //    {
            //        Console.WriteLine($"捕获到异常: {innerException.Message}");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"捕获到异常: {e.Message}");
            //}
        }
        private static readonly object _lockObject = new object();
        public /*async Task*/ void LoadPathViewModel()
        {
            bool bb = false;
            string path = "H:\\GomPak\\MirServer\\Mir200\\Envir";
            string pathEffectItemList = path + "\\" + "EffectItemList.txt";
            List<Tuple<string, int>> dataList = new List<Tuple<string, int>>();
            var dataTable = db.Queryable<object>().AS("StdItems", "o").ToDataTable();
            List<EffectList> effectList = new List<EffectList>();
            string patheffectimageList = path + "\\" + "EffectImageList.txt";
            List<Tuple<int, string>> effectimageList = new List<Tuple<int, string>>();
            string pathEffectList = path + "\\" + "EffectList.txt";

            
                using (StreamReader sr = new StreamReader(patheffectimageList, Encoding.GetEncoding("gb2312")))
                {
                    string line;
                    int cid = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('\t'); // 假设数据是用制表符分隔的
                        if (parts.Length == 1)
                        {
                            effectimageList.Add(new Tuple<int, string>(cid, parts[0]));
                        }
                        cid++;
                    }
                }
            

            
                using (StreamReader sr = new StreamReader(pathEffectList, Encoding.GetEncoding("gb2312")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('\t'); // 假设数据是用制表符分隔的
                        if (parts.Length >= 26)
                        {
                            EffectList effectList1 = new EffectList();
                            effectList1.id = int.Parse(parts[0]);
                            effectList1.a1 = int.Parse(parts[1]);
                            effectList1.a2 = int.Parse(parts[2]);
                            effectList1.a3 = int.Parse(parts[3]);
                            effectList1.a4 = int.Parse(parts[4]);
                            effectList1.a5 = int.Parse(parts[5]);
                            effectList1.a6 = int.Parse(parts[6]);
                            effectList1.a7 = int.Parse(parts[7]);
                            effectList1.a8 = int.Parse(parts[8]);
                            effectList1.a9 = int.Parse(parts[9]);
                            effectList1.a10 = int.Parse(parts[10]);
                            effectList1.a11 = int.Parse(parts[11]);
                            effectList1.a12 = int.Parse(parts[12]);
                            effectList1.a13 = int.Parse(parts[13]);
                            effectList1.a14 = int.Parse(parts[14]);
                            effectList1.a15 = int.Parse(parts[15]);
                            effectList1.a16 = int.Parse(parts[16]);
                            effectList1.a17 = int.Parse(parts[17]);
                            effectList1.a18 = int.Parse(parts[18]);
                            effectList1.a19 = int.Parse(parts[19]);
                            effectList1.a20 = int.Parse(parts[20]);
                            effectList1.a21 = int.Parse(parts[21]);
                            effectList1.a22 = int.Parse(parts[22]);
                            effectList1.a23 = int.Parse(parts[23]);
                            effectList1.a24 = int.Parse(parts[24]);
                            effectList1.a25 = int.Parse(parts[25]);
                            effectList.Add(effectList1);
                        }
                    }
                }



                using (StreamReader sr = new StreamReader(pathEffectItemList, Encoding.GetEncoding("gb2312")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('\t'); // 假设数据是用制表符分隔的
                        if (parts.Length == 2)
                        {
                            string itemName = parts[0];
                            int itemValue = int.Parse(parts[1]);
                            dataList.Add(new Tuple<string, int>(itemName, itemValue));
                        }
                    }
                }
            

            // 创建锁对象实例
            lock (_lockObject)
            {
                // 输出数组内容
                foreach (var item in dataList)
                {
                    if (bb)
                        return;
                    var aaa = new PatchModel() { Name = item.Item1 };
                    var result = (from row in dataTable.AsEnumerable()
                                  where row.Field<string>("name") == item.Item1
                                  select new
                                  {
                                      Name = row.Field<string>("name"),
                                      looks = row.Field<int>("Looks"),
                                      // 其他需要处理的列
                                  }).FirstOrDefault();

                    if (result != null)
                    {
                        var result1 = effectList.FirstOrDefault(e => e.id == item.Item2);
                        aaa.PatchA1 = result.looks.ToString();
                        List<ImageItem> list1 = G.GetimagesList(result.looks, "stateitem");
                        if (list1.Count > 0)
                            aaa.PatchA1image = G.Getimage(list1[0].ImagePath, list1[0].imagewidth, list1[0].imageheight);
                        list1 = G.GetimagesList(result.looks, "Items");
                        if (list1.Count > 0)
                            aaa.PatchA2image = G.Getimage(list1[0].ImagePath, list1[0].imagewidth, list1[0].imageheight);
                        list1 = G.GetimagesList(result.looks, "DnItems");
                        if (list1.Count > 0)
                            aaa.PatchA3image = G.Getimage(list1[0].ImagePath, list1[0].imagewidth, list1[0].imageheight);
                        aaa.PatchA2 = result.looks.ToString();
                        aaa.PatchA3 = result.looks.ToString();
                        if (result1 != null)
                        {
                            aaa.PatchA4 = result1;
                            var result2 = effectimageList.FirstOrDefault(e => e.Item1 == result1.a3);
                            if (result2 != null)
                            {
                                list1 = G.GetimagesList(result1.a6, result1.a7, result2.Item2);
                                List<BitmapImage> bitmapImages = new List<BitmapImage>();
                                foreach (var itema in list1)
                                {
                                    bitmapImages.Add(G.Getimage(itema.ImagePath, itema.imagewidth, itema.imageheight));

                                }
                                if (bitmapImages != null)
                                {
                                    //aaa.PatchA4image = G.CreateGifFromBitmapImages(bitmapImages);
                                    //bb = true;
                                }

                            }

                        }
                        // 处理其他列的数据
                    }
                    // 使用 Dispatcher 异步更新界面

                    _ = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        PatchSourceT.Add(aaa);
                        Workspace.This.Git.Insert(aaa.Id + ":" + aaa.Name);
                    }));
                }
            }
                

        }
        public void Add(PatchModel patchModel)
        {
            PatchSourceT.Add(patchModel);
        }

        public void Edit(PatchModel patchModel)
        {
            var data = PatchSourceT.First(it => it.Id == patchModel.Id);
            if(data != null) 
            {
                data.YinqingPath = patchModel.YinqingPath;
                data.DengluqiPath = patchModel.DengluqiPath;
                data.BudingPath = patchModel.BudingPath;
                data.Pakmima = patchModel.Pakmima;
                data.Name = patchModel.Name;
                data.Type = patchModel.Type;
                data.PatchA1 = patchModel.PatchA1;
                data.PatchA2 = patchModel.PatchA2;
                data.PatchA3 = patchModel.PatchA3;
                data.PatchA4 = patchModel.PatchA4;
                data.PatchA5 = patchModel.PatchA5;
                data.PatchA6 = patchModel.PatchA6;
            }
        }
        public void Del(PatchModel patchModel)
        {
            PatchSourceT.Remove(patchModel);
        }

        #region 验证方法

        public bool Verify()
        {
            var list = PatchSourceT.Where(it=>it.IsSelected==true).ToList();
            if (list.Count == 0)
            {
                HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                {
                    Message = "选中项不能为空",
                    WaitTime = 1
                });
                return false;
            }
            else
            {
                foreach (var item in list)
                {
                    if (!Directory.Exists(item.YinqingPath))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "引擎路径不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!Directory.Exists(item.DengluqiPath))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "登录器路径不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!Directory.Exists(item.BudingPath))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "补丁路径不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!File.Exists(item.DengluqiPath + @"\Config.ini"))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "登录器配置文件 Config.ini 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    else
                    {
                        IniFiles iniFiles = new IniFiles(item.DengluqiPath + @"\Config.ini");
                        string budingA1 = iniFiles.IniReadvalue("Setup", "ResourcesDir");
                        if (budingA1 != "")
                        {
                            if (!Directory.Exists(item.BudingPath + @"\" + budingA1))
                            {
                                HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                                {
                                    Message = "自定义补丁目录 " + budingA1 + " 不存在",
                                    WaitTime = 1
                                });
                                return false;
                            }
                        }
                    }
                    if (!File.Exists(item.Pakmima))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "pak密码文件不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!File.Exists(item.YinqingPath + @"\Config.ini"))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "引擎配置文件 Config.ini 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    else
                    {
                        IniFiles iniFiles = new IniFiles(item.YinqingPath + @"\Config.ini");
                        string budingA1 = iniFiles.IniReadvalue("GameConf", "UseAccessDB");
                        string budingA2 = iniFiles.IniReadvalue("GameConf", "AccessFileName");
                        if (budingA1 != "1")
                        {
                            HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                            {
                                Message = "请设置数据库为access数据库",
                                WaitTime = 1
                            });
                            return false;
                        }
                        if (budingA2 == "")
                        {
                            HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                            {
                                Message = "access数据库设置为空",
                                WaitTime = 1
                            });
                            return false;
                        }
                        else
                        {
                            if (!File.Exists(budingA2))
                            {
                                HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                                {
                                    Message = "access数据库文件 " + budingA2 + " 不存在",
                                    WaitTime = 1
                                });
                                return false;
                            }
                            else
                            {
                                string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+ budingA2;
                                //string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + budingA2;
                                SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                                {
                                    ConnectionString = ConnectionString,
                                    DbType = SqlSugar.DbType.Access,
                                    IsAutoCloseConnection = true
                                });
                                var dt2 = db.Ado.GetDataTable("select * from StdItems where Name='" + item.Name + "'");
                                if(dt2 == null)
                                {
                                    HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                                    {
                                        Message = "access数据库文件不存在 " + item.Name + " 装备",
                                        WaitTime = 1
                                    });
                                    return false;
                                }
                            }
                        }
                    }
                    if (!Directory.Exists(item.PatchA1))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "身上补丁路径 "+ item.PatchA1 + " 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!Directory.Exists(item.PatchA2))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "背包补丁路径 " + item.PatchA2 + " 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (!Directory.Exists(item.PatchA3))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "地上补丁路径 " + item.PatchA3 + " 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    //if (!Directory.Exists(item.PatchA4))
                    //{
                    //    HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                    //    {
                    //        Message = "身上特效补丁路径 " + item.PatchA4 + " 不存在",
                    //        WaitTime = 1
                    //    });
                    //    return false;
                    //}
                    if (!Directory.Exists(item.PatchA5))
                    {
                        HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                        {
                            Message = "背包特效补丁路径 " + item.PatchA5 + " 不存在",
                            WaitTime = 1
                        });
                        return false;
                    }
                    if (item.Type == DemoType.武器 || item.Type == DemoType.衣服)
                    {
                        if (!Directory.Exists(item.PatchA6))
                        {
                            HandyControl.Controls.Growl.Warning(new HandyControl.Data.GrowlInfo
                            {
                                Message = "外观特效补丁路径 " + item.PatchA6 + " 不存在",
                                WaitTime = 1
                            });
                            return false;
                        }
                    }
                } 
            }
            return true;
        }

        #endregion

        private ObservableCollection<PatchModel> _patchSourceT = new ObservableCollection<PatchModel>();
        public ObservableCollection<PatchModel> PatchSourceT
        {
            get { return _patchSourceT; }
            set
            {
                if (_patchSourceT != value)
                {
                    _patchSourceT = value;
                    RaisePropertyChanged("PatchSourceT");
                }
            }
        }
        #region 增加
        RelayCommand<object> _mergeAddtCommand = null;
        public ICommand MergeAddtCommand
        {
            get
            {
                if (_mergeAddtCommand == null)
                {
                    _mergeAddtCommand = new RelayCommand<object>((p) => CanMergeAddtCommand(p));
                }

                return _mergeAddtCommand;
            }
        }

        private bool CanMergeAddtCommand(object p)
        {
            PatchAdd patchAdd = new PatchAdd(1, null);
            patchAdd.ShowDialog();
            return true;
        }
        #endregion

        #region 复制增加
        RelayCommand<object> _mergeCopyAddtCommand = null;
        public ICommand MergeCopyAddtCommand
        {
            get
            {
                if (_mergeCopyAddtCommand == null)
                {
                    _mergeCopyAddtCommand = new RelayCommand<object>((p) => CanMergeCopyAddtCommand(p));
                }

                return _mergeCopyAddtCommand;
            }
        }

        private bool CanMergeCopyAddtCommand(object p)
        {
            if (p != null)
            {
                PatchAdd patchAdd = new PatchAdd(2, (PatchModel)p);
                patchAdd.ShowDialog();
            }
            return true;
        }
        #endregion

        #region 修改
        RelayCommand<object> _mergeEditAddtCommand = null;
        public ICommand MergeEditAddtCommand
        {
            get
            {
                if (_mergeEditAddtCommand == null)
                {
                    _mergeEditAddtCommand = new RelayCommand<object>((p) => CanMergeEditAddtCommand(p));
                }

                return _mergeEditAddtCommand;
            }
        }

        private bool CanMergeEditAddtCommand(object p)
        {
            if (p != null)
            {
                PatchAdd patchAdd = new PatchAdd(3, (PatchModel)p);
                patchAdd.ShowDialog();
            }
            return true;
        }
        #endregion

        #region 删除
        RelayCommand<object> _mergeDelAddtCommand = null;
        public ICommand MergeDelAddtCommand
        {
            get
            {
                if (_mergeDelAddtCommand == null)
                {
                    _mergeDelAddtCommand = new RelayCommand<object>((p) => CanMergeDelAddtCommand(p));
                }

                return _mergeDelAddtCommand;
            }
        }

        private bool CanMergeDelAddtCommand(object p)
        {
            if (p != null)
            {
                Del((PatchModel)p);
            }
            else
            {
                var list = PatchSourceT.Where(it => it.IsSelected == true).ToList();
                foreach ( var item in list ) 
                {
                    Del(item);
                }
            }
            return true;
        }
        #endregion

        #region 读取配置
        RelayCommand<object> _mergereadtCommand = null;
        public ICommand MergereadtCommand
        {
            get
            {
                if (_mergereadtCommand == null)
                {
                    _mergereadtCommand = new RelayCommand<object>((p) => CanMergereadtCommand(p));
                }

                return _mergereadtCommand;
            }
        }

        private bool CanMergereadtCommand(object p)
        {
            if (File.Exists("Patchitems.dll"))
            {
                string str = File.ReadAllText("Patchitems.dll");
                var model = JsonConvert.DeserializeObject<List<PatchModel>> (str);
                PatchSourceT = new ObservableCollection<PatchModel>();
                foreach (var item in model)
                {
                    PatchSourceT.Add(item);
                }
            }

            return true;
        }
        #endregion

        #region 保存配置
        RelayCommand<object> _mergesavetCommand = null;
        public ICommand MergesavetCommand
        {
            get
            {
                if (_mergesavetCommand == null)
                {
                    _mergesavetCommand = new RelayCommand<object>((p) => CanMergesavetCommand(p));
                }

                return _mergesavetCommand;
            }
        }

        private bool CanMergesavetCommand(object p)
        {
            string resultJson = JsonConvert.SerializeObject(PatchSourceT);
            File.WriteAllText("Patchitems.dll", resultJson);
            HandyControl.Controls.Growl.Success(new HandyControl.Data.GrowlInfo
            {
                Message = "保存成功",
                WaitTime = 1
            });
            return true;
        }
        #endregion

        #region 验证
        RelayCommand<object> _mergeverifytCommand = null;
        public ICommand MergeverifytCommand
        {
            get
            {
                if (_mergeverifytCommand == null)
                {
                    _mergeverifytCommand = new RelayCommand<object>((p) => CanMergeverifytCommand(p));
                }

                return _mergeverifytCommand;
            }
        }

        private bool CanMergeverifytCommand(object p)
        {
            if (Verify())
            {
                HandyControl.Controls.Growl.Success(new HandyControl.Data.GrowlInfo
                {
                    Message = "验证通过",
                    WaitTime = 1
                });
            }

            return true;
        }
        #endregion

        #region 执行
        RelayCommand<object> _mergeperformtCommand = null;
        public ICommand MergeperformtCommand
        {
            get
            {
                if (_mergeperformtCommand == null)
                {
                    _mergeperformtCommand = new RelayCommand<object>((p) => CanMergeperformtCommand(p));
                }

                return _mergeperformtCommand;
            }
        }

        private bool CanMergeperformtCommand(object p)
        {
            if (Verify())
            {
                _ = StartAsync();
            }
            return true;
        }
        #endregion


        #region 创建Pak
        public async Task<bool> CreatePakAsync(string pakname,string pakpath,string pakfile,string pakmima)
        {
            
            return true;
        }


        #endregion

        #region 批量导入素材
        /// <summary>
        /// 批量导入素材
        /// </summary>
        /// <param name="pakname"></param>
        /// <param name="pakpath"></param>
        /// <param name="pakfile"></param>
        /// <param name="pakmimapath"></param>
        /// <param name="patch"></param>
        /// <param name="id">0导入补丁，1导入空白图片1张</param>
        /// <returns></returns>
        public async Task<bool> ImportPakAsync(string pakname, string pakpath, string pakfile, string pakmimapath,string patch,int id)
        {
            
            return true;
        }
        #endregion

        public void Delay(int millSeconds)
        {
            Thread t = new Thread(o => Thread.Sleep(millSeconds));
            t.Start(this);
            while (t.IsAlive)
            {
                //防止UI假死
                System.Windows.Forms.Application.DoEvents();
            }
        }

        #region 批量导入素材返回数据库插入编号
        public async Task<int> ImportPakAsync01(string pakname, string pakpath, string pakfile, string pakmimapath, string patch)
        {
           
            return 1;
        }
        #endregion

        #region 执行批量导入代码
        //progress progress = new progress();
        public async Task<bool> StartAsync()
        {
           
            return true;
        }

        /// <summary>
        /// 写入txt文本pak配置文件
        /// </summary>
        /// <param name="pakfile1">Pak文件名字</param>
        /// <param name="pakmima1">pak密码</param>
        /// <param name="item"></param>
        /// <param name="budingA1"></param>
        /// <param name="EffectItemListpak"></param>
        /// <param name="id">0其他，1武器，2衣服</param>
        /// <returns></returns>
        private static string WritePakTxt(string pakfile1, string pakmima1, string dengluqipath, string budingA1, string EffectItemListpak,string budingpath,int id)
        {
            return "";
        }

        
      
      

        #endregion

        #region 全选反选
        RelayCommand<object> _selectOrUnSelectAll = null;
        public ICommand SelectOrUnSelectAll
        {
            get
            {
                if (_selectOrUnSelectAll == null)
                {
                    _selectOrUnSelectAll = new RelayCommand<object>((p) => CanSelectOrUnSelectAll(p));
                }

                return _selectOrUnSelectAll;
            }
        }
        private bool isSelectFlag =false;
        private bool CanSelectOrUnSelectAll(object p)
        {
            if (PatchSourceT != null && isSelectFlag == false)
            {
                for (int i = 0; i < PatchSourceT.Count; i++)
                {
                    PatchSourceT[i].IsSelected = true;
                }
                isSelectFlag = true;
            }
            else if (PatchSourceT != null && isSelectFlag == true)
            {
                for (int i = 0; i < PatchSourceT.Count; i++)
                {
                    PatchSourceT[i].IsSelected = false;
                }
                isSelectFlag = false;
            }
            return true;
        }
        #endregion
    }

    public class PatchModel : ViewModelBases
    {
        public PatchModel()
        {
            Id = PatchViewModel.patchid + 1;
            PatchViewModel.patchid += 1;
        }
        private int _id;
        /// <summary>
        /// id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.RaisePropertyChanged("Id");
            }
        }

        private string _yinqingPath;
        /// <summary>
        /// id
        /// </summary>
        public string YinqingPath
        {
            get { return _yinqingPath; }
            set
            {
                _yinqingPath = value;
                this.RaisePropertyChanged("YinqingPath");
            }
        }
        private string _dengluqiPath;
        /// <summary>
        /// id
        /// </summary>
        public string DengluqiPath
        {
            get { return _dengluqiPath; }
            set
            {
                _dengluqiPath = value;
                this.RaisePropertyChanged("DengluqiPath");
            }
        }
        private string _budingPath;
        /// <summary>
        /// id
        /// </summary>
        public string BudingPath
        {
            get { return _budingPath; }
            set
            {
                _budingPath = value;
                this.RaisePropertyChanged("BudingPath");
            }
        }
        private string _pakmima;
        /// <summary>
        /// id
        /// </summary>
        public string Pakmima
        {
            get { return _pakmima; }
            set
            {
                _pakmima = value;
                this.RaisePropertyChanged("Pakmima");
            }
        }

        private string _name;
        /// <summary>
        /// id
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }
        private DemoType _type;
        /// <summary>
        /// id
        /// </summary>
        public DemoType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                this.RaisePropertyChanged("Type");
            }
        }
        private string _patchA1;
        /// <summary>
        /// id
        /// </summary>
        public string PatchA1
        {
            get { return _patchA1; }
            set
            {
                _patchA1 = value;
                this.RaisePropertyChanged("PatchA1");
            }
        }
        private BitmapImage _patchA1image;
        /// <summary>
        /// id
        /// </summary>
        public BitmapImage PatchA1image
        {
            get { return _patchA1image; }
            set
            {
                _patchA1image = value;
                this.RaisePropertyChanged("PatchA1image");
            }
        }
        private BitmapImage _patchA2image;
        /// <summary>
        /// id
        /// </summary>
        public BitmapImage PatchA2image
        {
            get { return _patchA2image; }
            set
            {
                _patchA2image = value;
                this.RaisePropertyChanged("PatchA2image");
            }
        }
        private BitmapImage _patchA3image;
        /// <summary>
        /// id
        /// </summary>
        public BitmapImage PatchA3image
        {
            get { return _patchA3image; }
            set
            {
                _patchA3image = value;
                this.RaisePropertyChanged("PatchA3image");
            }
        }
        private string _patchA2;
        /// <summary>
        /// id
        /// </summary>
        public string PatchA2
        {
            get { return _patchA2; }
            set
            {
                _patchA2 = value;
                this.RaisePropertyChanged("PatchA2");
            }
        }
        private string _patchA3;
        /// <summary>
        /// id
        /// </summary>
        public string PatchA3
        {
            get { return _patchA3; }
            set
            {
                _patchA3 = value;
                this.RaisePropertyChanged("PatchA3");
            }
        }
        private EffectList _patchA4;
        /// <summary>
        /// id
        /// </summary>
        public EffectList PatchA4
        {
            get { return _patchA4; }
            set
            {
                _patchA4 = value;
                this.RaisePropertyChanged("PatchA4");
            }
        }
        private BitmapImage _patchA4image;
        /// <summary>
        /// id
        /// </summary>
        public BitmapImage PatchA4image
        {
            get { return _patchA4image; }
            set
            {
                _patchA4image = value;
                this.RaisePropertyChanged("PatchA4image");
            }
        }
        private string _patchA5;
        /// <summary>
        /// id
        /// </summary>
        public string PatchA5
        {
            get { return _patchA5; }
            set
            {
                _patchA5 = value;
                this.RaisePropertyChanged("PatchA5");
            }
        }
        private string _patchA6;
        /// <summary>
        /// id
        /// </summary>
        public string PatchA6
        {
            get { return _patchA6; }
            set
            {
                _patchA6 = value;
                this.RaisePropertyChanged("PatchA6");
            }
        }
        private bool _isSelected;
        /// <summary>
        /// id
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        private List<DemoDataModel> _nameItems;
        /// <summary>
        /// id
        /// </summary>
        public List<DemoDataModel> NameItems
        {
            get { return _nameItems; }
            set
            {
                _nameItems = value;
                this.RaisePropertyChanged("NameItems");
            }
        }
    }
    public class DemoDataModel
    {
        public string Name { get; set; }
    }
    public class Patchtexiao
    {
        public int start { get; set; } = 0;
        public int length { get; set; } = 0;
        public int number { get; set; } = 0;
    }
    public class EffectList 
    {
        public int id { get; set; } = 0;
        public int a1 { get; set; } = -1;
        public int a2 { get; set; } = -1;
        public int a3 { get; set; } = -1;
        public int a4 { get; set; } = 0;
        public int a5 { get; set; } = 0;
        public int a6 { get; set; } = 0;
        public int a7 { get; set; } = 0;
        public int a8 { get; set; } = 0;
        public int a9 { get; set; } = 0;
        public int a10 { get; set; } = 0;
        public int a11 { get; set; } = 0;
        public int a12 { get; set; } = 0;
        public int a13 { get; set; } = 0;
        public int a14 { get; set; } = 0;
        public int a15 { get; set; } = 0;
        public int a16 { get; set; } = 0;
        public int a17 { get; set; } = 0;
        public int a18 { get; set; } = 0;
        public int a19 { get; set; } = 0;
        public int a20 { get; set; } = 0;
        public int a21 { get; set; } = 0;
        public int a22 { get; set; } = 0;
        public int a23 { get; set; } = 0;
        public int a24 { get; set; } = 0;
        public int a25 { get; set; } = 0;
    }
}
