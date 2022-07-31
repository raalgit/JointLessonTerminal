using JLClient.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Profile.Components
{
    public class StatisticCardViewModel : ObservableObject
    {
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;

        public StatisticCardViewModel()
        {

        }
    }
}
