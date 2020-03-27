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

		private Models.MainSettings _mainSettings;

		private Models.XmlAnything<Models.ISchema> _schema;
		private FolderSchemaViewModel _parent;

		private ObservableCollection<ISchemaViewModel> _schemasViewModels;
		private Page _page = new Views.Pages.FolderSchemaPage();
		private FolderSchemaViewModel _selectedChild;
		private Views.Pages.FolderSchemaPage _childPage = new Views.Pages.FolderSchemaPage();

		private String _log;

		#endregion


		#region Constructors/Destructors

		public FolderSchemaViewModel(
			Models.MainSettings mainSettings,
			Models.FolderSchema schema,
			FolderSchemaViewModel parent,
			String log)
		{

			_log = log;
			_mainSettings = mainSettings;

			_schema = new Models.XmlAnything<Models.ISchema>();
			_schema.Value = schema;
			_parent = parent;

			_schemasViewModels = new ObservableCollection<ISchemaViewModel>();
			foreach (Models.XmlAnything<Models.ISchema> xmlChild in (_schema.Value as Models.FolderSchema).Children) {
				if (typeof(Models.FolderSchema) == xmlChild.Value.GetType())
					_schemasViewModels.Add(
						new FolderSchemaViewModel(
							mainSettings,
							xmlChild.Value as Models.FolderSchema,
							this,
							log
						)
					);
				if (typeof(Models.FileSchema) == xmlChild.Value.GetType())
					_schemasViewModels.Add(
						new FileSchemaViewModel(
							_mainSettings,
							xmlChild.Value as Models.FileSchema,
							this,
							log
						)
					);
			}

			AddDataCommand = new Commands.AddDataCommand(this);
			RemoveDataCommand = new Commands.RemoveDataCommand(this);
			AddFileSchemaCommand = new Commands.AddFileSchemaCommand(this);
			AddFolderSchemaCommand = new Commands.AddFolderSchemaCommand(this);
			RemoveSchemaCommand = new Commands.RemoveSchemaCommand(this);
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
				FileSchemaViewModel newSchema = new FileSchemaViewModel(_mainSettings, new Models.FileSchema(), this, _log);
				Models.XmlAnything<Models.ISchema> xmlAnything = new Models.XmlAnything<Models.ISchema>();
				xmlAnything.Value = newSchema.Schema;
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(xmlAnything);
			}
			if ("folder" == mode) {
				FolderSchemaViewModel newSchema = new FolderSchemaViewModel(_mainSettings, new Models.FolderSchema(), this, _log);
				Models.XmlAnything<Models.ISchema> xmlAnything = new Models.XmlAnything<Models.ISchema>();
				xmlAnything.Value = newSchema.Schema;
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(xmlAnything);
			}
		}


		public void RemoveSchema(ISchemaViewModel schema)
		{
			ChildrenViewModels.Remove(schema);
			if (typeof(FolderSchemaViewModel) == schema.GetType())
				Schema.Children.Remove((schema as FolderSchemaViewModel)._schema);
			if (typeof(FileSchemaViewModel) == schema.GetType())
				Schema.Children.Remove((schema as FileSchemaViewModel).XmlSchema);
		}


		#endregion


		#region Members


		public Models.FolderSchema Schema
		{
			get { return _schema.Value as Models.FolderSchema; }
			set
			{
				_schema.Value = value;
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

		public FolderSchemaViewModel Parent
		{
			get { return _parent; }
			set
			{
				_parent = value;
				OnPropertyChanged("Parent");
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
		public ICommand RemoveSchemaCommand
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
