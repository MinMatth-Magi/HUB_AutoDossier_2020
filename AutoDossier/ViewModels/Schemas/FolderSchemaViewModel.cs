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

	public class FolderSchemaViewModel : IViewModel, INotifyPropertyChanged, ISchemaViewModel
	{


		#region Fields

		private Models.FolderSchema _schema;
		private ObservableCollection<ISchemaViewModel> _schemasViewModels;
		private Page _page = new Views.Pages.FolderSchemaPage();
		private FolderSchemaViewModel _selectedChild;
		private Views.Pages.FolderSchemaPage _childPage = new Views.Pages.FolderSchemaPage();

		#endregion


		#region Constructors/Destructors

		public FolderSchemaViewModel(Models.FolderSchema schema)
		{
			_schema = schema;
			_schemasViewModels = new ObservableCollection<ISchemaViewModel>();
			foreach (Models.XmlAnything<Models.ISchema> xmlChild in  _schema.Children) {
				if (typeof(Models.FolderSchema) == xmlChild.Value.GetType())
					_schemasViewModels.Add(new FolderSchemaViewModel(xmlChild.Value as Models.FolderSchema));
				if (typeof(Models.FileSchema) == xmlChild.Value.GetType())
					_schemasViewModels.Add(new FileSchemaViewModel(xmlChild.Value as Models.FileSchema));
			}

			AddDataCommand = new Commands.AddDataCommand(this);
			RemoveDataCommand = new Commands.RemoveDataCommand(this);
			AddFileSchemaCommand = new Commands.AddFileSchemaCommand(this);
			AddFolderSchemaCommand = new Commands.AddFolderSchemaCommand(this);
		}

		#endregion


		#region Methodes


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
			Schema.Data.ScopedDatas.Remove(data);
		}


		public void AddSchema(string mode, ObservableCollection<Models.XmlAnything<Models.ISchema>> schemaList)
		{
			if ("file" == mode) {
				FileSchemaViewModel newSchema = new FileSchemaViewModel(new Models.FileSchema());
				Models.XmlAnything<Models.ISchema> xmlAnything = new Models.XmlAnything<Models.ISchema>();
				xmlAnything.Value = newSchema.Schema;
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(xmlAnything);
			}
			if ("folder" == mode) {
				FolderSchemaViewModel newSchema= new FolderSchemaViewModel(new Models.FolderSchema());
				Models.XmlAnything<Models.ISchema> xmlAnything = new Models.XmlAnything<Models.ISchema>();
				xmlAnything.Value = newSchema.Schema;
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(xmlAnything);
			}
		}


		#endregion


		#region Members


		public Models.FolderSchema Schema
		{
			get { return _schema; }
			set
			{
				_schema = value;
				OnPropertyChanged("Schema");
			}
		}


		public FolderSchemaViewModel SelectedChild
		{
			get { return _selectedChild; }
			set {
				_selectedChild = value;
				OnPropertyChanged("SelectedChild");
				ChildPage = new Views.Pages.FolderSchemaPage()
					{ DataContext = _selectedChild };
			}
		}


		public Page ChildPage
		{
			get { return (null != SelectedChild) ? _childPage : null; }
			private set
			{
				_childPage = value as Views.Pages.FolderSchemaPage;
				OnPropertyChanged("ChildPage");
			}
		}


		public ObservableCollection<ISchemaViewModel> ChildrenViewModels
		{
			get { return _schemasViewModels; }
			set
			{
				_schemasViewModels = value;
				OnPropertyChanged("ChildrenViewModels");
			}
		}


		public ObservableCollection<ISchemaViewModel> FolderChildrenViewModels
		{
			get {
				ObservableCollection<ISchemaViewModel> folderChildrenViewModels = new ObservableCollection<ISchemaViewModel>();
				foreach (ISchemaViewModel schemaViewModel in _schemasViewModels)
					if (typeof(FolderSchemaViewModel) == schemaViewModel.GetType())
						folderChildrenViewModels.Add(schemaViewModel);
				return folderChildrenViewModels;
			}
		}


		public ObservableCollection<ISchemaViewModel> FileChildrenViewModels
		{
			get {
				ObservableCollection<ISchemaViewModel> fileChildrenViewModels = new ObservableCollection<ISchemaViewModel>();
				foreach (ISchemaViewModel schemaViewModel in _schemasViewModels)
					if (typeof(FileSchemaViewModel) == schemaViewModel.GetType())
						fileChildrenViewModels.Add(schemaViewModel);
				return fileChildrenViewModels;
			}
		}


		#region Commands Members


		public ICommand AddDataCommand
		{ get; private set; }
		public ICommand RemoveDataCommand
		{ get; private set; }

		public ICommand AddFileSchemaCommand
		{ get; private set; }
		public ICommand AddFolderSchemaCommand
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
