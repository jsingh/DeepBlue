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
	}
}