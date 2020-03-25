using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutoDossier.Converters.TemplateSelector
{

	public class SchemaEditTemplateSelector : DataTemplateSelector
	{

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			ContentPresenter contentPresenter = container as ContentPresenter;
			ViewModels.ISchemaViewModel element = contentPresenter.Content as ViewModels.ISchemaViewModel;

			if (typeof(ViewModels.FolderSchemaViewModel) == element.GetType())
				return contentPresenter.FindResource("FolderSchemaEditTemplate") as DataTemplate;
			if (typeof(ViewModels.FileSchemaViewModel) == element.GetType())
				return contentPresenter.FindResource("FileSchemaEditTemplate") as DataTemplate;
			return null;
		}

	}

}
