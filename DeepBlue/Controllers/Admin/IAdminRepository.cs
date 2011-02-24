using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;

namespace DeepBlue.Controllers.Admin {
	public interface IAdminRepository {
		
		#region EntiryType
		List<InvestorEntityType> GetAllInvestorEntityTypes();
		void AddEntiryType(DeepBlue.Models.Entity.Investor investor);
		void SaveEntiryType();
		#endregion
		

	}
}
