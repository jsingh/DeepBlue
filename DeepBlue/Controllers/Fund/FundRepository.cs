using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Fund;
using DeepBlue.Helpers;
using System.Data.Objects;

namespace DeepBlue.Controllers.Fund {
	public class FundRepository : IFundRepository {

		#region Fund

		public List<FundListModel> GetAllFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Fund> funds = context.FundsTable;
				if (fundId > 0) {
					funds = funds.Where(fund => fund.FundID == fundId);
				}
				IQueryable<Models.Fund.FundListModel> fundListQuery = (from fund in funds
																	   select new FundListModel {
																		   FundId = fund.FundID,
																		   FundName = fund.FundName,
																		   TaxId = fund.TaxID,
																		   InceptionDate = fund.InceptionDate,
																		   ScheduleTerminationDate = fund.ScheduleTerminationDate,
																		   CommitmentAmount = fund.InvestorFunds.Sum(investorfund => investorfund.TotalCommitment),
																		   UnfundedAmount = fund.InvestorFunds.Sum(investorfund => investorfund.UnfundedAmount)
																	   });
				fundListQuery = fundListQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Fund.FundListModel> paginatedList = new PaginatedList<Models.Fund.FundListModel>(fundListQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<InvestorListModel> GetAllInvestorFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<InvestorListModel> investorFundListQuery = (from investorFund in context.InvestorFundsTable
																	   where investorFund.FundID == fundId
																	   select new InvestorListModel {
																		   InvestorName = investorFund.Investor.InvestorName,
																		   CommittedAmount = investorFund.TotalCommitment,
																		   UnfundedAmount = investorFund.UnfundedAmount,
																		   CloseDate = investorFund
																					   .InvestorFundTransactions
																					   .Where(transaction => transaction.FundClosingID > 0)
																					   .FirstOrDefault().FundClosing.FundClosingDate,
																	   });
				investorFundListQuery = investorFundListQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<InvestorListModel> paginatedList = new PaginatedList<InvestorListModel>(investorFundListQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Helpers.FundLists GetAllFunds(int pageIndex, int pageSize) {
			Helpers.FundLists funds = new Helpers.FundLists();
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Helpers.FundList> fundListQuery = (from fund in context.FundsTable
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
							  .EntityFilter()
							  .SingleOrDefault(fund => fund.FundID == fundId);
			}
		}

		public Models.Entity.Fund FindFund(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundsTable
							  .Where(fund => fund.FundName == fundName)
							  .FirstOrDefault();
			}
		}


		public FundDetail FindLastFundDetail() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.FundsTable
						orderby fund.FundID descending
						select new FundDetail {
							FundId = fund.FundID,
							FundName = fund.FundName
						}).FirstOrDefault();
			}
		}

		public List<AutoCompleteList> FindDealFunds(int underlyingFundId, string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.FundsTable
															  join deal in context.DealsTable on fund.FundID equals deal.FundID
															  join dealUnderlyingFund in context.DealUnderlyingFundsTable on deal.DealID equals dealUnderlyingFund.DealID
															  where fund.FundName.StartsWith(fundName) && dealUnderlyingFund.UnderlyingFundID == underlyingFundId
															  group fund by fund.FundID into funds
															  orderby funds.FirstOrDefault().FundName
															  select new AutoCompleteList {
																  id = funds.FirstOrDefault().FundID,
																  label = funds.FirstOrDefault().FundName,
																  value = funds.FirstOrDefault().FundName
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindFunds(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.FundsTable
															  where fund.FundName.StartsWith(fundName)
															  orderby fund.FundName
															  select new AutoCompleteList {
																  id = fund.FundID,
																  label = fund.FundName,
																  value = fund.FundName
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindFunds(string fundName, ref int totalCount) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.FundsTable
															  where fund.FundName.StartsWith(fundName)
															  orderby fund.FundName
															  select new AutoCompleteList {
																  id = fund.FundID,
																  label = fund.FundName,
																  value = fund.FundName
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, AutoCompleteOptions.RowsLength, ref totalCount);
			}
		}

		public List<AutoCompleteList> FindFundClosings(string fundName, int? fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fundClosing in context.FundClosingsTable
															  where fundClosing.Name.StartsWith(fundName)
															  && fundClosing.FundID == (fundId ?? 0)
															  orderby fundClosing.Name
															  select new AutoCompleteList {
																  id = fundClosing.FundClosingID,
																  label = fundClosing.Name,
																  value = fundClosing.Name
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public IEnumerable<Helpers.ErrorInfo> SaveFund(Models.Entity.Fund fund) {
			return fund.Save();
		}

		public bool TaxIdAvailable(string taxId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from fund in context.FundsTable
						 where fund.TaxID == taxId && fund.FundID != fundId
						 select fund.FundID).Count()) > 0 ? true : false;
			}
		}

		public bool FundNameAvailable(string fundName, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from fund in context.FundsTable
						 where fund.FundName == fundName && fund.FundID != fundId
						 select fund.FundID).Count()) > 0 ? true : false;
			}
		}

		public decimal FindTotalCommittedAmount(int fundId, int investorTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFundsTable.Where(investorFund => investorFund.InvestorTypeID == investorTypeId && investorFund.FundID == fundId).Sum(invfund => invfund.TotalCommitment);
			}
		}

		public string FindFundName(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundsTable.Where(fund => fund.FundID == fundId).Select(fund => fund.FundName).SingleOrDefault();
			}
		}

		public CreateModel FindFundDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.FundsTable
						where fund.FundID == fundId
						select new CreateModel {
							FundName = fund.FundName,
							TaxId = fund.TaxID,
							InceptionDate = fund.InceptionDate,
							ScheduleTerminationDate = fund.ScheduleTerminationDate,
							FinalTerminationDate = fund.FinalTerminationDate,
							NumofAutoExtensions = fund.NumofAutoExtensions,
							DateClawbackTriggered = fund.DateClawbackTriggered,
							RecycleProvision = fund.RecycleProvision,
							MgmtFeesCatchUpDate = fund.MgmtFeesCatchUpDate,
							Carry = fund.Carry,
							FundId = fund.FundID,
							FundRateSchedules = (from rateSchedule in fund.FundRateSchedules
												 select new FundRateScheduleDetail {
													 FundId = fund.FundID,
													 FundRateScheduleId = rateSchedule.FundRateScheduleID,
													 InvestorTypeId = rateSchedule.InvestorTypeID,
													 RateScheduleId = rateSchedule.RateScheduleID,
													 RateScheduleTypeId = rateSchedule.RateScheduleTypeID,
												 }),
							BankDetail = (from fundAccount in fund.FundAccounts
										  select new FundBankDetail {
											  ABANumber = fundAccount.Routing,
											  Account = fundAccount.Account,
											  AccountNumber = fundAccount.AccountNumberCash,
											  AccountOf = fundAccount.AccountOf,
											  Attention = fundAccount.Attention,
											  BankName = fundAccount.BankName,
											  FFCNumber = fundAccount.FFCNumber,
											  IBAN = fundAccount.IBAN,
											  Swift = fundAccount.SWIFT,
											  AccountFax = fundAccount.Fax,
											  Reference = fundAccount.Reference,
											  AccountId = fundAccount.FundAccountID,
											  AccountPhone = fundAccount.Phone,
											  FFC = fundAccount.FFC,
										  })
						}).SingleOrDefault();
			}
		}

		#endregion

		#region Fund Rate Schedules

		public List<FundRateSchedule> GetAllFundRateSchdules(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from rateSchedule in context.FundRateSchedules
													.Include("ManagementFeeRateSchedule")
													.Include("ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers")
													.Include("ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers.MultiplierType")
													.EntityFilter()
						where rateSchedule.FundID == fundId
						select rateSchedule).ToList();
			}
		}

		public List<MultiplierType> GetAllMultiplierTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.MultiplierTypesTable.OrderBy(type => type.Name).ToList();
			}
		}

		public void DeleteFundRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundRateSchedule rateSchedule = context.FundRateSchedulesTable
													 .SingleOrDefault(schedule => schedule.FundRateScheduleID == id);
				context.FundRateSchedules.DeleteObject(rateSchedule);
				context.SaveChanges();
			}
		}

		public void DeleteManagementFeeRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ManagementFeeRateSchedule rateSchedule = context.ManagementFeeRateSchedules
														.Include("ManagementFeeRateScheduleTiers")
														.EntityFilter()
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
				return context.FundRateSchedules.Include("ManagementFeeRateSchedule").EntityFilter()
												.SingleOrDefault(schedule => schedule.FundRateScheduleID == id);
			}
		}

		public ManagementFeeRateSchedule FindManagementFeeRateSchedule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ManagementFeeRateSchedules
												.Include("ManagementFeeRateScheduleTiers")
												.Include("ManagementFeeRateScheduleTiers.MultiplierType")
												.EntityFilter()
												.SingleOrDefault(schedule => schedule.ManagementFeeRateScheduleID == id);
			}
		}

		public IEnumerable<ErrorInfo> SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule) {
			return managementFeeRateSchedule.Save();
		}

		public ManagementFeeRateScheduleTier FindManagementFeeRateScheduleTier(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ManagementFeeRateScheduleTiersTable.SingleOrDefault(tier => tier.ManagementFeeRateScheduleTierID == id);
			}
		}

		#endregion
	}
}