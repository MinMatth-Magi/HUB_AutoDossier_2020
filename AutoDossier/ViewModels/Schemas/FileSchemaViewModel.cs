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

	public class FileSchemaViewModel : IViewModel, INotifyPropertyChanged, ISchemaViewModel
	{


		#region Fields

		private Models.MainSettings _mainSettings;

		private String _log;

		private Models.XmlAnything<Models.ISchema> _schema;
		private FolderSchemaViewModel _parent;

		private Models.AutoDossierEngine _engine;

		#endregion


		#region Constructors/Destructors

		public FileSchemaViewModel(
			Models.MainSettings mainSettings,
			Models.FileSchema schema,
			FolderSchemaViewModel parent,
			String log)
		{
			_log = log;
			_mainSettings = mainSettings;

			_schema = new Models.XmlAnything<Models.ISchema>();
			_schema.Value = schema;
			_parent = parent;

			_engine = new Models.AutoDossierEngine(_mainSettings, schema, parent, _log);

			AddDataCommand = new Commands.AddDataCommand(this);
			RemoveDataCommand = new Commands.RemoveDataCommand(this);
			EngineOnOffCommand = new Commands.EngineOnOffCommand(this);
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

		public void EngineOnOff()
		{
			if (_engine.IsActive)
				_engine.Deactivate();
			else
				_engine.Activate();
		}


		#endregion


		#region Members


		public Models.XmlAnything<Models.ISchema> XmlSchema
		{
			get { return _schema; }
		}


		public Models.FileSchema Schema
		{
			get { return _schema.Value as Models.FileSchema; }
			set
			{
				_schema.Value = value;
				OnPropertyChanged("Schema");
			}
		}


		public Models.AutoDossierEngine Engine
		{
			get { return _engine; }
			set
			{
				_engine = value;
				OnPropertyChanged("Engine");
			}
		}


		#region Commands Members


		public ICommand AddDataCommand
		{ get; private set; }
		public ICommand RemoveDataCommand
		{ get; private set; }

		public ICommand EngineOnOffCommand
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
