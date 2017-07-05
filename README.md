# LapX Slot Car Lap Counter

## About

LapX is a lap counter program for slot car racing.  LapX aupports Scalextric RMS
sensor track (C8143) as well as parallel and serial port sensors.


* 1 to 6 Lanes depending on sensor.
* Typical accuracy under 5ms for parallel port sensors.
* Supports Scalextric RMS sensor track (C8143)
* Supports parallel port sensors using optional inpout32.dll
* Choice of themes
* Practice, Race (lap limited) and Endurance (time limited) modes

## Screenshots

![screenshot 1](/screenshots/lapx1_med.jpg)

![screenshot 2](/screenshots/lapx2_med.jpg)

![screenshot 3](/screenshots/lapx3_med.jpg)

## Installation Instructions

Import the project into Visual Studio.  It was written for an earlier version, so
settings may need to be changed.

LapX requires .Net framework 3.5 or higher to be installed.

The inpout32.dll library is required to use LapX with parallel port sensors.
This library will only run on 32-bit versions of Windows and is used to read the state of
the parallel IO ports directly, bypassing OS protections. 

Inpout32.dll can be run from the LapX directory or placed in c:/windows/system32.
UAC may need to be disabled on some versions of Windows when using inpout32.dll as
it works by loading a custom device driver into the kernel.


## Main Window

* Start/Stop Button -
  Starts and stops a race.

* Race Mode Combo Box -
  Choose the type of race from Practice, Race (lap limited) or Endurance (time limited).

* Options Button -
  Change the configuration options of the lap counter.

* Exit Button - 
  Exits the LapX application.


## Main Window Keyboard Shortcuts

* Space - 
  Starts and stops a race.

* 1 to 6 Keys at top of keyboard - 
  Increment lap count of a lane.  Useful for incrementing missed laps though
  they can only be triggered after a chosen time since the last lap was detected.

* Up/Down Cursor Keys - 
  Change the value of the Race Mode Combo Box.


## Options Dialog box

### General Tab

* Lanes - 
  Number of lanes the track has (1-6)

* Minimum Lap Time - 
  The minimum time that must elapse before a lane's lap counter can be triggered again.
  Used to avoid detecting false laps.  This should be smaller than the fastest lap time
  of any car.  Value is in seconds and the precision is 1 millsecond, e.g.  3.142.

* Pre Race Delay - 
  The length of time before the red start lights go on.  Allows users to get ready
  for the race.  Value is in seconds and the precision is 1 millsecond, e.g.  7.536.

* Jump Start - 
  Choose whether to count a lap or ignore a lap before the red start lights go out.

* Sample Interval - 
  The time in milliseconds between each read of the lane sensors.  Increasing this
  setting will reduce CPU load but will also reduce lap time accuracy and also reduce the
  chance of a car being detected.

* Show Statistics - 
  Shows how many times the sensors are being sampled every second. 


### Sensors Tab

Select sensor to use from the list on the left hand side.

* RMS Tab

 * Serial Port Name - 
   Choose the serial port to use
  
 * Use Fast IO - 
   Reads lane data directly from serial port registers and increases precision.  Works
   only with onboard serial ports COM1 and COM2.  Left unticked any serial port should
   work (including USB adaptors) but the precision can vary..


* Parallel Tab

 * Lane Pins (Lanes 1 to 5) - 
   Choose the parallel port status pin to use for each lane. 

 * Invert Parallel Port Inputs - 
   If a 5v signal (logic high) applied to a status pin indicates the presence of a car
   then this option should be left unchecked.  If a 0v (logic low) signal is used to
   indicate the presence of a car then this option should be checked.


* Serial Tab

 * Lane Pins (Lanes 1 to 4) - 
   Choose the serial port pin to use for each lane (Pin numbers based on 9-pin serial
   port). 
  
 * Invert Serial Port Inputs - 
   Inverts the serial port inputs


### Sounds Tab

Enable the type of sounds to play whenever a car crosses the sensor. Sounds must be in
.wav format.


### Display Tab

* Lane Colors (Lanes 1 to 6) - 
  Choose the color to represent a lane

* Lane Style - 
  Choose the style of graphics to be used to display the lap counter data.


## Changes

06 Jan 12
Changed start light sequence so that the lights come on one at a time.  Altered
task priorities which should improve accuracy.  Modified themes and added more
sound effects.

31 Jul 11
Updated Options dialog box.  Placed all types of sensor on a single tab to make
it easier to expand the number of sensor types.  Added using serial port pins 1,
6, 8 and 9 as lane sensors and added 'Fast IO' option to read directly from the
serial port using inpout32 to increase precision.

10 Jul 11
Added Statistics display to show how many times the sensors are read per second,
the largest time between sensor readings and how often the time between readings
has taken longer than 10ms and 20ms.

23 Jun 11
Added 'Sample Interval' slider to options to control how often the sensors are
read.  Range is from 1ms to 10ms.  Added bouncing animations for fastest laps
to Twilight theme.








