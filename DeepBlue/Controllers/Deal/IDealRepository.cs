using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;
using DeepBlue.Models.Deal;

namespace DeepBlue.Controllers.Deal {
	public interface IDealRepository {

		#region Deal
		Models.Entity.Deal FindDeal(int dealId);
		DealDetailModel FindDealDetail(int dealId);
		IEnumerable<ErrorInfo> SaveDeal(Models.Entity.Deal deal);
		List<AutoCompleteList> FindDeals(string dealName);
		List<AutoCompleteList> FindDeals(int fundId, string dealName);
		bool DealNameAvailable(string dealName, int dealId, int fundId);
		int GetMaxDealNumber(int fundId);
		List<DealListModel> GetAllDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		object GetDealDetail(int dealId);
		#endregion

		#region DealExpense
		DealClosingCost FindDealClosingCost(int dealClosingCostId);
		DealClosingCostModel FindDealClosingCostModel(int dealClosingCostId);
		void DeleteDealClosingCost(int dealClosingCostId);
		IEnumerable<ErrorInfo> SaveDealClosingCost(DealClosingCost dealClosingCost);
		#endregion

		#region DealUnderlyingFund
		DealUnderlyingFund FindDealUnderlyingFund(int dealUnderlyingFundId);
		DealUnderlyingFundModel FindDealUnderlyingFundModel(int dealUnderlyingFundId);
		List<DealUnderlyingFundModel> GetAllDealUnderlyingFundDetails(int dealId);
		List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId);
		List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId, int dealClosingId);
		List<DealUnderlyingFund> GetAllDealClosingUnderlyingFunds(int dealId);
		List<DealUnderlyingFund> GetAllNotClosingDealUnderlyingFunds(int underlyingFundId, int dealId);
		List<DealUnderlyingFund> GetAllClosingDealUnderlyingFunds(int underlyingFundId, int fundId);
		void DeleteDealUnderlyingFund(int dealUnderlyingFundId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund);
		#endregion

		#region DealUnderlyingDirect
		DealUnderlyingDirect FindDealUnderlyingDirect(int dealUnderlyingDirectId);
		DealUnderlyingDirectModel FindDealUnderlyingDirectModel(int dealUnderlyingDirectId);
		void DeleteDealUnderlyingDirect(int dealUnderlyingDirectId);
		List<DealUnderlyingDirectModel> GetAllDealUnderlyingDirects(int dealId);
		List<DealUnderlyingDirectListModel> GetAllDealUnderlyingDirects(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<DealUnderlyingDirect> GetAllDealClosingUnderlyingDirects(int dealId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect);
		List<AutoCompleteList> FindDealUnderlyingDirects(string fundName);
		List<AutoCompleteList> FindIssuers(string issuerName);
		#endregion

		#region DealClosing
		IEnumerable<ErrorInfo> SaveDealClosing(DealClosing dealClosing);
		CreateDealCloseModel FindDealClosingModel(int dealClosingId, int dealId);
		FinalDealCloseModel GetFinalDealClosingModel(int dealId);
		DealClosing FindDealClosing(int dealClosingId);
		List<DealClosing> GetAllDealClosing(int dealId);
		int GetMaxDealClosingNumber(int dealId);
		bool DealCloseDateAvailable(DateTime dealCloseDate, int dealId, int dealCloseId);
		List<DealUnderlyingDirect> GetDealUnderlyingDirects(int dealId);
		List<DealUnderlyingDirect> GetDealUnderlyingDirects(int dealId, int dealCloseId);
		List<DealCloseListModel> GetAllDealClosingLists(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int dealId);
		#endregion

		#region DealReport
		List<DealReportModel> GetAllReportDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId);
		List<DealReportModel> GetAllExportDeals(string sortName, string sortOrder, int fundId);
		#endregion

		#region UnderlyingFund
		List<UnderlyingFundListModel> GetAllUnderlyingFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<UnderlyingFund> GetAllUnderlyingFunds();
		CreateUnderlyingFundModel FindUnderlyingFundModel(int issuerId);
		UnderlyingFund FindUnderlyingFund(int underlyingFundId);
		IEnumerable<ErrorInfo> SaveUnderlyingFund(UnderlyingFund underlyingFund);
		bool UnderlyingFundNameAvailable(string fundName, int underlyingFundId);
		List<AutoCompleteList> FindUnderlyingFunds(string fundName);
		#endregion

		#region UnderlyingFundCashDistribution
		UnderlyingFundCashDistribution FindUnderlyingFundCashDistribution(int underlyingFundCashDistributionId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution);
		List<UnderlyingFundCashDistributionModel> GetAllUnderlyingFundCashDistributions(int underlyingFundId);
		bool DeleteUnderlyingFundCashDistribution(int id);
		#endregion

		#region UnderlyingFundPostRecordCashDistribution
		List<UnderlyingFundPostRecordCashDistributionModel> GetAllUnderlyingFundPostRecordCashDistributions(int underlyingFundId);
		CashDistribution FindUnderlyingFundPostRecordCashDistribution(int cashDistributionId);
		CashDistribution FindUnderlyingFundPostRecordCashDistribution(int underlyingFundCashDistributionId, int underlyingFundId, int dealId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCashDistribution(CashDistribution cashDistribution);
		decimal GetSumOfCashDistribution(int underlyingFundId, int dealId);
		bool DeleteUnderlyingFundPostRecordCashDistribution(int id);
		#endregion

		#region UnderlyingFundCapitalCall
		UnderlyingFundCapitalCall FindUnderlyingFundCapitalCall(int underlyingFundCapitalCallId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCall underlyingFundCapitalCall);
		List<UnderlyingFundCapitalCallModel> GetAllUnderlyingFundCapitalCalls(int underlyingFundId);
		bool DeleteUnderlyingFundCapitalCall(int id);
		#endregion

		#region UnderlyingFundPostRecordCapitalCall
		UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallLineItemId);
		UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallId, int underlyingFundId, int dealId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCapitalCall(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem);
		List<UnderlyingFundPostRecordCapitalCallModel> GetAllUnderlyingFundPostRecordCapitalCalls(int underlyingFundId);
		decimal GetSumOfUnderlyingFundCapitalCallLineItem(int underlyingFundId, int dealId);
		bool DeleteUnderlyingFundPostRecordCapitalCall(int id);
		#endregion

		#region UnderlyingFundValuation
		List<UnderlyingFundValuationModel> GetAllUnderlyingFundValuations(int underlyingFundId);
		UnderlyingFundValuationModel FindUnderlyingFundValuationModel(int underlyingFundId, int fundId);
		UnderlyingFundNAV FindUnderlyingFundNAV(int underlyingFundId, int fundId);
		bool DeleteUnderlyingFundValuation(int id);
		decimal SumOfTotalCapitalCalls(int underlyingFundId, int fundId);
		decimal SumOfTotalDistributions(int underlyingFundId, int fundId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundNAV(UnderlyingFundNAV underlyingFundNAV);
		decimal FindFundNAV(int underlyingFundId, int fundId);
		#endregion

		#region UnderlyingFundNAVHistory
		IEnumerable<ErrorInfo> SaveUnderlyingFundNAVHistory(UnderlyingFundNAVHistory underlyingFundNAVHistroy);
		#endregion

		#region EquitySplit
		EquitySplit FindEquitySplit(int equityId);
		IEnumerable<ErrorInfo> SaveEquitySplit(EquitySplit equitySplit);
		#endregion

		#region SecurityConversion
		SecurityConversion FindSecurityConversion(int newSecurityId, int newSecurityTypeId);
		IEnumerable<ErrorInfo> SaveSecurityConversion(SecurityConversion securityConversion);
		#endregion

		#region FundActivityHistory
		IEnumerable<ErrorInfo> SaveFundActivityHistory(FundActivityHistory fundActivityHistory);
		#endregion

		#region FundExpense
		IEnumerable<ErrorInfo> SaveFundExpense(FundExpense fundExpense);
		FundExpense FindFundExpense(int fundId);
		FundExpenseModel FindFundExpenseModel(int fundId);
		#endregion

		#region NewHoldingPattern
		List<NewHoldingPatternModel> NewHoldingPatternList(int activityTypeId, int securityTypeId, int securityId);
		#endregion

		#region UnderlyingDirectValuation
		IEnumerable<ErrorInfo> SaveUnderlyingDirectValuation(UnderlyingDirectLastPrice underlyingDirectLastPrice);
		List<UnderlyingDirectValuationModel> UnderlyingDirectValuationList(int issuerId);
		UnderlyingDirectLastPrice FindUnderlyingDirectLastPrice(int fundId, int securityId, int securityTypeId);
		UnderlyingDirectValuationModel FindUnderlyingDirectValuationModel(int underlyingDirectLastPriceId);
		decimal FindLastPurchasePrice(int fundId, int securityId, int securityTypeId);
		bool DeleteUnderlyingDirectValuation(int id);
		#endregion

		#region UnderlyingDirectValuationHistory
		IEnumerable<ErrorInfo> SaveUnderlyingDirectValuationHistory(UnderlyingDirectLastPriceHistory underlyingDirectLastPriceHistory);
		#endregion

		#region UnfundedAdjustment
		List<UnfundedAdjustmentModel> GetAllUnfundedAdjustments(int underlyingFundId);
		DealUnderlyingFundAdjustment FindDealUnderlyingFundAdjustment(int dealUnderlyingFundId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingFundAdjustment(DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment);
		bool DeleteUnfundedAdjustment(int id);
		#endregion

		#region Reconcile
		List<ReconcileListModel> GetAllReconciles(int pageSize, int pageIndex, DateTime fromDate, DateTime toDate, int underlyingFundId, int fundId, ref int totalRows);
		#endregion

		#region Direct
		CreateIssuerModel FindIssuerModel(int id);
		#endregion

	}
}