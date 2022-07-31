using JLClient.Core.Arguments;
using JLClient.Core.Arguments.InnerModels;
using JLClient.Core.Observer;
using JLClient.MVVM.ViewModel.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JLClient.Core.Manual
{
    [Serializable]
    public class PageTest : ObservableObject
    {
        #region Экспорт данные
        public string Question { get { return question; } set { question = value; OnPropsChanged("Question"); } }
        #endregion

        #region Команды
        [JsonIgnore] public RelayCommand ShowTestCommand { get; set; }
        #endregion

        #region Поля
        private string question;
        private TestCardViewModel cardVM;
        private bool isOpen = false;
        #endregion

        #region События
        public event EventHandler OnTestDisplay;
        #endregion

        public PageTest() 
        {
            ShowTestCommand = new RelayCommand(_ => Show());
        }

        public void CreateNew()
        {
            if (isOpen) return;

            cardVM = new TestCardViewModel(TestMod.DEVELOPMENT);
            cardVM.OnTestComplete += CardVM_OnTestComplete;
            isOpen = true;

            OnTestDisplay?.Invoke(this, new OnStartTestArgument()
            {
                cardVM = cardVM,
                Show = true
            });
        }

        private void CardVM_OnTestComplete(object sender, EventArgs e)
        {
            if (e is TestCompleteArgument arg)
            {
                // получение и сохранение
                Question = arg.Question;

                isOpen = false;
                cardVM = null;
                OnTestDisplay?.Invoke(this, new OnStartTestArgument()
                {
                    cardVM = cardVM,
                    Show = false
                });
            }
        }

        private void Show()
        {
            cardVM = new TestCardViewModel(TestMod.ANSWERE);
            cardVM.OnTestComplete += CardVM_OnTestComplete;
            isOpen = true;

            OnTestDisplay?.Invoke(this, new OnStartTestArgument()
            {
                cardVM = cardVM,
                Show = true
            });
        }
    }
}
