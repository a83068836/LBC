using LBC.Class;
using LBC.ViewModel;
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
using System.Xml.Linq;
using Masuit.Tools;

namespace LBC.View.Patch
{
    /// <summary>
    /// PatchAdd.xaml 的交互逻辑
    /// </summary>
    public partial class PatchAdd
    {
       
        int _id;
        int _index;
        public PatchAdd(int id, PatchModel patchModel)
        {
            InitializeComponent();
            _id = id;
            if (id == 1)
            {
                this.Title = "增加";
                A6.SelectedIndex = 2;
            }
            else if (id == 2)
            {
                this.Title = "复制增加";
                A1.Text = patchModel.YinqingPath;
                A2.Text = patchModel.DengluqiPath;
                A3.Text = patchModel.BudingPath;
                A4.Text = patchModel.Pakmima;
                A5.Text = patchModel.Name;
                A6.SelectedValue = patchModel.Type;
                A7.Text = patchModel.PatchA1;
                A8.Text = patchModel.PatchA2;
                A9.Text = patchModel.PatchA3;
                //A10.Text = patchModel.PatchA4;
                A11.Text = patchModel.PatchA5;
                A12.Text = patchModel.PatchA6;
                _index = patchModel.Id;
            }
            else if (id == 3)
            {
                this.Title = "修改";
                A1.Text = patchModel.YinqingPath;
                A2.Text = patchModel.DengluqiPath;
                A3.Text = patchModel.BudingPath;
                A4.Text = patchModel.Pakmima;
                A5.Text = patchModel.Name;
                A6.SelectedValue = patchModel.Type;
                A7.Text = patchModel.PatchA1;
                A8.Text = patchModel.PatchA2;
                A9.Text = patchModel.PatchA3;
                //A10.Text = patchModel.PatchA4;
                A11.Text = patchModel.PatchA5;
                A12.Text = patchModel.PatchA6;
                _index = patchModel.Id;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_id == 1 || _id == 2)
            {
                Patch.patchViewModel.Add(new PatchModel()
                {
                    YinqingPath = A1.Text,
                    DengluqiPath = A2.Text,
                    BudingPath = A3.Text,
                    Pakmima = A4.Text,
                    Name = A5.Text,
                    Type = (Data.DemoType)A6.SelectedValue,
                    PatchA1 = A7.Text,
                    PatchA2 = A8.Text,
                    PatchA3 = A9.Text,
                    //PatchA4 = A10.Text,
                    PatchA5 = A11.Text,
                    PatchA6 = A12.Text
                });
            }
            else if(_id==3)
            {
                Patch.patchViewModel.Edit(new PatchModel()
                {
                    Id = _index,
                    YinqingPath = A1.Text,
                    DengluqiPath = A2.Text,
                    BudingPath = A3.Text,
                    Pakmima = A4.Text,
                    Name = A5.Text,
                    Type = (Data.DemoType)A6.SelectedValue,
                    PatchA1 = A7.Text,
                    PatchA2 = A8.Text,
                    PatchA3 = A9.Text,
                    //PatchA4 = A10.Text,
                    PatchA5 = A11.Text,
                    PatchA6 = A12.Text
                });
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            string result = FileHelpers.OpenFolderBrowserDialogA(s.Name);
            if (result != "")
            {
                var findName = (TextBox)FindName("A" + s.Name.Substring(1));
                if (findName != null)
                {
                    findName.Text = result;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            string result = FileHelpers.OpenFolderBrowserDialogA(s.Name);
            if (result != "")
            {
                var findName = (TextBox)FindName("A" + s.Name.Substring(1));
                if (findName != null)
                {
                    findName.Text = result;
                }
            }
        }
    }
}
