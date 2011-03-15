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
			if (AdminRepository.DeleteInvestorType(id) == false) {
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
		public ActionResult EntityType() {
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
			if (AdminRepository.DeleteInvestorEntityType(id) == false) {
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

		#region FundClosing
		//
		// GET: /Admin/FundClosing
		[HttpGet]
		public ActionResult FundClosing() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/FundClosingList
		[HttpGet]
		public ActionResult FundClosingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<FundClosing> fundClosings = AdminRepository.GetAllFundClosings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(fundClosings);
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
		public string DeleteFundClosing(int id) {
			if (AdminRepository.DeleteFundClosing(id) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FundClosingNameAvailable(string Name, int FundClosingID) {
			if (AdminRepository.FundClosingNameAvailable(Name, FundClosingID))
				return "Name is already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Custom Field

		public ActionResult CustomField() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/CustomFieldList
		[HttpGet]
		public ActionResult CustomFieldList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<CustomField> customFields = AdminRepository.GetAllCustomFields(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(customFields);
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
			} else {
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
		public string DeleteCustomField(int id) {
			if (AdminRepository.DeleteCustomField(id) == false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CustomFieldTextAvailable(string CustomFieldText, int CustomFieldId) {
			if (AdminRepository.CustomFieldTextAvailable(CustomFieldText, CustomFieldId))
				return "Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Data Type

		public ActionResult DataType() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/DataTypeList
		[HttpGet]
		public ActionResult DataTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<DataType> dataTypes = AdminRepository.GetAllDataTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(dataTypes);
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
		public string DeleteDataType(int id) {
			if (AdminRepository.DeleteDataType(id) == false) {
				return "Cann't Delete! Child record found!";
			} else {
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

		public ActionResult Module() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
