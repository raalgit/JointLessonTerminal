using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JLClient.MVVM.View.Components.Editor.Components
{
    /// <summary>
    /// Логика взаимодействия для ManualTreeCard.xaml
    /// </summary>
    public partial class ManualTreeCard : UserControl
    {
        public ManualTreeCard()
        {
            InitializeComponent();
            ucWindow.MaxHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 100;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                var buttonParent = button.Parent;
                if (buttonParent != null && buttonParent is StackPanel nodePanel)
                {
                    nodePanel.ContextMenu.DataContext = nodePanel.DataContext;
                    nodePanel.ContextMenu.IsOpen = true;
                }
            }
        }
    }
}
