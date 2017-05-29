using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace LapX
{
	[ValueConversion(typeof(decimal), typeof(string))]
	public class DurationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			if (value != null)
			{
				decimal price = (decimal)((long)value) / 1000;
				return price.ToString();
			}
			else
				return "INVALID";
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			CultureInfo culture)
		{
			string price = (string)value;

			decimal result;

			if (Decimal.TryParse(price, NumberStyles.Any, culture, out result))
			{
				return (long)(Decimal.Floor(result * 1000));
			}
			
			return DependencyProperty.UnsetValue;
			
		}
	}
}
