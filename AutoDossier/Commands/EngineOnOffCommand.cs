using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoDossier.Commands
{

	public class EngineOnOffCommand : ICommand
	{

		#region Fields

		private ViewModels.FileSchemaViewModel _viewModel;

		#endregion


		#region Constructors/Destructors

		public EngineOnOffCommand(ViewModels.FileSchemaViewModel viewModel)
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
			_viewModel.EngineOnOff();
		}

		#endregion

	}

}
