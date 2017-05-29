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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace LapX
{
	/// <summary>
	/// Interaction logic for StartLightControl.xaml
	/// </summary>
	public partial class StartLightControl : UserControl
	{
		public static readonly DependencyProperty LightsProperty
			= DependencyProperty.Register("Lights",
			typeof(int), typeof(StartLightControl),
			new PropertyMetadata(LightsChangedCallback));
		
		public int Lights
		{	
			get { return (int)GetValue(LightsProperty); }
			set { if (value != Lights) SetValue(LightsProperty, value); } 
		}


		// Light 1

		public static readonly DependencyProperty Light1InProperty
			= DependencyProperty.Register("Light1In",
			typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xe5, 0x77, 0x77),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light1In
		{
			get { return (Color)GetValue(Light1InProperty); }
			set { if (value != Light1In) SetValue(Light1InProperty, value); }
		}


		public static readonly DependencyProperty Light1OutProperty
			= DependencyProperty.Register("Light1Out",
			typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xb6, 0x00, 0x00),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light1Out
		{
			get { return (Color)GetValue(Light1OutProperty); }
			set { if (value != Light1Out) SetValue(Light1OutProperty, value); }
		}


		// Light 2
		
		public static readonly DependencyProperty Light2InProperty
			= DependencyProperty.Register("Light2In",
			typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xe5, 0x77, 0x77),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light2In
		{
			get { return (Color)GetValue(Light2InProperty); }
			set { if (value != Light2In) SetValue(Light2InProperty, value); }
		}


		public static readonly DependencyProperty Light2OutProperty
					= DependencyProperty.Register("Light2Out",
					typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xb6, 0x00, 0x00),
							FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light2Out
		{
			get { return (Color)GetValue(Light2OutProperty); }
			set { if (value != Light2Out) SetValue(Light2OutProperty, value); }
		}


		// Light 3
		
		public static readonly DependencyProperty Light3InProperty
			= DependencyProperty.Register("Light3In",
			typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xe5, 0x77, 0x77),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light3In
		{
			get { return (Color)GetValue(Light3InProperty); }
			set { if (value != Light3In) SetValue(Light3InProperty, value); }
		}


		public static readonly DependencyProperty Light3OutProperty
					= DependencyProperty.Register("Light3Out",
					typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xb6, 0x00, 0x00),
							FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light3Out
		{
			get { return (Color)GetValue(Light3OutProperty); }
			set { if (value != Light3Out) SetValue(Light3OutProperty, value); }
		}

		
		// Light 4

		public static readonly DependencyProperty Light4InProperty
			= DependencyProperty.Register("Light4In",
			typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xe5, 0x77, 0x77),
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light4In
		{
			get { return (Color)GetValue(Light4InProperty); }
			set { if (value != Light4In) SetValue(Light4InProperty, value); }
		}


		public static readonly DependencyProperty Light4OutProperty
					= DependencyProperty.Register("Light4Out",
					typeof(Color), typeof(StartLightControl), new FrameworkPropertyMetadata(Color.FromArgb(0xff, 0xb6, 0x00, 0x00),
							FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public Color Light4Out
		{
			get { return (Color)GetValue(Light4OutProperty); }
			set { if (value != Light4Out) SetValue(Light4OutProperty, value); }
		}

		
		DoubleAnimation FadeOutAnimation;
		DoubleAnimation FadeInAnimation;

		ColorAnimation LightInAnimation;
		ColorAnimation LightOutAnimation;

		ColorAnimation LightInFadeAnimation;
		ColorAnimation LightOutFadeAnimation;

		
		// StartLightControl():  FIXME:  Try adding a boolean visibility animation to see if it improves performance.

		public StartLightControl()
		{
			InitializeComponent();
			
			FadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.Parse("0:0:2"));
			FadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.Parse("0:0:1"));
						
			LightInAnimation = new ColorAnimation(Color.FromArgb(0xff, 0x7E, 0x20, 0x20), Color.FromArgb(0xff, 0xe5, 0x77, 0x77),
				TimeSpan.Parse("0:0:0.5"), FillBehavior.HoldEnd);

			LightOutAnimation = new ColorAnimation(Color.FromArgb(0xff, 0x31, 0x00, 0x00), Color.FromArgb(0xff, 0xb6, 0x00, 0x00),
				TimeSpan.Parse("0:0:0.5"), FillBehavior.HoldEnd);


			LightInFadeAnimation = new ColorAnimation(Color.FromArgb(0xff, 0xe5, 0x77, 0x77), Color.FromArgb(0xff, 0x7E, 0x20, 0x20),
				TimeSpan.Parse("0:0:0.1"), FillBehavior.HoldEnd);

			LightOutFadeAnimation = new ColorAnimation( Color.FromArgb(0xff, 0xb6, 0x00, 0x00), Color.FromArgb(0xff, 0x31, 0x00, 0x00),
				TimeSpan.Parse("0:0:0.1"), FillBehavior.HoldEnd);

			Light1In = Light2In = Light3In = Light4In = Color.FromArgb(0xff, 0x7e, 0x20, 0x20);
			Light1Out = Light2Out = Light3Out = Light4Out = Color.FromArgb(0xff, 0x31, 0x00, 0x00);

		}


		// LightsChangedCallback();
		// Updates the control visibility and the state of each light

		static void LightsChangedCallback(DependencyObject dobj,
			DependencyPropertyChangedEventArgs args)
		{
			StartLightControl slc = dobj as StartLightControl;
			
			switch (slc.Lights)
			{
				case -1:	// Fade display out
					slc.LightRect.BeginAnimation(OpacityProperty, slc.FadeOutAnimation);
					break;
				case 0:		// Lights off - Go Go Go!
					slc.BeginAnimation(Light1InProperty, slc.LightInFadeAnimation);
					slc.BeginAnimation(Light1OutProperty, slc.LightOutFadeAnimation);

					slc.BeginAnimation(Light2InProperty, slc.LightInFadeAnimation);
					slc.BeginAnimation(Light2OutProperty, slc.LightOutFadeAnimation);

					slc.BeginAnimation(Light3InProperty, slc.LightInFadeAnimation);
					slc.BeginAnimation(Light3OutProperty, slc.LightOutFadeAnimation);
		
					slc.BeginAnimation(Light4InProperty, slc.LightInFadeAnimation);
					slc.BeginAnimation(Light4OutProperty, slc.LightOutFadeAnimation);

					break;
				case 1:		// 4 light
					slc.BeginAnimation(Light4InProperty, slc.LightInAnimation);
					slc.BeginAnimation(Light4OutProperty, slc.LightOutAnimation);
					break;
				case 2:		// 3 light
					slc.BeginAnimation(Light3InProperty, slc.LightInAnimation);
					slc.BeginAnimation(Light3OutProperty, slc.LightOutAnimation);
					break;

				case 3:		// 2 light
					slc.BeginAnimation(Light2InProperty, slc.LightInAnimation);
					slc.BeginAnimation(Light2OutProperty, slc.LightOutAnimation);
					break;

				case 4:		// 1 light
					slc.BeginAnimation(Light1InProperty, slc.LightInAnimation);
					slc.BeginAnimation(Light1OutProperty, slc.LightOutAnimation);
					break;

				case 5:		// Fade display in
					slc.Light1In = slc.Light2In = slc.Light3In = slc.Light4In = Color.FromArgb(0xff, 0x7E, 0x20, 0x20);
					slc.Light1Out = slc.Light2Out = slc.Light3Out = slc.Light4Out = Color.FromArgb(0xff, 0x31, 0x00, 0x00);

					slc.BeginAnimation(Light1InProperty, null);
					slc.BeginAnimation(Light1OutProperty, null);

					slc.BeginAnimation(Light2InProperty, null);
					slc.BeginAnimation(Light2OutProperty, null);
		
					slc.BeginAnimation(Light3InProperty, null);
					slc.BeginAnimation(Light3OutProperty, null);
					
					slc.BeginAnimation(Light4InProperty, null);
					slc.BeginAnimation(Light4OutProperty, null);

					slc.LightRect.BeginAnimation(OpacityProperty, slc.FadeInAnimation);
					break;

			
				default:	// For bad values of Lights property, show the lights
					slc.Light1In = slc.Light2In = slc.Light3In = slc.Light4In = Color.FromArgb(0xff, 0x7E, 0x20, 0x20);
					slc.Light1Out = slc.Light2Out = slc.Light3Out = slc.Light4Out = Color.FromArgb(0xff, 0x31, 0x00, 0x00);
					slc.LightRect.Opacity = 1.0;
					break;
			}
		}
	}
}
