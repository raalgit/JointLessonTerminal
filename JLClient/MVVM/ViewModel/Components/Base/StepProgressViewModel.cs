using JLClient.Core.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLClient.MVVM.ViewModel.Components.Base
{
    public class StepProgressViewModel : ObservableObject
    {
        public List<string> Steps { get { return steps; } set { steps = value; OnPropsChanged("Steps"); } }
        public List<string> steps;

        public int Progress { get { return progress; } set { progress = value; OnPropsChanged("Progress"); } }
        public int progress;

        private int stepLength;

        public StepProgressViewModel()
        {
            Progress = 0;
            stepLength = 0;
            Steps = new List<string>();
        }

        public bool Init(List<string> steps, int currentProcess)
        {
            if (steps == null) return false;

            stepLength = 100 / steps.Count - 1;
            if (currentProcess < 0) return false;
            
            Steps = steps;
            Progress = currentProcess;

            return true;
        }

        public void SetProcess(int currentStep)
        {
            if (currentStep < 0) return;

            Progress = currentStep;
        }

        public void SetStep(int step)
        {
            if (step < 1 || step > steps.Count)
                return;

            Progress = stepLength * step;
        }
    }
}
