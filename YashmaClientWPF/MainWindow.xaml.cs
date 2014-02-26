﻿using System;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;
        List<string> selected_node = new List<string>();
        bool ignor = false;
        string id = "0";
        List<TreeItem> tree_items;

        public MainWindow()
        {
            InitializeComponent();
            selected_node.Add("");
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            tree_items = Utility.SelectItems(selected_node[selected_node.Count - 1]);

            for (int i = 1; tree_items.Count == 1; i++)
            {
                tree_items = Utility.SelectItems(selected_node[selected_node.Count - i]);
            }

            Thread t1 = new Thread(new ThreadStart(Update));
            t1.Start();
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
            if (selected_node[selected_node.Count - 1].Length > 0)
            {
                timer.Interval = TimeSpan.FromMilliseconds(1000);
                timer.Start();
            }
        }

        /// <summary>
        /// Вызывает обновление рабочей области при выборе элемента из деревовидного меню
        /// </summary>
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selected_node.Add(((sender as TextBlock).Tag as XmlAttribute).Value);
            tree_items = Utility.SelectItems(selected_node[selected_node.Count - 1]);
            Thread t1 = new Thread(new ThreadStart(Update));
            t1.Start();
        }

        /// <summary>
        /// Выводит весь полученый список товаров, если в списке один элемент, выводит карточку товара.
        /// </summary>
        /// <param name="tree_items">Список товаров</param>
        private void Update()
        {
            Dispatcher.Invoke(new ThreadStart(delegate
            {
                if (tree_items.Count != 1)
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
                    ItemCardWindow icw = new ItemCardWindow(tree_items[0]);
                    icw.Show();
                }
                content.UpdateLayout();
            }));
        }

        /// <summary>
        /// Вызывает обновление рабочей области при плике по айтему товара
        /// </summary>
        void contentElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selected_node.Add((sender as ContentElement).path);
            tree_items = Utility.SelectItems(selected_node[selected_node.Count - 1]);
            Thread t1 = new Thread(new ThreadStart(Update));
            t1.Start();
        }

        #endregion
    }
}
