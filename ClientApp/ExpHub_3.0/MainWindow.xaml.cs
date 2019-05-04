using ExpHub_3._0.Pages;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Displaied = false;

        public MainWindow()
        {
            InitializeComponent();

            var uri = new Uri(@"Resources/" + Properties.Settings.Default.theme + ".xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);

            Content_Column.Width = new GridLength(SystemParameters.PrimaryScreenWidth - 40, GridUnitType.Pixel);

            if (Properties.Settings.Default.Person != "")
            {
                Content.JournalOwnership = JournalOwnership.UsesParentJournal;
                Content.Navigate(new Library());
            } else
            {
                Content.JournalOwnership = JournalOwnership.UsesParentJournal;
                Content.Navigate(new LogIn());
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Displaied)
            {
                Lable.Visibility = Visibility.Visible;
                Lesson.Visibility = Visibility.Visible;
                Library.Visibility = Visibility.Visible;
                Shop.Visibility = Visibility.Visible;
                Upload.Visibility = Visibility.Visible;
                Messages.Visibility = Visibility.Visible;
                My_Messages.Visibility = Visibility.Visible;
                Dialogs.Visibility = Visibility.Visible;
                Account.Visibility = Visibility.Visible;
                Settings.Visibility = Visibility.Visible;
                Close.Visibility = Visibility.Visible;

                while (NavigationMenu.Width < 200)
                {
                    First_Column.Width = new GridLength(First_Column.Width.Value + 1, GridUnitType.Pixel);
                    Content_Column.Width = new GridLength(Content_Column.Width.Value - 1, GridUnitType.Pixel);
                    NavigationMenu.Width = NavigationMenu.Width + 1;
                    if (NavigationMenu.Width % 20 == 0)
                    {
                        await Task.Delay(1);
                    }
                }

                Displaied = !Displaied;
            } else
            {
                while (NavigationMenu.Width > 40)
                {
                    First_Column.Width = new GridLength(First_Column.Width.Value - 1, GridUnitType.Pixel);
                    Content_Column.Width = new GridLength(Content_Column.Width.Value + 1, GridUnitType.Pixel);
                    NavigationMenu.Width = NavigationMenu.Width - 1;
                    if (NavigationMenu.Width % 20 == 0)
                    {
                        await Task.Delay(1);
                    }
                }

                Lable.Visibility = Visibility.Collapsed;
                Lesson.Visibility = Visibility.Collapsed;
                Library.Visibility = Visibility.Collapsed;
                Shop.Visibility = Visibility.Collapsed;
                Upload.Visibility = Visibility.Collapsed;
                Messages.Visibility = Visibility.Collapsed;
                My_Messages.Visibility = Visibility.Collapsed;
                Dialogs.Visibility = Visibility.Collapsed;
                Account.Visibility = Visibility.Collapsed;
                Settings.Visibility = Visibility.Collapsed;
                Close.Visibility = Visibility.Collapsed;

                Displaied = !Displaied;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Content.JournalOwnership = JournalOwnership.UsesParentJournal;
            Content.Navigate(new LogIn());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Content.JournalOwnership = JournalOwnership.UsesParentJournal;
            Content.Navigate(new Settings());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Content.JournalOwnership = JournalOwnership.UsesParentJournal;
            Content.Navigate(new Library());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Content.JournalOwnership = JournalOwnership.UsesParentJournal;
            Content.Navigate(new Shop());
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Content.JournalOwnership = JournalOwnership.UsesParentJournal;
            Content.Navigate(new LoadLesson());
        }
    }
}
