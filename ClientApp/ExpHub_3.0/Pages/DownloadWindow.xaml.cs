using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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


namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        Thread thread;
        bool work = true;

        public DownloadWindow()
        {
            InitializeComponent();

             thread= new Thread(() => {
                Lesson lesson = (Lesson)Application.Current.Properties["Lesson"];

                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_Progress);
                client.DownloadFileAsync(new Uri(lesson.Path), Path.Combine(Properties.Settings.Default.Load_Path, lesson.Name, ".zip"));
            });
            thread.Start();

        }

        private void client_Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            double btsIn = double.Parse(e.BytesReceived.ToString());
            double btsTotlal = double.Parse(e.TotalBytesToReceive.ToString());
            double procent = btsIn / btsTotlal * 100;
            Status.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
            progressBar.Value = int.Parse(Math.Truncate(procent).ToString());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (work) {
                thread.Suspend();
                Pause.Content = Properties.Resources.Resume;
                work = !work;
            } else
            {
                thread.Resume();
                Pause.Content = Properties.Resources.Pause;
                work = !work;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            thread.Abort();
            Close();
        }
    }
}
