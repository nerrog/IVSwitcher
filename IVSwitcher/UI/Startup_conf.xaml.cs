using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IVSwitcher
{

    /// <summary>
    /// Startup_conf.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class Startup_conf : Page
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public Startup_conf()
        {
            InitializeComponent();
            IVLogger.info("Startup_conf loaded");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Show OSS licence
            new licence().Show();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Bitmap canvas = new Bitmap(30, 30);
            Graphics g = Graphics.FromImage(canvas);
            g.DrawIcon(SystemIcons.Application, 0, 0);
            g.Dispose();
            def_icon.Source = ImageSourceFromBitmap(canvas);

        }


        private void OK_Btn_Click(object sender, RoutedEventArgs e)
        {

            if (url_textbox.Text == "")
            {
                System.Windows.MessageBox.Show(Properties.Resources.startup_cfg_url_error);
                IVLogger.warn("Url box is empty return");
                return;
            }


            string jsonStr = IV_JSON.Read_JSON_AllLine(AppDomain.CurrentDomain.BaseDirectory + "settings.json", "utf-8");

            IV_JSON.IV_SW_JSON _JSON = new IV_JSON.IV_SW_JSON();
            _JSON = JsonSerializer.Deserialize<IV_JSON.IV_SW_JSON>(jsonStr);
            _JSON.exec_url = url_textbox.Text;

            if ((bool)USE_EPIC.IsChecked)
            {
                _JSON.use_epic = true;
            }
            else
            {
                _JSON.use_epic = false;
            }

            JsonSerializerOptions JSON_options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            string jsonStr_NEW = JsonSerializer.Serialize(_JSON, JSON_options);


            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "settings.json", jsonStr_NEW);

            IVLogger.info("Write settings.json");

            if ((bool)Add_Desktop_SC.IsChecked)
            {
                Create_Desktop_SC(Properties.Resources.startup_cfg_SC_name_mod, "-mod", Properties.Resources.startup_cfg_SC_comment);
                Create_Desktop_SC(Properties.Resources.startup_cfg_SC_name_online, "-online", Properties.Resources.startup_cfg_SC_comment);
            }

            Confi_Dialog.IsOpen = true;
        }

        private void Create_Desktop_SC(string SC_Name, string parameter, string comment)
        {
            IVLogger.info("Creating Desktop Shortcut "+parameter);

            string exepath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            string shortcutPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            SC_Name+".lnk");

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

            IWshRuntimeLibrary.IWshShortcut shortcut =
                (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            
            shortcut.TargetPath = exepath;

            //パラメーター
            shortcut.Arguments = parameter;
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            shortcut.WindowStyle = 1;
            //コメント
            shortcut.Description = comment;
            //アイコン
            if ((bool)SC_GTAV.IsChecked)
            {
                string jsonStr = IV_JSON.Read_JSON_AllLine(AppDomain.CurrentDomain.BaseDirectory + "settings.json", "utf-8");
                IV_JSON.IV_SW_JSON _SW_JSON = new IV_JSON.IV_SW_JSON();
                _SW_JSON = JsonSerializer.Deserialize<IV_JSON.IV_SW_JSON>(jsonStr);

                shortcut.IconLocation = _SW_JSON.GTAV_PATH + @"\GTA5.exe" + ",0";
            }
            else
            {
                shortcut.IconLocation = exepath + ",0";
            }

            //ショートカットを作成
            try { 
                shortcut.Save();
            }
            catch(Exception e)
            {
                IVLogger.error("Create Desktop Shortcut failed" + e.Message);
            }

            Marshal.FinalReleaseComObject(shortcut);
            Marshal.FinalReleaseComObject(shell);
        }

        private void exe_chk_box_Click(object sender, RoutedEventArgs e)
        {
            Open_btn.Visibility = Visibility.Visible;
        }

        private void url_chk_box_Click(object sender, RoutedEventArgs e)
        {
            Open_btn.Visibility = Visibility.Collapsed;
        }

        private void Open_btn_Click(object sender, RoutedEventArgs e)
        {
            //ファイルを開くダイアログボックスの作成 
            var ofd = new OpenFileDialog();
            ofd.Filter = "exe file|*.exe";
            if (ofd.ShowDialog() == DialogResult.Cancel) return;
            url_textbox.Text = ofd.FileName;
        }
    }
}
