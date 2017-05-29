using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;


namespace LapX
{
	/// <summary>
	/// Interaction logic for FileBrowser.xaml
	/// </summary>
	public partial class FileBrowser : UserControl
	{
		public static readonly DependencyProperty FileNameProperty =
				DependencyProperty.Register("FileName",
				typeof(string), typeof(FileBrowser),
				new FrameworkPropertyMetadata(string.Empty,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string FileName
		{
			get { return (string)GetValue(FileNameProperty); }
			set { SetValue(FileNameProperty, value); }
		}

		

		public FileBrowser()
		{
			InitializeComponent();
		}
		

		private void BrowseButtonClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog FileDialog = new OpenFileDialog();

			// TO-DO: Make Filter a dependency property
			
			FileDialog.Filter = "Sound files (*.wav)|*.wav";
			FileDialog.AddExtension = true;
			FileDialog.InitialDirectory = Directory.GetCurrentDirectory();
			FileDialog.RestoreDirectory = true;
			if (FileDialog.ShowDialog() == true)
				this.FileName = FileDialog.FileName;
		}
	}
}
