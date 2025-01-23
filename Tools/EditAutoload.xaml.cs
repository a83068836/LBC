using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LBC.Tools
{
    /// <summary>
    /// EditAutoload.xaml 的交互逻辑
    /// </summary>
    public partial class EditAutoload
    {
        public EditAutoload()
        {
            InitializeComponent();
            
        }
        // 定义私有字段
        private int _id;
        private string _name;
        private string _item1;
        private string _item2;
        private string _item3;

        // 公共属性定义及控件更新逻辑
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                idTextBox.Text = value.ToString();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                nameTextBox.Text = value;
                this.Title = Name;
            }
        }

        public string Item1
        {
            get { return _item1; }
            set
            {
                _item1 = value;
                item1TextBox.Text = value;
            }
        }

        public string Item2
        {
            get { return _item2; }
            set
            {
                _item2 = value;
                item2TextBox.Text = value;
            }
        }

        public string Item3
        {
            get { return _item3; }
            set
            {
                _item3 = value;
                item3TextBox.Text = value;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            { 
                Name=nameTextBox.Text;
                Item1 = item1TextBox.Text;
                Item2 = item2TextBox.Text;
                Item3 = item3TextBox.Text;
                this.DialogResult = true;
                this.Close();
            }
        }
        private bool ValidateInputs()
        {
            // 验证 nameTextBox.Text 是否为空
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                MessageBox.Show("Name不能为空");
                return false;
            }

            // 验证 item1TextBox.Text 是否为空
            if (string.IsNullOrWhiteSpace(item1TextBox.Text))
            {
                MessageBox.Show("item1不能为空");
                return false;
            }

            // 验证 item2TextBox.Text 是否为空
            if (string.IsNullOrWhiteSpace(item2TextBox.Text))
            {
                MessageBox.Show("item2不能为空");
                return false;
            }

            // 验证 item3TextBox.Text 是否为空
            if (string.IsNullOrWhiteSpace(item3TextBox.Text))
            {
                MessageBox.Show("item3不能为空");
                return false;
            }

            // 验证 item1TextBox.Text 是否为数字
            if (!int.TryParse(item1TextBox.Text, out _))
            {
                MessageBox.Show("item1必须为数字");
                return false;
            }

            // 验证 item2TextBox.Text 是否为数字
            if (!int.TryParse(item2TextBox.Text, out _))
            {
                MessageBox.Show("item2必须为数字");
                return false;
            }

            // 验证 item3TextBox.Text 是否为数字
            if (!int.TryParse(item3TextBox.Text, out _))
            {
                MessageBox.Show("item3必须为数字");
                return false;
            }

            // 所有验证都通过
            return true;
        }

    }
}
