using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TextEditLib.Class;
using UnitComboLib.Command;

namespace LBC.ViewModels
{
    internal class NavigationViewModel: ViewModelBase
    {
        private TextEditor textEditor;
        private bool isbutton=false;

        public ICommand BackCommand { get; }
        public ICommand ForwardCommand { get; }
        public ICommand DownCommand => new RelayCommand<HistoryItem>(DownExCommand);
        private ObservableCollection<HistoryItem> _historyItems;

        public NavigationViewModel(TextEditor textEditor)
        {
            _historyItems= new ObservableCollection<HistoryItem>();
            this.textEditor = textEditor;
            BackCommand = new RelayCommand(OnBack, CanBack);
            ForwardCommand = new RelayCommand(OnForward, CanForward);
        }
        public ObservableCollection<HistoryItem> HistoryItems
        {
            get => _historyItems;
            set
            {
                if (_historyItems != value)
                {
                    _historyItems = value;
                    RaisePropertyChanged(nameof(HistoryItems));
                    UpdateSortedHistoryItems();
                }
            }
        }
        private void DownExCommand(HistoryItem parameter)
        {
            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (parameter == item)
                {
                    isbutton = true;
                    item.IsSelected = true;
                    _sortedHistoryItems?.View.Refresh();
                    Workspace.This.SetCaretPosition(item.Tag, item.position);
                    isbutton = false;
                    break;
                }
                else
                {
                    item.IsSelected = false;
                }
                isbutton = false;
            }
            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (parameter != item)
                {
                    item.IsSelected= false;
                }
            }
        }
        private void UpdateSortedHistoryItems()
        {
            _sortedHistoryItems = new CollectionViewSource { Source = HistoryItems };
            _sortedHistoryItems.SortDescriptions.Add(new SortDescription("LastUpdated", ListSortDirection.Descending));
            RaisePropertyChanged(nameof(SortedHistoryItems));
        }
        public void AddHistoryItem(HistoryItem historyItem)
        {
            if (isbutton)
            {
                isbutton = false;
                return;
            }
            if (historyItem == null)
                return;
            if (HistoryItems.Count == 0 || historyItem.Line != HistoryItems.Last().Line)
            {
                historyItem.DisplayText = $"{historyItem.Filename}：{historyItem.Text[..Math.Min(100, historyItem.Text.Length)]}";
                historyItem.LastUpdated= DateTime.Now;
                historyItem.IsSelected = true;
                HistoryItems.Add(historyItem);
                if (HistoryItems.Count > 15)
                {
                    var oldestItem = HistoryItems.OrderBy(item => item.LastUpdated).First();
                    HistoryItems.Remove(oldestItem);
                }
            }
            else
            {
                historyItem.DisplayText = $"{historyItem.Filename}：{historyItem.Text[..Math.Min(100, historyItem.Text.Length)]}";
                HistoryItems[HistoryItems.Count - 1] = historyItem;
                HistoryItems[HistoryItems.Count - 1].LastUpdated = DateTime.Now;
            }
            foreach (var item in HistoryItems)
            {
                if (item == historyItem)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
            _sortedHistoryItems?.View.Refresh();
        }
        private CollectionViewSource _sortedHistoryItems;

        public ICollectionView SortedHistoryItems
        {
            get
            {
                if (_sortedHistoryItems == null)
                {
                    UpdateSortedHistoryItems();
                }
                return _sortedHistoryItems.View;
            }
        }

        private void OnBack(object parameter)
        {
            isbutton = true;
            bool b = false;
            int a = 1;
            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (b)
                {
                    item.IsSelected = true;
                    _sortedHistoryItems?.View.Refresh();
                    Workspace.This.SetCaretPosition(item.Tag, item.position);
                    isbutton = false;
                    break;
                }
                else
                {
                    if (item.IsSelected)
                    {
                        if (HistoryItems.Count - a > 0)
                        {
                            item.IsSelected = false;
                            b = true;
                        }
                    }
                        
                }
               a++;
            }
            isbutton = false;
        }

        public bool CanBack(object parameter)
        {
            int i = 1;
            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (item.IsSelected && i == HistoryItems.Count)
                {
                    return false;
                }
                ++i;
            }
            return true;
        }

        private void OnForward(object parameter)
        {
            isbutton = true;
            bool b = false;
            int a = 1;
            HistoryItem previousItem = null;

            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (b)
                {
                    previousItem.IsSelected = true;
                    _sortedHistoryItems?.View.Refresh();
                    Workspace.This.SetCaretPosition(previousItem.Tag, previousItem.position);
                    isbutton = false;
                    break;
                }
                else
                {
                    if (item.IsSelected)
                    {
                        if (a > 1) // 确保有前一条数据
                        {
                            previousItem.IsSelected = true;
                            item.IsSelected = false;
                            _sortedHistoryItems?.View.Refresh();
                            Workspace.This.SetCaretPosition(previousItem.Tag, previousItem.position);
                        }
                        else
                        {
                            Workspace.This.SetCaretPosition(previousItem.Tag, previousItem.position);
                        }
                        isbutton = false;
                        break;
                    }
                }
                previousItem = item;
                a++;
            }
            isbutton = false;
        }

        private bool CanForward(object parameter)
        {
            int i = 0;
            foreach (HistoryItem item in SortedHistoryItems)
            {
                if (item.IsSelected && i == 0)
                {
                    return false;
                }
                ++i;
            }
            return true;
        }
    }
}
