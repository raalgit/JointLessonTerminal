using JLClient.Core.Arguments;
using JLClient.Core.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace JLClient.Core.Manual
{
    [Serializable]
    public class ManualData : ObservableObject
    {
        #region Экспорт данные
        public bool ManualIsUploaded { get { return manualIsUploaded; } set { manualIsUploaded = value; if (manualIsUploaded) ChangeStep(3); } }
        public bool ManualIsComputed { get { return manualIsComputed; } set { manualIsComputed = value; if (manualIsComputed) ChangeStep(2); } }
        public bool ManualIsCreated { get { return manualIsCreated; } set { manualIsCreated = value; if (manualIsCreated) ChangeStep(1); } }
        public string Name { get { return _discipline; } set { _discipline = value; OnPropsChanged("Discipline"); } }
        public Author Author { get { return _author; } set { _author = value; OnPropsChanged("Author"); } }
        public MaterialDate MaterialDate { get; set; }
        public ManualNode[] ManualRoot { 
            get { return manualRoot; } 
            set { 
                manualRoot = value; 
                if (manualRoot.Length > 0)
                {
                    manualRoot[0].OnTestDisplay += RootNode_OnTestStart;
                }

                OnPropsChanged("ManualRoot"); 
            } 
        }
        #endregion

        #region Поля
        private bool manualIsUploaded;
        private bool manualIsComputed;
        private bool manualIsCreated;
        private string _discipline;
        private Author _author;
        private ManualNode[] manualRoot;
        #endregion

        #region События
        public event EventHandler OnTestDisplay;
        public event EventHandler OnProcessChange;
        #endregion

        public ManualData()
        {

        }

        public void Init()
        {
            var rootNode = new ManualNode()
            {
                access = BlockAccess.READ_ALL,
                title = "Учебный материал"
            };

            Name = "Тест";

            rootNode.Init();
            rootNode.OnTestDisplay += RootNode_OnTestStart;
            ManualRoot = new ManualNode[1] { rootNode };

            manualIsCreated = true;
            ManualIsUploaded = false;
            ManualIsComputed = false;
        }

        private void RootNode_OnTestStart(object sender, EventArgs e)
        {
            OnTestDisplay?.Invoke(sender, e);
        }

        public void SetStep(int step)
        {
            ManualIsCreated = false;
            ManualIsUploaded = false;
            ManualIsComputed = false;
            switch (step)
            {
                case 1:
                    ManualIsCreated = true;
                    break;
                case 2:
                    ManualIsCreated = true;
                    ManualIsComputed = true;
                    break;
                case 3:
                    ManualIsCreated = true;
                    ManualIsComputed = true;
                    ManualIsUploaded = true;
                    break;
            }
        }

        public int GetStep()
        {
            if (ManualIsUploaded) return 3;
            else if (ManualIsComputed) return 2;
            else return 1;
        }

        private void ChangeStep(int step)
        {
            var stepArg = new ManualProcessChangeArgument();
            stepArg.Step = step;

            OnProcessChange?.Invoke(this, stepArg);
        }
    }
}
