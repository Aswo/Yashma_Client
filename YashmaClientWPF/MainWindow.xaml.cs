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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace YashmaClientWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public XmlDataProvider DataProvider { get; set; }

        private void LoadTreeMenu()
        {
            DataProvider = new XmlDataProvider();
            DataProvider.Source = new Uri(Environment.CurrentDirectory + "\\data\\tree_menu.xml");
            DataProvider.XPath = "/NodeList";
            this.tree_menu.DataContext = DataProvider;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTreeMenu();
            Utility.LoadItems();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //tree_menu.Width = Width / 100 * 25;
           // content_scroll.Width = Width / 100 * 75;
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            XmlAttribute xa = textBlock.Tag as XmlAttribute;
            Update(Utility.SelectItems(xa.Value));
        }

        private void Update(List<TreeItem> tree_items)
        {
            content.Children.Clear();
            content.ColumnDefinitions.Clear();
            content.RowDefinitions.Clear();

            int row_count = (int)Math.Ceiling(Width / 220) - 1;
            int col_count = (int)Math.Ceiling((double)tree_items.Count / row_count);

            for (int i = 0; i < row_count; i++) content.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < col_count; i++) content.RowDefinitions.Add(new RowDefinition());

            for (int i = 0, m = 0; i < col_count; i++)
            {
                for (int j = 0; j < row_count; j++)
                {
                    if (m < tree_items.Count)
                    {
                        ImageSourceConverter imgs = new ImageSourceConverter();
                        ContentElement contentElement = new ContentElement();
                        contentElement.article.Content = tree_items[m].Name;
                        contentElement.MouseUp += contentElement_MouseUp;
                        contentElement.icon.SetValue(Image.SourceProperty, imgs.ConvertFromString(Environment.CurrentDirectory + "\\data\\images\\" + tree_items[m++].Image + "_p.png"));

                        Grid.SetRow(contentElement, i);
                        Grid.SetColumn(contentElement, j);

                        content.Children.Add(contentElement);
                    }
                }
            }
            content.UpdateLayout();
        }

        void contentElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("1");
        }
    }
}
