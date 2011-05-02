using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Report;

namespace DeepBlue.Controllers.Report {
	public class ReportRepository : IReportRepository {

		#region IReportRepository Members
		public List<DistributionLineItem> DistributionLineItems(int fundId, int capitalDistributionlId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from distributionDetail in context.CapitalDistributionLineItems
						where distributionDetail.CapitalDistribution.FundID == fundId && distributionDetail.CapitalDistributionID == capitalDistributionlId
						select new DistributionLineItem {
							 InvestorName = distributionDetail.Investor.InvestorName,
							 Commitment = distributionDetail.Investor.InvestorFunds.Where(investorFund => investorFund.FundID == fundId).Sum(investor => investor.TotalCommitment),
							 DistributionAmount = distributionDetail.DistributionAmount,
							 Designation = distributionDetail.Investor.InvestorContacts.FirstOrDefault().Contact.Designation
						}).ToList();
			}
		}
	
		public CapitalDistribution FindCapitalDistribution(int capitalDistributionlId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributions
					.Include("Fund")
					.SingleOrDefault(distribution => distribution.CapitalDistributionID == capitalDistributionlId);
			}
		}

		public List<CapitalCallItem> CapitalCallLineItems(int fundId, int capitalCalllId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCallDetail in context.CapitalCallLineItems
						where capitalCallDetail.CapitalCall.FundID == fundId && capitalCallDetail.CapitalCallID == capitalCalllId  
						select new CapitalCallItem {
							InvestorName = capitalCallDetail.Investor.InvestorName,
							Commitment = capitalCallDetail.Investor.InvestorFunds.Where(investorFund => investorFund.FundID == fundId).Sum(investor => investor.TotalCommitment),
							Expenses = capitalCallDetail.FundExpenses,
							Investments = capitalCallDetail.InvestmentAmount,
							ManagementFees  = capitalCallDetail.ManagementFees
						}).ToList();
			}
		}

		public Models.Entity.CapitalCall FindCapitalCall(int capitalCalllId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCalls
					.Include("Fund")
					.SingleOrDefault(capitalCall => capitalCall.CapitalCallID == capitalCalllId);
			}
		}
		#endregion
	}
}