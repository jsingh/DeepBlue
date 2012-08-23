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
using DeepBlue.Models.Accounting;


namespace DeepBlue.Controllers.Accounting {
	public class AccountingRepository : IAccountingRepository {

		#region VirtualAccount

		public List<VirtualAccountListModel> GetAllVirtualAccountings(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<VirtualAccountListModel> query = (from virtualAccount in context.VirtualAccountsTable
															 select new VirtualAccountListModel {
																 AccountName = virtualAccount.AccountName,
																 FundName = virtualAccount.Fund.FundName,
																 ParentVirtualAccountName = (virtualAccount.VirtualAccount2 != null ? virtualAccount.VirtualAccount2.AccountName : ""),
																 LedgerBalance = virtualAccount.LedgerBalance,
																 VirtualAccountID = virtualAccount.VirtualAccountID
															 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<VirtualAccountListModel> paginatedList = new PaginatedList<VirtualAccountListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public VirtualAccountModel FindVirtualAccountModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from virtualAccount in context.VirtualAccountsTable
						where virtualAccount.VirtualAccountID == id
						select new VirtualAccountModel {
							AccountName = virtualAccount.AccountName,
							ActualAccountID = virtualAccount.ActualAccountID,
							FundID = virtualAccount.FundID,
							FundName = virtualAccount.Fund.FundName,
							LedgerBalance = virtualAccount.LedgerBalance,
							ParentVirtualAccountID = virtualAccount.ParentVirtualAccountID,
							ParentVirtualAccountName = (virtualAccount.VirtualAccount2 != null ? virtualAccount.VirtualAccount2.AccountName : ""),
							VirtualAccountID = virtualAccount.VirtualAccountID,
						}).FirstOrDefault();
			}
		}

