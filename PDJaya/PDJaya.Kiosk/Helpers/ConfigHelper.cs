using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using IniParser;
using IniParser.Model;
using System.IO;
using PDJaya.Tools;

namespace PDJaya.Kiosk.Helpers
{
    public class ConfigHelper
    {
        IniData fileConfig { set; get; }
        FileIniDataParser parser { set; get; }
        const string IniFile = "Configuration.ini";
        public ConfigHelper()
        {
            parser = new FileIniDataParser();
            if (!File.Exists(IniFile))
            {
                //File.WriteAllText(IniFile, "[Config]");
                fileConfig = new IniData();
            }
            else
            {
                fileConfig = parser.ReadFile(IniFile);
            }
        }
        public static bool CheckConfigIsExists()
        {
            return File.Exists(IniFile);
        }

        public void ReadConfigFromFile()
        {
            try
            {
                GlobalVars.Config.DesignHeight = Convert.ToDouble(GetSetting("DesignHeight"));
                GlobalVars.Config.DesignWidth = Convert.ToDouble(GetSetting("DesignWidth"));
                GlobalVars.Config.DeviceNo = GetSetting("DeviceClient");
                var ts = GetSetting("SyncTime").Split(':');
                GlobalVars.Config.SyncTime = new TimeSpan(int.Parse(ts[0]), int.Parse(ts[1]), int.Parse(ts[2]));
                GlobalVars.Config.ServiceAuth = GetSetting("ServiceAuth");
                GlobalVars.Config.ServiceHost = GetSetting("ServiceHost");
                GlobalVars.Config.Location = GetSetting("Location");
                GlobalVars.Config.ApiScope = GetSetting("ApiScope");
                GlobalVars.Config.ApiKey = GetSetting("ApiKey");
                GlobalVars.Config.PrintFile = GetSetting("PrintFile");
                GlobalVars.Config.IP = GetSetting("IP");
                GlobalVars.Config.MarketNo = GetSetting("MarketNo");
                ts = GetSetting("AutoCloseTimeout").Split(':');
                GlobalVars.Config.AutoCloseTimeout = new TimeSpan(int.Parse(ts[0]), int.Parse(ts[1]), int.Parse(ts[2]));
                GlobalVars.Config.GMTTimeGap = int.Parse(GetSetting("GMTTimeGap"));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Read Log Error:"+ex.Message);
            }
        }

        public async Task<bool> GetConfigFromServer(string DeviceNo)
        {
            try
            {
                PDJayaSync sync = new PDJayaSync();
                var configdata = await sync.GetConfigFromServer(DeviceNo);
                //write to app config
                SetSetting("DesignHeight", GlobalVars.Config.DesignHeight.ToString());
                SetSetting("DesignWidth", GlobalVars.Config.DesignWidth.ToString());
                SetSetting("DeviceNo", configdata.DeviceNo.ToString());
                SetSetting("SyncTime", $"{GlobalVars.Config.SyncTime.Hours}:{GlobalVars.Config.SyncTime.Minutes}:{GlobalVars.Config.SyncTime.Seconds}");
                SetSetting("ServiceAuth", GlobalVars.Config.ServiceAuth.ToString());
                SetSetting("ServiceHost", GlobalVars.Config.ServiceHost.ToString());
                SetSetting("Location", configdata.Location.ToString());
                SetSetting("ApiScope", GlobalVars.Config.ApiScope.ToString());
                SetSetting("ApiKey", GlobalVars.Config.ApiKey.ToString());
                SetSetting("PrintFile", GlobalVars.Config.PrintFile.ToString());
                SetSetting("IP", configdata.IP.ToString());
                SetSetting("MarketNo", configdata.MarketNo.ToString());
                SetSetting("AutoCloseTimeout", $"{GlobalVars.Config.AutoCloseTimeout.Hours}:{GlobalVars.Config.AutoCloseTimeout.Minutes}:{GlobalVars.Config.AutoCloseTimeout.Seconds}");
                SetSetting("GMTTimeGap", GlobalVars.Config.GMTTimeGap.ToString());
                Save();
                //ReadConfigFromFile();
                return true;
            }
            catch(Exception ex) {
                Logs.WriteLog("Error write to config file:" + ex.Message);
                Console.WriteLine("Error write to config file:"+ ex.Message); }
            return false;
        }
        public string GetSetting(string key)
        {
            return fileConfig["Config"][key];
        }

        public void SetSetting(string key, string value)
        {
            fileConfig["Config"][key] = value;
        }

        public void Save()
        {
            parser.WriteFile(IniFile, fileConfig);
        }
    }
}
