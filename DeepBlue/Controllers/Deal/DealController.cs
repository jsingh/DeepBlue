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

namespace DeepBlue.Controllers.Deal {
	public class DealController : Controller {

		public IDealRepository DealRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public DealController()
			: this(new DealRepository(), new AdminRepository()) {
		}

		public DealController(IDealRepository dealRepository, IAdminRepository adminRepository) {
			DealRepository = dealRepository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Deal/New
		public ActionResult New() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Create New Deal";
			ViewData["PageName"] = "Create New Deal";
			CreateModel model = new CreateModel();
			model.Contacts = SelectListFactory.GetEmptySelectList();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.PurchaseTypes = SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
			model.DealClosingCostTypes = SelectListFactory.GetDealClosingCostTypeSelectList(AdminRepository.GetAllDealClosingCostTypes());
			return View(model);
		}

		//
		// GET: /Deal/Edit
		public ActionResult Edit() {
			ViewData["MenuName"] = "DealManagement";
			ViewData["SubmenuName"] = "Modify Deal";
			ViewData["PageName"] = "Modify Deal";
			CreateModel model = new CreateModel();
			model.Contacts = SelectListFactory.GetEmptySelectList();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.PurchaseTypes = SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
			model.DealClosingCostTypes = SelectListFactory.GetDealClosingCostTypeSelectList(AdminRepository.GetAllDealClosingCostTypes());
			return View("New", model);
		}

		//
		// POST: /Deal/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			DealDetail model = new DealDetail();
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
				deal.DealNumber = model.DealNumber;
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
				//if (deal.Contact1 == null) {
				//    deal.Contact1 = new Contact();
				//    deal.Contact1.CreatedBy = AppSettings.CreatedByUserId;
				//    deal.Contact1.CreatedDate = DateTime.Now;
				//}
				//deal.Contact1.EntityID = (int)ConfigUtil.CurrentEntityID;
				//deal.Contact1.ContactName = model.ContactName;
				//deal.Contact1.FirstName = model.SellerName;
				//deal.Contact1.LastName = "n/a";
				//deal.Contact1.LastUpdatedBy = AppSettings.CreatedByUserId;
				//deal.Contact1.LastUpdatedDate = DateTime.Now;
				//AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.Email, model.Email);
				//AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.Fax, model.Fax);
				//AddCommunication(deal.Contact1, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
				IEnumerable<ErrorInfo> errorInfo = DealRepository.SaveDeal(deal);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
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
			DealDetail deal = new DealDetail();
			deal.DealNumber = DealRepository.GetMaxDealNumber(fundId) + 1;
			return Json(deal, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeal/5
		public JsonResult FindDeal(int dealId) {
			Models.Entity.Deal deal = DealRepository.FindDeal(dealId);
			DealDetail dealDetail = new DealDetail();
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
			var index = 0;
			foreach (var dealClosingCost in deal.DealClosingCosts) {
				index++;
				dealDetail.DealExpenses.Add(new DealClosingCostModel {
					Amount = dealClosingCost.Amount,
					Date = (dealClosingCost.Date ?? Convert.ToDateTime("01/01/1900")),
					DealId = dealClosingCost.DealID,
					DealClosingCostId = dealClosingCost.DealClosingCostID,
					DealClosingCostTypeId = dealClosingCost.DealClosingCostTypeID,
					Description = dealClosingCost.DealClosingCostType.Name
				});
			}
			return Json(dealDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeals
		[HttpGet]
		public JsonResult FindDeals(string term) {
			return Json(DealRepository.FindDeals(term), JsonRequestBehavior.AllowGet);
		}

		#region DealExpense
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
			return Json(new DealClosingCostModel {
				Amount = dealClosingCost.Amount,
				Date = (dealClosingCost.Date ?? Convert.ToDateTime("01/01/1900")),
				DealClosingCostId = dealClosingCost.DealClosingCostID,
				DealClosingCostTypeId = dealClosingCost.DealClosingCostTypeID,
				DealId = dealClosingCost.DealID,
				Description = dealClosingCost.DealClosingCostType.Name
			}, JsonRequestBehavior.AllowGet);
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

		public ActionResult Result() {
			return View();
		}
	}
}
