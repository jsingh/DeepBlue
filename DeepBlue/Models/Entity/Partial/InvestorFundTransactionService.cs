using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
    public interface IInvestorFundTransactionService {
        void SaveInvestorFundTransaction(InvestorFundTransaction investor);
    }

	public class InvestorFundTransactionService : IInvestorFundTransactionService {
		public void SaveInvestorFundTransaction(InvestorFundTransaction investorFundTransaction) {
            using (DeepBlueEntities context = new DeepBlueEntities()) {
				context.InvestorFundTransactions.AddObject(investorFundTransaction);
                context.SaveChanges();
            }
        }
    }
}