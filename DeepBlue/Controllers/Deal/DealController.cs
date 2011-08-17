using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using System.Text;
using DeepBlue.Models.Deal;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Admin;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Deal.Enums;
using DeepBlue.Controllers.CapitalCall;
using System.Web.UI;
using System.Net;

namespace DeepBlue.Controllers.Deal {
	public class DealController : Controller {

		public IDealRepository DealRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public IFundRepository FundRepository { get; set; }

		public DealController()
			: this(new DealRepository(), new AdminRepository(), new CapitalCallRepository(), new FundRepository()) {
		}

		public DealController(IDealRepository dealRepository, IAdminRepository adminRepository, ICapitalCallRepository capitalCallRepository, IFundRepository fundRepository) {
			DealRepository = dealRepository;
			AdminRepository = adminRepository;
			CapitalCallRepository = capitalCallRepository;
			FundRepository = fundRepository;
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
			model.PurchaseTypes = SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
			model.DealClosingCostTypes = SelectListFactory.GetDealClosingCostTypeSelectList(AdminRepository.GetAllDealClosingCostTypes());
			model.UnderlyingFunds = SelectListFactory.GetUnderlyingFundSelectList(DealRepository.GetAllUnderlyingFunds());
			model.Issuers = SelectListFactory.GetIssuerSelectList(DealRepository.GetAllIssuers());
			model.SecurityTypes = SelectListFactory.GetSecurityTypeSelectList(AdminRepository.GetAllSecurityTypes());
			model.Securities = SelectListFactory.GetEmptySelectList();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.UploadTypes = SelectListFactory.GetUploadTypeSelectList();
			model.DocumentStatusTypes = SelectListFactory.GetDealDocumentStatusList();
			model.DealId = DealRepository.FindLastDealId();
			Models.Fund.FundDetail dealFundDetail = FundRepository.FindLastFundDetail();
			if (dealFundDetail != null) {
				model.FundId = dealFundDetail.FundId;
				model.FundName = dealFundDetail.FundName;
			}
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
				dealUnderlyingDirect.Equities = SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(dealUnderlyingDirect.IssuerId));
				dealUnderlyingDirect.FixedIncomes = SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(dealUnderlyingDirect.IssuerId));
			}
			return Json(dealDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeals
		[HttpGet]
		public JsonResult FindDeals(string term, int? fundId) {
			return Json(DealRepository.FindDeals(term, fundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/GetDealDetail
		[HttpGet]
		public JsonResult GetDealDetail(int id) {
			return Json(DealRepository.GetDealDetail(id), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region DealExpense

		//
		// GET: /Deal/CreateDealExpense
		[HttpPost]
		public ActionResult CreateDealExpense(FormCollection collection) {
			DealClosingCostModel model = new DealClosingCostModel();
			this.TryUpdateModel(model, collection);
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

		#region DealDocument

		//
		// POST: /Document/CreateDealFundDocument
		[HttpPost]
		public string CreateDealFundDocument(FormCollection collection) {
			DealDocumentModel model = new DealDocumentModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;
			string documentUploadPath = string.Empty;
			int key = 0;
			switch ((DeepBlue.Models.Document.DocumentStatus)model.DocumentStatusId) {
				case Models.Document.DocumentStatus.Fund:
					if ((model.DocumentFundId ?? 0) <= 0) {
						ModelState.AddModelError("DocumentFundId", "Document For is required");
					}
					documentUploadPath = "DealFundDocumentUploadPath";
					key = model.DocumentFundId ?? 0;
					break;
				case Models.Document.DocumentStatus.Investor:
					if ((model.DocumentInvestorId ?? 0) <= 0) {
						ModelState.AddModelError("DocumentInvestorId", "Document For is required");
					}
					documentUploadPath = "DealInvestorDocumentUploadPath";
					key = model.DocumentInvestorId ?? 0;
					break;
			}
			if (ModelState.IsValid) {
				DealFundDocument dealFundDocument = DealRepository.FindDealFundDocument(model.DealFundDocumentId ?? 0);
				if (dealFundDocument == null) {
					dealFundDocument = new DealFundDocument();
					dealFundDocument.CreatedBy = AppSettings.CreatedByUserId;
					dealFundDocument.CreatedDate = DateTime.Now;
				}
				dealFundDocument.LastUpdatedBy = AppSettings.CreatedByUserId;
				dealFundDocument.LastUpdatedDate = DateTime.Now;
				dealFundDocument.EntityID = (int)ConfigUtil.CurrentEntityID;
				dealFundDocument.DocumentTypeID = model.DocumentTypeId;
				dealFundDocument.DocumentDate = model.DocumentDate;
				dealFundDocument.FundID = model.DocumentFundId;
				dealFundDocument.DealID = model.DealId;
				if (dealFundDocument.File == null) {
					dealFundDocument.File = new Models.Entity.File();
					dealFundDocument.File.CreatedBy = AppSettings.CreatedByUserId;
					dealFundDocument.File.CreatedDate = DateTime.Now;
				}
				DeepBlue.Models.Document.UploadType uploadType = (DeepBlue.Models.Document.UploadType)model.DocumentUploadTypeId;
				FileInfo fileInfo = null;
				resultModel.Result += CreateDocumentFile(dealFundDocument.File, uploadType, model.DocumentFilePath, Request.Files[0], ref fileInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					errorInfo = AdminRepository.SaveFile(dealFundDocument.File);
				}
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					if (dealFundDocument.File != null) {
						dealFundDocument.FileID = dealFundDocument.File.FileID;
					}
					errorInfo = DealRepository.SaveDealFundDocument(dealFundDocument);
					if (errorInfo == null) {
						if (uploadType == Models.Document.UploadType.Upload) {
							UploadFile fileUpload = new UploadFile(documentUploadPath);
							fileUpload.Move(fileInfo.FullName, (int)ConfigUtil.CurrentEntityID, dealFundDocument.DealID, key, dealFundDocument.DocumentTypeID, fileInfo.Name);
							dealFundDocument.File.FileName = fileUpload.FileName;
							dealFundDocument.File.FilePath = fileUpload.FilePath;
							dealFundDocument.File.Size = fileUpload.Size;
						}
						errorInfo = DealRepository.SaveDealFundDocument(dealFundDocument);
					}
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
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
			return JsonSerializer.ToJsonObject(new { error = string.Empty, data = resultModel.Result }).ToString();
		}

		//
		// GET: /Document/DealFundDocumentList
		[HttpGet]
		public ActionResult DealFundDocumentList(int dealId) {
			List<DealFundDocumentList> dealFundDocuments = DealRepository.GetAllDealFundDocuments(dealId);
			ViewData["TotalRows"] = dealFundDocuments.Count();
			ViewData["PageNo"] = 1;
			return View(dealFundDocuments);
		}

		//
		// GET: /Document/DeleteDealFundDocumentFile
		[HttpGet]
		public string DeleteDealFundDocumentFile(int id) {
			if (DealRepository.DeleteDealFundDocument(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region DealUnderlyingFund
		//
		// GET: /Deal/CreateDealUnderlyingFund
		[HttpPost]
		public ActionResult CreateDealUnderlyingFund(FormCollection collection) {
			DealUnderlyingFundModel model = new DealUnderlyingFundModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				DealUnderlyingFund dealUnderlyingFund = null;
				IEnumerable<ErrorInfo> errorInfo = SaveDealUnderlyingFund(model, out dealUnderlyingFund);
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

		private IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFundModel model, out DealUnderlyingFund dealUnderlyingFund) {
			// Attempt to create deal underlying fund.

			dealUnderlyingFund = DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId ?? 0);
			if (dealUnderlyingFund == null) {
				dealUnderlyingFund = new DealUnderlyingFund();
			}

			dealUnderlyingFund.CommittedAmount = model.CommittedAmount;
			dealUnderlyingFund.DealClosingID = model.DealClosingId;
			dealUnderlyingFund.DealID = model.DealId;
			dealUnderlyingFund.Percent = model.Percent;
			dealUnderlyingFund.GrossPurchasePrice = model.GrossPurchasePrice;
			dealUnderlyingFund.PostRecordDateCapitalCall = model.PostRecordDateCapitalCall;
			dealUnderlyingFund.PostRecordDateDistribution = model.PostRecordDateDistribution;
			dealUnderlyingFund.ReassignedGPP = model.ReassignedGPP;
			dealUnderlyingFund.RecordDate = model.RecordDate;
			dealUnderlyingFund.UnderlyingFundID = model.UnderlyingFundId;
			dealUnderlyingFund.UnfundedAmount = model.UnfundedAmount;

			dealUnderlyingFund.NetPurchasePrice = (dealUnderlyingFund.GrossPurchasePrice ?? 0) + (dealUnderlyingFund.PostRecordDateCapitalCall ?? 0) - (dealUnderlyingFund.PostRecordDateDistribution ?? 0);
			dealUnderlyingFund.AdjustedCost = (dealUnderlyingFund.ReassignedGPP ?? 0) + (dealUnderlyingFund.PostRecordDateCapitalCall ?? 0) - (dealUnderlyingFund.PostRecordDateDistribution ?? 0);

			IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);

			if (errorInfo == null) {
				if ((model.FundNAV ?? 0) > 0) {
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
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindDealUnderlyingFund
		[HttpGet]
		public JsonResult FindDealUnderlyingFund(int dealUnderlyingFundId) {
			return Json(DealRepository.FindDealUnderlyingFundModel(dealUnderlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteDealUnderlyingFund/1
		[HttpGet]
		public string DeleteDealUnderlyingFund(int id) {
			if (DealRepository.DeleteDealUnderlyingFund(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		#endregion

		#region DealUnderlyingDirect

		//
		// POST: /Deal/CreateDealUnderlyingDirect
		[HttpPost]
		public ActionResult CreateDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model = new DealUnderlyingDirectModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {

				DealUnderlyingDirect dealUnderlyingDirect = null;

				IEnumerable<ErrorInfo> errorInfo = SaveDealUnderlyingDirect(model, out dealUnderlyingDirect);

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

		private IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirectModel model, out DealUnderlyingDirect dealUnderlyingDirect) {
			// Attempt to create deal underlying direct

			dealUnderlyingDirect = DealRepository.FindDealUnderlyingDirect(model.DealUnderlyingDirectId ?? 0);
			if (dealUnderlyingDirect == null) {
				dealUnderlyingDirect = new DealUnderlyingDirect();
			}

			dealUnderlyingDirect.AdjustedFMV = model.AdjustedFMV;
			dealUnderlyingDirect.DealClosingID = model.DealClosingId;
			dealUnderlyingDirect.DealID = model.DealId;
			dealUnderlyingDirect.FMV = model.FMV;
			dealUnderlyingDirect.NumberOfShares = model.NumberOfShares;
			dealUnderlyingDirect.Percent = model.Percent;
			dealUnderlyingDirect.PurchasePrice = model.PurchasePrice;
			dealUnderlyingDirect.RecordDate = model.RecordDate;
			dealUnderlyingDirect.SecurityID = model.SecurityId;
			dealUnderlyingDirect.SecurityTypeID = model.SecurityTypeId;
			dealUnderlyingDirect.TaxCostBase = model.TaxCostBase;
			dealUnderlyingDirect.TaxCostDate = model.TaxCostDate;

			IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
			if (errorInfo == null) {
				if (model.PurchasePrice > 0) {
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
			}

			return errorInfo;
		}

		//
		// GET: /Deal/FindDealUnderlyingDirect/1
		[HttpGet]
		public JsonResult FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			DealUnderlyingDirectModel model = DealRepository.FindDealUnderlyingDirectModel(dealUnderlyingDirectId);
			model.Equities = SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(model.IssuerId));
			model.FixedIncomes = SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(model.IssuerId));
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
		public string DeleteDealUnderlyingDirect(int id) {
			if (DealRepository.DeleteDealUnderlyingDirect(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
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
					securityLists = SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(issuerId));
					break;
				case Models.Deal.Enums.SecurityType.FixedIncome:
					securityLists = SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(issuerId));
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
			model.DealId = DealRepository.FindLastDealId();
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
		// GET: /Deal/CreateClosingDealUnderlyingFund
		[HttpPost]
		public ActionResult CreateClosingDealUnderlyingFund(FormCollection collection) {
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			DealUnderlyingFundModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;
			// Validate add new row.
			resultModel.Result = string.Empty;
			rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(DealUnderlyingFundModel), "_");
			model = new DealUnderlyingFundModel();
			this.TryUpdateModel(model, rowCollection);
			model.RecordDate = DateTime.Now;
			model.DealClosingId = null;
			errorInfo = ValidationHelper.Validate(model);
			DealUnderlyingFund dealUnderlyingFund = null;
			if (errorInfo.Any() == false) {
				errorInfo = SaveDealUnderlyingFund(model, out dealUnderlyingFund);
			}
			if (errorInfo == null) {
				if (dealUnderlyingFund != null) {
					resultModel.Result = "True||" + dealUnderlyingFund.DealUnderlyingtFundID;
				}
			}
			else {
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result", resultModel);
		}

		//
		// GET: /Deal/CreateClosingDealUnderlyingDirect
		[HttpPost]
		public ActionResult CreateClosingDealUnderlyingDirect(FormCollection collection) {
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			DealUnderlyingDirectModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;
			// Validate add new row.
			resultModel.Result = string.Empty;
			rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(DealUnderlyingDirectModel), "$");
			model = new DealUnderlyingDirectModel();
			this.TryUpdateModel(model, rowCollection);
			model.RecordDate = DateTime.Now;
			model.DealClosingId = null;
			errorInfo = ValidationHelper.Validate(model);
			DealUnderlyingDirect dealUnderlyingDirect = null;
			if (errorInfo.Any() == false) {
				errorInfo = SaveDealUnderlyingDirect(model, out dealUnderlyingDirect);
			}
			if (errorInfo == null) {
				if (dealUnderlyingDirect != null) {
					resultModel.Result = "True||" + dealUnderlyingDirect.DealUnderlyingDirectID;
				}
			}
			else {
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result", resultModel);
		}

		//
		// GET: /Deal/UpdateDealClosing
		[HttpPost]
		public ActionResult UpdateDealClosing(FormCollection collection) {
			CreateDealCloseModel model = new CreateDealCloseModel();
			this.TryUpdateModel(model);
			int totalUnderlyingFunds;
			int totalUnderlyingDirects;
			int.TryParse(collection["TotalUnderlyingFunds"], out totalUnderlyingFunds);
			int.TryParse(collection["TotalUnderlyingDirects"], out totalUnderlyingDirects);
			bool isNoErrors = false;
			ResultModel resultModel = new ResultModel();
			if (model.DealNumber == 0) {
				ModelState.AddModelError("DealNumber", "Deal Close Number is required");
			}
			if (ModelState.IsValid) {

				// Check error on deal underlying directs and underlying funds.
				IEnumerable<ErrorInfo> errorInfo = UpdateDealClosingFundsAndDirects(collection, totalUnderlyingFunds, totalUnderlyingDirects, 0, false, model.CloseDate, true);
				if (errorInfo != null)
					isNoErrors = !errorInfo.Any();
				else
					isNoErrors = true;

				if (isNoErrors) {

					// Attempt to create deal closing.

					DealClosing dealClosing = DealRepository.FindDealClosing(model.DealClosingId);

					if (dealClosing == null) {
						dealClosing = new DealClosing();
						dealClosing.DealNumber = DealRepository.GetMaxDealClosingNumber(model.DealId);
					}

					dealClosing.CloseDate = model.CloseDate;
					dealClosing.DealID = model.DealId;
					dealClosing.IsFinalClose = model.IsFinalClose;

					errorInfo = DealRepository.SaveDealClosing(dealClosing);

					if (errorInfo == null) {
						errorInfo = UpdateDealClosingFundsAndDirects(collection
																		, totalUnderlyingFunds
																		, totalUnderlyingDirects
																		, dealClosing.DealClosingID, false, model.CloseDate, false);
					}

					if (errorInfo == null) {
						resultModel.Result = "True||" + dealClosing.DealClosingID;
					}
				}

				if (string.IsNullOrEmpty(resultModel.Result)) {
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
			int totalUnderlyingFunds;
			int totalUnderlyingDirects;
			int.TryParse(collection["TotalUnderlyingFunds"], out totalUnderlyingFunds);
			int.TryParse(collection["TotalUnderlyingDirects"], out totalUnderlyingDirects);
			bool isNoErrors = false;
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = UpdateDealClosingFundsAndDirects(collection,
				totalUnderlyingFunds,
				totalUnderlyingDirects, 0, true, model.CloseDate, true);
			if (errorInfo != null)
				isNoErrors = !errorInfo.Any();
			else
				isNoErrors = true;

			if (isNoErrors) {
				UpdateDealClosingFundsAndDirects(collection, totalUnderlyingFunds, totalUnderlyingDirects, 0, true, model.CloseDate, false);
			}
			resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> UpdateDealClosingFundsAndDirects(FormCollection collection,
			int totalUnderlyingFunds,
			int totalUnderlyingDirects,
			int dealClosingId,
			bool isFinalClose,
			DateTime finalCloseDate,
			bool isValidateOnly) {

			IEnumerable<ErrorInfo> errorInfo = null;

			int rowIndex = 0;
			FormCollection rowCollection;
			DealUnderlyingFundModel dealUnderlyingFundModel = null;


			// Save each deal underlying fund rows.
			for (rowIndex = 1; rowIndex < totalUnderlyingFunds + 1; rowIndex++) {
				if (errorInfo != null) {
					if (errorInfo.Any())
						break;
				}
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(DealUnderlyingFundModel), "_");
				dealUnderlyingFundModel = new DealUnderlyingFundModel();
				this.TryUpdateModel(dealUnderlyingFundModel, rowCollection);
				if (dealUnderlyingFundModel.RecordDate == null) {
					dealUnderlyingFundModel.RecordDate = DateTime.Now;
				}
				errorInfo = ValidationHelper.Validate(dealUnderlyingFundModel);
				if (isValidateOnly == false) {
					if (errorInfo.Any() == false) {
						if (isFinalClose == false) {
							dealUnderlyingFundModel.DealClosingId = null;
							if (dealUnderlyingFundModel.IsClose)
								dealUnderlyingFundModel.DealClosingId = dealClosingId;

							errorInfo = null;
						}
						else {
							errorInfo = SaveFinalDealClosing(dealUnderlyingFundModel.DealClosingId ?? 0, finalCloseDate);
						}
						if (errorInfo == null) {
							DealUnderlyingFund dealUnderlyingFund = null;
							errorInfo = SaveDealUnderlyingFund(dealUnderlyingFundModel, out dealUnderlyingFund);
							if (errorInfo != null)
								break;
						}
					}
				}
			}

			if (errorInfo != null) {
				if (errorInfo.Any() == false) errorInfo = null;
			}

			if (errorInfo == null) {
				rowIndex = 0;
				rowCollection = null;
				DealUnderlyingDirectModel dealUnderlyingDirectModel = null;

				// Save each deal underlying direct rows.
				for (rowIndex = 1; rowIndex < totalUnderlyingDirects + 1; rowIndex++) {
					if (errorInfo != null) {
						if (errorInfo.Any()) {
							break;
						}
					}
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(DealUnderlyingDirectModel), "$");
					dealUnderlyingDirectModel = new DealUnderlyingDirectModel();
					this.TryUpdateModel(dealUnderlyingDirectModel, rowCollection);
					if (dealUnderlyingDirectModel.RecordDate == null) {
						dealUnderlyingDirectModel.RecordDate = DateTime.Now;
					}
					errorInfo = ValidationHelper.Validate(dealUnderlyingDirectModel);
					if (isValidateOnly == false) {
						if (errorInfo.Any() == false) {
							if (isFinalClose == false) {
								dealUnderlyingDirectModel.DealClosingId = null;
								if (dealUnderlyingDirectModel.IsClose)
									dealUnderlyingDirectModel.DealClosingId = dealClosingId;

								errorInfo = null;
							}
							else {
								errorInfo = SaveFinalDealClosing(dealUnderlyingDirectModel.DealClosingId ?? 0, finalCloseDate);
							}
							DealUnderlyingDirect dealUnderlyingDirect = null;
							errorInfo = SaveDealUnderlyingDirect(dealUnderlyingDirectModel, out dealUnderlyingDirect);
							if (errorInfo != null)
								break;
						}
					}
				}
			}
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveFinalDealClosing(int dealClosingId, DateTime finalCloseDate) {
			IEnumerable<ErrorInfo> errorInfo = null;
			DealClosing dealClosing = DealRepository.FindDealClosing(dealClosingId);
			if (dealClosing != null) {
				dealClosing.IsFinalClose = true;
				dealClosing.CloseDate = finalCloseDate;
				errorInfo = DealRepository.SaveDealClosing(dealClosing);
			}
			return errorInfo;
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
			Models.Fund.FundDetail dealFundDetail = FundRepository.FindLastFundDetail();
			if (dealFundDetail == null) dealFundDetail = new Models.Fund.FundDetail();
			return View(dealFundDetail);
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
					cell = new List<object> { 
						deal.DealId
						, deal.DealNumber
						, deal.DealName
						, deal.DealDate.ToString("MM/dd/yyyy")
						, FormatHelper.CurrencyFormat(deal.NetPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.GrossPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.CommittedAmount)
						, FormatHelper.CurrencyFormat(deal.UnfundedAmount)
						, FormatHelper.CurrencyFormat(deal.TotalAmount)
					}
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
			int totalRows = 0;
			DealExportModel model = new DealExportModel();
			model.Deals = DealRepository.GetAllReportDeals(1, 200, sortName, sortOrder, ref totalRows, fundId);
			ViewData["ExportTypeId"] = exportTypeId;
			return View("ExportDetail", model);
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
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.UploadTypes = SelectListFactory.GetUploadTypeSelectList();
			return View(model);
		}

		[HttpGet]
		public ActionResult FindUnderlyingFund(int underlyingFundId, int issuerId) {
			CreateUnderlyingFundModel model = DealRepository.FindUnderlyingFundModel(underlyingFundId, issuerId);
			if (model == null) model = new CreateUnderlyingFundModel();
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult UpdateUnderlyingFund(FormCollection collection) {
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

		[HttpPost]
		public string CreateUnderlyingFundDocument(FormCollection collection) {
			UnderlyingFundDocumentModel model = new UnderlyingFundDocumentModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;
			if (ModelState.IsValid) {
				UnderlyingFundDocument underlyingFundDocument = DealRepository.FindUnderlyingFundDocument(model.UnderlyingFundDocumetId);
				if (underlyingFundDocument == null) {
					underlyingFundDocument = new UnderlyingFundDocument();
					underlyingFundDocument.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFundDocument.CreatedDate = DateTime.Now;
				}
				underlyingFundDocument.LastUpdatedBy = AppSettings.CreatedByUserId;
				underlyingFundDocument.LastUpdatedDate = DateTime.Now;
				underlyingFundDocument.EntityID = (int)ConfigUtil.CurrentEntityID;
				underlyingFundDocument.DocumentTypeID = model.DocumentTypeId;
				underlyingFundDocument.DocumentDate = model.DocumentDate;
				underlyingFundDocument.UnderlyingFundID = model.UnderlyingFundId;
				if (underlyingFundDocument.File == null) {
					underlyingFundDocument.File = new Models.Entity.File();
					underlyingFundDocument.File.CreatedBy = AppSettings.CreatedByUserId;
					underlyingFundDocument.File.CreatedDate = DateTime.Now;
				}
				DeepBlue.Models.Document.UploadType uploadType = (DeepBlue.Models.Document.UploadType)model.UploadTypeId;
				FileInfo fileInfo = null;
				resultModel.Result += CreateDocumentFile(underlyingFundDocument.File, uploadType, model.FilePath, Request.Files[0], ref fileInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					errorInfo = AdminRepository.SaveFile(underlyingFundDocument.File);
				}
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					if (underlyingFundDocument.File != null) {
						underlyingFundDocument.FileID = underlyingFundDocument.File.FileID;
					}
					errorInfo = DealRepository.SaveUnderlyingFundDocument(underlyingFundDocument);
					if (errorInfo == null) {
						if (uploadType == Models.Document.UploadType.Upload) {
							UploadFile fileUpload = new UploadFile("UnderlyingFundDocumentUploadPath");
							fileUpload.Move(fileInfo.FullName, (int)ConfigUtil.CurrentEntityID, underlyingFundDocument.UnderlyingFundID, underlyingFundDocument.DocumentTypeID, fileInfo.Name);
							underlyingFundDocument.File.FileName = fileUpload.FileName;
							underlyingFundDocument.File.FilePath = fileUpload.FilePath;
							underlyingFundDocument.File.Size = fileUpload.Size;
						}
						errorInfo = DealRepository.SaveUnderlyingFundDocument(underlyingFundDocument);
					}
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
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
			return JsonSerializer.ToJsonObject(new { error = string.Empty, data = resultModel.Result }).ToString();
		}

		[HttpGet]
		public string DeleteUnderlyingFundDocumentFile(int id) {
			if (DealRepository.DeleteUnderlyingFundDocument(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public ActionResult UnderlyingFundDocumentList(int pageIndex, int pageSize, string sortName, string sortOrder, int underlyingFundId) {
			int totalRows = 0;
			List<UnderlyingFundDocumentList> underlyingFundDocuments = DealRepository.GetAllUnderlyingFundDocuments(underlyingFundId, pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(underlyingFundDocuments);
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
			return Json(DealRepository.FindEquityDirects(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeDirects
		[HttpGet]
		public JsonResult FindFixedIncomeDirects(string term) {
			return Json(DealRepository.FindFixedIncomeDirects(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeSecurityConversionModel
		[HttpGet]
		public JsonResult FindFixedIncomeSecurityConversionModel(int fixedIncomeId) {
			return Json(DealRepository.FindFixedIncomeSecurityConversionModel(fixedIncomeId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquitySecurityConversionModel
		[HttpGet]
		public JsonResult FindEquitySecurityConversionModel(int equityId) {
			return Json(DealRepository.FindEquitySecurityConversionModel(equityId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateSplitActivity
		[HttpPost]
		public ActionResult CreateSplitActivity(FormCollection collection) {
			EquitySplitModel model = new EquitySplitModel();
			this.TryUpdateModel(model, collection);
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
				equitySplit.SplitDate = model.SplitDate;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveEquitySplit(equitySplit);
				if (errorInfo == null) {
					List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.ActivityTypeId, equitySplit.EquiteSplitID, model.SecurityTypeId, equitySplit.EquityID);
					foreach (var pattern in newHoldingPatterns) {
						errorInfo = CreateFundActivityHistory(pattern.FundId, pattern.OldNoOfShares, pattern.NewNoOfShares, equitySplit.EquiteSplitID, model.ActivityTypeId);
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
			this.TryUpdateModel(model, collection);
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
				securityConversion.ConversionDate = model.ConversionDate;
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveSecurityConversion(securityConversion);
				if (errorInfo == null) {

					// Get All Deal Underlying Directs

					List<DealUnderlyingDirect> dealUnderlyingDirects = DealRepository.GetAllDealUnderlyingDirects(securityConversion.OldSecurityTypeID, securityConversion.OldSecurityID);
					foreach (var dealUnderlyingDirect in dealUnderlyingDirects) {

						SecurityConversionDetail securityConversionDetail = new SecurityConversionDetail();
						securityConversionDetail.SecurityConversionID = securityConversion.SecurityConversionID;

						// Set old number of shares and fmv.
						securityConversionDetail.OldNumberOfShares = dealUnderlyingDirect.NumberOfShares;
						securityConversionDetail.OldFMV = dealUnderlyingDirect.FMV;

						// Update Deal Underlying Direct number of shares and fmv.
						dealUnderlyingDirect.NumberOfShares = securityConversionDetail.OldNumberOfShares * securityConversion.SplitFactor;
						dealUnderlyingDirect.FMV = dealUnderlyingDirect.NumberOfShares * dealUnderlyingDirect.PurchasePrice;

						// Set new number of shares and fmv.
						securityConversionDetail.NewNumberOfShares = dealUnderlyingDirect.NumberOfShares;
						securityConversionDetail.NewFMV = dealUnderlyingDirect.FMV;

						securityConversionDetail.DealUnderlyingDirectID = dealUnderlyingDirect.DealUnderlyingDirectID;

						errorInfo = DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
						if (errorInfo == null)
							errorInfo = DealRepository.SaveSecurityConversionDetail(securityConversionDetail);
						else
							break;
					}

					if (errorInfo == null) {
						List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(model.ActivityTypeId, securityConversion.SecurityConversionID, model.OldSecurityTypeId, model.OldSecurityId);
						foreach (var pattern in newHoldingPatterns) {
							errorInfo = CreateFundActivityHistory(pattern.FundId, pattern.OldNoOfShares, pattern.NewNoOfShares, securityConversion.SecurityConversionID, model.ActivityTypeId);
							if (errorInfo != null)
								break;
						}
					}

				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + securityConversion.SecurityConversionID;
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

		private IEnumerable<ErrorInfo> CreateFundActivityHistory(int fundId, int? oldNoOfShares, int? newNoOfShares, int activityId, int activityTypeId) {
			FundActivityHistory fundActivityHistory = new FundActivityHistory();
			fundActivityHistory.ActivityID = activityId;
			fundActivityHistory.ActivityTypeID = activityTypeId;
			fundActivityHistory.FundID = fundId;
			fundActivityHistory.OldNumberOfShares = oldNoOfShares ?? 0;
			fundActivityHistory.NewNumberOfShares = newNoOfShares ?? 0;
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
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCashDistributionModel), "_");
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
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCashDistributionModel), "_");
					model = new UnderlyingFundCashDistributionModel();
					this.TryUpdateModel(model, rowCollection);
					bool isManualCashDistribution = false;
					Boolean.TryParse(collection["IsManualCashDistribution"], out isManualCashDistribution);
					model.IsManualCashDistribution = isManualCashDistribution;
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundCashDistribution(model, collection);
					}
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistributionModel model, FormCollection collection) {
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
					if (model.IsManualCashDistribution == true && dealUnderlyingFunds.Count > 1) {
						cashDistribution.Amount = DataTypeHelper.ToDecimal(collection[underlyingFundCashDistribution.FundID.ToString() + "_" + dealUnderlyingFund.DealID.ToString() + "_" + "CallAmount"]);
					}
					else {
						cashDistribution.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / (dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0))) * underlyingFundCashDistribution.Amount;
					}

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
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCashDistributionModel), "_");
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
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCashDistributionModel), "_");
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
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCapitalCallModel), "_");
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
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundCapitalCallModel), "_");
					model = new UnderlyingFundCapitalCallModel();
					this.TryUpdateModel(model, rowCollection);
					bool isManualCapitalCall = false;
					Boolean.TryParse(collection["IsManualCapitalCall"], out isManualCapitalCall);
					model.IsManualCapitalCall = isManualCapitalCall;
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundCapitalCall(model, collection);
						if (errorInfo != null)
							resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCallModel model, FormCollection collection) {
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
					if (model.IsManualCapitalCall && dealUnderlyingFunds.Count > 1) {
						underlyingFundCapitalCallLineItem.Amount = DataTypeHelper.ToDecimal(collection[underlyingFundCapitalCall.FundID.ToString() + "_" + dealUnderlyingFund.DealID.ToString() + "_" + "CallAmount"]);
					}
					else {
						underlyingFundCapitalCallLineItem.Amount = ((dealUnderlyingFund.CommittedAmount ?? 0) / (dealUnderlyingFunds.Sum(fund => fund.CommittedAmount ?? 0))) * underlyingFundCapitalCall.Amount;
					}

					underlyingFundCapitalCallLineItem.ReceivedDate = underlyingFundCapitalCall.ReceivedDate;
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
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCapitalCallModel), "_");
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
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundPostRecordCapitalCallModel), "_");
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

		#region UnderlyingFundStockDistribution

		//
		// POST : /Deal/CreateUnderlyingFundStockDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundStockDistribution(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundStockDistributionModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;

			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				resultModel.Result = string.Empty;
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundStockDistributionModel), "_");
				model = new UnderlyingFundStockDistributionModel();
				this.TryUpdateModel(model, rowCollection);
				if (model.SecurityId > 0) {
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
					rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(UnderlyingFundStockDistributionModel), "_");
					model = new UnderlyingFundStockDistributionModel();
					this.TryUpdateModel(model, rowCollection);
					bool isManualStockDistribution = false;
					Boolean.TryParse(collection["IsManualStockDistribution"], out isManualStockDistribution);
					model.IsManualStockDistribution = isManualStockDistribution;
					errorInfo = ValidationHelper.Validate(model);
					if (errorInfo.Any() == false) {
						errorInfo = SaveUnderlyingFundStockDistribution(model, collection);
						if (errorInfo != null)
							resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundStockDistribution(UnderlyingFundStockDistributionModel model, FormCollection collection) {
			IEnumerable<ErrorInfo> errorInfo = null;
			// Attempt to create underlying fund cash distribution.

			UnderlyingFundStockDistribution underlyingFundStockDistribution = new UnderlyingFundStockDistribution();
			underlyingFundStockDistribution.UnderlyingFundID = model.UnderlyingFundId;
			underlyingFundStockDistribution.FundID = model.FundId;
			underlyingFundStockDistribution.DistributionDate = model.DistributionDate;
			underlyingFundStockDistribution.NoticeDate = model.NoticeDate;

			underlyingFundStockDistribution.NumberOfShares = model.NumberOfShares;
			underlyingFundStockDistribution.PurchasePrice = model.PurchasePrice;

			underlyingFundStockDistribution.FMV = underlyingFundStockDistribution.NumberOfShares * underlyingFundStockDistribution.PurchasePrice;

			underlyingFundStockDistribution.SecurityID = model.SecurityId;
			underlyingFundStockDistribution.SecurityTypeID = model.SecurityTypeId;
			underlyingFundStockDistribution.TaxCostBase = model.TaxCostBase;
			underlyingFundStockDistribution.TaxCostDate = model.TaxCostDate;

			errorInfo = DealRepository.SaveUnderlyingFundStockDistribution(underlyingFundStockDistribution);
			if (errorInfo == null) {

				// Attempt to create stock distribution to each deal.
				List<StockDistributionLineItemModel> deals = DealRepository.GetAllDeals(underlyingFundStockDistribution.SecurityTypeID,
																						underlyingFundStockDistribution.SecurityID,
																						underlyingFundStockDistribution.FundID,
																						underlyingFundStockDistribution.UnderlyingFundID);
				foreach (var deal in deals) {
					UnderlyingFundStockDistributionLineItem stockDistributionItem = new UnderlyingFundStockDistributionLineItem();
					stockDistributionItem.DealID = deal.DealId;
					stockDistributionItem.UnderlyingFundID = underlyingFundStockDistribution.UnderlyingFundID;

					// Calculate capital call amount = (Deal Underlying Fund Committed Amount) / (Total Deal Underlying Fund Committed Amount) * Total Capital Call Amount.
					if (model.IsManualStockDistribution) {
						stockDistributionItem.FMV = DataTypeHelper.ToDecimal(collection[underlyingFundStockDistribution.FundID.ToString() + "_" + deal.DealId.ToString() + "_" + "FMV"]);
						stockDistributionItem.NumberOfShares = DataTypeHelper.ToDecimal(collection[underlyingFundStockDistribution.FundID.ToString() + "_" + deal.DealId.ToString() + "_" + "NumberOfShares"]);
					}
					else {
						stockDistributionItem.NumberOfShares = (((decimal)(deal.NumberOfShares ?? 0) / (decimal)deals.Sum(d => d.NumberOfShares)) * underlyingFundStockDistribution.NumberOfShares);
						stockDistributionItem.FMV = stockDistributionItem.NumberOfShares * underlyingFundStockDistribution.PurchasePrice;
					}

					stockDistributionItem.UnderlyingFundStockDistributionID = underlyingFundStockDistribution.UnderlyingFundStockDistributionID;
					errorInfo = DealRepository.SaveUnderlyingFundStockDistributionLineItem(stockDistributionItem);
					if (errorInfo != null)
						break;
				}

			}
			return errorInfo;
		}

		//
		// GET: /Deal/UnderlyingFundStockDistributionList
		[HttpGet]
		public JsonResult UnderlyingFundStockDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundStockDistributions(underlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/StockDistributionDirectList
		[HttpGet]
		public JsonResult StockDistributionDirectList(int securityTypeId, int securityId, int fundId) {
			return Json(DealRepository.GetAllStockDistributionDirectList(securityTypeId, securityId, fundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindStockIssuers
		[HttpGet]
		public JsonResult FindStockIssuers(int underlyingFundId, int fundId, string term) {
			return Json(DealRepository.FindStockIssuers(underlyingFundId, fundId, term), JsonRequestBehavior.AllowGet);
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
		// POST : /Deal/FundExpenseList
		[HttpGet]
		public JsonResult FundExpenseList(int fundId) {
			return Json(DealRepository.GetAllFundExpenses(fundId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/FundExpense
		[HttpPost]
		public ActionResult CreateFundExpense(FormCollection collection) {
			FundExpenseModel model = new FundExpenseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {

				// Attempt to create fund expense.

				FundExpense fundExpense = DealRepository.FindFundExpense(model.FundExpenseId);
				if (fundExpense == null) {
					fundExpense = new FundExpense();
					fundExpense.CreatedBy = AppSettings.CreatedByUserId;
					fundExpense.CreatedDate = DateTime.Now;
				}
				fundExpense.FundID = model.FundId;
				fundExpense.FundExpenseTypeID = model.FundExpenseTypeId;
				fundExpense.Amount = model.Amount;
				fundExpense.Date = model.Date;
				fundExpense.LastUpdatedBy = AppSettings.CreatedByUserId;
				fundExpense.LastUpdatedDate = DateTime.Now;
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
		public JsonResult FindFundExpense(int fundExpenseId) {
			return Json(DealRepository.FindFundExpenseModel(fundExpenseId), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region NewHoldingPattern
		//
		// GET: /Deal/NewHoldingPatternList
		public ActionResult NewHoldingPatternList(int activityTypeId, int activityId, int securityTypeId, int securityId) {
			List<NewHoldingPatternModel> newHoldingPatterns = DealRepository.NewHoldingPatternList(activityTypeId, activityId, securityTypeId, securityId);
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
			return DealRepository.FindEquitySymbol(id);
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
		// GET: /Deal/FindUnfundedAdjustment
		[HttpGet]
		public JsonResult FindUnfundedAdjustment(int dealUnderlyingFundId) {
			return Json(DealRepository.FindUnfundedAdjustmentModel(dealUnderlyingFundId), JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnfundedAdjustment
		[HttpPost]
		public ActionResult UpdateUnfundedAdjustment(FormCollection collection) {
			UnfundedAdjustmentModel model = new UnfundedAdjustmentModel();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo = null;
				DealUnderlyingFund dealUnderlyingFund = DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId);
				if (dealUnderlyingFund != null) {
					dealUnderlyingFund.CommittedAmount = model.CommitmentAmount;
					dealUnderlyingFund.UnfundedAmount = model.UnfundedAmount;
					errorInfo = DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
					if (errorInfo == null) {
						DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment = new DealUnderlyingFundAdjustment();
						dealUnderlyingFundAdjustment.CreatedBy = AppSettings.CreatedByUserId;
						dealUnderlyingFundAdjustment.CreatedDate = DateTime.Now;
						dealUnderlyingFundAdjustment.LastUpdatedBy = AppSettings.CreatedByUserId;
						dealUnderlyingFundAdjustment.LastUpdatedDate = DateTime.Now;
						dealUnderlyingFundAdjustment.CommitmentAmount = model.CommitmentAmount;
						dealUnderlyingFundAdjustment.UnfundedAmount = model.UnfundedAmount;
						dealUnderlyingFundAdjustment.DealUnderlyingFundID = model.DealUnderlyingFundId;
						errorInfo = DealRepository.SaveDealUnderlyingFundAdjustment(dealUnderlyingFundAdjustment);
					}
					resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
					if (string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result = "True||" + dealUnderlyingFund.DealUnderlyingtFundID;
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
			ReconcileSearchModel model = new ReconcileSearchModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			List<ReconcileReportModel> allReconciles = null;
			if (ModelState.IsValid) {
				switch ((DeepBlue.Models.Deal.Enums.ReconcileType)model.ReconcileType) {
					case ReconcileType.All:
						allReconciles = DealRepository.GetAllReconciles((model.StartDate ?? Convert.ToDateTime("01/01/1900")), (model.EndDate ?? DateTime.MaxValue), model.FundId);
						break;
					case ReconcileType.UnderlyingFundCapitalCall:
						allReconciles = DealRepository.GetAllUnderlyingCapitalCallReconciles((model.StartDate ?? Convert.ToDateTime("01/01/1900")), (model.EndDate ?? DateTime.MaxValue), model.FundId);
						break;
					case ReconcileType.UnderlyingFundCashDistribution:
						allReconciles = DealRepository.GetAllUnderlyingDistributionReconciles((model.StartDate ?? Convert.ToDateTime("01/01/1900")), (model.EndDate ?? DateTime.MaxValue), model.FundId);
						break;
					case ReconcileType.CapitalCall:
						allReconciles = DealRepository.GetAllCapitalCallReconciles((model.StartDate ?? Convert.ToDateTime("01/01/1900")), (model.EndDate ?? DateTime.MaxValue), model.FundId);
						break;
					case ReconcileType.CapitalDistribution:
						allReconciles = DealRepository.GetAllCapitalDistributionReconciles((model.StartDate ?? Convert.ToDateTime("01/01/1900")), (model.EndDate ?? DateTime.MaxValue), model.FundId);
						break;
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Results = allReconciles }, JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateReconcile
		[HttpPost]
		public ActionResult CreateReconcile(FormCollection collection) {
			int totalRows = 0;
			int.TryParse(collection["TotalRows"], out totalRows);
			int rowIndex = 0;
			ResultModel resultModel = new ResultModel();
			FormCollection rowCollection;
			ReconcileModel model = null;
			IEnumerable<ErrorInfo> errorInfo = null;
			// Validate each rows.
			for (rowIndex = 0; rowIndex < totalRows; rowIndex++) {
				rowCollection = FormCollectionHelper.GetFormCollection(collection, rowIndex, typeof(ReconcileModel), "_");
				model = new ReconcileModel();
				this.TryUpdateModel(model, rowCollection);
				IEnumerable<ErrorInfo> validateErrorInfo = ValidationHelper.Validate(model);
				if (validateErrorInfo.Any() == false) {
					if (model.IsReconciled) {
						errorInfo = SaveReconcile(model);
						if (errorInfo != null)
							break;
					}
				}
			}
			resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
			return View("Result", resultModel);
		}

		private IEnumerable<ErrorInfo> SaveReconcile(ReconcileModel model) {
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = ValidationHelper.Validate(model);
			if (errorInfo.Any() == false) {
				switch ((ReconcileType)model.ReconcileTypeId) {
					case ReconcileType.CapitalCall:
						CapitalCallLineItem capitalCallLineItem = CapitalCallRepository.FindCapitalCallLineItem(model.Id);
						if (capitalCallLineItem != null) {
							capitalCallLineItem.IsReconciled = model.IsReconciled;
							capitalCallLineItem.PaidON = model.PaidOn;
							capitalCallLineItem.LastUpdatedBy = AppSettings.CreatedByUserId;
							capitalCallLineItem.LastUpdatedDate = DateTime.Now;
							errorInfo = CapitalCallRepository.SaveCapitalCallLineItem(capitalCallLineItem);
						}
						break;
					case ReconcileType.CapitalDistribution:
						CapitalDistributionLineItem capitalDistributionLineItem = CapitalCallRepository.FindCapitalDistributionLineItem(model.Id);
						if (capitalDistributionLineItem != null) {
							capitalDistributionLineItem.IsReconciled = model.IsReconciled;
							capitalDistributionLineItem.PaidON = model.PaidOn;
							capitalDistributionLineItem.LastUpdatedBy = AppSettings.CreatedByUserId;
							capitalDistributionLineItem.LastUpdatedDate = DateTime.Now;
							errorInfo = CapitalCallRepository.SaveCapitalDistributionLineItem(capitalDistributionLineItem);
						}
						break;
					case ReconcileType.UnderlyingFundCapitalCall:
						UnderlyingFundCapitalCall underlyingFundCapitalCall = DealRepository.FindUnderlyingFundCapitalCall(model.Id);
						if (underlyingFundCapitalCall != null) {
							underlyingFundCapitalCall.IsReconciled = model.IsReconciled;
							underlyingFundCapitalCall.PaidON = model.PaidOn;
							underlyingFundCapitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
							underlyingFundCapitalCall.LastUpdatedDate = DateTime.Now;
							errorInfo = DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);
						}
						break;
					case ReconcileType.UnderlyingFundCashDistribution:
						UnderlyingFundCashDistribution underlyingFundCashDistribution = DealRepository.FindUnderlyingFundCashDistribution(model.Id);
						if (underlyingFundCashDistribution != null) {
							underlyingFundCashDistribution.IsReconciled = model.IsReconciled;
							underlyingFundCashDistribution.PaidON = model.PaidOn;
							underlyingFundCashDistribution.LastUpdatedBy = AppSettings.CreatedByUserId;
							underlyingFundCashDistribution.LastUpdatedDate = DateTime.Now;
							errorInfo = DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
						}
						break;
				}
			}
			return errorInfo;
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

			model.IssuerDetailModel.IssuerRatings = SelectListFactory.GetEmptySelectList();

			model.EquityDetailModel.Currencies = SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
			model.EquityDetailModel.ShareClassTypes = SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
			model.EquityDetailModel.EquityTypes = SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
			model.EquityDetailModel.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.EquityDetailModel.UploadTypes = SelectListFactory.GetUploadTypeSelectList();
			model.EquityDetailModel.EquitySecurityTypes = SelectListFactory.GetEquitySecurityTypeList();

			model.FixedIncomeDetailModel.FixedIncomeTypes = SelectListFactory.GetFixedIncomeTypesSelectList(AdminRepository.GetAllFixedIncomeTypes());
			model.FixedIncomeDetailModel.Currencies = model.EquityDetailModel.Currencies;
			model.FixedIncomeDetailModel.DocumentTypes = model.EquityDetailModel.DocumentTypes;
			model.FixedIncomeDetailModel.UploadTypes = model.EquityDetailModel.UploadTypes;

			model.EquityDetailModel.EquityCurrencyId = (int)DeepBlue.Models.Deal.Enums.Currency.USD;
			model.FixedIncomeDetailModel.FixedIncomeCurrencyId = (int)DeepBlue.Models.Deal.Enums.Currency.USD;

			return View(model);
		}

		//
		// GET: /Deal/FindIssuer
		[HttpGet]
		public JsonResult FindIssuer(int id) {
			CreateIssuerModel model = DealRepository.FindIssuerModel(id);
			if(model == null) model = new CreateIssuerModel();
			if (model.IssuerDetailModel == null) model.IssuerDetailModel = new IssuerDetailModel();
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public JsonResult EquityList(int issuerId, int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<EquityListModel> equities = DealRepository.GetAllEquity(issuerId, pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var equity in equities) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { equity.EquityId, equity.Symbol, equity.Industry, equity.EquityType }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquityFixedIncomeIssuers
		[HttpGet]
		public JsonResult FindEquityFixedIncomeIssuers(string term) {
			return Json(DealRepository.FindEquityFixedIncomeIssuers(term), JsonRequestBehavior.AllowGet);
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
			if (model.IsUnderlyingFundModel) 
				resultModel.Result = resultModel.Result.Replace("Issuer Name", "GP").Replace("Parent Name","GP Parent");
			else
				resultModel.Result = resultModel.Result.Replace("Issuer Name", "Company").Replace("Parent Name", "Company Parent");
			return View("Result", resultModel);
		}

		//
		// POST: /Deal/UpdateIssuer
		[HttpPost]
		public string UpdateIssuer(FormCollection collection) {
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;

			IssuerDetailModel model = new IssuerDetailModel();
			this.TryUpdateModel(model);

			errorInfo = ValidationHelper.Validate(model);
			resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
			resultModel.Result = resultModel.Result.Replace("Issuer Name", "Company").Replace("Parent Name", "Company Parent"); 

			string ErrorMessage = IssuerNameAvailable(model.Name, model.IssuerId);

			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				resultModel.Result += ErrorMessage + "\n";
			}

			if (string.IsNullOrEmpty(resultModel.Result)) {
				Models.Entity.Issuer issuer = null;
				errorInfo = SaveIssuer(model, out issuer);
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (errorInfo == null) {

					EquityDetailModel equityDetailModel = new EquityDetailModel();
					bool isValidDocument = false;
					string documentError = string.Empty;
					this.TryUpdateModel(equityDetailModel);

					UnderlyingDirectDocumentModel equityUnderlyingDirectDocumentModel = new UnderlyingDirectDocumentModel {
						DocumentDate = equityDetailModel.EquityDocumentDate,
						DocumentTypeId = equityDetailModel.EquityDocumentTypeId,
						File = equityDetailModel.EquityFile,
						SecurityId = equityDetailModel.EquityId,
						SecurityTypeId = (int)Models.Deal.Enums.SecurityType.Equity,
						FilePath = equityDetailModel.EquityFilePath,
						UploadTypeId = equityDetailModel.EquityUploadTypeId
					};



					if (string.IsNullOrEmpty(equityDetailModel.EquitySymbol) == false ||
						equityDetailModel.EquityIndustryId > 0 ||
						equityDetailModel.ShareClassTypeId > 0 ||
						string.IsNullOrEmpty(equityDetailModel.EquityISINO) == false ||
						string.IsNullOrEmpty(equityDetailModel.EquityComments) == false ||
						equityDetailModel.EquityDocumentTypeId > 0 ||
						((equityDetailModel.EquityUploadTypeId == (int)Models.Document.UploadType.Upload) && equityDetailModel.EquityFile != null) ||
						((equityDetailModel.EquityUploadTypeId == (int)Models.Document.UploadType.Link) && string.IsNullOrEmpty(equityDetailModel.EquityFilePath) == false) ||
						equityDetailModel.EquityTypeId > 0) {
						errorInfo = ValidationHelper.Validate(equityDetailModel);
						resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					}

					documentError = ValidateUnderlyingDirectDocument(equityUnderlyingDirectDocumentModel, ref isValidDocument);
					if (isValidDocument) {
						resultModel.Result += documentError;
					}

					FixedIncomeDetailModel fixedIncomeDetailModel = new FixedIncomeDetailModel();
					this.TryUpdateModel(fixedIncomeDetailModel);

					UnderlyingDirectDocumentModel fixedIncomeUnderlyingDirectDocumentModel = new UnderlyingDirectDocumentModel {
						DocumentDate = fixedIncomeDetailModel.FixedIncomeDocumentDate,
						DocumentTypeId = fixedIncomeDetailModel.FixedIncomeDocumentTypeId,
						File = fixedIncomeDetailModel.FixedIncomeFile,
						SecurityId = fixedIncomeDetailModel.FixedIncomeId,
						SecurityTypeId = (int)Models.Deal.Enums.SecurityType.FixedIncome,
						FilePath = fixedIncomeDetailModel.FixedIncomeFilePath,
						UploadTypeId = fixedIncomeDetailModel.FixedIncomeUploadTypeId
					};

					if (fixedIncomeDetailModel.FaceValue > 0 ||
						fixedIncomeDetailModel.Maturity.HasValue ||
						fixedIncomeDetailModel.IssuedDate.HasValue ||
						string.IsNullOrEmpty(fixedIncomeDetailModel.CouponInformation) == false ||
						fixedIncomeDetailModel.Frequency > 0 ||
						fixedIncomeDetailModel.FirstCouponDate.HasValue ||
						fixedIncomeDetailModel.FirstAccrualDate.HasValue ||
						fixedIncomeDetailModel.FixedIncomeTypeId > 0 ||
						fixedIncomeDetailModel.FixedIncomeIndustryId > 0 ||
						fixedIncomeDetailModel.FixedIncomeDocumentTypeId > 0 ||
						((fixedIncomeDetailModel.FixedIncomeUploadTypeId == (int)Models.Document.UploadType.Upload) && fixedIncomeDetailModel.FixedIncomeFile != null) ||
						((fixedIncomeDetailModel.FixedIncomeUploadTypeId == (int)Models.Document.UploadType.Link) && string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeFilePath) == false) ||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeISINO) == false ||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeComments) == false ||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeSymbol) == false) {
						errorInfo = ValidationHelper.Validate(fixedIncomeDetailModel);
						resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					}

					documentError = ValidateUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel, ref isValidDocument);
					if (isValidDocument) {
						resultModel.Result += documentError;
					}

					if (string.IsNullOrEmpty(resultModel.Result)) {
						if (fixedIncomeDetailModel.FixedIncomeTypeId > 0) {
							errorInfo = SaveFixedIncome(ref fixedIncomeDetailModel);
							if (errorInfo == null) {
								fixedIncomeUnderlyingDirectDocumentModel.SecurityId = fixedIncomeDetailModel.FixedIncomeId;
								resultModel.Result += SaveUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel);
							}
							resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
						}
						if (equityDetailModel.EquityTypeId > 0) {
							errorInfo = SaveEquity(ref equityDetailModel);
							if (errorInfo == null) {
								equityUnderlyingDirectDocumentModel.SecurityId = equityDetailModel.EquityId;
								resultModel.Result += SaveUnderlyingDirectDocument(equityUnderlyingDirectDocumentModel);
							}
							resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
						}
					}
				}
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + issuer.IssuerID + "||" + issuer.Name;
				}
			}
			return JsonSerializer.ToJsonObject(new { error = string.Empty, data = resultModel.Result }).ToString();
		}

		private string ValidateUnderlyingDirectDocument(UnderlyingDirectDocumentModel model, ref bool isValidDocument) {
			ResultModel resultModel = new ResultModel();
			Models.Document.UploadType uploadType = (Models.Document.UploadType)model.UploadTypeId;
			Models.Deal.Enums.SecurityType securityType = (Models.Deal.Enums.SecurityType)model.SecurityTypeId;
			string securityTypeName = (securityType == Models.Deal.Enums.SecurityType.Equity ? "Equity" : "Fixed Income");
			string fileName = string.Empty;
			string ext = string.Empty;
			if (model.File != null) {
				fileName = model.File.FileName;
			}
			if (model.DocumentTypeId > 0 ||
				(uploadType == Models.Document.UploadType.Upload && string.IsNullOrEmpty(fileName) == false) ||
				(uploadType == Models.Document.UploadType.Link && string.IsNullOrEmpty(model.FilePath) == false)
				) {

				isValidDocument = true;

				if (model.DocumentTypeId == 0)
					resultModel.Result += securityTypeName + " Document Type required\n";

				if (model.DocumentDate.Year < 1900)
					resultModel.Result += securityTypeName + " Document Date required\n";

				switch (uploadType) {
					case Models.Document.UploadType.Link:
						if (string.IsNullOrEmpty(model.FilePath)) {
							resultModel.Result += "Link is required";
						}
						else {
							if (FileHelper.CheckFilePath(model.FilePath) == false) {
								ModelState.AddModelError("FilePath", "Invalid Link.");
							}
							else {
								fileName = Path.GetFileName(model.FilePath);
								ext = Path.GetExtension(model.FilePath);
								model.FilePath = model.FilePath.Replace(fileName, "");
								if (model.FilePath.ToLower().StartsWith("http://") == false)
									model.FilePath = "http://" + model.FilePath;
								break;
							}
						}
						break;
					case Models.Document.UploadType.Upload:
						if (model.File == null) {
							resultModel.Result += "File is required";
						}
						if (model.File != null) {
							if (string.IsNullOrEmpty(model.File.FileName)) {
								resultModel.Result += "File is required";
							}
							else {
								ext = Path.GetExtension(model.File.FileName).ToLower();
							}
						}
						break;
				}

				if (string.IsNullOrEmpty(ext) == false) {
					string fileTypeError = string.Empty;
					FileType fileType = FileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(), ext, out fileTypeError);
					if (fileType == null) {
						resultModel.Result += fileTypeError;
					}
				}
			}
			return resultModel.Result;
		}

		private string SaveUnderlyingDirectDocument(UnderlyingDirectDocumentModel model) {
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;
			string uploadPath = (model.SecurityTypeId == (int)Models.Deal.Enums.SecurityType.Equity ? "EquityDocumentUploadPath" : "FixedIncomeDocumentUploadPath");
			Models.Document.UploadType uploadType = (Models.Document.UploadType)model.UploadTypeId;
			Models.Deal.Enums.SecurityType securityType = (Models.Deal.Enums.SecurityType)model.SecurityTypeId;
			FileInfo fileInfo = null;
			bool isValidDocument = false;
			resultModel.Result = ValidateUnderlyingDirectDocument(model, ref isValidDocument);
			if (isValidDocument) {
				if (string.IsNullOrEmpty(resultModel.Result)) {
					UnderlyingDirectDocument underlyingDirectDocument = new UnderlyingDirectDocument();
					underlyingDirectDocument.CreatedBy = AppSettings.CreatedByUserId;
					underlyingDirectDocument.CreatedDate = DateTime.Now;
					underlyingDirectDocument.LastUpdatedBy = AppSettings.CreatedByUserId;
					underlyingDirectDocument.LastUpdatedDate = DateTime.Now;
					underlyingDirectDocument.EntityID = (int)ConfigUtil.CurrentEntityID;
					underlyingDirectDocument.DocumentDate = model.DocumentDate;
					underlyingDirectDocument.DocumentTypeID = model.DocumentTypeId;
					underlyingDirectDocument.SecurityTypeID = model.SecurityTypeId;
					underlyingDirectDocument.SecurityID = model.SecurityId;
					underlyingDirectDocument.File = new Models.Entity.File();
					underlyingDirectDocument.File.CreatedBy = AppSettings.CreatedByUserId;
					underlyingDirectDocument.File.CreatedDate = DateTime.Now;
					underlyingDirectDocument.File.LastUpdatedBy = AppSettings.CreatedByUserId;
					underlyingDirectDocument.File.LastUpdatedDate = DateTime.Now;
					underlyingDirectDocument.File.EntityID = (int)ConfigUtil.CurrentEntityID;

					resultModel.Result += CreateDocumentFile(underlyingDirectDocument.File, uploadType, model.FilePath, model.File, ref fileInfo);
					if (string.IsNullOrEmpty(resultModel.Result)) {
						errorInfo = AdminRepository.SaveFile(underlyingDirectDocument.File);
					}
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					if (string.IsNullOrEmpty(resultModel.Result)) {
						if (underlyingDirectDocument.File != null) {
							underlyingDirectDocument.FileID = underlyingDirectDocument.File.FileID;
						}
						errorInfo = DealRepository.SaveUnderlyingDirectDocument(underlyingDirectDocument);
						if (errorInfo == null) {
							if (uploadType == Models.Document.UploadType.Upload) {
								UploadFile fileUpload = new UploadFile(uploadPath);
								fileUpload.Move(fileInfo.FullName, (int)ConfigUtil.CurrentEntityID, underlyingDirectDocument.UnderlyingDirectDocumentID, underlyingDirectDocument.DocumentTypeID, fileInfo.Name);
								underlyingDirectDocument.File.FileName = fileUpload.FileName;
								underlyingDirectDocument.File.FilePath = fileUpload.FilePath;
								underlyingDirectDocument.File.Size = fileUpload.Size;
							}
							errorInfo = DealRepository.SaveUnderlyingDirectDocument(underlyingDirectDocument);
						}
						resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return resultModel.Result;
		}

		private IEnumerable<ErrorInfo> SaveEquity(ref EquityDetailModel model) {
			Models.Entity.Equity equity = DealRepository.FindEquity(model.EquityId);
			EquitySecurityType equitySecurityType = (EquitySecurityType)model.EquitySecurityTypeId;
			if (equity == null) {
				equity = new Models.Entity.Equity();
			}
			equity.CurrencyID = ((model.EquityCurrencyId ?? 0) > 0 ? model.EquityCurrencyId : null);
			equity.EquityTypeID = model.EquityTypeId;
			equity.IndustryID = ((model.EquityIndustryId ?? 0) > 0 ? model.EquityIndustryId : null);
			equity.IssuerID = model.IssuerId;
			equity.Public = (equitySecurityType == EquitySecurityType.Public ? true : false);
			equity.ShareClassTypeID = ((model.ShareClassTypeId ?? 0) > 0 ? model.ShareClassTypeId : null);
			equity.Symbol = model.EquitySymbol;
			equity.EntityID = (int)ConfigUtil.CurrentEntityID;
			equity.Comments = model.EquityComments;
			equity.ISIN = model.EquityISINO;
			IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveEquity(equity);
			model.EquityId = equity.EquityID;
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveFixedIncome(ref FixedIncomeDetailModel model) {
			Models.Entity.FixedIncome fixedIncome = DealRepository.FindFixedIncome(model.FixedIncomeId);
			if (fixedIncome == null) {
				fixedIncome = new Models.Entity.FixedIncome();
			}
			fixedIncome.CurrencyID = ((model.FixedIncomeCurrencyId ?? 0) > 0 ? model.FixedIncomeCurrencyId : null);
			fixedIncome.FixedIncomeTypeID = model.FixedIncomeTypeId;
			fixedIncome.IndustryID = ((model.FixedIncomeIndustryId ?? 0) > 0 ? model.FixedIncomeIndustryId : null);
			fixedIncome.IssuerID = model.IssuerId;
			fixedIncome.ISIN = model.FixedIncomeISINO;
			fixedIncome.Symbol = model.FixedIncomeSymbol;
			fixedIncome.EntityID = (int)ConfigUtil.CurrentEntityID;
			fixedIncome.Maturity = model.Maturity;
			fixedIncome.IssuedDate = model.IssuedDate;
			fixedIncome.Frequency = model.Frequency;
			fixedIncome.FirstCouponDate = model.FirstCouponDate;
			fixedIncome.FirstAccrualDate = model.FirstAccrualDate;
			fixedIncome.FaceValue = model.FaceValue;
			fixedIncome.CouponInformation = model.CouponInformation;
			fixedIncome.Comments = model.FixedIncomeComments;
			IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveFixedIncome(fixedIncome);
			model.FixedIncomeId = fixedIncome.FixedIncomeID;
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveIssuer(IssuerDetailModel model, out Models.Entity.Issuer issuer) {
			issuer = DealRepository.FindIssuer(model.IssuerId);
			if (issuer == null) {
				issuer = new Models.Entity.Issuer();
			}
			issuer.Name = model.Name;
			issuer.ParentName = model.ParentName;
			issuer.CountryID = model.CountryId;
			issuer.EntityID = (int)ConfigUtil.CurrentEntityID;
			return DealRepository.SaveIssuer(issuer);
		}

		//
		// GET: /Fund/FindIssuers
		[HttpGet]
		public JsonResult FindIssuers(string term) {
			return Json(DealRepository.FindIssuers(term), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public string IssuerNameAvailable(string Name, int IssuerID) {
			if (DealRepository.IssuerNameAvailable(Name, IssuerID))
				return "Name already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Create Document
		private string CreateDocumentFile(Models.Entity.File file, DeepBlue.Models.Document.UploadType uploadType, string filePath, HttpPostedFileBase postFile, ref  FileInfo fileInfo) {
			string fileName = string.Empty;
			string ext = string.Empty;
			long fileSize = 0;
			string errorMessage = string.Empty;
			Models.Entity.FileType fileType = null;
			errorMessage = string.Empty;
			ResultModel resultModel = new ResultModel();

			switch (uploadType) {
				case Models.Document.UploadType.Link:
					if (string.IsNullOrEmpty(filePath)) {
						resultModel.Result += "Link is required";
					}
					else {
						if (FileHelper.CheckFilePath(filePath) == false) {
							ModelState.AddModelError("FilePath", "Invalid Link.");
						}
						else {
							fileName = Path.GetFileName(filePath);
							ext = Path.GetExtension(filePath);
							filePath = filePath.Replace(fileName, "");
							if (filePath.ToLower().StartsWith("http://") == false)
								filePath = "http://" + filePath;
							break;
						}
					}
					break;
				case Models.Document.UploadType.Upload:
					if (postFile == null) {
						resultModel.Result += "File is required\n";
					}
					if (postFile != null) {
						if (string.IsNullOrEmpty(postFile.FileName)) {
							resultModel.Result += "File is required\n";
						}
						else {
							ext = Path.GetExtension(postFile.FileName).ToLower();
						}
					}
					break;
			}

			string fileTypeError = string.Empty;
			fileType = FileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(), ext, out fileTypeError);
			if (fileType == null) {
				resultModel.Result += fileTypeError;
			}

			if (string.IsNullOrEmpty(resultModel.Result)) {
				file.LastUpdatedBy = AppSettings.CreatedByUserId;
				file.LastUpdatedDate = DateTime.Now;
				file.FileTypeID = fileType.FileTypeID;
				file.EntityID = (int)ConfigUtil.CurrentEntityID;
				if (uploadType == Models.Document.UploadType.Upload) {
					UploadFile fileUpload = new UploadFile(postFile, "TempUploadPath", postFile.FileName);
					fileUpload.Upload();
					fileInfo = fileUpload.FileInfo;
					fileName = fileUpload.FileName;
					filePath = fileUpload.FilePath;
					fileSize = fileUpload.Size;
				}
				file.FileName = fileName;
				file.FilePath = filePath;
				file.Size = fileSize;
				IEnumerable<ErrorInfo> errorInfo = ValidationHelper.Validate(file);
				errorMessage += ValidationHelper.GetErrorInfo(errorInfo);
			}

			return resultModel.Result;
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
