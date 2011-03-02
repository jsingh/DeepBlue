using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Transaction;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionRepository : ITransactionRepository {

		DeepBlueEntities DeepBlueContext = new DeepBlueEntities();
		
		#region ITransactionRepository Members

		public List<Models.Entity.Fund> GetAllFundNames() {
			return (from fund in DeepBlueContext.Funds
					orderby fund.FundName
					select fund).ToList();
		}

		public List<Models.Entity.FundClosing> GetAllFundClosings() {
			return (from fundClose in DeepBlueContext.FundClosings
					orderby fundClose.FundClosingDate descending 
					select fundClose).ToList();
		}

	 
		#endregion
	}
}