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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LBC.Controls
{
    public class ImageSequencePlayer : Control
    {
        private int currentFrameIndex;
        private DispatcherTimer timer;

        static ImageSequencePlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSequencePlayer), new FrameworkPropertyMetadata(typeof(ImageSequencePlayer)));
        }

        public ImageSequencePlayer()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public static readonly DependencyProperty FramesProperty =
            DependencyProperty.Register("Frames", typeof(List<BitmapImage>), typeof(ImageSequencePlayer), new PropertyMetadata(null, OnFramesChanged));

        public List<BitmapImage> Frames
        {
            get { return (List<BitmapImage>)GetValue(FramesProperty); }
            set { SetValue(FramesProperty, value); }
        }

        private static void OnFramesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var player = d as ImageSequencePlayer;
            if (player != null)
            {
                player.currentFrameIndex = 0;
            }
        }

        public BitmapImage CurrentFrame
        {
            get
            {
                if (Frames == null || Frames.Count == 0)
                    return null;
                return Frames[currentFrameIndex];
            }
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (Frames == null || Frames.Count == 0)
                return;
            currentFrameIndex = (currentFrameIndex + 1) % Frames.Count;
            InvalidateVisual();
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (CurrentFrame != null)
            {
                drawingContext.DrawImage(CurrentFrame, new Rect(0, 0, ActualWidth, ActualHeight));
            }
        }
        public void StartPlayback()
        {
            timer.Start();
        }

        public void StopPlayback()
        {
            timer.Stop();
        }
    }
}
