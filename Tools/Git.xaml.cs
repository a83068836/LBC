using LBC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = HandyControl.Controls.MessageBox;
using UnitComboLib.Command;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using System.Diagnostics;
using FilterTreeViewLib;
using System.Collections.ObjectModel;
using LBC.Class;
using WebFirst.Entities;
using System.Windows.Forms;
using HandyControl.Controls;
using FilterTreeViewLib.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Net.NetworkInformation;
using SharpCompress.Common;

namespace LBC.Tools
{
	/// <summary>
	/// Git.xaml 的交互逻辑
	/// </summary>
	public partial class Git
	{
		public Git()
		{
			InitializeComponent();
			this.DataContext = new GitViewModel();
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://console.cloud.tencent.com/cam/capi") { UseShellExecute = true });
        }
    }
    class GitViewModel : ViewModelBase
    {
        public ICommand SaveCommand => new RelayCommand<string>(ExecuteSaveCommand);
        public ICommand Save1Command => new RelayCommand<string>(ExecuteSave1Command);
        public ICommand Save2Command => new RelayCommand<string>(ExecuteSave2Command);
        public ICommand upCommand => new RelayCommand<string>(ExecuteupCommand);
        public ICommand down1Command => new RelayCommand<string>(ExecutedownCommand);
        public ICommand upServerCommand => new RelayCommand<string>(ExecuteupServerCommand);

        private ObservableCollection<BucketItem> _comboBoxItems;
        public ObservableCollection<BucketItem> ComboBoxItems
        {
            get { return _comboBoxItems; }
            set
            {
                _comboBoxItems = value;
                RaisePropertyChanged("ComboBoxItems");
            }
        }

        private BucketItem _selectedItem;
        public BucketItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        public GitViewModel()
        {
            SecretId = App.global.SecretId;
            SecretKey = App.global.SecretKey;
            Appid = App.global.appid;
            servermac = App.global.servermac;
            ExecuteSaveCommand("");
        }
        
        private async void ExecuteupCommand(object parameter)
        {
            ProgressBarVisible = Visibility.Collapsed;
            GetCount(Workspace.This.Explorer.Root.CountryRootItems[0]);
            _ = Task.Run(() => DoAsyncLoop(Workspace.This.Explorer.Root.CountryRootItems[0]));
        }
        private void ExecuteupServerCommand(object parameter)
        {
            MyMessage myMessage = new MyMessage();
            myMessage.type = 11;
            myMessage.l_BucketClient = new WebFirst.Entities.L_bucketClient()
            {
                SecretId = App.global.SecretId,
                SecretKey = App.global.SecretKey,
                appid = App.global.appid,
                buketName = App.global.buketName,
                region = App.global.region,
                servermac = App.global.servermac,
                mac = App.global.mac,
            };
            App.WebSocketTest.SendMessage(myMessage);
        }
        private async void ExecutedownCommand(object parameter)
        {
            var list = Sqlsugar.db.Queryable<L_setting>().Where(it => it.Id == 1).ToList();
            if (list.Count == 0)
            {
                Workspace.This.Git.Insert("请先设置本地版本路径！！！");
                return;
            }
            string path = list[0].folderPath;
            ProgressBarVisible = Visibility.Collapsed;
            var a = await CosBuilder.SelectObjectList();
            ProgressBarValueMax = a.contentsList.Count;
            _ = Task.Run(() => DoAsyncLoop(a, path));
        }
        async Task DoAsyncLoop(COSXML.Model.Tag.ListBucket list,string path)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(20);

            List<Task> tasks = new List<Task>();

