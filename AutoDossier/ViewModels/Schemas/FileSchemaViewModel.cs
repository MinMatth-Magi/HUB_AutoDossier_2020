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

		private Models.FileSchema _schema;

		#endregion


		#region Constructors/Destructors

		public FileSchemaViewModel(Models.FileSchema schema)
		{
			_schema = schema;

			AddDataCommand = new Commands.AddDataCommand(this);
			RemoveDataCommand = new Commands.RemoveDataCommand(this);
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


		#endregion


		#region Members


		public Models.FileSchema Schema
		{
			get { return _schema; }
			set
			{
				_schema = value;
				OnPropertyChanged("Schema");
			}
		}


		#region Commands Members


		public ICommand AddDataCommand
		{ get; private set; }
		public ICommand RemoveDataCommand
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
