using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.CapitalCall {
	public class CapitalCallRepository : ICapitalCallRepository {

		#region ICapitalCallRepository Members

		public List<Models.Entity.CapitalCall> GetCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CapitalCall> query = (from capitalCall in context.CapitalCalls
															   where capitalCall.FundID == fundId
															   select capitalCall);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.CapitalCall> paginatedList = new PaginatedList<Models.Entity.CapitalCall>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<Models.Entity.CapitalCall> GetCapitalCalls(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCalls
						where capitalCall.FundID == fundId
						orderby capitalCall.CapitalCallID descending
						select capitalCall).ToList();
			}
		}

		public List<InvestorFund> GetAllInvestorFunds(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFunds.Where(investorFunds => investorFunds.FundID == fundId).ToList();
			}
		}

		public Models.Entity.Fund FindFund(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Funds
							  .Include("CapitalCalls")
							  .Include("InvestorFunds")
							  .Include("FundRateSchedules")
							  .Include("CapitalDistributions")
							  .SingleOrDefault(fund => fund.FundID == fundId);
			}
		}

		public Models.Entity.CapitalCall FindCapitalCall(int capitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCalls
							  .Include("CapitalCallLineItems")
							  .Include("Fund")
							  .Include("CapitalCallLineItems.Investor")
							  .SingleOrDefault(capitalCall => capitalCall.CapitalCallID == capitalCallId);
			}
		}

		public CapitalCallLineItem FindCapitalCallLineItem(int capitalCallLineItemId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCallLineItems.Where(capitalCallLineItem => capitalCallLineItem.CapitalCallLineItemID == capitalCallLineItemId).SingleOrDefault();
			}
		}

		public CapitalDistributionLineItem FindCapitalDistributionLineItem(int capitalDistributionLineItemId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributionLineItems.Where(capitalDistributionLineItem => capitalDistributionLineItem.CapitalDistributionLineItemID == capitalDistributionLineItemId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveCapitalCall(Models.Entity.CapitalCall capitalCall) {
			return capitalCall.Save();
		}

		public IEnumerable<ErrorInfo> SaveCapitalCallLineItem(CapitalCallLineItem capitalCallLineItem) {
			return capitalCallLineItem.Save();
		}

		public IEnumerable<ErrorInfo> SaveCapitalDistribution(CapitalDistribution capitalDistribution) {
			return capitalDistribution.Save();
		}

		public IEnumerable<ErrorInfo> SaveCapitalDistributionLineItem(CapitalDistributionLineItem capitalDistributionLineItem) {
			return capitalDistributionLineItem.Save();
		}

		public List<CapitalDistribution> GetCapitalDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<CapitalDistribution> query = (from capitalDistribution in context.CapitalDistributions
														 where capitalDistribution.FundID == fundId
														 select capitalDistribution);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<CapitalDistribution> paginatedList = new PaginatedList<CapitalDistribution>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<CapitalDistribution> GetCapitalDistributions(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistribution in context.CapitalDistributions
						where capitalDistribution.FundID == fundId
						orderby capitalDistribution.CapitalDistributionID descending
						select capitalDistribution).ToList();
			}
		}

		public int FindCapitalCallNumber(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				int? totalCapitalCalls = context.CapitalCalls.Where(capitalCall => capitalCall.FundID == fundId).Count();
				return (totalCapitalCalls ?? 0) + 1;
			}
		}

		public int FindCapitalCallDistributionNumber(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				int? totalCapitalDistribution = context.CapitalDistributions.Where(capitalDistribution => capitalDistribution.FundID == fundId).Count();
				return (totalCapitalDistribution ?? 0) + 1;
			}
		}

		public FundDetail FindFundDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						where fund.FundID == fundId
						select new FundDetail {
							CapitalCallNumber = fund.CapitalCalls.Count() + 1,
							DistributionNumber = fund.CapitalDistributions.Count() + 1,
							FundId = fund.FundID,
							FundName = fund.FundName,
							TotalCommitment = fund.CapitalCalls.Sum(capitalCall => capitalCall.CapitalAmountCalled),
							UnfundedAmount = fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount),
							TotalDistribution = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.DistributionAmount),
							TotalProfit = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.Profits)
						}).SingleOrDefault();
			}
		}

		public DetailModel FindDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						where fund.FundID == fundId
						select new DetailModel {
							FundId = fund.FundID,
							FundName = fund.FundName,
							CapitalCommitted = fund.CapitalCalls.Sum(capitalCall => capitalCall.CapitalAmountCalled),
							UnfundedAmount = fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount),
							FundExpenses = fund.CapitalCalls.Sum(capitalCall => capitalCall.FundExpenses),
							ManagementFees = fund.CapitalCalls.Sum(capitalCall => capitalCall.ManagementFees),
							CapitalDistributed = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.DistributionAmount),
							ReturnFundExpenses = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.ReturnFundExpenses),
							ReturnManagementFees = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.ReturnManagementFees),
							ProfitsReturned = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.PreferredReturn),
							CapitalDistributions = (from capitalDistribution in fund.CapitalDistributions
													orderby capitalDistribution.CapitalDistributionID descending
													select new CapitalDistributionDetail {
														CapitalDistributed = capitalDistribution.DistributionAmount,
														Number = capitalDistribution.DistributionNumber,
														CapitalDistrubutionId = capitalDistribution.CapitalDistributionID,
														CapitalDistributionDate = capitalDistribution.CapitalDistributionDate,
														CapitalDistributionDueDate = capitalDistribution.CapitalDistributionDueDate,
														Profit = (capitalDistribution.Profits ?? 0) + (capitalDistribution.LPProfits ?? 0),
														ProfitReturn = capitalDistribution.PreferredReturn,
														ReturnFundExpenses = capitalDistribution.ReturnFundExpenses,
														ReturnManagementFees = capitalDistribution.ReturnManagementFees
													}),
							CapitalCalls = (from capitalCall in fund.CapitalCalls
											orderby capitalCall.CapitalCallID descending
											select new CapitalCallDetail {
												CapitalCallId = capitalCall.CapitalCallID,
												CapitalCallAmount = capitalCall.CapitalAmountCalled,
												CapitalCallDate = capitalCall.CapitalCallDate,
												CapitalCallDueDate = capitalCall.CapitalCallDueDate,
												FundExpenses = capitalCall.FundExpenses,
												ManagementFees = capitalCall.ManagementFees,
												Number = capitalCall.CapitalCallNumber
											})
						}).SingleOrDefault();
			}
		}

		public List<CapitalCallInvestorDetail> GetCapitalCallInvestors(int capitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCallLineItem in context.CapitalCallLineItems
						where capitalCallLineItem.CapitalCallID == capitalCallId
						select new CapitalCallInvestorDetail {
							CapitalCallAmount = capitalCallLineItem.CapitalAmountCalled,
							CapitalCallDate = capitalCallLineItem.CapitalCall.CapitalCallDate,
							CapitalCallDueDate = capitalCallLineItem.CapitalCall.CapitalCallDueDate,
							FundExpenses = capitalCallLineItem.FundExpenses,
							InvestorName = capitalCallLineItem.Investor.InvestorName,
							ManagementFees = capitalCallLineItem.ManagementFees
						}).ToList();
			}
		}

		#endregion
	}
}