using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using System.Reflection;
using System.Linq.Expressions;

namespace DeepBlue.Controllers.Admin {
	public class AdminRepository : IAdminRepository {

		#region InvestorManagement

		#region  InvestorEntityType

		public List<Models.Entity.InvestorEntityType> GetAllInvestorEntityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.InvestorEntityType> query = (from entityType in context.InvestorEntityTypes
																	  select entityType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.InvestorEntityType> paginatedList = new PaginatedList<Models.Entity.InvestorEntityType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public InvestorEntityType FindInvestorEntityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorEntityTypes.SingleOrDefault(entityType => entityType.InvestorEntityTypeID == id);
			}
		}

		public bool InvestorEntityTypeNameAvailable(string investorEntityTypeName, int investorEntityTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from entityType in context.InvestorEntityTypes
						 where entityType.InvestorEntityTypeName == investorEntityTypeName && entityType.InvestorEntityTypeID != investorEntityTypeID
						 select entityType.InvestorEntityTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteInvestorEntityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorEntityType investorEntityType = context.InvestorEntityTypes.SingleOrDefault(entityType => entityType.InvestorEntityTypeID == id);
				if (investorEntityType != null) {
					if (investorEntityType.Investors.Count == 0) {
						context.InvestorEntityTypes.DeleteObject(investorEntityType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorEntityType(InvestorEntityType investorEntityType) {
			return investorEntityType.Save();
		}

		#endregion

		#region  InvestorType

		public List<Models.Entity.InvestorType> GetAllInvestorTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.InvestorType> query = (from investorType in context.InvestorTypes
																select investorType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<InvestorType> paginatedList = new PaginatedList<InvestorType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<InvestorType> GetAllInvestorTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorType in context.InvestorTypes
						where investorType.Enabled == true
						orderby investorType.InvestorTypeName
						select investorType).ToList();
			}
		}

		public InvestorType FindInvestorType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorTypes.SingleOrDefault(type => type.InvestorTypeID == id);
			}
		}

