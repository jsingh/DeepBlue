using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;
using DeepBlue.Models.Admin.Enums;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Transaction;

namespace DeepBlue.Controllers.Admin {
	public class AdminController : Controller {

		public IAdminRepository AdminRepository { get; set; }

		public ITransactionRepository TransactionRepository { get; set; }

		public AdminController()
			: this(new AdminRepository(), new TransactionRepository()) {
		}

		public AdminController(IAdminRepository adminRepository, ITransactionRepository transactionRepository) {
			AdminRepository = adminRepository;
			TransactionRepository = transactionRepository;
		}

		#region InvestorType
		//
		// GET: /Admin/InvestorType
		[HttpGet]
		public ActionResult InvestorType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminInvestor";
			ViewData["PageName"] = "InvestorType";
			return View();
		}

		//
		// GET: /Admin/InvestorTypeList
		[HttpGet]
		public JsonResult InvestorTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<InvestorType> investorTypes = AdminRepository.GetAllInvestorTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var invertorType in investorTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { invertorType.InvestorTypeID,
						invertorType.InvestorTypeName,
					   invertorType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
			string ErrorMessage = InvestorTypeNameAvailable(model.InvestorTypeName, model.InvestorTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("InvestorTypeName", ErrorMessage);
			}
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
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteInvestorType(int id) {
			if (AdminRepository.DeleteInvestorType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
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
		public ActionResult EntityType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminInvestor";
			ViewData["PageName"] = "EntityType";
			return View();
		}

		//
		// GET: /Admin/EntityTypeList
		[HttpGet]
		public JsonResult EntityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.InvestorEntityType> investorEntityTypes = AdminRepository.GetAllInvestorEntityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var investorEntityType in investorEntityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {investorEntityType.InvestorEntityTypeID,
					  investorEntityType.InvestorEntityTypeName,
					  investorEntityType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
			string ErrorMessage = InvestorEntityTypeNameAvailable(model.InvestorEntityTypeName, model.InvestorEntityTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("InvestorEntityTypeName", ErrorMessage);
			}
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
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteInvestorEntityType(int id) {
			if (AdminRepository.DeleteInvestorEntityType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
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

		#region FundClosing
		//
		// GET: /Admin/FundClosing
		[HttpGet]
		public ActionResult FundClosing() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminFund";
			ViewData["PageName"] = "FundClosing";
			return View();
		}

		//
		// GET: /Admin/FundClosingList
		[HttpGet]
		public JsonResult FundClosingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.FundClosing> fundClosings = AdminRepository.GetAllFundClosings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fundClosing in fundClosings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {fundClosing.FundClosingID,
					  (fundClosing.FundClosingDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"),
					  fundClosing.Name,
					  fundClosing.Fund.FundName,
					  fundClosing.IsFirstClosing}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/FundClosing
		[HttpGet]
		public ActionResult EditFundClosing(int id) {
			EditFundClosingModel model = new EditFundClosingModel();
			FundClosing fundClosing = AdminRepository.FindFundClosing(id);
			if (fundClosing != null) {
				model.FundClosingID = fundClosing.FundClosingID;
				model.Name = fundClosing.Name;
				model.FundClosingDate = fundClosing.FundClosingDate;
				model.FundID = fundClosing.FundID;
				model.IsFirstClosing = fundClosing.IsFirstClosing;
			}
			model.FundNames = SelectListFactory.GetFundSelectList(TransactionRepository.GetAllFundNames());
			return View(model);
		}

		////
		//// GET: /Admin/UpdateFundClosing
		[HttpPost]
		public ActionResult UpdateFundClosing(FormCollection collection) {
			EditFundClosingModel model = new EditFundClosingModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = FundClosingNameAvailable(model.Name, model.FundClosingID, model.FundID);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FundClosing fundClosing = AdminRepository.FindFundClosing(model.FundClosingID);
				if (fundClosing == null) {
					fundClosing = new FundClosing();
				}
				fundClosing.Name = model.Name;
				fundClosing.IsFirstClosing = model.IsFirstClosing;
				fundClosing.FundClosingDate = model.FundClosingDate;
				fundClosing.FundID = model.FundID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveFundClosing(fundClosing);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteFundClosing(int id) {
			if (AdminRepository.DeleteFundClosing(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FundClosingNameAvailable(string Name, int FundClosingID, int FundID) {
			if (AdminRepository.FundClosingNameAvailable(Name, FundClosingID, FundID))
				return "Name is already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Custom Field

		public ActionResult CustomField() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminCustomField";
			ViewData["PageName"] = "CustomField";
			return View();
		}

		//
		// GET: /Admin/CustomFieldList
		[HttpGet]
		public JsonResult CustomFieldList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.CustomField> customFields = AdminRepository.GetAllCustomFields(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var customField in customFields) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {customField.CustomFieldID,
					  customField.CustomFieldText,
					  customField.MODULE.ModuleName,
					  customField.DataType.DataTypeName,
					  customField.Search}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EditCustomField(int id) {
			EditCustomFieldModel model = new EditCustomFieldModel();
			CustomField customField = AdminRepository.FindCustomField(id);
			model.Modules = SelectListFactory.GetModuleSelectList(AdminRepository.GetAllModules());
			model.DataTypes = SelectListFactory.GetDataTypeSelectList(AdminRepository.GetAllDataTypes());
			model.OptionFields = new List<EditOptionFieldModel>();
			if (customField != null) {
				model.CustomFieldId = customField.CustomFieldID;
				model.CustomFieldText = customField.CustomFieldText;
				model.DataTypeId = customField.DataTypeID;
				model.ModuleId = customField.ModuleID;
				model.OptionalText = customField.OptionalText;
				model.Search = customField.Search;
				foreach (var field in customField.OptionFields) {
					model.OptionFields.Add(new EditOptionFieldModel {
						CustomFieldId = field.CustomFieldID,
						IsDefault = field.IsDefault,
						OptionFieldId = field.OptionFieldID,
						OptionText = field.OptionText,
						SortOrder = field.SortOrder
					});
				}
			}
			else {
				model.OptionFields.Add(new EditOptionFieldModel());
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateCustomField
		[HttpPost]
		public ActionResult UpdateCustomField(FormCollection collection) {
			EditCustomFieldModel model = new EditCustomFieldModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = CustomFieldTextAvailable(model.CustomFieldText, model.CustomFieldId, model.ModuleId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("CustomFieldText", ErrorMessage);
			}
			if (ModelState.IsValid) {
				CustomField customField = AdminRepository.FindCustomField(model.CustomFieldId);
				if (customField == null) {
					customField = new CustomField();
				}
				customField.CustomFieldText = model.CustomFieldText;
				customField.DataTypeID = model.DataTypeId;
				customField.ModuleID = model.ModuleId;
				customField.OptionalText = model.OptionalText;
				customField.Search = model.Search;
				customField.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCustomField(customField);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteCustomField(int id) {
			if (AdminRepository.DeleteCustomField(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CustomFieldTextAvailable(string CustomFieldText, int CustomFieldId, int ModuleId) {
			if (AdminRepository.CustomFieldTextAvailable(CustomFieldText, CustomFieldId, ModuleId))
				return "Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Data Type

		public ActionResult DataType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminCustomField";
			ViewData["PageName"] = "DataType";
			return View();
		}

		//
		// GET: /Admin/DataTypeList
		[HttpGet]
		public JsonResult DataTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.DataType> dataTypes = AdminRepository.GetAllDataTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var dataType in dataTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {dataType.DataTypeID,
					  dataType.DataTypeName}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/DataType
		[HttpGet]
		public ActionResult EditDataType(int id) {
			EditDataTypeModel model = new EditDataTypeModel();
			DataType dataType = AdminRepository.FindDataType(id);
			if (dataType != null) {
				model.DataTypeId = dataType.DataTypeID;
				model.DataTypeName = dataType.DataTypeName;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateDataType
		[HttpPost]
		public ActionResult UpdateDataType(FormCollection collection) {
			EditDataTypeModel model = new EditDataTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = DataTypeNameAvailable(model.DataTypeName, model.DataTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("DataTypeName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				DataType dataType = AdminRepository.FindDataType(model.DataTypeId);
				if (dataType == null) {
					dataType = new DataType();
				}
				dataType.DataTypeName = model.DataTypeName;
				dataType.DataTypeID = model.DataTypeId;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveDataType(dataType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteDataType(int id) {
			if (AdminRepository.DeleteDataType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string DataTypeNameAvailable(string DataTypeName, int DataTypeId) {
			if (AdminRepository.DataTypeNameAvailable(DataTypeName, DataTypeId))
				return "Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Module

		public ActionResult MODULE() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/ModuleList
		[HttpGet]
		public JsonResult ModuleList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.MODULE> modules = AdminRepository.GetAllModules(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var module in modules) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {module.ModuleID,
					  module.ModuleName}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EditModule(int id) {
			EditModule model = new EditModule();
			MODULE module = AdminRepository.FindModule(id);
			if (module != null) {
				model.ModuleID = module.ModuleID;
				model.ModuleName = module.ModuleName;

			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateModule
		[HttpPost]
		public ActionResult UpdateModule(FormCollection collection) {
			EditModule model = new EditModule();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = ModuleFieldTextAvailable(model.ModuleName, model.ModuleID);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("ModuleName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				MODULE module = AdminRepository.FindModule(model.ModuleID);
				if (module == null) {
					module = new MODULE();
				}
				module.ModuleName = model.ModuleName;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveModule(module);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteModule(int id) {
			if (AdminRepository.DeleteModuleId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ModuleFieldTextAvailable(string ModuleFieldText, int ModuleFieldId) {
			if (AdminRepository.ModuleTextAvailable(ModuleFieldText, ModuleFieldId))
				return "Custom Field Text already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region Communication Type
		//
		// GET: /Admin/CommunicationType
		[HttpGet]
		public ActionResult CommunicationType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminInvestor";
			ViewData["PageName"] = "CommunicationType";
			return View();
		}

		//
		// GET: /Admin/CommunicationTypeList
		[HttpGet]
		public JsonResult CommunicationTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.CommunicationType> communicationTypes = AdminRepository.GetAllCommunicationTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var communicationType in communicationTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { communicationType.CommunicationTypeID,
						communicationType.CommunicationTypeName,
					   communicationType.CommunicationGrouping.CommunicationGroupingName,
					  communicationType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CommunicationType
		[HttpGet]
		public ActionResult EditCommunicationType(int id) {
			EditCommunicationTypeModel model = new EditCommunicationTypeModel();
			DeepBlue.Models.Entity.CommunicationType communicationType = AdminRepository.FindCommunicationType(id);
			model.CommunicationGroupings = SelectListFactory.GetCommunicationGroupingSelectList(AdminRepository.GetAllCommunicationGroupings());
			if (communicationType != null) {
				model.CommunicationTypeId = communicationType.CommunicationTypeID;
				model.CommunicationTypeName = communicationType.CommunicationTypeName;
				model.Enabled = communicationType.Enabled;
				model.CommunicationGroupId = communicationType.CommunicationGroupingID;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateCommunicationType
		[HttpPost]
		public ActionResult UpdateCommunicationType(FormCollection collection) {
			EditCommunicationTypeModel model = new EditCommunicationTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = CommunicationTypeNameAvailable(model.CommunicationTypeName, model.CommunicationTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("CommunicationTypeName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				DeepBlue.Models.Entity.CommunicationType communicationType = AdminRepository.FindCommunicationType(model.CommunicationTypeId);
				if (communicationType == null) {
					communicationType = new DeepBlue.Models.Entity.CommunicationType();
				}
				communicationType.CommunicationTypeName = model.CommunicationTypeName;
				communicationType.Enabled = model.Enabled;
				communicationType.EntityID = (int)ConfigUtil.CurrentEntityID;
				communicationType.CommunicationGroupingID = model.CommunicationGroupId;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCommunicationType(communicationType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteCommunicationType(int id) {
			if (AdminRepository.DeleteCommunicationType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CommunicationTypeNameAvailable(string CommunicationTypeName, int CommunicationTypeID) {
			if (AdminRepository.CommunicationTypeNameAvailable(CommunicationTypeName, CommunicationTypeID))
				return "Communication Type Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region Communication Grouping
		//
		// GET: /Admin/CommunicationGrouping
		[HttpGet]
		public ActionResult CommunicationGrouping() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminInvestor";
			ViewData["PageName"] = "CommunicationGrouping";
			return View();
		}

		//
		// GET: /Admin/CommunicationGroupingList
		[HttpGet]
		public JsonResult CommunicationGroupingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<Models.Entity.CommunicationGrouping> communicationGroupings = AdminRepository.GetAllCommunicationGroupings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var communicationGroup in communicationGroupings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { communicationGroup.CommunicationGroupingID, communicationGroup.CommunicationGroupingName }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CommunicationGrouping
		[HttpGet]
		public ActionResult EditCommunicationGrouping(int id) {
			EditCommunicationGroupingModel model = new EditCommunicationGroupingModel();
			Models.Entity.CommunicationGrouping communicationGrouping = AdminRepository.FindCommunicationGrouping(id);
			if (communicationGrouping != null) {
				model.CommunicationGroupingId = communicationGrouping.CommunicationGroupingID;
				model.CommunicationGroupingName = communicationGrouping.CommunicationGroupingName;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateCommunicationGrouping
		[HttpPost]
		public ActionResult UpdateCommunicationGrouping(FormCollection collection) {
			EditCommunicationGroupingModel model = new EditCommunicationGroupingModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = CommunicationGroupingNameAvailable(model.CommunicationGroupingName, model.CommunicationGroupingId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("CommunicationGroupingName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Models.Entity.CommunicationGrouping communicationGrouping = AdminRepository.FindCommunicationGrouping(model.CommunicationGroupingId);
				if (communicationGrouping == null) {
					communicationGrouping = new Models.Entity.CommunicationGrouping();
				}
				communicationGrouping.CommunicationGroupingName = model.CommunicationGroupingName;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCommunicationGrouping(communicationGrouping);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteCommunicationGrouping(int id) {
			if (AdminRepository.DeleteCommunicationGrouping(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CommunicationGroupingNameAvailable(string CommunicationGroupingName, int CommunicationGroupingID) {
			if (AdminRepository.CommunicationGroupingNameAvailable(CommunicationGroupingName, CommunicationGroupingID))
				return "Communication Grouping Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region Purchase Type
		//
		// GET: /Admin/PurchaseType
		[HttpGet]
		public ActionResult PurchaseType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "PurchaseType";
			return View();
		}

		//
		// GET: /Admin/PurchaseTypeList
		[HttpGet]
		public JsonResult PurchaseTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.PurchaseType> purchaseTypes = AdminRepository.GetAllPurchaseTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var purchaseType in purchaseTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {purchaseType.PurchaseTypeID,
					  purchaseType.Name}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/PurchaseType
		[HttpGet]
		public ActionResult EditPurchaseType(int id) {
			EditPurchaseTypeModel model = new EditPurchaseTypeModel();
			PurchaseType purchaseType = AdminRepository.FindPurchaseType(id);
			if (purchaseType != null) {
				model.PurchaseTypeId = purchaseType.PurchaseTypeID;
				model.Name = purchaseType.Name;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdatePurchaseType
		[HttpPost]
		public ActionResult UpdatePurchaseType(FormCollection collection) {
			EditPurchaseTypeModel model = new EditPurchaseTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = PurchaseTypeNameAvailable(model.Name, model.PurchaseTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				PurchaseType purchaseType = AdminRepository.FindPurchaseType(model.PurchaseTypeId);
				if (purchaseType == null) {
					purchaseType = new PurchaseType();
				}
				purchaseType.Name = model.Name;
				purchaseType.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SavePurchaseType(purchaseType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeletePurchaseType(int id) {
			if (AdminRepository.DeletePurchaseType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string PurchaseTypeNameAvailable(string Name, int PurchaseTypeID) {
			if (AdminRepository.PurchaseTypeNameAvailable(Name, PurchaseTypeID))
				return "Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region Deal Closing Cost Type
		//
		// GET: /Admin/DealClosingCostType
		[HttpGet]
		public ActionResult DealClosingCostType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "DealClosingCostType";
			return View();
		}

		//
		// GET: /Admin/DealClosingCostTypeList
		[HttpGet]
		public JsonResult DealClosingCostTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.DealClosingCostType> dealClosingCostTypes = AdminRepository.GetAllDealClosingCostTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var dealClosingCostType in dealClosingCostTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {dealClosingCostType.DealClosingCostTypeID,
					  dealClosingCostType.Name}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/DealClosingCostType
		[HttpGet]
		public ActionResult EditDealClosingCostType(int id) {
			EditDealClosingCostTypeModel model = new EditDealClosingCostTypeModel();
			DealClosingCostType DealClosingCostType = AdminRepository.FindDealClosingCostType(id);
			if (DealClosingCostType != null) {
				model.DealClosingCostTypeId = DealClosingCostType.DealClosingCostTypeID;
				model.Name = DealClosingCostType.Name;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateDealClosingCostType
		[HttpPost]
		public ActionResult UpdateDealClosingCostType(FormCollection collection) {
			EditDealClosingCostTypeModel model = new EditDealClosingCostTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = DealClosingCostTypeNameAvailable(model.Name, model.DealClosingCostTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				DealClosingCostType DealClosingCostType = AdminRepository.FindDealClosingCostType(model.DealClosingCostTypeId);
				if (DealClosingCostType == null) {
					DealClosingCostType = new DealClosingCostType();
				}
				DealClosingCostType.Name = model.Name;
				DealClosingCostType.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveDealClosingCostType(DealClosingCostType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteDealClosingCostType(int id) {
			if (AdminRepository.DeleteDealClosingCostType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string DealClosingCostTypeNameAvailable(string Name, int DealClosingCostTypeID) {
			if (AdminRepository.DealClosingCostTypeNameAvailable(Name, DealClosingCostTypeID))
				return "Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region Security Type
		//
		// GET: /Admin/SecurityType
		[HttpGet]
		public ActionResult SecurityType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "SecurityType";
			return View();
		}

		//
		// GET: /Admin/SecurityTypeList
		[HttpGet]
		public JsonResult SecurityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.SecurityType> securityTypes = AdminRepository.GetAllSecurityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var securityType in securityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {securityType.SecurityTypeID,
					  securityType.Name,
					  securityType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/SecurityType
		[HttpGet]
		public ActionResult EditSecurityType(int id) {
			EditSecurityTypeModel model = new EditSecurityTypeModel();
			SecurityType securityType = AdminRepository.FindSecurityType(id);
			if (securityType != null) {
				model.SecurityTypeId = securityType.SecurityTypeID;
				model.Name = securityType.Name;
				model.Enabled = securityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateSecurityType
		[HttpPost]
		public ActionResult UpdateSecurityType(FormCollection collection) {
			EditSecurityTypeModel model = new EditSecurityTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = SecurityTypeNameAvailable(model.Name, model.SecurityTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				SecurityType securityType = AdminRepository.FindSecurityType(model.SecurityTypeId);
				if (securityType == null) {
					securityType = new SecurityType();
				}
				securityType.Name = model.Name;
				securityType.Enabled = model.Enabled;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveSecurityType(securityType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteSecurityType(int id) {
			if (AdminRepository.DeleteSecurityType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string SecurityTypeNameAvailable(string Name, int SecurityTypeID) {
			if (AdminRepository.SecurityTypeNameAvailable(Name, SecurityTypeID))
				return "Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region UnderlyingFundType

		public ActionResult UnderlyingFundType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "UnderlyingFundType";
			return View();
		}

		//
		// GET: /Admin/UnderlyingList
		[HttpGet]
		public JsonResult UnderlyingFundTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.UnderlyingFundType> underlyingFundTypes = AdminRepository.GetAllUnderlyingFundTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var underlyingFundType in underlyingFundTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {underlyingFundType.UnderlyingFundTypeID,
					  underlyingFundType.Name}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditUnderlyingFundType
		[HttpGet]
		public ActionResult EditUnderlyingFundType(int id) {
			EditUnderlyingFundTypeModel model = new EditUnderlyingFundTypeModel();
			UnderlyingFundType underlyinfundtype = AdminRepository.FindUnderlyingFundType(id);
			if (underlyinfundtype != null) {
				model.UnderlyingFundTypeId = underlyinfundtype.UnderlyingFundTypeID;
				model.Name = underlyinfundtype.Name;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateUnderlyingFundType
		[HttpPost]
		public ActionResult UpdateUnderlyingFundType(FormCollection collection) {
			EditUnderlyingFundTypeModel model = new EditUnderlyingFundTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = UnderlyingFundTypeNameAvailable(model.Name, model.UnderlyingFundTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				UnderlyingFundType underlyingFundType = AdminRepository.FindUnderlyingFundType(model.UnderlyingFundTypeId);
				if (underlyingFundType == null) {
					underlyingFundType = new UnderlyingFundType();
				}
				underlyingFundType.Name = model.Name;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveUnderlyingFundType(underlyingFundType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteUnderlyingFundType(int id) {
			if (AdminRepository.DeleteUnderlyingFundTypeId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string UnderlyingFundTypeNameAvailable(string Name, int UnderlyingFundTypeId) {
			if (AdminRepository.UnderlyingFundTypeNameAvailable(Name, UnderlyingFundTypeId))
				return "Name already exists.";
			else
				return string.Empty;
		}


		
		#endregion

		#region ShareClassType

		public ActionResult ShareClassType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "ShareClassType";
			return View();
		}

		//
		// GET: /Admin/ShareClassTypeList
		[HttpGet]
		public JsonResult ShareClassTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.ShareClassType> shareClassTypes = AdminRepository.GetAllShareClassTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var shareClassType in shareClassTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {shareClassType.ShareClassTypeID,
					  shareClassType.ShareClass,
					  shareClassType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditShareClassType
		[HttpGet]
		public ActionResult EditShareClassType(int id) {
			EditShareClassTypeModel model = new EditShareClassTypeModel();
			ShareClassType shareClassType = AdminRepository.FindShareClassType(id);
			if (shareClassType != null) {
				model.ShareClassTypeId = shareClassType.ShareClassTypeID;
				model.ShareClass = shareClassType.ShareClass;
				model.Enabled = shareClassType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateShareClassType
		[HttpPost]
		public ActionResult UpdateShareClassType(FormCollection collection) {
			EditShareClassTypeModel model = new EditShareClassTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = ShareClassAvailable(model.ShareClass, model.ShareClassTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				ShareClassType shareClassType = AdminRepository.FindShareClassType(model.ShareClassTypeId);
				if (shareClassType == null) {
					shareClassType = new ShareClassType();
					shareClassType.CreatedBy = AppSettings.CreatedByUserId;
					shareClassType.CreatedDate = DateTime.Now;
				}
				shareClassType.ShareClass = model.ShareClass;
				shareClassType.Enabled = model.Enabled;
				shareClassType.EntityID = (int)ConfigUtil.CurrentEntityID;
				shareClassType.LastUpdatedBy = AppSettings.CreatedByUserId;
				shareClassType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveShareClassType(shareClassType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteShareClassType(int id) {
			if (AdminRepository.DeleteShareClassTypeId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ShareClassAvailable(string ShareClass, int ShareClassTypeId) {
			if (AdminRepository.ShareClassTypeNameAvailable(ShareClass, ShareClassTypeId))
				return "Share Class already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region ReportingType

		public ActionResult ReportingType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "ReportingType";
			return View();
		}

		//
		// GET: /Admin/ReportingTypeList
		[HttpGet]
		public JsonResult ReportingTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.ReportingType> reportingTypes = AdminRepository.GetAllReportingTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var reportingType in reportingTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {reportingType.ReportingTypeID,
					  reportingType.Reporting,
					  reportingType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditReportingType
		[HttpGet]
		public ActionResult EditReportingType(int id) {
			EditReportingTypeModel model = new EditReportingTypeModel();
			ReportingType reportingType = AdminRepository.FindReportingType(id);
			if (reportingType != null) {
				model.ReportingTypeId = reportingType.ReportingTypeID;
				model.Reporting = reportingType.Reporting;
				model.Enabled = reportingType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateReportingType
		[HttpPost]
		public ActionResult UpdateReportingType(FormCollection collection) {
			EditReportingTypeModel model = new EditReportingTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = ReportingTypeAvailable(model.Reporting, model.ReportingTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				ReportingType reportingType = AdminRepository.FindReportingType(model.ReportingTypeId);
				if (reportingType == null) {
					reportingType = new ReportingType();
					reportingType.CreatedBy = AppSettings.CreatedByUserId;
					reportingType.CreatedDate = DateTime.Now;
				}
				reportingType.Reporting = model.Reporting;
				reportingType.Enabled = model.Enabled;
				reportingType.EntityID = (int)ConfigUtil.CurrentEntityID;
				reportingType.LastUpdatedBy = AppSettings.CreatedByUserId;
				reportingType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveReportingType(reportingType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteReportingType(int id) {
			if (AdminRepository.DeleteReportingTypeId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingTypeAvailable(string Reporting, int ReportingTypeId) {
			if (AdminRepository.ReportingTypeNameAvailable(Reporting, ReportingTypeId))
				return "Reporting Type already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region ReportingFrequency

		public ActionResult ReportingFrequency() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "ReportingFrequency";
			return View();
		}

		//
		// GET: /Admin/ReportingFrequencyList
		[HttpGet]
		public JsonResult ReportingFrequencyList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.ReportingFrequency> reportingFrequencys = AdminRepository.GetAllReportingFrequencies(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var reportingFrequency in reportingFrequencys) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {reportingFrequency.ReportingFrequencyID,
					  reportingFrequency.ReportingFrequency1,
					  reportingFrequency.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditReportingFrequency
		[HttpGet]
		public ActionResult EditReportingFrequency(int id) {
			EditReportingFrequencyModel model = new EditReportingFrequencyModel();
			ReportingFrequency reportingFrequency = AdminRepository.FindReportingFrequency(id);
			if (reportingFrequency != null) {
				model.ReportingFrequencyId = reportingFrequency.ReportingFrequencyID;
				model.ReportingFrequency = reportingFrequency.ReportingFrequency1;
				model.Enabled = reportingFrequency.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateReportingFrequency
		[HttpPost]
		public ActionResult UpdateReportingFrequency(FormCollection collection) {
			EditReportingFrequencyModel model = new EditReportingFrequencyModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = ReportingFrequencyAvailable(model.ReportingFrequency, model.ReportingFrequencyId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				ReportingFrequency reportingFrequency = AdminRepository.FindReportingFrequency(model.ReportingFrequencyId);
				if (reportingFrequency == null) {
					reportingFrequency = new ReportingFrequency();
					reportingFrequency.CreatedBy = AppSettings.CreatedByUserId;
					reportingFrequency.CreatedDate = DateTime.Now;
				}
				reportingFrequency.ReportingFrequency1 = model.ReportingFrequency;
				reportingFrequency.Enabled = model.Enabled;
				reportingFrequency.EntityID = (int)ConfigUtil.CurrentEntityID;
				reportingFrequency.LastUpdatedBy = AppSettings.CreatedByUserId;
				reportingFrequency.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveReportingFrequency(reportingFrequency);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteReportingFrequency(int id) {
			if (AdminRepository.DeleteReportingFrequencyId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingFrequencyAvailable(string ReportingFrequency, int ReportingFrequencyId) {
			if (AdminRepository.ReportingFrequencyNameAvailable(ReportingFrequency, ReportingFrequencyId))
				return "Reporting Frequency already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region Geography

		public ActionResult Geography() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "Geography";
			return View();
		}

		//
		// GET: /Admin/GeographyList
		[HttpGet]
		public JsonResult GeographyList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.Geography> geographys = AdminRepository.GetAllGeographys(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var geography in geographys) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {geography.GeographyID,
					  geography.Geography1,
					  geography.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditGeography
		[HttpGet]
		public ActionResult EditGeography(int id) {
			EditGeographyModel model = new EditGeographyModel();
			Geography geography = AdminRepository.FindGeography(id);
			if (geography != null) {
				model.GeographyId = geography.GeographyID;
				model.Geography = geography.Geography1;
				model.Enabled = geography.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateGeography
		[HttpPost]
		public ActionResult UpdateGeography(FormCollection collection) {
			EditGeographyModel model = new EditGeographyModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = GeographyAvailable(model.Geography, model.GeographyId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Geography geography = AdminRepository.FindGeography(model.GeographyId);
				if (geography == null) {
					geography = new Geography();
					geography.CreatedBy = AppSettings.CreatedByUserId;
					geography.CreatedDate = DateTime.Now;
				}
				geography.Geography1 = model.Geography;
				geography.Enabled = model.Enabled;
				geography.EntityID = (int)ConfigUtil.CurrentEntityID;
				geography.LastUpdatedBy = AppSettings.CreatedByUserId;
				geography.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveGeography(geography);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteGeography(int id) {
			if (AdminRepository.DeleteGeographyId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string GeographyAvailable(string Geography, int GeographyId) {
			if (AdminRepository.GeographyNameAvailable(Geography, GeographyId))
				return "Geography already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region Industry

		public ActionResult Industry() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "Industry";
			return View();
		}

		//
		// GET: /Admin/IndustryList
		[HttpGet]
		public JsonResult IndustryList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.Industry> industrys = AdminRepository.GetAllIndustrys(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var industry in industrys) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {industry.IndustryID,
					  industry.Industry1,
					  industry.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditIndustry
		[HttpGet]
		public ActionResult EditIndustry(int id) {
			EditIndustryModel model = new EditIndustryModel();
			Industry industry = AdminRepository.FindIndustry(id);
			if (industry != null) {
				model.IndustryId = industry.IndustryID;
				model.Industry = industry.Industry1;
				model.Enabled = industry.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateIndustry
		[HttpPost]
		public ActionResult UpdateIndustry(FormCollection collection) {
			EditIndustryModel model = new EditIndustryModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = IndustryAvailable(model.Industry, model.IndustryId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Industry industry = AdminRepository.FindIndustry(model.IndustryId);
				if (industry == null) {
					industry = new Industry();
					industry.CreatedBy = AppSettings.CreatedByUserId;
					industry.CreatedDate = DateTime.Now;
				}
				industry.Industry1 = model.Industry;
				industry.Enabled = model.Enabled;
				industry.EntityID = (int)ConfigUtil.CurrentEntityID;
				industry.LastUpdatedBy = AppSettings.CreatedByUserId;
				industry.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveIndustry(industry);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteIndustry(int id) {
			if (AdminRepository.DeleteIndustryId(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string IndustryAvailable(string Industry, int IndustryId) {
			if (AdminRepository.IndustryNameAvailable(Industry, IndustryId))
				return "Industry already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region File Type

		public ActionResult FileType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminFileType";
			ViewData["PageName"] = "FileType";
			return View();
		}

		//
		// GET: /Admin/FileTypeList
		[HttpGet]
		public JsonResult FileTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.FileType> fileTypes = AdminRepository.GetAllFileTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fileType in fileTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {fileType.FileTypeID,
					  fileType.FileTypeName,
					  fileType.FileExtension
					  }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EditFileType(int id) {
			EditFileTypeModel model = new EditFileTypeModel();
			FileType fileType = AdminRepository.FindFileType(id);
			if (fileType != null) {
				model.FileTypeId = fileType.FileTypeID;
				model.FileTypeName = fileType.FileTypeName;
				model.FileExtension = fileType.FileExtension;
				model.Description = fileType.Description;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateFileType
		[HttpPost]
		public ActionResult UpdateFileType(FormCollection collection) {
			EditFileTypeModel model = new EditFileTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = FileTypeNameAvailable(model.FileTypeName, model.FileTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("FileTypeText", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FileType fileType = AdminRepository.FindFileType(model.FileTypeId);
				if (fileType == null) {
					fileType = new FileType();
				}
				fileType.FileTypeName = model.FileTypeName;
				fileType.FileExtension = model.FileExtension;
				fileType.Description = model.Description;
				fileType.EntityID = (int)ConfigUtil.CurrentEntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveFileType(fileType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteFileType(int id) {
			if (AdminRepository.DeleteFileType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FileTypeNameAvailable(string FileTypeText, int FileTypeId) {
			if (AdminRepository.FileTypeNameAvailable(FileTypeText, FileTypeId))
				return "Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region EquityType
		//
		// GET: /Admin/EquityType
		[HttpGet]
		public ActionResult EquityType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "EquityType";
			return View();
		}

		//
		// GET: /Admin/EquityTypeList
		[HttpGet]
		public JsonResult EquityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.EquityType> equityTypes = AdminRepository.GetAllEquityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var equityType in equityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {equityType.EquityTypeID,
					  equityType.Equity, equityType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EquityType
		[HttpGet]
		public ActionResult EditEquityType(int id) {
			EditEquityTypeModel model = new EditEquityTypeModel();
			EquityType equityType = AdminRepository.FindEquityType(id);
			if (equityType != null) {
				model.EquityTypeId = equityType.EquityTypeID;
				model.Equity = equityType.Equity;
				model.Enabled = equityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateEquityType
		[HttpPost]
		public ActionResult UpdateEquityType(FormCollection collection) {
			EditEquityTypeModel model = new EditEquityTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = EquityTypeNameAvailable(model.Equity, model.EquityTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Equity", ErrorMessage);
			}
			if (ModelState.IsValid) {
				EquityType equityType = AdminRepository.FindEquityType(model.EquityTypeId);
				if (equityType == null) {
					equityType = new EquityType();
					equityType.CreatedBy = AppSettings.CreatedByUserId;
					equityType.CreatedDate = DateTime.Now;
				}
				equityType.Equity = model.Equity;
				equityType.Enabled = model.Enabled;
				equityType.EntityID = (int)ConfigUtil.CurrentEntityID;
				equityType.LastUpdatedBy = AppSettings.CreatedByUserId;
				equityType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveEquityType(equityType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteEquityType(int id) {
			if (AdminRepository.DeleteEquityType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string EquityTypeNameAvailable(string Equity, int EquityTypeID) {
			if (AdminRepository.EquityTypeNameAvailable(Equity, EquityTypeID))
				return "Equity already exist";
			else
				return string.Empty;
		}
		#endregion

		#region FixedIncomeType
		//
		// GET: /Admin/FixedIncomeType
		[HttpGet]
		public ActionResult FixedIncomeType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "AdminDeal";
			ViewData["PageName"] = "FixedIncomeType";
			return View();
		}

		//
		// GET: /Admin/FixedIncomeTypeList
		[HttpGet]
		public JsonResult FixedIncomeTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.FixedIncomeType> fixedIncomeTypes = AdminRepository.GetAllFixedIncomeTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fixedIncomeType in fixedIncomeTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {fixedIncomeType.FixedIncomeTypeID,
					  fixedIncomeType.FixedIncomeType1, fixedIncomeType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/FixedIncomeType
		[HttpGet]
		public ActionResult EditFixedIncomeType(int id) {
			EditFixedIncomeTypeModel model = new EditFixedIncomeTypeModel();
			FixedIncomeType fixedIncomeType = AdminRepository.FindFixedIncomeType(id);
			if (fixedIncomeType != null) {
				model.FixedIncomeTypeId = fixedIncomeType.FixedIncomeTypeID;
				model.FixedIncomeType = fixedIncomeType.FixedIncomeType1;
				model.Enabled = fixedIncomeType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateFixedIncomeType
		[HttpPost]
		public ActionResult UpdateFixedIncomeType(FormCollection collection) {
			EditFixedIncomeTypeModel model = new EditFixedIncomeTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = FixedIncomeTypeNameAvailable(model.FixedIncomeType, model.FixedIncomeTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("FixedIncome", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FixedIncomeType fixedIncomeType = AdminRepository.FindFixedIncomeType(model.FixedIncomeTypeId);
				if (fixedIncomeType == null) {
					fixedIncomeType = new FixedIncomeType();
					fixedIncomeType.CreatedBy = AppSettings.CreatedByUserId;
					fixedIncomeType.CreatedDate = DateTime.Now;
				}
				fixedIncomeType.FixedIncomeType1 = model.FixedIncomeType;
				fixedIncomeType.Enabled = model.Enabled;
				fixedIncomeType.EntityID = (int)ConfigUtil.CurrentEntityID;
				fixedIncomeType.LastUpdatedBy = AppSettings.CreatedByUserId;
				fixedIncomeType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveFixedIncomeType(fixedIncomeType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True";
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
		public string DeleteFixedIncomeType(int id) {
			if (AdminRepository.DeleteFixedIncomeType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FixedIncomeTypeNameAvailable(string FixedIncomeType, int FixedIncomeTypeID) {
			if (AdminRepository.FixedIncomeTypeNameAvailable(FixedIncomeType, FixedIncomeTypeID))
				return "Fixed Income Type already exist";
			else
				return string.Empty;
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
