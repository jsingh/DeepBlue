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

				// Attempt to create deal.

				Models.Entity.Deal deal = DealRepository.FindDeal(model.DealId);
				if (deal == null) {
					deal = new Models.Entity.Deal();
					deal.CreatedBy = AppSettings.CreatedByUserId;
					deal.CreatedDate = DateTime.Now;
					deal.DealNumber = DealRepository.GetMaxDealNumber(model.FundId);
				}

				deal.EntityID = (int)ConfigUtil.CurrentEntityID;
				deal.LastUpdatedBy = AppSettings.CreatedByUserId;
				deal.LastUpdatedDate = DateTime.Now;
				deal.DealName = model.DealName;
				deal.FundID = model.FundId;
				deal.IsPartnered = model.IsPartnered;
				deal.PurchaseTypeID = model.PurchaseTypeId;

				if (deal.IsPartnered) {

					// Attempt to create deal partner.

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

		//
		// GET: /Deal/DealdNameAvailable
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
		// GET: /Deal/GetDealDetail
		[HttpGet]
		public JsonResult GetDealDetail(int id) {
			return Json(DealRepository.GetDealDetail(id), JsonRequestBehavior.AllowGet);
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

				// Attempt to create deal expense.

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

		#region DealSellerInfo

		//
		// POST: /Deal/CreateSellerInfo
		[HttpPost]
		public ActionResult CreateSellerInfo(FormCollection collection) {
			DealSellerDetailModel model = new DealSellerDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {

				// Attempt to create deal seller information.

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

					// Attempt to create communication values.

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

				// Attempt to create deal underlying fund.

				DealUnderlyingFund dealUnderlyingFund = DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId ?? 0);
				if (dealUnderlyingFund == null) {
					dealUnderlyingFund = new DealUnderlyingFund();
				}
				dealUnderlyingFund.DealID = model.DealId;
				dealUnderlyingFund.Percent = model.Percent;
				dealUnderlyingFund.RecordDate = model.RecordDate;
				dealUnderlyingFund.UnderlyingFundID = model.UnderlyingFundId;
				dealUnderlyingFund.GrossPurchasePrice = model.GrossPurchasePrice;
				dealUnderlyingFund.ReassignedGPP = model.ReassignedGPP;
				dealUnderlyingFund.CommittedAmount = model.CommittedAmount;
				dealUnderlyingFund.UnfundedAmount = model.UnfundedAmount;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
				if (errorInfo == null) {

					// Check the user changes the fund navigation.

					UnderlyingFundNAV underlyingFundNAV = DealRepository.FindUnderlyingFundNAV(model.UnderlyingFundId, model.FundId);
					bool isCreateFundNAV = false;

					if (underlyingFundNAV == null)
						isCreateFundNAV = true;		// Create new underlying fund valuation.
					else if (underlyingFundNAV.FundNAV != model.FundNAV)
						isCreateFundNAV = true; 	// If the user changes the fund navigation then update underlying fund valuation.

					if (isCreateFundNAV)
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
		// POST: /Deal/CreateDealUnderlyingDirect
		[HttpPost]
		public ActionResult CreateDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model = new DealUnderlyingDirectModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {

				// Attempt to create deal underlying direct

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
				if (errorInfo == null) {

					// Check the user changes the purchase price.

					UnderlyingDirectLastPrice underlyingDirectLastPrice = DealRepository.FindUnderlyingDirectLastPrice(model.FundId, model.SecurityId, model.SecurityTypeId);
					bool isCreateLastPrice = false;
					if (underlyingDirectLastPrice == null)
						isCreateLastPrice = true;
					else if (underlyingDirectLastPrice.LastPrice != model.PurchasePrice)
						isCreateLastPrice = true;

					// If the user changes the purchase price then create underlying direct valuation.

					if (isCreateLastPrice)
						errorInfo = SaveUnderlyingDirectValuation(model.FundId, model.SecurityId, model.SecurityTypeId, model.PurchasePrice, DateTime.Now, out underlyingDirectLastPrice);
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
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

		//
		// GET: /Deal/FindDealUnderlyingDirects
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

		//
		// GET: /Deal/GetSecurity
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
		// GET: /Deal/Close
		[HttpGet]
		public ActionResult Close() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["PageName"] = "CloseDeal";
			CreateDealCloseModel model = new CreateDealCloseModel();
			return View(model);
		}

		//
		// GET: /Deal/GetDealCloseDetails
		[HttpGet]
		public JsonResult GetDealCloseDetails(int id, int dealId) {
			return Json(DealRepository.FindDealClosingModel(id, dealId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/GetFianlDealCloseDetails
		[HttpGet]
		public JsonResult GetFianlDealCloseDetails(int dealId) {
			return Json(DealRepository.GetFinalDealClosingModel(dealId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/UpdateDealClosing
		[HttpPost]
		public ActionResult UpdateDealClosing(FormCollection collection) {
			CreateDealCloseModel model = new CreateDealCloseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			string[] dealUnderlyingFundIds = (collection["DealUnderlyingFundId"] == null ? string.Empty : collection["DealUnderlyingFundId"]).ToString().Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			string[] dealUnderlyingDirectIds = (collection["DealUnderlyingDirectId"] == null ? string.Empty : collection["DealUnderlyingDirectId"]).ToString().Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			if (model.DealNumber == 0) {
				ModelState.AddModelError("DealNumber", "Deal Close Number is required");
			}
			if (ModelState.IsValid) {

				// Attempt to create deal closing.

				DealClosing dealClosing = DealRepository.FindDealClosing(model.DealClosingId);
				if (dealClosing == null) {
					dealClosing = new DealClosing();
					dealClosing.DealNumber = DealRepository.GetMaxDealClosingNumber(model.DealId);
				}
				dealClosing.CloseDate = model.CloseDate;
				dealClosing.DealID = model.DealId;
				dealClosing.IsFinalClose = model.IsFinalClose;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealClosing(dealClosing);
				if (errorInfo == null) {
					List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetDealUnderlyingFunds(dealClosing.DealID);
					foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
						if (dealUnderlyingFundIds.Contains(dealUnderlyingFund.DealUnderlyingtFundID.ToString())) {

							// Update deal underlying fund changes.

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

							// Update deal underlying direct changes.

							dealUnderlyingDirect.NumberOfShares = DataTypeHelper.ToInt32(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "NumberOfShares"]);
							dealUnderlyingDirect.PurchasePrice = DataTypeHelper.ToDecimal(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "PurchasePrice"]);
							dealUnderlyingDirect.FMV = dealUnderlyingDirect.NumberOfShares * dealUnderlyingDirect.PurchasePrice;
							dealUnderlyingDirect.DealClosingID = dealClosing.DealClosingID;

						}
						else if (dealUnderlyingDirect.DealClosingID == dealClosing.DealClosingID) {
							dealUnderlyingDirect.DealClosingID = null;
						}
						errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
						if (errorInfo != null) {
							break;
						}
						else {

							// Check the user changes the purchase price.

							UnderlyingDirectLastPrice underlyingDirectLastPrice = DealRepository.FindUnderlyingDirectLastPrice(model.FundId, dealUnderlyingDirect.SecurityID, dealUnderlyingDirect.SecurityTypeID);
							bool isCreateLastPrice = false;
							if (underlyingDirectLastPrice == null)
								isCreateLastPrice = true;
							else if (underlyingDirectLastPrice.LastPrice != dealUnderlyingDirect.PurchasePrice)
								isCreateLastPrice = true;

							// If the user changes the purchase price then create underlying direct valuation.

							if (isCreateLastPrice)
								errorInfo = SaveUnderlyingDirectValuation(model.FundId, dealUnderlyingDirect.SecurityID, dealUnderlyingDirect.SecurityTypeID, dealUnderlyingDirect.PurchasePrice, DateTime.Now, out underlyingDirectLastPrice);
						}
					}
				}
				if (errorInfo == null) {
					resultModel.Result = "True||" + dealClosing.DealClosingID;
				}
				else {
					resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
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
		// GET: /Deal/UpdateFinalDealClosing
		[HttpPost]
		public ActionResult UpdateFinalDealClosing(FormCollection collection) {
			CreateDealCloseModel model = new CreateDealCloseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();

			// Attempt to create final deal closing.

			if (ModelState.IsValid) {

				IEnumerable<ErrorInfo> errorInfo = null;

				List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllDealClosingUnderlyingFunds(model.DealId);
				foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
					// Update deal underlying fund changes.
					dealUnderlyingFund.PostRecordDateCapitalCall = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "PostRecordDateCapitalCall"]);
					dealUnderlyingFund.PostRecordDateDistribution = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "PostRecordDateDistribution"]);
					dealUnderlyingFund.ReassignedGPP = DataTypeHelper.ToDecimal(collection[dealUnderlyingFund.DealUnderlyingtFundID.ToString() + "_" + "ReassignedGPP"]);
					errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
					if (errorInfo != null)
						break;
				}

				List<DealUnderlyingDirect> dealUnderlyingDirects = DealRepository.GetAllDealClosingUnderlyingDirects(model.DealId);
				foreach (var dealUnderlyingDirect in dealUnderlyingDirects) {
					// Update deal underlying direct changes.
					dealUnderlyingDirect.NumberOfShares = DataTypeHelper.ToInt32(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "NumberOfShares"]);
					dealUnderlyingDirect.PurchasePrice = DataTypeHelper.ToDecimal(collection[dealUnderlyingDirect.DealUnderlyingDirectID.ToString() + "_" + "PurchasePrice"]);
					dealUnderlyingDirect.FMV = dealUnderlyingDirect.NumberOfShares * dealUnderlyingDirect.PurchasePrice;
					errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
					if (errorInfo != null) {
						break;
					}
					else {

						// Check the user changes the purchase price.

						UnderlyingDirectLastPrice underlyingDirectLastPrice = DealRepository.FindUnderlyingDirectLastPrice(model.FundId, dealUnderlyingDirect.SecurityID, dealUnderlyingDirect.SecurityTypeID);
						bool isCreateLastPrice = false;
						if (underlyingDirectLastPrice == null)
							isCreateLastPrice = true;
						else if (underlyingDirectLastPrice.LastPrice != dealUnderlyingDirect.PurchasePrice)
							isCreateLastPrice = true;

						// If the user changes the purchase price then create underlying direct valuation.

						if (isCreateLastPrice)
							errorInfo = SaveUnderlyingDirectValuation(model.FundId, dealUnderlyingDirect.SecurityID, dealUnderlyingDirect.SecurityTypeID, dealUnderlyingDirect.PurchasePrice, DateTime.Now, out underlyingDirectLastPrice);
					}
				}

				if (errorInfo == null) {
					List<DealClosing> dealClosings = DealRepository.GetAllDealClosing(model.DealId);
					foreach (var dealClose in dealClosings) {
						dealClose.CloseDate = model.CloseDate;
						dealClose.IsFinalClose = model.IsFinalClose;
						errorInfo = DealRepository.SaveDealClosing(dealClose);
						if (errorInfo != null)
							break;
					}
				}

				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
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
		// GET: /Deal/DealCloseDateAvailable
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
					cell = new List<object> { dealClose.DealClosingId, dealClose.DealNumber, dealClose.DealCloseName, dealClose.CloseDate.ToString("MM/dd/yyyy"), FormatHelper.CurrencyFormat(dealClose.TotalNetPurchasePrice) }
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
					cell = new List<object> { deal.DealId, deal.DealNumber.ToString() + ".", deal.DealName, deal.FundName, FormatHelper.CurrencyFormat(deal.CommittedAmount), FormatHelper.CurrencyFormat(deal.UnfundedAmount), FormatHelper.CurrencyFormat(deal.TotalAmount) }
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

		[HttpGet]
		public ActionResult UnderlyingFunds() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "UnderlyingFunds";
			ViewData["PageName"] = "UnderlyingFunds";
			CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
			model.UnderlyingFundTypes = SelectListFactory.GetUnderlyingFundTypeSelectList(AdminRepository.GetAllUnderlyingFundTypes());
			model.ReportingTypes = SelectListFactory.GetReportingTypeSelectList(AdminRepository.GetAllReportingTypes());
			model.Reportings = SelectListFactory.GetReportingFrequencySelectList(AdminRepository.GetAllReportingFrequencies());
			model.Industries = SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
			model.Geographyes = SelectListFactory.GetGeographySelectList(AdminRepository.GetAllGeographies());
			model.FundStructures = SelectListFactory.GetEmptySelectList();
			model.InvestmentTypes = SelectListFactory.GetInvestmentTypeSelectList(AdminRepository.GetAllInvestmentTypes());
			model.ManagerContacts = SelectListFactory.GetEmptySelectList();
			model.FundRegisteredOffices = SelectListFactory.GetEmptySelectList();
			return View(model);
		}

		[HttpGet]
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
			model.FundStructures = SelectListFactory.GetEmptySelectList();
			model.InvestmentTypes = SelectListFactory.GetInvestmentTypeSelectList(AdminRepository.GetAllInvestmentTypes());
			model.ManagerContacts = SelectListFactory.GetEmptySelectList();
			model.FundRegisteredOffices = SelectListFactory.GetEmptySelectList();
			return View(model);
		}

		[HttpGet]
		public ActionResult FindUnderlyingFund(int issuerId) {
			CreateUnderlyingFundModel model = DealRepository.FindUnderlyingFundModel(issuerId);
			if (model == null) {
				model = new CreateUnderlyingFundModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult UpdateUnderlyingFund(FormCollection collection) {
			collection["TaxRate"] = (collection["TaxRate"] != null ? collection["TaxRate"].Replace("%", "") : string.Empty);
			collection["ManagementFee"] = (collection["ManagementFee"] != null ? collection["ManagementFee"].Replace("%", "") : string.Empty);
			collection["IncentiveFee"] = (collection["IncentiveFee"] != null ? collection["IncentiveFee"].Replace("%", "") : string.Empty);
			CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
			this.TryUpdateModel(model, collection);
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

				if (string.IsNullOrEmpty(model.BankName) == false && string.IsNullOrEmpty(model.Account) == false) {
					if (underlyingFund.Account == null) {
						underlyingFund.Account = new Account();
						underlyingFund.Account.CreatedBy = AppSettings.CreatedByUserId;
						underlyingFund.Account.CreatedDate = DateTime.Now;
					}
					underlyingFund.Account.LastUpdatedBy = AppSettings.CreatedByUserId;
					underlyingFund.Account.LastUpdatedDate = DateTime.Now;
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
				}

				if (underlyingFund.Contact == null) {
					underlyingFund.Contact = new Contact();
					underlyingFund.Contact.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFund.Contact.CreatedDate = DateTime.Now;
				}
				underlyingFund.Contact.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFund.Contact.LastUpdatedDate = DateTime.Now;
				underlyingFund.Contact.EntityID = (int)ConfigUtil.CurrentEntityID;
				underlyingFund.Contact.ContactName = model.ContactName;
				underlyingFund.Contact.LastName = "n/a";
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.Email, model.Email);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress);
				AddCommunication(underlyingFund.Contact, Models.Admin.Enums.CommunicationType.MailingAddress, model.Address);
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveUnderlyingFund(underlyingFund);
				if (errorInfo != null) {
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				}
				else {
					resultModel.Result = "True||" + underlyingFund.UnderlyingtFundID;
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
		public JsonResult FindEquityDirects(string term) {
			return Json(IssuerRepository.FindEquityDirects(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeDirects
		[HttpGet]
		public JsonResult FindFixedIncomeDirects(string term) {
			return Json(IssuerRepository.FindFixedIncomeDirects(term), JsonRequestBehavior.AllowGet);
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
					List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.ActivityTypeId, model.SecurityTypeId, equitySplit.EquityID);
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
					List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.ActivityTypeId, model.NewSecurityTypeId, model.NewSecurityId);
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
		// POST : /Deal/CreateUnderlyingFundCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundCashDistribution(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundCashDistributionModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;

			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				resultModel.Result = string.Empty;
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCashDistributionModel));
				model = new UnderlyingFundCashDistributionModel();
				this.TryUpdateModel(model, rowCollection);
				if (model.Amount > 0) {
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any()) {
						foreach (var err in errorInfo) {
							if (string.IsNullOrEmpty(err.ErrorMessage) == false)
								resultModel.Result += rowIndex + "_" + err.PropertyName + "||" + err.ErrorMessage + "\n";
						}
						break;
					}
				}
			}

			if (string.IsNullOrEmpty(resultModel.Result)) {
				for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
					resultModel.Result = string.Empty;
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCashDistributionModel));
					model = new UnderlyingFundCashDistributionModel();
					this.TryUpdateModel(model, rowCollection);
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundCashDistribution(model);
					}
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistributionModel model) {
			IEnumerable<ErrorInfo> errorInfo = null;
			// Attempt to create underlying fund cash distribution.

			UnderlyingFundCashDistribution underlyingFundCashDistribution = null;
			if (model.UnderlyingFundCashDistributionId > 0)
				underlyingFundCashDistribution = DealRepository.FindUnderlyingFundCashDistribution(model.UnderlyingFundCashDistributionId);
			if (underlyingFundCashDistribution == null) {
				underlyingFundCashDistribution = new UnderlyingFundCashDistribution();
				underlyingFundCashDistribution.CreatedBy = AppSettings.CreatedByUserId;
				underlyingFundCashDistribution.CreatedDate = DateTime.Now;
			}
			underlyingFundCashDistribution.UnderlyingFundID = model.UnderlyingFundId;
			underlyingFundCashDistribution.Amount = model.Amount ?? 0;
			underlyingFundCashDistribution.CashDistributionTypeID = model.CashDistributionTypeId ?? 0;
			underlyingFundCashDistribution.IsPostRecordDateTransaction = false;
			underlyingFundCashDistribution.FundID = model.FundId;
			underlyingFundCashDistribution.NoticeDate = model.NoticeDate;
			underlyingFundCashDistribution.PaidDate = model.PaidDate;
			underlyingFundCashDistribution.ReceivedDate = model.ReceivedDate;
			underlyingFundCashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
			underlyingFundCashDistribution.LastUpdatedDate = DateTime.Now;
			errorInfo = DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
			if (errorInfo == null) {

				// Attempt to create cash distribution to each deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllClosingDealUnderlyingFunds(underlyingFundCashDistribution.UnderlyingFundID, underlyingFundCashDistribution.FundID);
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

					// Assign Underlying Fund Cash Distribution.
					cashDistribution.UnderluingFundCashDistributionID = underlyingFundCashDistribution.UnderlyingFundCashDistributionID;

					// Calculate distribution amount = (Deal Underlying Fund Committed Amount) / (Total Deal Underlying Fund Committed Amount) * Total Cash Distribution Amount.
					cashDistribution.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / (dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0))) * underlyingFundCashDistribution.Amount;

					cashDistribution.UnderlyingFundID = underlyingFundCashDistribution.UnderlyingFundID;
					cashDistribution.DealID = dealUnderlyingFund.DealID;
					errorInfo = DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
				}
			}
			return errorInfo;
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
		// POST : /Deal/CreateUnderlyingFundPostRecordCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCashDistribution(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundPostRecordCashDistributionModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;

			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				resultModel.Result = string.Empty;
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCashDistributionModel));
				model = new UnderlyingFundPostRecordCashDistributionModel();
				this.TryUpdateModel(model, rowCollection);
				if (model.Amount > 0) {
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any()) {
						foreach (var err in errorInfo) {
							if (string.IsNullOrEmpty(err.ErrorMessage) == false)
								resultModel.Result += rowIndex + "_" + err.PropertyName + "||" + err.ErrorMessage + "\n";
						}
						break;
					}
				}
			}

			if (string.IsNullOrEmpty(resultModel.Result)) {
				for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
					resultModel.Result = string.Empty;
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCashDistributionModel));
					model = new UnderlyingFundPostRecordCashDistributionModel();
					this.TryUpdateModel(model, rowCollection);
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundPostRecordCashDistribution(model);
					}
				}
			}

			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCashDistribution(UnderlyingFundPostRecordCashDistributionModel model) {
			IEnumerable<ErrorInfo> errorInfo = null;
			// Attempt to create post record cash distribution.

			CashDistribution cashDistribution = null;
			if (model.CashDistributionId > 0)
				cashDistribution = DealRepository.FindUnderlyingFundPostRecordCashDistribution(model.CashDistributionId);
			if (cashDistribution == null) {
				cashDistribution = new CashDistribution();
				cashDistribution.CreatedBy = AppSettings.CreatedByUserId;
				cashDistribution.CreatedDate = DateTime.Now;
			}
			cashDistribution.UnderlyingFundID = model.UnderlyingFundId;
			cashDistribution.Amount = model.Amount ?? 0;
			cashDistribution.DealID = model.DealId;
			cashDistribution.DistributionDate = model.DistributionDate;
			cashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
			cashDistribution.LastUpdatedDate = DateTime.Now;

			//Assign null value in Underlying Fund Cash Distribution.
			cashDistribution.UnderluingFundCashDistributionID = null;

			errorInfo = DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
			if (errorInfo == null) {
				// Update post record date distribution amount to deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllNotClosingDealUnderlyingFunds(cashDistribution.UnderlyingFundID, cashDistribution.DealID);
				foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
					if (dealUnderlyingFund.DealClosingID == null) {
						dealUnderlyingFund.PostRecordDateDistribution = DealRepository.GetSumOfCashDistribution(cashDistribution.UnderlyingFundID, cashDistribution.DealID);
						errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if (errorInfo != null)
							break;
					}
				}
			}
			return errorInfo;
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
		// POST : /Deal/CreateUnderlyingFundCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundCapitalCall(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundCapitalCallModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;
			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				resultModel.Result = string.Empty;
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCapitalCallModel));
				model = new UnderlyingFundCapitalCallModel();
				this.TryUpdateModel(model, rowCollection);
				if (model.Amount > 0) {
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any()) {
						foreach (var err in errorInfo) {
							if (string.IsNullOrEmpty(err.ErrorMessage) == false)
								resultModel.Result += rowIndex + "_" + err.PropertyName + "||" + err.ErrorMessage + "\n";
						}
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(resultModel.Result)) {
				for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCapitalCallModel));
					model = new UnderlyingFundCapitalCallModel();
					this.TryUpdateModel(model, rowCollection);
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundCapitalCall(model);
						if (errorInfo != null)
							resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return View("Result", resultModel);
		}


		private IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCallModel model) {
			IEnumerable<ErrorInfo> errorInfo = null;
			// Attempt to create underlying fund capital call.
			UnderlyingFundCapitalCall underlyingFundCapitalCall = null;
			if (model.UnderlyingFundCapitalCallId > 0)
				underlyingFundCapitalCall = DealRepository.FindUnderlyingFundCapitalCall(model.UnderlyingFundCapitalCallId);
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
			errorInfo = DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);
			if (errorInfo == null) {

				// Attempt to create capital call line item to each deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllClosingDealUnderlyingFunds(underlyingFundCapitalCall.UnderlyingFundID, underlyingFundCapitalCall.FundID);
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

					// Assign underlying fund capital call.
					underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCallID = underlyingFundCapitalCall.UnderlyingFundCapitalCallID;

					// Calculate capital call amount = (Deal Underlying Fund Committed Amount) / (Total Deal Underlying Fund Committed Amount) * Total Capital Call Amount.

					underlyingFundCapitalCallLineItem.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / (dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0))) * underlyingFundCapitalCall.Amount;

					underlyingFundCapitalCallLineItem.UnderlyingFundID = underlyingFundCapitalCall.UnderlyingFundID;
					underlyingFundCapitalCallLineItem.DealID = dealUnderlyingFund.DealID;

					// Update capital call amount to deal underlying fund and reduce unfunded amount.

					dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.UnfundedAmount - underlyingFundCapitalCallLineItem.Amount;

					errorInfo = DealRepository.SaveUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCallLineItem);

					if (errorInfo == null)
						errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);

					if (errorInfo != null)
						break;
				}

			}
			return errorInfo;
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
		// POST : /Deal/CreateUnderlyingFundPostRecordCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCapitalCall(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundPostRecordCapitalCallModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;

			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				resultModel.Result = string.Empty;
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCapitalCallModel));
				model = new UnderlyingFundPostRecordCapitalCallModel();
				this.TryUpdateModel(model, rowCollection);
				if (model.Amount > 0) {
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any()) {
						foreach (var err in errorInfo) {
							if (string.IsNullOrEmpty(err.ErrorMessage) == false)
								resultModel.Result += rowIndex + "_" + err.PropertyName + "||" + err.ErrorMessage + "\n";
						}
						break;
					}
				}
			}

			if (string.IsNullOrEmpty(resultModel.Result)) {
				for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
					resultModel.Result = string.Empty;
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCapitalCallModel));
					model = new UnderlyingFundPostRecordCapitalCallModel();
					this.TryUpdateModel(model, rowCollection);
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundPostRecordCapitalCall(model);
					}
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCapitalCall(UnderlyingFundPostRecordCapitalCallModel model) {
			IEnumerable<ErrorInfo> errorInfo = null;
			// Attempt to create post record capital call.

			UnderlyingFundCapitalCallLineItem capitalCallLineItem = null;
			if (model.UnderlyingFundCapitalCallLineItemId > 0) {
				capitalCallLineItem = DealRepository.FindUnderlyingFundPostRecordCapitalCall(model.UnderlyingFundCapitalCallLineItemId);
			}
			if (capitalCallLineItem == null) {
				capitalCallLineItem = new UnderlyingFundCapitalCallLineItem();
				capitalCallLineItem.CreatedBy = AppSettings.CreatedByUserId;
				capitalCallLineItem.CreatedDate = DateTime.Now;
			}
			capitalCallLineItem.UnderlyingFundID = model.UnderlyingFundId;
			capitalCallLineItem.Amount = model.Amount ?? 0;
			capitalCallLineItem.CapitalCallDate = model.CapitalCallDate;
			capitalCallLineItem.DealID = model.DealId;
			capitalCallLineItem.LastUpdatedBy = AppSettings.CreatedByUserId;
			capitalCallLineItem.LastUpdatedDate = DateTime.Now;
			errorInfo = DealRepository.SaveUnderlyingFundPostRecordCapitalCall(capitalCallLineItem);
			if (errorInfo == null) {

				// Update post record date capital call amount to deal underlying fund and reduce unfunded amount.

				List<DealUnderlyingFund> dealUnderlyingFunds = DealRepository.GetAllNotClosingDealUnderlyingFunds(capitalCallLineItem.UnderlyingFundID, capitalCallLineItem.DealID);
				foreach (var dealUnderlyingFund in dealUnderlyingFunds) {
					if (dealUnderlyingFund.DealClosingID == null && dealUnderlyingFund.DealID == capitalCallLineItem.DealID) {
						dealUnderlyingFund.PostRecordDateCapitalCall = DealRepository.GetSumOfUnderlyingFundCapitalCallLineItem(dealUnderlyingFund.UnderlyingFundID, dealUnderlyingFund.DealID);
						dealUnderlyingFund.UnfundedAmount = dealUnderlyingFund.UnfundedAmount - capitalCallLineItem.Amount;
						errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if (errorInfo != null)
							break;
					}
				}
			}

			return errorInfo;
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
		public JsonResult FindUnderlyingFundValuation(int underlyingFundId, int fundId) {
			UnderlyingFundValuationModel model = DealRepository.FindUnderlyingFundValuationModel(underlyingFundId, fundId);
			if (model == null) {
				model = new UnderlyingFundValuationModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
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
					resultModel.Result = "True||" + underlyingFundNAV.UnderlyingFundNAVID + "||" + underlyingFundNAV.FundID;
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

		private UnderlyingFundNAV CreateUnderlyingFundValuation(int underlyingFundId, int fundId, decimal fundNAV, DateTime fundNAVDate, out IEnumerable<ErrorInfo> errorInfo) {

			// Attempt to create deal underlying fund valuation.

			UnderlyingFundNAV underlyingFundNAV = DealRepository.FindUnderlyingFundNAV(underlyingFundId, fundId);
			decimal existingFundNAV = 0;
			DateTime existingFundNAVDate = Convert.ToDateTime("01/01/1900");
			if (underlyingFundNAV == null) {
				underlyingFundNAV = new UnderlyingFundNAV();
				underlyingFundNAV.CreatedBy = AppSettings.CreatedByUserId;
				underlyingFundNAV.CreatedDate = DateTime.Now;
			}
			else {
				existingFundNAV = underlyingFundNAV.FundNAV ?? 0;
				existingFundNAVDate = underlyingFundNAV.FundNAVDate;
			}
			underlyingFundNAV.UnderlyingFundID = underlyingFundId;
			underlyingFundNAV.FundID = fundId;
			underlyingFundNAV.FundNAV = fundNAV;
			underlyingFundNAV.FundNAVDate = fundNAVDate;
			underlyingFundNAV.LastUpdatedBy = AppSettings.CreatedByUserId;
			underlyingFundNAV.LastUpdatedDate = DateTime.Now;
			errorInfo = DealRepository.SaveUnderlyingFundNAV(underlyingFundNAV);
			if (errorInfo == null) {

				// Attempt to create underlying fund navigation history.

				UnderlyingFundNAVHistory underlyingFundNAVHistory = new UnderlyingFundNAVHistory();
				underlyingFundNAVHistory.UnderlyingFundNAVID = underlyingFundNAV.UnderlyingFundNAVID;
				underlyingFundNAVHistory.FundNAV = underlyingFundNAV.FundNAV;
				underlyingFundNAVHistory.FundNAVDate = underlyingFundNAV.FundNAVDate;
				underlyingFundNAVHistory.Calculation = null;
				underlyingFundNAVHistory.IsAudited = false;
				underlyingFundNAVHistory.CreatedBy = AppSettings.CreatedByUserId;
				underlyingFundNAVHistory.CreatedDate = DateTime.Now;
				underlyingFundNAVHistory.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFundNAVHistory.LastUpdatedDate = DateTime.Now;

				if (existingFundNAV == underlyingFundNAV.FundNAV
					&& existingFundNAVDate == underlyingFundNAV.FundNAVDate) {
					underlyingFundNAVHistory.Calculation = underlyingFundNAV.FundNAV + ":" + DealRepository.SumOfTotalCapitalCalls(underlyingFundNAV.UnderlyingFundID, underlyingFundNAV.FundID) + ":" + DealRepository.SumOfTotalDistributions(underlyingFundNAV.UnderlyingFundID, underlyingFundNAV.FundID);
				}
				errorInfo = DealRepository.SaveUnderlyingFundNAVHistory(underlyingFundNAVHistory);
			}
			return underlyingFundNAV;
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

		//
		// GET: /Deal/FindFundNAV
		[HttpGet]
		public decimal FindFundNAV(int underlyingFundId, int fundId) {
			return DealRepository.FindFundNAV(underlyingFundId, fundId);
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

				// Attempt to create fund expense.

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
		public ActionResult NewHoldingPatternList(int activityTypeId, int securityTypeId, int securityId) {
			List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(activityTypeId, securityTypeId, securityId);
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

		#region UnderlyingDirectValuation

		//
		// GET: /Deal/UnderlyingDirectValuationList
		[HttpGet]
		public JsonResult UnderlyingDirectValuationList(int issuerId) {
			return Json(DealRepository.UnderlyingDirectValuationList(issuerId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingDirectValuation
		[HttpPost]
		public ActionResult CreateUnderlyingDirectValuation(FormCollection collection) {
			FormCollection rowCollection;
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int index = 0; string[] values; string value;
			ResultModel resultModel = new ResultModel();
			for (index = 0; index < totalRows; index++) {
				rowCollection = new FormCollection();
				foreach (string key in collection.Keys) {
					values = collection[key].Split((",").ToCharArray());
					if (values.Length > index)
						value = values[index];
					else
						value = string.Empty;
					rowCollection.Add(key, value);
				}
				SaveUnderlyingDirectValuation(rowCollection);
			}
			return View("Result", resultModel);
		}

		private ResultModel SaveUnderlyingDirectValuation(FormCollection collection) {
			UnderlyingDirectValuationModel model = new UnderlyingDirectValuationModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo;
				UnderlyingDirectLastPrice underlyingDirectLastPrice;
				errorInfo = SaveUnderlyingDirectValuation(model.FundId, model.SecurityId, model.SecurityTypeId, (model.NewPrice ?? 0), model.NewPriceDate, out underlyingDirectLastPrice);
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + underlyingDirectLastPrice.UnderlyingDirectLastPriceID;
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
			return resultModel;
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingDirectValuation(int fundId, int securityId, int securityTypeId, decimal newPrice, DateTime newPriceDate,
			out UnderlyingDirectLastPrice underlyingDirectLastPrice) {
			// Attempt to create underlying direct valuation.
			underlyingDirectLastPrice = DealRepository.FindUnderlyingDirectLastPrice(fundId, securityId, securityTypeId);
			IEnumerable<ErrorInfo> errorInfo;
			if (underlyingDirectLastPrice == null) {
				underlyingDirectLastPrice = new UnderlyingDirectLastPrice();
				underlyingDirectLastPrice.CreatedBy = AppSettings.CreatedByUserId;
				underlyingDirectLastPrice.CreatedDate = DateTime.Now;
			}
			underlyingDirectLastPrice.FundID = fundId;
			underlyingDirectLastPrice.SecurityID = securityId;
			underlyingDirectLastPrice.SecurityTypeID = securityTypeId;
			underlyingDirectLastPrice.LastPrice = newPrice;
			underlyingDirectLastPrice.LastPriceDate = newPriceDate;
			underlyingDirectLastPrice.LastUpdatedBy = AppSettings.CreatedByUserId;
			underlyingDirectLastPrice.LastUpdatedDate = DateTime.Now;
			errorInfo = DealRepository.SaveUnderlyingDirectValuation(underlyingDirectLastPrice);
			if (errorInfo == null) {
				// Attempt to create underlying direct valuation history.
				UnderlyingDirectLastPriceHistory lastPricehistory = new UnderlyingDirectLastPriceHistory();
				lastPricehistory.UnderlyingDirectLastPriceID = underlyingDirectLastPrice.UnderlyingDirectLastPriceID;
				lastPricehistory.LastPrice = underlyingDirectLastPrice.LastPrice;
				lastPricehistory.LastPriceDate = underlyingDirectLastPrice.LastPriceDate;
				lastPricehistory.LastUpdatedBy = AppSettings.CreatedByUserId;
				lastPricehistory.LastUpdatedDate = DateTime.Now;
				lastPricehistory.CreatedBy = AppSettings.CreatedByUserId;
				lastPricehistory.CreatedDate = DateTime.Now;
				errorInfo = DealRepository.SaveUnderlyingDirectValuationHistory(lastPricehistory);
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindUnderlyingDirectValuation
		[HttpGet]
		public JsonResult FindUnderlyingDirectValuation(int id) {
			UnderlyingDirectValuationModel model = DealRepository.FindUnderlyingDirectValuationModel(id);
			if (model == null) {
				model = new UnderlyingDirectValuationModel();
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindLastPurchasePrice
		[HttpGet]
		public decimal FindLastPurchasePrice(int fundId, int securityId, int securityTypeId) {
			return DealRepository.FindLastPurchasePrice(fundId, securityId, securityTypeId);
		}

		[HttpGet]
		public string DeleteUnderlyingDirectValuation(int id) {
			if (DealRepository.DeleteUnderlyingDirectValuation(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region UnfundedAdjustment

		//
		// GET: /Deal/UnfundedAdjustmentList
		[HttpGet]
		public JsonResult UnfundedAdjustmentList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnfundedAdjustments(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnfundedAdjustment
		[HttpPost]
		public ActionResult CreateUnfundedAdjustment(FormCollection collection) {
			FormCollection rowCollection;
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int index = 0; string[] values; string value;
			ResultModel resultModel = new ResultModel();
			for (index = 0; index < totalRows; index++) {
				rowCollection = new FormCollection();
				foreach (string key in collection.Keys) {
					values = collection[key].Split((",").ToCharArray());
					if (values.Length > index)
						value = values[index];
					else
						value = string.Empty;
					rowCollection.Add(key, value);
				}
				SaveUnfundedAdjustment(rowCollection);
			}
			return View("Result", resultModel);
		}

		private ResultModel SaveUnfundedAdjustment(FormCollection collection) {
			UnfundedAdjustmentModel model = new UnfundedAdjustmentModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = ValidationHelper.Validate(model);
			if (errorInfo.Any() == false) {
				DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment = DealRepository.FindDealUnderlyingFundAdjustment(model.DealUnderlyingFundId);
				if (dealUnderlyingFundAdjustment == null) {
					dealUnderlyingFundAdjustment = new DealUnderlyingFundAdjustment();
					dealUnderlyingFundAdjustment.CreatedBy = AppSettings.CreatedByUserId;
					dealUnderlyingFundAdjustment.CreatedDate = DateTime.Now;
				}
				dealUnderlyingFundAdjustment.LastUpdatedBy = AppSettings.CreatedByUserId;
				dealUnderlyingFundAdjustment.LastUpdatedDate = DateTime.Now;
				dealUnderlyingFundAdjustment.CommitmentAmount = model.CommitmentAmount;
				dealUnderlyingFundAdjustment.UnfundedAmount = model.UnfundedAmount;
				dealUnderlyingFundAdjustment.DealUnderlyingFundID = model.DealUnderlyingFundId;
				errorInfo = DealRepository.SaveDealUnderlyingFundAdjustment(dealUnderlyingFundAdjustment);
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			}
			else {
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			}
			return resultModel;
		}

		[HttpGet]
		public string DeleteUnfundedAdjustment(int id) {
			if (DealRepository.DeleteUnfundedAdjustment(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#endregion

		#region Reconcile

		//
		// GET: /Deal/Reconcile
		[HttpGet]
		public ActionResult Reconcile() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Reconcile";
			ViewData["PageName"] = "Reconcile";
			return View(new ReconcileModel());
		}

		//
		// POST: /Deal/ReconcileList
		[HttpPost]
		public JsonResult ReconcileList(FormCollection collection) {
			ReconcileModel model = new ReconcileModel();
			this.TryUpdateModel(model, collection);
			int totalRows = 0;
			var allReconciles = DealRepository.GetAllReconciles(model.PageIndex, model.PageSize, (model.FromDate ?? Convert.ToDateTime("01/01/1900")), (model.ToDate ?? DateTime.MaxValue), (model.UnderlyingFundId ?? 0), (model.FundId ?? 0), ref totalRows).ToList();
			return Json(new { Total = totalRows, Results = allReconciles }, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Directs

		//
		// GET: /Deal/Directs
		[HttpGet]
		public ActionResult Directs() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "AddDirects";
			ViewData["PageName"] = "AddDirects";
			CreateIssuerModel model = new CreateIssuerModel();
			model.EquityDetailModel.Currencies = SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
			model.EquityDetailModel.ShareClassTypes = SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
			model.EquityDetailModel.EquityTypes = SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
			model.FixedIncomeDetailModel.Currencies = model.EquityDetailModel.Currencies;
			model.IssuerDetailModel.IssuerRatings = SelectListFactory.GetEmptySelectList();
			model.FixedIncomeDetailModel.FixedIncomeTypes = SelectListFactory.GetFixedIncomeTypesSelectList(AdminRepository.GetAllFixedIncomeTypes());
			return View(model);
		}

		//
		// GET: /Deal/FindIssuer
		[HttpGet]
		public JsonResult FindIssuer(int id) {
			return Json(DealRepository.FindIssuerModel(id), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindIssuers
		[HttpGet]
		public JsonResult FindIssuers(string term) {
			return Json(DealRepository.FindIssuers(term), JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateIssuer
		[HttpPost]
		public ActionResult CreateIssuer(FormCollection collection) {
			IssuerDetailModel model = new IssuerDetailModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = IssuerNameAvailable(model.Name, model.IssuerId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.Issuer issuer = null;
				IEnumerable<ErrorInfo> errorInfo = SaveIssuer(model, out issuer);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + issuer.IssuerID + "||" + issuer.Name;
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
		// POST: /Deal/UpdateIssuer
		[HttpPost]
		public ActionResult UpdateIssuer(FormCollection collection) {
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;

			IssuerDetailModel model = new IssuerDetailModel();
			this.TryUpdateModel(model, collection);
			errorInfo = ValidationHelper.Validate(model);
			resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
			string ErrorMessage = IssuerNameAvailable(model.Name, model.IssuerId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				resultModel.Result += ErrorMessage + "\n";
			}
			if (string.IsNullOrEmpty(resultModel.Result)) {
				Models.Entity.Issuer issuer = null;
				errorInfo = SaveIssuer(model, out issuer);
				if (errorInfo == null) {
					EquityDetailModel equityDetailModel = new EquityDetailModel();
					this.TryUpdateModel(equityDetailModel, collection);
					if (equityDetailModel.EquityTypeId > 0) {
						equityDetailModel.CurrencyId = DataTypeHelper.ToInt32(collection["EQ_CurrencyId"]);
						equityDetailModel.IndustryId = DataTypeHelper.ToInt32(collection["EQ_IndustryId"]);
						equityDetailModel.Symbol = collection["EQ_Symbol"];
						errorInfo = ValidationHelper.Validate(equityDetailModel);
						if (errorInfo.Any() == false) {
							errorInfo = SaveEquity(equityDetailModel);
						}
					}
				}
				if (errorInfo == null) {
					FixedIncomeDetailModel fixedIncomeDetailModel = new FixedIncomeDetailModel();
					this.TryUpdateModel(fixedIncomeDetailModel, collection);
					if (fixedIncomeDetailModel.FixedIncomeTypeId > 0) {
						fixedIncomeDetailModel.CurrencyId = DataTypeHelper.ToInt32(collection["FI_CurrencyId"]);
						fixedIncomeDetailModel.IndustryId = DataTypeHelper.ToInt32(collection["FI_IndustryId"]);
						fixedIncomeDetailModel.Symbol = collection["FI_Symbol"];
						errorInfo = ValidationHelper.Validate(fixedIncomeDetailModel);
						if (errorInfo.Any() == false) {
							errorInfo = SaveFixedIncome(fixedIncomeDetailModel);
						}
					}
				}
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + issuer.IssuerID + "||" + issuer.Name;
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveEquity(EquityDetailModel model) {
			Models.Entity.Equity equity = IssuerRepository.FindEquity(model.EquityId);
			if (equity == null) {
				equity = new Models.Entity.Equity();
			}
			equity.CurrencyID = ((model.CurrencyId ?? 0) > 0 ? model.CurrencyId : null);
			equity.EquityTypeID = model.EquityTypeId;
			equity.IndustryID = ((model.IndustryId ?? 0) > 0 ? model.IndustryId : null);
			equity.IssuerID = model.IssuerId;
			equity.Public = model.Public;
			equity.ShareClassTypeID = ((model.ShareClassTypeId ?? 0) > 0 ? model.ShareClassTypeId : null);
			equity.Symbol = model.Symbol;
			equity.EntityID = (int)ConfigUtil.CurrentEntityID;
			return IssuerRepository.SaveEquity(equity);
		}

		private IEnumerable<ErrorInfo> SaveFixedIncome(FixedIncomeDetailModel model) {
			Models.Entity.FixedIncome fixedIncome = IssuerRepository.FindFixedIncome(model.FixedIncomeId);
			if (fixedIncome == null) {
				fixedIncome = new Models.Entity.FixedIncome();
			}
			fixedIncome.CurrencyID = ((model.CurrencyId ?? 0) > 0 ? model.CurrencyId : null);
			fixedIncome.FixedIncomeTypeID = model.FixedIncomeTypeId;
			fixedIncome.IndustryID = ((model.IndustryId ?? 0) > 0 ? model.IndustryId : null);
			fixedIncome.IssuerID = model.IssuerId;
			fixedIncome.Symbol = model.Symbol;
			fixedIncome.EntityID = (int)ConfigUtil.CurrentEntityID;
			fixedIncome.Maturity = model.Maturity;
			fixedIncome.IssuedDate = model.IssuedDate;
			fixedIncome.Frequency = model.Frequency;
			fixedIncome.FirstCouponDate = model.FirstCouponDate;
			fixedIncome.FirstAccrualDate = model.FirstAccrualDate;
			fixedIncome.FaceValue = model.FaceValue;
			fixedIncome.CouponInformation = model.CouponInformation;
			return IssuerRepository.SaveFixedIncome(fixedIncome);
		}

		private IEnumerable<ErrorInfo> SaveIssuer(IssuerDetailModel model, out Models.Entity.Issuer issuer) {
			issuer = IssuerRepository.FindIssuer(model.IssuerId);
			if (issuer == null) {
				issuer = new Models.Entity.Issuer();
			}
			issuer.Name = model.Name;
			issuer.ParentName = model.ParentName;
			issuer.CountryID = model.CountryId;
			issuer.EntityID = (int)ConfigUtil.CurrentEntityID;
			return IssuerRepository.SaveIssuer(issuer);
		}

		[HttpGet]
		public string IssuerNameAvailable(string Name, int IssuerID) {
			if (IssuerRepository.IssuerNameAvailable(Name, IssuerID))
				return "Name already exist";
			else
				return string.Empty;
		}

		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
