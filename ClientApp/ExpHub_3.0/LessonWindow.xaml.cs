using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для LessonWindow.xaml
    /// </summary>
    public partial class LessonWindow : Window
    {
        Lesson lesson;
        public LessonWindow()
        {
            InitializeComponent();

            Topmost = true;

            lesson = (Lesson)Application.Current.Properties["Lesson"];

            Name.Text = lesson.Name;
            Desription.Text = lesson.Description;
            Price.Text = lesson.Price.ToString() + " " + Properties.Resources.Currency;
            Creator.Text = lesson.Creator;
            RealiseData.Text = lesson.ReleaseDate.ToString("dd MMMM yyyy");
            PurchaseData.Text = lesson.PurchaseDate.ToString("dd MMMM yyyy");

            Prev.Source = lesson.GetPrev();

            string mode = Application.Current.Properties["Mode"].ToString();
            switch (mode)
            {
                case "buy":
                    Buy.Visibility = Visibility.Visible;
                    Status.Visibility = Visibility.Collapsed;
                    StatusN.Visibility = Visibility.Collapsed;
                    break;
                case "load and work":
                    if (Directory.Exists(Path.Combine(Properties.Settings.Default.Load_Path, lesson.Name)))
                    {
                        Status.Text = Properties.Resources.OnPC;
                        Launch.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Status.Text = Properties.Resources.NotOnPC;
                        Download.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["Lesson"] = lesson;
            Download download = new Download("lesson");
            download.Show();
        }

        private async void Buy_Click(object sender, RoutedEventArgs e)
        {
            Person person = Network.Deserialize<Person>(Properties.Settings.Default.Person);

            Lesson new_lesson = new Lesson();
            new_lesson.LessonID = lesson.LessonID;
            new_lesson.Price = lesson.Price;
            var content = new StringContent(Network.Serialize(new_lesson));

            await Network.Response(content, "application/json", Properties.Settings.Default.URI + "api/account/" + person.userid.ToString() + "/lessons/purchase", HttpMethod.Post);
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = File.CreateText(Path.Combine(Path.GetDirectoryName(Properties.Settings.Default.Unity_Path), "settings.txt")))
            {
                sw.WriteLine(Properties.Settings.Default.VR); // используют ли VR
                sw.WriteLine(Path.Combine(Properties.Settings.Default.Load_Path, lesson.Name)); // папка расположения урока
            }
            Process.Start(Properties.Settings.Default.Unity_Path);
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
