using AvalonDock;
using AvalonDock.Layout.Serialization;
using ICSharpCode.AvalonEdit;
using LBC.ViewModels;
using Lcode.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LBC.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainView
    {
        string layOutXml = System.IO.Path.Combine(Workspace.DirAppData, Workspace.LayoutFileName);
        public MainView()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Unloaded += new RoutedEventHandler(MainWindow_Unloaded);
            this.Closed += MainView_Closed;

        }

        private void MainView_Closed(object? sender, EventArgs e)
        {
            var serializer = new AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
            serializer.Serialize(layOutXml);
            Application.Current.Shutdown();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            NonClientAreaContent = new NonClientAreaContent();
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.LayoutSerializationCallback += (s, args) =>
            {
                if (args.Model.ContentId == ExplorerViewModel.ToolContentId)
                    args.Content = Workspace.This.Explorer;
                else if (args.Model.ContentId == PropertiesViewModel.ToolContentId)
                    args.Content = Workspace.This.Props;
                else if (args.Model.ContentId == ErrorViewModel.ToolContentId)
                    args.Content = Workspace.This.Errors;
                else if (args.Model.ContentId == OutputViewModel.ToolContentId)
                    args.Content = Workspace.This.Output;
                else if (args.Model.ContentId == GitChangesViewModel.ToolContentId)
                    args.Content = Workspace.This.Git;
                else if (args.Model.ContentId == ToolboxViewModel.ToolContentId)
                    args.Content = Workspace.This.Toolbox;
                else
                {
                    args.Content = Workspace.This.Open(args.Model.ContentId);

                    if (args.Content == null)
                        args.Cancel = true;
                }
            };

            if (File.Exists(layOutXml))
                layoutSerializer.Deserialize(layOutXml);
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            var serializer = new AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
            serializer.Serialize(layOutXml);
            Application.Current.Shutdown();
        }

        #region LoadLayoutCommand

        private RelayCommand _loadLayoutCommand = null;

        public ICommand LoadLayoutCommand
        {
            get
            {
                if (_loadLayoutCommand == null)
                {
                    _loadLayoutCommand = new RelayCommand((p) => OnLoadLayout(), (p) => CanLoadLayout());
                }

                return _loadLayoutCommand;
            }
        }

        private bool CanLoadLayout()
        {
            return File.Exists(layOutXml);
        }

        private void OnLoadLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);

            // Here I've implemented the LayoutSerializationCallback just to show
            //  a way to feed layout desarialization with content loaded at runtime
            // Actually I could in this case let AvalonDock to attach the contents
            // from current layout using the content ids
            // LayoutSerializationCallback should anyway be handled to attach contents
            // not currently loaded
            layoutSerializer.LayoutSerializationCallback += (s, e) =>
            {
                //if (e.Model.ContentId == FileStatsViewModel.ToolContentId)
                //    e.Content = Workspace.This.FileStats;
                //else if (!string.IsNullOrWhiteSpace(e.Model.ContentId) &&
                //    File.Exists(e.Model.ContentId))
                //    e.Content = Workspace.This.Open(e.Model.ContentId);
            };
            layoutSerializer.Deserialize(layOutXml);
        }

        #endregion LoadLayoutCommand

        #region SaveLayoutCommand

        private RelayCommand _saveLayoutCommand = null;

        public ICommand SaveLayoutCommand
        {
            get
            {
                if (_saveLayoutCommand == null)
                {
                    _saveLayoutCommand = new RelayCommand((p) => OnSaveLayout(), (p) => CanSaveLayout());
                }

                return _saveLayoutCommand;
            }
        }

        private bool CanSaveLayout()
        {
            return true;
        }

        private void OnSaveLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(dockManager);
            layoutSerializer.Serialize(layOutXml);
        }

        #endregion SaveLayoutCommand

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            var textEditor = FindTextEditorByName(dockManager, "textEditor1");
            if (textEditor != null)
            {
                textEditor.ScrollToEnd();
            }
        }
        private TextEditor FindTextEditorByName(DependencyObject parent, string name)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextEditor textEditor && textEditor.Name == name)
                {
                    return textEditor;
                }
                else
                {
                    var result = FindTextEditorByName(child, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
    
}
