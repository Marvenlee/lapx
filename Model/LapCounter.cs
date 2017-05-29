using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;




namespace LapX
{
	// Move into LapCounter class?

    delegate void LapCounterDelegate();
    public enum State { Started, Stopped, Paused };
    public enum Mode { Race, Endurance, Qualifying, Practice };
	public enum JumpStart { CountLap, IgnoreLap };
	public enum SensorType {None, ScalextricRMS, Parallel, Serial};


    class LapCounter
    {
		// Public 

		public int SamplingInterval = 1;	// How often in ms to sample the sensors
        public int LaneCount = 2;
		public Mode Mode = Mode.Practice;
		public JumpStart JumpStart = JumpStart.CountLap;
		public long MinimumLapTime = 1000;
		public long PreRaceDelay = 5000;
		public int MaxLap = 10;
		public long MaxDuration = 60000;

		public State State = State.Stopped;
		public LapCounterDelegate RefreshDelegate;
		public LapCounterDelegate StopRaceDelegate;

		public SensorType SensorType = SensorType.None;
				
		public string SerialPortName = "COM1";
		public int RMSPortBase = 0x3f8;
		public bool RMSUseInpOut32 = false;

		public int ParPortBase = 0x378;
		public bool ParallelPortInvertLogic;
		public int Lane1ParPortBit;
		public int Lane2ParPortBit;
		public int Lane3ParPortBit;
		public int Lane4ParPortBit;
		public int Lane5ParPortBit;

		public int SerPortBase = 0x3f8;
		public bool SerialPortInvertLogic;
		public int Lane1SerPortBit;
		public int Lane2SerPortBit;
		public int Lane3SerPortBit;
		public int Lane4SerPortBit;
		

        // Private

		Object Lock = new Object();  // Lock around all race data
		SerialPort SerialPort;
        Thread CounterThread;
		volatile bool ThreadContinueLoop = true;
		Random RNG;				// RNG for varying the start light hold time
		int[] KeyLaps;			// FIXME: Move KeyLaps into each Lane class?
		int[] Sort;				// Lane numbers sorted by race position
		
		long[] LaneClearTime;				// To be used to ensure cars don't hover
											// over lane sensor, thus triggering
											// the lane count without doing a lap.

		long PrevTime;			// Time before last sample
		long DiffTicks;			// Time difference between samples
		long CurrentTime;		// Time of current sample
		long RaceStartTime;		// Determined by PreRaceDelay and Random delay
		long ElapsedTime;		// Elapsed time in milliseconds since race started
        long NextUpdateTime;	// When the next display update is scheduled
		int Lights;				// State of start light sequence -1 = off, 0 = green etc
		Lane[] Lane;			// Data relating to each lane
		long FastestLapTime = 0;
		private bool RaceWon;
		private bool RaceFinished;

		// Benchmarking 
		long SamplesPerSecond;
		long PeakSample;
		long SamplesOver10ms;
		long SamplesOver20ms;
		


        // Constructor
		
        public LapCounter()
        {
			RNG = new Random();
        }



        
        // Initializes the LapCounter object to begin a race.   Lane data initialized,
        // serial port or other IO initialized and thread to handle sampling of IO is
        // created.

        public int BeginRace()
        {
			int t;
			byte b;

			SerialPort = new SerialPort();

			Lane = new Lane[LaneCount];

			for (t = 0; t < LaneCount; t++)
				Lane[t] = new Lane();

			Sort = new int[LaneCount];
			KeyLaps = new int[LaneCount];
			LaneClearTime = new long[LaneCount];

			
			for (t = 0; t < LaneCount; t++)
			{
				Lane[t].Clear();
				LaneClearTime[t] = MinimumLapTime;
			}
			
			if (SensorType == SensorType.ScalextricRMS)
			{
				SerialPort.PortName = SerialPortName;
				SerialPort.BaudRate = 9600;
				SerialPort.Parity = Parity.None;
				SerialPort.DataBits = 8;
				SerialPort.StopBits = StopBits.One;
				SerialPort.Handshake = Handshake.None;
				SerialPort.ReadTimeout = 0;
				SerialPort.WriteTimeout = 1000;
				SerialPort.DtrEnable = true;
				SerialPort.Open();

				if (SerialPortName == "COM1")
					RMSPortBase = 0x3f8;
				else if (SerialPortName == "COM2")
					RMSPortBase = 0x2f8;
				else
					RMSPortBase = 0;

				if (RMSUseInpOut32 == true)
				{
					if (RMSPortBase == 0x3f8)
						InpOut32.Output (0x3f9, 0);
					else if (RMSPortBase == 0x2f8)
						InpOut32.Output (0x2f9, 0);
				}
			}


			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			Thread.CurrentThread.Priority = ThreadPriority.Normal;

            CounterThread = new Thread(LapCounterThread);
            CounterThread.Priority = ThreadPriority.Highest;

			
			
            ThreadContinueLoop = true;
			State = State.Started;

			CounterThread.Start();

			return 0;
        }




