using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IVSwitcher
{
    /// <summary>
    /// load_online.xaml の相互作用ロジック
    /// </summary>
    public partial class load_online : Page
    {
        public load_online()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            var version = assembly.Version;
            version_label.Text = assembly.Name + " " + version.ToString(3);

            await Task.Delay(1000);

            //Lunch GTA5 (ONLINE)

            GameLoader.GTAV_Loader("online");

            Application.Current.Shutdown();
        }
    }
}
