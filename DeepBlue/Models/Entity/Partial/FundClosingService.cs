using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity {
	public interface IFundClosingService {
		void SaveFundClose(FundClosing fundclose);
	}
	public class FundClosingService : IFundClosingService {

		#region IFundClosingService Members

		public void SaveFundClose(FundClosing  fundclose) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fundclose.FundClosingID == 0) {
					context.FundClosings.AddObject(fundclose);
				} else {
					context.FundClosings.SingleOrDefault(entityType => entityType.FundClosingID == fundclose.FundClosingID);
					context.FundClosings.ApplyCurrentValues(fundclose);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}