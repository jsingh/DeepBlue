using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Fund {
	public class FundRepository : IFundRepository {

		#region IFundRepository Members

		public List<FundListModel> GetAllFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Fund.FundListModel> fundListQuery = (from fund in context.Funds
																	   select new FundListModel {
																		   FundId = fund.FundID,
																		   FundName = fund.FundName,
																		   TaxId = fund.TaxID,
																		   FundStartDate = fund.InceptionDate,
																		   ScheduleTerminationDate = fund.ScheduleTerminationDate
																	   });
				fundListQuery = fundListQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Fund.FundListModel> paginatedList = new PaginatedList<Models.Fund.FundListModel>(fundListQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Models.Entity.Fund FindFund(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Funds
							  .Include("FundClosings")
							  .Include("FundAccounts")
							  .SingleOrDefault(fund => fund.FundID == fundId);
			}
		}

		public IEnumerable<Helpers.ErrorInfo> SaveFund(Models.Entity.Fund fund) {
			return fund.Save();
		}

		public bool TaxIdAvailable(string taxId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from fund in context.Funds
						 where fund.TaxID == taxId && fund.FundID != fundId
						 select fund.FundID).Count()) > 0 ? true : false;
			}
		}

		public bool FundNameAvailable(string fundName, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from fund in context.Funds
						 where fund.FundName == fundName && fund.FundID != fundId
						 select fund.FundID).Count()) > 0 ? true : false;
			}
		}

		public List<Models.Entity.Fund> FindFunds(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.Funds
						where fund.FundName.Contains(fundName)
						select fund).ToList();
			}
		}

		#endregion
	}
}