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

		public CapitalCallDetail FindCapitalCallDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				List<Models.Entity.CapitalCall> calls = context.CapitalCalls.Where(capitalCallDetail => capitalCallDetail.FundID == fundId).ToList();
				CapitalCallDetail detail = new CapitalCallDetail();
				if (calls.Count > 0) {
					detail.CapitalCommitted = FormatHelper.CurrencyFormat(calls.Sum(capitalCall => capitalCall.CapitalAmountCalled));
					detail.FundName = calls.First().Fund.FundName;
					detail.FundExpenses = FormatHelper.CurrencyFormat(calls.Sum(capitalCall => capitalCall.FundExpenses ?? 0));
					detail.ManagementFees = FormatHelper.CurrencyFormat(calls.Sum(capitalCall => capitalCall.ManagementFees ?? 0));
					detail.UnfundedAmount = FormatHelper.CurrencyFormat(calls.Sum(capitalCall => capitalCall.Fund.InvestorFunds.Sum(fund => fund.UnfundedAmount ?? 0)));
				} else {
					detail.FundName = (from fund in context.Funds
									   where fund.FundID == fundId
									   select fund.FundName).FirstOrDefault();
				}
				return detail;
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

		#endregion
	}
}