        // Notify the LapCounter thread that a race should be stopped.
		// "Does not" wait for thread to terminate, so is non blocking.
		// Final cleanup and termination is done by the FinishRaceCallback
		// that then calls FinishRace().
		// 

        public void CancelRace()
        {
            ThreadContinueLoop = false;
        }



		// Performs cleanup once a race is finished.  Should be called
		// by the FinishRaceCallback on the Dispatcher/UI context 
		// to perform this cleanup.

		public void FinishRace()
		{
			CounterThread.Join();

			if (SensorType == SensorType.ScalextricRMS)
			{
				SerialPort.Close();
			}
			
			State = State.Stopped;
		}




		// Request the lap count of a lane is incremented
		
        public void IncrementLapCount(int laneNr)
        {
			if (State == State.Started && laneNr < LaneCount)
				KeyLaps[laneNr]++;
        }




		// Returns the RaceData class that holds the state of the race
		// at that instant.  We boost the priority of the calling thread
		// to that of the LapCounter's thread as we copy the locked variables.
		// This should act as a poor man's priority ceiling mutex to avoid
		// priority inversion of the LapCounter's thread.

		public RaceData GetRaceState()
		{
			int t;
			ThreadPriority pri;
			RaceData raceData = new RaceData();

			raceData.Lane = new Lane[LaneCount];

			for (t = 0; t < LaneCount; t++)
				raceData.Lane[t] = new Lane();
			
			pri = Thread.CurrentThread.Priority;
			Thread.CurrentThread.Priority = ThreadPriority.Highest;

			lock (Lock)
			{
				raceData.ElapsedTime = ElapsedTime;
				raceData.Lights = Lights;
				raceData.LaneCount = LaneCount;
				raceData.RaceWon = RaceWon;
				raceData.RaceFinished = RaceFinished;

				raceData.SamplesPerSecond = SamplesPerSecond;
				raceData.PeakSample = PeakSample;
				raceData.SamplesOver10ms = SamplesOver10ms;
				raceData.SamplesOver20ms = SamplesOver20ms;


				for (t = 0; t < LaneCount; t++)
				{
					raceData.Lane[t].Lap = Lane[t].Lap;
					raceData.Lane[t].Position = Lane[t].Position;
					raceData.Lane[t].LastLapTime = Lane[t].LastLapTime;
					raceData.Lane[t].BestLapTime = Lane[t].BestLapTime;
					raceData.Lane[t].Finished = Lane[t].Finished;
					raceData.Lane[t].LapType = Lane[t].LapType;

					// Clear the Lap detected flags now the View has a copy of it.
					Lane[t].LapType = LapType.NoDetect;
				}
			}

			Thread.CurrentThread.Priority = pri;

			return raceData;
		}




		// Thread of the lap counter.  All sensing and race logic is
		// done on this thread.  Delegates are called whenever the
		// display needs updating or the race ends.

