using JLClient.Core.Arguments;
using JLClient.Core.Manual;
using JLClient.Core.Observer;
using JLClient.MVVM.ViewModel.Components.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Editor.Components
{
    public class ManualTreeViewModel : ObservableObject
    {
        public ManualData CurrentManualData { 
            get { return currentManualData; } 
            set { 
                currentManualData = value; 
                CurrentManualData.OnTestDisplay += CurrentManualData_OnTestStart; 
                OnManualDataChange?.Invoke(this, null);
                OnPropsChanged("CurrentManualData"); 
            } 
        }
        private ManualData currentManualData;

        public TestCardViewModel TestCardViewModel { get { return testCardViewModel; } set { testCardViewModel = value; OnPropsChanged("TestCardViewModel"); } }
        private TestCardViewModel testCardViewModel;

        public bool IsTestDisplaying { get { return isTestDisplaying; } set { isTestDisplaying = value; OnPropsChanged("IsTestDisplaying"); } }
        private bool isTestDisplaying;

        public event EventHandler OnManualDataChange;
        
        public ManualTreeViewModel()
        {

        }

        public void Init()
        {
            CurrentManualData = new ManualData();
            currentManualData.Init();
            IsTestDisplaying = false;
        }

        public void SetManual(ManualData manual)
        {
            CurrentManualData = manual;
            IsTestDisplaying = false;
        }

        private void CurrentManualData_OnTestStart(object sender, EventArgs e)
        {
            if (e is OnStartTestArgument arg)
            {
                IsTestDisplaying = arg.Show;
                TestCardViewModel = arg.cardVM;
            }
        }
    }
}
