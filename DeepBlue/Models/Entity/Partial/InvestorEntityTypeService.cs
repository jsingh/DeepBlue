using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity.Partial {
	public interface IInvestorEntityTypeService {
		void SaveInvestorEntityType(InvestorEntityType investor);
	}
	public class InvestorEntityTypeService : IInvestorEntityTypeService {

		#region IInvestorEntityTypeService Members

		public void SaveInvestorEntityType(InvestorEntityType investorEntityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorEntityType.InvestorEntityTypeID == 0) {
					context.InvestorEntityTypes.AddObject(investorEntityType);
				} else {
					var origianlEntityType = context.InvestorEntityTypes.SingleOrDefault(entityType => entityType.InvestorEntityTypeID == investorEntityType.InvestorEntityTypeID);
					context.InvestorEntityTypes.ApplyCurrentValues(investorEntityType);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}