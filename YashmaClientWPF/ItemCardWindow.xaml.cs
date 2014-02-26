using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace YashmaClientWPF
{
    /// <summary>
    /// Логика взаимодействия для ZoomImageWindow.xaml
    /// </summary>
    public partial class ItemCardWindow : Window
    {
        private Point origin;
        private Point start;
        private Matrix def;

        public ItemCardWindow(TreeItem tree_item)
        {
            InitializeComponent();

            string[] temp;
            ImageSourceConverter imgs = new ImageSourceConverter();
            name.Content = "Артикул: " + tree_item.Name;
            weight.Content = "Вес: " + (temp = tree_item.Weight.Split('|'))[0] + " - " + temp[temp.Length - 1];
            sample.Content = "Проба: " + tree_item.Sample;
            description.Content = tree_item.Description;

            image.SetValue(Image.SourceProperty, imgs.ConvertFromString(Environment.CurrentDirectory + "\\data\\images\\" + tree_item.Image + "_f.png"));

            size_list.Children.Clear();
            size_list.ColumnDefinitions.Clear();
            size_list.RowDefinitions.Clear();
            
            string[] sizes = tree_item.Size.Split('|');
            string[] weights = tree_item.Weight.Split('|');

            int row_count = 5;
            int col_count = (int)Math.Ceiling((double)sizes.Length / row_count);

            for (int i = 0; i < row_count; i++) size_list.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < col_count; i++) size_list.RowDefinitions.Add(new RowDefinition());

            for (int i = 0, m = 0; i < col_count; i++)
            {
                for (int j = 0; j < row_count; j++)
                {
                    if (m < sizes.Length)
                    {
                        SizeGridItem sgi = new SizeGridItem();
                        sgi.info.Content = sizes[m] + " - " + weights[m++] + " гр.";

                        Grid.SetRow(sgi, i);
                        Grid.SetColumn(sgi, j);

                        size_list.Children.Add(sgi);
                    }
                }
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
