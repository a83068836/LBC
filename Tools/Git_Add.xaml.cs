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
    /// Git_Add.xaml 的交互逻辑
    /// </summary>
    public partial class Git_Add
    {
        public string Title { get; set; }
        public Git_Add()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Title = this.title.Text;
            DialogResult = true;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string newText = textBox.Text + e.Text;

            // 使用正则表达式检查输入是否符合要求
            if (!Regex.IsMatch(newText, @"^[a-z0-9-]{0,21}$"))
            {
                e.Handled = true; // 阻止输入
                MessageBox.Show("只能输入小写字母、数字和-，长度为 21 个字符");
            }
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string cleanedText = Regex.Replace(textBox.Text, @"[^a-z0-9-]", ""); // 清理非法字符
            if (cleanedText.Length > 21)
            {
                cleanedText = cleanedText.Substring(0, 21); // 截取前 21 个字符
            }
            textBox.Text = cleanedText; // 更新 TextBox 的文本
            textBox.CaretIndex = cleanedText.Length; // 将光标移动到文本末尾
        }
    }
}
