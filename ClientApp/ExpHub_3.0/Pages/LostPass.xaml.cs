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
using System.Windows.Shapes;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для LostPass.xaml
    /// </summary>
    public partial class LostPass : Window
    {
        public LostPass()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "Email" || string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                (sender as TextBox).Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                (sender as TextBox).Text = "Email";
            }
        }

        private void TextBox_Initialized(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "Email";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
