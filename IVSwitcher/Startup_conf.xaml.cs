﻿using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
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

        private void Open_Browser_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://cdn.nerrog.net/IV/IV_SW.mp4");
        }

        private void OK_Btn_Click(object sender, RoutedEventArgs e)
        {

            if (url_textbox.Text == "")
            {
                MessageBox.Show(Properties.Resources.startup_cfg_url_error);
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

            if ((bool)Add_Desktop_SC.IsChecked)
            {
                Create_Desktop_SC(Properties.Resources.startup_cfg_SC_name_mod, "-mod", Properties.Resources.startup_cfg_SC_comment);
                Create_Desktop_SC(Properties.Resources.startup_cfg_SC_name_online, "-online", Properties.Resources.startup_cfg_SC_comment);
            }

            Confi_Dialog.IsOpen = true;
        }

        private void Create_Desktop_SC(string SC_Name, string parameter, string comment)
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();

            string shortcutPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            SC_Name+".lnk");

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

            IWshRuntimeLibrary.IWshShortcut shortcut =
                (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            
            shortcut.TargetPath = myAssembly.Location;

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
                shortcut.IconLocation = myAssembly.Location + ",0";
            }

            //ショートカットを作成
            shortcut.Save();

            Marshal.FinalReleaseComObject(shortcut);
            Marshal.FinalReleaseComObject(shell);
        }
    }
}