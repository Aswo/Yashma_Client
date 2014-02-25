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
        string selected_node = "";
        string id = "0";

        Order order = new Order();

        public MainWindow()
        {
            InitializeComponent();
        }

        public XmlDataProvider DataProvider { get; set; }

        /// <summary>
        /// Загружает деревовидное меню
        /// </summary>
        private void LoadTreeMenu()
        {
            try
            {
                DataProvider = new XmlDataProvider();
                DataProvider.Source = new Uri(Environment.CurrentDirectory + "\\data\\tree_menu.xml");
                DataProvider.XPath = "/NodeList";
                this.tree_menu.DataContext = DataProvider;
            }
            catch
            {
                MessageBox.Show("Не найден файл tree_menu.xml");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTreeMenu();
            Utility.LoadItems();
        }

        #region Обновление рабочей области
        /// <summary>
        /// Вызывает обновление рабочей области при изменении размеров окна
        /// </summary>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (selected_node.Length > 0) Update(Utility.SelectItems(selected_node));
        }

        /// <summary>
        /// Вызывает обновление рабочей области при выборе элемента из деревовидного меню
        /// </summary>
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Update(Utility.SelectItems(selected_node = ((sender as TextBlock).Tag as XmlAttribute).Value));
        }

        /// <summary>
        /// Выводит весь полученый список товаров, если в списке один элемент, выводит карточку товара.
        /// </summary>
        /// <param name="tree_items">Список товаров</param>
        private void Update(List<TreeItem> tree_items)
        {
            content.Children.Clear();
            content.ColumnDefinitions.Clear();
            content.RowDefinitions.Clear();

            if (tree_items.Count != 1)
            {
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
                            contentElement.path = tree_items[m].Tag;
                            contentElement.article.Content = tree_items[m].Name;
                            contentElement.MouseUp += contentElement_MouseUp;
                            contentElement.icon.SetValue(Image.SourceProperty, imgs.ConvertFromString(Environment.CurrentDirectory + "\\data\\images\\" + tree_items[m++].Image + "_p.png"));

                            Grid.SetRow(contentElement, i);
                            Grid.SetColumn(contentElement, j);

                            content.Children.Add(contentElement);
                        }
                    }
                }
            }
            else
            {
                string[] temp;
                ContentElement contentElement = new ContentElement();
                ImageSourceConverter imgs = new ImageSourceConverter();

                ItemCard itemCard = new ItemCard();
                itemCard.name.Content = "Артикул: " + tree_items[0].Name;
                itemCard.weight.Content = "Вес: " + (temp = tree_items[0].Weight.Split('|'))[0] + " - " + temp[temp.Length - 1];
                itemCard.sample.Content = "Проба: " + tree_items[0].Sample;
                itemCard.zoom.MouseUp += zoom_MouseUp;
                itemCard.booking.Click += booking_Click;
                itemCard.order.Click += order_Click;
                itemCard.description.Content = tree_items[0].Description;
                itemCard.Margin = content.Margin;
                itemCard.icon.SetValue(Image.SourceProperty, imgs.ConvertFromString(Environment.CurrentDirectory + "\\data\\images\\" + (id = tree_items[0].Image) + "_p.png"));

                content.Children.Add(itemCard);
            }
            content.UpdateLayout();
        }

        /// <summary>
        /// Показывает корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void order_Click(object sender, RoutedEventArgs e)
        {

        }


        /// <summary>
        /// Добавляет позицию в товар
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void booking_Click(object sender, RoutedEventArgs e)
        {
            order.Add();
        }

        void zoom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ZoomImageWindow ziw = new ZoomImageWindow(id);
            ziw.Show();
        }

        /// <summary>
        /// Вызывает обновление рабочей области при плике по айтему товара
        /// </summary>
        void contentElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Update(Utility.SelectItems(selected_node = (sender as ContentElement).path));
        }

        #endregion
    }
}
