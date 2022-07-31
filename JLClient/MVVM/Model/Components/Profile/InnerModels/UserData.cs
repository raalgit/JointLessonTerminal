using JLClient.Core.PersistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.Model.Components.Profile.InnerModels
{
    public class UserData
    {
        public User User { get; set; }
        public Role[] Roles { get; set; }
    }
}