        private void LapCounterThread()
        {
			long TSampleCount = 0;
			long TPeakSample = 0;
			long TSamplesOver10ms = 0;
			long TSamplesOver20ms = 0;
			long BeginTimeWindow = 0;
			long EndTimeWindow = 0;

			byte laneBits;
            bool uiUpdateRequired = false;
            Stopwatch watch = new Stopwatch();
			uint oldPeriod;
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			oldPeriod = MMTimer.TimeBeginPeriod(1);

            watch.Start();
			RaceWon = false;
			RaceFinished = false;
			FastestLapTime = 0;
            CurrentTime = 0;
            ElapsedTime = 0;
			NextUpdateTime = 1000;


			// FIXME:  Need to set minimum StartTime etc also handle new Light Sequence

			RaceStartTime = PreRaceDelay + 5000;
			DiffTicks = 0;
			BeginTimeWindow = 0;


			UpdateLanePositions();
			RefreshDelegate();
			
            while (ThreadContinueLoop)
            {
				Thread.Sleep(SamplingInterval);
				PrevTime = CurrentTime;
                CurrentTime = watch.ElapsedMilliseconds;
				DiffTicks = CurrentTime - PrevTime;


				lock (Lock)
				{
					TSampleCount++;

					if (DiffTicks > TPeakSample)
						TPeakSample = DiffTicks;

					if (DiffTicks > 10)
						TSamplesOver10ms++;

					if (DiffTicks > 20)
						TSamplesOver20ms++;



					if (CurrentTime >= RaceStartTime)
						ElapsedTime = CurrentTime - RaceStartTime;

					if (CurrentTime > NextUpdateTime)
					{
						NextUpdateTime = (CurrentTime + 1000)/1000*1000;

						// Update and copy statistics for previous
						// second, then reset the temp values for the
						// next second.

						EndTimeWindow = CurrentTime;

						if (EndTimeWindow == BeginTimeWindow)
							EndTimeWindow++;
						
						SamplesPerSecond = (TSampleCount * (EndTimeWindow - BeginTimeWindow)) / 1000;
						PeakSample = TPeakSample;
						SamplesOver10ms = TSamplesOver10ms;
						SamplesOver20ms = TSamplesOver20ms;
						
						BeginTimeWindow = CurrentTime;
						TSampleCount = 0;
						TPeakSample = 0;
						TSamplesOver10ms = 0;
						TSamplesOver20ms = 0;

						uiUpdateRequired = true;
					}

					if (StartLightsChanged() == true)
						uiUpdateRequired = true;
				
					laneBits = ReadLaneSensors();

					if (UpdateRace(laneBits) == true)
						ThreadContinueLoop = false;

					if (laneBits != 0)
						uiUpdateRequired = true;
				}

				if (uiUpdateRequired == true)
                    RefreshDelegate();
				
				uiUpdateRequired = false;
			}

            watch.Stop();

			MMTimer.TimeEndPeriod(oldPeriod);

			StopRaceDelegate();
		}




		// Changes the value of Lights depending on race time.

		bool StartLightsChanged()
		{
			int newLights;

			if (CurrentTime >= RaceStartTime + 2000)
				newLights = -1;
			else if (CurrentTime >= RaceStartTime)
				newLights = 0;
			else if (CurrentTime >= RaceStartTime - 1000)
				newLights = 1;
			else if (CurrentTime >= RaceStartTime - 2000)
				newLights = 2;
			else if (CurrentTime >= RaceStartTime - 3000)
				newLights = 3;
			else if (CurrentTime >= RaceStartTime - 4000)
				newLights = 4;
			else 
				newLights = 5;

			if (newLights != Lights)
			{
				Lights = newLights;
				return true;
			}
			else
				return false;
		}






		// Reads the Keyboard, Serial and Parallel port
		// Returns a bitmap of lanes that have been triggered.
		
