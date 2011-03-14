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

		public void SaveFundClose(FundClosing  fundcloseid) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fundcloseid.FundClosingID == 0) {
					context.FundClosings.AddObject(fundcloseid);
				} else {
					context.FundClosings.SingleOrDefault(entityType => entityType.FundClosingID == fundcloseid.FundClosingID);
					context.FundClosings.ApplyCurrentValues(fundcloseid);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}