using JLClient.Core.PersistModels;

namespace JLClient.Core.Settings
{
    public class UserSettings
    {
        private static UserSettings Instance { get; set; }
        protected UserSettings()
        {

        }

        public static UserSettings GetInstance()
        {
            if (Instance == null)
                Instance = new UserSettings();
            return Instance;
        }


        public User CurrentUser { get; set; }
        public Group[] Groups { get; set; }
        public Role[] Roles { get; set; }
        public string JWT { get; set; }
        public string SignalrConnectionId { get; set; }
        public string OsVersion { get; set; }
        public bool FullSupport { get; set; }

    }
}
