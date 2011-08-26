using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.Data;

namespace DeepBlue.Models.Entity {
    public interface IInvestorFundTransactionService {
        void SaveInvestorFundTransaction(InvestorFundTransaction investor);
    }

	public class InvestorFundTransactionService : IInvestorFundTransactionService {
		public void SaveInvestorFundTransaction(InvestorFundTransaction investorFundTransaction) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorFundTransaction.InvestorFundTransactionID == 0) {
					context.InvestorFundTransactions.AddObject(investorFundTransaction);
				}
				else {
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("InvestorFundTransactions", investorFundTransaction);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorFundTransaction);
					}
				}
				context.SaveChanges();
			}
        }
    }
}