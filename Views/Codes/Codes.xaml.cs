using System;
using System.Collections.Generic;
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
using System.Threading;
using System.Collections;
using System.Text.RegularExpressions;
using AvalonDock;
using AvalonDock.Layout;

namespace LBC.View
{
    /// <summary>
    /// Codes.xaml 的交互逻辑
    /// </summary>
    public partial class Codes
    {
        int number = 2;
        public Codes()
        {
            InitializeComponent();
        }

        private void DockManager_Loaded(object sender, RoutedEventArgs e)
        {
            CodeMain codeMain = new CodeMain(this, "代码预览");
            codeLeft.NavigationService.Navigate(codeMain);
            CreateTab(DocumentPane, "代码编辑", "代码编辑");
            CreateTab(DocumentPane, "{1}", "{1}");
            Button button = new Button();
            button.ToolTip = "{1}";
            button.Content = "{1}";
            button.Foreground= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2d51"));
            button.Click += Button_Click;
            toolbar1.Items.Add(button);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            foreach (var content in DocumentPane.Children)
            {
                if (content.Title == "代码编辑")
                {
                    Frame? frame1 = content.Content as Frame;
                    CodeMain _codeMain = frame1.Content as CodeMain;
                    if (_codeMain != null)
                    {
                        //_codeMain.textEditor.Document.Insert(_codeMain.textEditor.SelectionStart, button.Content.ToString());
                        _codeMain.textEditor.Document.Replace(_codeMain.textEditor.SelectionStart, _codeMain.textEditor.SelectionLength, button.Content.ToString());
                    }
                    break;
                }
            }
        }
        static ImageSourceConverter ISC = new ImageSourceConverter();
        public void CreateTab(LayoutDocumentPane layoutDocumentPane, string name, object tag)
        {
            try {
                Frame frame = new Frame();
                LayoutAnchorable layOutAnc = new LayoutAnchorable()
                {
                    Title = name,
                    ToolTip = tag,
                    CanClose = false,
                    CanHide = false,
                    CanAutoHide = false,
                    ContentId = tag.ToString(),
                    IsSelected = true,
                    IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/LBC;component/Themes/document.png") as ImageSource
                };
                CodeMain codeMain = new CodeMain(this, name);
                layOutAnc.Content = frame;
                layOutAnc.IsSelected = true;
                //layOutAnc.IsActive = true;
                frame.NavigationService.Navigate(codeMain);
                layoutDocumentPane.Children.Add(layOutAnc);
            }
            catch (Exception ex)
            {
            
            }
           
        }

        private void menu01_Click(object sender, RoutedEventArgs e)
        {
            CreateTab(DocumentPane, "{"+ number.ToString() + "}", "{" + number.ToString() + "}");
            Button button = new Button();
            button.ToolTip = "{" + number.ToString() + "}";
            button.Content = "{" + number.ToString() + "}";
            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2d51"));
            button.Click += Button_Click;
            toolbar1.Items.Add(button);
            number++;
        }
        public void DetLeft(string text)
        {
            CodeMain _codeMain = codeLeft.Content as CodeMain;
            if (_codeMain != null)
            {
                _codeMain.textEditor.Text = text;
            }
        }
        //1.循环list中所有的元素然后删除
        public static ArrayList removeDuplicate_1(ArrayList arrayList)
        {
            for (int i = 0; i < arrayList.Count; i++)
            {
                for (int j = i + 1; j < arrayList.Count; j++)
                {
                    if (arrayList[i].Equals(arrayList[j]))
                    {
                        arrayList.RemoveAt(j);
                        if (i > 0)
                        {
                            i--;
                        }
                    }
                }
            }

            return arrayList;
        }
        public void ReBuildObjectExplorer()
        {
            this.Dispatcher.Invoke(new Action(delegate {
                try
                {
                    string text = string.Empty;
                    foreach (var content in DocumentPane.Children)
                    {
                        if (content.Title == "代码编辑")
                        {
                            Frame frame1 = content.Content as Frame;
                            CodeMain _codeMain = frame1.Content as CodeMain;
                            if (_codeMain != null)
                            {
                                text = _codeMain.textEditor.Text;
                            }
                            break;
                        }
                    }
                    if (text == "")
                    {
                        DetLeft("");
                    }
                    string returntext = "";
                    string regexCode = @"\{(.)\}|\{(._ADD)\}";
                    Regex reg = new Regex(regexCode);
                    MatchCollection mc = reg.Matches(text);
                    ArrayList A = new ArrayList();
                    foreach (var item in mc)
                    {
                        A.Add(item.ToString());
                    }
                    A = removeDuplicate_1(A);
                    string Aid = "";
                    foreach (var content in DocumentPane.Children)
                    {
                        if (content.Title == "{1}")
                        {
                            Frame frame1 = content.Content as Frame;
                            CodeMain _codeMain = frame1.Content as CodeMain;
                            if (_codeMain != null)
                            {
                                Aid = _codeMain.textEditor.Text;
                            }
                        }
                    }
                    string[] B = Regex.Split(Aid, "\r\n", RegexOptions.IgnoreCase);
                    returntext += ";{\r\n";
                    for (int i = 0; i < B.Length; i++)
                    {
                        string LinA = text;
                        for (int j = 0; j < A.Count; j++)
                        {
                            string suibian = A[j].ToString();
                            if (suibian.IndexOf("_ADD") > 0)
                            {
                                suibian = suibian.Replace("_ADD", "");
                            }

                            foreach (var content in DocumentPane.Children)
                            {
                                if (content.Title == suibian)
                                {
                                    Frame frame1 = content.Content as Frame;
                                    CodeMain _codeMain = frame1.Content as CodeMain;
                                    if (_codeMain != null)
                                    {
                                        string Bid = _codeMain.textEditor.Text;
                                        string[] B1 = Regex.Split(Bid, "\r\n", RegexOptions.IgnoreCase);
                                        if (A[j].ToString().IndexOf("_ADD") > 0)
                                        {
                                            LinA = LinA.Replace(A[j].ToString(), B1.Length > 0 ? (int.Parse(B1[0].ToString()) + i).ToString() : A[j].ToString());
                                        }
                                        else
                                        {
                                            LinA = LinA.Replace(suibian, B1.Length > i ? B1[i].ToString() : suibian);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        returntext += LinA + "\r\n";
                    }
                    returntext += "\r\n;}";
                    DetLeft(returntext);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }));
        }
    }
}
