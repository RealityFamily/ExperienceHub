using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public bool correct = false;

        public Register()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            switch ((sender as TextBox).Name)
            {
                case "Name":
                    if ((sender as TextBox).Text == Properties.Resources.FirstName || string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = "";
                    }
                    break;

                case "LastName":
                    if ((sender as TextBox).Text == Properties.Resources.LastName || string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = "";
                    }
                    break;

                case "Email":
                    if ((sender as TextBox).Text == "Email" || string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = "";
                    }
                    break;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            switch ((sender as TextBox).Name)
            {
                case "Name":
                    if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = Properties.Resources.FirstName;
                    }
                    break;

                case "LastName":
                    if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = Properties.Resources.LastName;
                    }
                    break;

                case "Email":
                    if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = "Email";
                    }
                    break;
            }
        }

        private void TextBox_Initialized(object sender, EventArgs e)
        {
            switch ((sender as TextBox).Name)
            {
                case "Name":
                    (sender as TextBox).Text = Properties.Resources.FirstName;
                    break;

                case "LastName":
                    (sender as TextBox).Text = Properties.Resources.LastName;
                    break;

                case "Email":
                    (sender as TextBox).Text = "Email";
                    break;
            }
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            switch ((sender as PasswordBox).Name)
            {
                case "Password":
                    if ((sender as PasswordBox).Password == Properties.Resources.Password || string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                    {
                        (sender as PasswordBox).Password = "";
                    }
                    break;

                case "ConfPassword":
                    if ((sender as PasswordBox).Password == Properties.Resources.Conf_password || string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                    {
                        (sender as PasswordBox).Password = "";
                    }
                    break;
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            switch ((sender as PasswordBox).Name)
            {
                case "Password":
                    if (string.IsNullOrWhiteSpace((sender as PasswordBox).Password)) {
                        (sender as PasswordBox).Password = Properties.Resources.Password;
                    }
                    break;

                case "ConfPassword":
                    if (string.IsNullOrWhiteSpace((sender as PasswordBox).Password)) {
                        (sender as PasswordBox).Password = Properties.Resources.Conf_password;
                    }
                    break;
            }
        }

        private void Password_Initialized(object sender, EventArgs e)
        {
            switch ((sender as PasswordBox).Name)
            {
                case "Password":
                    (sender as PasswordBox).Password = Properties.Resources.Password;
                    break;

                case "ConfPassword":
                    (sender as PasswordBox).Password = Properties.Resources.Conf_password;
                    break;
            }
        }

        private void ConfPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (CheckPass != null)
            {
                CheckPass.Visibility = Visibility.Visible;
                if (Password.Password == ConfPassword.Password)
                {
                    CheckPass.Text = Properties.Resources.PassCorrect;
                    CheckPass.Foreground = new SolidColorBrush(Colors.Green);
                    correct = true;
                }
                else
                {
                    CheckPass.Text = Properties.Resources.PassUncorrect;
                    CheckPass.Foreground = new SolidColorBrush(Colors.Red);
                    correct = false;
                }
                if ((sender as PasswordBox).Password == Properties.Resources.Conf_password || string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                {
                    CheckPass.Text = "";
                    CheckPass.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text != string.Empty &&
                LastName.Text != string.Empty &&
                Email.Text != string.Empty &&
                Password.Password != string.Empty &&
                ConfPassword.Password != string.Empty &&
                correct)
            {
                Person person = new Person();

                person.firstname = Name.Text;
                person.lastname = LastName.Text;
                person.email = Email.Text;
                person.password = Password.Password;

                string json = Network.Serialize(person);
                HttpContent content = new StringContent(json);

                var answer = await Network.Response(content, "application/json", Properties.Settings.Default.URI + "api/registration", HttpMethod.Post);

                if (answer.answer != null)
                {
                    Application.Current.Properties["correct_registration"] = json;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    if (answer.response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
                        //DialogResult = false;
                        Error_Sing_Up.Text = Properties.Resources.ConnectionError;
                        Error_Sing_Up.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
