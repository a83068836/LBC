﻿using LBC.ViewModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LBC.View.Patch
{
    /// <summary>
    /// Patch.xaml 的交互逻辑
    /// </summary>
    public partial class Patch
    {
        public static PatchViewModel patchViewModel= new PatchViewModel();
        public Patch()
        {
            InitializeComponent();
            this.DataContext = patchViewModel;
        }
    }
}
