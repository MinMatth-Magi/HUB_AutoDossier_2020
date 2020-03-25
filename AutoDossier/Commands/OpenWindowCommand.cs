using AutoDossier.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoDossier.Commands
{

	public class OpenWindowCommand : ICommand
	{

		public enum WindowType { SETTINGS };

		#region Fields

		private WindowType _windowType;
		private ViewModels.IViewModel _nestedViewModel;

		#endregion


		#region Constructors/Destructors

		public OpenWindowCommand(WindowType windowType, ViewModels.IViewModel nestedViewModel)
		{
			_windowType = windowType;
			_nestedViewModel = nestedViewModel;
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
			Window window = null;
			switch (_windowType)
			{
				case WindowType.SETTINGS:
					window = new SettingsWindow();
					break;
			}
			window.DataContext = _nestedViewModel;
			if (null != window)
				window.ShowDialog();
		}

		#endregion

	}

}
