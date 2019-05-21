using ExperenceHubApp;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        private bool work;
        private WebClient client = new WebClient();
        private string path;

        public Download()
        {
            InitializeComponent();

            Topmost = true;

            Lesson lesson = (Lesson)Application.Current.Properties["Lesson"];

            client.DownloadProgressChanged += client_Progress;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            Uri uri = new Uri(Properties.Settings.Default.URI + lesson.Path);
            path = Path.Combine(Properties.Settings.Default.Load_Path, lesson.Name + ".zip");

            if (!Directory.Exists(Properties.Settings.Default.Load_Path)){
                Directory.CreateDirectory(Properties.Settings.Default.Load_Path);
            }

            client.DownloadFileAsync(uri, path);
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            OK.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Collapsed;

            Status.Text = Properties.Resources.DownloadFinished;
            ZipFile.ExtractToDirectory(path, Path.Combine(Properties.Settings.Default.Load_Path, Path.GetFileNameWithoutExtension(path)));
            File.Delete(path);
        }

        private void client_Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            double btsIn = double.Parse(e.BytesReceived.ToString());
            double btsTotlal = double.Parse(e.TotalBytesToReceive.ToString());
            double procent = btsIn / btsTotlal * 100;

            Status.Text = Properties.Resources.Downloading + " " + Math.Truncate(procent).ToString() + "%";
            progressBar.Value = int.Parse(Math.Truncate(procent).ToString());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            client.CancelAsync();
            Close();
        }
    }
}
