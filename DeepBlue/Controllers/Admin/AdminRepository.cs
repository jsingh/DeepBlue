using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;

namespace DeepBlue.Controllers.Admin {
	public class AdminRepository : IAdminRepository {

		#region IAdminRepository Members

		public List<Models.Entity.InvestorEntityType> GetAllInvestorEntityTypes() {
			List<Models.Entity.InvestorEntityType> entityTypes;
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				entityTypes = (from entityType in context.InvestorEntityTypes
							   orderby entityType.InvestorEntityTypeName
							   select entityType).ToList();
			}
			return entityTypes;
		}
		
		#endregion
	}
}