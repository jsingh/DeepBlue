using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	public interface IInvestorFundService {
		void SaveInvestorFund(InvestorFund investor);
	}

	public class InvestorFundService : IInvestorFundService {
		public void SaveInvestorFund(InvestorFund investorFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorFund.InvestorFundID == 0) {
					context.InvestorFunds.AddObject(investorFund);
				} else {
					foreach(var investorFundTransaction in investorFund.InvestorFundTransactions){
						context.InvestorFundTransactions.Attach(new InvestorFundTransaction { InvestorFundTransactionID = investorFundTransaction.InvestorFundTransactionID });
						context.InvestorFundTransactions.ApplyCurrentValues(investorFundTransaction);
					}
					context.InvestorFunds.Attach(new InvestorFund { InvestorFundID = investorFund.InvestorFundID });
					context.InvestorFunds.ApplyCurrentValues(investorFund);
				}
				context.SaveChanges();
			}
		}
	}
}