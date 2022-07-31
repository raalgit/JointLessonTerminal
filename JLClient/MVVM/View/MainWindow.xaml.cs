using JLClient.MVVM.View.Base;
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

namespace JLClient.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StyleThemes currentTheme;
        private ImageSource backgroundImage;
        private ImageSource backgroundDarkImage;

        public MainWindow()
        {
            InitializeComponent();
            //backgroundImage = new BitmapImage(new Uri("../../Res/Images/background.png", UriKind.Relative));
            //backgroundDarkImage = new BitmapImage(new Uri("../../Res/Images/background-dark.png", UriKind.Relative));
        }

        private void Menu_ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_AboutBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void setTheme(StyleThemes theme)
        {
            if (theme != currentTheme)
            {
                currentTheme = theme;
                string newTheme = String.Empty;
                string originalTheme = String.Empty;
                switch (theme)
                {
                    case StyleThemes.DARK:
                        newTheme = "dark";
                        originalTheme = "base";
                        break;
                    case StyleThemes.BASE:
                        newTheme = "base";
                        originalTheme = "dark";
                        break;
                    default:
                        return;
                }

                Application.Current.Resources.BeginInit();

                var uriColorNewTheme = new Uri($"../../Theme/colors-{newTheme}.xaml", UriKind.Relative);
                var uriColorOriginalTheme = new Uri($"../../Theme/colors-{originalTheme}.xaml", UriKind.Relative);

                ResourceDictionary resourceColorDict = Application.LoadComponent(uriColorNewTheme) as ResourceDictionary;
                ResourceDictionary originalColorDict = Application.LoadComponent(uriColorOriginalTheme) as ResourceDictionary;

                Application.Current.Resources.MergedDictionaries.Add(resourceColorDict);
                Application.Current.Resources.MergedDictionaries.Remove(originalColorDict);

                Application.Current.Resources.EndInit();
            }
        }

        private void Menu_Styles_Base_Click(object sender, RoutedEventArgs e)
        {
            setTheme(StyleThemes.BASE);
        }

        private void Menu_Styles_Dark_Click(object sender, RoutedEventArgs e)
        {
            setTheme(StyleThemes.DARK);
        }

        private void Menu_SettingsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_EditorOffline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_EditorOnline_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
