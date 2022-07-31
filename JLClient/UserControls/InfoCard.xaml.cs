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

namespace JLClient.UserControls
{
    /// <summary>
    /// Логика взаимодействия для InfoCard.xaml
    /// </summary>
    public partial class InfoCard : UserControl
    {
        public InfoCard()
        {
            InitializeComponent();
        }

        public string Title 
        { 
            get 
            {
                return (string)GetValue(TitleProp);
            }
            set
            {
                SetValue(TitleProp, value);
            }
        }

        public string Description
        {
            get
            {
                return (string)GetValue(DescProp);
            }
            set
            {
                SetValue(DescProp, value);
            }
        }

        public static readonly DependencyProperty TitleProp = DependencyProperty.Register("Title", typeof(string), typeof(InfoCard));
        public static readonly DependencyProperty DescProp = DependencyProperty.Register("Description", typeof(string), typeof(InfoCard));
    }
}
