using FilterTreeViewLib.ViewModels.Base;
using FilterTreeViewLib.ViewModels;
using FilterTreeViewLib.ViewModelsSearch.SearchModels.Enums;
using FilterTreeViewLib.ViewModelsSearch.SearchModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using FilterTreeView.Tasks;
using System.Windows;
using LBC.Class;
using SqlSugar;
using WebFirst.Entities;

namespace LBC.ViewModels
{
    internal class ExplorerViewModel : ToolViewModel
    {
        #region fields
        public const string ToolContentId = "Explorer";
        private readonly OneTaskProcessor _procesor;


        private MetaLocationRootViewModel _Root;

        private ICommand _SearchCommand;
        private bool _Disposed;

        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public ExplorerViewModel()
            : base("解决方案")
        {
            Workspace.This.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;
            _SearchString = "";
            _IsProcessing = _IsLoading = false;
            _CountSearchMatches = 0;
            _IsStringContainedSearchOption = true;
            _procesor = new OneTaskProcessor();
            _Root = new MetaLocationRootViewModel();
            _Disposed = false;
            //_ = Task.Run(() => LoadSampleDataAsync("", 0));
            //LoadTing("L:\\FTP");
            LoadSampleDataAsync("", 0);

        }
        #endregion constructors

        //static async Task DoAsyncLoop(MetaLocationViewModel model1)
        //{
        //    foreach (var item in model1.Children)
        //    {
        //        if (File.Exists(item.Tag))
        //        {
        //            bool boo = await FilterTreeViewLib.CosBuilder.CheckObjectAsync(item.Tag);
        //            if (!boo)
        //            {
        //                await FilterTreeViewLib.CosBuilder.UploadFileAsync(item.Tag);
        //            }
        //            item.SaveType = BusinessLib.Models.SaveType.SaveOk;
        //            Application.Current.Dispatcher.Invoke(() =>
        //            {
        //                Workspace.This.Git.Insert("上传云端成功：" + item.Tag + " ");
        //            });
        //        }

        //        if (item.ChildrenCount > 0)
        //        {
        //            await DoAsyncLoop(item);
        //        }
        //    }
        //    model1.SaveType = BusinessLib.Models.SaveType.SaveOk;
        //}
        static async Task DoAsyncLoop(MetaLocationViewModel model1)
        {
            // 限制最多同时运行的线程数为10
            SemaphoreSlim semaphore = new SemaphoreSlim(1);

            await Task.Run(async () =>
            {
                foreach (var item in model1.Children)
                {
                    await semaphore.WaitAsync(); // 等待信号量可用

                    _ = Task.Run(async () =>
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

                            if (item.ChildrenCount > 0)
                            {
                                // 递归调用处理子文件夹
                                await DoAsyncLoop(item);
                            }
                        }
                        finally
                        {
                            semaphore.Release(); // 释放信号量
                        }
                    });
                }
            });

