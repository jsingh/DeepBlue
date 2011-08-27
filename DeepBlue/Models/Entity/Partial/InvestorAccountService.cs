using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IInvestorAccountService {
		void SaveInvestorAccount(InvestorAccount investorAccount);
	}

	public class InvestorAccountService : IInvestorAccountService {
		public void SaveInvestorAccount(InvestorAccount investorAccount) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorAccount.InvestorAccountID == 0) {
					context.InvestorAccounts.AddObject(investorAccount);
				}
				else {
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("InvestorAccounts", investorAccount);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorAccount);
					}
				}
				context.SaveChanges();
			}

		}
	}
}