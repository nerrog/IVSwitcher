using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace IVSwitcher
{
    class IV_JSON
    {


        public class IV_SW_JSON
        {
            public string GTAV_PATH { get; set; }
            public List<string> dlls { get; set; }
            public string exec_url { get; set; }
            public bool use_epic { get; set; }
        }

        public static string Read_JSON_AllLine(string filePath, string encodingName)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show(Properties.Resources.main_json_not_found+"\n"+filePath,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return "json_error";
            }
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(encodingName));
            string allLine = sr.ReadToEnd();
            sr.Close();

            return allLine;
        }
    }
}
