using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace IVSwitcher
{
    class GameLoader
    {

        public static async void GTAV_Loader(string startup_type)
        {
            string jsonStr = IV_JSON.Read_JSON_AllLine(AppDomain.CurrentDomain.BaseDirectory + "settings.json", "utf-8");

            if(jsonStr == "json_error"|| jsonStr == "")
            {
                IVLogger.error("settings.json load error!");
                Application.Current.Shutdown();
            }

            IV_JSON.IV_SW_JSON _SW_JSON = new IV_JSON.IV_SW_JSON();
            _SW_JSON = JsonSerializer.Deserialize<IV_JSON.IV_SW_JSON>(jsonStr);

            if (startup_type == "mod")
            {
                //load mod version
                //e.g. xxx.dll.iv_sw → xxx.dll


                foreach (string str in _SW_JSON.dlls)
                {
                    if (File.Exists(str + ".iv_sw"))
                    {
                        File.Move(str + ".iv_sw", str);
                        IVLogger.info("Renamed "+str);
                    }
                }

                if (File.Exists(_SW_JSON.GTAV_PATH + @"\dinput8.dll.iv_sw"))
                {
                    File.Move(_SW_JSON.GTAV_PATH + @"\dinput8.dll.iv_sw", _SW_JSON.GTAV_PATH + @"\dinput8.dll");
                    IVLogger.info("Renamed dinput8.dll");
                }
            }
            else
            {
                //load vanilla version
                //e.g. xxx.dll → xxx.dll.iv_sw

                foreach (string str in _SW_JSON.dlls)
                {
                    if (File.Exists(str))
                    {
                        File.Move(str, str + ".iv_sw");
                        IVLogger.info("Renamed " + str + ".iv_sw");
                    }
                }

                if (File.Exists(_SW_JSON.GTAV_PATH + @"\dinput8.dll"))
                {
                    File.Move(_SW_JSON.GTAV_PATH + @"\dinput8.dll", _SW_JSON.GTAV_PATH + @"\dinput8.dll.iv_sw");
                    IVLogger.info("Renamed dinput8.dll.iv_sw");
                }
            }


            //Kill Epic Games Launcher

            if (_SW_JSON.use_epic)
            {
                Process[] ps =
                Process.GetProcessesByName("EpicGamesLauncher");

                foreach (Process p in ps)
                {
                    p.Kill();
                    IVLogger.info("Killed EpicGamesLaunche.exe ("+p.Id+")");
                }
            }


            await Task.Delay(500);

            Process process = new Process();
            process.StartInfo.WorkingDirectory = _SW_JSON.GTAV_PATH;

            process = Process.Start(_SW_JSON.exec_url);

            IVLogger.info("Starting "+_SW_JSON.exec_url);

            process.WaitForExit();

            string mes = "Process :" + process.Id + " Exit " + process.ExitCode.ToString();

            if (process.ExitCode != 0)
            {
                IVLogger.warn(mes);
            }
            else
            {
                IVLogger.info(mes);
            }

            

            await Task.Delay(1000);

            IVLogger.info("Shutdown IV Switcher bye.");

            Application.Current.Shutdown();
        }


    }
}
