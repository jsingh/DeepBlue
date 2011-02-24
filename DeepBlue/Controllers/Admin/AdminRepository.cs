using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;

namespace DeepBlue.Controllers.Admin {
	public class AdminRepository : IAdminRepository {
	
		DeepBlueEntities DeepBlueDb = new DeepBlueEntities();

		#region IAdminRepository Members

		public List<Models.Entity.InvestorEntityType> GetAllInvestorEntityTypes() {
			return (from entityType in DeepBlueDb.InvestorEntityTypes
					orderby entityType.InvestorEntityTypeName
					select entityType).ToList();
		}

		public void AddEntiryType(Models.Entity.Investor investor) {
			throw new NotImplementedException();
		}

		public void SaveEntiryType() {
			throw new NotImplementedException();
		}

		#endregion
	}
}