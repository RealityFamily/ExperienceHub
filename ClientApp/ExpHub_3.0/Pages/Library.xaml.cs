using ExperenceHubApp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для Library.xaml
    /// </summary>
    public partial class Library : Page
    {
        public List<Lesson> lessons;
        public List<string> SortVer { get; set; }
        public Dictionary<string, List<string>> tr { get; set; }
        bool open = false;


        public Library()
        {
            InitializeComponent();

            SortVer = new List<string> { Properties.Resources.Alphabet, Properties.Resources.BuyDate, Properties.Resources.RecordDate };

            try
            {
                var a = Network.ResponseAwaiter(null, "application/json", Properties.Settings.Default.URI + "api/category", HttpMethod.Get).Item1;
                tr = Network.Deserialize<Dictionary<string, List<string>>>(a);

                Update();
                Place();
            } catch
            {
                TextBlock error = new TextBlock { Text = Properties.Resources.ConnectionError, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, FontSize = 20 };
                LibraryGrid.Children.Add(error);
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

                for (int i = 0; i < lessons.Count; i++)
                {
                    int j;
                    for (j = 0; j < a && (j + i * a) < lessons.Count; j++)
                    {
                        Border border = PasteCard(lessons[j + i * a]);
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

            StackPanel panel = new StackPanel();
            Image image = new Image { Source = lesson.GetPrev(), MaxHeight = 180, MaxWidth = 240, Stretch = Stretch.Fill, Margin = new Thickness(0, 5, 0, 5), HorizontalAlignment = HorizontalAlignment.Center };
            TextBlock name = new TextBlock { Text = lesson.Name, FontWeight = FontWeights.Bold };
            TextBlock creator = new TextBlock { Text = lesson.Creator, FontSize = 14 };

            panel.Children.Add(image); 
            panel.Children.Add(name);
            panel.Children.Add(creator);

            border.Child = panel;

            border.MouseDown += (sender, e) => {
                Application.Current.Properties["Lesson"] = lesson;
                Application.Current.Properties["Mode"] = "load and work";

                LessonWindow window = new LessonWindow();
                window.ShowDialog();
            };

            return border;
        }

        private void Update()
        {
            if (Properties.Settings.Default.Person != "")
            {
                Person person = Network.Deserialize<Person>(Properties.Settings.Default.Person);

                try
                {
                    person.Lessons.Clear();

                    var json = Network.ResponseAwaiter(null, "application/json", Properties.Settings.Default.URI + "api/account/" + person.userid.ToString() + "/get_my_lessons", HttpMethod.Get);
                    lessons = Network.Deserialize<List<Lesson>>(json.answer);
                    for (int i = 0; i < lessons.Count; i++)
                    {
                        person.Lessons.Add(lessons[i]);
                    }

                    Properties.Settings.Default.Person = Network.Serialize(person);
                    lessons = person.Lessons;
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
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedValue != null) {
                SubCategory.Visibility = Visibility.Visible;
                SubCategory.ItemsSource = tr[(sender as ComboBox).SelectedValue.ToString()];
            } else
            {
                SubCategory.Visibility = Visibility.Collapsed;
            }
        }
    }
}
