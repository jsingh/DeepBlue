﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Report;

namespace DeepBlue.Controllers.Report {
	public interface IReportRepository {

		#region CashDistribution
		List<DistributionLineItem> DistributionLineItems(int fundId, int capitalDistributionlId);
		CashDistributionReportDetail FindCapitalDistribution(int capitalDistributionlId);
		#endregion

		#region CapitalCall
		List<CapitalCallItem> CapitalCallLineItems(int fundId, int capitalCalllId);
		CapitalCallReportDetail FindCapitalCall(int capitalCalllId);
		#endregion

		#region DealDetail
		DealDetailReportModel FindDealDetailReport(int dealId);
		#endregion

		#region DealOrigination
		DealOriginationReportModel FindDealOriginationReport(int dealId);
		#endregion

		#region FundBreakDown
		FundBreakDownReportDetail FindFundBreakDownReport(int fundId);
		#endregion

		#region FeesExpense
		List<FeesExpenseReportDetail> FindFeesExpenseReport(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows,
															int fundId, DateTime startDate, DateTime endDate);
		#endregion

		#region Distribution
		List<DistributionReportDetail> FindDistributionReport(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows,
															   int fundId, DateTime startDate, DateTime endDate);
		#endregion

		#region SecurityValue
		List<SecurityValueReportDetail> FindSecurityValueReport(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows,
															   int fundId, DateTime startDate, DateTime endDate);
		#endregion

		#region UnderlyingFundNAV
		List<UnderlyingFundNAVReportDetail> FindUnderlyingFundNAVReport(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows,
														   int underlyingFundId, DateTime startDate, DateTime endDate);
		#endregion

		#region UnfundedCapitalCallBalance
		List<UnfundedCapitalCallBalanceReportDetail> FindUnfundedCapitalCallBalanceReport(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows,
														  int fundId, DateTime startDate, DateTime endDate);
		#endregion
	}
}
