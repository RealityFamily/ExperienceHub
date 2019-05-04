using ExperenceHubApp;
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

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для Cabinet.xaml
    /// </summary>
    public partial class Cabinet : Page
    {
        public Cabinet()
        {
            InitializeComponent();

            if(Properties.Settings.Default.Person == string.Empty)
            {
                Greetings.Text = Properties.Resources.AccountError;
            }
            else
            {
                Greetings.Text = "Добро пожаловать, " + Network.Deserialize<Person>(Properties.Settings.Default.Person).firstname + " " + Network.Deserialize<Person>(Properties.Settings.Default.Person).lastname;
            }
        }
    }
}
