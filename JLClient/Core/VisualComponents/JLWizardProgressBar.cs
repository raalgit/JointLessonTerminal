using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JLClient.Core.VisualComponents
{
    public class JLWizardProgressBar : ItemsControl
    {
        public static DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress",
                typeof(int),
                typeof(JLWizardProgressBar),
                new FrameworkPropertyMetadata(0, null, CoerceProgress));

        private static object CoerceProgress(DependencyObject target, object value)
        {
            JLWizardProgressBar wizardProgressBar = (JLWizardProgressBar)target;
            int progress = (int)value;

            if (progress < 0) progress = 0;
            else if (progress > 100) progress = 100;
            return progress;
        }


        static JLWizardProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(JLWizardProgressBar), 
                new FrameworkPropertyMetadata(typeof(JLWizardProgressBar))
                );
        }

        public JLWizardProgressBar()
        {

        }

        public int Progress
        {
            get { return (int)base.GetValue(ProgressProperty); }
            set { base.SetValue(ProgressProperty, value); }
        }

    }
}
