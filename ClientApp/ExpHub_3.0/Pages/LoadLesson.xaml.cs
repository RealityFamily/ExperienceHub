using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using winForm = System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpHub_3._0.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadLesson.xaml
    /// </summary>
    public partial class LoadLesson : Page
    {
        string PhotoPath;
        string FilePath;

        public Dictionary<string, List<string>> Tr { get; set; }

        public LoadLesson()
        {
            InitializeComponent();

            if (Properties.Settings.Default.Person != "")
            {
                Tr = Network.Deserialize<Dictionary<string, List<string>>>(Network.ResponseAwaiter(null, "application/json", Properties.Settings.Default.URI + "api/category", HttpMethod.Get).Item1);
                Main_Cont.Visibility = Visibility.Visible;
            } else
            {
                TextBlock error = new TextBlock { Text = Properties.Resources.AccountError, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20 };
                Content.Children.Add(error);
            }

        }

        private void Cat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SubCategory.ItemsSource = Tr[(sender as ComboBox).SelectedValue.ToString()];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            winForm.OpenFileDialog browserDialog = new winForm.OpenFileDialog();
            browserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            browserDialog.Filter = "Image files (*.jpeg, *.png)|*.jpeg; *.png";
            browserDialog.FilterIndex = 1;
            browserDialog.RestoreDirectory = true;
            if (browserDialog.ShowDialog() == winForm.DialogResult.OK)
            {
                PhotoName.Text = System.IO.Path.GetFileName(browserDialog.FileName);

                Prev.Source = new BitmapImage(new Uri(browserDialog.FileName));

                PhotoPath = browserDialog.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            winForm.OpenFileDialog browserDialog = new winForm.OpenFileDialog();
            browserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            browserDialog.Filter = "ZIP files (*.zip)|*.zip";
            browserDialog.FilterIndex = 1;
            browserDialog.RestoreDirectory = true;
            if (browserDialog.ShowDialog() == winForm.DialogResult.OK)
            {
                FileName.Text = System.IO.Path.GetFileName(browserDialog.FileName);

                FilePath = browserDialog.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Lesson lesson = new Lesson();
            Person person = Network.Deserialize<Person>(Properties.Settings.Default.Person);

            lesson.Name = Name.Text;
            lesson.Picture = System.IO.File.ReadAllBytes(PhotoPath);
            lesson.Price = float.Parse(Price.Text);
            lesson.Description = Description.Text;
            lesson.CreatorID = person.userid;

            string json = Network.Serialize<Lesson>(lesson);
            HttpContent content = new StringContent(json);

            var answer = Network.ResponseAwaiter(content, "application/json", Properties.Settings.Default.URI + "api/lesson/add", HttpMethod.Post);
            if (answer.answer != null)
            {
                lesson.LessonID = Network.Deserialize<Guid>(answer.answer);

                StreamContent fileContent = new StreamContent(System.IO.File.OpenRead(FilePath));
                MultipartFormDataContent Fcontent = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");
                Fcontent.Add(fileContent, "uploadedFile", System.IO.Path.GetFileName(FilePath));

                Network.ResponseAwaiter(Fcontent, Fcontent.Headers.ContentType.MediaType, Properties.Settings.Default.URI + "api/file/" + lesson.LessonID.ToString() + "/upload", HttpMethod.Post);
            } else
            {
                TextBlock error = new TextBlock { Text = Properties.Resources.ConnectionError, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20 };
                Content.Children.Add(error);
            }
        }
    }
}
