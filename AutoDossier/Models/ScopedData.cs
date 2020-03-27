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
	public class ScopedData : ISerializable, INotifyPropertyChanged
	{


		#region Fields

		private ObservableCollection<Data> _scopedDatas;

		#endregion


		#region Constructors/Destructors

		public ScopedData()
		{
			ScopedDatas = new ObservableCollection<Data> { };
		}

		public ScopedData(ScopedData scopedData)
		{
			ScopedDatas = new ObservableCollection<Data>();
			foreach (Data data in scopedData.ScopedDatas)
				ScopedDatas.Add(data);
		}

		public ScopedData(SerializationInfo info, StreamingContext context)
		{
			ScopedDatas = info.GetValue("ScopedDatas", typeof(ObservableCollection<Data>)) as ObservableCollection<Data>;
		}

		#endregion


		#region Methodes

		// Anytime you add a new field that you'd like to backup in serialization, add it here too.
		#region ISerializable methodes

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ScopedDatas", ScopedDatas);
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


		#endregion


		public void Copy(ScopedData scopedData)
		{
			ScopedDatas = new ObservableCollection<Data>();
			foreach (Data data in scopedData.ScopedDatas)
				ScopedDatas.Add(data);
		}


		#region Properties

		public ObservableCollection<Data> ScopedDatas
		{
			get { return _scopedDatas; }
			set
			{
				_scopedDatas = value;
				OnPropertyChanged("ScopedDatas");
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
