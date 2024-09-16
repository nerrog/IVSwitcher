using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace IVSwitcher
{
    /// <summary>
    /// load_mod.xaml の相互作用ロジック
    /// </summary>
    public partial class load_mod : Page
    {
        public load_mod()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();
            var version = assembly.Version;
            version_label.Text = assembly.Name + " " + version.ToString(3);

            await Task.Delay(1000);

            //Lunch GTA5 (MOD)

            GameLoader.GTAV_Loader("mod");

        }
    }
}
