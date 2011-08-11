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
							ManagementFees = capitalCallDetail.ManagementFees
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

		#region DealDetail
		public DealDetailReportModel FindDealDetailReport(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealDetailReportModel dealReportDetail = (from deal in context.Deals
														  where deal.DealID == dealId
														  select new DealDetailReportModel {
															  DealName = deal.DealName,
															  FundName = deal.Fund.FundName,
															  FundId = deal.Fund.FundID,
															  DealNumber = deal.DealNumber,
															  FirstFundCloseDate = deal.DealClosings.FirstOrDefault().CloseDate,
															  NetPurchasePrice = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.NetPurchasePrice),
															  CurrentUnfunded = deal.DealUnderlyingFunds.Where(dealUnderlyingFund => dealUnderlyingFund.DealClosingID != null).Sum(dealUnderlyingFund => dealUnderlyingFund.UnfundedAmount),
															  OriginalCommitment = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.CommittedAmount),
															  ClosingCosts = deal.DealClosingCosts.Sum(dealClosingCost => dealClosingCost.Amount),
															  UnfundedClosing = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.UnfundedAmount),
															  ReceivedDistributions = deal.CashDistributions.Sum(cashDistribution => cashDistribution.Amount),
															  NoOfShares = deal.DealUnderlyingDirects.Sum(dealUnderlyingDirect => dealUnderlyingDirect.NumberOfShares),
															  PurchasePrice = deal.DealUnderlyingDirects.Sum(dealUnderlyingDirect => dealUnderlyingDirect.PurchasePrice),
															  EstimatedNAV = deal.Fund.UnderlyingFundNAVs.Sum(underlyingFundNAV => underlyingFundNAV.FundNAV) +
																			 deal.DealUnderlyingDirects.Sum(dealUnderlyingDirect => dealUnderlyingDirect.FMV)
														  }).SingleOrDefault();
				if (dealReportDetail != null) {
					dealReportDetail.Details = (from capitalCall in context.UnderlyingFundCapitalCalls
												where capitalCall.FundID == dealReportDetail.FundId
												select new DealUnderlyingFundDetailModel {
													Amount = capitalCall.Amount,
													Date = capitalCall.ReceivedDate,
													FundName = capitalCall.UnderlyingFund.FundName,
													Type = "Capital Call"
												})
												.Union(
													from cashDistribution in context.UnderlyingFundCashDistributions
													where cashDistribution.FundID == dealReportDetail.FundId
													select new DealUnderlyingFundDetailModel {
														Amount = cashDistribution.Amount,
														Date = cashDistribution.ReceivedDate,
														FundName = cashDistribution.UnderlyingFund.FundName,
														Type = "Cash Distribution"
													}
												)
												.Union(
													from stockDistribuion in context.UnderlyingFundStockDistributions
													where stockDistribuion.FundID == dealReportDetail.FundId
													select new DealUnderlyingFundDetailModel {
														Amount = stockDistribuion.NumberOfShares * stockDistribuion.PurchasePrice,
														Date = stockDistribuion.DistributionDate,
														FundName = stockDistribuion.UnderlyingFund.FundName,
														Type = "Stock Distribution"
													}
												)
												.OrderBy(dealFundDetail => dealFundDetail.Date)
												.ToList();
				}
				return dealReportDetail;
			}
		}
		#endregion

		#region DealOrigination
		public DealOriginationReportModel FindDealOriginationReport(int dealId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from deal in context.Deals
						where deal.DealID == dealId
						select new DealOriginationReportModel {
							DealName = deal.DealName,
							FundName = deal.Fund.FundName,
							FundId = deal.Fund.FundID,
							DealNumber = deal.DealNumber,
							NetPurchasePrice = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.NetPurchasePrice),
							GrossPurchasePrice  = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.GrossPurchasePrice),
							Details = (from dealUnderlyingFund in deal.DealUnderlyingFunds
										select new DealOrganizationFundDetailModel {
											CommitmentAmount = dealUnderlyingFund.CommittedAmount,
											FundName = dealUnderlyingFund.UnderlyingFund.FundName,
											GrossPurchasePrice = dealUnderlyingFund.GrossPurchasePrice,
											NAV = dealUnderlyingFund.Deal.Fund.UnderlyingFundNAVs.Sum(underlyingFUndNAV => underlyingFUndNAV.FundNAV),
											NetPurchasePrice = dealUnderlyingFund.NetPurchasePrice,
											PostRecordAdjustMent = dealUnderlyingFund.PostRecordDateCapitalCall - dealUnderlyingFund.PostRecordDateDistribution,
											RecordDate = dealUnderlyingFund.RecordDate,
											UnfundedAmount = dealUnderlyingFund.UnfundedAmount
										})
						}).SingleOrDefault();
			}
		}
		#endregion
	}
}