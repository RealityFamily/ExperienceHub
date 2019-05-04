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

namespace ExpHub_3._0.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadLesson.xaml
    /// </summary>
    public partial class LoadLesson : Page
    {
        public Dictionary<string, List<string>> Tr { get; set; }

        public LoadLesson()
        {
            InitializeComponent();

            Tr = Network.Deserialize<Dictionary<string, List<string>>>(Network.ResponseAwaiter(null, "application/json", Properties.Settings.Default.URI + "api/category", HttpMethod.Get).Item1);
            
        }
    }
}
