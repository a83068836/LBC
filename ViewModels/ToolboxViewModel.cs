using ICSharpCode.AvalonEdit;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TextEditLib.Class;
using UnitComboLib.Command;

namespace LBC.ViewModels
{
	internal class ToolboxViewModel : ToolViewModel
	{
        #region fields
        public const string ToolContentId = "Toolbox";

        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public ToolboxViewModel()
            : base("������")
        {
            Workspace.This.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;
            OnlodeAsync();
        }
        #endregion constructors

        #region SetTBTextChanged
        RelayCommand<object> _setTBTextChanged = null;
        public ICommand SetTBTextChanged
        {
            get
            {
                if (_setTBTextChanged == null)
                {
                    _setTBTextChanged = new RelayCommand<object>((p) => OnSetTBTextChanged(p), (p) => CanSetTBTextChanged(p));
                }

                return _setTBTextChanged;
            }
        }

        private bool CanSetTBTextChanged(object p)
        {
            return true;
        }

        private void OnSetTBTextChanged(object p)
        {
            if (p != null && !string.IsNullOrWhiteSpace(p.ToString()))
            {
                ToolboxInfos = new ObservableCollection<Toolbox>(OldToolboxInfos.Where(t => t.DisplayName.ToLower().Contains(p.ToString().ToLower())));
                ;
            }
            else
            {
                ToolboxInfos = OldToolboxInfos;
            }
        }
        #endregion
        public async Task OnlodeAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var list = App.global.Globals_L_items;
            // ִ�й�����ѯ������ȡ��������Ϣ������
            var list1 = App.global.Globals_L_shuxing;
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            foreach (var item in list)
            {
                Toolbox toolbox = new Toolbox();
                toolbox.DisplayName = item.displayName;
                toolbox.Geshi = item.geshi;
                toolbox.ImagePath = GetImagePate(toolbox.DisplayName);
                var filteredList = list1.Where(shuxingItem => shuxingItem.tid == item.Id).ToList();
                foreach (var shuxing1 in filteredList)
                {
                    toolbox.Shuxing.Add(new Shuxing(shuxing1.title, shuxing1.shuoming, shuxing1.title));
                }
                ToolboxInfos.Add(toolbox);
            }
            stopwatch.Stop();
            long elapsedTime1 = stopwatch.ElapsedMilliseconds;
            OldToolboxInfos = ToolboxInfos;
        }
        public ImageSource GetImagePate(string name)
        {
            string resule = "/LBC;component/Resources/toolbox/Ĭ��.png";
            try
            {
                
                if (!string.IsNullOrEmpty(name))
                {
                    switch (name)
                    {
                        case "ͼƬ":
                            resule = "/LBC;component/Resources/toolbox/ͼƬ.png";
                            break;
                        case "����ͼƬ":
                            resule = "/LBC;component/Resources/toolbox/ͼƬ.png";
                            break;
                        case "��������":
                            resule = "/LBC;component/Resources/toolbox/����.png";
                            break;
                        default:
                            resule = "/LBC;component/Resources/toolbox/Ĭ��.png";
                            break;
                    }
                }
            }
            catch (Exception ex){
                var a =ex.Message;
            }
            
            return new BitmapImage(new Uri("pack://application:,,," + resule));
        }
        #region Properties
        private ObservableCollection<Toolbox> _toolboxInfos = new ObservableCollection<Toolbox>();
        private ObservableCollection<Toolbox> OldToolboxInfos = new ObservableCollection<Toolbox>();
        private Toolbox _selectedtoolbox;
        public ObservableCollection<Toolbox> ToolboxInfos
        {
            get => _toolboxInfos;
            set
            {
                _toolboxInfos = value;
                RaisePropertyChanged(nameof(ToolboxInfos));
            }
        }
        public Toolbox SelectedToolbox
        {
            get => _selectedtoolbox;
            set
            {
                if (_selectedtoolbox != value)
                {
                    _selectedtoolbox = value;
                    RaisePropertyChanged(nameof(SelectedToolbox));
                }
            }
        }
        #endregion

        #region ItemClickCommand
        RelayCommand<object> _itemClickCommand = null;
        public ICommand ItemClickCommand
        {
            get
            {
                if (_itemClickCommand == null)
                {
                    _itemClickCommand = new RelayCommand<object>((p) => CanItemClickCommand(p));
                }

                return _itemClickCommand;
            }
        }
        private bool CanItemClickCommand(object p)
        {
            if (p != null)
            {
                //Workspace.This.ActiveDocument.ScrollLine = (FoldInfo)p;
                //var scrollViewer = GetScrollViewer(AvalonEditInstance);
                //if (scrollViewer != null)
                //{
                //    // ���ù���λ�ã�����ʾ������Ϊ����������Ը�����Ҫ���е�����
                //    scrollViewer.ScrollToTop();
                //}
            }
            return true;
        }


        #endregion Properties

        #region methods
        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {

        }
        #endregion methods

    }
}
