using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Transaction;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionRepository : ITransactionRepository {

		DeepBlueEntities DeepBlueDb = new DeepBlueEntities();
		
		#region ITransactionRepository Members

		public List<Models.Entity.Fund> GetAllFundNames() {
			return (from fund in DeepBlueDb.Funds
					orderby fund.FundName
					select fund).ToList();
		}

		public List<Models.Entity.FundClosing> GetAllFundClosings() {
			return (from fundClose in DeepBlueDb.FundClosings
					orderby fundClose.FundClosingDate descending 
					select fundClose).ToList();
		}

		public void AddInvestorFund(InvestorFund investorFund) {
			DeepBlueDb.InvestorFunds.AddObject(investorFund);
		}
		
		public void Save() {
			DeepBlueDb.SaveChanges();
		}

		#endregion
	}
}