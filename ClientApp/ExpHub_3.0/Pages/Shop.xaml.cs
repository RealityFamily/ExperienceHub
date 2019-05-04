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
    /// Логика взаимодействия для Shop.xaml
    /// </summary>
    public partial class Shop : Page
    {
        public List<Lesson> lessons;
        public List<string> SortVer { get; set; }
        public Dictionary<string, List<string>> tr { get; set; }
        Person person;
        bool open = false;

        public Shop()
        {
            InitializeComponent();

            SortVer = new List<string> { Properties.Resources.Alphabet, Properties.Resources.BuyDate, Properties.Resources.RecordDate };

            tr = Network.Deserialize<Dictionary<string, List<string>>>(Network.ResponseAwaiter(null, "application/json", Properties.Settings.Default.URI + "api/category", HttpMethod.Get).Item1);

            Update();
        }

        private async void Update()
        {
            if (Properties.Settings.Default.Person != "")
            {
                 person = Network.Deserialize<Person>(Properties.Settings.Default.Person);

                try
                {
                    var json = await Network.Response(null, "application/json", Properties.Settings.Default.URI + "api/account/" + person.userid.ToString() + "/shop", HttpMethod.Get);
                    lessons = Network.Deserialize<List<Lesson>>(json.answer);
                    Place();
                }
                catch (Exception e)
                {
                    TextBlock error = new TextBlock { Text = Properties.Resources.ConnectionError, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                    LibraryGrid.Children.Add(error);
                }
            }
            else
            {
                TextBlock error = new TextBlock { Text = Properties.Resources.AccountError, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20 };
                LibraryGrid.Children.Add(error);
                Grid.SetColumnSpan(error, 6);
            }
        }

        private void Place()
        {
            if (Properties.Settings.Default.Person != "" && lessons != null)
            {
                LibraryGrid.Children.Clear();

                int a = (int)((FirstColumn.Width.Value - 40) / 340);

                LibraryGrid.ColumnDefinitions.Clear();

                for (int i = 0; i < a; i++)
                {
                    LibraryGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                LibraryGrid.RowDefinitions.Add(new RowDefinition());
                
                for (int i = 0; i <= lessons.Count / a ; i++)
                {
                    int j;
                    for (j = 0; j < a && (j + i * a) < lessons.Count; j++)
                    {
                        Border border = PasteCard(lessons[j + i*a]);
                        LibraryGrid.Children.Add(border);
                        Grid.SetColumn(border, j);
                        Grid.SetRow(border, i);
                    }

                    LibraryGrid.RowDefinitions.Add(new RowDefinition());

                }
            }
        }

        private Border PasteCard(Lesson lesson)
        {
            Border border = new Border
            {
                Width = 340,
                Height = 240,
                BorderBrush = new SolidColorBrush(Colors.DarkGray),
                CornerRadius = new CornerRadius(1),
                BorderThickness = new Thickness(3),
                Style = (Style)Application.Current.Resources["BorderStyle"],
                Margin = new Thickness(20)
            };

            //border.SetResourceReference(Border.BackgroundProperty, "BorderStyle");
            //border.Content = FindResource("BorderStyle");

            StackPanel panel = new StackPanel();
            // проверить, когда научимся загружать фотки в бд 
            //Image image = new Image { Source = GetImage(lesson).GetAwaiter().GetResult() }; 
            TextBlock name = new TextBlock { Text = lesson.Name, FontWeight = FontWeights.Bold };
            TextBlock creator = new TextBlock { Text = lesson.Creator, FontSize = 14 };

            //panel.Children.Add(image); 
            panel.Children.Add(name);
            panel.Children.Add(creator);

            border.Child = panel;

            border.MouseDown += async (sender, e) => {
                // проверить, когда научимся загружать фотки в бд 
                //Picture.Source = GetImage(lesson).GetAwaiter().GetResult(); 
                Name.Text = lesson.Name;
                Desription.Text = lesson.Description;
                Price.Text = Properties.Resources.Price + ": " + lesson.Price.ToString();
                Creator.Text = Properties.Resources.Creator + ": " + lesson.Creator;
                RealiseData.Text = Properties.Resources.RecordDate + ": " + lesson.ReleaseDate.ToString("dd MMMM yyyy");

                if (!open)
                {
                    while (SecondColums.Width.Value < 340)
                    {
                        SecondColums.Width = new GridLength(SecondColums.Width.Value + 1, GridUnitType.Pixel);
                        FirstColumn.Width = new GridLength(FirstColumn.Width.Value - 1, GridUnitType.Pixel);
                        if (SecondColums.Width.Value % 30 == 0)
                        {
                            await Task.Delay(1);
                        }
                        //Place();
                    }
                } else
                {
                    while (SecondColums.Width.Value > 0)
                    {
                        SecondColums.Width = new GridLength(SecondColums.Width.Value - 1, GridUnitType.Pixel);
                        FirstColumn.Width = new GridLength(FirstColumn.Width.Value + 1, GridUnitType.Pixel);
                        if (SecondColums.Width.Value % 30 == 0)
                        {
                            await Task.Delay(1);
                        }
                        //Place();
                    }
                }

                Place();

                open = !open;

                Buy.Click += async (sender1, e1) =>
                {
                    Lesson new_lesson = new Lesson();
                    new_lesson.LessonID = lesson.LessonID;
                    new_lesson.Price = lesson.Price;
                    var content = new StringContent(Network.Serialize(new_lesson));
                    
                    await Network.Response(content, "application/json", Properties.Settings.Default.URI + "api/account/" + person.userid.ToString() + "/lessons/purchase", HttpMethod.Post);

                    Update();
                };
            };

            return border;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedValue != null)
            {
                SubCategory.Visibility = Visibility.Visible;
                SubCategory.ItemsSource = tr[(sender as ComboBox).SelectedValue.ToString()];
            }
            else
            {
                SubCategory.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
