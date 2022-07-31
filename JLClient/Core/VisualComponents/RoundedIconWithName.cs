using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JLClient.Core.VisualComponents
{
    public class RoundedIconWithName : DependencyObject
    {
        public static DependencyProperty FirstLetterProperty =
            DependencyProperty.Register(
                "FirstLetter",
                typeof(FrameworkElement),
                typeof(RoundedIconWithName),
                new FrameworkPropertyMetadata()
                );

        public static DependencyProperty SecondLetterProperty =
            DependencyProperty.Register(
                "SecondLetter",
                typeof(FrameworkElement),
                typeof(RoundedIconWithName),
                new FrameworkPropertyMetadata()
                );

        static RoundedIconWithName()
        {

        }

        public static FrameworkElement GetFirstLetter(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(FirstLetterProperty);
        }

        public static void SetFirstLetter(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(FirstLetterProperty, value);
        }

        public static FrameworkElement GetSecondLetter(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(SecondLetterProperty);
        }

        public static void SetSecondLetter(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(SecondLetterProperty, value);
        }
    }
}
