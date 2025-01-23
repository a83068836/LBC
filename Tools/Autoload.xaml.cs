using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.Windows.Shell;
using System.IO;

namespace LBC.Tools
{
    /// <summary>
    /// Autoload.xaml 的交互逻辑
    /// </summary>
    public partial class Autoload
    {
        public Autoload()
        {
            InitializeComponent();
            this.Loaded += Add_Loaded;
        }
        private void Add_Loaded(object sender, RoutedEventArgs e)
        {
            iTabControl.Items.Clear();
            if (File.Exists(System.Windows.Forms.Application.StartupPath + @"Settings\Automatic.dll"))
            {
                string str = File.ReadAllText(System.Windows.Forms.Application.StartupPath + @"Settings\Automatic.dll");
                JArray json = (JArray)JsonConvert.DeserializeObject(str);
                for (int i = 0; i < json.Count; i++)
                {
                    TabItem tabItem = new TabItem() { Header = json[i]["name"].ToString() };
                    WrapPanel wrapPanel = new WrapPanel() { };
                    tabItem.Content = wrapPanel;
                    foreach (var item in json[i]["list"])
                    {
                        CheckBox checkBox = new CheckBox() { Content = item["name"].ToString(), Margin = new Thickness(10), IsChecked = (bool)item["ischecked"], ToolTip = item["id"].ToString() + "," + item["item1"].ToString() + "," + item["item2"].ToString() + "," + item["item3"].ToString() };
                        //checkBox.Style = (Style)FindResource("MaterialDesignCheckBox");
                        checkBox.Name = "S"+json[i]["name"].ToString() + item["id"].ToString();
                        checkBox.Click += CheckBox_Click;
                        checkBox.MouseDown += CheckBox_MouseDown;
                        ContextMenu checkboxContextMenu = (ContextMenu)FindResource("CheckBoxContextMenu");
                        checkBox.ContextMenu = checkboxContextMenu;
                        wrapPanel.Children.Add(checkBox);
                    }
                    iTabControl.Items.Add(tabItem);
                }
            }
        }

        private void CheckBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var s = (CheckBox)sender;
            selectMyClassleixing.name = s.Content.ToString();
            string[] strings = s.ToolTip.ToString().Split('\u002C');
            if (strings.Length > 3)
            {
                selectMyClassleixing.id = int.Parse(strings[0]);
                selectMyClassleixing.item1 = int.Parse(strings[1]);
                selectMyClassleixing.item2 = int.Parse(strings[2]);
                selectMyClassleixing.item3 = int.Parse(strings[3]);
            }
            controlname = s.Name;
        }
        string controlname = string.Empty;
        MyClassleixing selectMyClassleixing = new MyClassleixing();
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            List<MyClassyinqing> list = new List<MyClassyinqing>();
            for (int i = 0; i < iTabControl.Items.Count; i++)
            {
                TabItem tabItem = (TabItem)iTabControl.Items[i];
                WrapPanel wrapPanel = (WrapPanel)tabItem.Content;
                List<MyClassleixing> list1 = new List<MyClassleixing>();
                for (int j = 0; j < wrapPanel.Children.Count; j++)
                {
                    CheckBox checkBox = (CheckBox)wrapPanel.Children[j];
                    string[] strings = checkBox.ToolTip.ToString().Split('\u002C');
                    if (strings.Length > 3)
                    {
                        list1.Add(new MyClassleixing() { id = j, name = checkBox.Content.ToString(), ischecked = checkBox.IsChecked.Value, item1 = int.Parse(strings[1]), item2 = int.Parse(strings[2]), item3 = int.Parse(strings[3]) });
                    }
                }
                list.Add(new MyClassyinqing() { id = i, name = tabItem.Header.ToString(), list = list1 });
            }

            string resultJson = JsonConvert.SerializeObject(list);
            File.WriteAllText(System.Windows.Forms.Application.StartupPath + @"Settings\Automatic.dll", resultJson);
        }
        public class MyClassyinqing
        {
            public int id { get; set; }
            public string name { get; set; }
            public List<MyClassleixing> list { get; set; }
        }

        public class MyClassleixing
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool ischecked { get; set; }
            public int item1 { get; set; }
            public int item2 { get; set; }
            public int item3 { get; set; }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditAutoload editAutoload = new EditAutoload();
            // 传递参数到编辑窗口
            editAutoload.Id = selectMyClassleixing.id;
            editAutoload.Name = selectMyClassleixing.name;
            editAutoload.Item1 = selectMyClassleixing.item1.ToString();
            editAutoload.Item2 = selectMyClassleixing.item2.ToString();
            editAutoload.Item3 = selectMyClassleixing.item3.ToString();
            // 显示编辑窗口并等待窗口关闭
            bool? result = editAutoload.ShowDialog();

            // 检查是否保存了修改
            if (result == true)
            {
                CheckBox checkBox = FindVisualChild<CheckBox>(this, controlname);
                if (checkBox != null)
                {
                    checkBox.Content = editAutoload.Name;
                    checkBox.ToolTip = editAutoload.Id.ToString() + "," + editAutoload.Item1 + "," + editAutoload.Item2 + "," + editAutoload.Item3;
                    CheckBox_Click(null, null);
                }
            }
        }
        private T FindVisualChild<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            if (parent == null)
                return null;

            T child = null;
            int numChildren = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < numChildren; i++)
            {
                var visualChild = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;

                if (visualChild != null && visualChild.Name == name && visualChild is T)
                {
                    child = (T)visualChild;
                    break;
                }

                child = FindVisualChild<T>(visualChild, name);

                if (child != null)
                    break;
            }

            return child;
        }
    }
}
