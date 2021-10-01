using System;
using System.Windows;

namespace IVSwitcher
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IVLogger.info("Starting IV Switcher");

            string[] args = Environment.GetCommandLineArgs();

            Uri uri = new Uri("SettingPage.xaml", UriKind.Relative);
            if (args.Length != 1)
            {
                if(args[1] == "-mod")
                {
                    uri = new Uri("load_mod.xaml", UriKind.Relative);
                    IVLogger.info("Loading MOD screen");

                }
                else if (args[1] == "-online")
                {
                    uri = new Uri("load_online.xaml", UriKind.Relative);
                    IVLogger.info("Loading ONLINE screen");
                }
            }

            frame.Source = uri;
        }

    }
}
