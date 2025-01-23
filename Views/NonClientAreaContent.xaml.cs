//using Lcode.Tools;
using LBC.Tools;
using LBC.View;
using LBC.View.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Lcode.Views
{
    /// <summary>
    /// NonClientAreaContent.xaml 的交互逻辑
    /// </summary>
    public partial class NonClientAreaContent
    {
        public NonClientAreaContent()
        {
            InitializeComponent();
        }
        private void autoloadclick(object sender, RoutedEventArgs e)
		{
			LBC.Tools.Autoload autoload = new LBC.Tools.Autoload();
			autoload.ShowDialog();
		}

		private void itemsclick(object sender, RoutedEventArgs e)
		{
			//Lcode.Tools.Items items = new Lcode.Tools.Items();
			//items.ShowDialog();
		}

		private void cdkclick(object sender, RoutedEventArgs e)
		{
			//CDK cDK = new CDK();
			//cDK.Show();
		}

		private void codeclick(object sender, RoutedEventArgs e)
		{
            Codes codes = new Codes();
			codes.Show();
		}

		private void Batchreplaceclick(object sender, RoutedEventArgs e)
		{
			//Batchreplace batchreplace = new Batchreplace();
			//batchreplace.Show();
		}

		private void Visualizationclick(object sender, RoutedEventArgs e)
		{
			//Visualization visualization = new Visualization();
			//visualization.ShowDialog();
		}

		private void gitclick(object sender, RoutedEventArgs e)
		{
			Git git = new Git();
			git.ShowDialog();
		}

        private void patchclick(object sender, RoutedEventArgs e)
        {
			Patch patch = new Patch();
			patch.Show();
        }
    }
}
