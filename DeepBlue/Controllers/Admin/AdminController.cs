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
				} else {
					resultModel.Result = "True";
				}
			} else {
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
			ViewData["SubmenuName"] = "AdminInvestor";
			ViewData["PageName"] = "EntityType";
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
				} else {
					resultModel.Result = "True";
				}
			} else {
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
			ViewData["SubmenuName"] = "AdminFund";
			ViewData["PageName"] = "FundClosing";
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
				} else {
					resultModel.Result = "True";
				}
			} else {
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
			} else {
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
				} else {
					resultModel.Result = "True";
				}
			} else {
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
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string CustomFieldTextAvailable(string CustomFieldText, int CustomFieldId, int ModuleId) {
			if (AdminRepository.CustomFieldTextAvailable(CustomFieldText, CustomFieldId,ModuleId))
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
				} else {
					resultModel.Result = "True";
				}
			} else {
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

		public ActionResult MODULE() {
			ViewData["MenuName"] = "Admin";
			return View();
		}

		//
		// GET: /Admin/ModuleList
		[HttpGet]
		public ActionResult ModuleList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<MODULE> module = AdminRepository.GetAllModules(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(module);
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
		public ActionResult CommunicationTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<DeepBlue.Models.Entity.CommunicationType> communicationTypes = AdminRepository.GetAllCommunicationTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(communicationTypes);
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
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveCommunicationType(communicationType);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
					}
				} else {
					resultModel.Result = "True";
				}
			} else {
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
			} else {
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
		public ActionResult CommunicationGroupingList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<CommunicationGrouping> communicationGroupings = AdminRepository.GetAllCommunicationGroupings(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(communicationGroupings);
		}

		//
		// GET: /Admin/CommunicationGrouping
		[HttpGet]
		public ActionResult EditCommunicationGrouping(int id) {
			EditCommunicationGroupingModel model = new EditCommunicationGroupingModel();
			CommunicationGrouping communicationGrouping = AdminRepository.FindCommunicationGrouping(id);
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
				CommunicationGrouping communicationGrouping = AdminRepository.FindCommunicationGrouping(model.CommunicationGroupingId);
				if (communicationGrouping == null) {
					communicationGrouping = new CommunicationGrouping();
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
		public ActionResult PurchaseTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<PurchaseType> purchaseTypes = AdminRepository.GetAllPurchaseTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(purchaseTypes);
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
		public ActionResult DealClosingCostTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<DealClosingCostType> DealClosingCostTypes = AdminRepository.GetAllDealClosingCostTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(DealClosingCostTypes);
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
		public ActionResult SecurityTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<SecurityType> securityTypes = AdminRepository.GetAllSecurityTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(securityTypes);
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
		public ActionResult UnderlyingFundTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<UnderlyingFundType> module = AdminRepository.GetAllUnderlyingFundTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(module);
		}

		//
		// GET: /Admin/EditUnderlyingFundType
		[HttpGet]
		public ActionResult EditUnderlyingFundType(int id) {
			EditUnderlyingFundTypeModel model = new EditUnderlyingFundTypeModel();
			UnderlyingFundType underlyinfundtype = AdminRepository.FindUnderlyingFundType(id);
			if (underlyinfundtype != null) {
				model.UnderlyingFundTypeID = underlyinfundtype.UnderlyingFundTypeID;
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
			string ErrorMessage = UnderlyingFundTypeFieldTextAvailable(model.Name, model.UnderlyingFundTypeID);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				UnderlyingFundType underlyingfundtype = AdminRepository.FindUnderlyingFundType(model.UnderlyingFundTypeID);
				if (underlyingfundtype == null) {
					underlyingfundtype = new UnderlyingFundType();
				}
				underlyingfundtype.Name = model.Name;
				IEnumerable<ErrorInfo> errorInfo = AdminRepository.SaveUnderlyingFundType(underlyingfundtype);
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
		public string UnderlyingFundTypeFieldTextAvailable(string UnderlyingFundTypeFieldText, int UnderlyingFundtypeFieldId) {
			if (AdminRepository.UnderlyingFundTypeTextAvailable(UnderlyingFundTypeFieldText, UnderlyingFundtypeFieldId))
				return "Custom Field Text already exists.";
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
		public ActionResult ShareClassTypeList(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<ShareClassType> module = AdminRepository.GetAllShareClassTypes(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(module);
		}

		//
		// GET: /Admin/EditShareClassType
		[HttpGet]
		public ActionResult EditShareClassType(int id) {
			EditShareClassTypeModel model = new EditShareClassTypeModel();
			ShareClassType shareclasstype = AdminRepository.FindShareClassType(id);
			if (shareclasstype != null) {
				model.ShareClassTypeID = shareclasstype.ShareClassTypeID;
				model.ShareClass = shareclasstype.ShareClass;
				model.Enabled = shareclasstype.Enabled ;
				model.CreatedBy = shareclasstype.CreatedBy;
				model.CreatedDate = shareclasstype.CreatedDate;  
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
			string ErrorMessage = ShareClassTypeFieldTextAvailable(model.ShareClass, model.ShareClassTypeID);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("Name", ErrorMessage);
			}
			if (ModelState.IsValid) {
				ShareClassType shareclasstype = AdminRepository.FindShareClassType(model.ShareClassTypeID);
				if (shareclasstype == null) {
					shareclasstype = new ShareClassType();
				}
				shareclasstype.ShareClass = model.ShareClass;
				shareclasstype.Enabled = model.Enabled;
				shareclasstype.EntityID = (int)ConfigUtil.CurrentEntityID;
				shareclasstype.CreatedBy = AppSettings.CreatedByUserId;
				shareclasstype.CreatedDate = DateTime.Now; 
				shareclasstype.LastUpdatedBy = AppSettings.CreatedByUserId;
				shareclasstype.LastUpdatedDate = DateTime.Now;
				IEnumerable < ErrorInfo > errorInfo = AdminRepository.SaveShareClassType(shareclasstype);
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
		public string ShareClassTypeFieldTextAvailable(string ShareClassTypeFieldText, int ShareClassTypeFieldId) {
			if (AdminRepository.ShareClassTypeTextAvailable(ShareClassTypeFieldText, ShareClassTypeFieldId))
				return "Custom Field Text already exists.";
			else
				return string.Empty;
		}


		#endregion


		public ActionResult Result() {
			return View();
		}
	}
}
