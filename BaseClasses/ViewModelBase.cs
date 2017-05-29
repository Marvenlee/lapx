using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace LapX
{
	// Base class of ViewModel and other classes that are bound to by
	// WPF elements.  Used to implement NotifyPropertyChanged and
	// the SetProperty() method.
	//
	// Usage:
	//
	// public partial class MyWindow : WindowBase
	// {
	//		public int MyVal
	//		{
	//			get { return _MyVal; }
	//			set { SetProperty(ref _MyVal, value, "MyVal"); }
	//		}
	//		int _MyVal;
	// }
	//
	// At top of MyWindow.xaml
	// <src:WindowBase x:Class="MyWindow.OptionsWindow" >
	

	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void NotifyPropertyChanged(String info)
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
	}
}
