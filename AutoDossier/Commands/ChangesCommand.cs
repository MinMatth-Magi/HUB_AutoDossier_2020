﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoDossier.Commands
{

	public class ChangesCommand : ICommand
	{

		#region Fields

		private ViewModels.IChanges _viewModel;

		#endregion


		#region Constructors/Destructors

		public ChangesCommand(ViewModels.IChanges viewModel)
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
			if ("Apply" == parameter.ToString())
				_viewModel.ApplyChanges();
			else if ("Cancel" == parameter.ToString())
				_viewModel.CancelChanges();
		}

		#endregion

	}

}
