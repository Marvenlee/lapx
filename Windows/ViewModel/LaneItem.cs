using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;




namespace LapX
{
	// Item used by LaneView control to display Lane information.
	// Effectively a ViewModel of the Model's Lane class

	public class LaneItem : ViewModelBase
    {
		private string _Lap;
        public string Lap
        {
            get { return _Lap; }
			set { SetProperty (ref _Lap, value, "Lap");}
        }
		
		private string _Position;
        public string Position
        {
			get { return _Position; }
			set { SetProperty(ref _Position, value, "Position"); }
        }

		private string _PositionSuffix;
		public string PositionSuffix
		{
			get { return _PositionSuffix; }
			set { SetProperty(ref _PositionSuffix, value, "PositionSuffix"); }
		}

		private string _LastLapTime;
        public string LastLapTime
        {
            get { return _LastLapTime; }
			set { SetProperty(ref _LastLapTime, value, "LastLapTime"); }
        }

		private string _BestLapTime;
        public string BestLapTime
        {
            get { return _BestLapTime; }
			set { SetProperty(ref _BestLapTime, value, "BestLapTime"); }
		}

		private LapType _LapType;
		public LapType LapType
		{
			get { return _LapType; }
			set { SetProperty(ref _LapType, value, "LapType"); }
		}

		private Color _LaneColor;
		public Color LaneColor
		{
			get { return _LaneColor; }
			set { SetProperty(ref _LaneColor, value, "LaneColor"); }
		}

		private bool _Finished;
		public bool Finished
		{
			get { return _Finished; }
			set { SetProperty(ref _Finished, value, "Finished"); }
		}
	}
}
