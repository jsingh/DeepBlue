using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	public interface IInvestorFundService {
		void SaveInvestorFund(InvestorFund investor);
		void UpdateInvestorFund(DeepBlueEntities context);
	}

	public class InvestorFundService : IInvestorFundService {
		public void SaveInvestorFund(InvestorFund investorFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				context.InvestorFunds.AddObject(investorFund);
				context.SaveChanges();
			}
		}
		public void UpdateInvestorFund(DeepBlueEntities context) {
			context.SaveChanges();
		}
	}
}