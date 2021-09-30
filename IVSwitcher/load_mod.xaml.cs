using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.IO;

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

            string jsonStr = IV_JSON.Read_JSON_AllLine(AppDomain.CurrentDomain.BaseDirectory + "settings.json", "utf-8");
            IV_JSON.IV_SW_JSON _SW_JSON = new IV_JSON.IV_SW_JSON();
            _SW_JSON = JsonSerializer.Deserialize<IV_JSON.IV_SW_JSON>(jsonStr);

            foreach (string str in _SW_JSON.dlls)
            {
                if (File.Exists(str + ".iv_sw"))
                {
                    File.Move(str + ".iv_sw", str);
                }
            }

            if (File.Exists(_SW_JSON.GTAV_PATH + @"\dinput8.dll.iv_sw"))
            {
                File.Move(_SW_JSON.GTAV_PATH + @"\dinput8.dll.iv_sw", _SW_JSON.GTAV_PATH + @"\dinput8.dll");
            }

            await Task.Delay(500);

            //Kill Epic Games Launcher

            if (_SW_JSON.use_epic)
            {
                Process[] ps =
                Process.GetProcessesByName("EpicGamesLauncher");

                foreach (Process p in ps)
                {
                    p.Kill();
                }
            }

            Process.Start(_SW_JSON.exec_url);

            await Task.Delay(500);

            Application.Current.Shutdown();

        }
    }
}
