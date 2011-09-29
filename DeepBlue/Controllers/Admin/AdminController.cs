﻿using System;
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
	public class AdminController : BaseController {

		public IAdminRepository AdminRepository { get; set; }

		public ITransactionRepository TransactionRepository { get; set; }

		public AdminController()
			: this(new AdminRepository(), new TransactionRepository()) {
		}

		public AdminController(IAdminRepository adminRepository, ITransactionRepository transactionRepository) {
			AdminRepository = adminRepository;
			TransactionRepository = transactionRepository;
		}

		#region InvestorMangement

		#region InvestorType
		//
		// GET: /Admin/InvestorType
		[HttpGet]
		public ActionResult InvestorType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "InvestorManagement";
			ViewData["PageName"] = "InvestorType";
			return View();
		}

		//
		// GET: /Admin/InvestorTypeList
		[HttpGet]
		public JsonResult InvestorTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<InvestorType> investorTypes = AdminRepository.GetAllInvestorTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			InvestorType investorType = AdminRepository.FindInvestorType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> { investorType.InvestorTypeID,
						investorType.InvestorTypeName,
					   investorType.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				investorType.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveInvestorType(investorType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + investorType.InvestorTypeID;
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
		public string InvestorTypeNameAvailable(string investorType, int investorTypeId) {
			if (AdminRepository.InvestorTypeNameAvailable(investorType, investorTypeId))
				return "Investor Type already exist";
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
			ViewData["SubmenuName"] = "InvestorManagement";
			ViewData["PageName"] = "InvestorEntityType";
			return View();
		}

		//
		// GET: /Admin/EntityTypeList
		[HttpGet]
		public JsonResult EntityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.InvestorEntityType> investorEntityTypes = AdminRepository.GetAllInvestorEntityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
		public JsonResult EditInvestorEntityType(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			InvestorEntityType investorEntityType = AdminRepository.FindInvestorEntityType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {investorEntityType.InvestorEntityTypeID,
					  investorEntityType.InvestorEntityTypeName,
					  investorEntityType.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				investorEntityType.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveInvestorEntityType(investorEntityType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + investorEntityType.InvestorEntityTypeID;
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
		public string InvestorEntityTypeNameAvailable(string investorEntityType, int investorEntityTypeId) {
			if (AdminRepository.InvestorEntityTypeNameAvailable(investorEntityType, investorEntityTypeId))
				return "Investor Entity Type already exists.";
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
			ViewData["SubmenuName"] = "InvestorManagement";
			ViewData["PageName"] = "CommunicationType";
			EditCommunicationTypeModel model = new EditCommunicationTypeModel();
			model.CommunicationGroupings = SelectListFactory.GetCommunicationGroupingSelectList(AdminRepository.GetAllCommunicationGroupings());
			return View(model);
		}

		//
		// GET: /Admin/CommunicationTypeList
		[HttpGet]
		public JsonResult CommunicationTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.CommunicationType> communicationTypes = AdminRepository.GetAllCommunicationTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var communicationType in communicationTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { communicationType.CommunicationTypeID,
						communicationType.CommunicationTypeName,
					   communicationType.CommunicationGrouping.CommunicationGroupingName,
					  communicationType.Enabled,
					communicationType.CommunicationGroupingID}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CommunicationType
		[HttpGet]
		public JsonResult EditCommunicationType(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.CommunicationType communicationType = AdminRepository.FindCommunicationType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> { communicationType.CommunicationTypeID,
						communicationType.CommunicationTypeName,
					   communicationType.CommunicationGrouping.CommunicationGroupingName,
					  communicationType.Enabled,
					communicationType.CommunicationGroupingID}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				communicationType.EntityID = Authentication.CurrentEntity.EntityID;
				communicationType.CommunicationGroupingID = model.CommunicationGroupId;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCommunicationType(communicationType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + communicationType.CommunicationTypeID;
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
		public string CommunicationTypeNameAvailable(string communicationTypeName, int communicationTypeId) {
			if (AdminRepository.CommunicationTypeNameAvailable(communicationTypeName, communicationTypeId))
				return "Communication Type already exist";
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
			ViewData["SubmenuName"] = "InvestorManagement";
			ViewData["PageName"] = "CommunicationGrouping";
			return View();
		}

		//
		// GET: /Admin/CommunicationGroupingList
		[HttpGet]
		public JsonResult CommunicationGroupingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<Models.Entity.CommunicationGrouping> communicationGroupings = AdminRepository.GetAllCommunicationGroupings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			Models.Entity.CommunicationGrouping communicationGrouping = AdminRepository.FindCommunicationGrouping(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> { communicationGrouping.CommunicationGroupingID, communicationGrouping.CommunicationGroupingName }
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					resultModel.Result = "True||" + communicationGrouping.CommunicationGroupingID;
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
		public string CommunicationGroupingNameAvailable(string communicationGrouping, int communicationGroupingId) {
			if (AdminRepository.CommunicationGroupingNameAvailable(communicationGrouping, communicationGroupingId))
				return "Communication Group already exist";
			else
				return string.Empty;
		}

		#endregion

		#endregion

		#region Custom Field Management

		#region Custom Field

		public ActionResult CustomField() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "CustomFieldManagement";
			ViewData["PageName"] = "CustomField";
			EditCustomFieldModel model = new EditCustomFieldModel();
			model.Modules = SelectListFactory.GetModuleSelectList(AdminRepository.GetAllModules());
			model.DataTypes = SelectListFactory.GetDataTypeSelectList(AdminRepository.GetAllDataTypes());
			model.OptionFields = new List<EditOptionFieldModel>();
			model.OptionFields.Add(new EditOptionFieldModel());
			return View(model);
		}

		//
		// GET: /Admin/CustomFieldList
		[HttpGet]
		public JsonResult CustomFieldList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.CustomField> customFields = AdminRepository.GetAllCustomFields(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var customField in customFields) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
					  customField.CustomFieldID,
					  customField.CustomFieldText,
					  customField.ModuleID,
					  customField.MODULE.ModuleName,
					  customField.DataTypeID,
					  customField.DataType.DataTypeName,
					  customField.OptionalText,
					  customField.Search}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public JsonResult EditCustomField(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.CustomField customField = AdminRepository.FindCustomField(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {
					  customField.CustomFieldID,
					  customField.CustomFieldText,
					  customField.ModuleID,
					  customField.MODULE.ModuleName,
					  customField.DataTypeID,
					  customField.DataType.DataTypeName,
					  customField.OptionalText,
					  customField.Search}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				customField.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCustomField(customField);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + customField.CustomFieldID;
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
		public string CustomFieldTextAvailable(string customFieldText, int customFieldId, int moduleId) {
			if (AdminRepository.CustomFieldTextAvailable(customFieldText, customFieldId, moduleId))
				return "Custom Field already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Data Type

		public ActionResult DataType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "CustomFieldManagement";
			ViewData["PageName"] = "DataType";
			return View();
		}

		//
		// GET: /Admin/DataTypeList
		[HttpGet]
		public JsonResult DataTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.DataType> dataTypes = AdminRepository.GetAllDataTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.DataType dataType = AdminRepository.FindDataType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {dataType.DataTypeID,
					  dataType.DataTypeName}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					resultModel.Result = "True||" + dataType.DataTypeID;
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
		public string DataTypeNameAvailable(string dataTypeName, int dataTypeId) {
			if (AdminRepository.DataTypeNameAvailable(dataTypeName, dataTypeId))
				return "Data Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#endregion

		#region DealManagement

		#region Purchase Type
		//
		// GET: /Admin/PurchaseType
		[HttpGet]
		public ActionResult PurchaseType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "PurchaseType";
			return View();
		}

		//
		// GET: /Admin/PurchaseTypeList
		[HttpGet]
		public JsonResult PurchaseTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.PurchaseType> purchaseTypes = AdminRepository.GetAllPurchaseTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.PurchaseType purchaseType = AdminRepository.FindPurchaseType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {purchaseType.PurchaseTypeID,
					  purchaseType.Name}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				purchaseType.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SavePurchaseType(purchaseType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + purchaseType.PurchaseTypeID;
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
		public string PurchaseTypeNameAvailable(string name, int purchaseTypeId) {
			if (AdminRepository.PurchaseTypeNameAvailable(name, purchaseTypeId))
				return "Purchase Type already exist";
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
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "DealClosingCostType";
			return View();
		}

		//
		// GET: /Admin/DealClosingCostTypeList
		[HttpGet]
		public JsonResult DealClosingCostTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.DealClosingCostType> dealClosingCostTypes = AdminRepository.GetAllDealClosingCostTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.DealClosingCostType dealClosingCostType = AdminRepository.FindDealClosingCostType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {dealClosingCostType.DealClosingCostTypeID,
					  dealClosingCostType.Name}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
				DealClosingCostType dealClosingCostType = AdminRepository.FindDealClosingCostType(model.DealClosingCostTypeId);
				if (dealClosingCostType == null) {
					dealClosingCostType = new DealClosingCostType();
				}
				dealClosingCostType.Name = model.Name;
				dealClosingCostType.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveDealClosingCostType(dealClosingCostType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + dealClosingCostType.DealClosingCostTypeID;
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
		public string DealClosingCostTypeNameAvailable(string name, int dealClosingCostTypeId) {
			if (AdminRepository.DealClosingCostTypeNameAvailable(name, dealClosingCostTypeId))
				return "Deal Closing Cost Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region UnderlyingFundType

		public ActionResult UnderlyingFundType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.UnderlyingFundType underlyingFundType = AdminRepository.FindUnderlyingFundType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {underlyingFundType.UnderlyingFundTypeID,
					  underlyingFundType.Name}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					resultModel.Result = "True||" + underlyingFundType.UnderlyingFundTypeID;
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
			if (AdminRepository.DeleteUnderlyingFundType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string UnderlyingFundTypeNameAvailable(string name, int underlyingFundTypeId) {
			if (AdminRepository.UnderlyingFundTypeNameAvailable(name, underlyingFundTypeId))
				return "Underlying Fund Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ShareClassType

		public ActionResult ShareClassType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.ShareClassType shareClassType = AdminRepository.FindShareClassType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {shareClassType.ShareClassTypeID,
					  shareClassType.ShareClass,
					  shareClassType.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					shareClassType.CreatedBy = Authentication.CurrentUser.UserID;
					shareClassType.CreatedDate = DateTime.Now;
				}
				shareClassType.ShareClass = model.ShareClass;
				shareClassType.Enabled = model.Enabled;
				shareClassType.EntityID = Authentication.CurrentEntity.EntityID;
				shareClassType.LastUpdatedBy = Authentication.CurrentUser.UserID;
				shareClassType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveShareClassType(shareClassType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + shareClassType.ShareClassTypeID;
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
			if (AdminRepository.DeleteShareClassType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ShareClassAvailable(string shareClass, int shareClassTypeId) {
			if (AdminRepository.ShareClassTypeNameAvailable(shareClass, shareClassTypeId))
				return "Share Class already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region CashDistributionType

		public ActionResult CashDistributionType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "CashDistributionType";
			return View();
		}

		//
		// GET: /Admin/CashDistributionTypeList
		[HttpGet]
		public JsonResult CashDistributionTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.CashDistributionType> cashDistributionTypes = AdminRepository.GetAllCashDistributionTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var cashDistributionType in cashDistributionTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {cashDistributionType.CashDistributionTypeID,
					  cashDistributionType.Name,
					  cashDistributionType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditCashDistributionType
		[HttpGet]
		public ActionResult EditCashDistributionType(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.CashDistributionType cashDistributionType = AdminRepository.FindCashDistributionType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {cashDistributionType.CashDistributionTypeID,
					  cashDistributionType.Name,
					  cashDistributionType.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCashDistributionType
		[HttpPost]
		public ActionResult UpdateCashDistributionType(FormCollection collection) {
			EditCashDistributionTypeModel model = new EditCashDistributionTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = CashDistributionTypeAvailable(model.Name, model.CashDistributionTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				CashDistributionType cashDistributionType = AdminRepository.FindCashDistributionType(model.CashDistributionTypeId);
				if (cashDistributionType == null) {
					cashDistributionType = new CashDistributionType();
				}
				cashDistributionType.Name = model.Name;
				cashDistributionType.Enabled = model.Enabled;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCashDistributionType(cashDistributionType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + cashDistributionType.CashDistributionTypeID;
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
		public string DeleteCashDistributionType(int id) {
			if (AdminRepository.DeleteCashDistributionType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CashDistributionTypeAvailable(string cashDistributionType, int cashDistributionTypeId) {
			if (AdminRepository.CashDistributionTypeNameAvailable(cashDistributionType, cashDistributionTypeId))
				return "Cash Distribution Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region FundExpenseType

		public ActionResult FundExpenseType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "FundExpenseType";
			return View();
		}

		//
		// GET: /Admin/FundExpenseTypeList
		[HttpGet]
		public JsonResult FundExpenseTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.FundExpenseType> fundExpenseTypes = AdminRepository.GetAllFundExpenseTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fundExpenseType in fundExpenseTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {fundExpenseType.FundExpenseTypeID,
					  fundExpenseType.Name}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditFundExpenseType
		[HttpGet]
		public ActionResult EditFundExpenseType(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.FundExpenseType fundExpenseType = AdminRepository.FindFundExpenseType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {fundExpenseType.FundExpenseTypeID,
					  fundExpenseType.Name}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateFundExpenseType
		[HttpPost]
		public ActionResult UpdateFundExpenseType(FormCollection collection) {
			EditFundExpenseTypeModel model = new EditFundExpenseTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = FundExpenseTypeAvailable(model.Name, model.FundExpenseTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FundExpenseType fundExpenseType = AdminRepository.FindFundExpenseType(model.FundExpenseTypeId);
				if (fundExpenseType == null) {
					fundExpenseType = new FundExpenseType();
				}
				fundExpenseType.Name = model.Name;
				fundExpenseType.EntityID = Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveFundExpenseType(fundExpenseType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + fundExpenseType.FundExpenseTypeID;
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
		public string DeleteFundExpenseType(int id) {
			if (AdminRepository.DeleteFundExpenseType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FundExpenseTypeAvailable(string fundExpenseType, int fundExpenseTypeId) {
			if (AdminRepository.FundExpenseTypeNameAvailable(fundExpenseType, fundExpenseTypeId))
				return "Fund Expense Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ReportingFrequency

		public ActionResult ReportingFrequency() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.ReportingFrequency reportingFrequency = AdminRepository.FindReportingFrequency(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {reportingFrequency.ReportingFrequencyID,
					  reportingFrequency.ReportingFrequency1,
					  reportingFrequency.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					reportingFrequency.CreatedBy = Authentication.CurrentUser.UserID;
					reportingFrequency.CreatedDate = DateTime.Now;
				}
				reportingFrequency.ReportingFrequency1 = model.ReportingFrequency;
				reportingFrequency.Enabled = model.Enabled;
				reportingFrequency.EntityID = Authentication.CurrentEntity.EntityID;
				reportingFrequency.LastUpdatedBy = Authentication.CurrentUser.UserID;
				reportingFrequency.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveReportingFrequency(reportingFrequency);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + reportingFrequency.ReportingFrequencyID;
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
			if (AdminRepository.DeleteReportingFrequency(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingFrequencyAvailable(string reportingFrequency, int reportingFrequencyId) {
			if (AdminRepository.ReportingFrequencyNameAvailable(reportingFrequency, reportingFrequencyId))
				return "Reporting already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ReportingType

		public ActionResult ReportingType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.ReportingType reportingType = AdminRepository.FindReportingType(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {reportingType.ReportingTypeID,
					  reportingType.Reporting,
					  reportingType.Enabled}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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
					reportingType.CreatedBy = Authentication.CurrentUser.UserID;
					reportingType.CreatedDate = DateTime.Now;
				}
				reportingType.Reporting = model.Reporting;
				reportingType.Enabled = model.Enabled;
				reportingType.EntityID = Authentication.CurrentEntity.EntityID;
				reportingType.LastUpdatedBy = Authentication.CurrentUser.UserID;
				reportingType.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveReportingType(reportingType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + reportingType.ReportingTypeID;
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
			if (AdminRepository.DeleteReportingType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingTypeAvailable(string reporting, int reportingTypeId) {
			if (AdminRepository.ReportingTypeNameAvailable(reporting, reportingTypeId))
				return "Reporting Type already exists.";
			else
				return string.Empty;
		}

		#endregion

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
			List<DeepBlue.Models.Entity.MODULE> modules = AdminRepository.GetAllModules(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
			if (AdminRepository.DeleteModule(id) == false) {
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

		#region Security Type
		//
		// GET: /Admin/SecurityType
		[HttpGet]
		public ActionResult SecurityType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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

		#region Geography

		public ActionResult Geography() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
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
					geography.CreatedBy = Authentication.CurrentUser.UserID;
					geography.CreatedDate = DateTime.Now;
				}
				geography.Geography1 = model.Geography;
				geography.Enabled = model.Enabled;
				geography.EntityID = Authentication.CurrentEntity.EntityID;
				geography.LastUpdatedBy = Authentication.CurrentUser.UserID;
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
			if (AdminRepository.DeleteGeography(id) == false) {
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
		//
		// GET: /Admin/Industry
		[HttpGet]
		public ActionResult Industry() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "Industry";
			return View();
		}

		//
		// GET: /Admin/IndustryList
		[HttpGet]
		public JsonResult IndustryList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.Industry> industries = AdminRepository.GetAllIndustrys(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var industry in industries) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
						industry.IndustryID,
					    industry.Industry1,
					    industry.Enabled
					 }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/Industry
		[HttpGet]
		public ActionResult EditIndustry(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.Industry industry = AdminRepository.FindIndustry(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {
						industry.IndustryID,
					    industry.Industry1,
					    industry.Enabled
				}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateIndustry
		[HttpPost]
		public ActionResult UpdateIndustry(FormCollection collection) {
			EditIndustryModel model = new EditIndustryModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = IndustryNameAvailable(model.Industry, model.IndustryId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Industry industry = AdminRepository.FindIndustry(model.IndustryId);
				if (industry == null) {
					industry = new Industry();
					industry.CreatedBy = Authentication.CurrentUser.UserID;
					industry.CreatedDate = DateTime.Now;
				}
				industry.Industry1 = model.Industry;
				industry.EntityID = Authentication.CurrentEntity.EntityID;
				industry.Enabled = model.Enabled;
				industry.LastUpdatedBy = Authentication.CurrentUser.UserID;
				industry.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveIndustry(industry);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + industry.IndustryID;
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
			if (AdminRepository.DeleteIndustry(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string IndustryNameAvailable(string industryName, int industryId) {
			if (AdminRepository.IndustryNameAvailable(industryName, industryId))
				return "Industry already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Admin/FindIndustrys
		[HttpGet]
		public JsonResult FindIndustrys(string term) {
			return Json(AdminRepository.FindIndustrys(term), JsonRequestBehavior.AllowGet);
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
			List<DeepBlue.Models.Entity.FileType> fileTypes = AdminRepository.GetAllFileTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
				fileType.EntityID = Authentication.CurrentEntity.EntityID;
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
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "EquityType";
			return View();
		}

		//
		// GET: /Admin/EquityTypeList
		[HttpGet]
		public JsonResult EquityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.EquityType> equityTypes = AdminRepository.GetAllEquityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
					equityType.CreatedBy = Authentication.CurrentUser.UserID;
					equityType.CreatedDate = DateTime.Now;
				}
				equityType.Equity = model.Equity;
				equityType.Enabled = model.Enabled;
				equityType.EntityID = Authentication.CurrentEntity.EntityID;
				equityType.LastUpdatedBy = Authentication.CurrentUser.UserID;
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
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "FixedIncomeType";
			return View();
		}

		//
		// GET: /Admin/FixedIncomeTypeList
		[HttpGet]
		public JsonResult FixedIncomeTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.FixedIncomeType> fixedIncomeTypes = AdminRepository.GetAllFixedIncomeTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
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
				model.FixedIncomeType1 = fixedIncomeType.FixedIncomeType1;
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
			string ErrorMessage = FixedIncomeTypeNameAvailable(model.FixedIncomeType1, model.FixedIncomeTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("FixedIncome", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FixedIncomeType fixedIncomeType = AdminRepository.FindFixedIncomeType(model.FixedIncomeTypeId);
				if (fixedIncomeType == null) {
					fixedIncomeType = new FixedIncomeType();
					fixedIncomeType.CreatedBy = Authentication.CurrentUser.UserID;
					fixedIncomeType.CreatedDate = DateTime.Now;
				}
				fixedIncomeType.FixedIncomeType1 = model.FixedIncomeType1;
				fixedIncomeType.Enabled = model.Enabled;
				fixedIncomeType.EntityID = Authentication.CurrentEntity.EntityID;
				fixedIncomeType.LastUpdatedBy = Authentication.CurrentUser.UserID;
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

		#region ActivityType

		public ActionResult ActivityType() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "CustomFieldManagement";
			ViewData["PageName"] = "ActivityType";
			return View();
		}

		//
		// GET: /Admin/ActivityTypeList
		[HttpGet]
		public JsonResult ActivityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.ActivityType> activityTypes = AdminRepository.GetAllActivityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var activityType in activityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {activityType.ActivityTypeID,
					  activityType.Name,
					  activityType.Enabled}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditActivityType
		[HttpGet]
		public ActionResult EditActivityType(int id) {
			EditActivityTypeModel model = new EditActivityTypeModel();
			ActivityType activityType = AdminRepository.FindActivityType(id);
			if (activityType != null) {
				model.ActivityTypeId = activityType.ActivityTypeID;
				model.Name = activityType.Name;
				model.Enabled = activityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateActivityType
		[HttpPost]
		public ActionResult UpdateActivityType(FormCollection collection) {
			EditActivityTypeModel model = new EditActivityTypeModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = ActivityTypeAvailable(model.Name, model.ActivityTypeId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				ActivityType activityType = AdminRepository.FindActivityType(model.ActivityTypeId);
				if (activityType == null) {
					activityType = new ActivityType();
				}
				activityType.Name = model.Name;
				activityType.Enabled = model.Enabled;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveActivityType(activityType);
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
		public string DeleteActivityType(int id) {
			if (AdminRepository.DeleteActivityType(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ActivityTypeAvailable(string ActivityType, int ActivityTypeId) {
			if (AdminRepository.ActivityTypeNameAvailable(ActivityType, ActivityTypeId))
				return "Name already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region Country
		//
		// GET: /Admin/FindCountrys
		[HttpGet]
		public JsonResult FindCountrys(string term) {
			return Json(AdminRepository.FindCountrys(term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region State
		//
		// GET: /Admin/FindStates
		[HttpGet]
		public JsonResult FindStates(string term) {
			return Json(AdminRepository.FindStates(term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Currency
		//
		// GET: /Admin/Currency
		[HttpGet]
		public ActionResult Currency() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "DealManagement";
			ViewData["PageName"] = "Currency";
			return View();
		}

		//
		// GET: /Admin/CurrencyList
		[HttpGet]
		public JsonResult CurrencyList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.Currency> currencies = AdminRepository.GetAllCurrencies(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var currency in currencies) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
						currency.CurrencyID,
					    currency.Currency1,
					    currency.Enabled
					 }
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/Currency
		[HttpGet]
		public ActionResult EditCurrency(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.Currency currency = AdminRepository.FindCurrency(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {
						currency.CurrencyID,
					    currency.Currency1,
					    currency.Enabled
				}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCurrency
		[HttpPost]
		public ActionResult UpdateCurrency(FormCollection collection) {
			EditCurrencyModel model = new EditCurrencyModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = CurrencyNameAvailable(model.Currency, model.CurrencyId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				Currency currency = AdminRepository.FindCurrency(model.CurrencyId);
				if (currency == null) {
					currency = new Currency();
					currency.CreatedBy = Authentication.CurrentUser.UserID;
					currency.CreatedDate = DateTime.Now;
				}
				currency.Currency1 = model.Currency;
				currency.EntityID = Authentication.CurrentEntity.EntityID;
				currency.Enabled = model.Enabled;
				currency.LastUpdatedBy = Authentication.CurrentUser.UserID;
				currency.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCurrency(currency);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + currency.CurrencyID;
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
		public string DeleteCurrency(int id) {
			if (AdminRepository.DeleteCurrency(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CurrencyNameAvailable(string currencyName, int currencyId) {
			if (AdminRepository.CurrencyNameAvailable(currencyName, currencyId))
				return "Name already exist";
			else
				return string.Empty;
		}

		#endregion

		#region ExportExcel
		public ActionResult ExportExcel(string tableName) {
			return new ExportExcel { TableName = tableName, GridData = AdminRepository.FindTable(tableName) };
		}
		#endregion

		#region Fund Closing
		//
		// GET: /Admin/FundClosing
		[HttpGet]
		public ActionResult FundClosing() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "InvestorManagement";
			ViewData["PageName"] = "FundClosing";
			return View();
		}

		//
		// GET: /Admin/FundClosingList
		[HttpGet]
		public JsonResult FundClosingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<DeepBlue.Models.Entity.FundClosing> fundClosings = AdminRepository.GetAllFundClosings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fundClosing in fundClosings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {fundClosing.FundClosingID,
					  fundClosing.Name,
					 fundClosing.Fund.FundName,
					 fundClosing.FundClosingDate,
					 fundClosing.IsFirstClosing,
					 fundClosing.FundID
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditFundClosing
		[HttpGet]
		public ActionResult EditFundClosing(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			DeepBlue.Models.Entity.FundClosing fundClosing = AdminRepository.FindFundClosing(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {fundClosing.FundClosingID,
					  fundClosing.Name,
					 fundClosing.Fund.FundName,
					 fundClosing.FundClosingDate,
					 fundClosing.IsFirstClosing,
					 fundClosing.FundID
					}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateFundClosing
		[HttpPost]
		public ActionResult UpdateFundClosing(FormCollection collection) {
			EditFundClosingModel model = new EditFundClosingModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = FundClosingNameAvailable(model.Name, model.FundClosingId, model.FundId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				FundClosing fundClosing = AdminRepository.FindFundClosing(model.FundClosingId);
				if (fundClosing == null) {
					fundClosing = new FundClosing();
				}
				fundClosing.Name = model.Name;
				fundClosing.FundClosingDate = model.FundClosingDate;
				fundClosing.FundID = model.FundId;
				fundClosing.IsFirstClosing = model.IsFirstClosing;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveFundClosing(fundClosing);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + fundClosing.FundClosingID;
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
		public string FundClosingNameAvailable(string name, int fundClosingId, int fundId) {
			if (AdminRepository.FundClosingNameAvailable(name, fundClosingId, fundId))
				return "Name already exist";
			else
				return string.Empty;
		}
		#endregion

		#region DealContacts
		//
		// GET: /Fund/FindDealContacts
		[HttpGet]
		public JsonResult FindDealContacts(string term) {
			return Json(AdminRepository.FindDealContacts(term), JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region User

		[AdminAuthorize()]
		public ActionResult User() {
			ViewData["MenuName"] = "Admin";
			ViewData["SubmenuName"] = "UserManagement";
			ViewData["PageName"] = "User";
			return View();
		}

		//
		// GET: /Admin/UserList
		[HttpGet]
		public JsonResult UserList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<USER> Users = AdminRepository.GetAllUsers(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var user in Users) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
					  user.UserID,
					  user.FirstName,
					  user.LastName,
					  user.Login,
					  user.Email,
					  user.Enabled,
					  user.MiddleName,
					  user.IsAdmin,
					  user.PhoneNumber
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditUser
		[HttpGet]
		public ActionResult EditUser(int id) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			USER user = AdminRepository.FindUser(id);
			flexgridData.total = totalRows;
			flexgridData.page = 0;
			flexgridData.rows.Add(new FlexigridRow {
				cell = new List<object> {
					  user.UserID,
					  user.FirstName,
					  user.LastName,
					  user.Login,
					  user.Email,
					  user.Enabled,
					  user.MiddleName,
					  user.IsAdmin,
					  user.PhoneNumber
				}
			});
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateUser
		[HttpPost]
		public ActionResult UpdateUser(FormCollection collection) {
			EditUserModel model = new EditUserModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = UserNameAvailable(model.Login, model.UserId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Login", ErrorMessage);
			}
			ErrorMessage = EmailAvailable(model.Email, model.UserId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Email", ErrorMessage);
			}
			if (model.ChangePassword) {
				if (string.IsNullOrEmpty(model.Password))
					ModelState.AddModelError("Password", "Password is required");
			}
			if (ModelState.IsValid) {
				USER user = AdminRepository.FindUser(model.UserId);
				if (user == null) {
					user = new USER();
					user.CreatedDate = DateTime.Now;
				}

				user.EntityID = Authentication.CurrentEntity.EntityID;
				user.LastUpdatedDate = DateTime.Now;

				user.FirstName = model.FirstName;
				user.LastName = model.LastName;
				user.MiddleName = model.MiddleName;
				user.PhoneNumber = model.PhoneNumber;
				if (model.ChangePassword) {
					user.PasswordSalt = SecurityExtensions.CreateSalt();
					user.PasswordHash = model.Password.CreateHash(user.PasswordSalt);
				}
				user.Login = model.Login;
				user.Email = model.Email;
				user.Enabled = model.Enabled;
				user.IsAdmin = model.IsAdmin;

				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveUser(user);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				}
				else {
					resultModel.Result = "True||" + user.UserID;
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
		public string DeleteUser(int id) {
			if (AdminRepository.DeleteUser(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string UserNameAvailable(string userName, int UserId) {
			if (AdminRepository.UserNameAvailable(userName, UserId))
				return "Username already exists.";
			else
				return string.Empty;
		}


		[HttpGet]
		public string EmailAvailable(string email, int UserId) {
			if (AdminRepository.EmailAvailable(email, UserId))
				return "Email already exists.";
			else
				return string.Empty;
		}


		#endregion

		#region "Select List"
		[HttpGet]
		public JsonResult SelectList(string actionName) {
			List<SelectListItem> items = null;
			switch (actionName) {
				case "FundType":
					items = SelectListFactory.GetUnderlyingFundTypeSelectList(AdminRepository.GetAllUnderlyingFundTypes());
					break;
				case "ReportingFrequency":
					items = SelectListFactory.GetReportingFrequencySelectList(AdminRepository.GetAllReportingFrequencies());
					break;
				case "ReportingType":
					items = SelectListFactory.GetReportingTypeSelectList(AdminRepository.GetAllReportingTypes());
					break;
				case "Industry":
					items = SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
					break;
				case "Geography":
					items = SelectListFactory.GetGeographySelectList(AdminRepository.GetAllGeographies());
					break;
				case "InvestorType":
					items = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
					break;
				case "ActivityType":
					items = SelectListFactory.GetActivityTypeSelectList(AdminRepository.GetAllActivityTypes());
					break;
				case "Currency":
					items = SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
					break;
				case "ShareClassType":
					items = SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
					break;
				case "EquityType":
					items = SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
					break;
			}
			return Json(items, JsonRequestBehavior.AllowGet);
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
