using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoDossier.Models
{

	[Serializable()]
	public class FolderSchema : ISerializable, INotifyPropertyChanged, ISchema
	{


		#region Fields

		private string _value;
		private ScopedData _data;
		private ObservableCollection<ISchema> _children;

		#endregion


		#region Constructors/Destructors

		public FolderSchema()
		{
			_data = new ScopedData();
			_children = new ObservableCollection<ISchema>();
		}

		public FolderSchema(FolderSchema model)
		{
			Value = model.Value;
			Data = model.Data;
			Children = model.Children;
		}

		public FolderSchema(SerializationInfo info, StreamingContext context)
		{
			Value = info.GetValue("Value", typeof(string)) as string;
			Data = info.GetValue("Data", typeof(ScopedData)) as ScopedData;
			Children = info.GetValue("Children", typeof(ObservableCollection<ISchema>)) as ObservableCollection<ISchema>;
		}

		#endregion


		#region Methodes

		#region ISerializable methodes

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Value", Value);
			info.AddValue("Data", Data);
			info.AddValue("Children", Children);
		}

		#endregion


		#region Serialization/Deserialization

		public void Serialize(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(FolderSchema));
			using (TextWriter tw = new StreamWriter(path))
				serializer.Serialize(tw, this);
		}

		public static FolderSchema Deserialize(string path)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(FolderSchema));
			object obj;
			using (TextReader reader = new StreamReader(path))
			{
				obj = deserializer.Deserialize(reader);
				reader.Close();
			}
			return obj as FolderSchema;
		}

		#endregion

		#endregion



		public void Copy(FolderSchema model)
		{
			Value = model.Value;
			Data = model.Data;
			Children = model.Children;
		}


		#region Properties

		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public ScopedData Data
		{
			get { return _data; }
			set
			{
				_data = value;
				OnPropertyChanged("Data");
			}
		}

		public ObservableCollection<ISchema> Children
		{
			get { return _children; }
			set
			{
				_children = value;
				OnPropertyChanged("Children");
			}
		}

		#endregion


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion


	}

}
