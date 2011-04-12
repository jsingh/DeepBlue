using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Report;

namespace DeepBlue.Controllers.Report {
	public interface IReportRepository {

		#region Report
		List<DistributionLineItem> DistributionLineItems(int fundId, int capitalDistributionlId);
		CapitalDistribution FindCapitalDistribution(int capitalDistributionlId);

		List<CapitalCallItem> CapitalCallLineItems(int fundId, int capitalCalllId);
		Models.Entity.CapitalCall FindCapitalCall(int capitalCalllId);
		#endregion

	}
}
