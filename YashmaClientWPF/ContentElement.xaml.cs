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
    /// Логика взаимодействия для ContentElement.xaml
    /// </summary>
    public partial class ContentElement : UserControl
    {
        public ContentElement()
        {
            InitializeComponent();
        }
        public string path { get; set; }
    }
}
