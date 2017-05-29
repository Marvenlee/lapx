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
using System.Windows.Shapes;
using System.IO.Ports;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace LapX
{
	/// <summary>
	/// Interaction logic for OptionsWindow.xaml
	/// </summary>

	public partial class OptionsWindow : WindowBase
	{
		#region General Tab Properties

		public ObservableCollection<TextValue> LaneComboItems
		{
			get { return _LaneComboItems; }
			set { SetProperty(ref _LaneComboItems, value, "LaneComboItems"); }
		}

		ObservableCollection<TextValue> _LaneComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="1", Value=1},
			new TextValue {Text="2", Value=2},
			new TextValue {Text="3", Value=3},
			new TextValue {Text="4", Value=4},
			new TextValue {Text="5", Value=5},
			new TextValue {Text="6", Value=6},
		};

		public int LaneCount
		{
			get { return _LaneCount; }
			set { SetProperty(ref _LaneCount, value, "LaneCount"); }
		}
		int _LaneCount;

		public long MinimumLapTime
		{
			get { return _MinimumLapTime; }
			set { SetProperty(ref _MinimumLapTime, value, "MinimumLapTime"); }
		}
		long _MinimumLapTime;



		public long PreRaceDelay
		{
			get { return _PreRaceDelay; }
			set { SetProperty(ref _PreRaceDelay, value, "PreRaceDelay"); }
		}
		long _PreRaceDelay;

		

		
		public ObservableCollection<TextValue> JumpStartComboItems
		{
			get { return _JumpStartComboItems; }
			set { SetProperty(ref _JumpStartComboItems, value, "JumpStartComboItems"); }
		}

		ObservableCollection<TextValue> _JumpStartComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="Count Lap", Value=(int)JumpStart.CountLap},
			new TextValue {Text="Ignore Lap", Value=(int)JumpStart.IgnoreLap}
		};

		public int JumpStartAction
		{
			get { return _JumpStartAction; }
			set { SetProperty(ref _JumpStartAction, value, "JumpStartAction"); }
		}
		int _JumpStartAction;
		

		public int SamplingInterval
		{
			get { return _SamplingInterval; }
			set { SetProperty(ref _SamplingInterval, value, "SamplingInterval"); }
		}
		int _SamplingInterval;

		public bool ShowStatistics
		{
			get { return _ShowStatistics; }
			set { SetProperty(ref _ShowStatistics, value, "ShowStatistics"); }
		}
		bool _ShowStatistics;


		#endregion



		#region Sensor properties

		public int SensorType
		{
			get { return _SensorType; }
			set { SetProperty(ref _SensorType, value, "SensorType"); }
		}
		int _SensorType;


		// Scalextric RMS

		public string ScalexRMSComPortName
		{
			get { return _ScalexRMSComPortName; }
			set { SetProperty(ref _ScalexRMSComPortName, value, "ScalexRMSComPortName"); }
		}
		string _ScalexRMSComPortName;

		public string[] SerialPortNames
		{
			get { return _SerialPortNames; }
			set { SetProperty(ref _SerialPortNames, value, "SerialPortNames"); }
		}

		string[] _SerialPortNames;

		public bool RMSUseInpOut32
		{
			get { return _RMSUseInpOut32; }
			set { SetProperty(ref _RMSUseInpOut32, value, "RMSUseInpOut32"); }
		}
		bool _RMSUseInpOut32;
		
		
		// Parallel Port

		public ObservableCollection<TextValue> ParIOPortComboItems
		{
			get { return _ParIOPortComboItems; }
			set { SetProperty(ref _ParIOPortComboItems, value, "ParIOPortComboItems"); }
		}

		ObservableCollection<TextValue> _ParIOPortComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="LPT1 - 378",  Value = 0x378},
			new TextValue {Text="LPT2 - 278", Value = 0x278}
		};

		public int ParIOPort
		{
			get { return _ParIOPort; }
			set { SetProperty(ref _ParIOPort, value, "ParIOPort"); }
		}
		int _ParIOPort;
		

		public bool ParallelPortInvertLogic
		{
			get { return _ParallelPortInvertLogic; }
			set { SetProperty(ref _ParallelPortInvertLogic, value, "ParallelPortInvertLogic"); }
		}
		bool _ParallelPortInvertLogic;


		public ObservableCollection<TextValue> ParPinComboItems
		{
			get { return _ParPinComboItems; }
			set { SetProperty(ref _ParPinComboItems, value, "ParPinComboItems"); }
		}

		ObservableCollection<TextValue> _ParPinComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="None", Value= -1},
			new TextValue {Text="Pin 10", Value=6},
			new TextValue {Text="Pin 11", Value=7},
			new TextValue {Text="Pin 12", Value=5},
			new TextValue {Text="Pin 13", Value=4},
			new TextValue {Text="Pin 15", Value=3},
		};

		public int Lane1ParPin
		{
			get { return _Lane1ParPin; }
			set { SetProperty(ref _Lane1ParPin, value, "Lane1ParPin"); }
		}
		int _Lane1ParPin;

		public int Lane2ParPin
		{
			get { return _Lane2ParPin; }
			set { SetProperty(ref _Lane2ParPin, value, "Lane2ParPin"); }
		}
		int _Lane2ParPin;

		public int Lane3ParPin
		{
			get { return _Lane3ParPin; }
			set { SetProperty(ref _Lane3ParPin, value, "Lane3ParPin"); }
		}
		int _Lane3ParPin;

		public int Lane4ParPin
		{
			get { return _Lane4ParPin; }
			set { SetProperty(ref _Lane4ParPin, value, "Lane4ParPin"); }
		}
		int _Lane4ParPin;

		public int Lane5ParPin
		{
			get { return _Lane5ParPin; }
			set { SetProperty(ref _Lane5ParPin, value, "Lane5ParPin"); }
		}
		int _Lane5ParPin;


		//Serial Port

		// FIXME:  Add checkboxes to control output state of pins 4 and 7.
		// Sensor could use these as a power source instead of relying
		// on the voltages floating and grounding the inputs to trigger
		// a lap.

		public ObservableCollection<TextValue> SerIOPortComboItems
		{
			get { return _SerIOPortComboItems; }
			set { SetProperty(ref _SerIOPortComboItems, value, "SerIOPortComboItems"); }
		}

		ObservableCollection<TextValue> _SerIOPortComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="COM1 - 3F8",  Value = 0x3f8},
			new TextValue {Text="COM2 - 2F8", Value = 0x2f8}
		};

		public int SerIOPort
		{
			get { return _SerIOPort; }
			set { SetProperty(ref _SerIOPort, value, "SerIOPort"); }
		}
		int _SerIOPort;



		public bool SerPortInvertLogic
		{
			get { return _SerPortInvertLogic; }
			set { SetProperty(ref _SerPortInvertLogic, value, "SerPortInvertLogic"); }
		}
		bool _SerPortInvertLogic;


		public ObservableCollection<TextValue> SerPinComboItems
		{
			get { return _SerPinComboItems; }
			set { SetProperty(ref _SerPinComboItems, value, "SerPinComboItems"); }
		}

		ObservableCollection<TextValue> _SerPinComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="None", Value= -1},
			new TextValue {Text="Pin 1", Value=7},
			new TextValue {Text="Pin 6", Value=5},
			new TextValue {Text="Pin 8", Value=4},
			new TextValue {Text="Pin 9", Value=6}
		};

		public int Lane1SerPin
		{
			get { return _Lane1SerPin; }
			set { SetProperty(ref _Lane1SerPin, value, "Lane1SerPin"); }
		}
		int _Lane1SerPin;

		public int Lane2SerPin
		{
			get { return _Lane2SerPin; }
			set { SetProperty(ref _Lane2SerPin, value, "Lane2SerPin"); }
		}
		int _Lane2SerPin;

		public int Lane3SerPin
		{
			get { return _Lane3SerPin; }
			set { SetProperty(ref _Lane3SerPin, value, "Lane3SerPin"); }
		}
		int _Lane3SerPin;

		public int Lane4SerPin
		{
			get { return _Lane4SerPin; }
			set { SetProperty(ref _Lane4SerPin, value, "Lane4SerPin"); }
		}
		int _Lane4SerPin;


		#endregion
		

		#region Sounds Tab properties

		public bool UseFastestLapSound
		{
			get { return _UseFastestLapSound; }
			set { SetProperty(ref _UseFastestLapSound, value, "UseFastestLapSound"); }
		}
		bool _UseFastestLapSound;

		public string FastestLapFileName
		{
			get { return _FastestLapFileName; }
			set { SetProperty(ref _FastestLapFileName, value, "FastestLapFileName"); }
		}
		string _FastestLapFileName;
		
		public bool UsePersonalBestSound
		{
			get { return _UsePersonalBestSound; }
			set { SetProperty(ref _UsePersonalBestSound, value, "UsePersonalBestSound"); }
		}
		bool _UsePersonalBestSound;

		public string PersonalBestFileName
		{
			get { return _PersonalBestFileName; }
			set { SetProperty(ref _PersonalBestFileName, value, "PersonalBestFileName"); }
		}
		string _PersonalBestFileName;
		
		public bool UseDetectedLapSound
		{
			get { return _UseDetectedLapSound; }
			set { SetProperty(ref _UseDetectedLapSound, value, "UseDetectedLapSound"); }
		}
		bool _UseDetectedLapSound;

		public string DetectedLapFileName
		{
			get { return _DetectedLapFileName; }
			set { SetProperty(ref _DetectedLapFileName, value, "DetectedLapFileName"); }
		}
		string _DetectedLapFileName;
		
		public bool UseRedLightSound
		{
			get { return _UseRedLightSound; }
			set { SetProperty(ref _UseRedLightSound, value, "UseRedLightSound"); }
		}
		bool _UseRedLightSound;

		public string RedLightFileName
		{
			get { return _RedLightFileName; }
			set { SetProperty(ref _RedLightFileName, value, "RedLightFileName"); }
		}
		string _RedLightFileName;
		
		public bool UseRaceStartSound
		{
			get { return _UseRaceStartSound; }
			set { SetProperty(ref _UseRaceStartSound, value, "UseRaceStartSound"); }
		}
		bool _UseRaceStartSound;

		public string RaceStartFileName
		{
			get { return _RaceStartFileName; }
			set { SetProperty(ref _RaceStartFileName, value, "RaceStartFileName"); }
		}
		string _RaceStartFileName;
		
		public bool UseRaceFinishedSound
		{
			get { return _UseRaceFinishedSound; }
			set { SetProperty(ref _UseRaceFinishedSound, value, "UseRaceFinishedSound"); }
		}
		bool _UseRaceFinishedSound;

		public string RaceFinishedFileName
		{
			get { return _RaceFinishedFileName; }
			set { SetProperty(ref _RaceFinishedFileName, value, "RaceFinishedFileName"); }
		}
		string _RaceFinishedFileName;

		#endregion
		
		#region Display Tab properties
		
		public ObservableCollection<ColorValue> ColorComboItems
		{
			get { return _ColorComboItems; }
			set { SetProperty(ref _ColorComboItems, value, "ColorComboItems"); }
		}

		ObservableCollection<ColorValue> _ColorComboItems = new
			ObservableCollection<ColorValue>()
		{
			new ColorValue {Text="Red", Value=0, Color = Colors.Red},
			new ColorValue {Text="Orange", Value=1, Color = Colors.Orange},
			new ColorValue {Text="Yellow", Value=2, Color = Colors.Yellow},
			new ColorValue {Text="Green", Value=3, Color = Colors.Green},
			new ColorValue {Text="Blue", Value=4, Color = Colors.Blue},
			new ColorValue {Text="Violet", Value=5, Color = Colors.Violet},
			new ColorValue {Text="Black", Value=6, Color = Colors.DimGray},
			new ColorValue {Text="White", Value=7, Color = Colors.WhiteSmoke}
		};

		public Color Lane1Color
		{
			get { return _Lane1Color; }
			set { SetProperty(ref _Lane1Color, value, "Lane1Color"); }
		}
		Color _Lane1Color;

		public Color Lane2Color
		{
			get { return _Lane2Color; }
			set { SetProperty(ref _Lane2Color, value, "Lane2Color"); }
		}
		Color _Lane2Color;

		public Color Lane3Color
		{
			get { return _Lane3Color; }
			set { SetProperty(ref _Lane3Color, value, "Lane3Color"); }
		}
		Color _Lane3Color;

		public Color Lane4Color
		{
			get { return _Lane4Color; }
			set { SetProperty(ref _Lane4Color, value, "Lane4Color"); }
		}
		Color _Lane4Color;

		public Color Lane5Color
		{
			get { return _Lane5Color; }
			set { SetProperty(ref _Lane5Color, value, "Lane5Color"); }
		}
		Color _Lane5Color;

		public Color Lane6Color
		{
			get { return _Lane6Color; }
			set { SetProperty(ref _Lane6Color, value, "Lane6Color"); }
		}
		Color _Lane6Color;

		public ObservableCollection<TextValue> LaneStyleComboItems
		{
			get { return _LaneStyleComboItems; }
			set { SetProperty(ref _LaneStyleComboItems, value, "LaneStyleComboItems"); }
		}

		ObservableCollection<TextValue> _LaneStyleComboItems = new
			ObservableCollection<TextValue>()
		{
			new TextValue {Text="Standard", Value= 0},
			new TextValue {Text="Plain Wide", Value=1},
			new TextValue {Text="Glossy", Value=2},
			new TextValue {Text="Glossy Wide", Value=3},
			new TextValue {Text="Twilight", Value=4},
			new TextValue {Text="Red Sunset", Value=5},
			new TextValue {Text="Pale Moonlight", Value=6},
			new TextValue {Text="Standard 2", Value=7}
		};

		public int LaneStyle		// LaneStyle to hold dummy LaneItem example.
		{
			get { return _LaneStyle; }
			set { SetProperty(ref _LaneStyle, value, "LaneStyle"); }
		}
		int _LaneStyle;

		public ObservableCollection<LaneItem> LaneCollection
		{
			get { return _LaneCollection; }
		}
		ObservableCollection<LaneItem> _LaneCollection = new
			ObservableCollection<LaneItem>();

		#endregion


		public OptionsWindow()
		{
			InitializeComponent();
			DataContext = this;
			
			bool found = false;

			// Initialize ScalexRMSPortName to an item in above list.


			SerialPortNames = SerialPort.GetPortNames();
			
			foreach (String s in SerialPortNames)
			{
				if (s == ScalexRMSComPortName)
				{
					found = true;
					break;
				}
			}

			if (!found && SerialPortNames.Length > 0)
				ScalexRMSComPortName = SerialPortNames[0];



			// FIXME:
			// string[] = LaneViewer.GetStyles();   store as a string.
			// LaneViewer.SetStyle()
			
			LaneItem li = new LaneItem();
			li.LaneColor = Colors.Blue;
			li.Lap = "43";
			li.Position = "1";
			li.PositionSuffix = "st";
			li.LastLapTime = "7.891";
			li.BestLapTime = "3.142";
			li.LapType = LapType.NoDetect;
			li.Finished = false;
			LaneCollection.Add(li);
		}
		
		
		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			if (Validation.GetHasError(MinimumLapTimeTextbox) ||
				Validation.GetHasError(PreRaceDelayTextbox))
			{
				MessageBox.Show("Correct invalid fields befoe continuing", "LapX",
								MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}

			if (SensorType == (int)LapX.SensorType.ScalextricRMS
				&& (ScalexRMSComPortName == null || ScalexRMSComPortName == ""))
			{
				MessageBox.Show("No serial port selected", "LapX",
								MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}

			DialogResult = true;
		}
		

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
