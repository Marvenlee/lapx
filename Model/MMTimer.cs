﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace LapX
{
	static class MMTimer
	{
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
		public static extern uint TimeBeginPeriod(uint uMilliseconds);

		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
		public static extern uint TimeEndPeriod(uint uMilliseconds);
	}
}