using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Helpers;
using DeepBlue.Models.Accounting;
using DeepBlue.Models.Admin;

namespace DeepBlue.Controllers.Accounting {

	[OtherEntityAuthorize]
	public class AccountingController : BaseController {

		public IAccountingRepository AccountingRepository { get; set; }

		public AccountingController()
			: this(new AccountingRepository()) {
		}

		public AccountingController(IAccountingRepository accountingRepository) {
			AccountingRepository = accountingRepository;
		}

		#region VirtualAccount

		//
		// GET: /Accounting/VirtualAccount
		public ActionResult VirtualAccount() {
			return View();
		}

		//
		// GET: /Accounting/VirtualAccountList
		[HttpGet]
		public ActionResult VirtualAccountList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<VirtualAccountListModel> virtualAccounts = AccountingRepository.GetAllVirtualAccountings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var virtualAccount in virtualAccounts) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
					virtualAccount.VirtualAccountID,
					 virtualAccount.AccountName,
						virtualAccount.FundName,
						  virtualAccount.ParentVirtualAccountName,
						  virtualAccount.LedgerBalance
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Accounting/FindVirtualAccount/1
		public JsonResult FindVirtualAccount(int id) {
			VirtualAccountModel model = AccountingRepository.FindVirtualAccountModel(id);
			if (model == null) {
				model = new VirtualAccountModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Fund/FindParentVirtualAccounts
		[HttpGet]
		public JsonResult FindParentVirtualAccounts(string term, int virtualAccountID) {
			return Json(AccountingRepository.FindVirtualAccounts(term, virtualAccountID), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Fund/FindVirtualAccounts
		[HttpGet]
		public JsonResult FindVirtualAccounts(string term) {
			return Json(AccountingRepository.FindVirtualAccounts(term), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public string AccountNameAvailable(string fundName, int fundId) {
			if (AccountingRepository.AccountNameAvailable(fundName, fundId))
				return "Account Name already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Accounting/CreateVirtualAccount
		[HttpPost]
		public ActionResult CreateVirtualAccount(FormCollection collection) {
			VirtualAccountModel model = new VirtualAccountModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = AccountNameAvailable(model.AccountName, model.VirtualAccountID);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("AccountName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.VirtualAccount virtualAccount;
				if (model.VirtualAccountID > 0) {
					virtualAccount = AccountingRepository.FindVirtualAccount(model.VirtualAccountID);
				} else {
					virtualAccount = new Models.Entity.VirtualAccount();
					virtualAccount.CreatedBy = Authentication.CurrentUser.UserID;
					virtualAccount.CreatedDate = DateTime.Now;
				}
				virtualAccount.EntityID = Authentication.CurrentEntity.EntityID;
				virtualAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
				virtualAccount.LastUpdatedDate = DateTime.Now;
				virtualAccount.AccountName = model.AccountName;
				virtualAccount.FundID = model.FundID;
				virtualAccount.LedgerBalance = model.LedgerBalance;
				if (model.ActualAccountID > 0)
					virtualAccount.ActualAccountID = model.ActualAccountID;
				else
					virtualAccount.ActualAccountID = null;
				if (model.ParentVirtualAccountID > 0)
					virtualAccount.ParentVirtualAccountID = model.ParentVirtualAccountID;
				else
					virtualAccount.ParentVirtualAccountID = null;

				errorInfo = AccountingRepository.SaveVirtualAccount(virtualAccount);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
			} else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}

		[HttpGet]
		public string DeleteVirtualAccount(int id) {
			if (AccountingRepository.DeleteVirtualAccount(id) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region AccountingEntryTemplate

		//
		// GET: /Accounting/AccountingEntryTemplate
		public ActionResult AccountingEntryTemplate() {
			return View();
		}

		//
		// GET: /Accounting/AccountingEntryTemplateList
		[HttpGet]
		public ActionResult AccountingEntryTemplateList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<AccountingEntryTemplateListModel> accountingEntryTemplates = AccountingRepository.GetAllAccountingEntryTemplates(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var accountingEntryTemplate in accountingEntryTemplates) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
						accountingEntryTemplate.AccountingEntryTemplateID,
						accountingEntryTemplate.FundName,
						accountingEntryTemplate.VirtualAccountName,
						accountingEntryTemplate.AccountingTransactionTypeName,
						accountingEntryTemplate.AccountingEntryAmountTypeName,
						accountingEntryTemplate.IsCredit
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Accounting/FindAccountingEntryTemplate/1
		public JsonResult FindAccountingEntryTemplate(int id) {
			AccountingEntryTemplateModel model = AccountingRepository.FindAccountingEntryTemplateModel(id);
			if (model == null) {
				model = new AccountingEntryTemplateModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}
  
		//
		// GET: /Accounting/CreateAccountingEntryTemplate
		[HttpPost]
		public ActionResult CreateAccountingEntryTemplate(FormCollection collection) {
			AccountingEntryTemplateModel model = new AccountingEntryTemplateModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				Models.Entity.AccountingEntryTemplate accountingEntryTemplate;
				if (model.AccountingEntryTemplateID > 0) {
					accountingEntryTemplate = AccountingRepository.FindAccountingEntryTemplate(model.AccountingEntryTemplateID);
				} else {
					accountingEntryTemplate = new Models.Entity.AccountingEntryTemplate();
					accountingEntryTemplate.CreatedBy = Authentication.CurrentUser.UserID;
					accountingEntryTemplate.CreatedDate = DateTime.Now;
				}
				accountingEntryTemplate.EntityID = Authentication.CurrentEntity.EntityID;
				accountingEntryTemplate.LastUpdatedBy = Authentication.CurrentUser.UserID;
				accountingEntryTemplate.LastUpdatedDate = DateTime.Now;
				accountingEntryTemplate.FundID = model.FundID;
				accountingEntryTemplate.AccountingEntryAmountTypeID = model.AccountingEntryAmountTypeID;
				accountingEntryTemplate.AccountingEntryTemplateID = model.AccountingEntryTemplateID;
				accountingEntryTemplate.AccountingTransactionTypeID = model.AccountingTransactionTypeID;
				accountingEntryTemplate.Amount = model.Amount;
				accountingEntryTemplate.Description = model.Description;
				accountingEntryTemplate.VirtualAccountID = model.VirtualAccountID;
				accountingEntryTemplate.AccountingEntryAmountTypeData = model.AccountingEntryAmountTypeData;
				accountingEntryTemplate.IsCredit = model.IsCredit;
			
				errorInfo = AccountingRepository.SaveAccountingEntryTemplate(accountingEntryTemplate);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
			} else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}

		[HttpGet]
		public string DeleteAccountingEntryTemplate(int id) {
			if (AccountingRepository.DeleteAccountingEntryTemplate(id) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region AccountingTransactionType
		// GET: /Fund/FindAccountingTransactionTypes
		[HttpGet]
		public JsonResult FindAccountingTransactionTypes(string term) {
			return Json(AccountingRepository.FindAccountingTransactionTypes(term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region AccountingEntryAmountType
		// GET: /Fund/FindAccountingEntryAmountTypes
		[HttpGet]
		public JsonResult FindAccountingEntryAmountTypes(string term) {
			return Json(AccountingRepository.FindAccountingEntryAmountTypes(term), JsonRequestBehavior.AllowGet);
		}
		#endregion
	}
}
