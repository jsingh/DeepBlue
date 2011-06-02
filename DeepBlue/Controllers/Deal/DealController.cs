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
using DeepBlue.Models.Deal.Enums;
using System.Globalization;

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
			model.Issuers.Add(new SelectListItem { Selected = false, Text = "--Add Issuer--", Value = "-1" });
			model.SecurityTypes = SelectListFactory.GetSecurityTypeSelectList(AdminRepository.GetAllSecurityTypes());
			model.Securities = SelectListFactory.GetEmptySelectList();
			return model;
		}

		//
		// GET: /Deal/DealList
		[HttpGet]
		public ActionResult DealList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DealListModel> deals = DealRepository.GetAllDeals(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { deal.DealId, deal.DealName, deal.FundName }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			DealDetailModel model = new DealDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = DealdNameAvailable(model.DealName, model.DealId, model.FundId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("DealName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.Deal deal = DealRepository.FindDeal(model.DealId);
				if (deal == null) {
					deal = new Models.Entity.Deal();
					deal.CreatedBy = AppSettings.CreatedByUserId;
					deal.CreatedDate = DateTime.Now;
					deal.DealNumber = DealRepository.GetMaxDealNumber(model.FundId);
				}
				deal.EntityID = (int)ConfigUtil.CurrentEntityID;
				deal.DealName = model.DealName;
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
		public string DealdNameAvailable(string DealName, int DealId, int FundId) {
			if (DealRepository.DealNameAvailable(DealName, DealId, FundId))
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
			deal.DealNumber = DealRepository.GetMaxDealNumber(fundId);
			return Json(deal, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeal/5
		public JsonResult FindDeal(int dealId) {
			DealDetailModel dealDetail = DealRepository.FindDealDetail(dealId);
			foreach (var dealUnderlyingDirect in dealDetail.DealUnderlyingDirects) {
				dealUnderlyingDirect.Equities = SelectListFactory.GetEquitySelectList(IssuerRepository.GetAllEquity(dealUnderlyingDirect.IssuerId));
				dealUnderlyingDirect.FixedIncomes = SelectListFactory.GetFixedIncomeSelectList(IssuerRepository.GetAllFixedIncome(dealUnderlyingDirect.IssuerId));
			}
			return Json(dealDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeals
		[HttpGet]
		public JsonResult FindDeals(string term) {
			return Json(DealRepository.FindDeals(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFundDeals
		[HttpGet]
		public JsonResult FindFundDeals(int fundId, string term) {
			return Json(DealRepository.FindDeals(fundId, term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region DealExpense
		//
		// GET: /Deal/CreateDealExpense
		[HttpPost]
		public ActionResult CreateDealExpense(FormCollection collection) {
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
			return View("Result", resultModel);
		}

		//
		// GET: /Deal/FindDealClosingCost/1
		[HttpGet]
		public JsonResult FindDealClosingCost(int dealClosingCostId) {
			return Json(DealRepository.FindDealClosingCostModel(dealClosingCostId), JsonRequestBehavior.AllowGet);
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
		public ActionResult CreateSellerInfo(FormCollection collection) {
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
					AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.Company, model.CompanyName);
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
			return View("Result", resultModel);
		}
		#endregion

		#region DealUnderlyingFund
		//
		// GET: /Deal/CreateDealUnderlyingFund
		[HttpPost]
		public ActionResult CreateDealUnderlyingFund(FormCollection collection) {
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
				dealUnderlyingFund.Percent = model.Percent;
				dealUnderlyingFund.RecordDate = model.RecordDate;
				dealUnderlyingFund.UnderlyingFundID = model.UnderlyingFundId;
				dealUnderlyingFund.GrossPurchasePrice = model.GrossPurchasePrice;
				dealUnderlyingFund.ReassignedGPP = model.ReassignedGPP;
				dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.CommittedAmount - DealRepository.GetSumOfUnderlyingFundCapitalCallLineItem(dealUnderlyingFund.UnderlyingFundID, dealUnderlyingFund.DealID);
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
				if (errorInfo == null) {
					// Create Underlying Fund NAV 
					CreateUnderlyingFundValuation(dealUnderlyingFund.UnderlyingFundID, model.FundId, (model.FundNAV ?? 0), DateTime.Now, out errorInfo);
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
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
			return View("Result", resultModel);
		}

		//
		// GET: /Deal/FindDealUnderlyingFund/1
		[HttpGet]
		public JsonResult FindDealUnderlyingFund(int dealUnderlyingFundId) {
			return Json(DealRepository.FindDealUnderlyingFundModel(dealUnderlyingFundId), JsonRequestBehavior.AllowGet);
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
		public ActionResult CreateDealUnderlyingDirect(FormCollection collection) {
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
				dealUnderlyingDirect.SecurityID = model.SecurityId;
				dealUnderlyingDirect.SecurityTypeID = model.SecurityTypeId;
				dealUnderlyingDirect.TaxCostDate = model.TaxCostDate;
				dealUnderlyingDirect.TaxCostBase = model.TaxCostBase;
				dealUnderlyingDirect.PurchasePrice = model.PurchasePrice;
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
			return View("Result", resultModel);
		}

		//
		// GET: /Deal/FindDealUnderlyingDirect/1
		[HttpGet]
		public JsonResult FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			DealUnderlyingDirectModel model = DealRepository.FindDealUnderlyingDirectModel(dealUnderlyingDirectId);
			model.Equities = SelectListFactory.GetEquitySelectList(IssuerRepository.GetAllEquity(model.IssuerId));
			model.FixedIncomes = SelectListFactory.GetFixedIncomeSelectList(IssuerRepository.GetAllFixedIncome(model.IssuerId));
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult FindDealUnderlyingDirects(string term) {
			return Json(DealRepository.FindDealUnderlyingDirects(term), JsonRequestBehavior.AllowGet);
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
		////
		//// GET: /Deal/CreateDealClosing
		//[HttpGet]
		//public string CreateDealClosing(int dealId, out DealClosing dealClosing) {
		//    string result = string.Empty;
		//    dealClosing = new DealClosing { DealID = dealId, CloseDate = DateTime.Now, DealClosingID = 0 };
		//    IEnumerable<ErrorInfo> errorInfo = dealClosing.Save();
		//    if (errorInfo != null) {
		//        foreach (var err in errorInfo.ToList()) {
		//            result += err.PropertyName + " : " + err.ErrorMessage + "\n";
		//        }
		//    }
		//    return result;
		//}

		//
		// GET: /Deal/FindDealClosing
		//[HttpGet]
		//private DealClosing FindDealClosing(int dealId) {
		//    DealClosing dealClosing = DealRepository.FindDealClosing(dealId);
		//    if (dealClosing == null) {
		//        dealClosing = new DealClosing();
		//        CreateDealClosing(dealId, out dealClosing);
		//    }
		//    return dealClosing;
		//}

		//
		// GET: /Deal/Close
		[HttpGet]
		public ActionResult Close() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["PageName"] = "CloseDeal";
			CreateDealCloseModel model = new CreateDealCloseModel();
			return View(model);
		}

		[HttpGet]
		public ActionResult EditDealClosing(int id, int dealId) {
			return View(DealRepository.FindDealClosingModel(id, dealId));
		}

		[HttpPost]
		public ActionResult UpdateDealClosing(FormCollection collection) {
			CreateDealCloseModel model = new CreateDealCloseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			string[] dealUnderlyingFundIds = (collection["DealUnderlyingFundId"] == null ? string.Empty : collection["DealUnderlyingFundId"]).ToString().Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			string[] dealUnderlyingDirectIds = (collection["DealUnderlyingDirectId"] == null ? string.Empty : collection["DealUnderlyingDirectId"]).ToString().Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			/*if (dealUnderlyingFundIds.Length <= 0) {
				ModelState.AddModelError("DealUnderlyingFunds", "Select any one deal underlying fund.");
			}
			if (dealUnderlyingDirectIds.Length <= 0) {
				ModelState.AddModelError("DealUnderlyingDirects", "Select any one deal underlying direct.");
			}*/
			string ErrorMessage = DealCloseDateAvailable(model.CloseDate ?? Convert.ToDateTime("01/01/1900"), model.DealId, model.DealClosingId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("CloseDate", ErrorMessage);
			}
			if (ModelState.IsValid) {
				DealClosing dealClosing = DealRepository.FindDealClosing(model.DealClosingId);
				if (dealClosing == null) {
					dealClosing = new DealClosing();
					dealClosing.DealNumber = DealRepository.GetMaxDealClosingNumber(model.DealId);
				}
				dealClosing.CloseDate = model.CloseDate ?? Convert.ToDateTime("01/01/1900");
				dealClosing.DealID = model.DealId;
				dealClosing.IsFinalClose = model.IsFinalClose;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealClosing(dealClosing);
				if (errorInfo == null) {
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(dealClosing.DealID);
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFundIds.Contains(dealUnderlyingFund.DealUnderlyingtFundID.ToString())) {
							dealUnderlyingFund.CommittedAmount = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "CommittedAmount"]);
							dealUnderlyingFund.GrossPurchasePrice = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "GrossPurchasePrice"]);
							dealUnderlyingFund.PostRecordDateCapitalCall = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "PostRecordDateCapitalCall"]);
							dealUnderlyingFund.PostRecordDateDistribution = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "PostRecordDateDistribution"]);
							dealUnderlyingFund.ReassignedGPP = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "ReassignedGPP"]);
							dealUnderlyingFund.DealClosingID = dealClosing.DealClosingID;
						}
						else if (dealUnderlyingFund.DealClosingID == dealClosing.DealClosingID) {
							dealUnderlyingFund.DealClosingID = null;
						}
						errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if (errorInfo != null)
							break;
					}
					List<DealUnderlyingDirect> dealUnderlyingDirects = DealRepository.GetDealUnderlyingDirects(dealClosing.DealID);
					foreach (var dealUnderlyingDirect in dealUnderlyingDirects) {
						if (dealUnderlyingDirectIds.Contains(dealUnderlyingDirect.DealUnderlyingDirectID.ToString())) {
							dealUnderlyingDirect.NumberOfShares = DataTypeHelper.ToInt32(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "NumberOfShares"]);
							dealUnderlyingDirect.PurchasePrice = DataTypeHelper.ToDecimal(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "PurchasePrice"]);
							dealUnderlyingDirect.FMV = DataTypeHelper.ToDecimal(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "FMV"]);
							dealUnderlyingDirect.DealClosingID = null;
							dealUnderlyingDirect.DealClosingID = dealClosing.DealClosingID;
						}
						else if (dealUnderlyingDirect.DealClosingID == dealClosing.DealClosingID) {
							dealUnderlyingDirect.DealClosingID = null;
						}
						errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
						if (errorInfo != null)
							break;
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
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
		public string DealCloseDateAvailable(DateTime dealCloseDate, int dealId, int dealCloseId) {
			if (DealRepository.DealCloseDateAvailable(dealCloseDate, dealId, dealCloseId))
				return "Close Date already exists.";
			else
				return string.Empty;
		}

		//
		// GET: /Deal/DealClosingList
		[HttpGet]
		public JsonResult DealClosingList(int pageIndex, int pageSize, string sortName, string sortOrder, int dealId) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DealCloseListModel> dealClosings = DealRepository.GetAllDealClosingLists(pageIndex, pageSize, sortName, sortOrder, ref totalRows, dealId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var dealClose in dealClosings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { dealClose.DealClosingId, dealClose.DealName, dealClose.FundName, dealClose.CloseDate.ToString("MM/dd/yyyy") }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
		public JsonResult DealReportList(int pageIndex, int pageSize, string sortName, string sortOrder, int fundId) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DealReportModel> deals = DealRepository.GetAllReportDeals(pageIndex, pageSize, sortName, sortOrder, ref totalRows, fundId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { deal.DealId, deal.DealNumber.ToString() + ".", deal.DealName, deal.FundName, (deal.SellerName == null ? string.Empty : deal.SellerName) }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealUnderlyingDetails
		[HttpGet]
		public JsonResult DealUnderlyingDetails(int dealId) {
			DealUnderlyingDetail underlyingDetail = new DealUnderlyingDetail();
			underlyingDetail.DealUnderlyingFunds = DealRepository.GetAllDealUnderlyingFundDetails(dealId);
			underlyingDetail.DealUnderlyingDirects = DealRepository.GetAllDealUnderlyingDirects(dealId);
			return Json(underlyingDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealUnderlyingDetails
		[HttpGet]
		public ActionResult Export(int fundId, int exportTypeId, string sortName, string sortOrder) {
			Uri requestUrl = HttpContext.Request.Url;
			string url = string.Format("{0}://{1}{2}",
												  requestUrl.Scheme,
												  requestUrl.Authority,
												  "/Deal/ExportDetail?FundId=" + fundId
												  + "&SortName=" + sortName
												  + "&SortOrder=" + sortOrder);
			ActionResult result = null;
			switch ((Models.Deal.Enums.ExportType)exportTypeId) {
				case ExportType.Word:
					result = new ExportWordResult(url, "DealReport.doc");
					break;
				/*case ExportType.Pdf:
					result = new ExportPdfResult(url, "DealReport.pdf");
					break; */
			}
			return result;
		}

		//
		// GET: /Deal/ExportDetail
		[HttpGet]
		public ActionResult ExportDetail(FormCollection collection) {
			DealExportModel model = new DealExportModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.Deals = DealRepository.GetAllExportDeals(model.SortName, model.SortOrder, model.FundId);
			}
			return View(model);
		}

		#endregion

		#region UnderlyingFund
		public ActionResult UnderlyingFunds() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "UnderlyingFund";
			return View();
		}

		public ActionResult EditUnderlyingFund(int? id) {
			CreateUnderlyingFundModel model = DealRepository.FindUnderlyingFundModel(id ?? 0);
			if (model == null) {
				model = new CreateUnderlyingFundModel();
			}
			model.UnderlyingFundTypes = SelectListFactory.GetUnderlyingFundTypeSelectList(AdminRepository.GetAllUnderlyingFundTypes());
			model.ReportingTypes = SelectListFactory.GetReportingTypeSelectList(AdminRepository.GetAllReportingTypes());
			model.Reportings = SelectListFactory.GetReportingFrequencySelectList(AdminRepository.GetAllReportingFrequencies());
			model.Industries = SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
			model.Geographyes = SelectListFactory.GetGeographySelectList(AdminRepository.GetAllGeographies());
			model.Issuers = SelectListFactory.GetIssuerSelectList(IssuerRepository.GetAllIssuers());
			model.FundStructures = SelectListFactory.GetEmptySelectList();
			model.InvestmentTypes = SelectListFactory.GetInvestmentTypeSelectList(AdminRepository.GetAllInvestmentTypes());
			model.ManagerContacts = SelectListFactory.GetEmptySelectList();
			model.FundRegisteredOffices = SelectListFactory.GetEmptySelectList();
			return View(model);
		}

		[HttpPost]
		public ActionResult UpdateUnderlyingFund(FormCollection collection) {
			CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = UnderlyingFundNameAvailable(model.FundName, model.UnderlyingFundId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("FundName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				UnderlyingFund underlyingFund = DealRepository.FindUnderlyingFund(model.UnderlyingFundId);
				if (underlyingFund == null) {
					underlyingFund = new UnderlyingFund();
				}
				underlyingFund.EntityID = (int)ConfigUtil.CurrentEntityID;

				underlyingFund.IsFeesIncluded = model.IsFeesIncluded;
				underlyingFund.FundName = model.FundName;
				underlyingFund.FundTypeID = model.FundTypeId;
				underlyingFund.IssuerID = model.IssuerId;
				underlyingFund.VintageYear = model.VintageYear;
				underlyingFund.TotalSize = model.TotalSize;
				underlyingFund.TerminationYear = model.TerminationYear;
				underlyingFund.IncentiveFee = model.IncentiveFee;
				underlyingFund.LegalFundName = model.LegalFundName;
				underlyingFund.Description = model.Description;
				underlyingFund.FiscalYearEnd = model.FiscalYearEnd;
				underlyingFund.ManagementFee = model.ManagementFee;
				underlyingFund.Taxable = model.Taxable;
				underlyingFund.TaxRate = model.TaxRate;
				underlyingFund.AuditorName = model.AuditorName;
				underlyingFund.IsDomestic = model.IsDomestic;
				underlyingFund.Exempt = model.Exempt;
				underlyingFund.GeographyID = ((model.GeographyId ?? 0) > 0 ? model.GeographyId : null);
				underlyingFund.IndustryID = ((model.IndustryId ?? 0) > 0 ? model.IndustryId : null);
				underlyingFund.ReportingFrequencyID = ((model.ReportingFrequencyId ?? 0) > 0 ? model.ReportingFrequencyId : null);
				underlyingFund.ReportingTypeID = ((model.ReportingTypeId ?? 0) > 0 ? model.ReportingTypeId : null);
				underlyingFund.InvestmentTypeID = ((model.InvestmentTypeId ?? 0) > 0 ? model.InvestmentTypeId : null);
				underlyingFund.FundRegisteredOfficeID = ((model.FundRegisteredOfficeId ?? 0) > 0 ? model.FundRegisteredOfficeId : null);
				underlyingFund.FundStructureID = ((model.FundStructureId ?? 0) > 0 ? model.FundStructureId : null);
				underlyingFund.ManagerContactID = ((model.ManagerContactId ?? 0) > 0 ? model.ManagerContactId : null);
				if (underlyingFund.Account == null) {
					underlyingFund.Account = new Account();
					underlyingFund.Account.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFund.Account.CreatedDate = DateTime.Now;
				}
				underlyingFund.Account.BankName = model.BankName;
				underlyingFund.Account.AccountNumberCash = model.Account;
				underlyingFund.Account.Account1 = model.Account;
				underlyingFund.Account.AccountOf = model.AccountOf;
				underlyingFund.Account.Attention = model.Attention;
				underlyingFund.Account.EntityID = (int)ConfigUtil.CurrentEntityID;
				underlyingFund.Account.Reference = model.Reference;
				underlyingFund.Account.Routing = model.Routing;
				underlyingFund.Account.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFund.Account.LastUpdatedDate = DateTime.Now;
				if (underlyingFund.Contact == null) {
					underlyingFund.Contact = new Contact();
					underlyingFund.Contact.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFund.Contact.CreatedDate = DateTime.Now;
				}
				underlyingFund.Contact.EntityID = (int)ConfigUtil.CurrentEntityID;
				underlyingFund.Contact.ContactName = model.ContactName;
				underlyingFund.Contact.LastName = "n/a";

				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.Email, model.Email);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.MailingAddress, model.WebAddress);
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFund(underlyingFund);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
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
			return View("Result", resultModel);
		}

		[HttpGet]
		public string UnderlyingFundNameAvailable(string FundName, int UnderlyingFundId) {
			if (DealRepository.UnderlyingFundNameAvailable(FundName, UnderlyingFundId))
				return "Fund Name already exist";
			else
				return string.Empty;
		}

		[HttpGet]
		public JsonResult UnderlyingFundList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<UnderlyingFundListModel> underlyingFunds = DealRepository.GetAllUnderlyingFunds(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var underlyingFund in underlyingFunds) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { underlyingFund.UnderlyingFundId, underlyingFund.FundName, underlyingFund.FundType, underlyingFund.IssuerName }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult FindUnderlyingFunds(string term) {
			return Json(DealRepository.FindUnderlyingFunds(term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Activities

		public ActionResult Activities() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Activities";
			ViewData["PageName"] = "Activities";
			CreateActivityModel model = new CreateActivityModel();
			model.UnderlyingFundCashDistributionModel.CashDistributionTypes = SelectListFactory.GetCashDistributionTypeSelectList(AdminRepository.GetAllCashDistributionTypes());
			model.ActivityTypes = SelectListFactory.GetActivityTypeSelectList(AdminRepository.GetAllActivityTypes());
			List<Models.Entity.SecurityType> securityTypes = AdminRepository.GetAllSecurityTypes();
			model.EquitySplitModel.SecurityTypes = SelectListFactory.GetSecurityTypeSelectList(securityTypes);
			model.SecurityConversionModel.SecurityTypes = SelectListFactory.GetSecurityTypeSelectList(securityTypes);
			model.FundLevelExpenseModel.FundExpenseTypes = SelectListFactory.GetFundExpenseTypeSelectList(AdminRepository.GetAllFundExpenseTypes());
			return View(model);
		}

		#region SecurityActivities

		//
		// GET: /Deal/FindEquityDirects
		[HttpGet]
		public JsonResult FindEquityDirects(int dealUnderlyingDirectId, string term) {
			return Json(IssuerRepository.FindEquityDirects(dealUnderlyingDirectId, term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeDirects
		[HttpGet]
		public JsonResult FindFixedIncomeDirects(int dealUnderlyingDirectId, string term) {
			return Json(IssuerRepository.FindFixedIncomeDirects(dealUnderlyingDirectId, term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeSecurityConversionModel
		[HttpGet]
		public JsonResult FindFixedIncomeSecurityConversionModel(int fixedIncomeId) {
			return Json(IssuerRepository.FindFixedIncomeSecurityConversionModel(fixedIncomeId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquitySecurityConversionModel
		[HttpGet]
		public JsonResult FindEquitySecurityConversionModel(int equityId) {
			return Json(IssuerRepository.FindEquitySecurityConversionModel(equityId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateSplitActivity
		[HttpPost]
		public ActionResult CreateSplitActivity(FormCollection collection) {
			EquitySplitModel model = new EquitySplitModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				EquitySplit equitySplit = DealRepository.FindEquitySplit(model.EquityId);
				if (equitySplit == null) {
					equitySplit = new EquitySplit();
					equitySplit.CreatedBy = AppSettings.CreatedByUserId;
					equitySplit.CreatedDate = DateTime.Now;
				}
				equitySplit.LastUpdatedBy = AppSettings.CreatedByUserId;
				equitySplit.LastUpdatedDate = DateTime.Now;
				equitySplit.EquityID = model.EquityId;
				equitySplit.SplitFactor = model.SplitFactor ?? 0;
				equitySplit.SplitDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveEquitySplit(equitySplit);
				if (errorInfo == null) {
					List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.DealUnderlyingDirectId, model.ActivityTypeId,
						model.SecurityTypeId, equitySplit.EquityID);
					foreach (var pattern in newHoldingPatterns) {
						errorInfo = CreateFundActivityHistory(pattern.FundId, pattern.OldNoOfShares, (pattern.OldNoOfShares * equitySplit.SplitFactor), equitySplit.EquiteSplitID, model.ActivityTypeId);
						if (errorInfo != null)
							break;
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
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

		//
		// POST: /Deal/CreateConversionActivity
		[HttpPost]
		public ActionResult CreateConversionActivity(FormCollection collection) {
			SecurityConversionModel model = new SecurityConversionModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				SecurityConversion securityConversion = DealRepository.FindSecurityConversion(model.NewSecurityId, model.NewSecurityTypeId);
				if (securityConversion == null) {
					securityConversion = new SecurityConversion();
					securityConversion.CreatedBy = AppSettings.CreatedByUserId;
					securityConversion.CreatedDate = DateTime.Now;
				}
				securityConversion.OldSecurityID = model.OldSecurityId;
				securityConversion.OldSecurityTypeID = model.OldSecurityTypeId;
				securityConversion.NewSecurityID = model.NewSecurityId;
				securityConversion.NewSecurityTypeID = model.NewSecurityTypeId;
				securityConversion.LastUpdatedBy = AppSettings.CreatedByUserId;
				securityConversion.LastUpdatedDate = DateTime.Now;
				securityConversion.SplitFactor = model.SplitFactor ?? 0;
				securityConversion.ConversionDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveSecurityConversion(securityConversion);
				if (errorInfo == null) {
					List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.DealUnderlyingDirectId, model.ActivityTypeId, model.NewSecurityTypeId, model.NewSecurityId);
					foreach (var pattern in newHoldingPatterns) {
						errorInfo = CreateFundActivityHistory(pattern.FundId, pattern.OldNoOfShares, (pattern.OldNoOfShares * securityConversion.SplitFactor), securityConversion.SecurityConversionID, model.ActivityTypeId);
						if (errorInfo != null)
							break;
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
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

		private IEnumerable<ErrorInfo> CreateFundActivityHistory(int fundId, int oldNoOfShares, int newNoOfShares, int activityId, int activityTypeId) {
			FundActivityHistory fundActivityHistory = new FundActivityHistory();
			fundActivityHistory.ActivityID = activityId;
			fundActivityHistory.ActivityTypeID = activityTypeId;
			fundActivityHistory.FundID = fundId;
			fundActivityHistory.OldNumberOfShares = oldNoOfShares;
			fundActivityHistory.NewNumberOfShares = newNoOfShares;
			return DealRepository.SaveFundActivityHistory(fundActivityHistory);
		}

		#endregion

		#region UnderlyingFundCashDistribution

		//
		// GET: /Deal/FindUnderlyingFundCashDistribution
		[HttpGet]
		public JsonResult FindUnderlyingFundCashDistribution(int id) {
			UnderlyingFundCashDistributionModel model = DealRepository.FindUnderlyingFundCashDistributionModel(id);
			if (model == null) {
				model = new UnderlyingFundCashDistributionModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundCashDistribution(FormCollection collection) {
			UnderlyingFundCashDistributionModel model = new UnderlyingFundCashDistributionModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				UnderlyingFundCashDistribution underlyingFundCashDistribution = DealRepository.FindUnderlyingFundCashDistribution(model.UnderlyingFundCashDistributionId ?? 0);
				if (underlyingFundCashDistribution == null) {
					underlyingFundCashDistribution = new UnderlyingFundCashDistribution();
					underlyingFundCashDistribution.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFundCashDistribution.CreatedDate = DateTime.Now;
				}
				underlyingFundCashDistribution.UnderlyingFundID = model.UnderlyingFundId;
				underlyingFundCashDistribution.Amount = model.Amount ?? 0;
				underlyingFundCashDistribution.CashDistributionTypeID = model.CashDistributionTypeId;
				underlyingFundCashDistribution.IsPostRecordDateTransaction = model.IsPostRecordDateTransaction ?? false;
				underlyingFundCashDistribution.FundID = model.FundId;
				underlyingFundCashDistribution.NoticeDate = model.NoticeDate;
				underlyingFundCashDistribution.PaidDate = model.PaidDate;
				underlyingFundCashDistribution.ReceivedDate = model.ReceivedDate;
				underlyingFundCashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFundCashDistribution.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
				if (errorInfo == null) {
					// Create Cash Distribution
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllDealUnderlyingFunds(underlyingFundCashDistribution.UnderlyingFundID, underlyingFundCashDistribution.FundID);
					CashDistribution cashDistribution;
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						cashDistribution = DealRepository.FindUnderlyingFundPostRecordCashDistribution(underlyingFundCashDistribution.UnderlyingFundCashDistributionID,
																				underlyingFundCashDistribution.UnderlyingFundID,
																				dealUnderlyingFund.DealID);
						if (cashDistribution == null) {
							cashDistribution = new CashDistribution();
							cashDistribution.CreatedBy = AppSettings.CreatedByUserId;
							cashDistribution.CreatedDate = DateTime.Now;
						}
						cashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
						cashDistribution.LastUpdatedDate = DateTime.Now;
						cashDistribution.UnderluingFundCashDistributionID = underlyingFundCashDistribution.UnderlyingFundCashDistributionID;
						// Calculate distribution amount
						cashDistribution.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0)) * underlyingFundCashDistribution.Amount;
						cashDistribution.UnderlyingFundID = underlyingFundCashDistribution.UnderlyingFundID;
						cashDistribution.DealID = dealUnderlyingFund.DealID;
						errorInfo = DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
						if (errorInfo == null)
							errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						else
							break;
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + underlyingFundCashDistribution.UnderlyingFundCashDistributionID;
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

		//
		// GET: /Deal/UnderlyingFundCashDistributionList
		[HttpGet]
		public JsonResult UnderlyingFundCashDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundCashDistributions(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingFundCashDistribution
		[HttpGet]
		public string DeleteUnderlyingFundCashDistribution(int id) {
			if (DealRepository.DeleteUnderlyingFundCashDistribution(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundPostRecordCashDistribution

		//
		// GET: /Deal/UnderlyingFundPostRecordCashDistributionList
		[HttpGet]
		public ActionResult UnderlyingFundPostRecordCashDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundPostRecordCashDistributions(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindUnderlyingFundPostRecordCashDistribution
		[HttpGet]
		public JsonResult FindUnderlyingFundPostRecordCashDistribution(int id) {
			UnderlyingFundPostRecordCashDistributionModel model = DealRepository.FindUnderlyingFundPostRecordCashDistributionModel(id);
			if (model == null) {
				model = new UnderlyingFundPostRecordCashDistributionModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}


		//
		// POST : /Deal/CreateUnderlyingFundPostRecordCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCashDistribution(FormCollection collection) {
			UnderlyingFundPostRecordCashDistributionModel model = new UnderlyingFundPostRecordCashDistributionModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				CashDistribution cashDistribution = DealRepository.FindUnderlyingFundPostRecordCashDistribution(model.CashDistributionId);
				int existingDealId = 0;
				if (cashDistribution != null) {
					existingDealId = cashDistribution.DealID;
				}
				if (cashDistribution == null) {
					cashDistribution = new CashDistribution();
					cashDistribution.CreatedBy = AppSettings.CreatedByUserId;
					cashDistribution.CreatedDate = DateTime.Now;
				}
				cashDistribution.UnderlyingFundID = model.UnderlyingFundId;
				cashDistribution.Amount = model.Amount ?? 0;
				cashDistribution.DealID = model.DealId;
				cashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
				cashDistribution.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
				if (errorInfo == null) {
					//Create capital call line item
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(cashDistribution.UnderlyingFundID, cashDistribution.DealID);
					decimal distributionAmount = DealRepository.GetSumOfCashDistribution(cashDistribution.UnderlyingFundID, cashDistribution.DealID);
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFund.DealClosingID == null) {
							dealUnderlyingFund.PostRecordDateDistribution = distributionAmount;
							errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
							if (errorInfo != null)
								break;
						}
					}
					if (errorInfo == null) {
						// If DealId is change Then
						// Update existing underlying fund post record date distribution
						if (existingDealId > 0 && existingDealId != cashDistribution.DealID) {
							distributionAmount = DealRepository.GetSumOfCashDistribution(cashDistribution.UnderlyingFundID, existingDealId);
							dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(cashDistribution.UnderlyingFundID, existingDealId);
							foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
								if (dealUnderlyingFund.DealClosingID == null) {
									dealUnderlyingFund.PostRecordDateDistribution = distributionAmount;
									errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
									if (errorInfo != null)
										break;
								}
							}
						}
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + cashDistribution.CashDistributionID;
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

		//
		// GET: /Deal/DeleteUnderlyingFundPostRecordCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundPostRecordCashDistribution(int id) {
			if (DealRepository.DeleteUnderlyingFundPostRecordCashDistribution(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundCapitalCall

		//
		// GET: /Deal/FindUnderlyingFundCapitalCall
		[HttpGet]
		public JsonResult FindUnderlyingFundCapitalCall(int id) {
			UnderlyingFundCapitalCallModel model = DealRepository.FindUnderlyingFundCapitalCallModel(id);
			if (model == null) {
				model = new UnderlyingFundCapitalCallModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundCapitalCall(FormCollection collection) {
			UnderlyingFundCapitalCallModel model = new UnderlyingFundCapitalCallModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				UnderlyingFundCapitalCall underlyingFundCapitalCall = DealRepository.FindUnderlyingFundCapitalCall(model.UnderlyingFundCapitalCallId);
				if (underlyingFundCapitalCall == null) {
					underlyingFundCapitalCall = new UnderlyingFundCapitalCall();
					underlyingFundCapitalCall.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFundCapitalCall.CreatedDate = DateTime.Now;
				}
				underlyingFundCapitalCall.UnderlyingFundID = model.UnderlyingFundId;
				underlyingFundCapitalCall.Amount = model.Amount ?? 0;
				underlyingFundCapitalCall.IsPostRecordDateTransaction = model.IsPostRecordDateTransaction ?? false;
				underlyingFundCapitalCall.IsDeemedCapitalCall = model.IsDeemedCapitalCall ?? false;
				underlyingFundCapitalCall.FundID = model.FundId;
				underlyingFundCapitalCall.NoticeDate = model.NoticeDate;
				underlyingFundCapitalCall.ReceivedDate = model.ReceivedDate;
				underlyingFundCapitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFundCapitalCall.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);
				if (errorInfo == null) {
					//Create capital call line item
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllDealUnderlyingFunds(underlyingFundCapitalCall.UnderlyingFundID, underlyingFundCapitalCall.FundID);
					UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem;
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						underlyingFundCapitalCallLineItem = DealRepository.FindUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCall.UnderlyingFundCapitalCallID,
																				underlyingFundCapitalCall.UnderlyingFundID,
																				dealUnderlyingFund.DealID);
						if (underlyingFundCapitalCallLineItem == null) {
							underlyingFundCapitalCallLineItem = new UnderlyingFundCapitalCallLineItem();
							underlyingFundCapitalCallLineItem.CreatedBy = AppSettings.CreatedByUserId;
							underlyingFundCapitalCallLineItem.CreatedDate = DateTime.Now;
						}
						underlyingFundCapitalCallLineItem.LastUpdatedBy = AppSettings.CreatedByUserId;
						underlyingFundCapitalCallLineItem.LastUpdatedDate = DateTime.Now;

						underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCallID = underlyingFundCapitalCall.UnderlyingFundCapitalCallID;
						// Calculate capital call amount
						underlyingFundCapitalCallLineItem.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0)) * underlyingFundCapitalCall.Amount;
						underlyingFundCapitalCallLineItem.UnderlyingFundID = underlyingFundCapitalCall.UnderlyingFundID;
						underlyingFundCapitalCallLineItem.DealID = dealUnderlyingFund.DealID;
						errorInfo = DealRepository.SaveUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCallLineItem);
						if (errorInfo == null)
							errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						else
							break;
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + underlyingFundCapitalCall.UnderlyingFundCapitalCallID;
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

		//
		// GET: /Deal/UnderlyingFundCapitalCallList
		[HttpGet]
		public JsonResult UnderlyingFundCapitalCallList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundCapitalCalls(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingFundCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundCapitalCall(int id) {
			if (DealRepository.DeleteUnderlyingFundCapitalCall(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundPostRecordCapitalCall
		//
		// GET: /Deal/UnderlyingFundPostRecordCapitalCallList
		[HttpGet]
		public JsonResult UnderlyingFundPostRecordCapitalCallList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundPostRecordCapitalCalls(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindUnderlyingFundPostRecordCapitalCall
		[HttpGet]
		public JsonResult FindUnderlyingFundPostRecordCapitalCall(int id) {
			UnderlyingFundPostRecordCapitalCallModel model = DealRepository.FindUnderlyingFundPostRecordCapitalCallModel(id);
			if (model == null) {
				model = new UnderlyingFundPostRecordCapitalCallModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundPostRecordCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCapitalCall(FormCollection collection) {
			UnderlyingFundPostRecordCapitalCallModel model = new UnderlyingFundPostRecordCapitalCallModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				UnderlyingFundCapitalCallLineItem capitalCallLineItem = DealRepository.FindUnderlyingFundPostRecordCapitalCall(model.UnderlyingFundCapitalCallLineItemId);
				int existingDealId = 0;
				if (capitalCallLineItem != null) {
					existingDealId = capitalCallLineItem.DealID;
				}
				if (capitalCallLineItem == null) {
					capitalCallLineItem = new UnderlyingFundCapitalCallLineItem();
					capitalCallLineItem.CreatedBy = AppSettings.CreatedByUserId;
					capitalCallLineItem.CreatedDate = DateTime.Now;
				}
				capitalCallLineItem.UnderlyingFundID = model.UnderlyingFundId;
				capitalCallLineItem.Amount = model.Amount ?? 0;
				capitalCallLineItem.ReceivedDate = model.ReceivedDate;
				capitalCallLineItem.DealID = model.DealId;
				capitalCallLineItem.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCallLineItem.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFundPostRecordCapitalCall(capitalCallLineItem);
				if (errorInfo == null) {
					//Create capital call line item
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(capitalCallLineItem.UnderlyingFundID, capitalCallLineItem.DealID);
					decimal capitalCallAmount = DealRepository.GetSumOfUnderlyingFundCapitalCallLineItem(capitalCallLineItem.UnderlyingFundID, capitalCallLineItem.DealID);
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFund.DealClosingID == null) {
							dealUnderlyingFund.PostRecordDateCapitalCall = capitalCallAmount;
							dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.CommittedAmount - capitalCallAmount;
							errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
							if (errorInfo != null)
								break;
						}
					}
					if (errorInfo == null) {
						// If DealId is change Then
						// Update existing underlying fund post record date distribution
						if (existingDealId > 0 && existingDealId != capitalCallLineItem.DealID) {
							capitalCallAmount = DealRepository.GetSumOfUnderlyingFundCapitalCallLineItem(capitalCallLineItem.UnderlyingFundID, existingDealId);
							dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(capitalCallLineItem.UnderlyingFundID, existingDealId);
							foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
								if (dealUnderlyingFund.DealClosingID == null) {
									dealUnderlyingFund.PostRecordDateCapitalCall = capitalCallAmount;
									dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.CommittedAmount - capitalCallAmount;
									errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
									if (errorInfo != null)
										break;
								}
							}
						}
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + capitalCallLineItem.UnderlyingFundCapitalCallLineItemID;
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

		//
		// GET: /Deal/DeleteUnderlyingFundPostRecordCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundPostRecordCapitalCall(int id) {
			if (DealRepository.DeleteUnderlyingFundPostRecordCapitalCall(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}
		#endregion

		#region UnderlyingFundValuation
		//
		// GET: /Deal/UnderlyingFundValuationList
		[HttpGet]
		public JsonResult UnderlyingFundValuationList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundValuations(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindUnderlyingFundValuation
		[HttpGet]
		public JsonResult FindUnderlyingFundValuation(int id) {
			UnderlyingFundValuationModel model = DealRepository.FindUnderlyingFundValuationModel(id);
			if (model == null) {
				model = new UnderlyingFundValuationModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		private UnderlyingFundNAV CreateUnderlyingFundValuation(int underlyingFundId, int fundId, decimal fundNAV, DateTime fundNAVDate,
			out IEnumerable<ErrorInfo> errorInfo) {
			UnderlyingFundNAV underlyingFundNAV = DealRepository.FindUnderlyingFundNAV(underlyingFundId, fundId);
			decimal existingFundNAV = 0;
			DateTime existingFundNAVDate = Convert.ToDateTime("01/01/1900");
			int existingFundId = 0;
			int existingUnderlyingFundId = 0;
			if (underlyingFundNAV == null) {
				underlyingFundNAV = new UnderlyingFundNAV();
			}
			else {
				existingFundId = underlyingFundNAV.FundID;
				existingUnderlyingFundId = underlyingFundNAV.UnderlyingFundID;
				existingFundNAV = underlyingFundNAV.FundNAV ?? 0;
				existingFundNAVDate = underlyingFundNAV.FundNAVDate;
			}
			underlyingFundNAV.UnderlyingFundID = underlyingFundId;
			underlyingFundNAV.FundID = fundId;
			underlyingFundNAV.FundNAV = fundNAV;
			underlyingFundNAV.FundNAVDate = fundNAVDate;
			errorInfo = DealRepository.SaveUnderlyingFundNAV(underlyingFundNAV);
			if (errorInfo == null) {
				UnderlyingFundNAVHistory underlyingFundNAVHistory = new UnderlyingFundNAVHistory();
				underlyingFundNAVHistory.UnderlyingFundNAVID = underlyingFundNAV.UnderlyingFundNAVID;
				underlyingFundNAVHistory.FundNAV = underlyingFundNAV.FundNAV;
				underlyingFundNAVHistory.FundNAVDate = underlyingFundNAV.FundNAVDate;
				underlyingFundNAVHistory.Calculation = null;
				underlyingFundNAVHistory.IsAudited = false;
				if (existingFundNAV == underlyingFundNAV.FundNAV
					&& existingFundNAVDate == underlyingFundNAV.FundNAVDate
					&& existingFundId == underlyingFundNAV.FundID
					&& existingUnderlyingFundId == underlyingFundNAV.UnderlyingFundID) {
					underlyingFundNAVHistory.Calculation = underlyingFundNAV.FundNAV + ":" + DealRepository.SumOfTotalCapitalCalls(underlyingFundNAV.UnderlyingFundID, underlyingFundNAV.FundID) + ":" + DealRepository.SumOfTotalDistributions(underlyingFundNAV.UnderlyingFundID, underlyingFundNAV.FundID);
				}
				errorInfo = DealRepository.SaveUnderlyingFundNAVHistory(underlyingFundNAVHistory);
			}
			return underlyingFundNAV;
		}

		//
		// POST : /Deal/CreateUnderlyingFundValuation
		[HttpPost]
		public ActionResult CreateUnderlyingFundValuation(FormCollection collection) {
			UnderlyingFundValuationModel model = new UnderlyingFundValuationModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo;
				UnderlyingFundNAV underlyingFundNAV = CreateUnderlyingFundValuation(model.UnderlyingFundId, model.FundId, (model.UpdateNAV ?? 0), model.UpdateDate, out errorInfo);
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + underlyingFundNAV.UnderlyingFundNAVID;
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

		//
		// GET: /Deal/DeleteUnderlyingFundValuation
		[HttpGet]
		public string DeleteUnderlyingFundValuation(int id) {
			if (DealRepository.DeleteUnderlyingFundValuation(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region FundLevelExpense

		//
		// POST : /Deal/FundExpense
		[HttpPost]
		public ActionResult CreateFundExpense(FormCollection collection) {
			FundExpenseModel model = new FundExpenseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				FundExpense fundExpense = DealRepository.FindFundExpense(model.FundId);
				if (fundExpense == null) {
					fundExpense = new FundExpense();
				}
				fundExpense.FundID = model.FundId;
				fundExpense.FundExpenseTypeID = model.FundExpenseTypeId;
				fundExpense.Amount = model.Amount;
				fundExpense.Date = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveFundExpense(fundExpense);
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + fundExpense.FundExpenseID;
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

		//
		// POST : /Deal/FindFundExpense
		[HttpGet]
		public JsonResult FindFundExpense(int fundId) {
			return Json(DealRepository.FindFundExpenseModel(fundId), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region NewHoldingPattern
		//
		// GET: /Deal/NewHoldingPatternList
		public ActionResult NewHoldingPatternList(int dealUnderlyingDirectId, int activityTypeId, int securityTypeId, int securityId) {
			List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(dealUnderlyingDirectId, activityTypeId, securityTypeId, securityId);
			FlexigridData flexgridData = new FlexigridData();
			flexgridData.total = newHoldingPatterns.Count();
			flexgridData.page = 1;
			foreach (var pattern in newHoldingPatterns) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { pattern.FundName, (pattern.OldNoOfShares > 0 ? FormatHelper.NumberFormat(pattern.OldNoOfShares) : string.Empty), (pattern.NewNoOfShares > 0 ? FormatHelper.NumberFormat(pattern.NewNoOfShares) : string.Empty) }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquitySymbol
		public string FindEquitySymbol(int id) {
			return IssuerRepository.FindEquitySymbol(id);
		}

		#endregion

		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
