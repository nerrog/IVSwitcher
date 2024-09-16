using System.Windows;

namespace IVSwitcher
{
    /// <summary>
    /// licence.xaml の相互作用ロジック
    /// </summary>
    public partial class licence : Window
    {
        public licence()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }
    }
}
