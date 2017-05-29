using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;


namespace LapX
{
	public class DurationRule : ValidationRule
	{
		public decimal Min
		{
			get { return min; }
			set { min = value; }
		}
		decimal min = 0;


		public decimal Max
		{
			get { return max; }
			set { max = value; }
		}
		decimal max = 10;


		public override ValidationResult Validate(
						object value, CultureInfo culture)
		{
			Decimal number;

			if (!Decimal.TryParse((string)value, NumberStyles.Any, culture, out number))
				return new ValidationResult(false, "Invalid number format");
			

			if (number < min || number > max)
			{
				return new ValidationResult(
				false,
				string.Format("Number out of range ({0}-{1})", min, max));
			}


			return ValidationResult.ValidResult; // static valid result
		}
	}
}

