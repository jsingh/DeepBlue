using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;

namespace DeepBlue.Controllers.Issuer {
	public interface IIssuerRepository {
		List<IssuerDetailModel> GetAllIssuers();
		List<EquityDetailModel> GetAllEquity(int issuerId);
		List<FixedIncomeDetailModel> GetAllFixedIncome(int issuerId);

		EquityDetailModel FindEquity(int equityId);
		FixedIncomeDetailModel FindFixedIncome(int fixedIncomeId);
	}
}
