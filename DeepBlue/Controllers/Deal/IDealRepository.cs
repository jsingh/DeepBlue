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
		bool DealNameAvailable(string dealName, int dealId, int fundId);
		int GetMaxDealNumber(int fundId);
		List<DealListModel> GetAllDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
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
		List<DealUnderlyingFundDetail> GetAllDealUnderlyingFundDetails(int dealId);
		List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId);
		List<DealUnderlyingFund> GetAllDealUnderlyingFunds(int underlyingFundId, int fundId);
		void DeleteDealUnderlyingFund(int dealUnderlyingFundId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund);
		IEnumerable<ErrorInfo> UpdatePostRecordDateDistribution(int underlyingFundId, int fundId);
		IEnumerable<ErrorInfo> UpdatePostRecordDateCapitalCall(int underlyingFundId, int fundId);
		#endregion

		#region DealUnderlyingDirect
		DealUnderlyingDirect FindDealUnderlyingDirect(int dealUnderlyingDirectId);
		DealUnderlyingDirectModel FindDealUnderlyingDirectModel(int dealUnderlyingDirectId);
		void DeleteDealUnderlyingDirect(int dealUnderlyingDirectId);
		List<DealUnderlyingDirectDetail> GetAllDealUnderlyingDirects(int dealId);
		List<DealUnderlyingDirectListModel> GetAllDealUnderlyingDirects(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect);
		#endregion

		#region DealClosing
		IEnumerable<ErrorInfo> SaveDealClosing(DealClosing dealClosing);
		CreateDealCloseModel FindDealClosingModel(int dealClosingId, int dealId);
		DealClosing FindDealClosing(int dealClosingId);
		int GetMaxDealClosingNumber(int dealId);
		bool DealCloseDateAvailable(DateTime dealCloseDate, int dealId, int dealCloseId);
		List<DealUnderlyingDirect> GetDealUnderlyingDirects(int dealId);
		List<DealCloseListModel> GetAllDealClosingLists(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int dealId);
		#endregion

		#region DealReport
		List<DealReportModel> GetAllReportDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId);
		List<DealReportModel> GetAllExportDeals(string sortName, string sortOrder, int fundId);
		#endregion

		#region UnderlyingFund
		List<UnderlyingFundListModel> GetAllUnderlyingFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<UnderlyingFund> GetAllUnderlyingFunds();
		CreateUnderlyingFundModel FindUnderlyingFundModel(int underlyingFundId);
		UnderlyingFund FindUnderlyingFund(int underlyingFundId);
		IEnumerable<ErrorInfo> SaveUnderlyingFund(UnderlyingFund underlyingFund);
		bool UnderlyingFundNameAvailable(string fundName, int underlyingFundId);
		List<AutoCompleteList> FindUnderlyingFunds(string fundName);
		#endregion

		#region UnderlyingFundCashDistribution
		UnderlyingFundCashDistributionModel FindUnderlyingFundCashDistributionModel(int underlyingFundCashDistributionId);
		UnderlyingFundCashDistribution FindUnderlyingFundCashDistribution(int underlyingFundCashDistributionId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution);
		List<UnderlyingFundCashDistributionList> GetAllUnderlyingFundCashDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		UnderlyingFundCashDistributionList GetUnderlyingFundCashDistribution(int underlyingFundCashDitributionId);
		bool DeleteUnderlyingFundCashDistribution(int id);
		#endregion

		#region CashDistribution
		List<CashDistributionListModel> GetAllCashDistributions(int underlyingFundCashDistributionId);
		CashDistribution FindCashDistribution(int underlyingFundCashDistributionId, int underlyingFundId, int dealId);
		decimal GetSumOfCashDistribution(int underlyingFundId, int dealId);
		IEnumerable<ErrorInfo> SaveCashDistribution(CashDistribution cashDistribution);
		#endregion

		#region UnderlyingFundCapitalCall
		UnderlyingFundCapitalCallModel FindUnderlyingFundCapitalCallModel(int underlyingFundCapitalCallId);
		UnderlyingFundCapitalCall FindUnderlyingFundCapitalCall(int underlyingFundCapitalCallId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCall underlyingFundCapitalCall);
		List<UnderlyingFundCapitalCallList> GetAllUnderlyingFundCapitalCalls(int underlyingFundId);
		UnderlyingFundCapitalCallList GetUnderlyingFundCapitalCall(int underlyingFundCapitalCallId);
		bool DeleteUnderlyingFundCapitalCall(int id);
		#endregion

		#region UnderlyingFundCapitalCallLineItem
		//List<CashDistributionListModel> GetAllCashDistributions(int underlyingFundCashDistributionId);
		UnderlyingFundCapitalCallLineItem FindUnderlyingFundCapitalCallLineItem(int underlyingFundCapitalCallId, int underlyingFundId, int dealId);
		decimal GetSumOfUnderlyingFundCapitalCallLineItem(int underlyingFundId, int dealId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCallLineItem(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem);
		#endregion
	}
}
