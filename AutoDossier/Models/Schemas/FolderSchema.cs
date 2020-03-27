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

		private string _tag;
		private string _value;
		private ScopedData _data;
		private ObservableCollection<XmlAnything<ISchema>> _children;

		#endregion


		#region Constructors/Destructors

		public FolderSchema()
		{
			_data = new ScopedData();
			_children = new ObservableCollection<XmlAnything<ISchema>>();
		}

		public FolderSchema(FolderSchema model)
		{
			Tag = model.Tag;
			Value = model.Value;
			Data = model.Data;
			Children = model.Children;
		}

		public FolderSchema(SerializationInfo info, StreamingContext context)
		{
			Tag = info.GetValue("Tag", typeof(string)) as string;
			Value = info.GetValue("Value", typeof(string)) as string;
			Data = info.GetValue("Data", typeof(ScopedData)) as ScopedData;
			Children = info.GetValue("Children", typeof(ObservableCollection<XmlAnything<ISchema>>)) as ObservableCollection<XmlAnything<ISchema>>;
		}

		public void Copy(FolderSchema model)
		{
			Tag = model.Tag;
			Value = model.Value;
			Data.Copy(model.Data);
			Children = new ObservableCollection<XmlAnything<ISchema>>();
			foreach (XmlAnything<ISchema> schema in model.Children) {
				if (typeof(FolderSchema) == schema.Value.GetType()) {
					FolderSchema tmp = new FolderSchema();
					tmp.Copy(schema.Value as FolderSchema);
					Children.Add(new XmlAnything<ISchema>(tmp));
				}
				if (typeof(FileSchema) == schema.Value.GetType()) {
					FileSchema tmp = new FileSchema();
					tmp.Copy(schema.Value as FileSchema);
					Children.Add(new XmlAnything<ISchema>(tmp));
				}
			}
		}

		#endregion


		#region Methodes

		#region ISerializable methodes

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Tag", Tag);
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


		#region Properties

		public string Tag
		{
			get { return _tag; }
			set
			{
				_tag = value;
				OnPropertyChanged("Tag");
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

		public ScopedData Data
		{
			get { return _data; }
			set
			{
				_data = value;
				OnPropertyChanged("Data");
			}
		}

		public ObservableCollection<XmlAnything<ISchema>> Children
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
