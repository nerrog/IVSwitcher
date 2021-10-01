using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace IVSwitcher
{
    /// <summary>
    /// SettingPage.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingPage : Page
    {

        private bool gtav_found_status = false;


        public SettingPage()
        {
            InitializeComponent();
            IVLogger.info("Setting Page Loaded");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Show OSS licence
            new licence().Show();
        }

        private void GTAV_Folder_Select_Click(object sender, RoutedEventArgs e)
        {
            //フォルダ選択画面を開き、テキストボックスに代入する
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = $"{Properties.Resources.settings_location_text}",
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                GTAV_PATH.Text = cofd.FileName;

                if (File.Exists(cofd.FileName + @"\GTA5.exe"))
                {
                    gtavexe_status.Text = Properties.Resources.settings_gtav_exists;
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#008000");
                    gtavexe_status.Foreground = brush;
                    gtav_found_status = true;
                }
                else
                {
                    gtavexe_status.Text = Properties.Resources.settings_gtav_no_exists;
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#ff0000");
                    gtavexe_status.Foreground = brush;
                    gtav_found_status = false;
                }
            }
        }

        private void dll_select_btn_Click(object sender, RoutedEventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = $"{Properties.Resources.settings_dll_listbox}"
            })

            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                dll_listbox.Items.Add(cofd.FileName);
            }
        }

        private void dll_select_del_Click(object sender, RoutedEventArgs e)
        {
            if (dll_listbox.SelectedIndex == -1)
            {
                return;
            }
            dll_listbox.Items.RemoveAt(dll_listbox.SelectedIndex);
        }

        private void GTAV_PATH_LostFocus(object sender, RoutedEventArgs e)
        {
            if (File.Exists(GTAV_PATH.Text + @"\GTA5.exe"))
            {
                gtavexe_status.Text = Properties.Resources.settings_gtav_exists;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#008000");
                gtavexe_status.Foreground = brush;
                gtav_found_status = true;
            }
            else
            {
                gtavexe_status.Text = Properties.Resources.settings_gtav_no_exists;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#ff0000");
                gtavexe_status.Foreground = brush;
                gtav_found_status = false;
            }
        }

        private void settings_ok_Click(object sender, RoutedEventArgs e)
        {
            if (gtav_found_status)
            {
                var LIST_DLL = new List<string>();
                for (int i = 0; i < dll_listbox.Items.Count; i++)
                {
                    ListBoxItem lbi = (ListBoxItem)
                    dll_listbox.ItemContainerGenerator.ContainerFromIndex(i);
                    LIST_DLL.Add(lbi.Content.ToString());
                }

                IV_JSON.IV_SW_JSON _JSON = new IV_JSON.IV_SW_JSON();

                _JSON.GTAV_PATH = GTAV_PATH.Text;
                _JSON.dlls = LIST_DLL;
                _JSON.exec_url = "";

                JsonSerializerOptions JSON_options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                string jsonStr = JsonSerializer.Serialize(_JSON, JSON_options);


                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+"settings.json", jsonStr);
                IVLogger.info("Write settings.json");

                NavigationService.Navigate(new Startup_conf());

            }
            else
            {
                System.Windows.MessageBox.Show(Properties.Resources.settings_gtav_not_found_message);
                return;
            }
        }
    }
}
