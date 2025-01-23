using ICSharpCode.AvalonEdit;
using LBC.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LBC.View
{
    /// <summary>
    /// CodeMain.xaml 的交互逻辑
    /// </summary>
    public partial class CodeMain : Page
    {
        Codes _codes;
        CodeViewModel codeView = new CodeViewModel();
        public CodeMain(Codes codes,string title)
        {
            //Workspace.This.OnNewCode(codeView);
            this.DataContext = codeView;
            InitializeComponent();
            _codes = codes;
            Title = title;
            if (Title != "代码预览")
            {
                textEditor.TextChanged += TextEditor_TextChanged;
            }
            
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                   (o) => _codes.ReBuildObjectExplorer()
               );
        }
    }
}
