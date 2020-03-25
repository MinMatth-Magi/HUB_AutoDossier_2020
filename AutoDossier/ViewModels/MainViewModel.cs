using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace AutoDossier.ViewModels
{

	public class MainViewModel : IViewModel, INotifyPropertyChanged
	{

		#region Fields

		private Models.FolderSchema _arborescence;
		private Models.ScopedData _globalData;
		private ViewModels.SettingsViewModel _settingsViewModel;
		private ObservableCollection<Enums.ErrorCode> _errorCodes;

		#endregion

		
		#region Constructors/Destructors

		public MainViewModel()
		{
			_globalData = new Models.ScopedData { ScopedDatas = new ObservableCollection<Models.Data> {
				new Models.Data { Name = "ScanFolder", Value = "usr/scans" }
			} };
			try {
				XmlSerializer xs = new XmlSerializer(typeof(Models.FolderSchema));
				using (StreamReader rd = new StreamReader("Resources/Settings/arborescence.xml")) {
					_arborescence = xs.Deserialize(rd) as Models.FolderSchema;
				}
			} catch (Exception) {
				_arborescence = new Models.FolderSchema() {
					Value = "C:/Desktop/AutoDossier",
					Children = new ObservableCollection<Models.ISchema>() {
						new Models.FolderSchema() {
							Value = "{{name}}"
						}
					}
				};
			}
			_settingsViewModel = _settingsViewModel = new SettingsViewModel(_arborescence, _globalData);
			_errorCodes = new ObservableCollection<Enums.ErrorCode> { Enums.ErrorCode.NO_ERROR };
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
			get
			{
				return _arborescence;
			}
			private set
			{
				_arborescence = value;
				OnPropertyChanged("Arborescence");
			}
		}


		public Models.ScopedData GlobalData
		{
			get
			{
				return _globalData;
			}
			private set
			{
				_globalData = value;
				OnPropertyChanged("GlobalData");
			}
		}


		public ObservableCollection<Enums.ErrorCode> ErrorCodes
		{
			get
			{
				return _errorCodes;
			}
			private set
			{
				_errorCodes = value;
				OnPropertyChanged("ErrorCodes");
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
