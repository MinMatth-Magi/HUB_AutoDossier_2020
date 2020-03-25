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

		#endregion


		#region Constructors/Destructors

		public FolderSchemaViewModel(Models.FolderSchema schema)
		{
			_schema = schema;
			_schemasViewModels = new ObservableCollection<ISchemaViewModel>();
			foreach (Models.ISchema child in  _schema.Children)
			{
				if (typeof(Models.FolderSchema) == child.GetType())
					_schemasViewModels.Add(new FolderSchemaViewModel(child as Models.FolderSchema));
				if (typeof(Models.FileSchema) == child.GetType())
					_schemasViewModels.Add(new FileSchemaViewModel(child as Models.FileSchema));
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


		public void AddSchema(string mode, ObservableCollection<Models.ISchema> schemaList)
		{
			if ("file" == mode) {
				FileSchemaViewModel newSchema = new FileSchemaViewModel(new Models.FileSchema());
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(newSchema.Schema);
			}
			if ("folder" == mode) {
				FolderSchemaViewModel newSchema= new FolderSchemaViewModel(new Models.FolderSchema());
				ChildrenViewModels.Add(newSchema);
				schemaList.Add(newSchema.Schema);
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


		public ObservableCollection<ISchemaViewModel> ChildrenViewModels
		{
			get { return _schemasViewModels; }
			set
			{
				_schemasViewModels = value;
				OnPropertyChanged("ChildrenViewModels");
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
