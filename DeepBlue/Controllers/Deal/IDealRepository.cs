using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Models.Deal;
using DeepBlue.Models.Deal.Enums;

namespace DeepBlue.Controllers.Deal {
	public interface IDealRepository {

		#region Deal
		Models.Entity.Deal FindDeal(int dealId);
		Models.Entity.Deal FindDeal(string dealName, int fundID);
		int FindDealID(string fundName, int dealNumber);
		DealDetailModel FindDealDetail(int dealId);
		IEnumerable<ErrorInfo> SaveDeal(Models.Entity.Deal deal);
		List<AutoCompleteList> FindDeals(string dealName, int? fundId);
		bool DealNameAvailable(string dealName, int dealId, int fundId);
		int GetMaxDealNumber(int fundId);
		List<DealListModel> GetAllDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, bool? isNotClose, int? fundId, int? dealId);
		List<DealFundListModel> GetAllCloseDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? fundID, int? dealID);
		object GetDealDetail(int dealId);
		int FindLastDealId();
		bool DeleteDeal(int dealId);
		#endregion

		#region DealExpense
		DealClosingCost FindDealClosingCost(int dealClosingCostId);
		DealClosingCost FindDealClosingCost(int dealID, decimal amount, int dealClosingCostTypeID, DateTime date);
		DealClosingCostType FindDealClosingCostType(string dealClosingCostType);
		DealClosingCostModel FindDealClosingCostModel(int dealClosingCostId);
		void DeleteDealClosingCost(int dealClosingCostId);
		IEnumerable<ErrorInfo> SaveDealClosingCost(DealClosingCost dealClosingCost);
		#endregion

		#region DealFundDocument
		DealFundDocument FindDealFundDocument(int dealFundDocumentId);
		IEnumerable<ErrorInfo> SaveDealFundDocument(DealFundDocument dealFundDocument);
		List<DealFundDocumentList> GetAllDealFundDocuments(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int dealId);
		bool DeleteDealFundDocument(int dealFundDocumentId);
		#endregion

		#region DealUnderlyingFund
		DealUnderlyingFund FindDealUnderlyingFund(int dealUnderlyingFundId);
		DealUnderlyingFund FindDealUnderlyingFund(int dealID
													, int underlyingFundID
													, decimal grossPurchasePrice
													, DateTime effectiveDate
													, decimal capitalCommitment
													, decimal unfundedAmount
													, DateTime recordDate);
		DealUnderlyingFundModel FindDealUnderlyingFundModel(int dealUnderlyingFundId);
		List<DealUnderlyingFundModel> GetAllDealUnderlyingFundDetails(int dealId);
		List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId);
		List<DealUnderlyingFund> GetDealUnderlyingFunds(int dealId, int dealClosingId);
		List<DealUnderlyingFund> GetAllDealClosingUnderlyingFunds(int dealId);
		List<DealUnderlyingFund> GetAllNotClosingDealUnderlyingFunds(int underlyingFundId, int dealId);
		List<DealUnderlyingFund> GetAllClosingDealUnderlyingFunds(int underlyingFundId, int fundId);
		bool DeleteDealUnderlyingFund(int dealUnderlyingFundId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund);
		#endregion

		#region DealUnderlyingDirect
		DealUnderlyingDirect FindDealUnderlyingDirect(int dealUnderlyingDirectId);
		DealUnderlyingDirect FindDealUnderlyingDirect(int dealID
												, int securityID
												, int securityTypeID
												, DateTime recordDate
												, int noOfShares
												, decimal fmv
												, decimal purchasePrice
												, decimal taxCostBase
												, DateTime taxCostDate
												);
		DealUnderlyingDirectModel FindDealUnderlyingDirectModel(int dealUnderlyingDirectId);
		bool DeleteDealUnderlyingDirect(int dealUnderlyingDirectId);
		List<DealUnderlyingDirectModel> GetAllDealUnderlyingDirects(int dealId);
		List<DealUnderlyingDirect> GetAllDealUnderlyingDirects(int securityTypeId, int securityId);
		List<DealUnderlyingDirectListModel> GetAllDealUnderlyingDirects(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<DealUnderlyingDirect> GetAllDealClosingUnderlyingDirects(int dealId);
		List<DealUnderlyingDirect> GetAllDealClosingUnderlyingDirects(int securityTypeID, int securityID, int fundID);
		IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect);
		List<AutoCompleteList> FindDealUnderlyingDirects(string fundName);
		List<AutoCompleteListExtend> FindEquityFixedIncomeIssuers(string issuerName);
		object FindEquities();
		#endregion

		#region DealClosing
		IEnumerable<ErrorInfo> SaveDealClosing(DealClosing dealClosing);
		CreateDealCloseModel FindDealClosingModel(int dealClosingId, int dealId);
		CreateDealCloseModel FindDealClosingModel(int dealId);
		CreateDealCloseModel GetFinalDealClosingModel(int dealId);
		object GetFinalDealDetail(int dealId);
		DealClosing FindDealClosing(int dealClosingId);
		DealClosing FindDealClosing(int dealID, int fundID,DateTime closeDate);
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
		List<UnderlyingFundListModel> GetAllUnderlyingFunds(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? gpId, int? underlyingFundID);
		List<UnderlyingFund> GetAllUnderlyingFunds();
		CreateUnderlyingFundModel FindUnderlyingFundModel(int underlyingFundId, int issuerId);
		UnderlyingFund FindUnderlyingFund(int underlyingFundId);
		UnderlyingFund FindUnderlyingFund(string underlyingFundName);
		int? FindUnderlyingFundID(string underlyingFundName);
		int? FindFundID(string fundName);
		Address FindUnderlyingFundAddress(int underlyingFundId);
		IEnumerable<ErrorInfo> SaveUnderlyingFund(UnderlyingFund underlyingFund);
		bool UnderlyingFundNameAvailable(string fundName, int underlyingFundId);
		List<AutoCompleteList> FindReconcileUnderlyingFunds(string fundName, int? fundId);
		List<AutoCompleteList> FindUnderlyingFunds(string fundName);
		UnderlyingFundDocument FindUnderlyingFundDocument(int underlyingFundDocumentId);
		List<UnderlyingFundDocumentList> GetAllUnderlyingFundDocuments(int underlyingFundId, int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		IEnumerable<ErrorInfo> SaveUnderlyingFundDocument(UnderlyingFundDocument underlyingFundDocument);
		bool DeleteUnderlyingFundDocument(int underlyingFundDocumentId);
		List<UnderlyingFundContactList> GetAllUnderlyingFundContacts(int underlyingFundId, int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		UnderlyingFundContact FindUnderlyingFundContact(int underlyingFundContactId);
		IEnumerable<ErrorInfo> SaveUnderlyingFundContact(UnderlyingFundContact underlyingFundContact);
		string GetCommunicationValue(List<CommunicationDetailModel> communications, Models.Admin.Enums.CommunicationType communicationType);
		List<CommunicationDetailModel> GetContactCommunications(int? contactId);
		bool DeleteUnderlyingFundContact(int id);
		IEnumerable<ErrorInfo> SaveUnderlyingFundAddress(Address address);
		#endregion

		#region UnderlyingFundStockDistribution
		UnderlyingFundStockDistribution FindUnderlyingFundStockDistribution(int underlyingFundStockDistributionId);
		UnderlyingFundStockDistributionLineItem FindUnderlyingFundStockDistributionLineItem(int underlyingFundStockDistributionID, int underlyingFundID, int dealID);
		IEnumerable<ErrorInfo> SaveUnderlyingFundStockDistribution(UnderlyingFundStockDistribution underlyingFundStockDistribution);
		IEnumerable<ErrorInfo> SaveUnderlyingFundStockDistributionLineItem(UnderlyingFundStockDistributionLineItem underlyingFundStockDistributionLineItem);
		List<UnderlyingFundStockDistributionModel> GetAllUnderlyingFundStockDistributions(int underlyingFundId);
		List<AutoCompleteList> FindStockIssuers(string issuerName, int underlyingFundId);
		List<AutoCompleteListExtend> FindStockIssuers(int underlyingFundId, int fundId, int issuerId, string equitySymbol);
		List<StockDistributionLineItemModel> GetAllStockDistributionDeals(int fundId, int underlyingFundId);
		#endregion

		#region UnderlyingFundCashDistribution
		UnderlyingFundCashDistribution FindUnderlyingFundCashDistribution(int underlyingFundCashDistributionId);
		object FindUnderlyingFundCashDistribution(int fundID, decimal amount, DateTime noticeDate, int underlyingFundID);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution);
		List<UnderlyingFundCashDistributionModel> GetAllUnderlyingFundCashDistributions(int underlyingFundId);
		bool DeleteUnderlyingFundCashDistribution(int id);
		#endregion

		#region UnderlyingFundPostRecordCashDistribution
		List<UnderlyingFundPostRecordCashDistributionModel> GetAllUnderlyingFundPostRecordCashDistributions(int underlyingFundId);
		CashDistribution FindUnderlyingFundPostRecordCashDistribution(int cashDistributionId);
		CashDistribution FindUnderlyingFundPostRecordCashDistribution(int underlyingFundCashDistributionId, int underlyingFundId, int dealId);
		object FindUnderlyingFundPostRecordCashDistribution(int underlyingFundId, int dealId, decimal amount, DateTime distributionDate);
		IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCashDistribution(CashDistribution cashDistribution);
		decimal GetSumOfCashDistribution(int underlyingFundId, int dealId);
		bool DeleteUnderlyingFundPostRecordCashDistribution(int id);
		#endregion

		#region UnderlyingFundCapitalCall
		UnderlyingFundCapitalCall FindUnderlyingFundCapitalCall(int underlyingFundCapitalCallId);
		object FindUnderlyingFundCapitalCall(int fundID, decimal amount, DateTime noticeDate, DateTime dueDate, int underlyingFundID);
		IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCall underlyingFundCapitalCall);
		List<UnderlyingFundCapitalCallModel> GetAllUnderlyingFundCapitalCalls(int underlyingFundId);
		bool DeleteUnderlyingFundCapitalCall(int id);
		#endregion

		#region UnderlyingFundPostRecordCapitalCall
		UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallLineItemId);
		UnderlyingFundCapitalCallLineItem FindUnderlyingFundPostRecordCapitalCall(int underlyingFundCapitalCallId, int underlyingFundId, int dealId);
		object FindUnderlyingFundPostRecordCapitalCall(int underlyingFundId, int dealId, decimal amount, DateTime capitalCallDate);
		IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCapitalCall(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem);
		List<UnderlyingFundPostRecordCapitalCallModel> GetAllUnderlyingFundPostRecordCapitalCalls(int underlyingFundId);
		decimal GetSumOfUnderlyingFundCapitalCallLineItem(int underlyingFundId, int dealId);
		bool DeleteUnderlyingFundPostRecordCapitalCall(int id);
		#endregion

		#region UnderlyingDirectDividendDistribution
		UnderlyingDirectDividendDistribution FindUnderlyingDirectDividendDistribution(int underlyingFundCashDistributionId);
		DividendDistribution FindDividendDistribution(int underlyingDirectDividendDistributionID, int securityTypeID, int securityID, int dealID);
		IEnumerable<ErrorInfo> SaveUnderlyingDirectDividendDistribution(UnderlyingDirectDividendDistribution underlyingFundCashDistribution);
		IEnumerable<ErrorInfo> SaveDividendDistribution(DividendDistribution dividendDistribution);
		List<DividendDistributionModel> GetAllUnderlyingDirectDividendDistributions(int securityTypeID, int securityID);
		bool DeleteUnderlyingDirectDividendDistribution(int id);
		#endregion

		#region PostRecordDividendDistribution
		DividendDistribution FindPostRecordDividendDistribution(int dividendDistributionID);
		DividendDistribution FindPostRecordDividendDistribution(int underlyingFundDividendDistributionId, int securityTypeID, int securityID, int dealId);
		IEnumerable<ErrorInfo> SavePostRecordDividendDistribution(DividendDistribution underlyingFundDividendDistribution);
		List<PostRecordDividendDistributionModel> GetAllPostRecordDividendDistributions(int securityTypeID, int securityID);
		bool DeletePostRecordDividendDistribution(int id);
		#endregion

		#region UnderlyingFundValuation
		List<UnderlyingFundValuationModel> GetAllUnderlyingFundValuations(int underlyingFundId);
		UnderlyingFundValuationModel FindUnderlyingFundValuationModel(int underlyingFundId, int underlyingFundNAVId);
		UnderlyingFundNAV FindUnderlyingFundNAV(int underlyingFundId, int fundId, DateTime? effectiveDate);
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
		IEnumerable<ErrorInfo> SaveSecurityConversionDetail(SecurityConversionDetail securityConversionDetail);
		#endregion

		#region FundActivityHistory
		IEnumerable<ErrorInfo> SaveFundActivityHistory(FundActivityHistory fundActivityHistory);
		#endregion

		#region FundExpense
		IEnumerable<ErrorInfo> SaveFundExpense(FundExpense fundExpense);
		List<FundExpenseModel> GetAllFundExpenses(int fundId);
		FundExpense FindFundExpense(int fundExpenseId);
		FundExpenseModel FindFundExpenseModel(int fundExpenseId);
		#endregion

		#region NewHoldingPattern
		List<NewHoldingPatternModel> NewHoldingPatternList(int activityTypeId, int activityId, int securityTypeId, int securityId);
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
		UnfundedAdjustmentModel FindUnfundedAdjustmentModel(int dealUnderlyingFundId);
		IEnumerable<ErrorInfo> SaveDealUnderlyingFundAdjustment(DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment);
		#endregion

		#region Reconcile
		List<ReconcileReportModel> GetAllReconciles(DateTime startDate,
													DateTime endDate,
													int? fundId,
													int? underlyingFundId,
													bool isReconcile,
													int pageIndex,
													int pageSize,
													string sortName,
													string sortOrder,
													ref int[] totalRows);
		List<ReconcileReportModel> GetAllReconciles(DateTime startDate,
													DateTime endDate,
													int? fundId,
													int? underlyingFundId,
													bool isReconcile,
													int pageIndex,
													int pageSize,
													string sortName,
													string sortOrder,
													ref int totalRows,
													ReconcileType reconcileType);
		object GetAllFundExpenses(int? fundId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, ref int totalRows);
		#endregion

		#region Direct
		CreateIssuerModel FindIssuerModel(int id);
		bool DeleteIssuer(int issuerId);
		bool IssuerNameAvailable(string issuerName, int issuerId, bool isGP);
		IEnumerable<ErrorInfo> SaveIssuer(Models.Entity.Issuer issuer);
		List<DirectListModel> GetAllDirects(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, bool isGP, int? companyId);
		Models.Entity.Issuer FindIssuer(int issuerId);
		Models.Entity.Issuer FindIssuer(string issuerName);
		List<AutoCompleteList> FindIssuers(string issuerName);
		List<AutoCompleteList> FindCompanys(string issuerName);
		List<AutoCompleteList> FindGPs(string issuerName);
		List<DeepBlue.Models.Entity.Issuer> GetAllIssuers();
		IEnumerable<ErrorInfo> SaveUnderlyingDirectDocument(UnderlyingDirectDocument underlyingDirectDocument);
		bool FindAnnualMeetingDateHistory(int issuerID, DateTime annualMeetingDate);
		IEnumerable<ErrorInfo> SaveAnnualMeetingHistory(AnnualMeetingHistory annualMeetingHistory);
		List<AnnualMeetingHistory> GetAllAnnualMettingDates(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		IEnumerable<object> UnderlyingDirectList(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int companyId);

		#region Equity
		List<Equity> GetAllEquity(int issuerId);
		List<EquityListModel> GetAllEquity(int issuerId, int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Equity FindEquity(int equityId);
		Equity FindEquity(int issuerID, string symbol);
		IEnumerable<ErrorInfo> SaveEquity(Equity equity);
		bool DeleteEquity(int id);
		List<AutoCompleteList> FindEquityDirects(string issuerName);
		string FindEquitySymbol(int id);
		object FindEquitySecurityConversionModel(int equityId);
		object FindEquityModel(int id);
		#endregion

		#region FixedIncome
		FixedIncome FindFixedIncome(int fixedIncomeId);
		List<FixedIncome> GetAllFixedIncome(int issuerId);
		IEnumerable<ErrorInfo> SaveFixedIncome(FixedIncome fixedIncome);
		bool DeleteFixedIncome(int id);
		List<AutoCompleteList> FindFixedIncomeDirects(string issuerName);
		object FindFixedIncomeSecurityConversionModel(int fixedIncomeId);
		object FindFixedIncomeModel(int id);
		#endregion

		#endregion

		#region UnderlyingDirectDocument
		List<UnderlyingDirectDocumentList> GetAllUnderlyingDirectDocuments(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int securityID, int securityTypeID);
		bool DeleteUnderlyingDirectDocument(int underlyingDirectDocumentId);
		#endregion

		#region View Activities
		List<DealReportModel> GetAllActivitiesDeals(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int fundId, int? dealID);
		object GetUnderlyingFundCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? underlyingFundID, int dealID);
		object GetUnderlyingFundCashDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? underlyingFundID, int dealID);
		object GetUnderlyingFundPostRecordCapitalCalls(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? underlyingFundID, int dealID);
		object GetUnderlyingFundPostRecordCashDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? underlyingFundID, int dealID);
		object GetUnderlyingFundStockDistributions(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? underlyingFundID, int dealID);
		object GetUnderlyingFundAdjustments(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? dealUnderlyingFundID, int dealID);
		object GetUnderlyingFundValuations(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? dealUnderlyingFundID, int? underlyingFundID, int dealID);
		object GetUnderlyingFundValuationHistories(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? dealUnderlyingFundID, int? underlyingFundID, int dealID);
		#endregion

	}
}