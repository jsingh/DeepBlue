﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.CapitalCall {

	public interface ICapitalCallRepository {
		List<Models.Entity.CapitalCall> GetCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId);
		List<Models.Entity.CapitalCall> GetCapitalCalls(int fundId);
		List<CapitalDistribution> GetCapitalDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId);
		List<CapitalDistribution> GetCapitalDistributions(int fundId);
		Models.Entity.CapitalCall FindCapitalCall(int capitalCallId);
		CapitalCallLineItem FindCapitalCallLineItem(int capitalCallLineItemId);
		CapitalDistributionLineItem FindCapitalDistributionLineItem(int capitalDistributionLineItemId);
		Models.Entity.Fund FindFund(int fundId);
		FundDetail FindFundDetail(int fundId);
		int FindCapitalCallNumber(int fundId);
		int FindCapitalCallDistributionNumber(int fundId);

		DetailModel FindDetail(int fundId);
		List<CapitalCallInvestorDetail> GetCapitalCallInvestors(int capitalCallId);

		IEnumerable<ErrorInfo> SaveCapitalCall(Models.Entity.CapitalCall capitalCall);
		IEnumerable<ErrorInfo> SaveCapitalCallLineItem(CapitalCallLineItem capitalCallLineItem);
		IEnumerable<ErrorInfo> SaveCapitalDistribution(CapitalDistribution capitalDistribution);
		IEnumerable<ErrorInfo> SaveCapitalDistributionLineItem(CapitalDistributionLineItem capitalDistributionLineItem);
		List<InvestorFund> GetAllInvestorFunds(int fundId);
	}
}
