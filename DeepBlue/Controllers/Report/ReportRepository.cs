using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Report;

namespace DeepBlue.Controllers.Report {
	public class ReportRepository : IReportRepository {

		#region CashDistribution
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

		public CashDistributionReportDetail FindCapitalDistribution(int capitalDistributionlId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from cashDistribution in context.CapitalDistributions
						where cashDistribution.CapitalDistributionID == capitalDistributionlId
						select new CashDistributionReportDetail {
							DistributionDate = cashDistribution.CapitalDistributionDate,
							FundName = cashDistribution.Fund.FundName,
							TotalDistributionAmount = cashDistribution.DistributionAmount,
							Items = (from distributionDetail in cashDistribution.CapitalDistributionLineItems
									 select new DistributionLineItem {
										 InvestorName = distributionDetail.Investor.InvestorName,
										 Commitment = distributionDetail.Investor.InvestorFunds.Where(investorFund => investorFund.FundID == cashDistribution.FundID).Sum(investor => investor.TotalCommitment),
										 DistributionAmount = distributionDetail.DistributionAmount,
										 Designation = distributionDetail.Investor.InvestorContacts.FirstOrDefault().Contact.Designation
									 })
						}).SingleOrDefault();
			}
		}

		#endregion

		#region CapitalCall

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

		public CapitalCallReportDetail FindCapitalCall(int capitalCalllId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCalls
						where capitalCall.CapitalCallID == capitalCalllId
						select new CapitalCallReportDetail {
							AmountForInv = capitalCall.InvestmentAmount,
							CapitalCallDueDate = capitalCall.CapitalCallDueDate,
							ExistingInv = capitalCall.ExistingInvestmentAmount,
							FundName = capitalCall.Fund.FundName,
							Items = (from capitalCallDetail in capitalCall.CapitalCallLineItems
									 select new CapitalCallItem {
										 InvestorName = capitalCallDetail.Investor.InvestorName,
										 Commitment = capitalCallDetail.Investor.InvestorFunds.Where(investorFund => investorFund.FundID == capitalCall.FundID).Sum(investor => investor.TotalCommitment),
										 Expenses = capitalCallDetail.FundExpenses,
										 Investments = capitalCallDetail.InvestmentAmount,
										 ManagementFees = capitalCallDetail.ManagementFees
									 }),
							NewInv = capitalCall.NewInvestmentAmount,
							TotalCapitalCall = capitalCall.CapitalAmountCalled,
							TotalExpenses = capitalCall.FundExpenses,
							TotalManagementFees = capitalCall.ManagementFees,
						}).SingleOrDefault();
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
							GrossPurchasePrice = deal.DealUnderlyingFunds.Sum(dealUnderlyingFund => dealUnderlyingFund.GrossPurchasePrice),
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

		#region FundBreakDown
		public FundBreakDownReportDetail FindFundBreakDownReport(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						where fund.FundID == fundId
						select new FundBreakDownReportDetail {
							 FundName = fund.FundName,
							 TotalUnderlyingFunds = fund.Deals.Sum(deal => deal.DealUnderlyingFunds.Count),
							  TotalDirects = fund.Deals.Sum(deal => deal.DealUnderlyingDirects.Count),
						}).SingleOrDefault();
			}
		}
		#endregion
	}
}