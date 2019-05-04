using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void GotFoc(object sender, RoutedEventArgs e)
        {
            TextBox obj = sender as TextBox;

            if (obj != null)
            {
                if (obj.Text == Properties.Resources.Login || string.IsNullOrWhiteSpace(obj.Text))
                {
                    obj.Text = "";
                }
            }
            else
            {
                if ((sender as PasswordBox).Password == Properties.Resources.Password || string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                {
                    (sender as PasswordBox).Password = "";
                }
            }
        }

        private void LostFoc(object sender, RoutedEventArgs e)
        {
            TextBox obj = (sender as TextBox);

            if (obj != null)
            {
                if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                {
                    (sender as TextBox).Text = Properties.Resources.Login;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                {
                    (sender as PasswordBox).Password = Properties.Resources.Password;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Login.Text) && !string.IsNullOrWhiteSpace(Password.Password) && Login.Text != Properties.Resources.Login && Password.Password != Properties.Resources.Password) {
                Person person = new Person();

                Error_Sing_In.Visibility = Visibility.Collapsed;

                person.login = Login.Text;
                person.password = Password.Password;

                string json = Network.Serialize<Person>(person);
                HttpContent content = new StringContent(json);

                try
                {
                    var answer = await Network.Response(content, "application/json", Properties.Settings.Default.URI + "api/authorize", HttpMethod.Post);
                    if (answer.answer != null)
                    {
                        Properties.Settings.Default.Person = answer.answer;
                        Properties.Settings.Default.Save();

                        if (!Remember.IsChecked.Value) { Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler((sender1, e1) => {
                            Properties.Settings.Default.Person = "";
                            Properties.Settings.Default.Save();
                        }); }
                        
                        NavigationService.Navigate(new Cabinet());
                    }
                    else
                    {
                        if (answer.response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            Error_Sing_In.Text = Properties.Resources.ErrorEnterance;
                            Error_Sing_In.Visibility = Visibility.Visible;
                        }
                    }
                } catch (Exception exc)
                {
                    Error_Sing_In.Text = Properties.Resources.ConnectionError;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Register reg = new Register();
            if (reg.ShowDialog() == true)
            {
                Person registred = Network.Deserialize<Person>(Application.Current.Properties["correct_registration"].ToString());

                Login.Text = registred.login;
                Password.Password = registred.password;
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LostPass lp = new LostPass();
            lp.ShowDialog();
        }

        private void Init(object sender, EventArgs e)
        {
            TextBox obj = (sender as TextBox);

            if (obj != null)
            {
                obj.Text = Properties.Resources.Login;
            } else
            {
                (sender as PasswordBox).Password = Properties.Resources.Password;
            }
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Person != "")
            {
                Login.Text = Network.Deserialize<Person>(Properties.Settings.Default.Person).email;
                Password.Password = Network.Deserialize<Person>(Properties.Settings.Default.Person).password;
            }
        }
    }
}
