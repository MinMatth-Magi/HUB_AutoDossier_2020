using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace AutoDossier.ViewModels
{

	public class MainViewModel : IViewModel, INotifyPropertyChanged
	{

		#region Fields

		private Models.FolderSchema _arborescence;
		private SettingsViewModel _settingsViewModel;
		private ObservableCollection<Enums.ErrorCode> _errorCodes;
		private FolderSchemaViewModel _arborescenceViewModel;

		private Models.MainSettings _mainSettings;

		private String _log;

		#endregion


		#region Constructors/Destructors

		public MainViewModel()
		{
			_log = "";
			_mainSettings = new Models.MainSettings() { ScanFolder = "C:\\Users\\MinMatth-Magi\\Desktop\\Input", ScanFile="Fichier" };
			try {
				XmlSerializer xs = new XmlSerializer(typeof(Models.FolderSchema));
				using (StreamReader rd = new StreamReader("Resources/Settings/arborescence.xml")) {
					_arborescence = xs.Deserialize(rd) as Models.FolderSchema;
				}
			} catch (Exception) {
				_arborescence = new Models.FolderSchema();
			}
			_settingsViewModel = _settingsViewModel = new SettingsViewModel(_mainSettings, _arborescence, _log);
			_errorCodes = new ObservableCollection<Enums.ErrorCode> { Enums.ErrorCode.NO_ERROR };
			_arborescenceViewModel = new FolderSchemaViewModel(_mainSettings, _arborescence, null, _log);

			OpenWindowCommand = new Commands.OpenWindowCommand(Commands.OpenWindowCommand.WindowType.SETTINGS, _settingsViewModel);
			DummyDebugCommand = new Commands.DummyDebugCommand(this);
		}

		#endregion


		#region Methodes


		#region Dummy Debug Methodes


		public void Debug()
		{ }


		#endregion


		#endregion


		#region Members


		public Models.FolderSchema Arborescence
		{
			get { return _arborescence; }
			private set {
				_arborescence = value;
				OnPropertyChanged("Arborescence");
			}
		}


		public FolderSchemaViewModel ArborescenceViewModel
		{
			get { return _arborescenceViewModel; }
			set {
				_arborescenceViewModel = value;
				OnPropertyChanged("ArborescenceViewModel");
			}
		}


		public ObservableCollection<Enums.ErrorCode> ErrorCodes
		{
			get { return _errorCodes; }
			private set {
				_errorCodes = value;
				OnPropertyChanged("ErrorCodes");
			}
		}

		public String Log
		{
			get { return _log; }
			set
			{
				_log = value;
				OnPropertyChanged("Log");
			}
		}


		#region Command Members


		public ICommand OpenWindowCommand
		{ get; private set; }

		public ICommand DummyDebugCommand
		{ get; private set; }


		#endregion



		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion



		#endregion

	}

}
