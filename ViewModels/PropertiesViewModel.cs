using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TextEditLib.Class;
using UnitComboLib.Command;

namespace LBC.ViewModels
{
    internal class PropertiesViewModel : ToolViewModel
    {
        #region fields
        public const string ToolContentId = "Properties";

        #endregion fields

        #region constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public PropertiesViewModel()
            : base("方法")
        {
            ContentId = ToolContentId;
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
                    _setTBTextChanged = new RelayCommand<object>((p) => OnSetTBTextChanged(), (p) => CanSetTBTextChanged(p));
                }

                return _setTBTextChanged;
            }
        }

        private bool CanSetTBTextChanged(object p)
        {
            FoldingManagerUpdateAsync(Workspace.This.ActiveDocument.Document.Text, p.ToString());
            return true;
        }
        private void OnSetTBTextChanged()
        {
            //ApplicationViewModel.This.Close(this);
        }


        #endregion


        ObservableCollection<FoldInfo> FoldInfos1 = new ObservableCollection<FoldInfo>();
        public async Task FoldingManagerUpdateAsync(string str, string searchtext = "")
        {
            FoldInfos1 = new ObservableCollection<FoldInfo>();
            string inputString = str;
            string pattern = @"\[@(.*?)\]";
            string[] lines = inputString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            int startOffset = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                MatchCollection matches = Regex.Matches(lines[i], pattern);

                foreach (Match match in matches)
                {
                    string matchedValue = match.Groups[1].Value;
                    int lineNumber = i;
                    int startIndex = match.Index + startOffset;
                    int endIndex = startIndex + match.Length - 1;
                    FoldInfo foldInfo = new FoldInfo();
                    foldInfo.DisplayName = "[@" + matchedValue + "]";
                    foldInfo.lineNumber = lineNumber;
                    foldInfo.StartOffset = startOffset;
                    foldInfo.EndOffset = endIndex;
                    if (searchtext != "")
                    {
                        if (foldInfo.DisplayName.ToUpper().Contains(searchtext.ToUpper()))
                        {
                            FoldInfos1.Add(foldInfo);
                        }
                    }
                    else
                    {
                        FoldInfos1.Add(foldInfo);
                    }
                }

                startOffset += lines[i].Length + "\r\n".Length;
            }
            if (FoldInfos != null && FoldInfos1 != null)
            {
                if (FoldInfos.Count != FoldInfos1.Count)
                    FoldInfos = FoldInfos1;
                else
                {
                    bool areEqual = true;
                    for (int i = 0; i < FoldInfos1.Count; i++)
                    {
                        if (FoldInfos1[i].StartOffset != FoldInfos[i].StartOffset
                            || FoldInfos1[i].EndOffset != FoldInfos[i].EndOffset
                            || FoldInfos1[i].DisplayName != FoldInfos[i].DisplayName
                            || FoldInfos1[i].lineNumber != FoldInfos[i].lineNumber
                            || FoldInfos1[i].IsSelected != FoldInfos[i].IsSelected)
                        {
                            areEqual = false;
                            break;
                        }
                    }
                    if (areEqual)
                    {
                        // 完全相同
                    }
                    else
                    {
                        FoldInfos = FoldInfos1;
                    }
                }
            }
            else
            {
                FoldInfos = FoldInfos1;
            }
            //await Task.CompletedTask; // 这里可以使用
            await Task.Delay(0); //或其他异步操作，模拟真实的异步操作
        }

        private ObservableCollection<FoldInfo> _foldInfos;
        private FoldInfo _selectedFold;
        public ObservableCollection<FoldInfo> FoldInfos
        {
            get => _foldInfos;
            set
            {
                _foldInfos = value;
                RaisePropertyChanged(nameof(FoldInfos));
            }
        }
        public FoldInfo SelectedFold
        {
            get => _selectedFold;
            set
            {
                if (_selectedFold != value)
                {
                    _selectedFold = value;
                    RaisePropertyChanged(nameof(SelectedFold));
                    JumpToFold();
                }
            }
        }
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
                Workspace.This.ActiveDocument.ScrollLine = (FoldInfo)p;
                //var scrollViewer = GetScrollViewer(AvalonEditInstance);
                //if (scrollViewer != null)
                //{
                //    // 设置滚动位置（这里示意设置为顶部，你可以根据需要进行调整）
                //    scrollViewer.ScrollToTop();
                //}
            }
            return true;
        }
        #endregion

        private void JumpToFold()
        {
            // 跳转到选中折叠的起始位置
            //if (SelectedFold != null)
            //{
            //    _textEdit.TextArea.Caret.Offset = SelectedFold.StartOffset;
            //    _textEdit.TextArea.Caret.BringCaretToView();
            //}
        }


    }
}
