using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;

namespace DeepBlue.Models.Entity {
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
						context.FundAccounts.SingleOrDefault(account => account.FundAccountID == fundAccount.FundAccountID);
						context.FundAccounts.ApplyCurrentValues(fundAccount);
					}
					foreach (var fundClosing in fund.FundClosings) {
						context.FundClosings.SingleOrDefault(closing => closing.FundClosingID == fundClosing.FundClosingID);
						context.FundClosings.ApplyCurrentValues(fundClosing);
					}
					context.Funds.SingleOrDefault(f => f.FundID == fund.FundID);
					context.Funds.ApplyCurrentValues(fund);
				}
				context.SaveChanges();
			}
		}
	}
}