using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace RpNet.Maps.Server
{
    public class Main : BaseScript
    {
        public Main()
        {
            EventHandlers["RpNet.Exception"] += new Action<string, string>(LogError);
        }
        private void LogError(string message, string stackTrace)
        {
            Debug.WriteLine($"{DateTime.Now} - Trying Save Log!!");
            API.SaveResourceFile(API.GetCurrentResourceName(), "log/errors.txt", $"{message} | {stackTrace}\n", -1);
            Debug.WriteLine($"{DateTime.Now} - Log Saved!!");
        }

    }
}
