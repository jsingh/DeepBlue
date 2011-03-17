using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.Data;

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
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("InvestorFunds", investorFund);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorFund);
					}
				}
				context.SaveChanges();
			}
		}
	}
}