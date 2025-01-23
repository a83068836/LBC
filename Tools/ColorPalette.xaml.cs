using LBC.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextEditLib.Class;

namespace LBC.Tools
{
    /// <summary>
    /// ColorPalette.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPalette : UserControl
    {
        int blockWidth = 25;
        int rows = 16;
        int cols = 16;
        public static TRGBQuad[] ColorTable = new TRGBQuad[256];
        // -- properties
        public ColorPalette()
        {
            InitializeComponent();
            drawPalette();
        }
        void drawPalette()
        {
            int i = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Canvas canvas = new Canvas() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness() { Left = blockWidth * col+1, Top = blockWidth * row+1, Right = 0, Bottom = 0 } };
                    Rectangle rectangle = new Rectangle() { Width = blockWidth, Height = blockWidth, Fill = getColor(i), Stroke = getColor(i), StrokeThickness = 0 };
                    TextBlock text = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 12, TextAlignment = TextAlignment.Center, Width = blockWidth, Height = blockWidth, Text = i.ToString(), Margin = new Thickness() { Top=5} };
                    canvas.Children.Add(rectangle);
                    canvas.Children.Add(text);
                    canvas.MouseDown += (o, e) => { Console.WriteLine(i.ToString()); };
                    Grid1.Children.Add(canvas);
                    i++;
                }
            }
        }

        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("1");
        }

        Brush getColor(int i)
        {
            ColorTable[i].rgbRed = ColorHelper.ColorArray[i * 4 + 2];
            ColorTable[i].rgbGreen = ColorHelper.ColorArray[i * 4 + 1];
            ColorTable[i].rgbBlue = ColorHelper.ColorArray[i * 4];
            ColorTable[i].rgbReserved = ColorHelper.ColorArray[i * 4 + 3];
            return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, ColorTable[i].rgbRed, ColorTable[i].rgbGreen, ColorTable[i].rgbBlue));
        }
    }
}
