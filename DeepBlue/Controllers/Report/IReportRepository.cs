using System;
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

	}
}
