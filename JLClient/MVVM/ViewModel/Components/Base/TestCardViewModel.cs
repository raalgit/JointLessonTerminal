using JLClient.Core.Arguments;
using JLClient.Core.Arguments.InnerModels;
using JLClient.Core.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Base
{
    public class TestCardViewModel : ObservableObject
    {
        public bool IsLoading { get { return isLoading; } set { isLoading = value; OnPropsChanged("IsLoading"); } }
        private bool isLoading;

        private TestMod mod;

        public event EventHandler OnTestComplete;

        public RelayCommand CompleteCommand { get; set; }

        public TestCardViewModel(TestMod currentMode) 
        {
            mod = currentMode;
            CompleteCommand = new RelayCommand(_ => Complete());
        }

        public void Init()
        {
            
        }

        private void Complete()
        {
            OnTestComplete?.Invoke(this, new TestCompleteArgument()
            {
                Question = "12312412",
                Answeres = new List<string>() {
                    "123",
                    "1212"
                },
                IsSuccess = true
            });
        }
    }
}