		public VirtualAccount FindVirtualAccount(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from virtualAccount in context.VirtualAccountsTable
						where virtualAccount.VirtualAccountID == id
						select virtualAccount).FirstOrDefault();
			}
		}

		public bool AccountNameAvailable(string accountName, int virtualAccountID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from virtualAccount in context.VirtualAccountsTable
						 where virtualAccount.AccountName == accountName && virtualAccount.VirtualAccountID != virtualAccountID
						 select virtualAccount.VirtualAccountID).Count()) > 0 ? true : false;
			}
		}

		public IEnumerable<ErrorInfo> SaveVirtualAccount(VirtualAccount virtualAccount) {
			return virtualAccount.Save();
		}

		public List<AutoCompleteList> FindVirtualAccounts(string accountName, int virtualAccountID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var virtualAccountsTableQuery = context.VirtualAccountsTable;
				if (virtualAccountID > 0)
					virtualAccountsTableQuery = virtualAccountsTableQuery.Where(virAcc => virAcc.VirtualAccountID != virtualAccountID);

				IQueryable<AutoCompleteList> query = (from virtualAccount in virtualAccountsTableQuery
													  where virtualAccount.AccountName.StartsWith(accountName)
													  && (virtualAccount.ParentVirtualAccountID == null || (virtualAccount.ParentVirtualAccountID ?? 0) != virtualAccountID)
													  orderby virtualAccount.AccountName
													  select new AutoCompleteList {
														  id = virtualAccount.VirtualAccountID,
														  label = virtualAccount.AccountName,
														  value = virtualAccount.AccountName
													  });

				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindVirtualAccounts(string accountName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var virtualAccountsTableQuery = context.VirtualAccountsTable;
				IQueryable<AutoCompleteList> query = (from virtualAccount in virtualAccountsTableQuery
													  where virtualAccount.AccountName.StartsWith(accountName)
													  orderby virtualAccount.AccountName
													  select new AutoCompleteList {
														  id = virtualAccount.VirtualAccountID,
														  label = virtualAccount.AccountName,
														  value = virtualAccount.AccountName
													  });

				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public bool DeleteVirtualAccount(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				VirtualAccount virtualAccount = context.VirtualAccountsTable.FirstOrDefault(virAcc => virAcc.VirtualAccountID == id);
				if (virtualAccount != null) {
					foreach (var virAcc in virtualAccount.VirtualAccount1) {
						virAcc.ActualAccountID = null;
					}
					context.VirtualAccounts.DeleteObject(virtualAccount);
					context.SaveChanges();
					return true;
				}
				return false;
			}
		}

		#endregion

		#region AccountingEntryTemplate

		public List<AccountingEntryTemplateListModel> GetAllAccountingEntryTemplates(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AccountingEntryTemplateListModel> query = (from accountingEntryTemplate in context.AccountingEntryTemplatesTable
																	  select new AccountingEntryTemplateListModel {
																		  AccountingEntryAmountTypeName = accountingEntryTemplate.AccountingEntryAmountType.Name,
																		  AccountingEntryTemplateID = accountingEntryTemplate.AccountingEntryTemplateID,
																		  AccountingTransactionTypeName = accountingEntryTemplate.AccountingTransactionType.FriendlyName,
																		  FundName = accountingEntryTemplate.Fund.FundName,
																		  IsCredit = accountingEntryTemplate.IsCredit,
																		  VirtualAccountName = accountingEntryTemplate.VirtualAccount.AccountName
																	  });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<AccountingEntryTemplateListModel> paginatedList = new PaginatedList<AccountingEntryTemplateListModel>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public AccountingEntryTemplateModel FindAccountingEntryTemplateModel(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from accountingEntryTemplate in context.AccountingEntryTemplatesTable
						where accountingEntryTemplate.AccountingEntryTemplateID == id
						select new AccountingEntryTemplateModel {
							AccountingEntryAmountTypeData = accountingEntryTemplate.AccountingEntryAmountTypeData,
							AccountingEntryTemplateID = accountingEntryTemplate.AccountingEntryTemplateID,
							AccountingEntryAmountTypeID = accountingEntryTemplate.AccountingEntryAmountTypeID,
							AccountingTransactionTypeID = accountingEntryTemplate.AccountingTransactionTypeID,
							AccountingEntryAmountTypeName = accountingEntryTemplate.AccountingEntryAmountType.Name,
							AccountingTransactionTypeName = accountingEntryTemplate.AccountingTransactionType.FriendlyName,
							Amount = accountingEntryTemplate.Amount,
							Description = accountingEntryTemplate.Description,
							FundID = accountingEntryTemplate.FundID,
							FundName = accountingEntryTemplate.Fund.FundName,
							IsCredit = accountingEntryTemplate.IsCredit,
							VirtualAccountID = accountingEntryTemplate.VirtualAccountID,
							VirtualAccountName = accountingEntryTemplate.VirtualAccount.AccountName
						}).FirstOrDefault();
			}
		}

		public AccountingEntryTemplate FindAccountingEntryTemplate(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from accountingEntryTemplate in context.AccountingEntryTemplatesTable
						where accountingEntryTemplate.AccountingEntryTemplateID == id
						select accountingEntryTemplate).FirstOrDefault();
			}
		}

		public AccountingEntryTemplate FindAccountingEntryTemplate(int fundID,   int accountingTransactionTypeID, bool isCredit) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from accountingEntryTemplate in context.AccountingEntryTemplatesTable
						where accountingEntryTemplate.FundID == fundID  
						 && accountingEntryTemplate.AccountingTransactionTypeID == accountingTransactionTypeID 
						 && accountingEntryTemplate.IsCredit == isCredit 
						select accountingEntryTemplate).FirstOrDefault();
			}
		}

		public AccountingEntryTemplate FindAccountingEntryTemplate(int fundID, int accountingTransactionTypeID) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from accountingEntryTemplate in context.AccountingEntryTemplatesTable
						where accountingEntryTemplate.FundID == fundID
						 && accountingEntryTemplate.AccountingTransactionTypeID == accountingTransactionTypeID
						select accountingEntryTemplate).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveAccountingEntryTemplate(AccountingEntryTemplate accountingEntryTemplate) {
			return accountingEntryTemplate.Save();
		}


		public bool DeleteAccountingEntryTemplate(int id) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				AccountingEntryTemplate accountingEntryTemplate = context.AccountingEntryTemplatesTable.FirstOrDefault(virAcc => virAcc.AccountingEntryTemplateID == id);
				if (accountingEntryTemplate != null) {
					if (accountingEntryTemplate.AccountingEntries.Count() == 0) {
						context.AccountingEntryTemplates.DeleteObject(accountingEntryTemplate);
						context.SaveChanges();
						return true;
					}
				}
				return false;
			}
		}

		#endregion

		#region AccountingTransactionType
		public List<AutoCompleteList> FindAccountingTransactionTypes(string accountTransactionTypeName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> query = (from accTransactionType in context.AccountingTransactionTypeTable
													  where accTransactionType.FriendlyName.StartsWith(accountTransactionTypeName)
													  orderby accTransactionType.FriendlyName
													  select new AutoCompleteList {
														  id = accTransactionType.AccountingTransactionTypeID,
														  label = accTransactionType.FriendlyName,
														  value = accTransactionType.FriendlyName
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}
		#endregion

		#region AccountingEntryAmountType
		public List<AutoCompleteList> FindAccountingEntryAmountTypes(string accountingEntryAmountTypeName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> query = (from accEntryAmountType in context.AccountingEntryAmountTypesTable
													  where accEntryAmountType.Name.StartsWith(accountingEntryAmountTypeName)
													  orderby accEntryAmountType.Name
													  select new AutoCompleteList {
														  id = accEntryAmountType.AccouningEntryAmountTypeID,
														  label = accEntryAmountType.Name,
														  value = accEntryAmountType.Name
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}
		#endregion
		
		#region AccountingEntry 
		public IEnumerable<ErrorInfo> SaveAccountingEntry(AccountingEntry accountingEntry) {
			return accountingEntry.Save();
		}
		#endregion
	}
}