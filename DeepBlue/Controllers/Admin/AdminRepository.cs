using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Admin {
	public class AdminRepository : IAdminRepository {

		#region IAdminRepository InvestorEntityType Members

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

		#region IAdminRepository InvestorType Members

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

		#region IAdminRepository FundClosing

		public List<Models.Entity.FundClosing> GetAllFundClosings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.FundClosing> query = (from fund in context.FundClosings
																		 .Include("Fund")
															   select fund);
				if (sortName == "FundName") {
					query = (sortOrder == "asc" ? query.OrderBy(fund => fund.Fund.FundName) : query.OrderByDescending(fund => fund.Fund.FundName));
				} else {
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

		#region IAdminRepository CustomField Members

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

		public List<CustomField> GetAllCustomFields(int moduleId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from field in context.CustomFields
						where field.ModuleID == moduleId
						orderby field.CustomFieldText
						select field).ToList();
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
							  .SingleOrDefault(field => field.CustomFieldID == id);
			}
		}

		public CustomFieldValue FindCustomFieldValue(int customFieldId, int key) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CustomFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == customFieldId && fieldValue.Key == key);
			}
		}
	 

		public bool CustomFieldTextAvailable(string customFieldText, int customFieldId,int moduleId) {
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

		#region IAdminRepository DataType Members

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

		#region IAdminRepository Module

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

		public MODULE  FindModule(int id) {
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

		public bool DeleteModuleId(int id) {
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

		#region IAdminRepository Get

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

		#region IAdminRepository CommunicationType Members

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
				return context.CommunicationTypes.SingleOrDefault(type => type.CommunicationTypeID == id);
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

		#region IAdminRepository CommunicationGrouping Members

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

		#region IAdminRepository PurchaseType Members

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

		#region IAdminRepository DealClosingCostType Members

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

		#region IAdminRepository DocumentTypes
		public List<DocumentType> GetAllDocumentTypes() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from document in context.DocumentTypes
						orderby document.DocumentTypeName
						select document).ToList();
			}
		}
		#endregion

		#region IAdminRepository Communication
		public string GetContactCommunicationValue(int contactId, Models.Admin.Enums.CommunicationType communicationType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from contactCommunication in context.ContactCommunications
						where contactCommunication.ContactID == contactId && contactCommunication.Communication.CommunicationTypeID == (int)communicationType
						select contactCommunication.Communication.CommunicationValue).FirstOrDefault();
			}
		}
		#endregion

		#region IAdminRepository SecurityType Members

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

		#region IAdminRepository UnderlingFundType

		public List<Models.Entity.UnderlyingFundType> GetAllUnderlyingFundTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.UnderlyingFundType> query = (from module in context.UnderlyingFundTypes
														  select module);
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

		public bool UnderlyingFundTypeTextAvailable(string underlyingfundtypeFieldText, int underlyingfundtypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.UnderlyingFundTypes
						 where field.Name == underlyingfundtypeFieldText && field.UnderlyingFundTypeID != underlyingfundtypeFieldId
						 select field.UnderlyingFundTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteUnderlyingFundTypeId(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				UnderlyingFundType underlyingfundtype = context.UnderlyingFundTypes.SingleOrDefault(field => field.UnderlyingFundTypeID == id);
				if (underlyingfundtype != null) {
					if (underlyingfundtype.UnderlyingFunds.Count == 0) {
						context.UnderlyingFundTypes.DeleteObject(underlyingfundtype);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveUnderlyingFundType(UnderlyingFundType  underlyingfundtype) {
			return underlyingfundtype.Save();
		}
		#endregion

		#region IAdminRepository ShareClassType

		public List<Models.Entity.ShareClassType> GetAllShareClassTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.ShareClassType> query = (from module in context.ShareClassTypes
																	  select module);
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

		public bool ShareClassTypeTextAvailable(string shareclasstypeFieldText, int shareclasstypeFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.ShareClassTypes
						 where field.ShareClass == shareclasstypeFieldText && field.ShareClassTypeID != shareclasstypeFieldId
						 select field.ShareClassTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteShareClassTypeId(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				ShareClassType shareclasstype = context.ShareClassTypes.SingleOrDefault(field => field.ShareClassTypeID == id);
				if (shareclasstype != null) {
					if (shareclasstype.UnderlyingFunds.Count == 0) {
						context.ShareClassTypes.DeleteObject(shareclasstype);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		public IEnumerable<ErrorInfo> SaveShareClassType(ShareClassType shareclasstype) {
			return shareclasstype.Save();
		}
		#endregion
	}
}