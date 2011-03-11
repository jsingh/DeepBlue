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
					context.InvestorFunds.SingleOrDefault(fund => fund.InvestorFundID == investorFund.InvestorFundID);
					context.InvestorFunds.ApplyCurrentValues(investorFund);
				}
				context.SaveChanges();
			}
		}
	}
}