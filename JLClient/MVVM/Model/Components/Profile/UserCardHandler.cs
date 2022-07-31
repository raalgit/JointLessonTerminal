using JLClient.Core.PersistModels;
using JLClient.Core.Settings;
using JLClient.MVVM.Model.Components.Profile.InnerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Profile
{
    public class UserCardHandler
    {
        public UserData GetUserData()
        {
            UserSettings user = UserSettings.GetInstance();
            return new UserData
            {
                User = user.CurrentUser ?? new User(),
                Roles = user.Roles ?? new Role[0],
            };
        }

        public string GetFIO(User user)
        {
            if (user == null) return "";

            return string.Join(" ", new string[3] { user.secondName, user.firstName, user.thirdName });
        }

        public List<string> GetRolesAsStringList(Role[] roles)
        {
            if (roles == null) return new List<string>();

            return roles.Select(x => x.displayName).ToList();
        }
    }
}
