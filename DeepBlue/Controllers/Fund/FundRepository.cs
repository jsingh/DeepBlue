using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Fund {
	public class FundRepository : IFundRepository {

		DeepBlueEntities context = new DeepBlueEntities();

		#region IFundRepository Members

		public List<FundListModel> GetAllFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			IQueryable<Models.Fund.FundListModel> fundListQuery = (from fund in context.Funds
																   select new FundListModel {
																	   FundId = fund.FundID,
																	   FundName = fund.FundName,
																	   TaxId = fund.TaxID,
																	   FundStartDate = fund.InceptionDate,
																	   ScheduleTerminationDate = fund.ScheduleTerminationDate
																   });
			switch (sortName) {
				case "FundName":
					if (sortOrder == "desc")
						fundListQuery = fundListQuery.OrderByDescending(fundlist => fundlist.FundName);
					else
						fundListQuery = fundListQuery.OrderBy(fundlist => fundlist.FundName);
					break;
				case "TaxId":
					if (sortOrder == "desc")
						fundListQuery = fundListQuery.OrderByDescending(fundlist => fundlist.TaxId);
					else
						fundListQuery = fundListQuery.OrderBy(fundlist => fundlist.TaxId);
					break;
				case "FundStartDate":
					if (sortOrder == "desc")
						fundListQuery = fundListQuery.OrderByDescending(fundlist => fundlist.FundStartDate);
					else
						fundListQuery = fundListQuery.OrderBy(fundlist => fundlist.FundStartDate);
					break;
				case "ScheduleTerminationDate":
					if (sortOrder == "desc")
						fundListQuery = fundListQuery.OrderByDescending(fundlist => fundlist.ScheduleTerminationDate);
					else
						fundListQuery = fundListQuery.OrderBy(fundlist => fundlist.ScheduleTerminationDate);
					break;
			}
			PaginatedList<Models.Fund.FundListModel> paginatedFunds = new PaginatedList<Models.Fund.FundListModel>(fundListQuery, pageIndex, pageSize);
			totalRows = paginatedFunds.TotalCount;
			return paginatedFunds;
		}

		public Models.Entity.Fund FindFund(int fundId) {
			Models.Entity.Fund deepBlueFund;
			deepBlueFund = context.Funds.SingleOrDefault(fund => fund.FundID == fundId);
			return deepBlueFund;
		}

		public IEnumerable<Helpers.ErrorInfo> SaveFund(Models.Entity.Fund fund) {
			return fund.Save();
		}

		#endregion

	}
}