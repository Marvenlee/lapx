using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LapX
{
	// Data Transfer Object for retrieving race state

	class RaceData
	{
		public Lane[] Lane;
		public int LaneCount;
		public int Lights;
		public long ElapsedTime;
		public bool RaceWon;
		public bool RaceFinished;

		public long SamplesPerSecond;
		public long SamplesOver10ms;
		public long SamplesOver20ms;
		public long PeakSample;
	}
}