            // 更新文件夹及文件的状态
            _ = Application.Current.Dispatcher.InvokeAsync(() =>
            {
                model1.SaveType = BusinessLib.Models.SaveType.SaveOk;
                foreach (var item in model1.Children)
                {
                    item.SaveType = BusinessLib.Models.SaveType.SaveOk;
                }
            });

        }

        #region methods
        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {


        }
        #endregion methods
        #region baseviewmodel
        /// <summary>
        /// Standard event handler of the <seealso cref="INotifyPropertyChanged"/> interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Tell bound controls (via WPF binding) to refresh their display.
        /// 
        /// Sample call: this.NotifyPropertyChanged(() => this.IsSelected);
        /// where 'this' is derived from <seealso cref="BaseViewModel"/>
        /// and IsSelected is a property.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            this.OnPropertyChanged(memberExpression.Member.Name);
        }

        /// <summary>
        /// Tell bound controls (via WPF binding) to refresh their display.
        /// 
        /// Sample call: this.OnPropertyChanged("IsSelected");
        /// where 'this' is derived from <seealso cref="BaseViewModel"/>
        /// and IsSelected is a property.
        /// </summary>
        /// <param name="propertyName">Name of property to refresh</param>
        public void OnPropertyChanged(string propertyName)
        {
            try
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
            }
        }
        #endregion


        #region appbaseviewmodel
        #region fields
        private bool _IsStringContainedSearchOption;
        private int _CountSearchMatches;
        private bool _IsProcessing;
        private bool _IsLoading;

        private string _StatusStringResult;
        private string _SearchString;
        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        #endregion constructors

        #region properties
        /// <summary>
        /// Gets a property to determine if application is currently processing
        /// data (loading or searching for matches in the tree view) or not.
        /// </summary>
        public bool IsProcessing
        {
            get { return _IsProcessing; }
            set
            {
                if (_IsProcessing != value)
                {
                    _IsProcessing = value;
                    RaisePropertyChanged(nameof(IsProcessing));
                }
            }
        }

        /// <summary>
        /// Gets a property to determine if application is currently processing
        /// data (loading or searching for matches in the tree view) or not.
        /// </summary>
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                if (_IsLoading != value)
                {
                    _IsLoading = value;
                    RaisePropertyChanged(nameof(IsLoading));
                }
            }
        }

        /// <summary>
        /// Gets the input string from search textbox control.
        /// </summary>
        public string SearchString
        {
            get { return _SearchString; }
            set
            {
                if (_SearchString != value)
                {
                    _SearchString = value;
                    RaisePropertyChanged(nameof(SearchString));
                }
            }
        }

        /// <summary>
        /// Gets the search string that is synchronized with the results from the search algorithm.
        /// </summary>
        public string StatusStringResult
        {
            get { return _StatusStringResult; }
            set
            {
                if (_StatusStringResult != value)
                {
                    _StatusStringResult = value;
                    RaisePropertyChanged(nameof(StatusStringResult));
                }
            }
        }

        /// <summary>
        /// Determines whether the search is looking for:
        /// - strings that are contained in a given string or
        /// - strings that match the searched string with all letters.
        /// </summary>
        public bool IsStringContainedSearchOption
        {
            get { return _IsStringContainedSearchOption; }
            set
            {
                if (_IsStringContainedSearchOption != value)
                {
                    _IsStringContainedSearchOption = value;
                    RaisePropertyChanged(nameof(IsStringContainedSearchOption));
                }
            }
        }

        /// <summary>
        /// Gets the number of matches that are found durring
        /// a search in the tree view nodes for a given string.
        /// </summary>
        public int CountSearchMatches
        {
            get { return _CountSearchMatches; }
            protected set
            {
                if (_CountSearchMatches != value)
                {
                    _CountSearchMatches = value;
                    RaisePropertyChanged(nameof(CountSearchMatches));
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Loads the initial sample data from XML file into memory
        /// </summary>
        /// <returns></returns>
        //public async virtual  Task LoadSampleDataAsync() { }
        #endregion methods

        #region properties
        /// <summary>
        /// Gets the root viewmodel that can be bound to a treeview control.
        /// </summary>
        public MetaLocationRootViewModel Root
        {
            get
            {
                return _Root;
            }
            set
            {

                if (_Root != value)
                {
                    _Root = value;
                    RaisePropertyChanged(nameof(Root));
                }
            }
        }

        /// <summary>
        /// Gets a command that filters the display of nodes in a treeview
        /// with a filterstring (node is shown if filterstring is contained).
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new RelayCommand<object>(async (p) =>
                    {
                        string findThis = p as string;

                        if (findThis == null)
                            return;
                        if (findThis == "")
                        {
                            await LoadSampleDataAsync("", 0);
                        }
                        else
                        {
                            await SearchCommand_ExecutedAsync(findThis);
                        }  
                    },
                    (p =>
                    {
                        if (Root.BackUpCountryRootsCount == 0 && IsProcessing == false)
                            return false;

                        return true;
                    })
                    );
                }

                return _SearchCommand;
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Implements the <see cref="IDisposable"/> interface.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        FileSystemWatcher watcher;
        bool isProcessing = false;
        public async void LoadTing(string rootFolderPath)
        {
            try
            {
                watcher = new FileSystemWatcher();
                watcher.Path = rootFolderPath;
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                watcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;

                // 当文件系统有变动时，自动更新 TreeView
                watcher.Changed += (sender, e) =>
                {
                    if (!isProcessing)
                    {
                        isProcessing = true;
                        LoadSampleDataAsync(rootFolderPath, 1);
                        isProcessing = false;
                    }
                };
                watcher.Created += (sender, e) => LoadSampleDataAsync(rootFolderPath, 1);
                watcher.Deleted += (sender, e) => LoadSampleDataAsync(rootFolderPath, 1);
                watcher.Renamed += (sender, e) => LoadSampleDataAsync(rootFolderPath, 1);
            }
            catch { }
            // 监听文件系统事件

        }
        public async void LoadSampleDataAsyncT(string selectedPath, int id)
        {
            await LoadSampleDataAsync(selectedPath, id);
        }
        /// <summary>
        /// Loads the initial sample data from XML file into memory
        /// </summary>
        /// <returns></returns>
        public async Task LoadSampleDataAsync(string selectedPath, int id)
        {
            if (id == 0)
            {
                if (selectedPath == "")
                {
                    var list = Sqlsugar.db.Queryable<L_setting>().Where(it => it.Id == 1).ToList();
                    if (list.Count > 0)
                    {
                        selectedPath = list[0].folderPath;
                    }
                }
                else
                {
                    Sqlsugar.db.Queryable<L_setting>().Where(it => it.Id == 1).ToList();
                    L_setting l_Setting = new L_setting() { Id = 1, folderPath = selectedPath };
                    Sqlsugar.db.Storageable(l_Setting).ExecuteCommand();
                }
            }
            if (selectedPath == "")
                return;

            IsProcessing = true;
            IsLoading = true;
            StatusStringResult = "正在加载数据... 请稍后.";
            try
            {
                await Root.LoadData(selectedPath);

                IsLoading = false;
                StatusStringResult = string.Format("Searching... '{0}'", SearchString);

                await SearchCommand_ExecutedAsync(SearchString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("1");
            }
            finally
            {
                IsLoading = false;
                IsProcessing = false;
            }
            //MessageBox.Show("1");
        }

        /// <summary>
        /// Filters the display of nodes in a treeview
        /// with a filterstring (node is shown if filterstring is contained).
        /// </summary>
        protected async Task<int> SearchCommand_ExecutedAsync(string findThis)
        {
            // Setup search parameters
            SearchParams param = new SearchParams(findThis
                , (IsStringContainedSearchOption == true ?
                    SearchMatch.StringIsContained : SearchMatch.StringIsMatched));

            // Make sure the task always processes the last input but is not started twice
            try
            {
                    IsProcessing = true;

                    var tokenSource = new CancellationTokenSource();
                    Func<int> a = new Func<int>(() => Root.DoSearch(param, tokenSource.Token));
                    var t = await _procesor.ExecuteOneTask(a, tokenSource);

                    this.StatusStringResult = findThis;
                    CountSearchMatches = t;
                return CountSearchMatches;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            finally
            {
                IsProcessing = false;
            }

            return -1;
        }


        /// <summary>
        /// The bulk of the clean-up code is implemented here.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_Disposed == false)
            {
                if (disposing == true)
                {
                    _procesor.Dispose();
                }

                _Disposed = true;
            }
        }
        #endregion methods

        #endregion
    }
}
