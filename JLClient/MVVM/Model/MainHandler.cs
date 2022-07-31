using JLClient.Core;
using JLClient.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model
{
    public class MainHandler
    {
        public bool IsEditor 
        { 
            get
            {
                var settings = UserSettings.GetInstance();
                if (settings != null && settings.Roles != null && settings.Roles.Length > 0)
                {
                    return settings.Roles.Any(x => x.systemName == Consts.Roles.Editor);
                }

                return false;
            } 
        }

        public bool IsTeacher
        {
            get
            {
                var settings = UserSettings.GetInstance();
                if (settings != null && settings.Roles != null && settings.Roles.Length > 0)
                {
                    return settings.Roles.Any(x => x.systemName == Consts.Roles.Teacher);
                }

                return false;
            }
        }

        public void ClearUserData()
        {
            var settings = UserSettings.GetInstance();
            settings.SignalrConnectionId = null;
            settings.Roles = null;
            settings.CurrentUser = null;
            settings.JWT = null;
            settings.OsVersion = null;
            settings.FullSupport = false;
        }

        public void SetOSDataSetting()
        {
            var settings = UserSettings.GetInstance();
            var availableVersions = new List<string> { "8", "8.1", "10" };
            var os = Environment.OSVersion;
            var vs = os.Version;

            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                    settings.OsVersion = GetWin32NTVersion(vs);
                    settings.FullSupport = availableVersions.Contains(settings.OsVersion);
                    break;
                default:
                    settings.OsVersion = "Undefined";
                    settings.FullSupport = false;
                    break;
            }
        }

        private string GetWin32NTVersion(Version vs)
        {
            switch (vs.Major)
            {
                case 3:
                    return "NT 3.51";
                case 4:
                    return "NT 4.0";
                case 5:
                    return vs.Minor == 0 ? "2000" : "XP";
                case 6:
                    return GetWin32NTMajor6Version(vs.Minor);
                case 10:
                    return "10";
                default:
                    return "";
            }
        }

        private string GetWin32NTMajor6Version(int minor)
        {
            switch (minor)
            {
                case 0:
                    return "Vista";
                case 1:
                    return "7";
                case 2:
                    return "8";
                default:
                    return "8.1";
            }
        }
    }
}