		byte ReadLaneSensors()
		{
			int n;
			byte laneBits = 0;
			byte parBits = 0;
			byte serBits = 0;


			for (n = 0; n < LaneCount; n++)
			{
				if (KeyLaps[n] != 0)
				{
					KeyLaps[n] = 0;
					laneBits |= (byte)(1 << n);
				}
			}


			if (SensorType == SensorType.ScalextricRMS)
			{
				// FIXME:
				//
				// The serial port is effectively edge-triggered and so we rely
				// on using MinimumLapTime to avoid false detection of laps.
				// Perhaps the MimimumLapTime test in UpdateRace() could be
				// moved into here instead,  The parallel port sensor already
				// uses MinimumLapTime in here as it is effectively level-triggered
				// and handles it slightly differently to the serial port.

				// FIXME:
				// RMS access using InpOut32 could be improved.  We rely
				// on the Serial Port class and therefore the proper serial port
				// driver to open and initialize the port.  As setting the port
				// up with inpout32 and writing to the serial port registers
				// doesn't seem to work.   Try to find out why.
				// We disable the serial port interrupts to stop the real kernel
				// driver getting to the data before we access the receiver buffer
				
				
				if (RMSUseInpOut32 == true
					&& (RMSPortBase == 0x3f8 || RMSPortBase == 0x2f8))
				{
					n = InpOut32.In((short)(RMSPortBase + 0x05));

					if ((n & 0x01) == 0x01)
					{
						n = InpOut32.In((short)(RMSPortBase + 0x00));

						if (n >= '1' && n <= '6')
						{
							n -= '1';
							laneBits |= (byte)(1 << n);
						}
					}
				}
				else
				{
					try
					{
						n = SerialPort.ReadByte();

						if (n >= '1' && n <= '6')
						{
							n -= '1';
							laneBits |= (byte)(1 << n);
						}
					}
					catch (TimeoutException)
					{
					}
				}
			}
			else if (SensorType == SensorType.Parallel)
			{
				// Parallel Port Status Register 0x379
				// Bit 7 - Pin 11 - ~Busy       *
				// Bit 6 - Pin 10 - nAck        *
				// Bit 5 - Pin 12 - Paper Out   *
				// Bit 4 - Pin 13 - Select In   *
				// Bit 3 - Pin 15 - Error       *
				// Bit 2 - ~IRQ
				// Bit 1 - Reserved
				// Bit 0 - Reserved
				//
				// Usually a sensor pin is grounded to detect a car and
				// assumes the pin will naturally float high to 5v, else
				// an external power source will be needed for the sensors.


				parBits = (byte)(InpOut32.In((short)(ParPortBase + 0x01)));

				// FIXME:  Check if I'm using right bits and mask.
				// Not sure why I'm trying to mask the bits as
				// we read the corresponding bit for each lane
				// individually.
				// parBits = (parBits & 0x7f) | (0x80 & ~parBits);
				// parBits = (parBits & 0x77) | (0x88 & ~parBits);


				if (ParallelPortInvertLogic == true)
					parBits = (byte)~parBits;


				if (Lane1ParPortBit != -1 && (parBits & 1 << Lane1ParPortBit) != 0)
					laneBits |= 1 << 0;

				if (Lane2ParPortBit != -1 && (parBits & 1 << Lane2ParPortBit) != 0)
					laneBits |= 1 << 1;

				if (Lane3ParPortBit != -1 && (parBits & 1 << Lane3ParPortBit) != 0)
					laneBits |= 1 << 2;

				if (Lane4ParPortBit != -1 && (parBits & 1 << Lane4ParPortBit) != 0)
					laneBits |= 1 << 3;

				if (Lane5ParPortBit != -1 && (parBits & 1 << Lane5ParPortBit) != 0) 
					laneBits |= 1 << 4;


				// As the parallel port sensors are effectively level triggered
				// Just checking MinimumLapTime has occured since the last lap
				// was detected won't work as a car could stop over the lane
				// sensor and the lane would be incremented once every MinimumLapTime
				// has occured.  So we check that the lane has no car over it 
				// for MinimumLapTime ticks.
				//
								
				for (n = 0; n < LaneCount; n++)
				{
					// Increment the LaneClearTime for any laneBits that are zero.
					
					if ((laneBits & (1 << n)) == 0)
						LaneClearTime[n] += DiffTicks;

					// Clear bits in laneBits that have a LaneClearTime less than
					// MinimumLapTime, else accept the lane has a detected car
					// and reset the LaneClearTime.

					if ((laneBits & (1 << n)) != 0)
					{
						if (LaneClearTime[n] < MinimumLapTime)
							laneBits = (byte)(laneBits & ~(1 << n));
						else
							LaneClearTime[n] = 0;
					}
				}
			}
			else if (SensorType == SensorType.Serial)
			{
				// Use Pin 1, 6, 8 and 9 (9-pin connector) as inputs and
				// ground the pins to pin 5 to trigger a lap (assuming they float).
				// Maybe possible to use pins 4 and 7 to provide a +- power source
				// for sensors.  Needs to use the same algorithm as the
				// parallel port code above.

				// TODO:
				// This code, plus loading, saving settings in code-behind for
				// serial pins,  possibly add combo boxes for pins 4 and 7 to
				// set voltage.


				// Serial Port Modem Status Register - offset 0x06
				// Bit 7 - Pin 1 - DCD          *
				// Bit 6 - Pin 9 - RI           *
				// Bit 5 - Pin 6 - DSR          *
				// Bit 4 - Pin 8 - CTS          *
				// Bit 3 - 
				// Bit 2 - 
				// Bit 1 - 
				// Bit 0 - 
				
				serBits = (byte)(InpOut32.In((short)(SerPortBase + 0x06)));

				if (SerialPortInvertLogic == true)
					serBits = (byte)~serBits;

				if (Lane1SerPortBit != -1 && (serBits & 1 << Lane1SerPortBit) != 0)
					laneBits |= 1 << 0;

				if (Lane2SerPortBit != -1 && (serBits & 1 << Lane2SerPortBit) != 0)
					laneBits |= 1 << 1;

				if (Lane3SerPortBit != -1 && (serBits & 1 << Lane3SerPortBit) != 0)
					laneBits |= 1 << 2;

				if (Lane4SerPortBit != -1 && (serBits & 1 << Lane4SerPortBit) != 0)
					laneBits |= 1 << 3;

				
				for (n = 0; n < LaneCount; n++)
				{
					if ((laneBits & (1 << n)) == 0)
						LaneClearTime[n] += DiffTicks;

					if ((laneBits & (1 << n)) != 0)
					{
						if (LaneClearTime[n] < MinimumLapTime)
							laneBits &= (byte)~(1 << n);
						else
							LaneClearTime[n] = 0;
					}
				}
			}
			

			return laneBits;
		}

		





		
		// Updates the state of the race.  laneBits is a bitmap of the state
		// of the lane sensors. 1 = car detected, 0 = no car detected.
		// Checks if the race has started, whether to ignore jump starts
		// and prevents spurious laps being registered using the minimum lap time
		// setting.  It then updates lap counts, lap times and positions.
		//

