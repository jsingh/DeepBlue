﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;

namespace DeepBlue.Controllers.Issuer {
	public interface IIssuerRepository {

		#region Issuer
		List<IssuerDetailModel> GetAllIssuers();
		List<IssuerListModel> GetAllIssuers(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		bool DeleteIssuer(int issuerId);
		bool IssuerNameAvailable(string issuerName, int issuerId);
		IEnumerable<ErrorInfo> SaveIssuer(Models.Entity.Issuer issuer);
		Models.Entity.Issuer FindIssuer(int issuerId);
		EditIssuerModel FindIssuerModel(int issuerId);
		#endregion

		#region Equity
		List<Equity> GetAllEquity(int issuerId);
		EquityDetailModel FindEquityModel(int equityId);
		Equity FindEquity(int equityId);
		IEnumerable<ErrorInfo> SaveEquity(Equity equity);
		bool DeleteEquity(int id);
		#endregion

		#region FixedIncome
		FixedIncomeDetailModel FindFixedIncomeModel(int fixedIncomeId);
		FixedIncome FindFixedIncome(int fixedIncomeId);
		List<FixedIncome> GetAllFixedIncome(int issuerId);
		IEnumerable<ErrorInfo> SaveFixedIncome(FixedIncome fixedIncome);
		bool DeleteFixedIncome(int id);
		#endregion
	}
}
