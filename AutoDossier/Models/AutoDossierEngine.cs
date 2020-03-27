using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoDossier.Models
{

	public class AutoDossierEngine : INotifyPropertyChanged
	{


		#region

		private MainSettings _mainSettings;
		private FileSchema _fileSchema;
		private FileSystemWatcher _watcherInput;
		private bool _isActive;
		//private FileSystemWatcher _watcherOutput;
		private ViewModels.FolderSchemaViewModel _parent;
		private String _log;

		#endregion


		#region

		public AutoDossierEngine(
			MainSettings mainSettings,
			FileSchema fileSchema,
			ViewModels.FolderSchemaViewModel parent,
			String log)
		{
			_log = log;
			_mainSettings = mainSettings;
			_fileSchema = fileSchema;
			_isActive = false;
			_parent = parent;
		}

		#endregion


		public bool IsActive
		{
			get { return _isActive; }
			set {
				_isActive = value;
				OnPropertyChanged("IsActive");
			}
		}


		public void Activate()
		{
			if (Directory.Exists(_mainSettings.ScanFolder)) {
				OnPropertyChanged("Log");
				_watcherInput = new FileSystemWatcher(_mainSettings.ScanFolder);
				_watcherInput.Created += new FileSystemEventHandler(ActionTriggered);
				_watcherInput.EnableRaisingEvents = true;
				MessageBox.Show("Now watching \"" + _watcherInput.Path + "\"");
				IsActive = true;
			} else
				MessageBox.Show("Scan Folder (\"" + _mainSettings.ScanFolder + "\") can't be found");
		}

		public void Deactivate()
		{
			_watcherInput.Dispose();
			_watcherInput = null;
			IsActive = false;
		}

		private void ActionTriggered(object source, FileSystemEventArgs e)
		{
			if (Path.GetFileNameWithoutExtension(e.Name) == Path.GetFileNameWithoutExtension(_mainSettings.ScanFile))
			{
				MessageBox.Show("The expected file has been detected.");
				string path = Path.Combine(getFolder(_parent), _fileSchema.Value) + Path.GetExtension(e.Name);
				List<Data> datas = new List<Data>();
				getAllDatas(_parent, datas);
				MessageBox.Show(path);
				foreach (Data data in datas)
					path = path.Replace("{{" + data.Name + "}}", data.Value);
				MessageBox.Show(path);
				createIntermediateFolders(Path.GetDirectoryName(path));
				if (File.Exists(path))
					MessageBox.Show("Unable to move file: Destination file already exists");
				else
					File.Move(e.FullPath, path);
			}
		}

		private string getFolder(ViewModels.FolderSchemaViewModel parent)
		{
			if (null == parent.Parent)
				return parent.Schema.Value;
			else
				return Path.Combine(getFolder(parent.Parent), parent.Schema.Value);
		}


		private void getAllDatas(ViewModels.FolderSchemaViewModel parent, List<Data> datas)
		{
			if (null != parent.Parent)
				getAllDatas(parent.Parent, datas);
			foreach (Data data in parent.Schema.Data.ScopedDatas)
				datas.Add(data);
		}

		private void createIntermediateFolders(string path)
		{
			string directoryPath = Path.GetDirectoryName(path);
			bool existance = Directory.Exists(directoryPath);
			if (!Directory.Exists(directoryPath)) {
				createIntermediateFolders(directoryPath);
				Directory.CreateDirectory(path);
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{ PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

		#endregion


	}

}
