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
		bool DeleteInvestorEntityType(int id);
		IEnumerable<ErrorInfo> SaveInvestorEntityType(InvestorEntityType investorEntityType);
		#endregion

		#region InvestorType
		List<InvestorType> GetAllInvestorTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorType FindInvestorType(int id);
		bool InvestorTypeNameAvailable(string investorTypeName, int investorTypeID);
		bool DeleteInvestorType(int id);
		IEnumerable<ErrorInfo> SaveInvestorType(InvestorType investorType);
		#endregion

		#region FundClosing
		List<FundClosing> GetAllFundClosings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		FundClosing FindFundClosing(int id);
		bool FundClosingNameAvailable(string name, int fundClosingId);
		bool DeleteFundClosing(int id);
		IEnumerable<ErrorInfo> SaveFundClosing(FundClosing fundClosing);
		#endregion
		
		#region CustomField
		List<CustomField> GetAllCustomFields(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		List<CustomField> GetAllCustomFields(int moduleId);
		List<MODULE> GetAllModules();
		List<DataType> GetAllDataTypes();
		CustomField FindCustomField(int id);
		CustomFieldValue FindCustomFieldValue(int customFieldId,int key);
		bool CustomFieldTextAvailable(string customFieldText, int customFieldId);
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
	}
}
