using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Fund;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Admin;
using System.Text;
using DeepBlue.Models.Admin.Enums;

namespace DeepBlue.Controllers.Fund {
	public class FundController : Controller {

		public IFundRepository FundRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public FundController()
			: this(new FundRepository(), new AdminRepository()) {
		}

		public FundController(IFundRepository fundRepository, IAdminRepository adminRepository) {
			FundRepository = fundRepository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Fund/

		public ActionResult Index() {
			ViewData["MenuName"] = "Fund Tracker";
			return View();
		}

		//
		// GET: /Fund/List
		[HttpGet]
		public ActionResult List(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<FundListModel> fundLists = FundRepository.GetAllFunds(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(fundLists);
		}

		//
		// GET: /Fund/FindFunds
		[HttpGet]
		public JsonResult FindFunds(string term) {
			List<Models.Entity.Fund> funds = FundRepository.FindFunds(term);
			List<AutoCompleteList> autoCompleteLists = new List<AutoCompleteList>();
			foreach (var detail in funds) {
				autoCompleteLists.Add(new AutoCompleteList { id = detail.FundID.ToString(), label = detail.FundName, value = detail.FundName });
			}
			return Json(autoCompleteLists, JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Investor/New
		public ActionResult New() {
			CreateModel model = new CreateModel();
			/* Load Custom Fields */
			model.CustomField = new CustomFieldModel();
			model.CustomField.DisplayTwoColumn = true;
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			foreach (var field in model.CustomField.Fields) {
				model.CustomField.Values.Add(new CustomFieldValueDetail {
					CustomFieldId = field.CustomFieldID,
					CustomFieldValueId = 0,
					DataTypeId = field.DataTypeID,
					BooleanValue = false,
					CurrencyValue = 0,
					DateValue = string.Empty,
					IntegerValue = 0,
					TextValue = string.Empty,
					Key = 0
				});
			}
			return View(model);
		}

		//
		// GET: /Investor/Edit/1
		public ActionResult Edit(int id) {
			EditModel model = new EditModel();
			Models.Entity.Fund fund = FundRepository.FindFund(id);
			foreach (var fundAccount in fund.FundAccounts) {
				model.ABANumber = string.Empty;
				model.Account = fundAccount.Account;
				model.AccountNumberCash = fundAccount.AccountNumberCash;
				model.AccountOf = fundAccount.AccountOf;
				model.Attention = fundAccount.Attention;
				model.BankName = fundAccount.BankName;
				model.FFCNumber = fundAccount.FFCNumber;
				model.IBAN = fundAccount.IBAN;
				model.Swift = fundAccount.SWIFT;
				model.Fax = fundAccount.Fax;
				model.Reference = fundAccount.Reference;
				model.AccountId = fundAccount.FundAccountID;
			}
			model.FundId = fund.FundID;
			model.FundName = fund.FundName;
			model.TaxId = fund.TaxID ?? string.Empty;
			model.FundStartDate = fund.InceptionDate ?? Convert.ToDateTime("01/01/1900");
			model.ScheduleTerminationDate = fund.ScheduleTerminationDate;
			model.FinalTerminationDate = fund.FinalTerminationDate;
			model.NumofAutoExtensions = fund.NumofAutoExtensions;
			model.RecycleProvision = fund.RecycleProvision;
			model.DateClawbackTriggered = fund.DateClawbackTriggered;
			model.MgmtFeesCatchUpDate = fund.MgmtFeesCatchUpDate;
			model.Carry = fund.Carry;
			/* Load Custom Fields */
			model.CustomField = new CustomFieldModel();
			model.CustomField.DisplayTwoColumn = true;
			IList<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(id);
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			foreach (var field in model.CustomField.Fields) {
				var value = customFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == field.CustomFieldID);
				if (value != null) {
					model.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = value.CustomFieldID,
						CustomFieldValueId = value.CustomFieldID,
						DataTypeId = value.CustomField.DataTypeID,
						BooleanValue = value.BooleanValue ?? false,
						CurrencyValue = value.CurrencyValue ?? 0,
						DateValue = ((value.DateValue ?? Convert.ToDateTime("01/01/1900")).Year == 1900 ? string.Empty : (value.DateValue ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")),
						IntegerValue = value.IntegerValue ?? 0,
						TextValue = value.TextValue,
						Key = id
					});
				} else {
					model.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = field.CustomFieldID,
						CustomFieldValueId = 0,
						DataTypeId = field.DataTypeID,
						BooleanValue = false,
						CurrencyValue = 0,
						DateValue = string.Empty,
						IntegerValue = 0,
						TextValue = string.Empty,
						Key = id
					});
				}
			}

			return View(model);
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return SaveFundModel(collection);
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Update(FormCollection collection) {
			return SaveFundModel(collection);
		}

		private ActionResult SaveFundModel(FormCollection collection) {
			FundDetail model = new FundDetail();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				Models.Entity.Fund fund;
				if (model.FundId > 0) {
					fund = FundRepository.FindFund(model.FundId);
				} else {
					fund = new Models.Entity.Fund();
				}
				fund.Carry = model.Carry ?? 0;
				fund.DateClawbackTriggered = model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900");
				fund.EntityID = (int)ConfigUtil.CurrentEntityID;
				fund.FinalTerminationDate = model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900");
				fund.FundName = model.FundName;
				fund.InceptionDate = model.FundStartDate;
				fund.MgmtFeesCatchUpDate = model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900");
				fund.NumofAutoExtensions = model.NumofAutoExtensions ?? 0;
				fund.RecycleProvision = model.RecycleProvision ?? 0;
				fund.ScheduleTerminationDate = model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900");
				fund.TaxID = model.TaxId;

				//Add bank account
				FundAccount fundAccount;
				if (model.AccountId > 0) {
					fundAccount = fund.FundAccounts.SingleOrDefault(account => account.FundAccountID == model.AccountId);
				} else {
					fundAccount = new FundAccount();
					fund.FundAccounts.Add(fundAccount);
				}
				fundAccount.Account = model.Account;
				fundAccount.AccountNumberCash = model.AccountNumberCash ?? string.Empty;
				fundAccount.AccountOf = model.AccountOf ?? string.Empty;
				fundAccount.Attention = model.Attention ?? string.Empty;
				fundAccount.BankName = model.BankName ?? string.Empty;
				fundAccount.CreatedBy = AppSettings.CreatedByUserId;
				fundAccount.CreatedDate = DateTime.Now;
				fundAccount.EntityID = (int)ConfigUtil.CurrentEntityID;
				fundAccount.Fax = model.Fax ?? string.Empty;
				fundAccount.FFCNumber = model.FFCNumber ?? string.Empty;
				fundAccount.IBAN = model.IBAN ?? string.Empty;
				fundAccount.IsPrimary = false;
				fundAccount.LastUpdatedBy = AppSettings.CreatedByUserId;
				fundAccount.LastUpdatedDate = DateTime.Now;
				fundAccount.Phone = model.Telephone ?? string.Empty;
				fundAccount.Reference = model.Reference ?? string.Empty;
				fundAccount.Routing = 0;
				fundAccount.SWIFT = model.Swift ?? string.Empty;

				errorInfo = fund.Save();
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				} else {
					// Update custom field Values
					resultModel.Result += SaveCustomValues(collection, fund.FundID);
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

		private string SaveCustomValues(FormCollection collection, int key) {
			System.Text.StringBuilder result = new StringBuilder();
			IEnumerable<ErrorInfo> errorInfo;
			IList<CustomField> customFields = AdminRepository.GetAllCustomFields((int)Models.Admin.Enums.Module.Fund);
			foreach (var field in customFields) {
				var customFieldValue = collection["CustomField_" + field.CustomFieldID.ToString()];
				if (customFieldValue != null) {
					CustomFieldValue value = AdminRepository.FindCustomFieldValue(field.CustomFieldID, key);
					if (value == null) {
						value = new CustomFieldValue();
					}
					value.CreatedBy = AppSettings.CreatedByUserId;
					value.CreatedDate = DateTime.Now;
					value.CustomFieldID = field.CustomFieldID;
					value.Key = key;
					value.LastUpdatedBy = AppSettings.CreatedByUserId;
					value.LastUpdatedDate = DateTime.Now;
					switch ((CustomFieldDataType)field.DataTypeID) {
						case CustomFieldDataType.Integer:
							value.IntegerValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToInt32(customFieldValue));
							break;
						case CustomFieldDataType.DateTime:
							value.DateValue = (string.IsNullOrEmpty(customFieldValue) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(customFieldValue));
							break;
						case CustomFieldDataType.Text:
							value.TextValue = customFieldValue;
							break;
						case CustomFieldDataType.Currency:
							value.CurrencyValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToDecimal(customFieldValue));
							break;
						case CustomFieldDataType.Boolean:
							value.BooleanValue = (customFieldValue.Contains("true") ? true : false);
							break;
					}
					errorInfo = value.Save();
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							result.Append(err.PropertyName + " : " + err.ErrorMessage + "\n");
						}
					}
				}
			}
			return result.ToString();
		}

		[HttpGet]
		public string TaxIdAvailable(string TaxId, int FundId) {
			if (FundRepository.TaxIdAvailable(TaxId, FundId))
				return "TaxId already exist";
			else
				return string.Empty;
		}

		[HttpGet]
		public string FundNameAvailable(string FundName, int FundId) {
			if (FundRepository.FundNameAvailable(FundName, FundId))
				return "Fund Name already exist";
			else
				return string.Empty;
		}

	}
}
