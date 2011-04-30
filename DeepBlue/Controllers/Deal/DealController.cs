using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Deal;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Admin;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.Entity;
using System.Text;
using DeepBlue.Controllers.Issuer;
using DeepBlue.Models.Issuer;

namespace DeepBlue.Controllers.Deal {
	public class DealController : Controller {

		public IDealRepository DealRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public IIssuerRepository IssuerRepository { get; set; }

		public DealController()
			: this(new DealRepository(), new AdminRepository(), new IssuerRepository()) {
		}

		public DealController(IDealRepository dealRepository, IAdminRepository adminRepository, IIssuerRepository issuerRepository) {
			DealRepository = dealRepository;
			AdminRepository = adminRepository;
			IssuerRepository = issuerRepository;
		}

		#region Deal

		//
		// GET: /Deal/New
		public ActionResult New() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Create New Deal";
			ViewData["PageName"] = "Create New Deal";
			return View(GetNewDealModel());
		}

		//
		// GET: /Deal/Edit
		public ActionResult Edit() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Modify Deal";
			ViewData["PageName"] = "Modify Deal";
			return View("New", GetNewDealModel());
		}

		private CreateModel GetNewDealModel() {
			CreateModel model = new CreateModel();
			model.Contacts = SelectListFactory.GetEmptySelectList();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.PurchaseTypes = SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
			model.DealClosingCostTypes = SelectListFactory.GetDealClosingCostTypeSelectList(AdminRepository.GetAllDealClosingCostTypes());
			model.UnderlyingFunds = SelectListFactory.GetUnderlyingFundSelectList(DealRepository.GetAllUnderlyingFunds());
			model.Issuers = SelectListFactory.GetIssuerSelectList(IssuerRepository.GetAllIssuers());
			model.SecurityTypes = SelectListFactory.GetSecurityTypeSelectList(AdminRepository.GetAllSecurityTypes());
			model.Securities = SelectListFactory.GetEmptySelectList();
			return model;
		}

		//
		// GET: /Deal/DealList
		[HttpGet]
		public ActionResult DealList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			List<Models.Entity.Deal> deals = DealRepository.GetAllDeals(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(deals);
		}

		//
		// POST: /Deal/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			DealDetailModel model = new DealDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = DealdNameAvailable(model.DealName, model.DealId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("DealName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.Deal deal = DealRepository.FindDeal(model.DealId);
				if (deal == null) {
					deal = new Models.Entity.Deal();
					deal.CreatedBy = AppSettings.CreatedByUserId;
					deal.CreatedDate = DateTime.Now;
				}
				deal.EntityID = (int)ConfigUtil.CurrentEntityID;
				deal.DealName = model.DealName;
				deal.DealNumber = DealRepository.GetMaxDealNumber();
				deal.FundID = model.FundId;
				deal.IsPartnered = model.IsPartnered;
				if (deal.IsPartnered) {
					if (deal.Partner == null) {
						deal.Partner = new Partner();
						deal.Partner.CreatedBy = AppSettings.CreatedByUserId;
						deal.Partner.CreatedDate = DateTime.Now;
					}
					deal.Partner.EntityID = (int)ConfigUtil.CurrentEntityID;
					deal.Partner.LastUpdatedBy = AppSettings.CreatedByUserId;
					deal.Partner.LastUpdatedDate = DateTime.Now;
					deal.Partner.PartnerName = model.PartnerName;
				}
				else {
					deal.Partner = null;
				}
				deal.LastUpdatedBy = AppSettings.CreatedByUserId;
				deal.LastUpdatedDate = DateTime.Now;
				deal.PurchaseTypeID = model.PurchaseTypeId;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDeal(deal);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				DealClosing dealClosing = new DealClosing(); //Create new Deal Closing
				resultModel.Result = CreateDealClosing(deal.DealID, out dealClosing);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + deal.DealID;
				}
			}
			else {
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
		public string DealdNameAvailable(string DealName, int DealId) {
			if (DealRepository.DealNameAvailable(DealName, DealId))
				return "Deal Name already exist";
			else
				return string.Empty;
		}

		private void AddCommunication(DeepBlue.Models.Entity.Contact contact, DeepBlue.Models.Admin.Enums.CommunicationType communicationType, string value) {
			ContactCommunication contactCommunication = contact.ContactCommunications.SingleOrDefault(communication => communication.Communication.CommunicationTypeID == (int)communicationType);
			if (contactCommunication == null) {
				contactCommunication = new ContactCommunication();
				contactCommunication.CreatedBy = AppSettings.CreatedByUserId;
				contactCommunication.CreatedDate = DateTime.Now;
				contactCommunication.Communication = new Communication();
				contactCommunication.Communication.CreatedBy = AppSettings.CreatedByUserId;
				contactCommunication.Communication.CreatedDate = DateTime.Now;
				contact.ContactCommunications.Add(contactCommunication);
			}
			contactCommunication.EntityID = (int)ConfigUtil.CurrentEntityID;
			contactCommunication.LastUpdatedBy = AppSettings.CreatedByUserId;
			contactCommunication.LastUpdatedDate = DateTime.Now;
			contactCommunication.Communication.CommunicationTypeID = (int)communicationType;
			contactCommunication.Communication.CommunicationValue = (string.IsNullOrEmpty(value) == false ? value : string.Empty);
			contactCommunication.Communication.LastUpdatedBy = AppSettings.CreatedByUserId;
			contactCommunication.Communication.LastUpdatedDate = DateTime.Now;
			contactCommunication.Communication.EntityID = (int)ConfigUtil.CurrentEntityID;
		}

		//
		// GET: /Deal/Edit/5
		public JsonResult FindFund(int fundId) {
			DealDetailModel deal = new DealDetailModel();
			deal.DealNumber = DealRepository.GetMaxDealNumber();
			return Json(deal, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeal/5
		public JsonResult FindDeal(int dealId) {
			Models.Entity.Deal deal = DealRepository.FindDeal(dealId);
			DealDetailModel dealDetail = new DealDetailModel();
			dealDetail.ContactId = deal.ContactID;
			dealDetail.DealId = deal.DealID;
			dealDetail.DealName = deal.DealName;
			dealDetail.DealNumber = deal.DealNumber;
			dealDetail.FundId = deal.FundID;
			dealDetail.FundName = deal.Fund.FundName;
			if (deal.Partner != null) {
				dealDetail.PartnerName = deal.Partner.PartnerName;
			}
			dealDetail.PurchaseTypeId = deal.PurchaseTypeID ?? 0;
			dealDetail.IsPartnered = deal.IsPartnered;
			dealDetail.SellerInfo.DealId = deal.DealID;
			if (deal.Contact1 != null) {
				dealDetail.SellerInfo.SellerName = deal.Contact1.FirstName;
				dealDetail.SellerInfo.ContactName = deal.Contact1.ContactName;
				int sellerContactId = deal.SellerContactID ?? 0;
				if (sellerContactId > 0) {
					dealDetail.SellerInfo.Email = AdminRepository.GetContactCommunicationValue(sellerContactId, Models.Admin.Enums.CommunicationType.Email);
					dealDetail.SellerInfo.Fax = AdminRepository.GetContactCommunicationValue(sellerContactId, Models.Admin.Enums.CommunicationType.Fax);
					dealDetail.SellerInfo.Phone = AdminRepository.GetContactCommunicationValue(sellerContactId, Models.Admin.Enums.CommunicationType.HomePhone);
				}
			}
			foreach (var dealClosingCost in deal.DealClosingCosts) {
				dealDetail.DealExpenses.Add(GetDealClosingCostModel(dealClosingCost));
			}
			foreach (var dealUnderlyingFund in deal.DealUnderlyingFunds) {
				dealDetail.DealUnderlyingFunds.Add(GetDealUnderlyingFundModel(dealUnderlyingFund));
			}
			foreach (var dealUnderlyingDirect in deal.DealUnderlyingDirects) {
				dealDetail.DealUnderlyingDirects.Add(GetDealUnderlyingDirectModel(dealUnderlyingDirect));
			}
			return Json(dealDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeals
		[HttpGet]
		public JsonResult FindDeals(string term) {
			return Json(DealRepository.FindDeals(term), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region DealExpense
		//
		// GET: /Deal/CreateDealExpense
		[HttpPost]
		public string CreateDealExpense(FormCollection collection) {
			DealClosingCostModel model = new DealClosingCostModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				DealClosingCost dealClosingCost = DealRepository.FindDealClosingCost(model.DealClosingCostId ?? 0);
				if (dealClosingCost == null) {
					dealClosingCost = new DealClosingCost();
				}
				dealClosingCost.DealClosingCostTypeID = model.DealClosingCostTypeId;
				dealClosingCost.Amount = model.Amount;
				dealClosingCost.Date = model.Date;
				dealClosingCost.DealID = model.DealId;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealClosingCost(dealClosingCost);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + dealClosingCost.DealClosingCostID;
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return resultModel.Result;
		}

		//
		// GET: /Deal/FindDealClosingCost/1
		[HttpGet]
		public JsonResult FindDealClosingCost(int dealClosingCostId) {
			DealClosingCost dealClosingCost = DealRepository.FindDealClosingCost(dealClosingCostId);
			return Json(GetDealClosingCostModel(dealClosingCost), JsonRequestBehavior.AllowGet);
		}

		private DealClosingCostModel GetDealClosingCostModel(DealClosingCost dealClosingCost) {
			return new DealClosingCostModel {
				Amount = dealClosingCost.Amount,
				Date = (dealClosingCost.Date ?? Convert.ToDateTime("01/01/1900")),
				DealClosingCostId = dealClosingCost.DealClosingCostID,
				DealClosingCostTypeId = dealClosingCost.DealClosingCostTypeID,
				DealId = dealClosingCost.DealID,
				Description = dealClosingCost.DealClosingCostType.Name
			};
		}

		//
		// GET: /Deal/DeleteDealExpense/1
		[HttpGet]
		public void DeleteDealClosingCost(int id) {
			DealRepository.DeleteDealClosingCost(id);
		}
		#endregion

		#region Deal Seller Info
		//
		// POST: /Deal/CreateSellerInfo
		[HttpPost]
		public string CreateSellerInfo(FormCollection collection) {
			DealSellerDetailModel model = new DealSellerDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				Models.Entity.Deal deal = DealRepository.FindDeal(model.DealId);
				if (deal != null) {
					if (deal.Contact1 == null) {
						deal.Contact1 = new Contact();
						deal.Contact1.CreatedBy = AppSettings.CreatedByUserId;
						deal.Contact1.CreatedDate = DateTime.Now;
					}
					deal.Contact1.EntityID = (int)ConfigUtil.CurrentEntityID;
					deal.Contact1.ContactName = model.ContactName;
					deal.Contact1.FirstName = model.SellerName;
					deal.Contact1.LastName = "n/a";
					deal.Contact1.LastUpdatedBy = AppSettings.CreatedByUserId;
					deal.Contact1.LastUpdatedDate = DateTime.Now;
					AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.Email, model.Email);
					AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.Fax, model.Fax);
					AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
					IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDeal(deal);
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
						}
					}
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return resultModel.Result;
		}
		#endregion

		#region DealUnderlyingFund
		//
		// GET: /Deal/CreateDealUnderlyingFund
		[HttpPost]
		public string CreateDealUnderlyingFund(FormCollection collection) {
			DealUnderlyingFundModel model = new DealUnderlyingFundModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				DealUnderlyingFund dealUnderlyingFund = DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId ?? 0);
				if (dealUnderlyingFund == null) {
					dealUnderlyingFund = new DealUnderlyingFund();
				}
				dealUnderlyingFund.CommittedAmount = model.CommittedAmount;
				dealUnderlyingFund.DealID = model.DealId;
				dealUnderlyingFund.FundNav = model.FundNav;
				dealUnderlyingFund.Percent = model.Percent;
				dealUnderlyingFund.RecordDate = model.RecordDate;
				dealUnderlyingFund.UnderlyingFundID = model.UnderlyingFundId;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + dealUnderlyingFund.DealUnderlyingtFundID;
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return resultModel.Result;
		}

		//
		// GET: /Deal/FindDealUnderlyingFund/1
		[HttpGet]
		public JsonResult FindDealUnderlyingFund(int dealUnderlyingFundId) {
			DealUnderlyingFund dealUnderlyingFund = DealRepository.FindDealUnderlyingFund(dealUnderlyingFundId);
			return Json(GetDealUnderlyingFundModel(dealUnderlyingFund), JsonRequestBehavior.AllowGet);
		}

		private DealUnderlyingFundModel GetDealUnderlyingFundModel(DealUnderlyingFund dealUnderlyingFund) {
			return new DealUnderlyingFundModel {
				CommittedAmount = dealUnderlyingFund.CommittedAmount ?? 0,
				UnderlyingFundId = dealUnderlyingFund.UnderlyingFundID,
				RecordDate = dealUnderlyingFund.RecordDate ?? Convert.ToDateTime("01/01/1900"),
				Percent = dealUnderlyingFund.Percent ?? 0,
				FundNav = dealUnderlyingFund.FundNav ?? 0,
				FundName = dealUnderlyingFund.UnderlyingFund.FundName,
				DealId = dealUnderlyingFund.DealID,
				DealUnderlyingFundId = dealUnderlyingFund.DealUnderlyingtFundID
			};
		}

		//
		// GET: /Deal/DeleteDealUnderlyingFund/1
		[HttpGet]
		public void DeleteDealUnderlyingFund(int id) {
			DealRepository.DeleteDealUnderlyingFund(id);
		}
		#endregion

		#region DealUnderlyingDirect
		//
		// GET: /Deal/CreateDealUnderlyingDirect
		[HttpPost]
		public string CreateDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model = new DealUnderlyingDirectModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				DealUnderlyingDirect dealUnderlyingDirect = DealRepository.FindDealUnderlyingDirect(model.DealUnderlyingDirectId ?? 0);
				if (dealUnderlyingDirect == null) {
					dealUnderlyingDirect = new DealUnderlyingDirect();
				}
				dealUnderlyingDirect.DealID = model.DealId;
				dealUnderlyingDirect.Percent = model.Percent;
				dealUnderlyingDirect.RecordDate = model.RecordDate;
				dealUnderlyingDirect.FMV = model.FMV;
				dealUnderlyingDirect.NumberOfShares = model.NumberOfShares;
				dealUnderlyingDirect.DealClosingID = FindDealClosing(model.DealId).DealClosingID;
				dealUnderlyingDirect.SecurityID = model.SecurityId;
				dealUnderlyingDirect.SecurityTypeID = model.SecurityTypeId;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + dealUnderlyingDirect.DealUnderlyingDirectID;
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return resultModel.Result;
		}

		//
		// GET: /Deal/FindDealUnderlyingDirect/1
		[HttpGet]
		public JsonResult FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			DealUnderlyingDirect dealUnderlyingDirect = DealRepository.FindDealUnderlyingDirect(dealUnderlyingDirectId);
			return Json(GetDealUnderlyingDirectModel(dealUnderlyingDirect), JsonRequestBehavior.AllowGet);
		}

		private DealUnderlyingDirectModel GetDealUnderlyingDirectModel(DealUnderlyingDirect dealUnderlyingDirect) {
			EquityDetailModel quityModel;
			FixedIncomeDetailModel fixedIncomeModel;
			DealUnderlyingDirectModel model = new DealUnderlyingDirectModel {
				RecordDate = dealUnderlyingDirect.RecordDate ?? Convert.ToDateTime("01/01/1900"),
				Percent = dealUnderlyingDirect.Percent ?? 0,
				DealId = dealUnderlyingDirect.DealID,
				DealUnderlyingDirectId = dealUnderlyingDirect.DealUnderlyingDirectID,
				NumberOfShares = dealUnderlyingDirect.NumberOfShares,
				FMV = dealUnderlyingDirect.FMV,
				SecurityId = dealUnderlyingDirect.SecurityID,
				SecurityTypeId = dealUnderlyingDirect.SecurityTypeID,
				SecurityType = dealUnderlyingDirect.SecurityType.Name
			};
			switch ((DeepBlue.Models.Deal.Enums.SecurityType)model.SecurityTypeId) {
				case Models.Deal.Enums.SecurityType.Equity:
					quityModel = IssuerRepository.FindEquity(model.SecurityId);
					model.IssuerName = quityModel.IssuerName;
					model.IssuerId = quityModel.IssuerId;
					model.Security = quityModel.Symbol;
					break;
				case Models.Deal.Enums.SecurityType.FixedIncome:
					fixedIncomeModel = IssuerRepository.FindFixedIncome(model.SecurityId);
					model.IssuerName = fixedIncomeModel.IssuerName;
					model.IssuerId = fixedIncomeModel.IssuerId;
					model.Security = fixedIncomeModel.Symbol;
					break;
			}
			model.Equities = SelectListFactory.GetEquitySelectList(IssuerRepository.GetAllEquity(model.IssuerId));
			model.FixedIncomes = SelectListFactory.GetFixedIncomeSelectList(IssuerRepository.GetAllFixedIncome(model.IssuerId));
			return model;
		}

		//
		// GET: /Deal/DeleteDealUnderlyingDirect/1
		[HttpGet]
		public void DeleteDealUnderlyingDirect(int id) {
			DealRepository.DeleteDealUnderlyingDirect(id);
		}
		#endregion

		#region Security
		[HttpGet]
		public JsonResult GetSecurity(int issuerId, int securityTypeId) {
			List<SelectListItem> securityLists = null;
			switch ((DeepBlue.Models.Deal.Enums.SecurityType)securityTypeId) {
				case Models.Deal.Enums.SecurityType.Equity:
					securityLists = SelectListFactory.GetEquitySelectList(IssuerRepository.GetAllEquity(issuerId));
					break;
				case Models.Deal.Enums.SecurityType.FixedIncome:
					securityLists = SelectListFactory.GetFixedIncomeSelectList(IssuerRepository.GetAllFixedIncome(issuerId));
					break;
			}
			return Json(securityLists, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region DealClosing
		//
		// GET: /Deal/CreateDealClosing
		[HttpGet]
		public string CreateDealClosing(int dealId, out DealClosing dealClosing) {
			string result = string.Empty;
			dealClosing = new DealClosing { DealID = dealId, CloseDate = DateTime.Now, DealClosingID = 0 };
			IEnumerable<ErrorInfo> errorInfo = dealClosing.Save();
			if (errorInfo != null) {
				foreach (var err in errorInfo.ToList()) {
					result += err.PropertyName + " : " + err.ErrorMessage + "\n";
				}
			}
			return result;
		}

		//
		// GET: /Deal/FindDealClosing
		[HttpGet]
		private DealClosing FindDealClosing(int dealId) {
			DealClosing dealClosing = DealRepository.FindDealClosing(dealId);
			if (dealClosing == null) {
				dealClosing = new DealClosing();
				CreateDealClosing(dealId, out dealClosing);
			}
			return dealClosing;
		}
		#endregion

		#region DealReport

		//
		// GET: /Deal/Report
		[HttpGet]
		public ActionResult Report() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["PageName"] = "DealReport";
			return View();
		}

		//
		// GET: /Deal/DealReportList
		[HttpGet]
		public JsonResult DealReportList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DealReportModel> deals = DealRepository.GetAllReportDeals(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { deal.DealId, deal.DealNumber.ToString() + ".", deal.DealName, deal.FundName, deal.SellerName }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealUnderlyingDetails
		[HttpGet]
		public JsonResult DealUnderlyingDetails(int dealId) {
			DealUnderlyingDetail underlyingDetail = new DealUnderlyingDetail();
			underlyingDetail.DealUnderlyingFunds = DealRepository.GetAllDealUnderlyingFunds(dealId);
			underlyingDetail.DealUnderlyingDirects = DealRepository.GetAllDealUnderlyingDirects(dealId);
			return Json(underlyingDetail, JsonRequestBehavior.AllowGet);
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
