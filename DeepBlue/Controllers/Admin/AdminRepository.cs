using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using System.Reflection;
using System.Linq.Expressions;
using DeepBlue.Models.Deal;
using DeepBlue.Models.Admin;
using System.Text;

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

		public List<ShareClassType> GetAllShareClassTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from shareClassType in context.ShareClassTypes
						where shareClassType.Enabled == true
						orderby shareClassType.ShareClass
						select shareClassType).ToList();
			}
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
				return context.FundClosings.Include("Fund").SingleOrDefault(fundClose => fundClose.FundClosingID == id);
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

		#region  Communication
		public string GetContactCommunicationValue(int contactId, Models.Admin.Enums.CommunicationType communicationType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from contactCommunication in context.ContactCommunications
						where contactCommunication.ContactID == contactId && contactCommunication.Communication.CommunicationTypeID == (int)communicationType
						select contactCommunication.Communication.CommunicationValue).FirstOrDefault();
			}
		}

		private List<CommunicationDetailModel> GetContactCommunications(DeepBlueEntities context, int contactId) {
			return (from contactCommunication in context.ContactCommunications
					join communication in context.Communications on contactCommunication.CommunicationID equals communication.CommunicationID
					where contactCommunication.ContactID == contactId
					select new CommunicationDetailModel {
						CommunicationValue = communication.CommunicationValue,
						CommunicationTypeId = communication.CommunicationTypeID
					}).ToList();
		}

		public List<CommunicationDetailModel> GetContactCommunications(int? contactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from contactCommunication in context.ContactCommunications
						join communication in context.Communications on contactCommunication.CommunicationID equals communication.CommunicationID
						where contactCommunication.ContactID == contactId
						select new CommunicationDetailModel {
							CommunicationValue = communication.CommunicationValue,
							CommunicationTypeId = communication.CommunicationTypeID
						}).ToList();
			}
		}

		public string GetCommunicationValue(List<CommunicationDetailModel> communications, Models.Admin.Enums.CommunicationType communicationType) {
			return (from communication in communications
					where communication.CommunicationTypeId == (int)communicationType
					select communication.CommunicationValue).SingleOrDefault();
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
				return new PaginatedList<AutoCompleteList>(industryListQuery, 1, AutoCompleteOptions.RowsLength);
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
				return new PaginatedList<AutoCompleteList>(countryListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}
		#endregion

		#region State
		public List<AutoCompleteList> FindStates(string stateName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> stateListQuery = (from state in context.STATEs
															   where state.Name.StartsWith(stateName)
															   orderby state.Name
															   select new AutoCompleteList {
																   id = state.StateID,
																   label = state.Name,
																   value = state.Name
															   }).OrderBy(list => list.label);
				return new PaginatedList<AutoCompleteList>(stateListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}
		#endregion

		#region ExportExcel

		public List<string> GetAllTables(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, string tableName) {
			List<string> tables = new List<string> {
					"Accounts",
					"ActivityTypes",
					"Addresses",
					"AddressTypes",
					"AnnualMeetingHistories",
					"CapitalCallLineItems",
					"CapitalCallLineItemTypes",
					"CapitalCalls",
					"CapitalCallTypes",
					"CapitalDistributionLineItems",
					"CapitalDistributionProfits",
					"CapitalDistributions",
					"CashDistributions",
					"CashDistributionTypes",
					"CommunicationGroupings",
					"Communications",
					"CommunicationTypes",
					"ContactAddresses",
					"ContactCommunications",
					"Contacts",
					"COUNTRies",
					"Currencies",
					"CustomFields",
					"CustomFieldValues",
					"DataTypes",
					"DealClosingCosts",
					"DealClosingCostTypes",
					"DealClosings",
					"DealFundDocuments",
					"Deals",
					"DealUnderlyingDirects",
					"DealUnderlyingFundAdjustments",
					"DealUnderlyingFunds",
					"DocumentSections",
					"DocumentTypes",
					"ENTITies",
					"Equities",
					"EquitySplits",
					"EquityTypes",
					"Files",
					"FileTypes",
					"FixedIncomes",
					"FixedIncomeTypes",
					"FundAccounts",
					"FundActivityHistories",
					"FundClosings",
					"FundExpenses",
					"FundExpenseTypes",
					"FundRateSchedules",
					"Funds",
					"Geographies",
					"Industries",
					"InvestmentTypes",
					"InvestorAccounts",
					"InvestorAddresses",
					"InvestorCommunications",
					"InvestorContacts",
					"InvestorEntityTypes",
					"InvestorFundDocuments",
					"InvestorFunds",
					"InvestorFundTransactions",
					"Investors",
					"InvestorTypes",
					"Issuers",
					"LogDetails",
					"Logs",
					"LogTypes",
					"ManagementFeeRateSchedules",
					"ManagementFeeRateScheduleTiers",
					"MODULEs",
					"MultiplierTypes",
					"OptionFields",
					"OptionFieldValueLists",
					"Partners",
					"PurchaseTypes",
					"RateScheduleTypes",
					"ReportingFrequencies",
					"ReportingTypes",
					"SecurityConversionDetails",
					"SecurityConversions",
					"SecurityTypes",
					"SellerTypes",
					"ShareClassTypes",
					"STATEs",
					"TransactionTypes",
					"UnderlyingDirectDocuments",
					"UnderlyingDirectLastPriceHistories",
					"UnderlyingDirectLastPrices",
					"UnderlyingFundCapitalCallLineItems",
					"UnderlyingFundCapitalCalls",
					"UnderlyingFundCashDistributions",
					"UnderlyingFundContacts",
					"UnderlyingFundDocuments",
					"UnderlyingFundNAVHistories",
					"UnderlyingFundNAVs",
					"UnderlyingFunds",
					"UnderlyingFundStockDistributionLineItems",
					"UnderlyingFundStockDistributions",
					"UnderlyingFundTypes",
					"USERs",
			};
			var query = (from table in tables
						 where table.StartsWith(tableName)
						 select table);
			if (sortOrder == "asc") {
				query = query.OrderBy(q => q[0]);
			}
			else {
				query = query.OrderByDescending(q => q[0]);
			}
			totalRows = tables.Count();
			query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
			return query.ToList();
		}

		public object FindTable(string tableName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				object results = null;
				switch (tableName.ToLower()) {
					case "account":
						results = context.Accounts.ToList();
						break;
					case "activitytype":
						results = context.ActivityTypes.ToList();
						break;
					case "address":
						results = context.Addresses.ToList();
						break;
					case "addresstype":
						results = context.AddressTypes.ToList();
						break;
					case "annualmeetinghistory":
						results = context.AnnualMeetingHistories.ToList();
						break;
					case "capitalcall":
						results = context.CapitalCalls.ToList();
						break;
					case "capitalcalllineitem":
						results = context.CapitalCallLineItems.ToList();
						break;
					case "capitalcalllineitemtype":
						results = context.CapitalCallLineItemTypes.ToList();
						break;
					case "capitalcalltype":
						results = context.CapitalCallTypes.ToList();
						break;
					case "capitaldistribution":
						results = context.CapitalDistributions.ToList();
						break;
					case "capitaldistributionlineitem":
						results = context.CapitalDistributionLineItems.ToList();
						break;
					case "capitaldistributionprofit":
						results = context.CapitalDistributionProfits.ToList();
						break;
					case "cashdistribution":
						results = context.CashDistributions.ToList();
						break;
					case "cashdistributiontype":
						results = context.CashDistributionTypes.ToList();
						break;
					case "communication":
						results = context.Communications.ToList();
						break;
					case "communicationgrouping":
						results = context.CommunicationGroupings.ToList();
						break;
					case "communicationtype":
						results = context.CommunicationTypes.ToList();
						break;
					case "contact":
						results = context.Contacts.ToList();
						break;
					case "contactaddress":
						results = context.ContactAddresses.ToList();
						break;
					case "contactcommunication":
						results = context.ContactCommunications.ToList();
						break;
					case "country":
						results = context.COUNTRies.ToList();
						break;
					case "currency":
						results = context.Currencies.ToList();
						break;
					case "customfield":
						results = context.CustomFields.ToList();
						break;
					case "customfieldvalue":
						results = context.CustomFieldValues.ToList();
						break;
					case "datatype":
						results = context.DataTypes.ToList();
						break;
					case "deal":
						results = context.Deals.ToList();
						break;
					case "dealclosing":
						results = context.DealClosings.ToList();
						break;
					case "dealclosingcost":
						results = context.DealClosingCosts.ToList();
						break;
					case "dealclosingcosttype":
						results = context.DealClosingCostTypes.ToList();
						break;
					case "dealfunddocument":
						results = context.DealFundDocuments.ToList();
						break;
					case "dealunderlyingdirect":
						results = context.DealUnderlyingDirects.ToList();
						break;
					case "dealunderlyingfund":
						results = context.DealUnderlyingFunds.ToList();
						break;
					case "dealunderlyingfundadjustment":
						results = context.DealUnderlyingFundAdjustments.ToList();
						break;
					case "documentsection":
						results = context.DocumentSections.ToList();
						break;
					case "documenttype":
						results = context.DocumentTypes.ToList();
						break;
					case "entity":
						results = context.ENTITies.ToList();
						break;
					case "equity":
						results = context.Equities.ToList();
						break;
					case "equitysplit":
						results = context.EquitySplits.ToList();
						break;
					case "equitytype":
						results = context.EquityTypes.ToList();
						break;
					case "file":
						results = context.Files.ToList();
						break;
					case "filetype":
						results = context.FileTypes.ToList();
						break;
					case "fixedincome":
						results = context.FixedIncomes.ToList();
						break;
					case "fixedincometype":
						results = context.FixedIncomeTypes.ToList();
						break;
					case "fund":
						results = context.Funds.ToList();
						break;
					case "fundaccount":
						results = context.FundAccounts.ToList();
						break;
					case "fundactivityhistory":
						results = context.FundActivityHistories.ToList();
						break;
					case "fundclosing":
						results = context.FundClosings.ToList();
						break;
					case "fundexpense":
						results = context.FundExpenses.ToList();
						break;
					case "fundexpensetype":
						results = context.FundExpenseTypes.ToList();
						break;
					case "fundrateschedule":
						results = context.FundRateSchedules.ToList();
						break;
					case "geography":
						results = context.Geographies.ToList();
						break;
					case "industry":
						results = context.Industries.ToList();
						break;
					case "investmenttype":
						results = context.InvestmentTypes.ToList();
						break;
					case "investor":
						results = context.Investors.ToList();
						break;
					case "investoraccount":
						results = context.InvestorAccounts.ToList();
						break;
					case "investoraddress":
						results = context.InvestorAddresses.ToList();
						break;
					case "investorcommunication":
						results = context.InvestorCommunications.ToList();
						break;
					case "investorcontact":
						results = context.InvestorContacts.ToList();
						break;
					case "investorentitytype":
						results = context.InvestorEntityTypes.ToList();
						break;
					case "investorfund":
						results = context.InvestorFunds.ToList();
						break;
					case "investorfunddocument":
						results = context.InvestorFundDocuments.ToList();
						break;
					case "investorfundtransaction":
						results = context.InvestorFundTransactions.ToList();
						break;
					case "investortype":
						results = context.InvestorTypes.ToList();
						break;
					case "issuer":
						results = context.Issuers.ToList();
						break;
					case "log":
						results = context.Logs.ToList();
						break;
					case "logdetail":
						results = context.LogDetails.ToList();
						break;
					case "logtype":
						results = context.LogTypes.ToList();
						break;
					case "managementfeerateschedule":
						results = context.ManagementFeeRateSchedules.ToList();
						break;
					case "managementfeeratescheduletier":
						results = context.ManagementFeeRateScheduleTiers.ToList();
						break;
					case "module":
						results = context.MODULEs.ToList();
						break;
					case "multipliertype":
						results = context.MultiplierTypes.ToList();
						break;
					case "optionfield":
						results = context.OptionFields.ToList();
						break;
					case "optionfieldvaluelist":
						results = context.OptionFieldValueLists.ToList();
						break;
					case "partner":
						results = context.Partners.ToList();
						break;
					case "purchasetype":
						results = context.PurchaseTypes.ToList();
						break;
					case "ratescheduletype":
						results = context.RateScheduleTypes.ToList();
						break;
					case "reportingfrequency":
						results = context.ReportingFrequencies.ToList();
						break;
					case "reportingtype":
						results = context.ReportingTypes.ToList();
						break;
					case "securityconversion":
						results = context.SecurityConversions.ToList();
						break;
					case "securityconversiondetail":
						results = context.SecurityConversionDetails.ToList();
						break;
					case "securitytype":
						results = context.SecurityTypes.ToList();
						break;
					case "sellertype":
						results = context.SellerTypes.ToList();
						break;
					case "shareclasstype":
						results = context.ShareClassTypes.ToList();
						break;
					case "state":
						results = context.STATEs.ToList();
						break;
					case "transactiontype":
						results = context.TransactionTypes.ToList();
						break;
					case "underlyingdirectdocument":
						results = context.UnderlyingDirectDocuments.ToList();
						break;
					case "underlyingdirectlastprice":
						results = context.UnderlyingDirectLastPrices.ToList();
						break;
					case "underlyingdirectlastpricehistory":
						results = context.UnderlyingDirectLastPriceHistories.ToList();
						break;
					case "underlyingfund":
						results = context.UnderlyingFunds.ToList();
						break;
					case "underlyingfundcapitalcall":
						results = context.UnderlyingFundCapitalCalls.ToList();
						break;
					case "underlyingfundcapitalcalllineitem":
						results = context.UnderlyingFundCapitalCallLineItems.ToList();
						break;
					case "underlyingfundcashdistribution":
						results = context.UnderlyingFundCashDistributions.ToList();
						break;
					case "underlyingfundcontact":
						results = context.UnderlyingFundContacts.ToList();
						break;
					case "underlyingfunddocument":
						results = context.UnderlyingFundDocuments.ToList();
						break;
					case "underlyingfundnav":
						results = context.UnderlyingFundNAVs.ToList();
						break;
					case "underlyingfundnavhistory":
						results = context.UnderlyingFundNAVHistories.ToList();
						break;
					case "underlyingfundstockdistribution":
						results = context.UnderlyingFundStockDistributions.ToList();
						break;
					case "underlyingfundstockdistributionlineitem":
						results = context.UnderlyingFundStockDistributionLineItems.ToList();
						break;
					case "underlyingfundtype":
						results = context.UnderlyingFundTypes.ToList();
						break;
					case "user":
						results = context.USERs.ToList();
						break;
				}
				return results;
			}
		}

		public InvestorExportExcelModel GetAllInvestorExportList() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorExportExcelModel model = new InvestorExportExcelModel();
				model.Investors = (from investor in context.Investors
								   select new {
									   InvestorName = investor.InvestorName,
									   DisplayName = investor.Alias,
									   Social = investor.Social,
									   InvestorEntityType = (investor.InvestorEntityType != null ? investor.InvestorEntityType.InvestorEntityTypeName : string.Empty),
									   IsDomestic = investor.IsDomestic,
									   StateOfResidency = (investor.STATE != null ? investor.STATE.Name : string.Empty),
								   }).ToList();
				model.InvestorAddresses = (from investorAddress in context.InvestorAddresses
										   select new {
											   InvestorName = investorAddress.Investor.InvestorName,
											   Address1 = investorAddress.Address.Address1,
											   Address2 = investorAddress.Address.Address2,
											   City = investorAddress.Address.City,
											   Country = investorAddress.Address.Country,
											   Zip = investorAddress.Address.PostalCode,
											   State = investorAddress.Address.State,
											   Phone = (from investorCommunication in investorAddress.Investor.InvestorCommunications
														where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.HomePhone
														select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											   Email = (from investorCommunication in investorAddress.Investor.InvestorCommunications
														where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Email
														select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											   WebAddress = (from investorCommunication in investorAddress.Investor.InvestorCommunications
															 where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.WebAddress
															 select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											   Fax = (from investorCommunication in investorAddress.Investor.InvestorCommunications
													  where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Fax
													  select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
										   }).ToList();

				model.InvestorBanks = (from investorAccount in context.InvestorAccounts
									   select new {
										   InvestorName = investorAccount.Investor.InvestorName,
										   BankName = investorAccount.BankName,
										   ABANumber = investorAccount.Routing,
										   AccountName = investorAccount.Account,
										   AccountNumber = investorAccount.AccountNumberCash,
										   FFCName = investorAccount.FFC,
										   FFCNumber = investorAccount.FFCNumber,
										   Reference = investorAccount.Reference,
										   Swift = investorAccount.SWIFT,
										   IBAN = investorAccount.IBAN,
										   Phone = investorAccount.Phone,
										   Fax = investorAccount.Fax
									   }).ToList();

				model.InvestorContacts = (from investorContact in context.InvestorContacts
										  select new {
											  InvestorName = investorContact.Investor.InvestorName,
											  ContactPerson = investorContact.Contact.ContactName,
											  Designation = investorContact.Contact.Designation,
											  Phone = (from investorCommunication in investorContact.Contact.ContactCommunications
													   where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.HomePhone
													   select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											  Fax = (from investorCommunication in investorContact.Contact.ContactCommunications
													 where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Fax
													 select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											  Email = (from investorCommunication in investorContact.Contact.ContactCommunications
													   where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Email
													   select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											  WebAddress = (from investorCommunication in investorContact.Contact.ContactCommunications
															where investorCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.WebAddress
															select investorCommunication.Communication.CommunicationValue).FirstOrDefault(),
											  Address1 = (from investorAddress in investorContact.Contact.ContactAddresses
														  select investorAddress.Address.Address1).FirstOrDefault(),
											  City = (from investorAddress in investorContact.Contact.ContactAddresses
													  select investorAddress.Address.City).FirstOrDefault(),
											  State = (from investorAddress in investorContact.Contact.ContactAddresses
													   select (investorAddress.Address.STATE1 != null ? investorAddress.Address.STATE1.Name : string.Empty)
													  ).FirstOrDefault(),
											  Zip = (from investorAddress in investorContact.Contact.ContactAddresses
													 select investorAddress.Address.PostalCode
													 ).FirstOrDefault(),
											  Country = (from investorAddress in investorContact.Contact.ContactAddresses
														 select (investorAddress.Address.COUNTRY1 != null ? investorAddress.Address.COUNTRY1.CountryName : string.Empty)
											  ).FirstOrDefault(),
											  ReceivesDistributionNotices = investorContact.Contact.ReceivesDistributionNotices,
											  Financials = investorContact.Contact.ReceivesFinancials,
											  K1 = investorContact.Contact.ReceivesK1,
											  InvestorLetters = investorContact.Contact.ReceivesInvestorLetters
										  }).ToList();

				model.InvestorInvestments = (from investment in context.InvestorFunds
											 select new {
												 InvestorName = investment.Investor.InvestorName,
												 FundName = investment.Fund.FundName,
												 InvestorType = (investment.InvestorType != null ? investment.InvestorType.InvestorTypeName : string.Empty),
												 TotalCommitment = investment.TotalCommitment,
												 UnfundedAmount = (investment.UnfundedAmount ?? 0),
												 FundClose = investment.InvestorFundTransactions
												 .Where(transaction => transaction.FundClosingID > 0).FirstOrDefault().FundClosing.Name,
												 CloseDate = investment.InvestorFundTransactions
												 .Where(transaction => transaction.FundClosingID > 0).FirstOrDefault().FundClosing.FundClosingDate
											 }).ToList();
				return model;
			}
		}

		public FundExportExcelModel GetAllFundExportList() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundExportExcelModel model = new FundExportExcelModel();
				model.AmberbrookFunds = (from fund in context.Funds
										 select new {
											 FundName = fund.FundName,
											 TaxID = fund.TaxID,
											 FundStartDate = fund.InceptionDate,
											 ScheduleTerminationDate = fund.ScheduleTerminationDate,
											 FinalTerminationDate = fund.FinalTerminationDate,
											 AutomaticExtensions = fund.NumofAutoExtensions,
											 DateClawbackTriggered = fund.DateClawbackTriggered,
											 RecycleProvision = fund.RecycleProvision,
											 MgmtFeesCatchupDate = fund.MgmtFeesCatchUpDate,
											 Carry = fund.Carry,
										 }).ToList();

				model.Investors = (from investor in context.InvestorFunds
								   select new {
									   FundName = investor.Fund.FundName,
									   InvestorName = investor.Investor.InvestorName,
									   TotalCommitment = investor.TotalCommitment,
									   UnfundedAmount = (investor.UnfundedAmount ?? 0),
									   FundClose = investor.InvestorFundTransactions
									   .Where(transaction => transaction.FundClosingID > 0).FirstOrDefault().FundClosing.Name,
									   CloseDate = investor.InvestorFundTransactions
									   .Where(transaction => transaction.FundClosingID > 0).FirstOrDefault().FundClosing.FundClosingDate
								   }).ToList();

				model.RateSchdules = (from rateSchedule in context.FundRateSchedules
									  join managementFeeRateSchedule in context.ManagementFeeRateSchedules on rateSchedule.RateScheduleID equals managementFeeRateSchedule.ManagementFeeRateScheduleID
									  join managementFeeRateScheduleTier in context.ManagementFeeRateScheduleTiers on managementFeeRateSchedule.ManagementFeeRateScheduleID equals managementFeeRateScheduleTier.ManagementFeeRateScheduleID
									  select new {
										  FundName = rateSchedule.Fund.FundName,
										  StartDate = managementFeeRateScheduleTier.StartDate,
										  EndDate = managementFeeRateScheduleTier.EndDate,
										  FeeCalculationType = (managementFeeRateScheduleTier.MultiplierType != null ? managementFeeRateScheduleTier.MultiplierType.Name : string.Empty),
										  FlatFee = (managementFeeRateScheduleTier.MultiplierTypeID == (int)DeepBlue.Models.Fund.Enums.MutiplierType.FlatFee ? managementFeeRateScheduleTier.Multiplier : 0),
										  Rate = (managementFeeRateScheduleTier.MultiplierTypeID == (int)DeepBlue.Models.Fund.Enums.MutiplierType.CapitalCommitted ? managementFeeRateScheduleTier.Multiplier : 0),
										  Comments = managementFeeRateScheduleTier.Notes
									  }).ToList();

				model.BankInformations = (from fundAccount in context.FundAccounts
										  select new {
											  FundName = fundAccount.Fund.FundName,
											  BankName = fundAccount.BankName,
											  ABANumber = fundAccount.Routing,
											  AccountName = fundAccount.Account,
											  AccountNumber = fundAccount.AccountNumberCash,
											  FFCName = fundAccount.FFC,
											  FFCNumber = fundAccount.FFCNumber,
											  Reference = fundAccount.Reference,
											  Swift = fundAccount.SWIFT,
											  IBAN = fundAccount.IBAN,
											  Phone = fundAccount.Phone,
											  Fax = fundAccount.Fax
										  }).ToList();
				return model;
			}
		}

		public UnderlyingFundExportExcelModel GetAllUnderlyingFundExportList() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundExportExcelModel model = new UnderlyingFundExportExcelModel();

				model.UnderlyingFunds = (from underlyingFund in context.UnderlyingFunds
										 select new {
											 UnderlyingFundName = underlyingFund.FundName,
											 GP = underlyingFund.Issuer.Name,
											 FundType = (underlyingFund.UnderlyingFundType != null ? underlyingFund.UnderlyingFundType.Name : string.Empty),
											 VintageYear = underlyingFund.VintageYear,
											 FundSize = underlyingFund.TotalSize,
											 TerminationYear = underlyingFund.TerminationYear,
											 Reporting = (underlyingFund.ReportingFrequency != null ? underlyingFund.ReportingFrequency.ReportingFrequency1 : string.Empty),
											 ReportingType = (underlyingFund.ReportingType != null ? underlyingFund.ReportingType.Reporting : string.Empty),
											 FeesIncluded = underlyingFund.IsFeesIncluded,
											 Industry = (underlyingFund.Industry != null ? underlyingFund.Industry.Industry1 : string.Empty),
											 Geography = (underlyingFund.Geography != null ? underlyingFund.Geography.Geography1 : string.Empty),
											 Website = underlyingFund.Website,
											 WebUserName = underlyingFund.WebUserName,
											 WebPassword = underlyingFund.WebPassword,
											 Description = underlyingFund.Description,
											 Address1 = (underlyingFund.Address != null ? underlyingFund.Address.Address1 : string.Empty),
											 Address2 = (underlyingFund.Address != null ? underlyingFund.Address.Address2 : string.Empty),
											 City = (underlyingFund.Address != null ? underlyingFund.Address.City : string.Empty),
											 State = (underlyingFund.Address != null ? (underlyingFund.Address.STATE1 != null ? underlyingFund.Address.STATE1.Name : string.Empty) : string.Empty),
											 Zip = (underlyingFund.Address != null ? underlyingFund.Address.PostalCode : string.Empty),
											 Country = (underlyingFund.Address != null ? (underlyingFund.Address.COUNTRY1 != null ? underlyingFund.Address.COUNTRY1.CountryName : string.Empty) : string.Empty),
											 BankName = (underlyingFund.Account != null ? underlyingFund.Account.BankName : string.Empty),
											 ABANumber = (underlyingFund.Account != null ? underlyingFund.Account.Routing : null),
											 AccountName = (underlyingFund.Account != null ? underlyingFund.Account.Account1 : string.Empty),
											 AccountNumber = (underlyingFund.Account != null ? underlyingFund.Account.AccountNumberCash : string.Empty),
											 FFCName = (underlyingFund.Account != null ? underlyingFund.Account.FFC : string.Empty),
											 FFCNumber = (underlyingFund.Account != null ? underlyingFund.Account.FFCNumber : string.Empty),
											 Reference = (underlyingFund.Account != null ? underlyingFund.Account.Reference : string.Empty),
											 Swift = (underlyingFund.Account != null ? underlyingFund.Account.SWIFT : string.Empty),
											 IBAN = (underlyingFund.Account != null ? underlyingFund.Account.IBAN : string.Empty),
											 Phone = (underlyingFund.Account != null ? underlyingFund.Account.Phone : string.Empty),
											 Fax = (underlyingFund.Account != null ? underlyingFund.Account.Fax : string.Empty)
										 }).ToList();

				model.UnderlyingFundContacts = (from underlyingFundContact in context.UnderlyingFundContacts
												select new {
													UnderlyingFundName = underlyingFundContact.UnderlyingFund.FundName,
													ContactName = underlyingFundContact.Contact.ContactName,
													Title = underlyingFundContact.Contact.Title,
													PhoneNumber = (from underlyingFundCommunication in underlyingFundContact.Contact.ContactCommunications
																   where underlyingFundCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.HomePhone
																   select underlyingFundCommunication.Communication.CommunicationValue).FirstOrDefault(),
													Email = (from underlyingFundCommunication in underlyingFundContact.Contact.ContactCommunications
															 where underlyingFundCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Email
															 select underlyingFundCommunication.Communication.CommunicationValue).FirstOrDefault(),
													Notes = underlyingFundContact.Contact.Notes,
													Fax = (from underlyingFundCommunication in underlyingFundContact.Contact.ContactCommunications
														   where underlyingFundCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.Fax
														   select underlyingFundCommunication.Communication.CommunicationValue).FirstOrDefault(),
													WebAddress = (from underlyingFundCommunication in underlyingFundContact.Contact.ContactCommunications
																  where underlyingFundCommunication.Communication.CommunicationTypeID == (int)DeepBlue.Models.Admin.Enums.CommunicationType.WebAddress
																  select underlyingFundCommunication.Communication.CommunicationValue).FirstOrDefault(),
												}).ToList();

				model.UnderlyingFundCapitalCalls = (from underlyingFundCapitalCall in context.UnderlyingFundCapitalCalls
													select new {
														UnderlyingFundName = (underlyingFundCapitalCall.UnderlyingFund != null ?
															underlyingFundCapitalCall.UnderlyingFund.FundName : string.Empty),
														AmberbrookFundName = (underlyingFundCapitalCall.UnderlyingFund != null ?
														underlyingFundCapitalCall.Fund.FundName : string.Empty),
														underlyingFundCapitalCall.Amount,
														underlyingFundCapitalCall.NoticeDate,
														underlyingFundCapitalCall.PaidON,
														underlyingFundCapitalCall.ReceivedDate,
														DeemedCapitalCall = underlyingFundCapitalCall.IsDeemedCapitalCall,
														PostRecordDateTransaction = underlyingFundCapitalCall.IsPostRecordDateTransaction,
														Reconciled = underlyingFundCapitalCall.IsReconciled,
														underlyingFundCapitalCall.ReconciliationMethod,
														underlyingFundCapitalCall.ChequeNumber,
													}).ToList();
				model.UnderlyingFundCapitalCallLineItems = (from underlyingFundCapitalCallLineItem in context.UnderlyingFundCapitalCallLineItems
															select new {
																UnderlyingFundName = (underlyingFundCapitalCallLineItem.UnderlyingFund != null ?
																					  underlyingFundCapitalCallLineItem.UnderlyingFund.FundName : string.Empty),
																AmberbrookFundName = (underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCall != null ?
																					 underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCall.Fund.FundName : string.Empty),
																DealName = (underlyingFundCapitalCallLineItem.Deal != null ?
																			underlyingFundCapitalCallLineItem.Deal.DealName : string.Empty),
																underlyingFundCapitalCallLineItem.Amount,
																underlyingFundCapitalCallLineItem.CapitalCallDate,
																underlyingFundCapitalCallLineItem.ReceivedDate,
															}).ToList();
				model.UnderlyingFundCashDistributions = (from underlyingFundCashDistribution in context.UnderlyingFundCashDistributions
														 select new {
															 UnderlyingFundName = (underlyingFundCashDistribution.UnderlyingFund != null ?
																					underlyingFundCashDistribution.UnderlyingFund.FundName : string.Empty),
															 AmberbrookFundName = (underlyingFundCashDistribution.UnderlyingFund != null ?
																					underlyingFundCashDistribution.Fund.FundName : string.Empty),
															 underlyingFundCashDistribution.Amount,
															 CashDistributionType = (underlyingFundCashDistribution.CashDistributionType != null ?
																					  underlyingFundCashDistribution.CashDistributionType.Name : string.Empty),
															 underlyingFundCashDistribution.NoticeDate,
															 underlyingFundCashDistribution.PaidDate,
															 underlyingFundCashDistribution.PaidON,
															 underlyingFundCashDistribution.ReceivedDate,
															 PostRecordDateTransaction = underlyingFundCashDistribution.IsPostRecordDateTransaction,
															 Reconciled = underlyingFundCashDistribution.IsReconciled,
															 underlyingFundCashDistribution.ReconciliationMethod,
															 underlyingFundCashDistribution.ChequeNumber,
														 }).ToList();
				model.UnderlyingFundCashDistributionLineItems = (from underlyingFundCashDistributionLineItem in context.CashDistributions
																 select new {
																	 UnderlyingFundName = (underlyingFundCashDistributionLineItem.UnderlyingFund != null ?
																					   underlyingFundCashDistributionLineItem.UnderlyingFund.FundName : string.Empty),
																	 AmberbrookFundName = (underlyingFundCashDistributionLineItem.UnderlyingFundCashDistribution != null ?
																						  underlyingFundCashDistributionLineItem.UnderlyingFundCashDistribution.Fund.FundName : string.Empty),
																	 DealName = (underlyingFundCashDistributionLineItem.Deal != null ?
																				 underlyingFundCashDistributionLineItem.Deal.DealName : string.Empty),
																	 underlyingFundCashDistributionLineItem.Amount,
																	 underlyingFundCashDistributionLineItem.DistributionDate,
																 }).ToList();
				model.UnderlyingFundStockDistributions = (from underlyingFundStockDistribution in context.UnderlyingFundStockDistributions
														  select new {
															  UnderlyingFundName = (underlyingFundStockDistribution.UnderlyingFund != null ?
																				 underlyingFundStockDistribution.UnderlyingFund.FundName : string.Empty),
															  AmberbrookFundName = (underlyingFundStockDistribution.UnderlyingFund != null ?
																					 underlyingFundStockDistribution.Fund.FundName : string.Empty),
															  underlyingFundStockDistribution.NumberOfShares,
															  underlyingFundStockDistribution.PurchasePrice,
															  underlyingFundStockDistribution.FMV,
															  underlyingFundStockDistribution.DistributionDate,
															  underlyingFundStockDistribution.NoticeDate,
															  SecurityType = (underlyingFundStockDistribution.SecurityType != null ?
															  underlyingFundStockDistribution.SecurityType.Name : string.Empty),
															  underlyingFundStockDistribution.TaxCostBase,
															  underlyingFundStockDistribution.TaxCostDate,
														  }).ToList();
				model.UnderlyingFundStockDistributionLineItems = (from underlyingFundStockDistributionLineItem in context.UnderlyingFundStockDistributionLineItems
																  select new {
																	  UnderlyingFundName = (underlyingFundStockDistributionLineItem.UnderlyingFund != null ?
																				underlyingFundStockDistributionLineItem.UnderlyingFund.FundName : string.Empty),
																	  AmberbrookFundName = (underlyingFundStockDistributionLineItem.UnderlyingFundStockDistribution != null ?
																						   underlyingFundStockDistributionLineItem.UnderlyingFundStockDistribution.Fund.FundName : string.Empty),
																	  DealName = (underlyingFundStockDistributionLineItem.Deal != null ?
																				  underlyingFundStockDistributionLineItem.Deal.DealName : string.Empty),
																	  underlyingFundStockDistributionLineItem.NumberOfShares,
																	  underlyingFundStockDistributionLineItem.FMV,
																  }).ToList();

				return model;
			}
		}

		public UnderlyingDirectExportExcelModel GetAllUnderlyingDirectExportList() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingDirectExportExcelModel model = new UnderlyingDirectExportExcelModel();

				model.Directs = (from direct in context.Issuers
								 join country in context.COUNTRies on direct.CountryID equals country.CountryID into countries
								 from country in countries.DefaultIfEmpty()
								 where direct.IsGP == false
								 select new {
									 CompanyName = direct.Name,
									 direct.ParentName,
									 Country = (country != null ? country.CountryName : string.Empty),
									 direct.AnnualMeetingDate,
									 direct.IsGP
								 }).ToList();

				model.Equities = (from equity in context.Equities
								  select new {
									  CompanyName = (equity.Issuer != null ? equity.Issuer.Name : string.Empty),
									  equity.Symbol,
									  EquityType = (equity.EquityType != null ? equity.EquityType.Equity : string.Empty),
									  ShareClassType = (equity.ShareClassType != null ? equity.ShareClassType.ShareClass : string.Empty),
									  Currency = (equity.Currency != null ? equity.Currency.Currency1 : string.Empty),
									  Industry = (equity.Industry != null ? equity.Industry.Industry1 : string.Empty),
									  equity.ISIN,
									  equity.Public,
									  equity.Comments
								  }).ToList();

				model.FixedIncomes = (from fixedIncome in context.FixedIncomes
									  select new {
										 CompanyName = (fixedIncome.Issuer != null ? fixedIncome.Issuer.Name : string.Empty),
										 fixedIncome.Symbol,
										 FixedIncomeType = (fixedIncome.FixedIncomeType != null ? fixedIncome.FixedIncomeType.FixedIncomeType1 : string.Empty),
										 Currency = (fixedIncome.Currency != null ? fixedIncome.Currency.Currency1 : string.Empty),
										 Industry = (fixedIncome.Industry != null ? fixedIncome.Industry.Industry1 : string.Empty),
										 fixedIncome.CouponInformation,
										 fixedIncome.FaceValue,
										 fixedIncome.FirstAccrualDate,
										 fixedIncome.FirstCouponDate,
										 fixedIncome.Frequency,
										 fixedIncome.ISIN,
										 fixedIncome.IssuedDate,
										 fixedIncome.Maturity,
									  }).ToList();

				return model;
			}
		}

		#endregion

		#region Deal Contact

		private IQueryable<Contact> GetDealContactQuery(System.Data.Objects.ObjectSet<Contact> contacts) {
			return contacts.Where(contact => contact.InvestorContacts.Count <= 0
								  && contact.UnderlyingFundContacts.Count <= 0
								 && contact.Deals1.Count <= 0
								 );
		}

		public List<AutoCompleteList> FindDealContacts(string contactName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Contact> dealContactQuery = GetDealContactQuery(context.Contacts);
				IQueryable<AutoCompleteList> query = (from contact in dealContactQuery
													  where contact.ContactName.StartsWith(contactName)
													  orderby contact.ContactName
													  select new AutoCompleteList {
														  id = contact.ContactID,
														  label = contact.ContactName,
														  value = contact.ContactName
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<DealContactList> GetAllDealContacts(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<DealContactList> query = (from contact in GetDealContactQuery(context.Contacts)
													 select new DealContactList {
														 ContactId = contact.ContactID,
														 ContactName = contact.ContactName,
														 ContactTitle = contact.Title,
														 ContactNotes = contact.Notes,
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<DealContactList> paginatedList = new PaginatedList<DealContactList>(query, pageIndex, pageSize);
				foreach (var contact in paginatedList) {
					List<CommunicationDetailModel> communications = GetContactCommunications(context, contact.ContactId);
					contact.Email = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Email);
					contact.Phone = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.HomePhone);
					contact.WebAddress = GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.WebAddress);
				}
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public Contact FindContact(int contactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Contacts.Include("ContactCommunications").Include("ContactCommunications.Communication").Where(contact => contact.ContactID == contactId).SingleOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveDealContact(Contact contact) {
			return contact.Save();
		}

		public bool DeleteDealContact(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Contact dealContact = context.Contacts.SingleOrDefault(contact => contact.ContactID == id);
				if (dealContact != null) {
					if (dealContact.Deals.Count() > 0 ||
						dealContact.Deals1.Count() > 0 ||
						dealContact.InvestorContacts.Count() > 0 ||
						dealContact.UnderlyingFundContacts.Count() > 0
						) {
						return false;
					}
					var contactAddresses = dealContact.ContactAddresses.ToList();
					foreach (var contactAddress in contactAddresses) {
						context.ContactAddresses.DeleteObject(contactAddress);
					}
					var contactCommunications = dealContact.ContactCommunications.ToList();
					foreach (var contactCommunication in contactCommunications) {
						context.Communications.DeleteObject(contactCommunication.Communication);
						context.ContactCommunications.DeleteObject(contactCommunication);
					}
					context.Contacts.DeleteObject(dealContact);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		#endregion

		#region User

		public List<USER> GetAllUsers(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.USER> query = (from user in context.USERs
														select user);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.USER> paginatedList = new PaginatedList<Models.Entity.USER>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public USER FindUser(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.USERs.SingleOrDefault(user => user.UserID == id);
			}
		}

		public bool UserNameAvailable(string userName, int userId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from user in context.USERs
						 where user.Login == userName && user.UserID != userId
						 select user.UserID).Count()) > 0 ? true : false;
			}
		}

		public bool EmailAvailable(string email, int userId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from user in context.USERs
						 where user.Email == email && user.UserID != userId
						 select user.UserID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteUser(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				USER user = context.USERs.SingleOrDefault(deleteUser => deleteUser.UserID == id);
				if (user != null) {
					context.USERs.DeleteObject(user);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveUser(USER user) {
			return user.Save();
		}

		#endregion

		#region  DocumentType

		public List<Models.Entity.DocumentType> GetAllDocumentTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.DocumentType> query = (from documentType in context.DocumentTypes.Include("DocumentSection")
																select documentType);
				switch (sortName) {
					case "DocumentSectionName":
						query = (sortOrder == "asc" ? query.OrderBy(documentType => documentType.DocumentSection.Name) : query.OrderByDescending(documentType => documentType.DocumentSection.Name));
						break;
					default:
						query = query.OrderBy(sortName, (sortOrder == "asc"));
						break;
				}
				PaginatedList<DocumentType> paginatedList = new PaginatedList<DocumentType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<DocumentType> GetAllDocumentTypes(int documentSectionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from documentType in context.DocumentTypes
						where documentType.DocumentSectionID == documentSectionId
						orderby documentType.DocumentTypeName
						select documentType).ToList();
			}
		}

		public DocumentType FindDocumentType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DocumentTypes.Include("DocumentSection").SingleOrDefault(type => type.DocumentTypeID == id);
			}
		}

		public bool DocumentTypeNameAvailable(string documentTypeName, int documentTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.DocumentTypes
						 where type.DocumentTypeName == documentTypeName && type.DocumentTypeID != documentTypeID
						 select type.DocumentTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteDocumentType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DocumentType documentType = context.DocumentTypes.SingleOrDefault(type => type.DocumentTypeID == id);
				if (documentType != null) {
					if (documentType.DealFundDocuments.Count == 0
						&& documentType.InvestorFundDocuments.Count == 0
						&& documentType.UnderlyingDirectDocuments.Count == 0
						&& documentType.UnderlyingFundDocuments.Count == 0
						) {
						context.DocumentTypes.DeleteObject(documentType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveDocumentType(DocumentType documentType) {
			return documentType.Save();
		}

		public List<DocumentSection> GetAllDocumentSections() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.DocumentSections.OrderBy(documentSection => documentSection.Name).ToList();
			}
		}

		public List<AutoCompleteList> FindDocumentTypes(string documentTypeName, int documentSectionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> documentTypeListQuery = (from documentType in context.DocumentTypes
																	  where documentType.DocumentSectionID == documentSectionId
																	  orderby documentType.DocumentTypeName
																	  select new AutoCompleteList {
																		  id = documentType.DocumentTypeID,
																		  label = documentType.DocumentTypeName,
																		  value = documentType.DocumentTypeName
																	  }).OrderBy(list => list.label);
				return new PaginatedList<AutoCompleteList>(documentTypeListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		#endregion

		#region Log
		public List<Log> GetAllLogs(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Log> query = (from log in context.Logs.Include("LogDetails")
										 orderby log.LogID descending
										 select log);
				PaginatedList<Log> paginatedList = new PaginatedList<Log>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}
		#endregion

		#region  SellerType

		public List<Models.Entity.SellerType> GetAllSellerTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.SellerType> query = (from sellerType in context.SellerTypes
															  select sellerType);
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<Models.Entity.SellerType> paginatedList = new PaginatedList<Models.Entity.SellerType>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public SellerType FindSellerType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.SellerTypes.SingleOrDefault(field => field.SellerTypeID == id);
			}
		}

		public bool SellerTypeNameAvailable(string sellerTypeName, int sellerTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from sellerType in context.SellerTypes
						 where sellerType.SellerType1 == sellerTypeName && sellerType.SellerTypeID != sellerTypeId
						 select sellerType.SellerTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteSellerType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				SellerType sellerType = context.SellerTypes.SingleOrDefault(type => type.SellerTypeID == id);
				if (sellerType != null) {
					if (sellerType.Deals.Count == 0) {
						context.SellerTypes.DeleteObject(sellerType);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveSellerType(SellerType sellerType) {
			return sellerType.Save();
		}

		public List<SellerType> GetAllSellerTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from sellerType in context.SellerTypes
						where sellerType.Enabled == true
						orderby sellerType.SellerType1
						select sellerType).ToList();
			}
		}

		public List<AutoCompleteList> FindSellerTypes(string sellerTypeName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> query = (from sellerType in context.SellerTypes
													  where sellerType.SellerType1.StartsWith(sellerTypeName)
													  orderby sellerType.SellerType1
													  select new AutoCompleteList {
														  id = sellerType.SellerTypeID,
														  label = sellerType.SellerType1,
														  value = sellerType.SellerType1
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		#endregion

	}
}