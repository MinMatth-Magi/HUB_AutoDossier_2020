using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoDossier.Commands
{

	public class AddFileSchemaCommand : ICommand
	{

		#region Fields

		private ViewModels.FolderSchemaViewModel _viewModel;

		#endregion


		#region Constructors/Destructors

		public AddFileSchemaCommand(ViewModels.FolderSchemaViewModel viewModel)
		{
			_viewModel = viewModel;
		}

		#endregion


		#region ICommand Members

		public event System.EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
		public bool CanExecute(object parameter)
		{
			return true;
		}
		public void Execute(object parameter)
		{
			_viewModel.AddSchema("file", parameter as ObservableCollection<Models.XmlAnything<Models.ISchema>>);
		}

		#endregion

	}

}
