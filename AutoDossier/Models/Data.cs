using System;
using System.Collections.Generic;
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
	public class Data : ISerializable, INotifyPropertyChanged
	{


		#region Fields

		private string _name;
		private string _value;

		#endregion


		#region Constructors/Destructors

		// Constructor
		public Data()
		{
			Name = ""; Value = "";
		}

		// Copy constructor
		public Data(Data model)
		{
			Name = model.Name;
			Value = model.Value;
		}

		// Deserialization constructor
		public Data(SerializationInfo info, StreamingContext context)
		{
			Name = info.GetValue("Name", typeof(string)) as string;
			Value = "";
		}

		#endregion


		#region Methodes


		// Anytime you add a new field that you'd like to backup in serialization, add it here too.
		#region ISerializable methodes

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", Name);
		}

		#endregion


		// This code doesn't require any change, unless you know exactly what you're doing.
		// Its purpose is to translate a file into a class and back.
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


		// Erase old data to copy model one
		public void Copy(Data model)
		{
			Name = model.Name;
			Value = model.Value;
		}


		#endregion


		#region Properties


		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}


		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
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