            await Task.Run(async () =>
            {

                foreach (var item in list.contentsList)
                {
                    await semaphore.WaitAsync();

                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            await CosBuilder.DownObject(item.key, path);
                            ProgressBarValue++;
                            if (ProgressBarValue == ProgressBarValueMax)
                            {
                                ProgressBarVisible = Visibility.Visible;
                                ProgressBarValue = 0;
                                await Workspace.This.Explorer.LoadSampleDataAsync(path, 0);
                                Workspace.This.Git.Insert("全部下载完成");
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }
            });
        }
        private async void ExecuteSaveCommand(object parameter)
		{
			var a = await CosBuilder.SelectBucket();
			ComboBoxItems = new ObservableCollection<BucketItem>(a);
			if (ComboBoxItems.Count > 0)
			{
                Workspace.This.Git.Insert("获取存储桶数量：" + ComboBoxItems.Count);
                int a1 = 1;
                foreach (var item in ComboBoxItems) 
                {
                    if (item.Name == App.global.buketName + "-" + Appid)
                    {
                        SelectedItem = item;
                    }
                    Workspace.This.Git.Insert(a1 + "：" + item.Name);
                    a1++;
                }
            }
        }
		private void ExecuteSave1Command(object parameter)
		{
            var a = Sqlsugar.db.Queryable<L_bucketClient>().Where(o => o.Type == 1).ToList();
			if (a.Count > 0)
			{
				L_bucketClient l_BucketClient = a[0];
				l_BucketClient.SecretId = SecretId;
				l_BucketClient.SecretKey = SecretKey;
				l_BucketClient.appid = Appid;
                l_BucketClient.buketName = SelectedItem.Name.Replace("-" + Appid, "");
                l_BucketClient.region = SelectedItem.Location;
                l_BucketClient.servermac = servermac;
				Sqlsugar.db.Updateable(l_BucketClient).ExecuteCommand();
			}
			else
			{
                L_bucketClient l_BucketClient =new L_bucketClient();
				l_BucketClient.Type = 1;
                l_BucketClient.SecretId = SecretId;
                l_BucketClient.SecretKey = SecretKey;
                l_BucketClient.appid = Appid;
                l_BucketClient.buketName = SelectedItem.Name.Replace("-" + Appid, "");
                l_BucketClient.region = SelectedItem.Location;
                l_BucketClient.servermac = servermac;
                Sqlsugar.db.Insertable(l_BucketClient).ExecuteCommand();
            }
            App.global.SecretId = SecretId;
            App.global.SecretKey = SecretKey;
            App.global.appid = Appid;
            App.global.buketName= SelectedItem.Name.Replace("-" + Appid, "");
            App.global.region = SelectedItem.Location;
            App.global.servermac = servermac;
            Workspace.This.Git.Insert("Git配置报错成功！");
        }
        private void GetCount(MetaLocationViewModel model1)
        {
            foreach (var child in model1.BackUpNodes)
            {
                if (child != null)
                {
                    ProgressBarValueMax++;
                    if (child.ChildrenCount > 0)
                    {
                        GetCount(child);
                    }
                }
            }
        }
        async Task DoAsyncLoop(MetaLocationViewModel model1)
        {
            SemaphoreSlim semaphore = new SemaphoreSlim(20);

            List<Task> tasks = new List<Task>();

            await Task.Run(async () =>
            {
                
                foreach (var item in model1.BackUpNodes)
                {
                    await semaphore.WaitAsync();

                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            if (File.Exists(item.Tag))
                            {
                                bool boo = await CosBuilder.CheckObjectAsync(item.Tag);
                                if (!boo)
                                {
                                    await CosBuilder.UploadFileAsync(item.Tag);
                                }
                            }
                            item.SaveType = BusinessLib.Models.SaveType.SaveOk;
                            ProgressBarValue++;
                            if (ProgressBarValue == ProgressBarValueMax)
                            {
                                ProgressBarVisible = Visibility.Visible; 
                                ProgressBarValue = 0;
                                
                                Workspace.This.Git.Insert("全部上传云端完成");
                            }
                            if (item.ChildrenCount > 0)
                            {
                                // 递归调用处理子文件夹
                                await DoAsyncLoop(item);
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }
                    model1.SaveType = BusinessLib.Models.SaveType.SaveOk;
                    ProgressBarValue++;
            });
        }
        private string _SecretId = "";
        public string SecretId
        {
            get { return _SecretId; }
            set
            {
                if (_SecretId != value)
                {
                    _SecretId = value;
                    RaisePropertyChanged(nameof(SecretId));
                }
            }
        }
        private string _SecretKey = "";
        public string SecretKey
        {
            get { return _SecretKey; }
            set
            {
                if (_SecretKey != value)
                {
                    _SecretKey = value;
                    RaisePropertyChanged(nameof(SecretKey));
                }
            }
        }
        private string _Appid = "";
        public string Appid
        {
            get { return _Appid; }
            set
            {
                if (_Appid != value)
                {
                    _Appid = value;
                    RaisePropertyChanged(nameof(Appid));
                }
            }
        }
        private string _buketName = "";
        public string buketName
        {
            get { return _buketName; }
            set
            {
                if (_buketName != value)
                {
                    _buketName = value;
                    RaisePropertyChanged(nameof(buketName));
                }
            }
        }
        private string _region = "";
        public string region
        {
            get { return _region; }
            set
            {
                if (_region != value)
                {
                    _region = value;
                    RaisePropertyChanged(nameof(region));
                }
            }
        }
        private string _servermac = "";
        public string servermac
        {
            get { return _servermac; }
            set
            {
                if (_servermac != value)
                {
                    _servermac = value;
                    RaisePropertyChanged(nameof(servermac));
                }
            }
        }
        private int _ProgressBarValue = 0;
        public int ProgressBarValue
        {
            get { return _ProgressBarValue; }
            set
            {
                if (_ProgressBarValue != value)
                {
                    _ProgressBarValue = value;
                    RaisePropertyChanged(nameof(ProgressBarValue));
                }
            }
        }
        private int _ProgressBarValueMax = 100;
        public int ProgressBarValueMax
        {
            get { return _ProgressBarValueMax; }
            set
            {
                if (_ProgressBarValueMax != value)
                {
                    _ProgressBarValueMax = value;
                    RaisePropertyChanged(nameof(ProgressBarValueMax));
                }
            }
        }
        private Visibility _ProgressBarVisible = Visibility.Visible;
        public Visibility ProgressBarVisible
        {
            get { return _ProgressBarVisible; }
            set
            {
                if (_ProgressBarVisible != value)
                {
                    _ProgressBarVisible = value;
                    RaisePropertyChanged(nameof(ProgressBarVisible));
                }
            }
        }
        private async void ExecuteSave2Command(object parameter)
		{
            Git_Add git_Add = new Git_Add();
            var result = git_Add.ShowDialog();
            if (result == true)
            {
                if (git_Add.Title !="")
                {
                    var a = await CosBuilder.CreateBucket(git_Add.Title);
                    if (a.Message.Contains("200 OK\n"))
                    {
                        Workspace.This.Git.Insert("成功创建存储桶：" + git_Add.Title + "-" + Appid);
                    }
                    ExecuteSaveCommand("");
                }
            }
            
        }
	}
}
