using System;
using System.IO;
using Modding;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HKRoomLogger
{
    public class HKRoomLoggerMod : Mod
    {
        public static string LogPath => Path.Combine(Application.persistentDataPath, "RoomLogger.json");

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
            Log("Initializing");

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChange;

            Log("Initialized");
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

    }
}
