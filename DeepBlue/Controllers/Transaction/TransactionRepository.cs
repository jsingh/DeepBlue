using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Transaction;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionRepository : ITransactionRepository {

		#region TransactionRepository Members

		public List<Models.Entity.Fund> GetAllFundNames() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						orderby fund.FundName
						select fund).ToList();
			}
		}

		public List<FundClosing> GetAllFundClosings(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fundClose in context.FundClosings
						where fundClose.FundID == fundId
						orderby fundClose.FundClosingDate descending
						select fundClose).ToList();
			}
		}

		public object FindInvestorFundDetail(int investorFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorFund in context.InvestorFunds
						where investorFund.InvestorFundID == investorFundId
						select new {
							InvestorName = investorFund.Investor.InvestorName,
							InvestorId = investorFund.Investor.InvestorID,
							InvestorFundId = investorFund.InvestorFundID,
							OriginalCommitmentAmount = investorFund.TotalCommitment,
							UnfundedAmount = investorFund.UnfundedAmount ?? 0,
							Date = DateTime.Now,
							CounterPartyInvestorId = 0,
							CounterPartyInvestorName = string.Empty,
							TransactionTypeId = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell,
							FundId = investorFund.FundID
						}).ToList();
			}
		}

		#endregion
	}
}