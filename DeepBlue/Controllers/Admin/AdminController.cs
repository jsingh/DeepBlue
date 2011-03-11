using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;
using DeepBlue.Models.Admin.Enums;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Admin {
	public class AdminController : Controller {

		public IAdminRepository AdminRepository { get; set; }

		public AdminController()
			: this(new AdminRepository()) {
		}

		public AdminController(IAdminRepository repository) {
			AdminRepository = repository;
		}

		#region InvestorType
		//
		// GET: /Admin/InvestorType
		[HttpGet]
		public ActionResult InvestorType() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/InvestorTypeList
		[HttpGet]
		public ActionResult InvestorTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<InvestorType> investorTypes = AdminRepository.GetAllInvestorTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(investorTypes);
		}

		//
		// GET: /Admin/InvestorType
		[HttpGet]
		public ActionResult EditInvestorType(int id) {
			EditInvestorTypeModel model = new EditInvestorTypeModel();
			InvestorType investorType = AdminRepository.FindInvestorType(id);
			if (investorType != null) {
				model.InvestorTypeId = investorType.InvestorTypeID;
				model.InvestorTypeName = investorType.InvestorTypeName;
				model.Enabled = investorType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateInvestorType
		[HttpPost]
		public ActionResult UpdateInvestorType(FormCollection collection) {
			EditInvestorTypeModel model = new EditInvestorTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				InvestorType investorType = AdminRepository.FindInvestorType(model.InvestorTypeId);
				if (investorType == null) {
					investorType = new InvestorType();
				}
				investorType.InvestorTypeName = model.InvestorTypeName;
				investorType.Enabled = model.Enabled;
				investorType.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveInvestorType(investorType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage;
					}
				} else {
					resultModel.Result = "True";
				}
			} else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage;
						}
					}
				}
			}
			return View("Result", resultModel);
		}


		[HttpGet]
		public string DeleteInvestorType(int id) {
			bool isRelationExist = false;
			if (AdminRepository.DeleteInvestorType(id, ref isRelationExist) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string InvestorTypeNameAvailable(string InvestorTypeName, int InvestorTypeID) {
			if (AdminRepository.InvestorTypeNameAvailable(InvestorTypeName, InvestorTypeID))
				return "Investor Type Name already exist";
			else
				return string.Empty;
		}

		#endregion

		#region EntityType

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EntityType(){
			ViewData["MenuName"] = "Admin";
			return View();
		}
 
		//
		// GET: /Admin/EntityTypeList
		[HttpGet]
		public ActionResult EntityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<InvestorEntityType> investorEntityTypes = AdminRepository.GetAllInvestorEntityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(investorEntityTypes);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EditInvestorEntityType(int id) {
			EditInvestorEntityTypeModel model = new EditInvestorEntityTypeModel();
			InvestorEntityType investorEntityType = AdminRepository.FindInvestorEntityType(id);
			if (investorEntityType != null) {
				model.InvestorEntityTypeId = investorEntityType.InvestorEntityTypeID;
				model.InvestorEntityTypeName = investorEntityType.InvestorEntityTypeName;
				model.Enabled = investorEntityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateInvestorEntityType
		[HttpPost]
		public ActionResult UpdateInvestorEntityType(FormCollection collection) {
			EditInvestorEntityTypeModel model = new EditInvestorEntityTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				InvestorEntityType investorEntityType = AdminRepository.FindInvestorEntityType(model.InvestorEntityTypeId);
				if (investorEntityType == null) {
					investorEntityType = new InvestorEntityType();
				}
				investorEntityType.InvestorEntityTypeName = model.InvestorEntityTypeName;
				investorEntityType.Enabled = model.Enabled;
				investorEntityType.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveInvestorEntityType(investorEntityType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage;
					}
				} else {
					resultModel.Result = "True";
				}
			} else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage;
						}
					}
				}
			}
			return View("Result", resultModel);
		}
		
		[HttpGet]
		public string DeleteInvestorEntityType(int id) {
			bool isRelationExist = false;
			if (AdminRepository.DeleteInvestorEntityType(id, ref isRelationExist) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string InvestorEntityTypeNameAvailable(string InvestorEntityTypeName, int InvestorEntityTypeID) {
			if (AdminRepository.InvestorEntityTypeNameAvailable(InvestorEntityTypeName, InvestorEntityTypeID))
				return "Investor Entity Type Name already exists.";
			else
				return string.Empty;
		}

		#endregion


		public ActionResult Result() {
			return View();
		}
	}
}
