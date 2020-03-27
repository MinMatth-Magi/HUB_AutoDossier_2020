using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDossier.Models
{

	public class MainSettings : INotifyPropertyChanged
	{

		private string _scanFolder;
		private string _scanFile;

		public MainSettings()
		{
		}

		public string ScanFolder
		{
			get { return _scanFolder; }
			set
			{
				_scanFolder = value;
				OnPropertyChanged("ScanFolder");
			}
		}

		public string ScanFile
		{
			get { return _scanFile; }
			set
			{
				_scanFile = value;
				OnPropertyChanged("ScanFile");
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion

	}

}
