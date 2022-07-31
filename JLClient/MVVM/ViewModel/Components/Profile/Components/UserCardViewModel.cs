using JLClient.Core.Observer;
using JLClient.MVVM.Model.Components.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Profile.Components
{
    public class UserCardViewModel : ObservableObject
    {
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;

        public string FIO { get { return fio; } set { fio = value; OnPropsChanged("FIO"); } }
        private string fio;

        public List<string> UserRoles { get { return userRoles; } set { userRoles = value; OnPropsChanged("UserRoles"); } }
        private List<string> userRoles;

        public UserCardViewModel()
        {
            IsLoading = true;
        }

        public void Init()
        {
            var handler = new UserCardHandler();
            var user = handler.GetUserData();

            FIO = handler.GetFIO(user.User);
            UserRoles = handler.GetRolesAsStringList(user.Roles);
            IsLoading = false;
        }
    }
}
