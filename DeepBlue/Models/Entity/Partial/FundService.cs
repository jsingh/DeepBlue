using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace DeepBlue.Models.Entity.Partial {
	public interface IFundService {
		void SaveFund(Fund fund);
	}

	public class FundService : IFundService {
		public void SaveFund(Fund fund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fund.FundID == 0) {
					context.Funds.AddObject(fund);
				} else {
					//Update fund,fund account values
					foreach (var fundAccount in fund.FundAccounts) {
						context.FundAccounts.Attach(new FundAccount { FundAccountID = fundAccount.FundAccountID });
						context.FundAccounts.ApplyCurrentValues(fundAccount);
					}
					foreach(var fundClosing in fund.FundClosings){
						context.FundClosings.Attach(new FundClosing { FundClosingID = fundClosing.FundClosingID });
						context.FundClosings.ApplyCurrentValues(fundClosing);
					}
					context.Funds.Attach(new Fund { FundID = fund.FundID });
					context.Funds.ApplyCurrentValues(fund);
				}
				context.SaveChanges();
			}
		}
	}
}