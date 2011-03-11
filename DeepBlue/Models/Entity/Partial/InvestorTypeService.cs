using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity {
	public interface IInvestorTypeService {
		void SaveInvestorType(InvestorType investor);
	}
	public class InvestorTypeService : IInvestorTypeService {

		#region IInvestorTypeService Members

		public void SaveInvestorType(InvestorType investorEntityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorEntityType.InvestorTypeID == 0) {
					context.InvestorTypes.AddObject(investorEntityType);
				} else {
					var origianlEntityType = context.InvestorTypes.SingleOrDefault(entityType => entityType.InvestorTypeID == investorEntityType.InvestorTypeID);
					context.InvestorTypes.ApplyCurrentValues(investorEntityType);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}