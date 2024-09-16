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
            try
            {
                string jsonStr = IV_JSON.Read_JSON_AllLine(AppDomain.CurrentDomain.BaseDirectory + "settings.json", "utf-8");

                if (jsonStr == "json_error" || jsonStr == "")
                {
                    IVLogger.error("settings.json load error!");
                    Application.Current.Shutdown();
                }

                IV_JSON.IV_SW_JSON _SW_JSON = new IV_JSON.IV_SW_JSON();
                _SW_JSON = JsonSerializer.Deserialize<IV_JSON.IV_SW_JSON>(jsonStr);

                string disabledDllExtension = _SW_JSON.disableFileExtension;
                string gameStartOptions = "";

                if (startup_type == "mod")
                {
                    //load mod version
                    //e.g. xxx.dll.iv_sw → xxx.dll


                    foreach (string str in _SW_JSON.dlls)
                    {
                        if (File.Exists(str + disabledDllExtension))
                        {
                            File.Move(str + disabledDllExtension, str);
                            IVLogger.info("Renamed " + str);
                        }
                    }

                    if (File.Exists(_SW_JSON.GTAV_PATH + @$"\dinput8.dll{disabledDllExtension}"))
                    {
                        File.Move(_SW_JSON.GTAV_PATH + @$"\dinput8.dll{disabledDllExtension}", _SW_JSON.GTAV_PATH + @"\dinput8.dll");
                        IVLogger.info("Renamed dinput8.dll");
                    }

                    if (_SW_JSON.useLoadGameDirect)
                    {
                        gameStartOptions = "-scofflineonly";
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
                            File.Move(str, str + disabledDllExtension);
                            IVLogger.info("Renamed " + str + disabledDllExtension);
                        }
                    }

                    if (File.Exists(_SW_JSON.GTAV_PATH + @"\dinput8.dll"))
                    {
                        File.Move(_SW_JSON.GTAV_PATH + @"\dinput8.dll", _SW_JSON.GTAV_PATH + $@"\dinput8.dll{disabledDllExtension}");
                        IVLogger.info($"Renamed dinput8.dll{disabledDllExtension}");
                    }

                    if (_SW_JSON.useLoadGameDirect)
                    {
                        gameStartOptions = "-StraightIntoFreemode";
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
                        IVLogger.info("Killed EpicGamesLaunche.exe (" + p.Id + ")");
                    }
                }


                await Task.Delay(500);

                Process process = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = _SW_JSON.GTAV_PATH;

                string startUrl = _SW_JSON.exec_url;
                if (gameStartOptions != "")
                {
                    if (_SW_JSON.exec_url.StartsWith("steam://"))
                    {
                        startUrl = $"{_SW_JSON.exec_url}//{gameStartOptions}";
                    }
                    else
                    {
                        info.Arguments = gameStartOptions;
                    }
                }

                info.FileName = startUrl;
                info.UseShellExecute = true;
                process = Process.Start(info);

                IVLogger.info("Starting " + startUrl);

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
            catch (Exception ex)
            {
                IVLogger.error(ex.Message);
                IVLogger.error(ex.StackTrace);
            }
        }
    }
}
