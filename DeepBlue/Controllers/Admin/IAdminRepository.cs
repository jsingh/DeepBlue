﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using DeepBlue.Models.Deal;
using DeepBlue.Models.Admin;
using DeepBlue.Models.Fund;

namespace DeepBlue.Controllers.Admin {
	public interface IAdminRepository {

		#region Get
		List<COUNTRY> GetAllCountries();
		List<STATE> GetAllStates();
		List<AddressType> GetAllAddressTypes();
		List<Geography> GetAllGeographies();
		#endregion

		#region InvestorManagement

		#region EntityType
		List<InvestorEntityType> GetAllInvestorEntityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorEntityType FindInvestorEntityType(int id);
		InvestorEntityType FindInvestorEntityType(string investorEntityTypeName);
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
		List<CustomFieldDetail> GetAllCustomFields(int moduleId);
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

		#region PurchaseType
		List<PurchaseType> GetAllPurchaseTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<PurchaseType> GetAllPurchaseTypes();
		PurchaseType FindPurchaseType(int id);
		PurchaseType FindPurchaseType(string purchaseTypeName);
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

		#region Communication
		string GetContactCommunicationValue(int contactId, DeepBlue.Models.Admin.Enums.CommunicationType communicationType);
		List<CommunicationDetailModel> GetContactCommunications(int? contactId);
		string GetCommunicationValue(List<CommunicationDetailModel> communications, Models.Admin.Enums.CommunicationType communicationType);
		#endregion

		#region SecurityType
		List<SecurityType> GetAllSecurityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<SecurityType> GetAllSecurityTypes();
		SecurityType FindSecurityType(int id);
		SecurityType FindSecurityType(string securityTypeName);
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

		#region File
		IEnumerable<ErrorInfo> SaveFile(File file);
		File FindFile(int fileId);
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
		COUNTRY FindCountry(string countryName);
		#endregion

		#region State
		List<AutoCompleteList> FindStates(string countryName);
		STATE FindState(string stateName);
		#endregion

		#region ExportExcel
		List<string> GetAllTables(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, string tableName);
		object FindTable(string tableName);
		InvestorExportExcelModel GetAllInvestorExportList();
		FundExportExcelModel GetAllFundExportList();
		UnderlyingFundExportExcelModel GetAllUnderlyingFundExportList();
		UnderlyingDirectExportExcelModel GetAllUnderlyingDirectExportList();
		#endregion

		#region DealContacts
		List<AutoCompleteList> FindDealContacts(string contactName);
		Contact FindDealContact(string dealContactName);
		List<DealContactList> GetAllDealContacts(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Contact FindContact(int contactId);
		IEnumerable<ErrorInfo> SaveDealContact(Contact contact);
		bool DealContactNameAvailable(string dealContactName, int contactID);
		bool DeleteDealContact(int id);
		#endregion

		#region User
		List<USER> GetAllUsers(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		USER FindUser(int id);
		bool UserNameAvailable(string userName, int userId);
		bool EmailAvailable(string email, int userId);
		bool DeleteUser(int id);
		IEnumerable<ErrorInfo> SaveUser(USER user);
		#endregion

		#region  DocumentType
		List<Models.Entity.DocumentType> GetAllDocumentTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<DocumentType> GetAllDocumentTypes(int documentSectionId);
		DocumentType FindDocumentType(int id);
		bool DocumentTypeNameAvailable(string documentTypeName, int documentTypeID);
		bool DeleteDocumentType(int id);
		IEnumerable<ErrorInfo> SaveDocumentType(DocumentType documentType);
		List<DocumentSection> GetAllDocumentSections();
		List<AutoCompleteList> FindDocumentTypes(string documentTypeName, int documentSectionId);
		#endregion

		#region Log
		List<Log> GetAllLogs(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		#endregion

		#region SellerType
		List<SellerType> GetAllSellerTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		SellerType FindSellerType(int id);
		SellerType FindSellerType(string sellerTypeName);
		bool SellerTypeNameAvailable(string sellerTypeName, int sellerTypeId);
		bool DeleteSellerType(int id);
		IEnumerable<ErrorInfo> SaveSellerType(SellerType sellerType);
		List<AutoCompleteList> FindSellerTypes(string sellerTypeName);
		#endregion

		#region Entity
		List<ENTITY> GetAllEntities(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		ENTITY FindEntity(int id);
		bool EntityNameAvailable(string entityName, int entityID);
		bool EntityCodeAvailable(string entityCode, int entityID);
		bool DeleteEntity(int id);
		IEnumerable<ErrorInfo> SaveEntity(ENTITY entity);
		List<ENTITY> GetAllEntities();
		#endregion

		#region  Menu
		List<Models.Entity.Menu> GetAllMenus(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Menu FindMenu(int id);
		bool MenuNameAvailable(string menuName, int menuID);
		bool DeleteMenu(int id);
		IEnumerable<ErrorInfo> SaveMenu(Menu menu);
		List<AutoCompleteList> FindMenus(string menuName, int? menuID);
		#endregion

		#region Address
		Address FindAddress(int addressID);
		IEnumerable<ErrorInfo> SaveAddress(Address address);
		#endregion

		#region EntityMenu
		int? GetEntityMenuCount(int entityID);
		List<EntityMenuModel> GetAllEntityMenus();
		List<EntityMenuModel> GetAllEntityMenus(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		EntityMenuModel GetEntityMenu(int id);
		EntityMenu FindEntityMenu(int id);
		bool EntityMenuNameAvailable(string entityMenuName, int entityMenuID);
		bool DeleteEntityMenu(int id);
		bool SaveEntityMenu(int entityID);
		IEnumerable<ErrorInfo> SaveEntityMenu(EntityMenu entityMenu);
		#endregion

		#region ScheduleK1
		List<ScheduleK1ListModel> GetAllScheduleK1s(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? fundID, int? underlyingFundID);
		ScheduleK1Model FindScheduleK1Model(int partnersShareFormID);
		PartnersShareForm FindScheduleK1(int partnersShareFormID);
		bool DeleteScheduleK1(int id);
		IEnumerable<ErrorInfo> SaveScheduleK1(PartnersShareForm scheduleK1);
		#endregion

		#region Broker
		List<Models.Entity.Broker> GetAllBrokers(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		Models.Entity.Broker FindBroker(int brokerID);
		Models.Entity.Broker FindBroker(string brokerName);
		bool BrokerNameAvailable(string brokerName, int brokerID);
		bool DeleteBroker(int brokerID);
		IEnumerable<ErrorInfo> SaveBroker(Models.Entity.Broker broker);
		List<AutoCompleteList> FindBrokers(string brokerName);
		#endregion
	}
}
