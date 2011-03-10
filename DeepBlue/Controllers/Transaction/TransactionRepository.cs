using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Transaction;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionRepository : ITransactionRepository {
	
		#region ITransactionRepository Members

		public List<Models.Entity.Fund> GetAllFundNames() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						 orderby fund.FundName
						 select fund).ToList();
			}
		}

		public List<FundClosing> GetAllFundClosings(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return  (from fundClose in context.FundClosings
						where fundClose.FundID == fundId
						orderby fundClose.FundClosingDate descending
						select fundClose).ToList();
			}
		}

		#endregion
	}
}