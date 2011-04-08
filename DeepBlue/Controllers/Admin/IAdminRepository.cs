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
		List<InvestorEntityType> GetAllInvestorEntityTypes();
		List<AddressType> GetAllAddressTypes();
		#endregion

		#region EntityType
		List<InvestorEntityType> GetAllInvestorEntityTypes(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		InvestorEntityType FindInvestorEntityType(int id);
		bool InvestorEntityTypeNameAvailable(string investorEntityTypeName, int investorEntityTypeID);
		bool DeleteInvestorEntityType(int id);
		IEnumerable<ErrorInfo> SaveInvestorEntityType(InvestorEntityType investorEntityType);
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
		CustomFieldValue FindCustomFieldValue(int customFieldId,int key);
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
		bool DeleteModuleId(int id);
		IEnumerable<ErrorInfo>SaveModule(MODULE module);
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
	}
}
