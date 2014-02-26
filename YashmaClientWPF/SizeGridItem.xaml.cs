using System;
using System.Collections.Generic;
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

namespace YashmaClientWPF
{
    /// <summary>
    /// Логика взаимодействия для SizeGridItem.xaml
    /// </summary>
    public partial class SizeGridItem : UserControl
    {
        public int index { get; set; }
        public SizeGridItem()
        {
            InitializeComponent();
            index_view.Text = (index = 0).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            index_view.Text = (++index).ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            index_view.Text = --index < 0 ? "0" : index.ToString();
        }

        private void index_view_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void index_view_LostFocus(object sender, RoutedEventArgs e)
        {
            int ind = 0;
            index_view.Text = int.TryParse(index_view.Text, out ind) ? (index = Convert.ToInt32(index_view.Text)) < 0 ? "0" : index.ToString() : index_view.Text = index.ToString();
        }
    }
}
