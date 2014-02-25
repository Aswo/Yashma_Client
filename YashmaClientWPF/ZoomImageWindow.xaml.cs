using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YashmaClientWPF
{
    /// <summary>
    /// Логика взаимодействия для ZoomImageWindow.xaml
    /// </summary>
    public partial class ZoomImageWindow : Window
    {
        private Point origin;
        private Point start;
        private Matrix def;

        public ZoomImageWindow(string id)
        {
            InitializeComponent();
            ImageSourceConverter imgs = new ImageSourceConverter();
            image.SetValue(Image.SourceProperty, imgs.ConvertFromString(Environment.CurrentDirectory + "\\data\\images\\" + id + "_f.png"));

            using (FileStream fileStream = new FileStream(Environment.CurrentDirectory + "\\data\\images\\" + id + "_f.png", FileMode.Open, FileAccess.Read))
            {
                BitmapFrame frame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                Width = frame.PixelWidth;
                Height = frame.PixelHeight + 53 + 85;
            }
            def = image.RenderTransform.Value;
            WPFWindow.MouseWheel += MainWindow_MouseWheel;

            image.MouseLeftButtonDown += image_MouseLeftButtonDown;
            image.MouseLeftButtonUp += image_MouseLeftButtonUp;
            image.MouseMove += image_MouseMove;
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!image.IsMouseCaptured) return;
            Point p = e.MouseDevice.GetPosition(border);

            Matrix m = image.RenderTransform.Value;
            m.OffsetX = origin.X + (p.X - start.X);
            m.OffsetY = origin.Y + (p.Y - start.Y);

            image.RenderTransform = new MatrixTransform(m);
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (image.IsMouseCaptured) return;
            image.CaptureMouse();

            start = e.GetPosition(border);
            origin.X = image.RenderTransform.Value.OffsetX;
            origin.Y = image.RenderTransform.Value.OffsetY;
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(image);

            Matrix m = image.RenderTransform.Value;
            if (e.Delta > 0)
                m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
            else
                m.ScaleAtPrepend(1/1.1, 1/1.1, p.X, p.Y);

            image.RenderTransform = new MatrixTransform(m);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Matrix m = image.RenderTransform.Value;
            m.ScaleAtPrepend(1.1, 1.1,0, 0);
            image.RenderTransform = new MatrixTransform(m);
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Matrix m = image.RenderTransform.Value;
            m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, 0, 0);
            image.RenderTransform = new MatrixTransform(m);
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            image.RenderTransform = new MatrixTransform(def);
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
