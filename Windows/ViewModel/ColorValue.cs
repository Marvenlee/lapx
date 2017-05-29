using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace LapX
{
	public class ColorValue
	{
		public string Text { get; set; }

		// FIXME: Remove Value field?
		public int Value { get; set; }
		public Color Color { get; set; }
	}
}
