using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Helpers;
using DeepBlue.Models.Issuer;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Controllers.Issuer {
	public class IssuerController : Controller {

		public IIssuerRepository IssuerRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }


		public IssuerController()
			: this(new IssuerRepository(), new AdminRepository()) {
		}

		public IssuerController(IIssuerRepository issuerRepository, IAdminRepository adminRepository) {
			IssuerRepository = issuerRepository;
			AdminRepository = adminRepository;
		}

		#region Issuer
		//
		// GET: /Issuer/
		public ActionResult Index() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "Issuer";
			return View();
		}

		//
		// GET: /Issuer/IssuerList
		[HttpGet]
		public JsonResult IssuerList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<IssuerListModel> issuers = IssuerRepository.GetAllIssuers(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var issuer in issuers) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { issuer.IssuerId, issuer.Name, issuer.ParentName, issuer.Country }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Issuer/Issuer
		[HttpGet]
		public ActionResult EditIssuer(int id) {
			EditIssuerModel model = new EditIssuerModel();
			model.IssuerId = id;
			model.Countries = SelectListFactory.GetCountrySelectList(AdminRepository.GetAllCountries());
			model.EquityTypes = SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
			model.FixedIncomeTypes = SelectListFactory.GetFixedIncomeTypesSelectList(AdminRepository.GetAllFixedIncomeTypes());
			model.Industries = SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
			model.Currencies = SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
			model.ShareClassTypes = SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
			return View(model);
		}

		//
		// GET: /Issuer/FindIssuer
		[HttpGet]
		public JsonResult FindIssuer(int id) {
			EditIssuerModel model = IssuerRepository.FindIssuerModel(id);
			if (model == null) model = new EditIssuerModel();
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Issuer/UpdateIssuer
		[HttpPost]
		public ActionResult UpdateIssuer(FormCollection collection) {
			EditIssuerModel model = new EditIssuerModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = IssuerNameAvailable(model.Name, model.IssuerId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.Issuer issuer = IssuerRepository.FindIssuer(model.IssuerId);
				if (issuer == null) {
					issuer = new Models.Entity.Issuer();
				}
				issuer.Name = model.Name;
				issuer.ParentName = model.ParentName;
				issuer.CountryID = model.CountryId;
				issuer.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = IssuerRepository.SaveIssuer(issuer);
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

		[HttpGet]
		public string DeleteIssuer(int id) {
			if (IssuerRepository.DeleteIssuer(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string IssuerNameAvailable(string Name, int IssuerID) {
			if (IssuerRepository.IssuerNameAvailable(Name, IssuerID))
				return "Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region Equity
		//
		// GET: /Issuer/CreateEquity
		[HttpPost]
		public ActionResult CreateEquity(FormCollection collection) {
			EquityDetailModel model = new EquityDetailModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.Equity equity = IssuerRepository.FindEquity(model.EquityId);
				if (equity == null) {
					equity = new Models.Entity.Equity();
				}
				equity.CurrencyID = ((model.CurrencyId ?? 0) > 0 ? model.CurrencyId : null);
				equity.EquityTypeID = model.EquityTypeId;
				equity.IndustryID = ((model.IndustryId ?? 0) > 0 ? model.IndustryId : null);
				equity.IssuerID = model.IssuerId;
				equity.Public = model.Public;
				equity.ShareClassTypeID = ((model.ShareClassTypeId ?? 0)>0 ? model.ShareClassTypeId : null);
				equity.Symbol = model.Symbol;
				equity.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = IssuerRepository.SaveEquity(equity);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + equity.EquityID;
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
		// GET: /Issuer/FindEquity
		[HttpGet]
		public JsonResult FindEquity(int equityId) {
			return Json(IssuerRepository.FindEquityModel(equityId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Issuer/DeleteEquity
		[HttpGet]
		public string DeleteEquity(int id) {
			if (IssuerRepository.DeleteEquity(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}
		#endregion

		#region FixedIncome

		//
		// GET: /Issuer/CreateFixedIncome
		[HttpPost]
		public ActionResult CreateFixedIncome(FormCollection collection) {
			FixedIncomeDetailModel model = new FixedIncomeDetailModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
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
				IEnumerable<ErrorInfo> errorInfo = IssuerRepository.SaveFixedIncome(fixedIncome);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + fixedIncome.FixedIncomeID;
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
		// GET: /Issuer/FindFixedIncome
		[HttpGet]
		public JsonResult FindFixedIncome(int fixedIncomeId) {
			return Json(IssuerRepository.FindFixedIncomeModel(fixedIncomeId), JsonRequestBehavior.AllowGet);
		}
		
		//
		// GET: /Issuer/DeleteFixedIncome
		[HttpGet]
		public string DeleteFixedIncome(int id) {
			if (IssuerRepository.DeleteFixedIncome(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}
		#endregion

	}
}
