using LBC.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace LBC.Views
{
	/// <summary>
	/// Sql.xaml 的交互逻辑
	/// </summary>
	public partial class Sql
	{
        private SqlViewModel viewModel;
        public Sql()
		{
			InitializeComponent();
            if (viewModel == null)
            {
                viewModel = new SqlViewModel();
                this.DataContext = viewModel;
            }
        }
    }
}
