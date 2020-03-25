using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoDossier.ViewModels
{

	public interface IChanges
	{

		void ApplyChanges();
		void CancelChanges();

	}

}
