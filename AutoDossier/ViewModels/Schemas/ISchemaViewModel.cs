using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDossier.ViewModels
{

	public interface ISchemaViewModel
	{

		void AddData(Models.ScopedData data);
		void RemoveData(Models.Data data);

	}

}
