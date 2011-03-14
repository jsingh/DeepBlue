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

		public bool DeleteInvestorEntityType(int id, ref bool isRelationExist) {
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
				IQueryable<Models.Entity.InvestorType> investorTypeQuery = (from investorType in context.InvestorTypes
																			select investorType);
				investorTypeQuery = investorTypeQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<InvestorType> paginatedList = new PaginatedList<InvestorType>(investorTypeQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public InvestorType FindInvestorType(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorTypes.SingleOrDefault(entityType => entityType.InvestorTypeID == id);
			}
		}

		public bool InvestorTypeNameAvailable(string investorTypeName, int investorTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from type in context.InvestorTypes
						 where type.InvestorTypeName == investorTypeName && type.InvestorTypeID != investorTypeID
						 select type.InvestorTypeID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteInvestorType(int id, ref bool isRelationExist) {
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
				if (sortName != "FundName") {
					query = query.OrderBy(sortName, (sortOrder == "asc"));
				} else {
					query = (sortOrder == "asc" ? query.OrderBy(fund => fund.Fund.FundName) : query.OrderByDescending(fund => fund.Fund.FundName));
				}
				PaginatedList<Models.Entity.FundClosing> paginatedList = new PaginatedList<Models.Entity.FundClosing>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public FundClosing FindFundClosing(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.FundClosings.SingleOrDefault(entityType => entityType.FundClosingID == id);
			}
		}

		public bool FundClosingNameAvailable(string name, int fundclosingId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from entityType in context.FundClosings
						 where entityType.Name == name && entityType.FundClosingID != fundclosingId
						 select entityType.FundClosingID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteFundClosing(int id, ref bool isRelationExist) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				FundClosing fundclose = context.FundClosings.SingleOrDefault(entityType => entityType.FundClosingID == id);
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

		public IEnumerable<ErrorInfo> SaveFundClosing(FundClosing FundClosings) {
			return FundClosings.Save();
		}

		#endregion

		#region IAdminRepository CustomField Members

		public List<Models.Entity.CustomField> GetAllCustomFields(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.CustomField> customFieldQuery = (from customField in context.CustomFields
																									 .Include("MODULE")
																									 .Include("DataType")
																		  orderby customField.CustomFieldID
																		  select customField);
				customFieldQuery = customFieldQuery.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<CustomField> paginatedList = new PaginatedList<CustomField>(customFieldQuery, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public CustomField FindCustomField(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.CustomFields.SingleOrDefault(entityType => entityType.CustomFieldID == id);
			}
		}

		public bool CustomFieldTextAvailable(string customFieldText, int customFieldId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from field in context.CustomFields
						 where field.CustomFieldText == customFieldText && field.CustomFieldID != customFieldId
						 select field.CustomFieldID).Count()) > 0 ? true : false;
			}
		}

		public bool DeleteCustomField(int id, ref bool isRelationExist) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				CustomField customField = context.CustomFields.SingleOrDefault(type => type.CustomFieldID == id);
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
			//	return customField.Save();
			return null;
		}

		#endregion

	}
}