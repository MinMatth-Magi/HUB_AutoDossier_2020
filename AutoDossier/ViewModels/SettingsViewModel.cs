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

	public class SettingsViewModel : IViewModel, INotifyPropertyChanged, IChanges
	{


		#region Fields

		private String _log;

		private Models.MainSettings _mainSettings;

		private ObservableCollection<Page> _pages;
		private Models.FolderSchema _arborescence;
		private Models.FolderSchema _arborescenceModified;
		private FolderSchemaViewModel _arborescenceViewModel;

		#endregion


		#region Constructors/Destructors

		public SettingsViewModel(Models.MainSettings mainSettings, Models.FolderSchema mvp, String log)
		{
			_log = log;
			_mainSettings = mainSettings;
			_arborescence = mvp;
			_arborescenceModified = new Models.FolderSchema();
			_arborescenceModified.Copy(_arborescence);
			_arborescenceViewModel = new FolderSchemaViewModel(_mainSettings, _arborescenceModified, null, _log);

			_pages = new ObservableCollection<Page> {
				new Views.Pages.GeneralSettings { DataContext = this, Title = "General" },
				new Views.Pages.DatasPage { DataContext = this, Title = "Schema" }
			};
			ChangesCommand = new Commands.ChangesCommand(this);
		}

		#endregion


		#region Methodes


		#region IChanges Methodes

		public void ApplyChanges()
		{
			_arborescence.Copy(_arborescenceModified);
			XmlSerializer xs = new XmlSerializer(typeof(Models.FolderSchema));
			using (StreamWriter wr = new StreamWriter("Resources/Settings/Arborescence.xml"))
			{
				xs.Serialize(wr, _arborescence);
			}
		}

		public void CancelChanges()
		{
			_arborescenceModified.Copy(_arborescence);
			ArborescenceViewModel = new FolderSchemaViewModel(_mainSettings, _arborescenceModified, null, _log);
			OnPropertyChanged("Arborescence");
		}

		#endregion


		#region Dummy Debugging Methodes


		public void Debug()
		{}


		#endregion


		public void AddSchema(string mode, ObservableCollection<Models.ISchema> schemaList)
		{
			if ("file" == mode)
				schemaList.Add(new Models.FileSchema());
			if ("folder" == mode)
				schemaList.Add(new Models.FolderSchema());
		}


		#endregion


		#region Members

		public Models.MainSettings MainSettings
		{
			get { return _mainSettings; }
			set
			{
				_mainSettings = value;
				OnPropertyChanged("MainSettings");
			}
		}


		public ObservableCollection<Page> Pages
		{
			get { return _pages; }
			set
			{
				_pages = value;
				OnPropertyChanged("Pages");
			}
		}

		public Models.FolderSchema Arborescence
		{
			get { return _arborescenceModified; }
			set
			{
				_arborescenceModified = value;
				OnPropertyChanged("Arborescence");
			}
		}


		public FolderSchemaViewModel ArborescenceViewModel
		{
			get { return _arborescenceViewModel; }
			set
			{
				_arborescenceViewModel = value;
				OnPropertyChanged("ArborescenceViewModel");
			}
		}


		#region Commands Members

		public ICommand ChangesCommand
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
