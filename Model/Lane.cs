using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LapX
{
	public enum LapType { NoDetect, Detected, PersonalBest, FastestLap};

	public class Lane
    {
        public int LaneNr;                     // Lane number
        public int LaneColor;                  // Lane Color.
        public string Driver;                  // Driver Name;

        public int Lap;                 // Laps "completed" -1 for not started first lap.
        public int Position;            // Position in race.
        public long LastDetectedTime;   // Absolute time when car last went over lap counter
        public long LastLapTime;        // Time taken between last 2 car detections
        public long BestLapTime;        // Fastest lap time for this lane

		// public bool Penalty;
		public bool Finished;			// Lane has finished race

		public LapType LapType;			// The type of the last lap registered



        public Lane()
        {
            Lap = -1;
            Position = 0;
            LastDetectedTime = 0;
            LastLapTime = 0;
            BestLapTime = 0;
			LapType = LapType.NoDetect;
			Finished = false;
		}


		// Where and why is this used?

		public void Clear()
		{
			Lap = -1;
            Position = 0;
            LastDetectedTime = 0;
            LastLapTime = 0;
            BestLapTime = 0;
			LapType = LapType.NoDetect;
			Finished = false;
		}
    }
}
