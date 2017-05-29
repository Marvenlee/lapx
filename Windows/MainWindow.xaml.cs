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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using LapX.Properties;
using System.Media;
using System.Diagnostics;

namespace LapX
{
	/// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : WindowBase
	{
		#region Window properties

		public ObservableCollection<LaneItem> LaneCollection
		{
			get { return _LaneCollection; }
		}
		ObservableCollection<LaneItem> _LaneCollection = new ObservableCollection<LaneItem>();

		public string Clock
		{
			get { return clock; }
			set { this.SetProperty (ref clock, value, "Clock");}
		}
		string clock;

		public ObservableCollection<RaceValue> RaceModes
		{
			get { return _RaceModes; }
			set { SetProperty(ref _RaceModes, value, "RaceModes"); }
		}

		ObservableCollection<RaceValue> _RaceModes = new
			ObservableCollection<RaceValue>()
		{
			new RaceValue {Text="Free Practice", Value=0, Mode=Mode.Practice, MaxLaps=9999, MaxTime=0},
			new RaceValue {Text="5 Lap Race", Value=1, Mode=Mode.Race, MaxLaps=5, MaxTime=0},
			new RaceValue {Text="10 Lap Race", Value=2, Mode=Mode.Race, MaxLaps=10, MaxTime=0},
			new RaceValue {Text="20 Lap Race", Value=3, Mode=Mode.Race, MaxLaps=20, MaxTime=0},
			new RaceValue {Text="40 Lap Race", Value=4, Mode=Mode.Race, MaxLaps=40, MaxTime=0},
			new RaceValue {Text="2 Minute Race", Value=5, Mode=Mode.Endurance, MaxLaps=0, MaxTime=2},
			new RaceValue {Text="5 Minute Race", Value=5, Mode=Mode.Endurance, MaxLaps=0, MaxTime=5},
			new RaceValue {Text="10 Minute Race", Value=6, Mode=Mode.Endurance, MaxLaps=0, MaxTime=10},
			new RaceValue {Text="20 Minute Race", Value=7, Mode=Mode.Endurance, MaxLaps=0, MaxTime=20},
			new RaceValue {Text="30 Minute Race", Value=8, Mode=Mode.Endurance, MaxLaps=0, MaxTime=30}
		};
		
		public int RaceMode
		{
			get { return _RaceMode; }
			set { SetProperty(ref _RaceMode, value, "RaceMode"); }
		}
		int _RaceMode;


		public long SamplesPerSecond
		{
			get { return samplespersecond; }
			set { this.SetProperty(ref samplespersecond, value, "SamplesPerSecond"); }
		}
		long samplespersecond;

		public long PeakSample
		{
			get { return peaksample; }
			set { this.SetProperty(ref peaksample, value, "PeakSample"); }
		}
		long peaksample;

		public long SamplesOver10ms
		{
			get { return samplesover10ms; }
			set { this.SetProperty(ref samplesover10ms, value, "SamplesOver10ms"); }
		}
		long samplesover10ms;

		public long SamplesOver20ms
		{
			get { return samplesover20ms; }
			set { this.SetProperty(ref samplesover20ms, value, "SamplesOver20ms"); }
		}
		long samplesover20ms;


		public bool ShowStats
		{
			get { return showstats; }
			set { this.SetProperty(ref showstats, value, "ShowStats"); }
		}
		bool showstats;


		#endregion


		
		LapCounter LapCounter;
		Sounds Sounds;
		Color[] LaneColor = new Color[6];
		int LaneStyle;

		bool RaceStarted = false;
		int Lights;
		bool RaceWon = false;

		

		
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            
			LapCounter = new LapCounter();
			LapCounter.RefreshDelegate = UpdateDisplay;
			LapCounter.StopRaceDelegate = StopRaceDelegate;

			Sounds = new Sounds();
            LoadSettings();
			Lights = -1;

			InitializeLaneCollection();
        }


        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            StartStop();
        }
		

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
			Options();
		}

		
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Exit();
		}


		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.D1:
					LapCounter.IncrementLapCount(0);
					break;

				case Key.D2:
					LapCounter.IncrementLapCount(1);
					break;

				case Key.D3:
					LapCounter.IncrementLapCount(2);
					break;

				case Key.D4:
					LapCounter.IncrementLapCount(3);
					break;

				case Key.D5:
					LapCounter.IncrementLapCount(4);
					break;

				case Key.D6:
					LapCounter.IncrementLapCount(5);
					break;

				case Key.Up:
					if (RaceModeCombo.IsEnabled == true && RaceModeCombo.SelectedIndex > 0)
						RaceModeCombo.SelectedIndex--;
					break;

				case Key.Down:
					if (RaceModeCombo.IsEnabled == true && RaceModeCombo.SelectedIndex < 6)
						RaceModeCombo.SelectedIndex++;
					break;
			
				case Key.Space:
					StartStop();
					break;
			}
		}




		// Starts or stops a race and changes the state of the buttons in
		// this MainWindow

		private void StartStop()
		{
			if (RaceStarted == false)
			{
				InitializeLaneCollection();

				try
				{
					LapCounter.Mode = RaceModes[RaceMode].Mode;
					LapCounter.MaxLap = RaceModes[RaceMode].MaxLaps;
					LapCounter.MaxDuration = RaceModes[RaceMode].MaxTime * 60000;

					LapCounter.BeginRace();
					OptionsButton.IsEnabled = false;
					RaceModeCombo.IsEnabled = false;
					// StartStopButton.Content = "Stop";
					StartStopLabel.Content = "Stop";
					RaceStarted = true;
					RaceWon = false;
					Lights = -1;
				}
				catch
				{
					MessageBox.Show("Unable to start race", "LapX",
								MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
			else
			{
				LapCounter.CancelRace();
			}
		}




		// Exits LapX application, Cancels any active race and saves settings.
		
		void Exit()
		{
			if (RaceStarted == true) LapCounter.CancelRace();
			SaveSettings();
			Application.Current.Shutdown();
		}




		// Sets up the LaneCollection viewmodel.

		void InitializeLaneCollection()
		{
			int t;

			LaneCollection.Clear();
			
			for (t = 0; t < LapCounter.LaneCount; t++)
			{
				LaneItem li = new LaneItem();

				li.LaneColor = LaneColor[t];
				li.Lap = "---";
				li.Position = "-";
				li.PositionSuffix = "";
				li.LastLapTime = String.Format("{0:0}.{1:000}", 0, 0);
				li.BestLapTime = String.Format("{0:0}.{1:000}", 0, 0);
				li.LapType = LapType.NoDetect;
				li.Finished = false;

				LaneCollection.Add(li);
			}
		}




		// Callback.  updates clock, start lights and lap displays in response
		// to changes in the LapCounter.  Called on the thread of the
		// LapCounter,  so we have to invoke it on the context of the Dispatcher.
		//
		// Perhaps this code should be moved into a ViewModel or part of the
		// LaneViewer control.

		private void UpdateDisplay()
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
			{
				int t;
				RaceData rd;
				string[] OrdinalSuffix = { "", "st", "nd", "rd", "th", "th", "th", "th", "th" };

				rd = LapCounter.GetRaceState();

				Clock = String.Format("{0:0}:{1:00}", rd.ElapsedTime / 60000, (rd.ElapsedTime / 1000) % 60);

				for (t = 0; t < rd.LaneCount; t++)
				{
					if (rd.Lane[t].Lap >= 0)
						LaneCollection[t].Lap = rd.Lane[t].Lap.ToString();
					else
						LaneCollection[t].Lap = "--";

					LaneCollection[t].Position = rd.Lane[t].Position.ToString();
					LaneCollection[t].PositionSuffix = OrdinalSuffix[rd.Lane[t].Position];

					LaneCollection[t].LastLapTime = String.Format("{0:0}.{1:000}",
						rd.Lane[t].LastLapTime / 1000, rd.Lane[t].LastLapTime % 1000);

					LaneCollection[t].BestLapTime = String.Format("{0:0}.{1:000}",
						rd.Lane[t].BestLapTime / 1000, rd.Lane[t].BestLapTime % 1000);

					LaneCollection[t].LapType = rd.Lane[t].LapType;
					LaneCollection[t].Finished = rd.Lane[t].Finished;


					if (rd.Lane[t].Finished == true && RaceWon == false)
					{
						Sounds.PlayRaceFinished();
						RaceWon = true;
					}
					else
					{
						switch (rd.Lane[t].LapType)
						{
							case LapType.Detected:
								Sounds.PlayLapDetected();
								break;
							case LapType.PersonalBest:
								Sounds.PlayPersonalBest();
								break;
							case LapType.FastestLap:
								Sounds.PlayFastestLap();
								break;
						}
					}
					
					// We clear the LapType field to redetect PropertyChanged events.
					LaneCollection[t].LapType = LapType.NoDetect;
				}

				if (Lights != rd.Lights)
				{
					StartLight.Lights = rd.Lights;

					if (rd.Lights > 0)
						Sounds.PlayRedLight();
					else if (rd.Lights == 0)
						Sounds.PlayStartRace();

					Lights = rd.Lights;
				}

				// Update the benchmark debugging display

				SamplesPerSecond = rd.SamplesPerSecond;
				PeakSample = rd.PeakSample;
				SamplesOver10ms = rd.SamplesOver10ms;
				SamplesOver20ms = rd.SamplesOver20ms;


			});
		}




		// Callback.  Called by the LapCounter thread when the race is finished.
		// This then performs cleanup, calls lc.FinishRace() ro cleanup the
		// LapCounter and to join the LapCounter thread.  Changes the state of
		// the buttons on this MainWindow.

		private void StopRaceDelegate()
		{
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate()
			{
				LapCounter.FinishRace();
				StartLight.Lights = -1;
				OptionsButton.IsEnabled = true;
				RaceModeCombo.IsEnabled = true;
				RaceStarted = false;
				// StartStopButton.Content = "Start";
				StartStopLabel.Content = "Start";
			});
		}




		// Opens the dialog box and retrieves/stores all options.

		private void Options()
		{
			OptionsWindow Options = new OptionsWindow();
			Options.Owner = this;

			Options.LaneCount = LapCounter.LaneCount;
			Options.MinimumLapTime = LapCounter.MinimumLapTime;
			Options.PreRaceDelay = LapCounter.PreRaceDelay;
			Options.JumpStartAction = (int)LapCounter.JumpStart;
			Options.SamplingInterval = LapCounter.SamplingInterval;
			Options.ShowStatistics = ShowStats;

			Options.SensorType = (int)LapCounter.SensorType;

			Options.ScalexRMSComPortName = LapCounter.SerialPortName;
			Options.RMSUseInpOut32 = LapCounter.RMSUseInpOut32;

			Options.ParIOPort = LapCounter.ParPortBase;
			Options.ParallelPortInvertLogic = LapCounter.ParallelPortInvertLogic;
			Options.Lane1ParPin = LapCounter.Lane1ParPortBit;
			Options.Lane2ParPin = LapCounter.Lane2ParPortBit;
			Options.Lane3ParPin = LapCounter.Lane3ParPortBit;
			Options.Lane4ParPin = LapCounter.Lane4ParPortBit;
			Options.Lane5ParPin = LapCounter.Lane5ParPortBit;


			Options.SerIOPort = LapCounter.SerPortBase;
			Options.SerPortInvertLogic = LapCounter.SerialPortInvertLogic;
			Options.Lane1SerPin = LapCounter.Lane1SerPortBit;
			Options.Lane2SerPin = LapCounter.Lane2SerPortBit;
			Options.Lane3SerPin = LapCounter.Lane3SerPortBit;
			Options.Lane4SerPin = LapCounter.Lane4SerPortBit;
						
			Options.Lane1Color = LaneColor[0];
			Options.Lane2Color = LaneColor[1];
			Options.Lane3Color = LaneColor[2];
			Options.Lane4Color = LaneColor[3];
			Options.Lane5Color = LaneColor[4];
			Options.Lane6Color = LaneColor[5];
			Options.LaneStyle = LaneStyle;

			Options.UseDetectedLapSound = Sounds.UseLapDetectedSound;
			Options.UsePersonalBestSound = Sounds.UsePersonalBestSound;
			Options.UseFastestLapSound = Sounds.UseFastestLapSound;
			Options.UseRedLightSound = Sounds.UseRedLightSound;
			Options.UseRaceStartSound = Sounds.UseStartRaceSound;
			Options.UseRaceFinishedSound = Sounds.UseRaceFinishedSound;

			Options.DetectedLapFileName = Sounds.LapDetectedFilename;
			Options.PersonalBestFileName = Sounds.PersonalBestFilename;
			Options.FastestLapFileName = Sounds.FastestLapFilename;
			Options.RedLightFileName = Sounds.RedLightFilename;
			Options.RaceStartFileName = Sounds.StartRaceFilename;
			Options.RaceFinishedFileName = Sounds.RaceFinishedFilename;

			if (Options.ShowDialog() == true)
			{
				LapCounter.LaneCount = Options.LaneCount;
				LapCounter.MinimumLapTime = Options.MinimumLapTime;
				LapCounter.PreRaceDelay = Options.PreRaceDelay;
				LapCounter.JumpStart = (JumpStart)Options.JumpStartAction;
				LapCounter.SamplingInterval = Options.SamplingInterval;
				ShowStats = Options.ShowStatistics;

				LapCounter.SensorType = (SensorType)Options.SensorType;
				
				LapCounter.SerialPortName = Options.ScalexRMSComPortName;
				LapCounter.RMSUseInpOut32 = Options.RMSUseInpOut32;

				LapCounter.ParPortBase = Options.ParIOPort;
				LapCounter.ParallelPortInvertLogic = Options.ParallelPortInvertLogic;
				LapCounter.Lane1ParPortBit = Options.Lane1ParPin;
				LapCounter.Lane2ParPortBit = Options.Lane2ParPin;
				LapCounter.Lane3ParPortBit = Options.Lane3ParPin;
				LapCounter.Lane4ParPortBit = Options.Lane4ParPin;
				LapCounter.Lane5ParPortBit = Options.Lane5ParPin;

				LapCounter.SerPortBase = Options.SerIOPort;
				LapCounter.SerialPortInvertLogic = Options.SerPortInvertLogic;
				LapCounter.Lane1SerPortBit = Options.Lane1SerPin;
				LapCounter.Lane2SerPortBit = Options.Lane2SerPin;
				LapCounter.Lane3SerPortBit = Options.Lane3SerPin;
				LapCounter.Lane4SerPortBit = Options.Lane4SerPin;





				LaneColor[0] = Options.Lane1Color;
				LaneColor[1] = Options.Lane2Color;
				LaneColor[2] = Options.Lane3Color;
				LaneColor[3] = Options.Lane4Color;
				LaneColor[4] = Options.Lane5Color;
				LaneColor[5] = Options.Lane6Color;
				LaneStyle = Options.LaneStyle;


				Sounds.UseLapDetectedSound = Options.UseDetectedLapSound;
				Sounds.UsePersonalBestSound = Options.UsePersonalBestSound;
				Sounds.UseFastestLapSound = Options.UseFastestLapSound;
				Sounds.UseRedLightSound = Options.UseRedLightSound;
				Sounds.UseStartRaceSound = Options.UseRaceStartSound;
				Sounds.UseRaceFinishedSound = Options.UseRaceFinishedSound;

				Sounds.LapDetectedFilename = Options.DetectedLapFileName;
				Sounds.PersonalBestFilename = Options.PersonalBestFileName;
				Sounds.FastestLapFilename = Options.FastestLapFileName;
				Sounds.RedLightFilename = Options.RedLightFileName;
				Sounds.StartRaceFilename = Options.RaceStartFileName;
				Sounds.RaceFinishedFilename = Options.RaceFinishedFileName;

				LaneViewer.LaneStyle = LaneStyle;

				InitializeLaneCollection();
			}
		}




		// Load settings

		private void LoadSettings()
		{
			/* Check if this is first run, then initialise the filenames by concatenating the
				current directory with the value stored in Settings.

			string txtPath = Path.Combine(Environment.CurrentDirectory, "testfile.txt");
			*/

			

			RaceMode = Settings.Default.RaceMode;
			
			LapCounter.LaneCount = Settings.Default.Lanes;
			LapCounter.MinimumLapTime = Settings.Default.MinLapTime;
			LapCounter.PreRaceDelay = Settings.Default.PreRaceDelay;
			LapCounter.JumpStart = (JumpStart)Settings.Default.JumpStartAction;
			LapCounter.SamplingInterval = Settings.Default.SamplingInterval;
			ShowStats = Settings.Default.ShowStats;

			LapCounter.SensorType = (SensorType)Settings.Default.SensorType;
			
			LapCounter.SerialPortName = Settings.Default.SerialPort;
			LapCounter.RMSUseInpOut32 = Settings.Default.RMSUseInpOut32;

			LapCounter.ParPortBase = Settings.Default.ParallelIOPort;
			LapCounter.ParallelPortInvertLogic = Settings.Default.ParallelPortInvertLogic;
			LapCounter.Lane1ParPortBit = Settings.Default.Lane1ParPin;
			LapCounter.Lane2ParPortBit = Settings.Default.Lane2ParPin;
			LapCounter.Lane3ParPortBit = Settings.Default.Lane3ParPin;
			LapCounter.Lane4ParPortBit = Settings.Default.Lane4ParPin;
			LapCounter.Lane5ParPortBit = Settings.Default.Lane5ParPin;

			LapCounter.SerPortBase = Settings.Default.SerialIOPort;
			LapCounter.SerialPortInvertLogic = Settings.Default.SerialPortInvertLogic;
			LapCounter.Lane1SerPortBit = Settings.Default.Lane1SerPin;
			LapCounter.Lane2SerPortBit = Settings.Default.Lane2SerPin;
			LapCounter.Lane3SerPortBit = Settings.Default.Lane3SerPin;
			LapCounter.Lane4SerPortBit = Settings.Default.Lane4SerPin;
			
			LaneColor[0] = Settings.Default.Lane1Color;
			LaneColor[1] = Settings.Default.Lane2Color;
			LaneColor[2] = Settings.Default.Lane3Color;
			LaneColor[3] = Settings.Default.Lane4Color;
			LaneColor[4] = Settings.Default.Lane5Color;
			LaneColor[5] = Settings.Default.Lane6Color;
			LaneStyle = Settings.Default.LaneStyle;
			
			Sounds.UseLapDetectedSound = Settings.Default.UseDetectedLapSound;
			Sounds.UsePersonalBestSound = Settings.Default.UsePersonalBestSound;
			Sounds.UseFastestLapSound = Settings.Default.UseFastestLapSound;
			Sounds.UseRedLightSound = Settings.Default.UseRedLightSound; 
			Sounds.UseStartRaceSound = Settings.Default.UseStartRaceSound;
			Sounds.UseRaceFinishedSound = Settings.Default.UseRaceFinishedSound;
						
			Sounds.LapDetectedFilename = Settings.Default.DetectLapFileName;
			Sounds.PersonalBestFilename = Settings.Default.PersonalBestFileName;
			Sounds.FastestLapFilename = Settings.Default.FastestLapFileName;
			Sounds.RedLightFilename = Settings.Default.RedLightFileName;
			Sounds.StartRaceFilename = Settings.Default.StartRaceFileName;
			Sounds.RaceFinishedFilename = Settings.Default.RaceFinishedFileName;

			LaneViewer.LaneStyle = LaneStyle;
			
			// SetLaneStyle(LaneStyle);
		}




		// Saves settings

		private void SaveSettings()
		{
			Settings.Default.RaceMode = RaceMode;
			
			Settings.Default.Lanes = LapCounter.LaneCount;
			Settings.Default.MinLapTime = LapCounter.MinimumLapTime;
			Settings.Default.PreRaceDelay = LapCounter.PreRaceDelay;
			Settings.Default.JumpStartAction = (int)LapCounter.JumpStart;
			Settings.Default.SamplingInterval = LapCounter.SamplingInterval;
			Settings.Default.ShowStats = ShowStats;

			Settings.Default.SensorType = (int)LapCounter.SensorType;
			Settings.Default.SerialPort = LapCounter.SerialPortName;
			Settings.Default.RMSUseInpOut32 = LapCounter.RMSUseInpOut32;

			Settings.Default.ParallelIOPort = LapCounter.ParPortBase;
			Settings.Default.ParallelPortInvertLogic = LapCounter.ParallelPortInvertLogic;
			Settings.Default.Lane1ParPin = LapCounter.Lane1ParPortBit;
			Settings.Default.Lane2ParPin = LapCounter.Lane2ParPortBit;
			Settings.Default.Lane3ParPin = LapCounter.Lane3ParPortBit;
			Settings.Default.Lane4ParPin = LapCounter.Lane4ParPortBit;
			Settings.Default.Lane5ParPin = LapCounter.Lane5ParPortBit;

			Settings.Default.SerialIOPort = LapCounter.SerPortBase;
			Settings.Default.SerialPortInvertLogic = LapCounter.SerialPortInvertLogic;
			Settings.Default.Lane1SerPin = LapCounter.Lane1SerPortBit;
			Settings.Default.Lane2SerPin = LapCounter.Lane2SerPortBit;
			Settings.Default.Lane3SerPin = LapCounter.Lane3SerPortBit;
			Settings.Default.Lane4SerPin = LapCounter.Lane4SerPortBit;

			Settings.Default.Lane1Color = LaneColor[0];
			Settings.Default.Lane2Color = LaneColor[1];
			Settings.Default.Lane3Color = LaneColor[2];
			Settings.Default.Lane4Color = LaneColor[3];
			Settings.Default.Lane5Color = LaneColor[4];
			Settings.Default.Lane6Color = LaneColor[5];
			Settings.Default.LaneStyle = LaneStyle;
			
			Settings.Default.UseDetectedLapSound = Sounds.UseLapDetectedSound;
			Settings.Default.UsePersonalBestSound = Sounds.UsePersonalBestSound;
			Settings.Default.UseFastestLapSound = Sounds.UseFastestLapSound;
			Settings.Default.UseRedLightSound = Sounds.UseRedLightSound;
			Settings.Default.UseStartRaceSound = Sounds.UseStartRaceSound;
			Settings.Default.UseRaceFinishedSound = Sounds.UseRaceFinishedSound;
			
			Settings.Default.DetectLapFileName = Sounds.LapDetectedFilename;
			Settings.Default.PersonalBestFileName = Sounds.PersonalBestFilename;
			Settings.Default.FastestLapFileName = Sounds.FastestLapFilename;
			Settings.Default.RedLightFileName = Sounds.RedLightFilename;
			Settings.Default.StartRaceFileName = Sounds.StartRaceFilename;
			Settings.Default.RaceFinishedFileName = Sounds.RaceFinishedFilename;
			
			Settings.Default.Save();
		}
    }
}




