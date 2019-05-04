using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using winForm = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpHub_3._0
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public List<string> themes { get; set; }
        public List<string> launguage { get; set; }
        private bool vr_on = false;
        private string unity_folder;

        public Settings()
        {
            InitializeComponent();
            themes = new List<string> { "Gray_orange", "Dark", "White", "Light" };
            launguage = new List<string> { "Русский", "English" };

            if (Properties.Settings.Default.Load_Path != string.Empty)
            {
                Load_Path.Text = Properties.Settings.Default.Load_Path;
            }
            if (Properties.Settings.Default.Unity_Path != string.Empty)
            {
                Unity_Path.Text = Properties.Settings.Default.Unity_Path;
            }

            for (int i = 0; i < themes.Count; i++)
            {
                if (themes[i] == Properties.Settings.Default.theme)
                {
                    theme.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < launguage.Count; i++)
            {
                if (launguage[i] == Properties.Settings.Default.Language)
                {
                    Language.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string style = (sender as ComboBox).SelectedItem as string;
                Properties.Settings.Default.theme = style;
                var uri = new Uri(@"Resources/" + style + ".xaml", UriKind.Relative);
                ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                Properties.Settings.Default.Save();
            }
            catch (Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            winForm.FolderBrowserDialog browserDialog = new winForm.FolderBrowserDialog();
            browserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            browserDialog.ShowNewFolderButton = true;
            if (browserDialog.ShowDialog() == winForm.DialogResult.OK)
            {
                Load_Path.Text = browserDialog.SelectedPath;
                Properties.Settings.Default.Load_Path = browserDialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            winForm.FolderBrowserDialog browserDialog = new winForm.FolderBrowserDialog();
            browserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            browserDialog.ShowNewFolderButton = true;
            if (browserDialog.ShowDialog() == winForm.DialogResult.OK)
            {
                Unity_Path.Text = browserDialog.SelectedPath;
                Properties.Settings.Default.Unity_Path = browserDialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (!vr_on)
            {
                Unity_Path_Stack.Visibility = Visibility.Visible;
                vr_on = !vr_on;
            } else
            {
                Unity_Path_Stack.Visibility = Visibility.Collapsed;
                vr_on = !vr_on;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
