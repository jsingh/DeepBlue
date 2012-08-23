using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using DeepBlue.Models.Deal;
using DeepBlue.Models.Accounting;

namespace DeepBlue.Controllers.Accounting {
	public interface IAccountingRepository {

		#region VirtualAccount
		List<VirtualAccountListModel> GetAllVirtualAccountings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		VirtualAccountModel FindVirtualAccountModel(int id);
		VirtualAccount FindVirtualAccount(int id);
		IEnumerable<ErrorInfo> SaveVirtualAccount(Models.Entity.VirtualAccount virtualAccount);
		bool AccountNameAvailable(string accountName, int virtualAccountID);
		List<AutoCompleteList> FindVirtualAccounts(string accountName);
		List<AutoCompleteList> FindVirtualAccounts(string accountName, int virtualAccountID);
		bool DeleteVirtualAccount(int id);
		#endregion

		#region AccountingEntryTemplate
		List<AccountingEntryTemplateListModel> GetAllAccountingEntryTemplates(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows);
		AccountingEntryTemplateModel FindAccountingEntryTemplateModel(int id);
		AccountingEntryTemplate FindAccountingEntryTemplate(int id);
		AccountingEntryTemplate FindAccountingEntryTemplate(int fundID,  int accountingTransactionTypeID, bool isCredit);
		AccountingEntryTemplate FindAccountingEntryTemplate(int fundID, int accountingTransactionTypeID);
		IEnumerable<ErrorInfo> SaveAccountingEntryTemplate(Models.Entity.AccountingEntryTemplate accountingEntryTemplate);
		bool DeleteAccountingEntryTemplate(int id);
		#endregion

		#region AccountingTransactionType
		List<AutoCompleteList> FindAccountingTransactionTypes(string accountTransactionTypeName);
		#endregion

		#region AccountingEntryAmountType
		List<AutoCompleteList> FindAccountingEntryAmountTypes(string accountingEntryAmountTypeName);
		#endregion

		#region AccountingEntry
        IEnumerable<ErrorInfo> SaveAccountingEntry(AccountingEntry accountingEntry); 
		#endregion
	}
}
