using Modding;
using System;

namespace HKRoomLogger
{
    public class HKRoomLoggerMod : Mod
    {
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

            // put additional initialization logic here

            Log("Initialized");
        }
    }
}
