using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;

namespace DeepBlue.Controllers.Issuer {
	public class IssuerRepository : IIssuerRepository {
		List<IssuerDetailModel> IIssuerRepository.GetAllIssuers() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from issuer in context.Issuers
						select new IssuerDetailModel { IssuerId = issuer.IssuerID, Name = issuer.Name }).ToList();
			}
		}

		public List<EquityDetailModel> GetAllEquity(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.IssuerID == issuerId
						select new EquityDetailModel {
							EquityId = equity.EquityID,
							Symbol = equity.Symbol
						}).ToList();
			}
		}

		public List<FixedIncomeDetailModel> GetAllFixedIncome(int issuerId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.IssuerID == issuerId
						select new FixedIncomeDetailModel {
							FixedIncomeId = fixedIncome.FixedIncomeID,
							Symbol = fixedIncome.Symbol
						}).ToList();
			}
		}

		public EquityDetailModel FindEquity(int equityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equity in context.Equities
						where equity.EquityID == equityId
						select new EquityDetailModel {
							EquityId = equity.EquityID,
							Symbol = equity.Symbol,
							IssuerId = equity.IssuerID,
							IssuerName = equity.Issuer.Name
						}).SingleOrDefault();
			}
		}

		public FixedIncomeDetailModel FindFixedIncome(int fixedIncomeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncome in context.FixedIncomes
						where fixedIncome.FixedIncomeID == fixedIncomeId
						select new FixedIncomeDetailModel {
							FixedIncomeId = fixedIncome.FixedIncomeID,
							Symbol = fixedIncome.Symbol,
							IssuerId = fixedIncome.IssuerID,
							IssuerName = fixedIncome.Issuer.Name
						}).SingleOrDefault();
			}
		}
	}
}