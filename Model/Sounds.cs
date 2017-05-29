using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;


namespace LapX
{
	class Sounds
	{
		public void PlayFastestLap()
		{
			if (UseFastestLapSound == true)
				FastestLapSound.Play();
			else if (UseLapDetectedSound == true)
				LapDetectedSound.Play();
		}

		public void PlayPersonalBest()
		{
			if (UsePersonalBestSound == true)
				PersonalBestSound.Play();
			else if (UseLapDetectedSound == true)
				LapDetectedSound.Play();
		}

		public void PlayLapDetected()
		{
			if (UseLapDetectedSound == true)
				LapDetectedSound.Play();
		}

		public void PlayRedLight()
		{
			if (UseRedLightSound == true)
				RedLightSound.Play();
		}

		public void PlayStartRace()
		{
			if (UseStartRaceSound == true)
				StartRaceSound.Play();
		}

		public void PlayRaceFinished()
		{
			if (UseRaceFinishedSound == true)
				RaceFinishedSound.Play();
		}

		
		public bool UseFastestLapSound;
		public string FastestLapFilename
		{
			get { return _FastestLapFilename;}
			set
			{
				try
				{
					_FastestLapFilename = value;
					FastestLapSound = new SoundPlayer(_FastestLapFilename);
					FastestLapSound.Load();
				}
				catch
				{
					_FastestLapFilename = "";
					UseFastestLapSound = false;
				}
			}
		}
		string _FastestLapFilename;
		SoundPlayer FastestLapSound;
		


		public bool UsePersonalBestSound;
		public string PersonalBestFilename
		{
			get { return _PersonalBestFilename; }
			set
			{
				try
				{
					_PersonalBestFilename = value;
					PersonalBestSound = new SoundPlayer(_PersonalBestFilename);
					PersonalBestSound.Load();
				}
				catch
				{
					_PersonalBestFilename = "";
					UsePersonalBestSound = false;
				}
			}
		}
		string _PersonalBestFilename;
		SoundPlayer PersonalBestSound;
		

		public bool UseLapDetectedSound;
		public string LapDetectedFilename
		{
			get { return _LapDetectedFilename; }
			set
			{
				try
				{
					_LapDetectedFilename = value;
					LapDetectedSound = new SoundPlayer(_LapDetectedFilename);
					LapDetectedSound.Load();
				}
				catch
				{
					_LapDetectedFilename = "";
					UseLapDetectedSound = false;
				}
			}
		}
		string _LapDetectedFilename;
		SoundPlayer LapDetectedSound;
		

		public bool UseRedLightSound;
		public string RedLightFilename
		{
			get { return _RedLightFilename; }
			set
			{
				try
				{
					_RedLightFilename = value;
					RedLightSound = new SoundPlayer(_RedLightFilename);
					RedLightSound.Load();
				}
				catch
				{
					_RedLightFilename = "";
					UseRedLightSound = false;
				}
			}
		}
		string _RedLightFilename;
		SoundPlayer RedLightSound;
		

		public bool UseStartRaceSound;
		public string StartRaceFilename
		{
			get { return _StartRaceFilename; }
			set
			{
				try
				{
					_StartRaceFilename = value;
					StartRaceSound = new SoundPlayer(_StartRaceFilename);
					StartRaceSound.Load();
				}
				catch
				{
					_StartRaceFilename = "";
					UseStartRaceSound = false;
				}
			}
		}
		string _StartRaceFilename;
		SoundPlayer StartRaceSound;
		

		public bool UseRaceFinishedSound;
		public string RaceFinishedFilename
		{
			get { return _RaceFinishedFilename; }
			set
			{
				try
				{
					_RaceFinishedFilename = value;
					RaceFinishedSound = new SoundPlayer(_RaceFinishedFilename);
					RaceFinishedSound.Load();
				}
				catch
				{
					_RaceFinishedFilename = "";
					UseRaceFinishedSound = false;
				}
			}
		}
		string _RaceFinishedFilename;
		SoundPlayer RaceFinishedSound;
		

		

	}
}
