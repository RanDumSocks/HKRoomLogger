using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Modding;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace HKRoomLogger
{
    public class HKRoomLoggerMod : Mod, IMenuMod
    {
        public static string LogPath => Path.Combine(Application.persistentDataPath, "RoomLogger.json");
        public static string programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../Local/Programs/hkat/hkat.exe");

        public struct OutData
        {
            public string last;
            public string current;
        }

        private static HKRoomLoggerMod? _instance;

        internal static HKRoomLoggerMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(HKRoomLoggerMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public HKRoomLoggerMod() : base("HKRoomLogger")
        {
            _instance = this;
        }

        public override void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;
        }
        public void OnSceneChange(Scene oldScene, Scene newScene)
        {
            var data = new OutData
            {
                last = oldScene.name,
                current = newScene.name
            };

            Log($"{data.last} --> {data.current}");

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogPath));
                File.WriteAllText(LogPath, JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                LogError($"Error printing log to {LogPath}:\n{e}");
            }
        }

        public bool ToggleButtonInsideMenu => false;
        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            string desc;
            string title;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                desc = "Opens companion tracker if it exists. Will take you to the download page if not found on your system.";
                title = "Open Tracker";
            } 
            else 
            {
                desc = "The tracker is not avaliable for your OS. However, you can give it a looksie if you want.";
                title = "Open Tracker Repository";
            }

            return new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry
                {
                    Name = title,
                    Description = desc,
                    Values = new string[] { "" },
                    Saver = opt => { 
                        if (File.Exists(programPath))
                        {
                            Process.Start(programPath);
                        } else
                        {
                            Process.Start("https://github.com/RanDumSocks/HKAutoTrackerElectron/releases/latest");
                        }
                    },
                    Loader = () => {
                        return 1;
                    }
                }
            };
        }
    }
}