		bool UpdateRace (int laneBits)
		{
			int finishedCount = 0;
			bool raceFinished = false;
			int n;


			for (n = 0; n < LaneCount; n++)
			{
				if ((laneBits & (1 << n)) != 0)
				{
					if (Lane[n].Lap == -1)
					{
						if (JumpStart == JumpStart.CountLap ||
							(JumpStart == JumpStart.IgnoreLap && CurrentTime >= RaceStartTime))
						{
							Lane[n].Lap++;
							Lane[n].LastDetectedTime = CurrentTime;
							Lane[n].LapType = LapType.Detected;
						}
						
					}
					else if (Lane[n].Finished != true
						&& CurrentTime >= Lane[n].LastDetectedTime + MinimumLapTime)
					{
						Lane[n].Lap++;
						Lane[n].LastLapTime = CurrentTime - Lane[n].LastDetectedTime;
						Lane[n].LastDetectedTime = CurrentTime;
						Lane[n].LapType = LapType.Detected;

						if (Lane[n].LastLapTime < Lane[n].BestLapTime || Lane[n].BestLapTime == 0)
						{
							Lane[n].BestLapTime = Lane[n].LastLapTime;
							Lane[n].LapType = LapType.PersonalBest;

						}
						if (Lane[n].LastLapTime < FastestLapTime || FastestLapTime == 0)
						{
							FastestLapTime = Lane[n].LastLapTime;
							Lane[n].LapType = LapType.FastestLap;
						}
					}


					if (Mode == Mode.Race || Mode == Mode.Practice)
					{
						// IDEA: Have option to do all laps or first-past-the-post stops
						// race on that lap.  Add a white flag for last-lap indicator?

						if (Lane[n].Lap == MaxLap
							|| RaceWon == true)
						{
							Lane[n].Finished = true;
							RaceWon = true;
						}
					}
					else if (Mode == Mode.Endurance)
					{
						if (ElapsedTime > MaxDuration)
						{
							RaceWon = true;
							Lane[n].Finished = true;
						}
					}
					else
						throw new NotImplementedException();
					
				}
				
				if (Lane[n].Finished == true)
					finishedCount ++;
			}
		
			UpdateLanePositions();

			if (finishedCount == LaneCount)
				raceFinished = true;
			
			return raceFinished;
		}



		




		// Update the position field of each Lane,  sort by lap count and then by
		// lowest last_detected_time.
           
        private void UpdateLanePositions ()
        {
            int i, j, k, t;
			
            for (i = 0; i < LaneCount; i++)
                Sort[i] = i;
            
            for (i = 0; i < LaneCount - 1; i++)
            {
                k = i;

                for (j = i + 1; j < LaneCount; j++)
                    if ((Lane[Sort[j]].Lap > Lane[Sort[k]].Lap) ||
                        (Lane[Sort[j]].Lap == Lane[Sort[k]].Lap &&
                        Lane[Sort[j]].LastDetectedTime < Lane[Sort[k]].LastDetectedTime))
                        k = j;
                
                t = Sort[k];
                Sort[k] = Sort[i];
                Sort[i] = t;
            
            }

            for (i = 0; i < LaneCount; i++)
                Lane[Sort[i]].Position = i + 1;
        }
    }
}
