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
	public class FileSchema : ISerializable, INotifyPropertyChanged, ISchema
	{


		#region Fields

		private string _value;
		private ScopedData _data;

		#endregion


		#region Constructors/Destructors

		public FileSchema()
		{
			_data = new ScopedData();
		}

		public FileSchema(FileSchema model)
		{
			Value = model.Value;
			Data = model.Data;
		}

		public FileSchema(SerializationInfo info, StreamingContext context)
		{
			Value = info.GetValue("Value", typeof(string)) as string;
			Data = info.GetValue("Data", typeof(ScopedData)) as ScopedData;
		}

		#endregion


		#region Methodes

		#region ISerializable methodes

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Value", Value);
			info.AddValue("Data", Data);
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


		public void Copy(FileSchema model)
		{
			Value = model.Value;
			Data = model.Data;
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

		#endregion


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion


	}

}
