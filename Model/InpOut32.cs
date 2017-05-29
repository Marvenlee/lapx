using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace LapX
{
	// Class used for accessing the parallel port, non blocking, they
	// just read or write the contents of IO ports.
	//
	// Uses the common inpout32.dll to access the IO ports.  Could
	// be a vulnerability as it allows any IO port to be accessed.
	// Perhaps tweak the inpout32.dll source code to only allow access
	// to parallel ports.
	
	class InpOut32
	{
		[DllImport("inpout32.dll", EntryPoint = "Inp32")]
		public static extern  short In(short address);

		[DllImport("inpout32.dll", EntryPoint = "Out32")]
		public static extern void Output(short address, short value);

	}
}
