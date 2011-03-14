using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Admin {
	public interface IAdminRepository {

		#region EntityType
		List<InvestorEntityType> GetAllInvestorEntityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorEntityType FindInvestorEntityType(int id);
		bool InvestorEntityTypeNameAvailable(string investorEntityTypeName, int investorEntityTypeID);
		bool DeleteInvestorEntityType(int id, ref bool isRelationExist);
		IEnumerable<ErrorInfo> SaveInvestorEntityType(InvestorEntityType investorEntityType);
		#endregion

		#region InvestorType
		List<InvestorType> GetAllInvestorTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorType FindInvestorType(int id);
		bool InvestorTypeNameAvailable(string investorTypeName, int investorTypeID);
		bool DeleteInvestorType(int id, ref bool isRelationExist);
		IEnumerable<ErrorInfo> SaveInvestorType(InvestorType investorType);
		#endregion

		#region FundClosing
		List<FundClosing> GetAllFundClosings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		FundClosing FindFundClosing(int id);
		bool FundClosingNameAvailable(string name, int fundClosingId);
		bool DeleteFundClosing(int id, ref bool isRelationExist);
		IEnumerable<ErrorInfo> SaveFundClosing(FundClosing FundClosingID);
		#endregion


		#region CustomField
		List<CustomField> GetAllCustomFields(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		CustomField FindCustomField(int id);
		bool CustomFieldTextAvailable(string customFieldText, int customFieldId);
		bool DeleteCustomField(int id, ref bool isRelationExist);
		IEnumerable<ErrorInfo> SaveCustomField(CustomField investorType);
		#endregion
	}
}