		public bool InvestorTypeNameAvailable(string investorTypeName, int investorTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.InvestorTypes
						 where type.InvestorTypeName == investorTypeName && type.InvestorTypeID != investorTypeID
						 select type.InvestorTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteInvestorType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorType investorType = context.InvestorTypes.SingleOrDefault(type => type.InvestorTypeID == id);
				if (investorType != null) {
					if (investorType.InvestorFunds.Count == 0) {
						context.InvestorTypes.DeleteObject(investorType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorType(InvestorType investorType) {
			return investorType.Save();
		}

		#endregion

		#region  CommunicationType

		public List<Models.Entity.CommunicationType> GetAllCommunicationTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CommunicationType> query = (from communicationType in context.CommunicationTypes.Include("CommunicationGrouping")
																	 select communicationType);
				switch (sortName) {
					case "CommunicationGroupingName":
						query = (sortOrder == "asc" ? query.OrderBy(field => field.CommunicationGrouping.CommunicationGroupingName) : query.OrderByDescending(field => field.CommunicationGrouping.CommunicationGroupingName));
						break;
					default:
						query = query.OrderBy(sortName, (sortOrder == "asc"));
						break;
				}
				PaginatedList<CommunicationType> paginatedList = new PaginatedList<CommunicationType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<CommunicationType> GetAllCommunicationTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from communicationType in context.CommunicationTypes
						where communicationType.Enabled == true
						orderby communicationType.CommunicationTypeName
						select communicationType).ToList();
			}
		}

		public CommunicationType FindCommunicationType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CommunicationTypes.Include("CommunicationGrouping").SingleOrDefault(type => type.CommunicationTypeID == id);
			}
		}

		public bool CommunicationTypeNameAvailable(string communicationTypeName, int communicationTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.CommunicationTypes
						 where type.CommunicationTypeName == communicationTypeName && type.CommunicationTypeID != communicationTypeID
						 select type.CommunicationTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCommunicationType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CommunicationType communicationType = context.CommunicationTypes.SingleOrDefault(type => type.CommunicationTypeID == id);
				if (communicationType != null) {
					if (communicationType.Communications.Count == 0 && communicationType.CommunicationGrouping != null) {
						context.CommunicationTypes.DeleteObject(communicationType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveCommunicationType(CommunicationType communicationType) {
			return communicationType.Save();
		}

		#endregion

		#region  CommunicationGrouping

		public List<Models.Entity.CommunicationGrouping> GetAllCommunicationGroupings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CommunicationGrouping> query = (from communicationGrouping in context.CommunicationGroupings
																		 select communicationGrouping);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<CommunicationGrouping> paginatedList = new PaginatedList<CommunicationGrouping>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<CommunicationGrouping> GetAllCommunicationGroupings() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from communicationGrouping in context.CommunicationGroupings
						orderby communicationGrouping.CommunicationGroupingName
						select communicationGrouping).ToList();
			}
		}

		public CommunicationGrouping FindCommunicationGrouping(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CommunicationGroupings.SingleOrDefault(type => type.CommunicationGroupingID == id);
			}
		}

		public bool CommunicationGroupingNameAvailable(string communicationGroupingName, int communicationGroupingID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.CommunicationGroupings
						 where type.CommunicationGroupingName == communicationGroupingName && type.CommunicationGroupingID != communicationGroupingID
						 select type.CommunicationGroupingID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCommunicationGrouping(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CommunicationGrouping communicationGrouping = context.CommunicationGroupings.SingleOrDefault(type => type.CommunicationGroupingID == id);
				if (communicationGrouping != null) {
					if (communicationGrouping.CommunicationTypes.Count == 0) {
						context.CommunicationGroupings.DeleteObject(communicationGrouping);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveCommunicationGrouping(CommunicationGrouping communicationGrouping) {
			return communicationGrouping.Save();
		}

		#endregion

		#endregion

		#region CustomFieldManagement

		#region  CustomField

		public List<Models.Entity.CustomField> GetAllCustomFields(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CustomField> query = (from customField in context.CustomFields
																						 .Include("MODULE")
																						 .Include("DataType")
															   select customField);
				switch (sortName) {
					case "ModuleName":
						query = (sortOrder == "asc" ? query.OrderBy(field => field.MODULE.ModuleName) : query.OrderByDescending(field => field.MODULE.ModuleName));
						break;
					case "DataTypeName":
						query = (sortOrder == "asc" ? query.OrderBy(field => field.DataType.DataTypeName) : query.OrderByDescending(field => field.DataType.DataTypeName));
						break;
					default:
						query = query.OrderBy(sortName, (sortOrder == "asc"));
						break;
				}
				PaginatedList<CustomField> paginatedList = new PaginatedList<CustomField>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<CustomFieldDetail> GetAllCustomFields(int moduleId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from field in context.CustomFields
						where field.ModuleID == moduleId
						orderby field.CustomFieldText
						select new CustomFieldDetail {
							CustomFieldId = field.CustomFieldID,
							CustomFieldText = field.CustomFieldText,
							DataTypeId = field.DataTypeID,
							ModuleId = field.ModuleID,
							OptionalText = field.OptionalText,
							Search = field.Search
						}).ToList();
			}
		}

		public List<MODULE> GetAllModules() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.MODULEs.OrderBy(module => module.ModuleName).ToList();
			}
		}

		public List<CustomFieldValue> GetAllCustomFieldValues(int key) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from customField in context.CustomFieldValues.Include("CustomField")
						where customField.Key == key
						select customField).ToList();
			}
		}

		public List<DataType> GetAllDataTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DataTypes.OrderBy(dataType => dataType.DataTypeName).ToList();
			}
		}

		public CustomField FindCustomField(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CustomFields
							  .Include("OptionFields")
							  .Include("MODULE")
							  .Include("DataType")
							  .SingleOrDefault(field => field.CustomFieldID == id);
			}
		}

		public CustomFieldValue FindCustomFieldValue(int customFieldId, int key) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CustomFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == customFieldId && fieldValue.Key == key);
			}
		}

		public bool CustomFieldTextAvailable(string customFieldText, int customFieldId, int moduleId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.CustomFields
						 where field.CustomFieldText == customFieldText && field.CustomFieldID != customFieldId && field.ModuleID == moduleId
						 select field.CustomFieldID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCustomField(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CustomField customField = context.CustomFields.SingleOrDefault(field => field.CustomFieldID == id);
				if (customField != null) {
					if (customField.OptionFields.Count == 0 && customField.CustomFieldValues.Count == 0) {
						context.CustomFields.DeleteObject(customField);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveCustomField(CustomField customField) {
			return customField.Save();
		}

		public IEnumerable<ErrorInfo> SaveCustomFieldValue(CustomFieldValue customFieldValue) {
			return customFieldValue.Save();
		}
		#endregion

		#region  DataType

		public List<DataType> GetAllDataTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.DataType> query = (from customField in context.DataTypes
															select customField);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DataType> paginatedList = new PaginatedList<DataType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public DataType FindDataType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DataTypes.SingleOrDefault(dataType => dataType.DataTypeID == id);
			}
		}

		public bool DataTypeNameAvailable(string dataTypeName, int dataTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from dataType in context.DataTypes
						 where dataType.DataTypeName == dataTypeName && dataType.DataTypeID != dataTypeId
						 select dataType.DataTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteDataType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DataType dataType = context.DataTypes.SingleOrDefault(type => type.DataTypeID == id);
				if (dataType != null) {
					if (dataType.CustomFields.Count == 0) {
						context.DataTypes.DeleteObject(dataType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveDataType(DataType dataType) {
			return dataType.Save();
		}

		#endregion

		#endregion

		#region DealManagement

		#region  PurchaseType

		public List<Models.Entity.PurchaseType> GetAllPurchaseTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.PurchaseType> query = (from purchaseType in context.PurchaseTypes
																select purchaseType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<PurchaseType> paginatedList = new PaginatedList<PurchaseType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<PurchaseType> GetAllPurchaseTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from purchaseType in context.PurchaseTypes
						orderby purchaseType.Name
						select purchaseType).ToList();
			}
		}

		public PurchaseType FindPurchaseType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.PurchaseTypes.SingleOrDefault(type => type.PurchaseTypeID == id);
			}
		}

		public bool PurchaseTypeNameAvailable(string purchaseTypeName, int purchaseTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.PurchaseTypes
						 where type.Name == purchaseTypeName && type.PurchaseTypeID != purchaseTypeID
						 select type.PurchaseTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeletePurchaseType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				PurchaseType purchaseType = context.PurchaseTypes.SingleOrDefault(type => type.PurchaseTypeID == id);
				if (purchaseType != null) {
					if (purchaseType.Deals.Count == 0) {
						context.PurchaseTypes.DeleteObject(purchaseType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SavePurchaseType(PurchaseType purchaseType) {
			return purchaseType.Save();
		}

		#endregion

		#region  DealClosingCostType

		public List<Models.Entity.DealClosingCostType> GetAllDealClosingCostTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.DealClosingCostType> query = (from dealClosingCostType in context.DealClosingCostTypes
																	   select dealClosingCostType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealClosingCostType> paginatedList = new PaginatedList<DealClosingCostType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<DealClosingCostType> GetAllDealClosingCostTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from dealClosingCostType in context.DealClosingCostTypes
						orderby dealClosingCostType.Name
						select dealClosingCostType).ToList();
			}
		}

		public DealClosingCostType FindDealClosingCostType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DealClosingCostTypes.SingleOrDefault(type => type.DealClosingCostTypeID == id);
			}
		}

		public bool DealClosingCostTypeNameAvailable(string dealClosingCostTypeName, int dealClosingCostTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.DealClosingCostTypes
						 where type.Name == dealClosingCostTypeName && type.DealClosingCostTypeID != dealClosingCostTypeID
						 select type.DealClosingCostTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteDealClosingCostType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DealClosingCostType dealClosingCostType = context.DealClosingCostTypes.SingleOrDefault(type => type.DealClosingCostTypeID == id);
				if (dealClosingCostType != null) {
					if (dealClosingCostType.DealClosingCosts.Count == 0) {
						context.DealClosingCostTypes.DeleteObject(dealClosingCostType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveDealClosingCostType(DealClosingCostType dealClosingCostType) {
			return dealClosingCostType.Save();
		}

		#endregion

		#region  UnderlingFundType

		public List<Models.Entity.UnderlyingFundType> GetAllUnderlyingFundTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.UnderlyingFundType> query = (from underlyingfundtype in context.UnderlyingFundTypes
																	  select underlyingfundtype);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.UnderlyingFundType> paginatedList = new PaginatedList<Models.Entity.UnderlyingFundType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public UnderlyingFundType FindUnderlyingFundType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.UnderlyingFundTypes.SingleOrDefault(field => field.UnderlyingFundTypeID == id);
			}
		}

		public bool UnderlyingFundTypeNameAvailable(string underlyingfundtypeFieldText, int underlyingfundtypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.UnderlyingFundTypes
						 where field.Name == underlyingfundtypeFieldText && field.UnderlyingFundTypeID != underlyingfundtypeFieldId
						 select field.UnderlyingFundTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteUnderlyingFundType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundType underlyingFundType = context.UnderlyingFundTypes.SingleOrDefault(field => field.UnderlyingFundTypeID == id);
				if (underlyingFundType != null) {
					if (underlyingFundType.UnderlyingFunds.Count == 0) {
						context.UnderlyingFundTypes.DeleteObject(underlyingFundType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundType(UnderlyingFundType underlyingFundType) {
			return underlyingFundType.Save();
		}

		public List<UnderlyingFundType> GetAllUnderlyingFundTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from underlyingFundType in context.UnderlyingFundTypes
						orderby underlyingFundType.Name
						select underlyingFundType).ToList();
			}
		}
		#endregion

		#region  ShareClassType

		public List<Models.Entity.ShareClassType> GetAllShareClassTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.ShareClassType> query = (from shareclasstype in context.ShareClassTypes
																  select shareclasstype);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.ShareClassType> paginatedList = new PaginatedList<Models.Entity.ShareClassType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public ShareClassType FindShareClassType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ShareClassTypes.SingleOrDefault(field => field.ShareClassTypeID == id);
			}
		}

		public bool ShareClassTypeNameAvailable(string shareclasstypeFieldText, int shareclasstypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.ShareClassTypes
						 where field.ShareClass == shareclasstypeFieldText && field.ShareClassTypeID != shareclasstypeFieldId
						 select field.ShareClassTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteShareClassType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ShareClassType shareClassType = context.ShareClassTypes.SingleOrDefault(field => field.ShareClassTypeID == id);
				if (shareClassType != null) {
					if (shareClassType.UnderlyingFunds.Count == 0) {
						context.ShareClassTypes.DeleteObject(shareClassType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveShareClassType(ShareClassType shareClassType) {
			return shareClassType.Save();
		}
		#endregion

		#region  ReportingType

		public List<Models.Entity.ReportingType> GetAllReportingTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.ReportingType> query = (from reportingtype in context.ReportingTypes
																 select reportingtype);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.ReportingType> paginatedList = new PaginatedList<Models.Entity.ReportingType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public ReportingType FindReportingType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ReportingTypes.SingleOrDefault(field => field.ReportingTypeID == id);
			}
		}

		public bool ReportingTypeNameAvailable(string reportingtypeFieldText, int reportingtypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.ReportingTypes
						 where field.Reporting == reportingtypeFieldText && field.ReportingTypeID != reportingtypeFieldId
						 select field.ReportingTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteReportingType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ReportingType reportingType = context.ReportingTypes.SingleOrDefault(field => field.ReportingTypeID == id);
				if (reportingType != null) {
					if (reportingType.UnderlyingFunds.Count == 0) {
						context.ReportingTypes.DeleteObject(reportingType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveReportingType(ReportingType reportingType) {
			return reportingType.Save();
		}


		public List<ReportingType> GetAllReportingTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from reportingType in context.ReportingTypes
						where reportingType.Enabled == true
						orderby reportingType.Reporting
						select reportingType).ToList();
			}
		}

		#endregion

		#region  ReportingFrequency

		public List<Models.Entity.ReportingFrequency> GetAllReportingFrequencies(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.ReportingFrequency> query = (from reportingfrequency in context.ReportingFrequencies
																	  select reportingfrequency);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.ReportingFrequency> paginatedList = new PaginatedList<Models.Entity.ReportingFrequency>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public ReportingFrequency FindReportingFrequency(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ReportingFrequencies.SingleOrDefault(field => field.ReportingFrequencyID == id);
			}
		}

		public bool ReportingFrequencyNameAvailable(string reportingfrequencyFieldText, int reportingfrequencyFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.ReportingFrequencies
						 where field.ReportingFrequency1 == reportingfrequencyFieldText && field.ReportingFrequencyID != reportingfrequencyFieldId
						 select field.ReportingFrequencyID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteReportingFrequency(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ReportingFrequency reportingFrequency = context.ReportingFrequencies.SingleOrDefault(field => field.ReportingFrequencyID == id);
				if (reportingFrequency != null) {
					if (reportingFrequency.UnderlyingFunds.Count == 0) {
						context.ReportingFrequencies.DeleteObject(reportingFrequency);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveReportingFrequency(ReportingFrequency reportingFrequency) {
			return reportingFrequency.Save();
		}

		public List<ReportingFrequency> GetAllReportingFrequencies() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from reportingFrequency in context.ReportingFrequencies
						where reportingFrequency.Enabled == true
						orderby reportingFrequency.ReportingFrequency1
						select reportingFrequency).ToList();
			}
		}
		#endregion

		#region  CashDistributionType

		public List<Models.Entity.CashDistributionType> GetAllCashDistributionTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CashDistributionType> query = (from cashDistributionType in context.CashDistributionTypes
																		select cashDistributionType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<CashDistributionType> paginatedList = new PaginatedList<CashDistributionType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<CashDistributionType> GetAllCashDistributionTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from cashDistributionType in context.CashDistributionTypes
						where cashDistributionType.Enabled == true
						orderby cashDistributionType.Name
						select cashDistributionType).ToList();
			}
		}

		public CashDistributionType FindCashDistributionType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CashDistributionTypes.SingleOrDefault(type => type.CashDistributionTypeID == id);
			}
		}

		public bool CashDistributionTypeNameAvailable(string cashDistributionTypeName, int cashDistributionTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.CashDistributionTypes
						 where type.Name == cashDistributionTypeName && type.CashDistributionTypeID != cashDistributionTypeID
						 select type.CashDistributionTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCashDistributionType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CashDistributionType cashDistributionType = context.CashDistributionTypes.SingleOrDefault(type => type.CashDistributionTypeID == id);
				if (cashDistributionType != null) {
					if (cashDistributionType.UnderlyingFundCashDistributions.Count == 0) {
						context.CashDistributionTypes.DeleteObject(cashDistributionType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveCashDistributionType(CashDistributionType cashDistributionType) {
			return cashDistributionType.Save();
		}

		#endregion

		#region  FundExpenseType

		public List<Models.Entity.FundExpenseType> GetAllFundExpenseTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.FundExpenseType> query = (from fundExpenseType in context.FundExpenseTypes
																   select fundExpenseType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<FundExpenseType> paginatedList = new PaginatedList<FundExpenseType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<FundExpenseType> GetAllFundExpenseTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fundExpense in context.FundExpenseTypes
						orderby fundExpense.Name
						select fundExpense).ToList();
			}
		}


		public FundExpenseType FindFundExpenseType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundExpenseTypes.SingleOrDefault(type => type.FundExpenseTypeID == id);
			}
		}

		public bool FundExpenseTypeNameAvailable(string fundExpenseTypeName, int fundExpenseTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.FundExpenseTypes
						 where type.Name == fundExpenseTypeName && type.FundExpenseTypeID != fundExpenseTypeID
						 select type.FundExpenseTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteFundExpenseType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundExpenseType fundExpenseType = context.FundExpenseTypes.SingleOrDefault(type => type.FundExpenseTypeID == id);
				if (fundExpenseType != null) {
					if (fundExpenseType.FundExpenses.Count == 0) {
						context.FundExpenseTypes.DeleteObject(fundExpenseType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveFundExpenseType(FundExpenseType fundExpenseType) {
			return fundExpenseType.Save();
		}

		#endregion

		#endregion

		#region  FundClosing

		public List<Models.Entity.FundClosing> GetAllFundClosings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.FundClosing> query = (from fund in context.FundClosings
																		 .Include("Fund")
															   select fund);
				if (sortName == "FundName") {
					query = (sortOrder == "asc" ? query.OrderBy(fund => fund.Fund.FundName) : query.OrderByDescending(fund => fund.Fund.FundName));
				}
				else {
					query = query.OrderBy(sortName, (sortOrder == "asc"));
				}
				PaginatedList<Models.Entity.FundClosing> paginatedList = new PaginatedList<Models.Entity.FundClosing>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public FundClosing FindFundClosing(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundClosings.SingleOrDefault(fundClose => fundClose.FundClosingID == id);
			}
		}

		public bool FundClosingNameAvailable(string name, int fundclosingId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from fundClose in context.FundClosings
						 where fundClose.Name == name && fundClose.FundClosingID != fundclosingId && fundClose.FundID == fundId
						 select fundClose.FundClosingID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteFundClosing(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundClosing fundclose = context.FundClosings.SingleOrDefault(close => close.FundClosingID == id);
				if (fundclose != null) {
					if (fundclose.InvestorFundTransactions.Count() == 0) {
						context.FundClosings.DeleteObject(fundclose);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveFundClosing(FundClosing fundClosing) {
			return fundClosing.Save();
		}

		#endregion

		#region  Module

		public List<Models.Entity.MODULE> GetAllModules(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.MODULE> query = (from module in context.MODULEs
														  select module);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.MODULE> paginatedList = new PaginatedList<Models.Entity.MODULE>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public MODULE FindModule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.MODULEs.SingleOrDefault(field => field.ModuleID == id);
			}
		}

		public bool ModuleTextAvailable(string moduleFieldText, int moduleFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.MODULEs
						 where field.ModuleName == moduleFieldText && field.ModuleID != moduleFieldId
						 select field.ModuleID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteModule(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				MODULE module = context.MODULEs.SingleOrDefault(field => field.ModuleID == id);
				if (module != null) {
					if (module.CustomFields.Count == 0) {
						context.MODULEs.DeleteObject(module);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveModule(MODULE module) {
			return module.Save();
		}
		#endregion

		#region  Get

		public List<COUNTRY> GetAllCountries() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from country in context.COUNTRies
						orderby country.CountryName ascending
						select country).ToList();
			}
		}

		public List<STATE> GetAllStates() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from state in context.STATEs
						orderby state.Name ascending
						select state).ToList();
			}
		}

		public List<InvestorEntityType> GetAllInvestorEntityTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorEntityType in context.InvestorEntityTypes
						where investorEntityType.Enabled == true
						orderby investorEntityType.InvestorEntityTypeName
						select investorEntityType).ToList();
			}
		}

		public List<AddressType> GetAllAddressTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from addressType in context.AddressTypes
						orderby addressType.AddressTypeName
						select addressType).ToList();
			}
		}

		#endregion

		#region  DocumentTypes
		public List<DocumentType> GetAllDocumentTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from document in context.DocumentTypes
						orderby document.DocumentTypeName
						select document).ToList();
			}
		}
		#endregion

		#region  Communication
		public string GetContactCommunicationValue(int contactId, Models.Admin.Enums.CommunicationType communicationType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from contactCommunication in context.ContactCommunications
						where contactCommunication.ContactID == contactId && contactCommunication.Communication.CommunicationTypeID == (int)communicationType
						select contactCommunication.Communication.CommunicationValue).FirstOrDefault();
			}
		}
		#endregion

		#region  SecurityType

		public List<Models.Entity.SecurityType> GetAllSecurityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.SecurityType> query = (from securityType in context.SecurityTypes
																select securityType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<SecurityType> paginatedList = new PaginatedList<SecurityType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<SecurityType> GetAllSecurityTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from securityType in context.SecurityTypes
						orderby securityType.Name
						select securityType).ToList();
			}
		}

		public SecurityType FindSecurityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.SecurityTypes.SingleOrDefault(type => type.SecurityTypeID == id);
			}
		}

		public bool SecurityTypeNameAvailable(string securityTypeName, int securityTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.SecurityTypes
						 where type.Name == securityTypeName && type.SecurityTypeID != securityTypeID
						 select type.SecurityTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteSecurityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				SecurityType securityType = context.SecurityTypes.SingleOrDefault(type => type.SecurityTypeID == id);
				if (securityType != null) {
					if (securityType.DealUnderlyingDirects.Count == 0) {
						context.SecurityTypes.DeleteObject(securityType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveSecurityType(SecurityType securityType) {
			return securityType.Save();
		}

		#endregion

		#region  Geography

		public List<Models.Entity.Geography> GetAllGeographys(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Geography> query = (from geography in context.Geographies
															 select geography);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.Geography> paginatedList = new PaginatedList<Models.Entity.Geography>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Geography FindGeography(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Geographies.SingleOrDefault(field => field.GeographyID == id);
			}
		}

		public bool GeographyNameAvailable(string geographyFieldText, int geographyFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.Geographies
						 where field.Geography1 == geographyFieldText && field.GeographyID != geographyFieldId
						 select field.GeographyID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteGeography(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Geography geography = context.Geographies.SingleOrDefault(field => field.GeographyID == id);
				if (geography != null) {
					if (geography.UnderlyingFunds.Count == 0) {
						context.Geographies.DeleteObject(geography);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveGeography(Geography geography) {
			return geography.Save();
		}

		public List<Geography> GetAllGeographies() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from geogryphy in context.Geographies
						where geogryphy.Enabled == true
						orderby geogryphy.Geography1
						select geogryphy).ToList();
			}
		}
		#endregion

		#region  Industry

		public List<Models.Entity.Industry> GetAllIndustrys(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Industry> query = (from industry in context.Industries
															select industry);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.Industry> paginatedList = new PaginatedList<Models.Entity.Industry>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Industry FindIndustry(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Industries.SingleOrDefault(field => field.IndustryID == id);
			}
		}

		public bool IndustryNameAvailable(string industryFieldText, int industryFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.Industries
						 where field.Industry1 == industryFieldText && field.IndustryID != industryFieldId
						 select field.IndustryID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteIndustry(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Industry industry = context.Industries.SingleOrDefault(field => field.IndustryID == id);
				if (industry != null) {
					if (industry.UnderlyingFunds.Count == 0) {
						context.Industries.DeleteObject(industry);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveIndustry(Industry industry) {
			return industry.Save();
		}

		public List<Industry> GetAllIndusties() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from industry in context.Industries
						where industry.Enabled == true
						orderby industry.Industry1
						select industry).ToList();
			}
		}

		public List<AutoCompleteList> FindIndustrys(string industryName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> industryListQuery = (from industry in context.Industries
																  where industry.Industry1.StartsWith(industryName)
																  orderby industry.Industry1
																  select new AutoCompleteList {
																	  id = industry.IndustryID,
																	  label = industry.Industry1,
																	  value = industry.Industry1
																  });
				return new PaginatedList<AutoCompleteList>(industryListQuery, 1, 20);
			}
		}
		#endregion

		#region  FileType

		public List<Models.Entity.FileType> GetAllFileTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.FileType> query = (from fileType in context.FileTypes
															select fileType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.FileType> paginatedList = new PaginatedList<Models.Entity.FileType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public FileType FindFileType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FileTypes.SingleOrDefault(field => field.FileTypeID == id);
			}
		}

		public bool FileTypeNameAvailable(string fileTypeFieldText, int fileTypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.FileTypes
						 where field.FileTypeName == fileTypeFieldText && field.FileTypeID != fileTypeFieldId
						 select field.FileTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteFileType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FileType fileType = context.FileTypes.SingleOrDefault(field => field.FileTypeID == id);
				if (fileType != null) {
					if (fileType.Files.Count == 0) {
						context.FileTypes.DeleteObject(fileType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveFileType(FileType fileType) {
			return fileType.Save();
		}

		public List<FileType> GetAllFileTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fileType in context.FileTypes
						orderby fileType.FileTypeName
						select fileType).ToList();
			}
		}

		#endregion

		#region File

		public IEnumerable<ErrorInfo> SaveFile(File file) {
			return file.Save();
		}

		public File FindFile(int fileId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Files.Where(file => file.FileID == fileId).SingleOrDefault();
			}
		}

		#endregion

		#region  EquityType

		public List<Models.Entity.EquityType> GetAllEquityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.EquityType> query = (from equityType in context.EquityTypes
															  select equityType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<EquityType> paginatedList = new PaginatedList<EquityType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<EquityType> GetAllEquityTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from equityType in context.EquityTypes
						where equityType.Enabled == true
						orderby equityType.Equity
						select equityType).ToList();
			}
		}

		public EquityType FindEquityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.EquityTypes.SingleOrDefault(type => type.EquityTypeID == id);
			}
		}

		public bool EquityTypeNameAvailable(string equityTypeName, int equityTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.EquityTypes
						 where type.Equity == equityTypeName && type.EquityTypeID != equityTypeID
						 select type.EquityTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteEquityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				EquityType equityType = context.EquityTypes.SingleOrDefault(type => type.EquityTypeID == id);
				if (equityType != null) {
					if (equityType.Equities.Count == 0) {
						context.EquityTypes.DeleteObject(equityType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveEquityType(EquityType equityType) {
			return equityType.Save();
		}

		#endregion

		#region  FixedIncomeType

		public List<Models.Entity.FixedIncomeType> GetAllFixedIncomeTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.FixedIncomeType> query = (from fixedIncomeType in context.FixedIncomeTypes
																   select fixedIncomeType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<FixedIncomeType> paginatedList = new PaginatedList<FixedIncomeType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<FixedIncomeType> GetAllFixedIncomeTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from fixedIncomeType in context.FixedIncomeTypes
						where fixedIncomeType.Enabled == true
						orderby fixedIncomeType.FixedIncomeType1
						select fixedIncomeType).ToList();
			}
		}

		public FixedIncomeType FindFixedIncomeType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FixedIncomeTypes.SingleOrDefault(type => type.FixedIncomeTypeID == id);
			}
		}

		public bool FixedIncomeTypeNameAvailable(string fixedIncomeTypeName, int fixedIncomeTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.FixedIncomeTypes
						 where type.FixedIncomeType1 == fixedIncomeTypeName && type.FixedIncomeTypeID != fixedIncomeTypeID
						 select type.FixedIncomeTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteFixedIncomeType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FixedIncomeType fixedIncomeType = context.FixedIncomeTypes.SingleOrDefault(type => type.FixedIncomeTypeID == id);
				if (fixedIncomeType != null) {
					if (fixedIncomeType.FixedIncomes.Count == 0) {
						context.FixedIncomeTypes.DeleteObject(fixedIncomeType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveFixedIncomeType(FixedIncomeType fixedIncomeType) {
			return fixedIncomeType.Save();
		}

		#endregion

		#region  Currency

		public List<Models.Entity.Currency> GetAllCurrencies(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Currency> query = (from currency in context.Currencies
															select currency);
				query = query.OrderBy("CurrencyID", (sortOrder == "asc"));
				PaginatedList<Models.Entity.Currency> paginatedList = new PaginatedList<Models.Entity.Currency>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Currency FindCurrency(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Currencies.SingleOrDefault(field => field.CurrencyID == id);
			}
		}

		public bool CurrencyNameAvailable(string currencyFieldText, int currencyFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.Currencies
						 where field.Currency1 == currencyFieldText && field.CurrencyID != currencyFieldId
						 select field.CurrencyID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCurrency(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Currency currency = context.Currencies.SingleOrDefault(field => field.CurrencyID == id);
				if (currency != null) {
					if (currency.Equities.Count == 0 && currency.FixedIncomes.Count == 0) {
						context.Currencies.DeleteObject(currency);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveCurrency(Currency currency) {
			return currency.Save();
		}

		public List<Currency> GetAllCurrencies() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from currency in context.Currencies
						where currency.Enabled == true
						orderby currency.Currency1
						select currency).ToList();
			}
		}
		#endregion

		#region  ShareClassType
		public List<ShareClassType> GetAllShareClassTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from shareClassType in context.ShareClassTypes
						where shareClassType.Enabled == true
						orderby shareClassType.ShareClass
						select shareClassType).ToList();
			}
		}
		#endregion

		#region  InvestmentType
		public List<InvestmentType> GetAllInvestmentTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investmentType in context.InvestmentTypes
						where investmentType.Enabled == true
						orderby investmentType.Investment
						select investmentType).ToList();
			}
		}
		#endregion

		#region  ActivityType

		public List<Models.Entity.ActivityType> GetAllActivityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.ActivityType> query = (from activityType in context.ActivityTypes
																select activityType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<ActivityType> paginatedList = new PaginatedList<ActivityType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<ActivityType> GetAllActivityTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from activityType in context.ActivityTypes
						where activityType.Enabled == true
						orderby activityType.Name
						select activityType).ToList();
			}
		}

		public ActivityType FindActivityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ActivityTypes.SingleOrDefault(type => type.ActivityTypeID == id);
			}
		}

		public bool ActivityTypeNameAvailable(string activityTypeName, int activityTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.ActivityTypes
						 where type.Name == activityTypeName && type.ActivityTypeID != activityTypeID
						 select type.ActivityTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteActivityType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ActivityType activityType = context.ActivityTypes.SingleOrDefault(type => type.ActivityTypeID == id);
				if (activityType != null) {
					if (activityType.FundActivityHistories.Count == 0) {
						context.ActivityTypes.DeleteObject(activityType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveActivityType(ActivityType activityType) {
			return activityType.Save();
		}

		#endregion

		#region Country
		public List<AutoCompleteList> FindCountrys(string countryName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> countryListQuery = (from country in context.COUNTRies
																 where country.CountryName.StartsWith(countryName)
																 orderby country.CountryName
																 select new AutoCompleteList {
																	 id = country.CountryID,
																	 label = country.CountryName,
																	 value = country.CountryName
																 });
				return new PaginatedList<AutoCompleteList>(countryListQuery, 1, 20);
			}
		}
		#endregion


		#region DynamicData
		public object FindTable(string tableName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				object results = null;
				//int pageIndex = 0;
				int pageSize = 10000;
				switch (tableName.ToLower()) {
					case "account":
						results = context.Accounts.OrderBy(account => account.AccountID).Take(pageSize).ToList();
						break;
					case "activitytype":
						results = context.ActivityTypes.OrderBy(activitytype => activitytype.ActivityTypeID).Take(pageSize).ToList();
						break;
					case "address":
						results = context.Addresses.OrderBy(address => address.AddressID).Take(pageSize).ToList();
						break;
					case "addresstype":
						results = context.AddressTypes.OrderBy(addresstype => addresstype.AddressTypeID).Take(pageSize).ToList();
						break;
					case "capitalcall":
						results = context.CapitalCalls.OrderBy(capitalcall => capitalcall.CapitalCallID).Take(pageSize).ToList();
						break;
					case "capitalcalllineitem":
						results = context.CapitalCallLineItems.OrderBy(capitalcalllineitem => capitalcalllineitem.CapitalCallLineItemID).Take(pageSize).ToList();
						break;
					case "capitalcalllineitemtype":
						results = context.CapitalCallLineItemTypes.OrderBy(capitalcalllineitemtype => capitalcalllineitemtype.CapitalCallLineItemTypeID).Take(pageSize).ToList();
						break;
					case "capitalcalltype":
						results = context.CapitalCallTypes.OrderBy(capitalcalltype => capitalcalltype.CapitalCallTypeID).Take(pageSize).ToList();
						break;
					case "capitaldistribution":
						results = context.CapitalDistributions.OrderBy(capitaldistribution => capitaldistribution.CapitalDistributionID).Take(pageSize).ToList();
						break;
					case "capitaldistributionlineitem":
						results = context.CapitalDistributionLineItems.OrderBy(capitaldistributionlineitem => capitaldistributionlineitem.CapitalDistributionLineItemID).Take(pageSize).ToList();
						break;
					case "capitaldistributionprofit":
						results = context.CapitalDistributionProfits.OrderBy(capitaldistributionprofit => capitaldistributionprofit.CapitalDistributionProfitID).Take(pageSize).ToList();
						break;
					case "cashdistribution":
						results = context.CashDistributions.OrderBy(cashdistribution => cashdistribution.CashDistributionID).Take(pageSize).ToList();
						break;
					case "cashdistributiontype":
						results = context.CashDistributionTypes.OrderBy(cashdistributiontype => cashdistributiontype.CashDistributionTypeID).Take(pageSize).ToList();
						break;
					case "communication":
						results = context.Communications.OrderBy(communication => communication.CommunicationID).Take(pageSize).ToList();
						break;
					case "communicationgrouping":
						results = context.CommunicationGroupings.OrderBy(communicationgrouping => communicationgrouping.CommunicationGroupingID).Take(pageSize).ToList();
						break;
					case "communicationtype":
						results = context.CommunicationTypes.OrderBy(communicationtype => communicationtype.CommunicationTypeID).Take(pageSize).ToList();
						break;
					case "contact":
						results = context.Contacts.OrderBy(contact => contact.ContactID).Take(pageSize).ToList();
						break;
					case "contactaddress":
						results = context.ContactAddresses.OrderBy(contactaddress => contactaddress.ContactAddressID).Take(pageSize).ToList();
						break;
					case "contactcommunication":
						results = context.ContactCommunications.OrderBy(contactcommunication => contactcommunication.ContactCommunicationID).Take(pageSize).ToList();
						break;
					case "country":
						results = context.COUNTRies.OrderBy(country => country.CountryID).Take(pageSize).ToList();
						break;
					case "currency":
						results = context.Currencies.OrderBy(currency => currency.CurrencyID).Take(pageSize).ToList();
						break;
					case "customfield":
						results = context.CustomFields.OrderBy(customfield => customfield.CustomFieldID).Take(pageSize).ToList();
						break;
					case "customfieldvalue":
						results = context.CustomFieldValues.OrderBy(customfieldvalue => customfieldvalue.CustomFieldValueID).Take(pageSize).ToList();
						break;
					case "datatype":
						results = context.DataTypes.OrderBy(datatype => datatype.DataTypeID).Take(pageSize).ToList();
						break;
					case "deal":
						results = context.Deals.OrderBy(deal => deal.DealID).Take(pageSize).ToList();
						break;
					case "dealclosing":
						results = context.DealClosings.OrderBy(dealclosing => dealclosing.DealClosingID).Take(pageSize).ToList();
						break;
					case "dealclosingcost":
						results = context.DealClosingCosts.OrderBy(dealclosingcost => dealclosingcost.DealClosingCostID).Take(pageSize).ToList();
						break;
					case "dealclosingcosttype":
						results = context.DealClosingCostTypes.OrderBy(dealclosingcosttype => dealclosingcosttype.DealClosingCostTypeID).Take(pageSize).ToList();
						break;
					case "dealunderlyingdirect":
						results = context.DealUnderlyingDirects.OrderBy(dealunderlyingdirect => dealunderlyingdirect.DealUnderlyingDirectID).Take(pageSize).ToList();
						break;
					case "dealunderlyingfund":
						results = context.DealUnderlyingFunds.OrderBy(dealunderlyingfund => dealunderlyingfund.DealUnderlyingtFundID).Take(pageSize).ToList();
						break;
					case "dealunderlyingfundadjustment":
						results = context.DealUnderlyingFundAdjustments.OrderBy(dealunderlyingfundadjustment => dealunderlyingfundadjustment.DealUnderlyingFundAdjustmentID).Take(pageSize).ToList();
						break;
					case "documenttype":
						results = context.DocumentTypes.OrderBy(documenttype => documenttype.DocumentTypeID).Take(pageSize).ToList();
						break;
					case "equity":
						results = context.Equities.OrderBy(equity => equity.EquityID).Take(pageSize).ToList();
						break;
					case "equitysplit":
						results = context.EquitySplits.OrderBy(equitysplit => equitysplit.EquiteSplitID).Take(pageSize).ToList();
						break;
					case "equitytype":
						results = context.EquityTypes.OrderBy(equitytype => equitytype.EquityTypeID).Take(pageSize).ToList();
						break;
					case "file":
						results = context.Files.OrderBy(file => file.FileID).Take(pageSize).ToList();
						break;
					case "filetype":
						results = context.FileTypes.OrderBy(filetype => filetype.FileTypeID).Take(pageSize).ToList();
						break;
					case "fixedincome":
						results = context.FixedIncomes.OrderBy(fixedincome => fixedincome.FixedIncomeID).Take(pageSize).ToList();
						break;
					case "fixedincometype":
						results = context.FixedIncomeTypes.OrderBy(fixedincometype => fixedincometype.FixedIncomeTypeID).Take(pageSize).ToList();
						break;
					case "fund":
						results = context.Funds.OrderBy(fund => fund.FundID).Take(pageSize).ToList();
						break;
					case "fundaccount":
						results = context.FundAccounts.OrderBy(fundaccount => fundaccount.FundAccountID).Take(pageSize).ToList();
						break;
					case "fundactivityhistory":
						results = context.FundActivityHistories.OrderBy(fundactivityhistory => fundactivityhistory.FundActivityHistoryID).Take(pageSize).ToList();
						break;
					case "fundclosing":
						results = context.FundClosings.OrderBy(fundclosing => fundclosing.FundClosingID).Take(pageSize).ToList();
						break;
					case "fundexpense":
						results = context.FundExpenses.OrderBy(fundexpense => fundexpense.FundExpenseID).Take(pageSize).ToList();
						break;
					case "fundexpensetype":
						results = context.FundExpenseTypes.OrderBy(fundexpensetype => fundexpensetype.FundExpenseTypeID).Take(pageSize).ToList();
						break;
					case "fundrateschedule":
						results = context.FundRateSchedules.OrderBy(fundrateschedule => fundrateschedule.FundRateScheduleID).Take(pageSize).ToList();
						break;
					case "geography":
						results = context.Geographies.OrderBy(geography => geography.GeographyID).Take(pageSize).ToList();
						break;
					case "industry":
						results = context.Industries.OrderBy(industry => industry.IndustryID).Take(pageSize).ToList();
						break;
					case "investmenttype":
						results = context.InvestmentTypes.OrderBy(investmenttype => investmenttype.InvestmentTypeID).Take(pageSize).ToList();
						break;
					case "investor":
						results = context.Investors.OrderBy(investor => investor.InvestorID).Take(pageSize).ToList();
						break;
					case "investoraccount":
						results = context.InvestorAccounts.OrderBy(investoraccount => investoraccount.InvestorAccountID).Take(pageSize).ToList();
						break;
					case "investoraddress":
						results = context.InvestorAddresses.OrderBy(investoraddress => investoraddress.InvestorAddressID).Take(pageSize).ToList();
						break;
					case "investorcommunication":
						results = context.InvestorCommunications.OrderBy(investorcommunication => investorcommunication.InvestorCommunicationID).Take(pageSize).ToList();
						break;
					case "investorcontact":
						results = context.InvestorContacts.OrderBy(investorcontact => investorcontact.InvestorContactID).Take(pageSize).ToList();
						break;
					case "investorentitytype":
						results = context.InvestorEntityTypes.OrderBy(investorentitytype => investorentitytype.InvestorEntityTypeID).Take(pageSize).ToList();
						break;
					case "investorfund":
						results = context.InvestorFunds.OrderBy(investorfund => investorfund.InvestorFundID).Take(pageSize).ToList();
						break;
					case "investorfunddocument":
						results = context.InvestorFundDocuments.OrderBy(investorfunddocument => investorfunddocument.InvestorFundDocumentID).Take(pageSize).ToList();
						break;
					case "investorfundtransaction":
						results = context.InvestorFundTransactions.OrderBy(investorfundtransaction => investorfundtransaction.InvestorFundTransactionID).Take(pageSize).ToList();
						break;
					case "investortype":
						results = context.InvestorTypes.OrderBy(investortype => investortype.InvestorTypeID).Take(pageSize).ToList();
						break;
					case "issuer":
						results = context.Issuers.OrderBy(issuer => issuer.IssuerID).Take(pageSize).ToList();
						break;
					case "managementfeerateschedule":
						results = context.ManagementFeeRateSchedules.OrderBy(managementfeerateschedule => managementfeerateschedule.ManagementFeeRateScheduleID).Take(pageSize).ToList();
						break;
					case "managementfeeratescheduletier":
						results = context.ManagementFeeRateScheduleTiers.OrderBy(managementfeeratescheduletier => managementfeeratescheduletier.ManagementFeeRateScheduleTierID).Take(pageSize).ToList();
						break;
					case "module":
						results = context.MODULEs.OrderBy(module => module.ModuleID).Take(pageSize).ToList();
						break;
					case "multipliertype":
						results = context.MultiplierTypes.OrderBy(multipliertype => multipliertype.MultiplierTypeID).Take(pageSize).ToList();
						break;
					case "optionfield":
						results = context.OptionFields.OrderBy(optionfield => optionfield.OptionFieldID).Take(pageSize).ToList();
						break;
					case "optionfieldvaluelist":
						results = context.OptionFieldValueLists.OrderBy(optionfieldvaluelist => optionfieldvaluelist.OptionFieldValueListID).Take(pageSize).ToList();
						break;
					case "partner":
						results = context.Partners.OrderBy(partner => partner.PartnerID).Take(pageSize).ToList();
						break;
					case "purchasetype":
						results = context.PurchaseTypes.OrderBy(purchasetype => purchasetype.PurchaseTypeID).Take(pageSize).ToList();
						break;
					case "ratescheduletype":
						results = context.RateScheduleTypes.OrderBy(ratescheduletype => ratescheduletype.RateScheduleTypeID).Take(pageSize).ToList();
						break;
					case "reportingfrequency":
						results = context.ReportingFrequencies.OrderBy(reportingfrequency => reportingfrequency.ReportingFrequencyID).Take(pageSize).ToList();
						break;
					case "reportingtype":
						results = context.ReportingTypes.OrderBy(reportingtype => reportingtype.ReportingTypeID).Take(pageSize).ToList();
						break;
					case "securityconversion":
						results = context.SecurityConversions.OrderBy(securityconversion => securityconversion.SecurityConversionID).Take(pageSize).ToList();
						break;
					case "securityconversiondetail":
						results = context.SecurityConversionDetails.OrderBy(securityconversiondetail => securityconversiondetail.SecurityConversionDetailID).Take(pageSize).ToList();
						break;
					case "securitytype":
						results = context.SecurityTypes.OrderBy(securitytype => securitytype.SecurityTypeID).Take(pageSize).ToList();
						break;
					case "shareclasstype":
						results = context.ShareClassTypes.OrderBy(shareclasstype => shareclasstype.ShareClassTypeID).Take(pageSize).ToList();
						break;
					case "state":
						results = context.STATEs.OrderBy(state => state.StateID).Take(pageSize).ToList();
						break;
					case "transactiontype":
						results = context.TransactionTypes.OrderBy(transactiontype => transactiontype.TransactionTypeID).Take(pageSize).ToList();
						break;
					case "underlyingdirectdocument":
						results = context.UnderlyingDirectDocuments.OrderBy(underlyingdirectdocument => underlyingdirectdocument.UnderlyingDirectDocumentID).Take(pageSize).ToList();
						break;
					case "underlyingdirectlastprice":
						results = context.UnderlyingDirectLastPrices.OrderBy(underlyingdirectlastprice => underlyingdirectlastprice.UnderlyingDirectLastPriceID).Take(pageSize).ToList();
						break;
					case "underlyingdirectlastpricehistory":
						results = context.UnderlyingDirectLastPriceHistories.OrderBy(underlyingdirectlastpricehistory => underlyingdirectlastpricehistory.UnderlyingDirectLastPriceHistoryID).Take(pageSize).ToList();
						break;
					case "underlyingfund":
						results = context.UnderlyingFunds.OrderBy(underlyingfund => underlyingfund.UnderlyingtFundID).Take(pageSize).ToList();
						break;
					case "underlyingfundcapitalcall":
						results = context.UnderlyingFundCapitalCalls.OrderBy(underlyingfundcapitalcall => underlyingfundcapitalcall.UnderlyingFundCapitalCallID).Take(pageSize).ToList();
						break;
					case "underlyingfundcapitalcalllineitem":
						results = context.UnderlyingFundCapitalCallLineItems.OrderBy(underlyingfundcapitalcalllineitem => underlyingfundcapitalcalllineitem.UnderlyingFundCapitalCallLineItemID).Take(pageSize).ToList();
						break;
					case "underlyingfundcashdistribution":
						results = context.UnderlyingFundCashDistributions.OrderBy(underlyingfundcashdistribution => underlyingfundcashdistribution.UnderlyingFundCashDistributionID).Take(pageSize).ToList();
						break;
					case "underlyingfunddocument":
						results = context.UnderlyingFundDocuments.OrderBy(underlyingfunddocument => underlyingfunddocument.UnderlyingFundDocumentID).Take(pageSize).ToList();
						break;
					case "underlyingfundnav":
						results = context.UnderlyingFundNAVs.OrderBy(underlyingfundnav => underlyingfundnav.UnderlyingFundNAVID).Take(pageSize).ToList();
						break;
					case "underlyingfundnavhistory":
						results = context.UnderlyingFundNAVHistories.OrderBy(underlyingfundnavhistory => underlyingfundnavhistory.UnderlyingFundNAVHistoryID).Take(pageSize).ToList();
						break;
					case "underlyingfundstockdistribution":
						results = context.UnderlyingFundStockDistributions.OrderBy(underlyingfundstockdistribution => underlyingfundstockdistribution.UnderlyingFundStockDistributionID).Take(pageSize).ToList();
						break;
					case "underlyingfundstockdistributionlineitem":
						results = context.UnderlyingFundStockDistributionLineItems.OrderBy(underlyingfundstockdistributionlineitem => underlyingfundstockdistributionlineitem.UnderlyingFundStockDistributionLineItemID).Take(pageSize).ToList();
						break;
					case "underlyingfundtype":
						results = context.UnderlyingFundTypes.OrderBy(underlyingfundtype => underlyingfundtype.UnderlyingFundTypeID).Take(pageSize).ToList();
						break;
				}
				return results;
			}
		}
		#endregion
	}
}