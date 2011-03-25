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

		public IEnumerable<ErrorInfo> SaveCapitalCall(Models.Entity.CapitalCall capitalCall) {
			return capitalCall.Save();
		}

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

		public CapitalCallDetail FindCapitalCallDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				List<Models.Entity.CapitalCall> calls = context.CapitalCalls.Where(capitalCallDetail => capitalCallDetail.FundID == fundId).ToList();
				CapitalCallDetail detail = new CapitalCallDetail();
				if (calls.Count > 0) {
					detail.CapitalCommitted = string.Format("{0:C}", calls.Sum(capitalCall => capitalCall.CapitalAmountCalled));
					detail.FundName = calls.First().Fund.FundName;
					detail.FundExpenses = string.Format("{0:C}", calls.Sum(capitalCall => capitalCall.FundExpenses ?? 0));
					detail.ManagementFees = string.Format("{0:C}", calls.Sum(capitalCall => capitalCall.ManagementFees ?? 0));
					detail.UnfundedAmount = string.Format("{0:C}", calls.Sum(capitalCall => capitalCall.Fund.InvestorFunds.Sum(fund => fund.UnfundedAmount ?? 0)));
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
							  .SingleOrDefault(fund => fund.FundID == fundId);
			}
		}

		#endregion
	}
}