using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;


namespace LapX
{
    public class LaneView : ItemsControl
	{
		public static readonly DependencyProperty LaneStyleProperty
			= DependencyProperty.Register("LaneStyle",
			typeof(int), typeof(LaneView),
			new PropertyMetadata(LaneStyleChangedCallback));

		public int LaneStyle
		{
			get { return (int)GetValue(LaneStyleProperty); }
			set { if (value != LaneStyle) SetValue(LaneStyleProperty, value); }
		}


        static LaneView()
        {
			DefaultStyleKeyProperty.OverrideMetadata (typeof(LaneView), new FrameworkPropertyMetadata(typeof(LaneView)));
		}
		

		static void LaneStyleChangedCallback(DependencyObject dobj,
							DependencyPropertyChangedEventArgs args)
		{
			LaneView lv = dobj as LaneView;
			
			// FIXME:  Replace int with a string, name of template/resource?

			switch (lv.LaneStyle)
			{
				case 0:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateStandard");
					break;

				case 1:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateThin");
					break;

				case 2:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateGlossy");
					break;

				case 3:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateGlossyThin");
					break;
				
				case 4:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateTwilight");
					break;
				
				case 5:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateSunset");
					break;

				case 6:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateMoonlight");
					break;

				case 7:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateStandard2");
					break;

				default:
					lv.ItemTemplate
						= (DataTemplate)lv.FindResource("LVTemplateStandard");
					break;
			}
		}

    }
}


