using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JLClient.Core.VisualComponents
{
    public class ButtonWithImage : DependencyObject
    {
		public static DependencyProperty IconProperty = 
			DependencyProperty.Register(
				"Icon",
				typeof(FrameworkElement),
				typeof(ButtonWithImage),
				new FrameworkPropertyMetadata()
				);

		static ButtonWithImage()
		{

		}

		public static FrameworkElement GetIcon(DependencyObject obj)
		{
			return (FrameworkElement)obj.GetValue(IconProperty);
		}

		public static void SetIcon(DependencyObject obj, FrameworkElement value)
		{
			obj.SetValue(IconProperty, value);
		}
	}
}
