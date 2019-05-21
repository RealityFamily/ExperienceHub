using ExperenceHubApp;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Windows;
using System.Net.Http;
using System.Windows.Controls;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для Upload.xaml
    /// </summary>
    public partial class Upload : Window
    {
        private WebClient client = new WebClient();

        public Upload()
        {
            InitializeComponent();

            Lesson lesson = (Lesson)Application.Current.Properties["Lesson"];
            string file_path = (string)Application.Current.Properties["File_path"];

            client.UploadProgressChanged += Client_UploadProgressChanged;
            client.UploadFileCompleted += Client_UploadFileCompleted;
            Uri uri = new Uri(Properties.Settings.Default.URI + "api/file/" + lesson.LessonID.ToString() + "/upload");
            client.UploadFileAsync(uri, file_path);
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            OK.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Collapsed;

            Status.Text = Properties.Resources.UploadFinished;
        }

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            double btsIn = double.Parse(e.BytesSent.ToString());
            double btsTotlal = double.Parse(e.TotalBytesToSend.ToString());
            double procent = btsIn / btsTotlal * 100;

            Status.Text = Properties.Resources.Uploading + " " + Math.Truncate(procent) + "%";
            progressBar.Value = int.Parse(Math.Truncate(procent).ToString());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Frame cont = ((MainWindow)Application.Current.MainWindow).Content;
            cont.JournalOwnership = System.Windows.Navigation.JournalOwnership.UsesParentJournal;
            cont.Navigate(new Library());
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            client.CancelAsync();
            Close();
        }
    }
}
