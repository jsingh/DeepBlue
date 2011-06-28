using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Admin {
	public interface IAdminRepository {

		#region Get
		List<COUNTRY> GetAllCountries();
		List<STATE> GetAllStates();
		List<AddressType> GetAllAddressTypes();
		List<Geography> GetAllGeographies();
		#endregion

		#region EntityType
		List<InvestorEntityType> GetAllInvestorEntityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorEntityType FindInvestorEntityType(int id);
		bool InvestorEntityTypeNameAvailable(string investorEntityTypeName, int investorEntityTypeID);
		bool DeleteInvestorEntityType(int id);
		IEnumerable<ErrorInfo> SaveInvestorEntityType(InvestorEntityType investorEntityType);
		List<InvestorEntityType> GetAllInvestorEntityTypes();
		#endregion

		#region InvestorType
		List<InvestorType> GetAllInvestorTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<InvestorType> GetAllInvestorTypes();
		InvestorType FindInvestorType(int id);
		bool InvestorTypeNameAvailable(string investorTypeName, int investorTypeID);
		bool DeleteInvestorType(int id);
		IEnumerable<ErrorInfo> SaveInvestorType(InvestorType investorType);
		#endregion

		#region FundClosing
		List<FundClosing> GetAllFundClosings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		FundClosing FindFundClosing(int id);
		bool FundClosingNameAvailable(string name, int fundClosingId, int fundId);
		bool DeleteFundClosing(int id);
		IEnumerable<ErrorInfo> SaveFundClosing(FundClosing fundClosing);
		#endregion

		#region CustomField
		List<CustomField> GetAllCustomFields(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<CustomField> GetAllCustomFields(int moduleId);
		List<CustomFieldValue> GetAllCustomFieldValues(int key);
		List<MODULE> GetAllModules();
		List<DataType> GetAllDataTypes();
		CustomField FindCustomField(int id);
		CustomFieldValue FindCustomFieldValue(int customFieldId, int key);
		bool CustomFieldTextAvailable(string customFieldText, int customFieldId, int moduleId);
		bool DeleteCustomField(int id);
		IEnumerable<ErrorInfo> SaveCustomField(CustomField customField);
		IEnumerable<ErrorInfo> SaveCustomFieldValue(CustomFieldValue customFieldValue);
		#endregion

		#region DataType
		List<DataType> GetAllDataTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		DataType FindDataType(int id);
		bool DataTypeNameAvailable(string dataTypeText, int dataTypeId);
		bool DeleteDataType(int id);
		IEnumerable<ErrorInfo> SaveDataType(DataType dataType);
		#endregion

		#region Module
		List<MODULE> GetAllModules(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		MODULE FindModule(int id);
		bool ModuleTextAvailable(string module, int moduleId);
		bool DeleteModule(int id);
		IEnumerable<ErrorInfo> SaveModule(MODULE module);
		#endregion

		#region CommunicationType
		List<CommunicationType> GetAllCommunicationTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<CommunicationType> GetAllCommunicationTypes();
		CommunicationType FindCommunicationType(int id);
		bool CommunicationTypeNameAvailable(string communicationTypeName, int communicationTypeID);
		bool DeleteCommunicationType(int id);
		IEnumerable<ErrorInfo> SaveCommunicationType(CommunicationType communicationType);
		#endregion

		#region CommunicationGrouping
		List<CommunicationGrouping> GetAllCommunicationGroupings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<CommunicationGrouping> GetAllCommunicationGroupings();
		CommunicationGrouping FindCommunicationGrouping(int id);
		bool CommunicationGroupingNameAvailable(string communicationGroupingName, int communicationGroupingID);
		bool DeleteCommunicationGrouping(int id);
		IEnumerable<ErrorInfo> SaveCommunicationGrouping(CommunicationGrouping communicationGrouping);
		#endregion

		#region PurchaseType
		List<PurchaseType> GetAllPurchaseTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<PurchaseType> GetAllPurchaseTypes();
		PurchaseType FindPurchaseType(int id);
		bool PurchaseTypeNameAvailable(string purchaseTypeName, int purchaseTypeID);
		bool DeletePurchaseType(int id);
		IEnumerable<ErrorInfo> SavePurchaseType(PurchaseType purchaseType);
		#endregion

		#region DealClosingCostType
		List<DealClosingCostType> GetAllDealClosingCostTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<DealClosingCostType> GetAllDealClosingCostTypes();
		DealClosingCostType FindDealClosingCostType(int id);
		bool DealClosingCostTypeNameAvailable(string dealClosingCostTypeName, int dealClosingCostTypeID);
		bool DeleteDealClosingCostType(int id);
		IEnumerable<ErrorInfo> SaveDealClosingCostType(DealClosingCostType dealClosingCostType);
		#endregion

		#region DocumentType
		List<DocumentType> GetAllDocumentTypes();
		#endregion

		#region Communication
		string GetContactCommunicationValue(int contactId, DeepBlue.Models.Admin.Enums.CommunicationType communicationType);
		#endregion

		#region SecurityType
		List<SecurityType> GetAllSecurityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<SecurityType> GetAllSecurityTypes();
		SecurityType FindSecurityType(int id);
		bool SecurityTypeNameAvailable(string securityTypeName, int securityTypeID);
		bool DeleteSecurityType(int id);
		IEnumerable<ErrorInfo> SaveSecurityType(SecurityType securityType);
		#endregion

		#region UnderlyingFundType
		List<UnderlyingFundType> GetAllUnderlyingFundTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		UnderlyingFundType FindUnderlyingFundType(int id);
		bool UnderlyingFundTypeNameAvailable(string underlyingFundType, int underlyingfundtypeId);
		bool DeleteUnderlyingFundType(int id);
		IEnumerable<ErrorInfo> SaveUnderlyingFundType(UnderlyingFundType underlyingFundType);
		List<UnderlyingFundType> GetAllUnderlyingFundTypes();
		#endregion

		#region ShareClassType
		List<ShareClassType> GetAllShareClassTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		ShareClassType FindShareClassType(int id);
		bool ShareClassTypeNameAvailable(string shareclasstype, int shareclasstypeId);
		bool DeleteShareClassType(int id);
		IEnumerable<ErrorInfo> SaveShareClassType(ShareClassType shareClassType);
		#endregion

		#region ReportingType
		List<ReportingType> GetAllReportingTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		ReportingType FindReportingType(int id);
		bool ReportingTypeNameAvailable(string reportingtype, int reportingtypeId);
		bool DeleteReportingType(int id);
		IEnumerable<ErrorInfo> SaveReportingType(ReportingType reportingType);
		List<ReportingType> GetAllReportingTypes();
		#endregion

		#region ReportingFrequency
		List<ReportingFrequency> GetAllReportingFrequencies(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		ReportingFrequency FindReportingFrequency(int id);
		bool ReportingFrequencyNameAvailable(string reportingfrequency, int reportingfrequencyId);
		bool DeleteReportingFrequency(int id);
		IEnumerable<ErrorInfo> SaveReportingFrequency(ReportingFrequency reportingFrequency);
		List<ReportingFrequency> GetAllReportingFrequencies();
		#endregion

		#region Geography
		List<Geography> GetAllGeographys(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Geography FindGeography(int id);
		bool GeographyNameAvailable(string reportingfrequency, int reportingfrequencyId);
		bool DeleteGeography(int id);
		IEnumerable<ErrorInfo> SaveGeography(Geography geography);
		#endregion

		#region Industry
		List<Industry> GetAllIndustrys(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<Industry> GetAllIndusties();
		Industry FindIndustry(int id);
		bool IndustryNameAvailable(string industry, int industryId);
		bool DeleteIndustry(int id);
		IEnumerable<ErrorInfo> SaveIndustry(Industry industry);
		List<AutoCompleteList> FindIndustrys(string industryName);
		#endregion

		#region FileType
		List<FileType> GetAllFileTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		FileType FindFileType(int id);
		bool FileTypeNameAvailable(string fileType, int fileTypeId);
		bool DeleteFileType(int id);
		IEnumerable<ErrorInfo> SaveFileType(FileType fileType);
		List<FileType> GetAllFileTypes();
		#endregion

		#region EquityType
		List<EquityType> GetAllEquityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<EquityType> GetAllEquityTypes();
		EquityType FindEquityType(int id);
		bool EquityTypeNameAvailable(string equityTypeName, int equityTypeID);
		bool DeleteEquityType(int id);
		IEnumerable<ErrorInfo> SaveEquityType(EquityType equityType);
		#endregion

		#region FixedIncomeType
		List<FixedIncomeType> GetAllFixedIncomeTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<FixedIncomeType> GetAllFixedIncomeTypes();
		FixedIncomeType FindFixedIncomeType(int id);
		bool FixedIncomeTypeNameAvailable(string fixedIncomeTypeName, int fixedIncomeTypeID);
		bool DeleteFixedIncomeType(int id);
		IEnumerable<ErrorInfo> SaveFixedIncomeType(FixedIncomeType fixedIncomeType);
		#endregion

		#region Currency
		List<Currency> GetAllCurrencies(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<Currency> GetAllCurrencies();
		Currency FindCurrency(int id);
		bool CurrencyNameAvailable(string currency, int currencyId);
		bool DeleteCurrency(int id);
		IEnumerable<ErrorInfo> SaveCurrency(Currency currency);
		#endregion

		#region ShareClassType
		List<ShareClassType> GetAllShareClassTypes();
		#endregion

		#region InvestmentType
		List<InvestmentType> GetAllInvestmentTypes();
		#endregion

		#region CashDistributionTypes
		List<CashDistributionType> GetAllCashDistributionTypes();
		List<CashDistributionType> GetAllCashDistributionTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		CashDistributionType FindCashDistributionType(int id);
		bool CashDistributionTypeNameAvailable(string cashDistributiontype, int cashDistributionTypeId);
		bool DeleteCashDistributionType(int id);
		IEnumerable<ErrorInfo> SaveCashDistributionType(CashDistributionType cashDistributionType);
		#endregion

		#region ActivityTypes
		List<ActivityType> GetAllActivityTypes();
		List<ActivityType> GetAllActivityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		ActivityType FindActivityType(int id);
		bool ActivityTypeNameAvailable(string activityType, int activityTypeId);
		bool DeleteActivityType(int id);
		IEnumerable<ErrorInfo> SaveActivityType(ActivityType activityType);
		#endregion

		#region FundExpenseType
		List<FundExpenseType> GetAllFundExpenseTypes();
		List<FundExpenseType> GetAllFundExpenseTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		FundExpenseType FindFundExpenseType(int id);
		bool FundExpenseTypeNameAvailable(string fundExpenseType, int fundExpenseTypeId);
		bool DeleteFundExpenseType(int id);
		IEnumerable<ErrorInfo> SaveFundExpenseType(FundExpenseType fundExpenseType);
		#endregion

		#region Country
		List<AutoCompleteList> FindCountrys(string countryName);
		#endregion
	}
}
