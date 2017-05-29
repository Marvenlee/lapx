using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace LapX
{
	// Base class of Windows to help with data binding.  Similar to the
	// ViewModelBase class.  Used to implement NotifyPropertyChanged
	// and
	// the SetProperty() method.
	//
	// Usage:
	//	
	// public int MyVal
	// {
	//      get { return _MyVal; }
	//      set { SetProperty(ref _MyVal, value, "MyVal"); }
	// }
	// int _MyVal;
	//
	// 
	//

	public class WindowBase : Window, INotifyPropertyChanged
	{
		#region NotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public bool SetProperty<T>(ref T backingField, T value, string propertyName)
		{
			var changed = !EqualityComparer<T>.Default.Equals(backingField, value);

			if (changed)
			{
				backingField = value;
				NotifyPropertyChanged(propertyName);
			}
			return changed;
		}

		#endregion
	}
}
