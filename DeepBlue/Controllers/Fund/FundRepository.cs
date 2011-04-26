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
																		   InceptionDate = fund.InceptionDate,
																		   ScheduleTerminationDate = fund.ScheduleTerminationDate
																	   });
				fundListQuery = fundListQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Fund.FundListModel> paginatedList = new PaginatedList<Models.Fund.FundListModel>(fundListQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Helpers.FundLists GetAllFunds(int pageIndex, int pageSize) {
			Helpers.FundLists funds = new Helpers.FundLists();
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Helpers.FundList> fundListQuery = (from fund in context.Funds
															  select new Helpers.FundList {  
																		   FundId = fund.FundID,
																		   FundName = fund.FundName
																	   });
				fundListQuery = fundListQuery.OrderBy("FundName", true);
				PaginatedList<Helpers.FundList> paginatedList = new PaginatedList<Helpers.FundList>(fundListQuery, pageIndex, pageSize);
				funds.TotalPages = paginatedList.TotalPages;
				funds.PageNo = pageIndex;
				funds.FundDetails = paginatedList.ToList();
			}
			return funds;
		}

		public Models.Entity.Fund FindFund(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Funds
							  .Include("FundClosings")
							  .Include("FundAccounts")
							  .Include("FundRateSchedules")
							  .Include("InvestorFunds")
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

		public List<AutoCompleteList> FindFunds(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.Funds
						where fund.FundName.Contains(fundName)
						orderby fund.FundName
						select new AutoCompleteList {
							id = fund.FundID,
							label = fund.FundName,
							value = fund.FundName
						});
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, 20);
			}
		}

		public List<FundRateSchedule> GetAllFundRateSchdules(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from rateSchedule in context.FundRateSchedules
													.Include("ManagementFeeRateSchedule")
													.Include("ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers")
													.Include("ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers.MultiplierType")
						where rateSchedule.FundID == fundId
						select rateSchedule).ToList();
			}
		}

		public List<MultiplierType> GetAllMultiplierTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.MultiplierTypes.OrderBy(type => type.Name).ToList();
			}
		}

		public void DeleteFundRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundRateSchedule rateSchedule = context.FundRateSchedules
													 .SingleOrDefault(schedule => schedule.FundRateScheduleID == id);
				context.FundRateSchedules.DeleteObject(rateSchedule);
				context.SaveChanges();
			}
		}

		public void DeleteManagementFeeRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ManagementFeeRateSchedule rateSchedule = context.ManagementFeeRateSchedules
														.Include("ManagementFeeRateScheduleTiers")
														.SingleOrDefault(schedule => schedule.ManagementFeeRateScheduleID == id);
				foreach (var tier in rateSchedule.ManagementFeeRateScheduleTiers) {
					context.ManagementFeeRateScheduleTiers.DeleteObject(tier);
				}
				context.ManagementFeeRateSchedules.DeleteObject(rateSchedule);
				context.SaveChanges();
			}
		}

		public FundRateSchedule FindRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundRateSchedules.Include("ManagementFeeRateSchedule")
												.SingleOrDefault(schedule => schedule.FundRateScheduleID == id);
			}
		}

		public ManagementFeeRateSchedule FindManagementFeeRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ManagementFeeRateSchedules
												.Include("ManagementFeeRateScheduleTiers")
												.Include("ManagementFeeRateScheduleTiers.MultiplierType")
												.SingleOrDefault(schedule => schedule.ManagementFeeRateScheduleID == id);
			}
		}

		public IEnumerable<ErrorInfo> SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule) {
			return managementFeeRateSchedule.Save();
		}

		public ManagementFeeRateScheduleTier FindManagementFeeRateScheduleTier(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ManagementFeeRateScheduleTiers.SingleOrDefault(tier => tier.ManagementFeeRateScheduleTierID == id);
			}
		}

		public decimal FindTotalCommittedAmount(int fundId, int investorTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFunds.Where(investorFund => investorFund.InvestorTypeId == investorTypeId && investorFund.FundID == fundId).Sum(invfund => invfund.TotalCommitment);
			}
		}

		#endregion
	}
}