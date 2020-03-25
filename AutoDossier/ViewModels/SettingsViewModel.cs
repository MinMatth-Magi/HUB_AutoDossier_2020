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

		private ObservableCollection<Page> _pages;
		private Models.FolderSchema _arborescence;
		private Models.ScopedData _globalData;
		private Models.FolderSchema _arborescenceModified;
		private Models.ScopedData _globalDataModified;
		private FolderSchemaViewModel _arborescenceViewModel;

		#endregion


		#region Constructors/Destructors

		public SettingsViewModel(Models.FolderSchema mvp, Models.ScopedData globalData)
		{
			_arborescence = mvp;
			_arborescenceModified = new Models.FolderSchema(_arborescence);
			_globalData = globalData;
			_globalDataModified = new Models.ScopedData(_globalData);
			_arborescenceViewModel = new FolderSchemaViewModel(_arborescenceModified);

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
			using (StreamWriter wr = new StreamWriter("Resources/Settings/mvp.xml"))
			{
				xs.Serialize(wr, _arborescence);
			}
			_globalData.Copy(_globalDataModified);
		}

		public void CancelChanges()
		{
			_arborescenceModified = new Models.FolderSchema(_arborescence);
			_globalDataModified = new Models.ScopedData(_globalData);
			OnPropertyChanged("Arborescence");
			OnPropertyChanged("GlobalData");
		}

		#endregion


		#region Dummy Debugging Methodes


		public void Debug()
		{}


		#endregion


		public void AddData(Models.ScopedData scopedData)
		{
			scopedData.ScopedDatas.Add(new Models.Data());
		}

		public void RemoveData(Models.Data data)
		{
			GlobalData.ScopedDatas.Remove(data);
		}


		public void AddSchema(string mode, ObservableCollection<Models.ISchema> schemaList)
		{
			if ("file" == mode)
				schemaList.Add(new Models.FileSchema());
			if ("folder" == mode)
				schemaList.Add(new Models.FolderSchema());
		}


		#endregion


		#region Members


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


		public Models.ScopedData GlobalData
		{
			get { return _globalDataModified; }
			set
			{
				_globalDataModified = value;
				OnPropertyChanged("GlobalData");
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
