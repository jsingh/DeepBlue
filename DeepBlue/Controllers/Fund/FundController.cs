using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Fund;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Fund {
	public class FundController : Controller {

		public IFundRepository FundRepository { get; set; }

		public FundController()
			: this(new FundRepository()) {
		}

		public FundController(IFundRepository repository) {
			FundRepository = repository;
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
		// GET: /Fund/Success
		public ActionResult Success() {
			return View();
		}

		// GET: /Fund/Error
		public ActionResult Error() {
			return View();
		}

		//
		// GET: /Investor/New

		public ActionResult New(int? id) {
			CreateModel model = new CreateModel();
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
			return View(model);
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			if (ModelState.IsValid) {
				Models.Entity.Fund fund = new Models.Entity.Fund();
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
				FundAccount fundAccount = new FundAccount();
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

				fund.FundAccounts.Add(fundAccount);
				errorInfo = fund.Save();
				if (errorInfo != null) {
					return View("Error", errorInfo);
				} else {
					return View("Success");
				}
			} else {
				return View("New", model);
			}
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Update(FormCollection collection) {
			EditModel model = new EditModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			if (ModelState.IsValid) {
				Models.Entity.Fund fund = FundRepository.FindFund(model.FundId);
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
				FundAccount fundAccount = fund.FundAccounts.SingleOrDefault(account => account.FundAccountID == model.AccountId);
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

				fund.FundAccounts.Add(fundAccount);
				errorInfo = fund.Save();
				if (errorInfo != null) {
					return View("Error", errorInfo);
				} else {
					return View("Success");
				}
			} else {
				return View("New", model);
			}
		}

	}
}
