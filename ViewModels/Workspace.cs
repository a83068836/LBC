using AvalonDock.Themes;
using COSXML;
using FilterTreeViewLib.ViewModels;
using HL.Interfaces;
using ICSharpCode.AvalonEdit;
using LBC.Class;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace LBC.ViewModels
{
	internal class Workspace : ViewModelBase
	{
        #region fields

        private static Workspace _this = new Workspace();
        private ToolViewModel[] _tools;
        private ObservableCollection<FileViewModel> _files = new ObservableCollection<FileViewModel>();
        private ObservableCollection<CodeViewModel> _filesCode = new ObservableCollection<CodeViewModel>();
        private ReadOnlyObservableCollection<FileViewModel> _readonyFiles;
        private FileViewModel _activeDocument;
        private ErrorViewModel _errors;
        private PropertiesViewModel _props;
        private ExplorerViewModel _explorer;
        private OutputViewModel _output;
        private GitChangesViewModel _git;
        private ToolboxViewModel _toolbox;
        private NavigationViewModel _navigation;
        private RelayCommand _openCommand;
        private RelayCommand _newCommand;
        private RelayCommand _openProjectCommand;
        private RelayCommand _selectedThemeCommand;


        private Tuple<string, Theme> _selectedTheme;

        #endregion fields

        #region constructors

        /// <summary>
        /// Class constructor
        /// </summary>
        public Workspace()
        {
            SelectedTheme = Themes[0];
            //Nickname = "登录用户：" + App.global.l_OAuth.nickname;
            ExpirationTime = "到期时间：" + DateTime.Now.AddMinutes(App.expirationTime);
            Version = "当前版本：" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #endregion constructors

        public event EventHandler ActiveDocumentChanged;

        #region properties

        public static Workspace This => _this;

        public ReadOnlyObservableCollection<FileViewModel> Files
        {
            get
            {
                if (_readonyFiles == null)
                    _readonyFiles = new ReadOnlyObservableCollection<FileViewModel>(_files);

                return _readonyFiles;
            }
        }

        public IEnumerable<ToolViewModel> Tools
        {
            get
            {
                if (_tools == null)
                    _tools = new ToolViewModel[] { Explorer, Props, Errors, Output, Git, Toolbox };
                return _tools;
            }
        }

        public ExplorerViewModel Explorer
        {
            get
            {
                if (_explorer == null)
                    _explorer = new ExplorerViewModel();

                return _explorer;
            }
        }

        public PropertiesViewModel Props
        {
            get
            {
                if (_props == null)
                    _props = new PropertiesViewModel();

                return _props;
            }
        }

        public ErrorViewModel Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new ErrorViewModel();

                return _errors;
            }
        }

        public OutputViewModel Output
        {
            get
            {
                if (_output == null)
                    _output = new OutputViewModel();

                return _output;
            }
        }

        public GitChangesViewModel Git
        {
            get
            {
                if (_git == null)
                    _git = new GitChangesViewModel();

                return _git;
            }
        }

        public ToolboxViewModel Toolbox
        {
            get
            {
                if (_toolbox == null)
                    _toolbox = new ToolboxViewModel();

                return _toolbox;
            }
        }
        public NavigationViewModel Navigation
        {
            get
            {
                if (_navigation == null)
                    _navigation = new NavigationViewModel(new TextEditor());

                return _navigation;
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand((p) => OnOpen(p), (p) => CanOpen(p));
                }

                return _openCommand;
            }
        }
        public ICommand NewCommand
        {
            get
            {
                if (_newCommand == null)
                {
                    _newCommand = new RelayCommand((p) => OnNew(p), (p) => CanNew(p));
                }

                return _newCommand;
            }
        }

        #region 充值退出登录
        private RelayCommand _rechargeCommand;
        public ICommand RechargeCommand
        {
            get
            {
                if (_rechargeCommand == null)
                {
                    _rechargeCommand = new RelayCommand((p) => OnRecharg(p));
                }

                return _rechargeCommand;
            }
        }
        public void OnRecharg(object parameter)
        {
            //Lcode.Tools.Recharge recharge = new Tools.Recharge();
            //recharge.ShowDialog();
        }

        private RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new RelayCommand((p) => OnExit(p));
                }

                return _exitCommand;
            }
        }
        public void OnExit(object parameter)
        {
            CheckUserAuthentication.RemoveToken();
            RestartApplication();
        }
        // 重新启动应用程序
        private void RestartApplication()
        {
            var currentProcess = Process.GetCurrentProcess();
            var startInfo = new ProcessStartInfo
            {
                FileName = currentProcess.MainModule.FileName,
                UseShellExecute = true
            };

            Process.Start(startInfo);
            Application.Current.Shutdown();
            //currentProcess.CloseMainWindow();
        }
        #endregion

        #region 打开项目
        public ICommand OpenProjectCommand
        {
            get
            {
                if (_openProjectCommand == null)
                {
                    _openProjectCommand = new RelayCommand((p) => OnNewProject(p), (p) => CanNewProject(p));
                }

                return _openProjectCommand;
            }
        }
        private bool CanNewProject(object parameter)
        {
            return true;
        }

        private async void OnNewProject(object parameter)
        {
            // 创建一个 FolderBrowserDialog 对象
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();

            // 显示对话框以选择一个目录
            System.Windows.Forms.DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            // 检查用户是否点击了确定按钮
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // 获取所选目录的路径
                string selectedPath = folderBrowserDialog.SelectedPath;

                // 在此处使用所选目录的路径进行操作
                // ...
                await This.Explorer.LoadSampleDataAsync(selectedPath, 0);
            }

        }
        #endregion
        public FileViewModel ActiveDocument
        {
            get => _activeDocument;
            set
            {
                if (_activeDocument != value)
                {
                    _activeDocument = value;
                    RaisePropertyChanged(nameof(ActiveDocument));
                    if (ActiveDocumentChanged != null)
                        ActiveDocumentChanged(this, EventArgs.Empty);
                    if (ActiveDocument.Document != null)
                        Workspace.This.Props.FoldingManagerUpdateAsync(ActiveDocument.Document.Text);
                }
            }
        }

        public void SetCaretPosition(string fullfilename,int position)
        {
            if (This.ActiveDocument.FilePath == fullfilename)
            {
                This.ActiveDocument.CaretOffset = position;
                return;
            }   
            foreach (var file in Files)
            {
                if (file.FilePath == fullfilename)
                { 
                    This.ActiveDocument = file;
                    This.ActiveDocument.CaretOffset = position;
                    break;
                }
            }
        }
        public List<Tuple<string, Theme>> Themes { get; set; } = new List<Tuple<string, Theme>>
        {
            new Tuple<string, Theme>(nameof(Vs2013DarkTheme),new Vs2013DarkTheme()),
            new Tuple<string, Theme>(nameof(Vs2013LightTheme),new Vs2013LightTheme()),
            new Tuple<string, Theme>(nameof(Vs2013BlueTheme),new Vs2013BlueTheme())
        };

        public Tuple<string, Theme> SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                _selectedTheme = value;
                SwitchExtendedTheme();
                RaisePropertyChanged(nameof(SelectedTheme));
            }
        }

        public ICommand SelectedThemeCommand
        {
            get
            {
                if (_selectedThemeCommand == null)
                {
                    _selectedThemeCommand = new RelayCommand((p) => OnSelectedTheme(p));
                }

                return _selectedThemeCommand;
            }
        }
        public void OnSelectedTheme(object parameter)
        {
            if (parameter.ToString() == "黑色")
            {
                SelectedTheme = Themes.First();
            }
            else if (parameter.ToString() == "亮色")
            {
                SelectedTheme = Themes[1];
            }
            else
            {
                SelectedTheme = Themes[2];
            }
        }

        private bool _isSelected1;
        public bool IsSelected1
        {
            get { return _isSelected1; }
            set
            {
                _isSelected1 = value;
                RaisePropertyChanged(nameof(IsSelected1));
            }
        }
        private bool _isSelected2;
        public bool IsSelected2
        {
            get { return _isSelected2; }
            set
            {
                _isSelected2 = value;
                RaisePropertyChanged(nameof(IsSelected2));
            }
        }
        private bool _isSelected3;
        public bool IsSelected3
        {
            get { return _isSelected3; }
            set
            {
                _isSelected3 = value;
                RaisePropertyChanged(nameof(IsSelected3));
            }
        }

        #endregion properties


        #region methods
        
        private void SwitchExtendedTheme()
        {
            var hlManager = Base.ModelBase.GetService<IThemedHighlightingManager>();


            switch (_selectedTheme.Item1)
            {
                case "Vs2013DarkTheme":
                    Application.Current.Resources.MergedDictionaries.Clear();
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MLib;component/Themes/DarkTheme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/DarkBrushsExtended.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Generic.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Controls/VsResizeGrip.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/DarkBrushs.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/DarkIcons.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/UnitComboLib;component/Themes/DarkBrushs.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/Theme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/SkinDark.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/ICSharpCode.AvalonEdit;component/themes/DarkTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MLib;component/Themes/DarkTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Themes/DarkBrushsExtended.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDark.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Resources/Themes/Theme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Resources/Themes/SkinDark.xaml") });;
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/UnitComboLib;component/Themes/DarkBrushs.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/DarkBrushs.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/DarkIcons.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/ICSharpCode.AvalonEdit;component/themes/DarkTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Themes/AppFonts.xaml") });
                    hlManager.SetCurrentTheme("VS2019_Dark");
                    foreach (var file in _files)
                    {
                        RaisePropertyChanged(nameof(file.HighlightingDefinitions));
                        if (file != null)
                            file.OnAppThemeChanged(hlManager);
                    }
                    foreach (var file in _filesCode)
                    {
                        RaisePropertyChanged(nameof(file.HighlightingDefinitions));
                        if (file != null)
                            file.OnAppThemeChanged(hlManager);
                    }

                    IsSelected1 = true;
                    IsSelected2 = false;
                    IsSelected3 = false;
                    break;
                case "Vs2013LightTheme":
                    Application.Current.Resources.MergedDictionaries.Clear();
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MLib;component/Themes/LightTheme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/LightBrushsExtended.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Generic.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Controls/VsResizeGrip.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightBrushs.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightIcons.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/UnitComboLib;component/Themes/LightBrushs.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/Theme.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/SkinDefault.xaml") });
                    //Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/ICSharpCode.AvalonEdit;component/themes/LightTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MLib;component/Themes/LightTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Themes/LightBrushsExtended.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Resources/Themes/Theme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Resources/Themes/SkinDefault.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/UnitComboLib;component/Themes/LightBrushs.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightBrushs.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightIcons.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/ICSharpCode.AvalonEdit;component/themes/LightTheme.xaml") });
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/LBC;component/Themes/AppFonts.xaml") });

                    hlManager.SetCurrentTheme("Light");
                    foreach (var file in _files)
                    {
                        RaisePropertyChanged(nameof(file.HighlightingDefinitions));
                        if (file != null)
                            file.OnAppThemeChanged(hlManager);
                    }
                    foreach (var file in _filesCode)
                    {
                        RaisePropertyChanged(nameof(file.HighlightingDefinitions));
                        if (file != null)
                            file.OnAppThemeChanged(hlManager);
                    }
                    IsSelected1 = false;
                    IsSelected2 = true;
                    IsSelected3 = false;
                    break;
                //case "Vs2013BlueTheme":
                //    Application.Current.Resources.MergedDictionaries.Clear();
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MLib;component/Themes/LightTheme.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/BlueBrushsExtended.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Generic.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Themes/Controls/VsResizeGrip.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightBrushs.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/TextEditLib;component/Themes/LightIcons.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/UnitComboLib;component/Themes/LightBrushs.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/Theme.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Lcode;component/Resources/Themes/SkinDefault.xaml") });
                //    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/ICSharpCode.AvalonEdit;component/themes/LightTheme.xaml") });
                //    hlManager.SetCurrentTheme("Light");
                //    foreach (var file in _files)
                //    {
                //        RaisePropertyChanged(nameof(file.HighlightingDefinitions));
                //        if (file != null)
                //            file.OnAppThemeChanged(hlManager);
                //    }
                //    IsSelected1 = false;
                //    IsSelected2 = false;
                //    IsSelected3 = true;
                //    break;
                default:
                    break;
            }
        }

        internal void Close(FileViewModel fileToClose)
        {
            if (fileToClose.IsDirty)
            {
                var res = HandyControl.Controls.MessageBox.Show(string.Format("将更改保存为文件 '{0}'?", fileToClose.FilePath), "保存提示", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                    return;
                if (res == MessageBoxResult.Yes)
                {
                    Save(fileToClose);
                }
            }

            _files.Remove(fileToClose);
        }

        internal void Save(FileViewModel fileToSave, bool saveAsFlag = false)
        {
            string newTitle = string.Empty;

            if (fileToSave.FilePath == null || saveAsFlag)
            {
                var dlg = new SaveFileDialog();
                if (dlg.ShowDialog().GetValueOrDefault())
                {
                    fileToSave.FilePath = dlg.FileName;
                    newTitle = dlg.SafeFileName;
                }
            }
            if (fileToSave.FilePath == null)
            {
                return;
            }
            File.WriteAllText(fileToSave.FilePath, fileToSave.Document.Text, fileToSave.FileEncoding);
            ActiveDocument.IsDirty = false;
            ExecuteCodeButton_Click(fileToSave.FilePath);

            if (string.IsNullOrEmpty(newTitle)) return;
            ActiveDocument.Title = newTitle;
        }
        static async Task DoAsyncLoop(MetaLocationViewModel model1)
        {
            foreach (var item in model1.Children)
            {
                item.IsItemExpanded = true;
                item.SaveType = BusinessLib.Models.SaveType.SaveOk;
                if (item.ChildrenCount > 0)
                {
                    DoAsyncLoop(item);
                }
            }
        }
        private async void ExecuteCodeButton_Click(string FilePath)
        {
            await Task.Run(async () =>
            {
                Automaticloading.AutoLoading(FilePath);

                try
                {
                    CosBuilder.UploadFileAsync(FilePath);

                    //bucketClient.UpFile("Boxs\\BoxsList.txt", "E:\\Mirserver\\Mir200\\Envir\\Boxs\\BoxsList.txt");
                    //bucketClient.UploadFileAsync("Mirserver\\Mir200\\Envir\\MapQuest_Def", "QManage.txt", "E:\\Mirserver\\Mir200\\Envir\\MapQuest_Def\\QManage.txt");
                    //_ = Task.Run(() => DoAsyncLoop(Workspace.This.Explorer.Root.CountryRootItems[0]));
                    //FindChildSaveType(This.Explorer.Root.)
                    ChildSaveType(This.Explorer.Root.CountryRootItems[0],FilePath);
                    await CosBuilder.UploadFileAsync(FilePath);
                    MyMessage myMessage = new MyMessage();
                    myMessage.type = 0;
                    myMessage.filename = FilePath;
                    myMessage.l_BucketClient = new WebFirst.Entities.L_bucketClient()
                    {
                        SecretId = App.global.SecretId,
                        SecretKey = App.global.SecretKey,
                        appid = App.global.appid,
                        buketName = App.global.buketName,
                        region = App.global.region,
                        servermac = App.global.servermac,
                    };
                    myMessage.autojson= File.ReadAllText(System.Windows.Forms.Application.StartupPath + "Settings\\Automatic.dll");
                    App.WebSocketTest.SendMessage(myMessage);
                    //if (App.global.gitmodel != null)
                    //{

                    //App.global.client.UploadFile(FilePath, FilePath.Replace(App.global.gitmodel.LocalSyncPath, ""));
                    //string sdf = System.Windows.Forms.Application.StartupPath;
                    //App.global.client.UploadFile(sdf + @"Settings\\Automatic.dll", @"\Automatic.dll");
                    //File.WriteAllText(sdf + "Settings\\a.txt", FilePath.Replace(App.global.gitmodel.LocalSyncPath, ""));
                    //App.global.client.UploadFile(sdf + "Settings\\a.txt", @"\a.txt");
                    //}

                }
                catch (Exception ex)
                {
                    Workspace.This.Git.Insert("错误信息：" + ex.Message.ToString());
                }

            });

            Workspace.This.Git.Insert("上传成功：" + FilePath + "	云端M2自动加载成功");
        }

        private bool ChildSaveType(MetaLocationViewModel model1, string Tag)
        {
            bool isreturn = false;
            foreach (var item in model1.BackUpNodes)
            {
                if (isreturn)
                    return true;
                if (item.Tag == Tag)
                {
                    //item.Parent.IsItemExpanded = true;
                    item.SaveType = BusinessLib.Models.SaveType.SaveOk;
                    //item.Parent.IsItemExpanded = false;
                    return true;
                }
                if (item.ChildrenCount > 0)
                {
                    isreturn = ChildSaveType(item, Tag);
                }
            }
            return false;
        }
        private List<MetaLocationViewModel> AAA(MetaLocationViewModel metaLocationViewModel, List<MetaLocationViewModel> mm)
        {
            if (metaLocationViewModel.Parent != null)
            { 
                mm.Add(metaLocationViewModel.Parent);
                mm=AAA(metaLocationViewModel.Parent, mm);
                return mm;
            }
            return mm;
        }
        internal void SaveAll()
        {
            if (_files != null)
            {
                foreach (var f in Files)
                {
                    if (f == null) continue;
                    if (!f.IsDirty) continue;
                    ActiveDocument = f;
                    Save(f, false);
                }
            }
        }

        internal FileViewModel Open(string filepath)
        {
            var fileViewModel = _files.FirstOrDefault(fm => fm.FilePath == filepath);
            if (fileViewModel != null)
                return fileViewModel;

            if (File.Exists(filepath))
            {
                fileViewModel = new FileViewModel(filepath);
                _files.Add(fileViewModel);
                return fileViewModel;
            }
            else
            {
                return null;
            }
        }

        #region OpenCommand
        private bool CanOpen(object parameter) => true;

        private void OnOpen(object parameter)
        {

            if (parameter != null)
            {
                var pp = parameter as string;
                if (pp == "1")
                {
                    var dlg = new OpenFileDialog();
                    if (dlg.ShowDialog().GetValueOrDefault())
                    {
                        var fileViewModel = Open(dlg.FileName);
                        ActiveDocument = fileViewModel;
                    }
                }
                else
                {
                    var PP = parameter as MetaLocationViewModel;
                    string filePath = PP.Tag.ToString();
                    if (File.Exists(filePath))
                    {
                        var fileViewModel = Open(filePath);
                        ActiveDocument = fileViewModel;
                    }
                }
            }
        }

        #endregion OpenCommand

        #region NewCommand

        private bool CanNew(object parameter)
        {
            return true;
        }

        private void OnNew(object parameter)
        {
            _files.Add(new FileViewModel());
            ActiveDocument = _files.Last();
        }

        public void OnNewCode(CodeViewModel fileViewModel)
        {
            _filesCode.Add(fileViewModel);
        }

        #endregion NewCommand

        #endregion methods

        #region ADLayout
        //private AvalonDockLayoutViewModel mAVLayout = null;

        ///// <summary>
        ///// Expose command to load/save AvalonDock layout on application startup and shut-down.
        ///// </summary>
        //public AvalonDockLayoutViewModel ADLayout
        //{
        //	get
        //	{
        //		if (this.mAVLayout == null)
        //			this.mAVLayout = new AvalonDockLayoutViewModel();

        //		return this.mAVLayout;
        //	}
        //}

        public static string LayoutFileName
        {
            get
            {
                return "Layout.config";
            }
        }
        #endregion ADLayout

        #region 退出登录


        #endregion

        #region Application Properties
        /// <summary>
        /// Get a path to the directory where the application
        /// can persist/load user data on session exit and re-start.
        /// </summary>
        public static string DirAppData
        {
            get
            {
                string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                 System.IO.Path.DirectorySeparatorChar + Workspace.Company;

                try
                {
                    if (System.IO.Directory.Exists(dirPath) == false)
                        System.IO.Directory.CreateDirectory(dirPath);
                }
                catch
                {
                }

                return dirPath;
            }
        }

        public static string Company
        {
            get
            {
                return "LBC";
            }
        }
        #endregion Application Properties

        #region 首页底部信息
        private string nickname = string.Empty;
        public string Nickname
        {
            get
            {
                return this.nickname;
            }

            set
            {
                if (this.nickname != value)
                {
                    this.nickname = value;
                    this.RaisePropertyChanged("Nickname");
                }
            }
        }

        private string expirationTime = string.Empty;
        public string ExpirationTime
        {
            get
            {
                return this.expirationTime;
            }

            set
            {
                if (this.expirationTime != value)
                {
                    this.expirationTime = value;
                    this.RaisePropertyChanged("ExpirationTime");
                }
            }
        }
        private string version = string.Empty;
        public string Version
        {
            get
            {
                return this.version;
            }

            set
            {
                if (this.version != value)
                {
                    this.version = value;
                    this.RaisePropertyChanged("Version");
                }
            }
        }
        private string points = string.Empty;
        public string Points
        {
            get
            {
                return this.points;
            }

            set
            {
                if (this.points != value)
                {
                    this.points = value;
                    this.RaisePropertyChanged("Points");
                }
            }
        }
        #endregion
    }
}