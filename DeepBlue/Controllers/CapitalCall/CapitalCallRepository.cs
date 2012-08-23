using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Helpers;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using DeepBlue.Controllers.Accounting;

namespace DeepBlue.Controllers.CapitalCall {
	public class CapitalCallRepository : ICapitalCallRepository {

		#region CapitalCallRepository Members

		public CreateCapitalCallModel FindCapitalCallModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCallsTable
						where capitalCall.CapitalCallID == id
						select new CreateCapitalCallModel {
							CapitalAmountCalled = capitalCall.CapitalAmountCalled,
							CapitalCallDate = capitalCall.CapitalCallDate,
							CapitalCallDueDate = capitalCall.CapitalCallDueDate,
							CapitalCallNumber = capitalCall.CapitalCallNumber,
							FromDate = capitalCall.ManagementFeeStartDate,
							ToDate = capitalCall.ManagementFeeEndDate,
							ExistingInvestmentAmount = capitalCall.ExistingInvestmentAmount,
							FundExpenseAmount = capitalCall.FundExpenses,
							FundId = capitalCall.FundID,
							InvestedAmount = capitalCall.InvestmentAmount,
							InvestedAmountInterest = capitalCall.InvestedAmountInterest,
							ManagementFees = capitalCall.ManagementFees,
							ManagementFeeInterest = capitalCall.ManagementFeeInterest,
							NewInvestmentAmount = capitalCall.NewInvestmentAmount,
							AddFundExpenses = (capitalCall.ManagementFees > 0 ? true : false),
							AddManagementFees = (capitalCall.ManagementFees > 0 ? true : false),
							CapitalCallID = capitalCall.CapitalCallID,
							CapitalCallLineItemsCount = capitalCall.CapitalCallLineItems.Count(),
							FundName = capitalCall.Fund.FundName,
							UnfundedAmount = capitalCall.Fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount),
							TotalCommitment = capitalCall.Fund.InvestorFunds.Sum(investorFund => investorFund.TotalCommitment),
							CapitalCallLineItems = (from item in capitalCall.CapitalCallLineItems
													select new CapitalCallLineItemModel {
														CapitalAmountCalled = item.CapitalAmountCalled,
														CapitalCallID = item.CapitalCallID,
														CapitalCallLineItemID = item.CapitalCallLineItemID,
														ExistingInvestmentAmount = item.ExistingInvestmentAmount,
														InvestedAmountInterest = item.InvestedAmountInterest,
														FundExpenses = item.FundExpenses,
														InvestmentAmount = item.InvestmentAmount,
														ManagementFees = item.ManagementFees,
														InvestorName = (item.Investor != null ? item.Investor.InvestorName : string.Empty),
														ManagementFeeInterest = item.ManagementFeeInterest,
														NewInvestmentAmount = item.NewInvestmentAmount
													})
						}).SingleOrDefault();
			}
		}

		public CreateDistributionModel FindCapitalDistributionModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistribution in context.CapitalDistributionsTable
						where capitalDistribution.CapitalDistributionID == id
						select new CreateDistributionModel {
							CapitalDistributionDate = capitalDistribution.CapitalDistributionDate,
							CapitalDistributionDueDate = capitalDistribution.CapitalDistributionDueDate,
							CapitalDistributionID = capitalDistribution.CapitalDistributionID,
							CapitalReturn = capitalDistribution.CapitalReturn,
							DistributionAmount = capitalDistribution.DistributionAmount,
							DistributionNumber = capitalDistribution.DistributionNumber,
							FundId = capitalDistribution.FundID,
							GPProfits = capitalDistribution.Profits,
							LPProfits = capitalDistribution.LPProfits,
							PreferredCatchUp = capitalDistribution.PreferredCatchUp,
							PreferredReturn = capitalDistribution.PreferredReturn,
							ReturnFundExpenses = capitalDistribution.ReturnFundExpenses,
							ReturnManagementFees = capitalDistribution.ReturnManagementFees,
							TotalDistribution = capitalDistribution.Fund.CapitalDistributions.Sum(distribution => capitalDistribution.DistributionAmount),
							TotalProfit = capitalDistribution.Fund.CapitalDistributions.Sum(distribution => capitalDistribution.Profits),
							FundName = capitalDistribution.Fund.FundName,
							CapitalDistributionLineItemsCount = capitalDistribution.CapitalDistributionLineItems.Count(),
							CapitalDistributionLineItems = (from item in capitalDistribution.CapitalDistributionLineItems
															select new CapitalDistributionLineItemModel {
																CapitalDistributionID = item.CapitalDistributionID,
																CapitalDistributionLineItemID = item.CapitalDistributionLineItemID,
																CapitalReturn = item.CapitalReturn,
																DistributionAmount = item.DistributionAmount,
																InvestorName = item.Investor.InvestorName,
																LPProfits = item.LPProfits,
																PreferredCatchUp = item.PreferredCatchUp,
																PreferredReturn = item.PreferredReturn,
																Profits = item.Profits,
																ReturnFundExpenses = item.ReturnFundExpenses,
																ReturnManagementFees = item.ReturnManagementFees
															})
						}).SingleOrDefault();
			}
		}

		public List<Models.Entity.CapitalCall> GetCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CapitalCall> query = (from capitalCall in context.CapitalCallsTable
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
				return (from capitalCall in context.CapitalCallsTable
						where capitalCall.FundID == fundId
						orderby capitalCall.CapitalCallID descending
						select capitalCall).ToList();
			}
		}

		public List<InvestorFund> GetAllInvestorFunds(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFundsTable.Where(investorFunds => investorFunds.FundID == fundId).ToList();
			}
		}

		public Models.Entity.Fund FindFund(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Funds
							  .Include("CapitalCalls")
							  .Include("InvestorFunds")
							  .Include("FundRateSchedules")
							  .Include("CapitalDistributions")
							  .EntityFilter()
							  .SingleOrDefault(fund => fund.FundID == fundId);
			}
		}

		public Models.Entity.CapitalCall FindCapitalCall(int capitalCallId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCalls
							  .Include("CapitalCallLineItems")
							  .Include("Fund")
							  .Include("CapitalCallLineItems.Investor")
							  .EntityFilter()
							  .SingleOrDefault(capitalCall => capitalCall.CapitalCallID == capitalCallId);
			}
		}

		public Models.Entity.CapitalCall FindCapitalCall(int fundID, DateTime capitalCallDate, DateTime capitalCallDueDate, int capitalCallTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCalls
										  .Include("CapitalCallLineItems")
							  .Include("Fund")
							  .Include("CapitalCallLineItems.Investor")
							  .EntityFilter()
						where capitalCall.FundID == fundID
						&& capitalCall.CapitalCallDate == EntityFunctions.TruncateTime(capitalCallDate)
						&& capitalCall.CapitalCallDueDate == EntityFunctions.TruncateTime(capitalCallDueDate)
						&& capitalCall.CapitalCallTypeID == capitalCallTypeID
						select capitalCall).FirstOrDefault();
			}
		}

		public CapitalDistribution FindCapitalDistribution(int capitalDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributions
							  .Include("CapitalDistributionLineItems")
							  .Include("Fund")
							  .Include("CapitalDistributionLineItems.Investor")
							  .EntityFilter()
							  .SingleOrDefault(capitalDistribution => capitalDistribution.CapitalDistributionID == capitalDistributionId);
			}
		}

		public CapitalDistribution FindCapitalDistribution(int fundID, DateTime distributionDate, DateTime distributionDueDate, bool isManual) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistribution in context.CapitalDistributions
							  .Include("CapitalDistributionLineItems")
							  .Include("Fund")
							  .Include("CapitalDistributionLineItems.Investor")
							  .EntityFilter()
						where capitalDistribution.FundID == fundID
						&& capitalDistribution.CapitalDistributionDate == EntityFunctions.TruncateTime(distributionDate)
						&& capitalDistribution.CapitalDistributionDueDate == EntityFunctions.TruncateTime(distributionDueDate)
						&& capitalDistribution.IsManual == isManual
						select capitalDistribution).FirstOrDefault();
			}
		}

		public CapitalCallLineItem FindCapitalCallLineItem(int capitalCallLineItemId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCallLineItemsTable.Where(capitalCallLineItem => capitalCallLineItem.CapitalCallLineItemID == capitalCallLineItemId).SingleOrDefault();
			}
		}

		public CapitalCallLineItem FindCapitalCallLineItem(int capitalCallID, int investorID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalCallLineItemsTable
					.Where(capitalCallLineItem => capitalCallLineItem.CapitalCallID == capitalCallID && capitalCallLineItem.InvestorID == investorID)
					.SingleOrDefault();
			}
		}

		public CapitalDistributionLineItem FindCapitalDistributionLineItem(int capitalDistributionLineItemId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributionLineItemsTable.Where(capitalDistributionLineItem => capitalDistributionLineItem.CapitalDistributionLineItemID == capitalDistributionLineItemId).SingleOrDefault();
			}
		}

		public CapitalDistributionLineItem FindCapitalDistributionLineItem(int capitalDistributionID, int investorID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributionLineItemsTable
					.Where(capitalDistributionLineItem => capitalDistributionLineItem.CapitalDistributionID == capitalDistributionID && capitalDistributionLineItem.InvestorID == investorID)
					.FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveCapitalCall(Models.Entity.CapitalCall capitalCall) {
			IEnumerable<ErrorInfo> errorInfo = capitalCall.Save();
			if (errorInfo == null) {
				using (DeepBlueEntities context = new DeepBlueEntities()) {
					int capitalCallID = capitalCall.CapitalCallID;
					var findCapitalCall = context.CapitalCallsTable.Where(cc => cc.CapitalCallID == capitalCallID).FirstOrDefault();
					if (findCapitalCall != null) {
						foreach (var capitalCallLineItem in findCapitalCall.CapitalCallLineItems) {
							IAccounting accountingManager = new AccountingManager();
							if (capitalCallLineItem.IsReconciled)
								accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallReconcilationLineItem, findCapitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
							else
								accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallLineItem, findCapitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
						}
					}
				}
			}
			return errorInfo;
		}

		public IEnumerable<ErrorInfo> SaveCapitalCallOnly(Models.Entity.CapitalCall capitalCall) {
			IEnumerable<ErrorInfo> errorInfo = capitalCall.SaveCapitalCallOnly();
			if (errorInfo == null) {
				using (DeepBlueEntities context = new DeepBlueEntities()) {
					int capitalCallID = capitalCall.CapitalCallID;
					var findCapitalCall = context.CapitalCallsTable.Where(cc => cc.CapitalCallID == capitalCallID).FirstOrDefault();
					if (findCapitalCall != null) {
						foreach (var capitalCallLineItem in findCapitalCall.CapitalCallLineItems) {
							IAccounting accountingManager = new AccountingManager();
							if (capitalCallLineItem.IsReconciled)
								accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallReconcilationLineItem, findCapitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
							else
								accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallLineItem, findCapitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
						}
					}
				}
			}
			return errorInfo;
		}

		public IEnumerable<ErrorInfo> SaveCapitalCallLineItem(CapitalCallLineItem capitalCallLineItem) {
			IEnumerable<ErrorInfo> errorInfo = capitalCallLineItem.Save();
			if (errorInfo == null) {
				using (DeepBlueEntities context = new DeepBlueEntities()) {
					Models.Entity.CapitalCall capitalCall = context.CapitalCallsTable.Where(cc => cc.CapitalCallID == capitalCallLineItem.CapitalCallID).FirstOrDefault();
					if (capitalCall != null) {
						IAccounting accountingManager = new AccountingManager();
						if (capitalCallLineItem.IsReconciled)
							accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallReconcilationLineItem, capitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
						else
							accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalCallLineItem, capitalCall.FundID, capitalCallLineItem.CapitalAmountCalled, capitalCallLineItem);
					}
				}
			}
			return errorInfo;
		}

		public IEnumerable<ErrorInfo> SaveCapitalDistribution(CapitalDistribution capitalDistribution) {
			return capitalDistribution.Save();
		}

		public IEnumerable<ErrorInfo> SaveCapitalDistributionLineItem(CapitalDistributionLineItem capitalDistributionLineItem) {
			IEnumerable<ErrorInfo> errorInfo = capitalDistributionLineItem.Save();
			if (errorInfo == null) {
				using (DeepBlueEntities context = new DeepBlueEntities()) {
					CapitalDistribution capitalDistribution = context.CapitalDistributionsTable.Where(cd => cd.CapitalDistributionID == capitalDistributionLineItem.CapitalDistributionID).FirstOrDefault();
					if (capitalDistribution != null) {
						IAccounting accountingManager = new AccountingManager();
						accountingManager.CreateEntry(Models.Accounting.Enums.AccountingTransactionType.CapitalDistributionLineItem, capitalDistribution.FundID, capitalDistributionLineItem.DistributionAmount, capitalDistributionLineItem);
					}
				}
			}
			return errorInfo;
		}

		public List<CapitalDistribution> GetCapitalDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<CapitalDistribution> query = (from capitalDistribution in context.CapitalDistributionsTable
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
				return (from capitalDistribution in context.CapitalDistributionsTable
						where capitalDistribution.FundID == fundId
						orderby capitalDistribution.CapitalDistributionID descending
						select capitalDistribution).ToList();
			}
		}

		public int FindCapitalCallNumber(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				int? totalCapitalCalls = context.CapitalCallsTable.Where(capitalCall => capitalCall.FundID == fundId).Count();
				return (totalCapitalCalls ?? 0) + 1;
			}
		}

		public int FindCapitalCallDistributionNumber(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				int? totalCapitalDistribution = context.CapitalDistributionsTable.Where(capitalDistribution => capitalDistribution.FundID == fundId).Count();
				return (totalCapitalDistribution ?? 0) + 1;
			}
		}

		public FundDetail FindFundDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.FundsTable
						where fund.FundID == fundId
						select new FundDetail {
							CapitalCallNumber = fund.CapitalCalls.Count() + 1,
							DistributionNumber = fund.CapitalDistributions.Count() + 1,
							FundId = fund.FundID,
							FundName = fund.FundName,
							TotalCommitment = fund.InvestorFunds.Sum(investorFund => investorFund.TotalCommitment),
							UnfundedAmount = fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount),
							TotalDistribution = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.DistributionAmount),
							TotalProfit = fund.CapitalDistributions.Sum(capitalDistribution => capitalDistribution.Profits)
						}).SingleOrDefault();
			}
		}

		public DetailModel FindDetail(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fund in context.FundsTable
						where fund.FundID == fundId
						select new DetailModel {
							FundId = fund.FundID,
							FundName = fund.FundName,
							CapitalCommitted = fund.InvestorFunds.Sum(investorFund => investorFund.TotalCommitment),
							UnfundedAmount = fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount),
							CapitalCalled = fund.CapitalCalls.Sum(capitalCall => capitalCall.CapitalAmountCalled),
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
														ReturnManagementFees = capitalDistribution.ReturnManagementFees,
														LPProfit = capitalDistribution.LPProfits,
														CostReturn = capitalDistribution.CapitalReturn
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
				return (from capitalCallLineItem in context.CapitalCallLineItemsTable
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

		public List<CapitalDistributionInvestorDetail> GetCapitalDistributionInvestors(int capitalDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistributionLineItem in context.CapitalDistributionLineItemsTable
						where capitalDistributionLineItem.CapitalDistributionID == capitalDistributionId
						select new CapitalDistributionInvestorDetail {
							CapitalDistributed = capitalDistributionLineItem.DistributionAmount,
							CapitalDistributionDate = capitalDistributionLineItem.CapitalDistribution.CapitalDistributionDate,
							CapitalDistributionDueDate = capitalDistributionLineItem.CapitalDistribution.CapitalDistributionDueDate,
							InvestorName = capitalDistributionLineItem.Investor.InvestorName,
							Profit = capitalDistributionLineItem.Profits,
							ProfitReturn = capitalDistributionLineItem.PreferredReturn,
							ReturnFundExpenses = capitalDistributionLineItem.ReturnFundExpenses,
							ReturnManagementFees = capitalDistributionLineItem.ReturnManagementFees,
							LPProfit = capitalDistributionLineItem.LPProfits,
							CostReturn = capitalDistributionLineItem.CapitalReturn
						}).ToList();
			}
		}

		public List<ManagementFeeRateScheduleTierDetail> GetAllManagementFeeRateScheduleTiers(int fundId, DateTime startDate, DateTime endDate) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from rateSchedule in context.FundRateSchedulesTable
						join managementFeeRateSchedule in context.ManagementFeeRateSchedulesTable on rateSchedule.RateScheduleID equals managementFeeRateSchedule.ManagementFeeRateScheduleID
						join managementFeeRateScheduleTier in context.ManagementFeeRateScheduleTiersTable on managementFeeRateSchedule.ManagementFeeRateScheduleID equals managementFeeRateScheduleTier.ManagementFeeRateScheduleID
						where rateSchedule.FundID == fundId
						&& rateSchedule.InvestorTypeID == (int)Models.Investor.Enums.InvestorType.NonManagingMember
						&&
						(
						(EntityFunctions.TruncateTime(managementFeeRateScheduleTier.StartDate) <= EntityFunctions.TruncateTime(startDate) && EntityFunctions.TruncateTime(managementFeeRateScheduleTier.EndDate) >= EntityFunctions.TruncateTime(startDate)) ||
						(EntityFunctions.TruncateTime(managementFeeRateScheduleTier.StartDate) <= EntityFunctions.TruncateTime(endDate) && EntityFunctions.TruncateTime(managementFeeRateScheduleTier.EndDate) >= EntityFunctions.TruncateTime(endDate))
						)
						select new ManagementFeeRateScheduleTierDetail {
							StartDate = managementFeeRateScheduleTier.StartDate,
							EndDate = managementFeeRateScheduleTier.EndDate,
							Multiplier = managementFeeRateScheduleTier.Multiplier,
							MultiplierTypeId = managementFeeRateScheduleTier.MultiplierTypeID
						}).ToList();

			}
		}

		public List<NonManagingInvestorFundDetail> GetAllNonManagingInvestorFunds(int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorFund in context.InvestorFundsTable
						where investorFund.FundID == fundId && investorFund.InvestorTypeID == (int)Models.Investor.Enums.InvestorType.NonManagingMember
						select new NonManagingInvestorFundDetail {
							InvestorId = investorFund.InvestorID,
							TotalCommitment = investorFund.TotalCommitment
						}).ToList();
			}
		}

		public List<AutoCompleteList> FindCapitalCalls(string capitalCallName, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> capitalCallListQuery = (from capitalCall in context.CapitalCallsTable
																	 where (fundId > 0 ? capitalCall.FundID == fundId : capitalCall.FundID > 0)
																	 && capitalCall.CapitalCallNumber.StartsWith(capitalCallName)
																	 orderby capitalCall.Fund.FundName, capitalCall.CapitalCallID
																	 select new AutoCompleteList {
																		 id = capitalCall.CapitalCallID,
																		 label = capitalCall.CapitalCallNumber + "# (" + capitalCall.Fund.FundName + ")",
																		 value = capitalCall.CapitalCallNumber + "# (" + capitalCall.Fund.FundName + ")"
																	 });
				return new PaginatedList<AutoCompleteList>(capitalCallListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindCapitalDistributions(string capitalDistributionName, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> capitalDistributionListQuery = (from capitalDistribution in context.CapitalDistributionsTable
																			 where (fundId > 0 ? capitalDistribution.FundID == fundId : capitalDistribution.FundID > 0)
																			 && capitalDistribution.DistributionNumber.StartsWith(capitalDistributionName)
																			 orderby capitalDistribution.Fund.FundName, capitalDistribution.CapitalDistributionID
																			 select new AutoCompleteList {
																				 id = capitalDistribution.CapitalDistributionID,
																				 label = capitalDistribution.DistributionNumber + "# (" + capitalDistribution.Fund.FundName + ")",
																				 value = capitalDistribution.DistributionNumber + "# (" + capitalDistribution.Fund.FundName + ")"
																			 });
				return new PaginatedList<AutoCompleteList>(capitalDistributionListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public CapitalCallDetail FindCapitalCallDetail(int fundId, decimal? capitalCallAmount, DateTime? capitalCallDate, DateTime? capitalCallDueDate) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCalls
						where capitalCall.FundID == fundId && capitalCall.CapitalAmountCalled == capitalCallAmount
						&& EntityFunctions.TruncateTime(capitalCall.CapitalCallDate) == capitalCallDate
						&& EntityFunctions.TruncateTime(capitalCall.CapitalCallDueDate) == capitalCallDueDate
						select new CapitalCallDetail {
							CapitalCallId = capitalCall.CapitalCallID,
							CapitalCallAmount = capitalCall.CapitalAmountCalled,
							CapitalCallDate = capitalCall.CapitalCallDate,
							CapitalCallDueDate = capitalCall.CapitalCallDueDate,
							FundExpenses = capitalCall.FundExpenses,
							ManagementFees = capitalCall.ManagementFees,
							Number = capitalCall.CapitalCallNumber
						}).FirstOrDefault();
			}
		}

		public CapitalDistributionDetail FindCapitalDistributionDetail(int fundId, decimal? capitalDistributionAmount, DateTime? capitalDistributionDate, DateTime? capitalDistributionDueDate) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistribution in context.CapitalDistributions
						where capitalDistribution.FundID == fundId
						&& capitalDistribution.DistributionAmount == capitalDistributionAmount
						&& EntityFunctions.TruncateTime(capitalDistribution.CapitalDistributionDate) == capitalDistributionDate
						&& EntityFunctions.TruncateTime(capitalDistribution.CapitalDistributionDueDate) == capitalDistributionDueDate
						select new CapitalDistributionDetail {
							CapitalDistributed = capitalDistribution.DistributionAmount,
							Number = capitalDistribution.DistributionNumber,
							CapitalDistrubutionId = capitalDistribution.CapitalDistributionID,
							CapitalDistributionDate = capitalDistribution.CapitalDistributionDate,
							CapitalDistributionDueDate = capitalDistribution.CapitalDistributionDueDate,
							Profit = (capitalDistribution.Profits ?? 0) + (capitalDistribution.LPProfits ?? 0),
							ProfitReturn = capitalDistribution.PreferredReturn,
							ReturnFundExpenses = capitalDistribution.ReturnFundExpenses,
							ReturnManagementFees = capitalDistribution.ReturnManagementFees,
							LPProfit = capitalDistribution.LPProfits,
							CostReturn = capitalDistribution.CapitalReturn
						}).FirstOrDefault();
			}
		}

		#endregion
	}
}
