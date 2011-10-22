using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Helpers;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

namespace DeepBlue.Controllers.CapitalCall {
	public class CapitalCallRepository : ICapitalCallRepository {

		#region CapitalCallRepository Members

		public CreateCapitalCallModel FindCapitalCallModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalCall in context.CapitalCalls
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
				return (from capitalDistribution in context.CapitalDistributions
						where capitalDistribution.CapitalDistributionID == id
						select new CreateDistributionModel {
								CapitalDistributionDate = capitalDistribution.CapitalDistributionDate,
								CapitalDistributionDueDate = capitalDistribution.CapitalDistributionDueDate,
								CapitalDistributionID = capitalDistribution.CapitalDistributionID,
								CapitalReturn  = capitalDistribution.CapitalReturn,
								DistributionAmount = capitalDistribution.DistributionAmount,
								DistributionNumber = capitalDistribution.DistributionNumber,
								FundId = capitalDistribution.FundID,
								GPProfits = capitalDistribution.Profits,
								LPProfits  = capitalDistribution.LPProfits,
								PreferredCatchUp  = capitalDistribution.PreferredCatchUp,
								PreferredReturn = capitalDistribution.PreferredReturn,
								ReturnFundExpenses = capitalDistribution.ReturnFundExpenses,
								ReturnManagementFees = capitalDistribution.ReturnManagementFees,
								TotalDistribution = capitalDistribution.Fund.CapitalDistributions.Sum(distribution => capitalDistribution.DistributionAmount),
								TotalProfit = capitalDistribution.Fund.CapitalDistributions.Sum(distribution => capitalDistribution.Profits),
								FundName = capitalDistribution.Fund.FundName,
								CapitalDistributionLineItemsCount = capitalDistribution.CapitalDistributionLineItems.Count(),
								CapitalDistributionLineItems = (from item in capitalDistribution.CapitalDistributionLineItems
																select new  CapitalDistributionLineItemModel {
																		CapitalDistributionID = item.CapitalDistributionID,
																		CapitalDistributionLineItemID = item.CapitalDistributionLineItemID, 
																		CapitalReturn = item.CapitalReturn,
																		DistributionAmount  = item.DistributionAmount,
																		InvestorName = item.Investor.InvestorName,
																		LPProfits = item.LPProfits,
																		PreferredCatchUp = item.PreferredCatchUp,
																		PreferredReturn = item.PreferredReturn,
																		Profits = item.Profits,
																		ReturnFundExpenses= item.ReturnFundExpenses,
																		ReturnManagementFees  = item.ReturnManagementFees
																})
						}).SingleOrDefault();
			}
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

		public CapitalDistribution FindCapitalDistribution(int capitalDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CapitalDistributions
							  .Include("CapitalDistributionLineItems")
							  .Include("Fund")
							  .Include("CapitalDistributionLineItems.Investor")
							  .SingleOrDefault(capitalDistribution => capitalDistribution.CapitalDistributionID == capitalDistributionId);
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
							TotalCommitment = fund.InvestorFunds.Sum(investorFund => investorFund.TotalCommitment),
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

		public List<CapitalDistributionInvestorDetail> GetCapitalDistributionInvestors(int capitalDistributionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from capitalDistributionLineItem in context.CapitalDistributionLineItems
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
				return (from rateSchedule in context.FundRateSchedules
						join managementFeeRateSchedule in context.ManagementFeeRateSchedules on rateSchedule.RateScheduleID equals managementFeeRateSchedule.ManagementFeeRateScheduleID
						join managementFeeRateScheduleTier in context.ManagementFeeRateScheduleTiers on managementFeeRateSchedule.ManagementFeeRateScheduleID equals managementFeeRateScheduleTier.ManagementFeeRateScheduleID
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
				return (from investorFund in context.InvestorFunds
						where investorFund.FundID == fundId && investorFund.InvestorTypeId == (int)Models.Investor.Enums.InvestorType.NonManagingMember
						select new NonManagingInvestorFundDetail {
							InvestorId = investorFund.InvestorID,
							TotalCommitment = investorFund.TotalCommitment
						}).ToList();
			}
		}

		public List<AutoCompleteList> FindCapitalCalls(string capitalCallName, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> capitalCallListQuery = (from capitalCall in context.CapitalCalls 
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
				IQueryable<AutoCompleteList> capitalDistributionListQuery = (from capitalDistribution in context.CapitalDistributions
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
		#endregion
	}
}