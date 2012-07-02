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
using DeepBlue.Models.Deal;
using DeepBlue.Controllers.Deal;
using System.IO;
using Winnovative.WnvHtmlConvert;

namespace DeepBlue.Controllers.Admin {

	public class AdminController:BaseController {

		public IAdminRepository AdminRepository { get; set; }

		public ITransactionRepository TransactionRepository { get; set; }


		public AdminController()
			: this(new AdminRepository(),new TransactionRepository()) {
		}

		public AdminController(IAdminRepository adminRepository,ITransactionRepository transactionRepository) {
			AdminRepository=adminRepository;
			TransactionRepository=transactionRepository;
		}

		#region InvestorMangement

		#region InvestorType

		//
		// GET: /Admin/InvestorType
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult InvestorType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="InvestorManagement";
			ViewData["PageName"]="InvestorType";
			return View();
		}

		//
		// GET: /Admin/InvestorTypeList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult InvestorTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<InvestorType> investorTypes=AdminRepository.GetAllInvestorTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				investorTypes=investorTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var invertorType in investorTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { invertorType.InvestorTypeID,
						invertorType.InvestorTypeName,
					   invertorType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/InvestorType
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditInvestorType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			InvestorType investorType=AdminRepository.FindInvestorType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> { investorType.InvestorTypeID,
						investorType.InvestorTypeName,
					   investorType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateInvestorType
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateInvestorType(FormCollection collection) {
			EditInvestorTypeModel model=new EditInvestorTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=InvestorTypeNameAvailable(model.InvestorTypeName,model.InvestorTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("InvestorTypeName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				InvestorType investorType=AdminRepository.FindInvestorType(model.InvestorTypeId);
				if(investorType==null) {
					investorType=new InvestorType();
				}
				investorType.InvestorTypeName=model.InvestorTypeName;
				investorType.Enabled=model.Enabled;
				investorType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveInvestorType(investorType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+investorType.InvestorTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteInvestorType(int id) {
			if(AdminRepository.DeleteInvestorType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string InvestorTypeNameAvailable(string investorType,int investorTypeId) {
			if(AdminRepository.InvestorTypeNameAvailable(investorType,investorTypeId))
				return "Investor Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Investor EntityType

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public ActionResult EntityType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="InvestorManagement";
			ViewData["PageName"]="InvestorEntityType";
			return View();
		}

		//
		// GET: /Admin/EntityTypeList
		[HttpGet]
		public JsonResult EntityTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.InvestorEntityType> investorEntityTypes=AdminRepository.GetAllInvestorEntityTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				investorEntityTypes=investorEntityTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var investorEntityType in investorEntityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {investorEntityType.InvestorEntityTypeID,
					  investorEntityType.InvestorEntityTypeName,
					  investorEntityType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		public JsonResult EditInvestorEntityType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			InvestorEntityType investorEntityType=AdminRepository.FindInvestorEntityType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {investorEntityType.InvestorEntityTypeID,
					  investorEntityType.InvestorEntityTypeName,
					  investorEntityType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateInvestorEntityType
		[HttpPost]
		public ActionResult UpdateInvestorEntityType(FormCollection collection) {
			EditInvestorEntityTypeModel model=new EditInvestorEntityTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=InvestorEntityTypeNameAvailable(model.InvestorEntityTypeName,model.InvestorEntityTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("InvestorEntityTypeName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				InvestorEntityType investorEntityType=AdminRepository.FindInvestorEntityType(model.InvestorEntityTypeId);
				if(investorEntityType==null) {
					investorEntityType=new InvestorEntityType();
				}
				investorEntityType.InvestorEntityTypeName=model.InvestorEntityTypeName;
				investorEntityType.Enabled=model.Enabled;
				investorEntityType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveInvestorEntityType(investorEntityType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+investorEntityType.InvestorEntityTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteInvestorEntityType(int id) {
			if(AdminRepository.DeleteInvestorEntityType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string InvestorEntityTypeNameAvailable(string investorEntityType,int investorEntityTypeId) {
			if(AdminRepository.InvestorEntityTypeNameAvailable(investorEntityType,investorEntityTypeId))
				return "Investor Entity Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Communication Type
		//
		// GET: /Admin/CommunicationType
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult CommunicationType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="InvestorManagement";
			ViewData["PageName"]="CommunicationType";
			EditCommunicationTypeModel model=new EditCommunicationTypeModel();
			model.CommunicationGroupings=SelectListFactory.GetCommunicationGroupingSelectList(AdminRepository.GetAllCommunicationGroupings());
			return View(model);
		}

		//
		// GET: /Admin/CommunicationTypeList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult CommunicationTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.CommunicationType> communicationTypes=AdminRepository.GetAllCommunicationTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var communicationType in communicationTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { communicationType.CommunicationTypeID,
						communicationType.CommunicationTypeName,
					   communicationType.CommunicationGrouping.CommunicationGroupingName,
					  communicationType.Enabled,
					communicationType.CommunicationGroupingID}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CommunicationType
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult EditCommunicationType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.CommunicationType communicationType=AdminRepository.FindCommunicationType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> { communicationType.CommunicationTypeID,
						communicationType.CommunicationTypeName,
					   communicationType.CommunicationGrouping.CommunicationGroupingName,
					  communicationType.Enabled,
					communicationType.CommunicationGroupingID}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCommunicationType
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateCommunicationType(FormCollection collection) {
			EditCommunicationTypeModel model=new EditCommunicationTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=CommunicationTypeNameAvailable(model.CommunicationTypeName,model.CommunicationTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("CommunicationTypeName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				DeepBlue.Models.Entity.CommunicationType communicationType=AdminRepository.FindCommunicationType(model.CommunicationTypeId);
				if(communicationType==null) {
					communicationType=new DeepBlue.Models.Entity.CommunicationType();
				}
				communicationType.CommunicationTypeName=model.CommunicationTypeName;
				communicationType.Enabled=model.Enabled;
				communicationType.EntityID=Authentication.CurrentEntity.EntityID;
				communicationType.CommunicationGroupingID=model.CommunicationGroupId;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveCommunicationType(communicationType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+communicationType.CommunicationTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteCommunicationType(int id) {
			if(AdminRepository.DeleteCommunicationType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string CommunicationTypeNameAvailable(string communicationTypeName,int communicationTypeId) {
			if(AdminRepository.CommunicationTypeNameAvailable(communicationTypeName,communicationTypeId))
				return "Communication Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Communication Grouping
		//
		// GET: /Admin/CommunicationGrouping
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult CommunicationGrouping() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="InvestorManagement";
			ViewData["PageName"]="CommunicationGrouping";
			return View();
		}

		//
		// GET: /Admin/CommunicationGroupingList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult CommunicationGroupingList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<Models.Entity.CommunicationGrouping> communicationGroupings=AdminRepository.GetAllCommunicationGroupings(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var communicationGroup in communicationGroupings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { communicationGroup.CommunicationGroupingID,communicationGroup.CommunicationGroupingName }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CommunicationGrouping
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditCommunicationGrouping(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			Models.Entity.CommunicationGrouping communicationGrouping=AdminRepository.FindCommunicationGrouping(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> { communicationGrouping.CommunicationGroupingID,communicationGrouping.CommunicationGroupingName }
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCommunicationGrouping
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateCommunicationGrouping(FormCollection collection) {
			EditCommunicationGroupingModel model=new EditCommunicationGroupingModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=CommunicationGroupingNameAvailable(model.CommunicationGroupingName,model.CommunicationGroupingId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("CommunicationGroupingName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Models.Entity.CommunicationGrouping communicationGrouping=AdminRepository.FindCommunicationGrouping(model.CommunicationGroupingId);
				if(communicationGrouping==null) {
					communicationGrouping=new Models.Entity.CommunicationGrouping();
				}
				communicationGrouping.CommunicationGroupingName=model.CommunicationGroupingName;
				communicationGrouping.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveCommunicationGrouping(communicationGrouping);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+communicationGrouping.CommunicationGroupingID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteCommunicationGrouping(int id) {
			if(AdminRepository.DeleteCommunicationGrouping(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string CommunicationGroupingNameAvailable(string communicationGrouping,int communicationGroupingId) {
			if(AdminRepository.CommunicationGroupingNameAvailable(communicationGrouping,communicationGroupingId))
				return "Communication Group already exist";
			else
				return string.Empty;
		}

		#endregion

		#endregion

		#region Custom Field Management

		#region Custom Field

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult CustomField() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="CustomFieldManagement";
			ViewData["PageName"]="CustomField";
			EditCustomFieldModel model=new EditCustomFieldModel();
			model.Modules=SelectListFactory.GetModuleSelectList(AdminRepository.GetAllModules());
			model.DataTypes=SelectListFactory.GetDataTypeSelectList(AdminRepository.GetAllDataTypes());
			model.OptionFields=new List<EditOptionFieldModel>();
			model.OptionFields.Add(new EditOptionFieldModel());
			return View(model);
		}

		//
		// GET: /Admin/CustomFieldList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult CustomFieldList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.CustomField> customFields=AdminRepository.GetAllCustomFields(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var customField in customFields) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
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
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult EditCustomField(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.CustomField customField=AdminRepository.FindCustomField(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
					  customField.CustomFieldID,
					  customField.CustomFieldText,
					  customField.ModuleID,
					  customField.MODULE.ModuleName,
					  customField.DataTypeID,
					  customField.DataType.DataTypeName,
					  customField.OptionalText,
					  customField.Search}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCustomField
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateCustomField(FormCollection collection) {
			EditCustomFieldModel model=new EditCustomFieldModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=CustomFieldTextAvailable(model.CustomFieldText,model.CustomFieldId,model.ModuleId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("CustomFieldText",ErrorMessage);
			}
			if(ModelState.IsValid) {
				CustomField customField=AdminRepository.FindCustomField(model.CustomFieldId);
				if(customField==null) {
					customField=new CustomField();
				}
				customField.CustomFieldText=model.CustomFieldText;
				customField.DataTypeID=model.DataTypeId;
				customField.ModuleID=model.ModuleId;
				customField.OptionalText=model.OptionalText;
				customField.Search=model.Search;
				customField.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveCustomField(customField);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+customField.CustomFieldID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteCustomField(int id) {
			if(AdminRepository.DeleteCustomField(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string CustomFieldTextAvailable(string customFieldText,int customFieldId,int moduleId) {
			if(AdminRepository.CustomFieldTextAvailable(customFieldText,customFieldId,moduleId))
				return "Custom Field already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Data Type

		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult DataType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="CustomFieldManagement";
			ViewData["PageName"]="DataType";
			return View();
		}

		//
		// GET: /Admin/DataTypeList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult DataTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.DataType> dataTypes=AdminRepository.GetAllDataTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var dataType in dataTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {dataType.DataTypeID,
					  dataType.DataTypeName}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/DataType
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditDataType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.DataType dataType=AdminRepository.FindDataType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {dataType.DataTypeID,
					  dataType.DataTypeName}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateDataType
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateDataType(FormCollection collection) {
			EditDataTypeModel model=new EditDataTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=DataTypeNameAvailable(model.DataTypeName,model.DataTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("DataTypeName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				DataType dataType=AdminRepository.FindDataType(model.DataTypeId);
				if(dataType==null) {
					dataType=new DataType();
				}
				dataType.DataTypeName=model.DataTypeName;
				dataType.DataTypeID=model.DataTypeId;
				dataType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveDataType(dataType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+dataType.DataTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteDataType(int id) {
			if(AdminRepository.DeleteDataType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DataTypeNameAvailable(string dataTypeName,int dataTypeId) {
			if(AdminRepository.DataTypeNameAvailable(dataTypeName,dataTypeId))
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
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="PurchaseType";
			return View();
		}

		//
		// GET: /Admin/PurchaseTypeList
		[HttpGet]
		public JsonResult PurchaseTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.PurchaseType> purchaseTypes=AdminRepository.GetAllPurchaseTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				purchaseTypes=purchaseTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var purchaseType in purchaseTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {purchaseType.PurchaseTypeID,
					  purchaseType.Name}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/PurchaseType
		[HttpGet]
		public ActionResult EditPurchaseType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.PurchaseType purchaseType=AdminRepository.FindPurchaseType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {purchaseType.PurchaseTypeID,
					  purchaseType.Name}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdatePurchaseType
		[HttpPost]
		public ActionResult UpdatePurchaseType(FormCollection collection) {
			EditPurchaseTypeModel model=new EditPurchaseTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=PurchaseTypeNameAvailable(model.Name,model.PurchaseTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				PurchaseType purchaseType=AdminRepository.FindPurchaseType(model.PurchaseTypeId);
				if(purchaseType==null) {
					purchaseType=new PurchaseType();
				}
				purchaseType.Name=model.Name;
				purchaseType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SavePurchaseType(purchaseType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+purchaseType.PurchaseTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeletePurchaseType(int id) {
			if(AdminRepository.DeletePurchaseType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string PurchaseTypeNameAvailable(string name,int purchaseTypeId) {
			if(AdminRepository.PurchaseTypeNameAvailable(name,purchaseTypeId))
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
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="DealClosingCostType";
			return View();
		}

		//
		// GET: /Admin/DealClosingCostTypeList
		[HttpGet]
		public JsonResult DealClosingCostTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.DealClosingCostType> dealClosingCostTypes=AdminRepository.GetAllDealClosingCostTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				dealClosingCostTypes=dealClosingCostTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var dealClosingCostType in dealClosingCostTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {dealClosingCostType.DealClosingCostTypeID,
					  dealClosingCostType.Name}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/DealClosingCostType
		[HttpGet]
		public ActionResult EditDealClosingCostType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.DealClosingCostType dealClosingCostType=AdminRepository.FindDealClosingCostType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {dealClosingCostType.DealClosingCostTypeID,
					  dealClosingCostType.Name}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateDealClosingCostType
		[HttpPost]
		public ActionResult UpdateDealClosingCostType(FormCollection collection) {
			EditDealClosingCostTypeModel model=new EditDealClosingCostTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=DealClosingCostTypeNameAvailable(model.Name,model.DealClosingCostTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				DealClosingCostType dealClosingCostType=AdminRepository.FindDealClosingCostType(model.DealClosingCostTypeId);
				if(dealClosingCostType==null) {
					dealClosingCostType=new DealClosingCostType();
				}
				dealClosingCostType.Name=model.Name;
				dealClosingCostType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveDealClosingCostType(dealClosingCostType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+dealClosingCostType.DealClosingCostTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteDealClosingCostType(int id) {
			if(AdminRepository.DeleteDealClosingCostType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string DealClosingCostTypeNameAvailable(string name,int dealClosingCostTypeId) {
			if(AdminRepository.DealClosingCostTypeNameAvailable(name,dealClosingCostTypeId))
				return "Deal Closing Cost Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region UnderlyingFundType

		public ActionResult UnderlyingFundType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="UnderlyingFundType";
			return View();
		}

		//
		// GET: /Admin/UnderlyingList
		[HttpGet]
		public JsonResult UnderlyingFundTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.UnderlyingFundType> underlyingFundTypes=AdminRepository.GetAllUnderlyingFundTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				underlyingFundTypes=underlyingFundTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var underlyingFundType in underlyingFundTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {underlyingFundType.UnderlyingFundTypeID,
					  underlyingFundType.Name}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditUnderlyingFundType
		[HttpGet]
		public ActionResult EditUnderlyingFundType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.UnderlyingFundType underlyingFundType=AdminRepository.FindUnderlyingFundType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {underlyingFundType.UnderlyingFundTypeID,
					  underlyingFundType.Name}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateUnderlyingFundType
		[HttpPost]
		public ActionResult UpdateUnderlyingFundType(FormCollection collection) {
			EditUnderlyingFundTypeModel model=new EditUnderlyingFundTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=UnderlyingFundTypeNameAvailable(model.Name,model.UnderlyingFundTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				UnderlyingFundType underlyingFundType=AdminRepository.FindUnderlyingFundType(model.UnderlyingFundTypeId);
				if(underlyingFundType==null) {
					underlyingFundType=new UnderlyingFundType();
				}
				underlyingFundType.Name=model.Name;
				underlyingFundType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveUnderlyingFundType(underlyingFundType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+underlyingFundType.UnderlyingFundTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteUnderlyingFundType(int id) {
			if(AdminRepository.DeleteUnderlyingFundType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string UnderlyingFundTypeNameAvailable(string name,int underlyingFundTypeId) {
			if(AdminRepository.UnderlyingFundTypeNameAvailable(name,underlyingFundTypeId))
				return "Underlying Fund Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ShareClassType

		public ActionResult ShareClassType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="ShareClassType";
			return View();
		}

		//
		// GET: /Admin/ShareClassTypeList
		[HttpGet]
		public JsonResult ShareClassTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.ShareClassType> shareClassTypes=AdminRepository.GetAllShareClassTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				shareClassTypes=shareClassTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var shareClassType in shareClassTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {shareClassType.ShareClassTypeID,
					  shareClassType.ShareClass,
					  shareClassType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditShareClassType
		[HttpGet]
		public ActionResult EditShareClassType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.ShareClassType shareClassType=AdminRepository.FindShareClassType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {shareClassType.ShareClassTypeID,
					  shareClassType.ShareClass,
					  shareClassType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateShareClassType
		[HttpPost]
		public ActionResult UpdateShareClassType(FormCollection collection) {
			EditShareClassTypeModel model=new EditShareClassTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=ShareClassAvailable(model.ShareClass,model.ShareClassTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				ShareClassType shareClassType=AdminRepository.FindShareClassType(model.ShareClassTypeId);
				if(shareClassType==null) {
					shareClassType=new ShareClassType();
					shareClassType.CreatedBy=Authentication.CurrentUser.UserID;
					shareClassType.CreatedDate=DateTime.Now;
				}
				shareClassType.ShareClass=model.ShareClass;
				shareClassType.Enabled=model.Enabled;
				shareClassType.EntityID=Authentication.CurrentEntity.EntityID;
				shareClassType.LastUpdatedBy=Authentication.CurrentUser.UserID;
				shareClassType.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveShareClassType(shareClassType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+shareClassType.ShareClassTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteShareClassType(int id) {
			if(AdminRepository.DeleteShareClassType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ShareClassAvailable(string shareClass,int shareClassTypeId) {
			if(AdminRepository.ShareClassTypeNameAvailable(shareClass,shareClassTypeId))
				return "Share Class already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region CashDistributionType

		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult CashDistributionType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="CashDistributionType";
			return View();
		}

		//
		// GET: /Admin/CashDistributionTypeList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult CashDistributionTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.CashDistributionType> cashDistributionTypes=AdminRepository.GetAllCashDistributionTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var cashDistributionType in cashDistributionTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {cashDistributionType.CashDistributionTypeID,
					  cashDistributionType.Name,
					  cashDistributionType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditCashDistributionType
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditCashDistributionType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.CashDistributionType cashDistributionType=AdminRepository.FindCashDistributionType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {cashDistributionType.CashDistributionTypeID,
					  cashDistributionType.Name,
					  cashDistributionType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCashDistributionType
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateCashDistributionType(FormCollection collection) {
			EditCashDistributionTypeModel model=new EditCashDistributionTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=CashDistributionTypeAvailable(model.Name,model.CashDistributionTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				CashDistributionType cashDistributionType=AdminRepository.FindCashDistributionType(model.CashDistributionTypeId);
				if(cashDistributionType==null) {
					cashDistributionType=new CashDistributionType();
				}
				cashDistributionType.Name=model.Name;
				cashDistributionType.Enabled=model.Enabled;
				cashDistributionType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveCashDistributionType(cashDistributionType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+cashDistributionType.CashDistributionTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteCashDistributionType(int id) {
			if(AdminRepository.DeleteCashDistributionType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string CashDistributionTypeAvailable(string cashDistributionType,int cashDistributionTypeId) {
			if(AdminRepository.CashDistributionTypeNameAvailable(cashDistributionType,cashDistributionTypeId))
				return "Cash Distribution Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region FundExpenseType

		public ActionResult FundExpenseType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="FundExpenseType";
			return View();
		}

		//
		// GET: /Admin/FundExpenseTypeList
		[HttpGet]
		public JsonResult FundExpenseTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.FundExpenseType> fundExpenseTypes=AdminRepository.GetAllFundExpenseTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				fundExpenseTypes=fundExpenseTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var fundExpenseType in fundExpenseTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {fundExpenseType.FundExpenseTypeID,
					  fundExpenseType.Name}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditFundExpenseType
		[HttpGet]
		public ActionResult EditFundExpenseType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.FundExpenseType fundExpenseType=AdminRepository.FindFundExpenseType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {fundExpenseType.FundExpenseTypeID,
					  fundExpenseType.Name}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateFundExpenseType
		[HttpPost]
		public ActionResult UpdateFundExpenseType(FormCollection collection) {
			EditFundExpenseTypeModel model=new EditFundExpenseTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=FundExpenseTypeAvailable(model.Name,model.FundExpenseTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				FundExpenseType fundExpenseType=AdminRepository.FindFundExpenseType(model.FundExpenseTypeId);
				if(fundExpenseType==null) {
					fundExpenseType=new FundExpenseType();
				}
				fundExpenseType.Name=model.Name;
				fundExpenseType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveFundExpenseType(fundExpenseType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+fundExpenseType.FundExpenseTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteFundExpenseType(int id) {
			if(AdminRepository.DeleteFundExpenseType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string FundExpenseTypeAvailable(string fundExpenseType,int fundExpenseTypeId) {
			if(AdminRepository.FundExpenseTypeNameAvailable(fundExpenseType,fundExpenseTypeId))
				return "Fund Expense Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ReportingFrequency

		public ActionResult ReportingFrequency() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="ReportingFrequency";
			return View();
		}

		//
		// GET: /Admin/ReportingFrequencyList
		[HttpGet]
		public JsonResult ReportingFrequencyList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.ReportingFrequency> reportingFrequencys=AdminRepository.GetAllReportingFrequencies(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				reportingFrequencys=reportingFrequencys.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var reportingFrequency in reportingFrequencys) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {reportingFrequency.ReportingFrequencyID,
					  reportingFrequency.ReportingFrequency1,
					  reportingFrequency.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditReportingFrequency
		[HttpGet]
		public ActionResult EditReportingFrequency(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.ReportingFrequency reportingFrequency=AdminRepository.FindReportingFrequency(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {reportingFrequency.ReportingFrequencyID,
					  reportingFrequency.ReportingFrequency1,
					  reportingFrequency.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateReportingFrequency
		[HttpPost]
		public ActionResult UpdateReportingFrequency(FormCollection collection) {
			EditReportingFrequencyModel model=new EditReportingFrequencyModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=ReportingFrequencyAvailable(model.ReportingFrequency,model.ReportingFrequencyId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				ReportingFrequency reportingFrequency=AdminRepository.FindReportingFrequency(model.ReportingFrequencyId);
				if(reportingFrequency==null) {
					reportingFrequency=new ReportingFrequency();
					reportingFrequency.CreatedBy=Authentication.CurrentUser.UserID;
					reportingFrequency.CreatedDate=DateTime.Now;
				}
				reportingFrequency.ReportingFrequency1=model.ReportingFrequency;
				reportingFrequency.Enabled=model.Enabled;
				reportingFrequency.EntityID=Authentication.CurrentEntity.EntityID;
				reportingFrequency.LastUpdatedBy=Authentication.CurrentUser.UserID;
				reportingFrequency.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveReportingFrequency(reportingFrequency);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+reportingFrequency.ReportingFrequencyID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteReportingFrequency(int id) {
			if(AdminRepository.DeleteReportingFrequency(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingFrequencyAvailable(string reportingFrequency,int reportingFrequencyId) {
			if(AdminRepository.ReportingFrequencyNameAvailable(reportingFrequency,reportingFrequencyId))
				return "Reporting already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ReportingType

		public ActionResult ReportingType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="ReportingType";
			return View();
		}

		//
		// GET: /Admin/ReportingTypeList
		[HttpGet]
		public JsonResult ReportingTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.ReportingType> reportingTypes=AdminRepository.GetAllReportingTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				reportingTypes=reportingTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var reportingType in reportingTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {reportingType.ReportingTypeID,
					  reportingType.Reporting,
					  reportingType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditReportingType
		[HttpGet]
		public ActionResult EditReportingType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.ReportingType reportingType=AdminRepository.FindReportingType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {reportingType.ReportingTypeID,
					  reportingType.Reporting,
					  reportingType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateReportingType
		[HttpPost]
		public ActionResult UpdateReportingType(FormCollection collection) {
			EditReportingTypeModel model=new EditReportingTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=ReportingTypeAvailable(model.Reporting,model.ReportingTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				ReportingType reportingType=AdminRepository.FindReportingType(model.ReportingTypeId);
				if(reportingType==null) {
					reportingType=new ReportingType();
					reportingType.CreatedBy=Authentication.CurrentUser.UserID;
					reportingType.CreatedDate=DateTime.Now;
				}
				reportingType.Reporting=model.Reporting;
				reportingType.Enabled=model.Enabled;
				reportingType.EntityID=Authentication.CurrentEntity.EntityID;
				reportingType.LastUpdatedBy=Authentication.CurrentUser.UserID;
				reportingType.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveReportingType(reportingType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+reportingType.ReportingTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteReportingType(int id) {
			if(AdminRepository.DeleteReportingType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string ReportingTypeAvailable(string reporting,int reportingTypeId) {
			if(AdminRepository.ReportingTypeNameAvailable(reporting,reportingTypeId))
				return "Reporting Type already exists.";
			else
				return string.Empty;
		}

		#endregion

		#endregion

		#region Module

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult MODULE() {
			ViewData["MenuName"]="AdminManagement";
			return View();
		}

		//
		// GET: /Admin/ModuleList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult ModuleList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.MODULE> modules=AdminRepository.GetAllModules(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var module in modules) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {module.ModuleID,
					  module.ModuleName}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditModule(int id) {
			EditModule model=new EditModule();
			MODULE module=AdminRepository.FindModule(id);
			if(module!=null) {
				model.ModuleID=module.ModuleID;
				model.ModuleName=module.ModuleName;

			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateModule
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateModule(FormCollection collection) {
			EditModule model=new EditModule();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=ModuleFieldTextAvailable(model.ModuleName,model.ModuleID);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("ModuleName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				MODULE module=AdminRepository.FindModule(model.ModuleID);
				if(module==null) {
					module=new MODULE();
				}
				module.ModuleName=model.ModuleName;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveModule(module);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True";
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteModule(int id) {
			if(AdminRepository.DeleteModule(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string ModuleFieldTextAvailable(string ModuleFieldText,int ModuleFieldId) {
			if(AdminRepository.ModuleTextAvailable(ModuleFieldText,ModuleFieldId))
				return "Custom Field Text already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Security Type

		//
		// GET: /Admin/SecurityType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult SecurityType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="SecurityType";
			return View();
		}

		//
		// GET: /Admin/SecurityTypeList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult SecurityTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.SecurityType> securityTypes=AdminRepository.GetAllSecurityTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var securityType in securityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {securityType.SecurityTypeID,
					  securityType.Name,
					  securityType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/SecurityType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditSecurityType(int id) {
			EditSecurityTypeModel model=new EditSecurityTypeModel();
			SecurityType securityType=AdminRepository.FindSecurityType(id);
			if(securityType!=null) {
				model.SecurityTypeId=securityType.SecurityTypeID;
				model.Name=securityType.Name;
				model.Enabled=securityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateSecurityType
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateSecurityType(FormCollection collection) {
			EditSecurityTypeModel model=new EditSecurityTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=SecurityTypeNameAvailable(model.Name,model.SecurityTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				SecurityType securityType=AdminRepository.FindSecurityType(model.SecurityTypeId);
				if(securityType==null) {
					securityType=new SecurityType();
				}
				securityType.Name=model.Name;
				securityType.Enabled=model.Enabled;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveSecurityType(securityType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True";
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteSecurityType(int id) {
			if(AdminRepository.DeleteSecurityType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string SecurityTypeNameAvailable(string Name,int SecurityTypeID) {
			if(AdminRepository.SecurityTypeNameAvailable(Name,SecurityTypeID))
				return "Name already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Geography

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult Geography() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="Geography";
			return View();
		}

		//
		// GET: /Admin/GeographyList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult GeographyList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.Geography> geographys=AdminRepository.GetAllGeographys(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var geography in geographys) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {geography.GeographyID,
					  geography.Geography1,
					  geography.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditGeography
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditGeography(int id) {
			EditGeographyModel model=new EditGeographyModel();
			Geography geography=AdminRepository.FindGeography(id);
			if(geography!=null) {
				model.GeographyId=geography.GeographyID;
				model.Geography=geography.Geography1;
				model.Enabled=geography.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateGeography
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateGeography(FormCollection collection) {
			EditGeographyModel model=new EditGeographyModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=GeographyAvailable(model.Geography,model.GeographyId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Geography geography=AdminRepository.FindGeography(model.GeographyId);
				if(geography==null) {
					geography=new Geography();
					geography.CreatedBy=Authentication.CurrentUser.UserID;
					geography.CreatedDate=DateTime.Now;
				}
				geography.Geography1=model.Geography;
				geography.Enabled=model.Enabled;
				geography.EntityID=Authentication.CurrentEntity.EntityID;
				geography.LastUpdatedBy=Authentication.CurrentUser.UserID;
				geography.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveGeography(geography);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True";
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteGeography(int id) {
			if(AdminRepository.DeleteGeography(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string GeographyAvailable(string Geography,int GeographyId) {
			if(AdminRepository.GeographyNameAvailable(Geography,GeographyId))
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
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="Industry";
			return View();
		}

		//
		// GET: /Admin/IndustryList
		[HttpGet]
		public JsonResult IndustryList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.Industry> industries=AdminRepository.GetAllIndustrys(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				industries=industries.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var industry in industries) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
						industry.IndustryID,
					    industry.Industry1,
					    industry.Enabled
					 }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/Industry
		[HttpGet]
		public ActionResult EditIndustry(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.Industry industry=AdminRepository.FindIndustry(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
						industry.IndustryID,
					    industry.Industry1,
					    industry.Enabled
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateIndustry
		[HttpPost]
		public ActionResult UpdateIndustry(FormCollection collection) {
			EditIndustryModel model=new EditIndustryModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=IndustryNameAvailable(model.Industry,model.IndustryId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Industry industry=AdminRepository.FindIndustry(model.IndustryId);
				if(industry==null) {
					industry=new Industry();
					industry.CreatedBy=Authentication.CurrentUser.UserID;
					industry.CreatedDate=DateTime.Now;
				}
				industry.Industry1=model.Industry;
				industry.EntityID=Authentication.CurrentEntity.EntityID;
				industry.Enabled=model.Enabled;
				industry.LastUpdatedBy=Authentication.CurrentUser.UserID;
				industry.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveIndustry(industry);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+industry.IndustryID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteIndustry(int id) {
			if(AdminRepository.DeleteIndustry(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string IndustryNameAvailable(string industryName,int industryId) {
			if(AdminRepository.IndustryNameAvailable(industryName,industryId))
				return "Industry already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Admin/FindIndustrys
		[HttpGet]
		public JsonResult FindIndustrys(string term) {
			return Json(AdminRepository.FindIndustrys(term),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region File Type

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult FileType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="AdminFileType";
			ViewData["PageName"]="FileType";
			return View();
		}

		//
		// GET: /Admin/FileTypeList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult FileTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.FileType> fileTypes=AdminRepository.GetAllFileTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var fileType in fileTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {fileType.FileTypeID,
					  fileType.FileTypeName,
					  fileType.FileExtension
					  }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EntityType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditFileType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.FileType fileType=AdminRepository.FindFileType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {fileType.FileTypeID,
					  fileType.FileTypeName,
					  fileType.FileExtension}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateFileType
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateFileType(FormCollection collection) {
			EditFileTypeModel model=new EditFileTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=FileTypeNameAvailable(model.FileTypeName,model.FileTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("FileTypeText",ErrorMessage);
			}
			if(ModelState.IsValid) {
				FileType fileType=AdminRepository.FindFileType(model.FileTypeId);
				if(fileType==null) {
					fileType=new FileType();
				}
				fileType.FileTypeName=model.FileTypeName;
				fileType.FileExtension=model.FileExtension;
				fileType.Description=model.Description;
				fileType.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveFileType(fileType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+fileType.FileTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteFileType(int id) {
			if(AdminRepository.DeleteFileType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string FileTypeNameAvailable(string FileTypeText,int FileTypeId) {
			if(AdminRepository.FileTypeNameAvailable(FileTypeText,FileTypeId))
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
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="EquityType";
			return View();
		}

		//
		// GET: /Admin/EquityTypeList
		[HttpGet]
		public JsonResult EquityTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.EquityType> equityTypes=AdminRepository.GetAllEquityTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				equityTypes=equityTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var equityType in equityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
						equityType.EquityTypeID,
					    equityType.Equity,
						equityType.Enabled
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EquityType
		[HttpGet]
		public ActionResult EditEquityType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			EquityType equityType=AdminRepository.FindEquityType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {equityType.EquityTypeID,
					  equityType.Equity,
					  equityType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateEquityType
		[HttpPost]
		public ActionResult UpdateEquityType(FormCollection collection) {
			EditEquityTypeModel model=new EditEquityTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=EquityTypeNameAvailable(model.EquityType,model.EquityTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Equity",ErrorMessage);
			}
			if(ModelState.IsValid) {
				EquityType equityType=AdminRepository.FindEquityType(model.EquityTypeId);
				if(equityType==null) {
					equityType=new EquityType();
					equityType.CreatedBy=Authentication.CurrentUser.UserID;
					equityType.CreatedDate=DateTime.Now;
				}
				equityType.Equity=model.EquityType;
				equityType.Enabled=model.Enabled;
				equityType.EntityID=Authentication.CurrentEntity.EntityID;
				equityType.LastUpdatedBy=Authentication.CurrentUser.UserID;
				equityType.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveEquityType(equityType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+equityType.EquityTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteEquityType(int id) {
			if(AdminRepository.DeleteEquityType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string EquityTypeNameAvailable(string Equity,int EquityTypeID) {
			if(AdminRepository.EquityTypeNameAvailable(Equity,EquityTypeID))
				return "Equity Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region FixedIncomeType
		//
		// GET: /Admin/FixedIncomeType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult FixedIncomeType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="FixedIncomeType";
			return View();
		}

		//
		// GET: /Admin/FixedIncomeTypeList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult FixedIncomeTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.FixedIncomeType> fixedIncomeTypes=AdminRepository.GetAllFixedIncomeTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var fixedIncomeType in fixedIncomeTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {fixedIncomeType.FixedIncomeTypeID,
					  fixedIncomeType.FixedIncomeType1, fixedIncomeType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/FixedIncomeType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditFixedIncomeType(int id) {
			EditFixedIncomeTypeModel model=new EditFixedIncomeTypeModel();
			FixedIncomeType fixedIncomeType=AdminRepository.FindFixedIncomeType(id);
			if(fixedIncomeType!=null) {
				model.FixedIncomeTypeId=fixedIncomeType.FixedIncomeTypeID;
				model.FixedIncomeType1=fixedIncomeType.FixedIncomeType1;
				model.Enabled=fixedIncomeType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateFixedIncomeType
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateFixedIncomeType(FormCollection collection) {
			EditFixedIncomeTypeModel model=new EditFixedIncomeTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=FixedIncomeTypeNameAvailable(model.FixedIncomeType1,model.FixedIncomeTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("FixedIncome",ErrorMessage);
			}
			if(ModelState.IsValid) {
				FixedIncomeType fixedIncomeType=AdminRepository.FindFixedIncomeType(model.FixedIncomeTypeId);
				if(fixedIncomeType==null) {
					fixedIncomeType=new FixedIncomeType();
					fixedIncomeType.CreatedBy=Authentication.CurrentUser.UserID;
					fixedIncomeType.CreatedDate=DateTime.Now;
				}
				fixedIncomeType.FixedIncomeType1=model.FixedIncomeType1;
				fixedIncomeType.Enabled=model.Enabled;
				fixedIncomeType.EntityID=Authentication.CurrentEntity.EntityID;
				fixedIncomeType.LastUpdatedBy=Authentication.CurrentUser.UserID;
				fixedIncomeType.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveFixedIncomeType(fixedIncomeType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True";
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteFixedIncomeType(int id) {
			if(AdminRepository.DeleteFixedIncomeType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string FixedIncomeTypeNameAvailable(string FixedIncomeType,int FixedIncomeTypeID) {
			if(AdminRepository.FixedIncomeTypeNameAvailable(FixedIncomeType,FixedIncomeTypeID))
				return "Fixed Income Type already exist";
			else
				return string.Empty;
		}

		#endregion

		#region ActivityType

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult ActivityType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="CustomFieldManagement";
			ViewData["PageName"]="ActivityType";
			return View();
		}

		//
		// GET: /Admin/ActivityTypeList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult ActivityTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.ActivityType> activityTypes=AdminRepository.GetAllActivityTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var activityType in activityTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {activityType.ActivityTypeID,
					  activityType.Name,
					  activityType.Enabled}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditActivityType
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditActivityType(int id) {
			EditActivityTypeModel model=new EditActivityTypeModel();
			ActivityType activityType=AdminRepository.FindActivityType(id);
			if(activityType!=null) {
				model.ActivityTypeId=activityType.ActivityTypeID;
				model.Name=activityType.Name;
				model.Enabled=activityType.Enabled;
			}
			return View(model);
		}

		//
		// GET: /Admin/UpdateActivityType
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateActivityType(FormCollection collection) {
			EditActivityTypeModel model=new EditActivityTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=ActivityTypeAvailable(model.Name,model.ActivityTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				ActivityType activityType=AdminRepository.FindActivityType(model.ActivityTypeId);
				if(activityType==null) {
					activityType=new ActivityType();
				}
				activityType.Name=model.Name;
				activityType.Enabled=model.Enabled;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveActivityType(activityType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True";
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteActivityType(int id) {
			if(AdminRepository.DeleteActivityType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string ActivityTypeAvailable(string ActivityType,int ActivityTypeId) {
			if(AdminRepository.ActivityTypeNameAvailable(ActivityType,ActivityTypeId))
				return "Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region Country

		//
		// GET: /Admin/FindCountrys
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult FindCountrys(string term) {
			return Json(AdminRepository.FindCountrys(term),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region State
		//
		// GET: /Admin/FindStates
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult FindStates(string term) {
			return Json(AdminRepository.FindStates(term),JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Currency
		//
		// GET: /Admin/Currency
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult Currency() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="Currency";
			return View();
		}

		//
		// GET: /Admin/CurrencyList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult CurrencyList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.Currency> currencies=AdminRepository.GetAllCurrencies(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var currency in currencies) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
						currency.CurrencyID,
					    currency.Currency1,
					    currency.Enabled
					 }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/Currency
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditCurrency(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.Currency currency=AdminRepository.FindCurrency(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
						currency.CurrencyID,
					    currency.Currency1,
					    currency.Enabled
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateCurrency
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateCurrency(FormCollection collection) {
			EditCurrencyModel model=new EditCurrencyModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=CurrencyNameAvailable(model.Currency,model.CurrencyId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Currency currency=AdminRepository.FindCurrency(model.CurrencyId);
				if(currency==null) {
					currency=new Currency();
					currency.CreatedBy=Authentication.CurrentUser.UserID;
					currency.CreatedDate=DateTime.Now;
				}
				currency.Currency1=model.Currency;
				currency.EntityID=Authentication.CurrentEntity.EntityID;
				currency.Enabled=model.Enabled;
				currency.LastUpdatedBy=Authentication.CurrentUser.UserID;
				currency.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveCurrency(currency);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+currency.CurrencyID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteCurrency(int id) {
			if(AdminRepository.DeleteCurrency(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string CurrencyNameAvailable(string currencyName,int currencyId) {
			if(AdminRepository.CurrencyNameAvailable(currencyName,currencyId))
				return "Name already exist";
			else
				return string.Empty;
		}

		#endregion

		#region Fund Closing
		//
		// GET: /Admin/FundClosing
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult FundClosing() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="InvestorManagement";
			ViewData["PageName"]="FundClosing";
			return View();
		}

		//
		// GET: /Admin/FundClosingList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult FundClosingList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.FundClosing> fundClosings=AdminRepository.GetAllFundClosings(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var fundClosing in fundClosings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {fundClosing.FundClosingID,
					  fundClosing.Name,
					 fundClosing.Fund.FundName,
					 fundClosing.FundClosingDate,
					 fundClosing.IsFirstClosing,
					 fundClosing.FundID
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditFundClosing
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditFundClosing(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.FundClosing fundClosing=AdminRepository.FindFundClosing(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {fundClosing.FundClosingID,
					  fundClosing.Name,
					 fundClosing.Fund.FundName,
					 fundClosing.FundClosingDate,
					 fundClosing.IsFirstClosing,
					 fundClosing.FundID
					}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateFundClosing
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateFundClosing(FormCollection collection) {
			EditFundClosingModel model=new EditFundClosingModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=FundClosingNameAvailable(model.Name,model.FundClosingId,model.FundId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				FundClosing fundClosing=AdminRepository.FindFundClosing(model.FundClosingId);
				if(fundClosing==null) {
					fundClosing=new FundClosing();
				}
				fundClosing.Name=model.Name;
				fundClosing.FundClosingDate=model.FundClosingDate;
				fundClosing.FundID=model.FundId;
				fundClosing.IsFirstClosing=model.IsFirstClosing;
				fundClosing.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveFundClosing(fundClosing);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+fundClosing.FundClosingID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteFundClosing(int id) {
			if(AdminRepository.DeleteFundClosing(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string FundClosingNameAvailable(string name,int fundClosingId,int fundId) {
			if(AdminRepository.FundClosingNameAvailable(name,fundClosingId,fundId))
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
			return Json(AdminRepository.FindDealContacts(term),JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Entity

		[AdminAuthorize()]
		[SystemEntityAuthorize]
		public ActionResult Entities() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="EntitySetup";
			ViewData["PageName"]="Entity";
			return View();
		}

		//
		// GET: /Admin/EntityList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult EntityList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<ENTITY> Entities=AdminRepository.GetAllEntities(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var entity in Entities) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
					  entity.EntityID,
					  entity.EntityName,
					  entity.EntityCode,
					  entity.Enabled
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditEntity
		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult EditEntity(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			ENTITY entity=AdminRepository.FindEntity(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
					  entity.EntityID,
					  entity.EntityName,
					  entity.EntityCode,
					  entity.Enabled
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateEntity
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateEntity(FormCollection collection) {
			ICacheManager cacheManager=new MemoryCacheManager();
			ENTITY model=new ENTITY();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			bool isNewEntity=false;
			string ErrorMessage=EntityNameAvailable(model.EntityName,model.EntityID);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("EntityName",ErrorMessage);
			}
			ErrorMessage=EntityCodeAvailable(model.EntityCode,model.EntityID);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("EntityCode",ErrorMessage);
			}
			if(ModelState.IsValid) {
				ENTITY entity=AdminRepository.FindEntity(model.EntityID);
				if(entity==null) {
					entity=new ENTITY();
					entity.CreatedDate=DateTime.Now;
					isNewEntity=true;
				}
				entity.EntityName=model.EntityName;
				entity.EntityCode=model.EntityCode;
				entity.Enabled=model.Enabled;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveEntity(entity);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+entity.EntityID;
					// Create default user
					if(isNewEntity) {
						string username="admin";
						string password="admin";
						string firstname="admin";
						string lastname="admin";
						string email="admin@admin.com";
						USER user=new USER();
						user.Login=username;
						user.PasswordSalt=SecurityExtensions.CreateSalt();
						user.PasswordHash=password.CreateHash(user.PasswordSalt);
						user.Email=email;
						user.FirstName=firstname;
						user.LastName=lastname;
						user.EntityID=entity.EntityID;
						user.CreatedDate=DateTime.Now;
						user.LastUpdatedDate=DateTime.Now;
						user.Enabled=true;
						user.IsAdmin=true;
						AdminRepository.SaveUser(user);
					}
				}
				// Create entity menu
				if(AdminRepository.GetEntityMenuCount(entity.EntityID)<=0) {
					AdminRepository.SaveEntityMenu(entity.EntityID);
					// Remove entity menu cache
					cacheManager.RemoveByPattern(string.Format(MenuHelper.ENTITYMENUKEY,entity.EntityID));
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteEntity(int id) {
			if(id>1) {
				if(AdminRepository.DeleteEntity(id)==false) {
					return "Cann't Delete! Child record found!";
				} else {
					return string.Empty;
				}
			} else {
				return "Cann't Delete! Child record found!";
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string EntityNameAvailable(string entityName,int EntityId) {
			if(AdminRepository.EntityNameAvailable(entityName,EntityId))
				return "Entity Name already exists.";
			else
				return string.Empty;
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string EntityCodeAvailable(string entityName,int EntityId) {
			if(AdminRepository.EntityCodeAvailable(entityName,EntityId))
				return "Entity Code already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region User

		[HttpGet]
		[AdminAuthorize]
		public ActionResult Users() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="UserManagement";
			ViewData["PageName"]="User";
			EditUserModel model=new EditUserModel();
			model.Entities=SelectListFactory.GetEntitiesSelectList(AdminRepository.GetAllEntities());
			return View(model);
		}

		//
		// GET: /Admin/UserList
		[HttpGet]
		public JsonResult UserList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<USER> Users=AdminRepository.GetAllUsers(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var user in Users) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
					  user.UserID,
					  user.FirstName,
					  user.LastName,
					  user.Login,
					  user.Email,
					  user.Enabled,
					  user.MiddleName,
					  user.IsAdmin,
					  user.PhoneNumber,
					  user.EntityID
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditUser
		[HttpGet]
		public ActionResult EditUser(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			USER user=AdminRepository.FindUser(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
					  user.UserID,
					  user.FirstName,
					  user.LastName,
					  user.Login,
					  user.Email,
					  user.Enabled,
					  user.MiddleName,
					  user.IsAdmin,
					  user.PhoneNumber,
					  user.EntityID
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateUser
		[HttpPost]
		public ActionResult UpdateUser(FormCollection collection) {
			EditUserModel model=new EditUserModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=UserNameAvailable(model.Login,model.UserId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Login",ErrorMessage);
			}
			ErrorMessage=EmailAvailable(model.Email,model.UserId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Email",ErrorMessage);
			}
			if(model.ChangePassword) {
				if(string.IsNullOrEmpty(model.Password))
					ModelState.AddModelError("Password","Password is required");
			}
			if(ModelState.IsValid) {
				USER user=AdminRepository.FindUser(model.UserId);
				if(user==null) {
					user=new USER();
					user.CreatedDate=DateTime.Now;
				}
				user.EntityID=Authentication.CurrentEntity.EntityID;
				user.LastUpdatedDate=DateTime.Now;

				user.FirstName=model.FirstName;
				user.LastName=model.LastName;
				user.MiddleName=model.MiddleName;
				user.PhoneNumber=model.PhoneNumber;
				if(model.ChangePassword) {
					user.PasswordSalt=SecurityExtensions.CreateSalt();
					user.PasswordHash=model.Password.CreateHash(user.PasswordSalt);
				}
				user.Login=model.Login;
				user.Email=model.Email;
				user.Enabled=model.Enabled;
				user.IsAdmin=model.IsAdmin;

				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveUser(user);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+user.UserID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteUser(int id) {
			if(AdminRepository.DeleteUser(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string UserNameAvailable(string userName,int UserId) {
			if(AdminRepository.UserNameAvailable(userName,UserId))
				return "Username already exists.";
			else
				return string.Empty;
		}

		[HttpGet]
		public string EmailAvailable(string email,int UserId) {
			if(AdminRepository.EmailAvailable(email,UserId))
				return "Email already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region DocumentType

		[HttpGet]
		public ActionResult DocumentType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DocumentManagement";
			ViewData["PageName"]="DocumentType";
			EditDocumentTypeModel model=new EditDocumentTypeModel();
			model.DocumentSections=SelectListFactory.GetDocumentSectionList(AdminRepository.GetAllDocumentSections()); ;
			return View(model);
		}

		//
		// GET: /Admin/DocumentTypeList
		[HttpGet]
		public JsonResult DocumentTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.DocumentType> documentTypes=AdminRepository.GetAllDocumentTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				documentTypes=documentTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var documentType in documentTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {documentType.DocumentTypeID,
					  documentType.DocumentTypeName,
					  documentType.DocumentSectionID,
					  documentType.DocumentSection.Name
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditDocumentType
		[HttpGet]
		public JsonResult EditDocumentType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DocumentType documentType=AdminRepository.FindDocumentType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {documentType.DocumentTypeID,
					 documentType.DocumentTypeName,
					 documentType.DocumentSectionID,
					 documentType.DocumentSection.Name
					}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateDocumentType
		[HttpPost]
		public ActionResult UpdateDocumentType(FormCollection collection) {
			EditDocumentTypeModel model=new EditDocumentTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=DocumentTypeAvailable(model.DocumentTypeName,model.DocumentTypeID);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				DocumentType documentType=AdminRepository.FindDocumentType(model.DocumentTypeID);
				if(documentType==null) {
					documentType=new DocumentType();
				}
				documentType.EntityID=Authentication.CurrentEntity.EntityID;
				documentType.DocumentTypeName=model.DocumentTypeName;
				documentType.DocumentSectionID=model.DocumentSectionID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveDocumentType(documentType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+documentType.DocumentTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteDocumentType(int id) {
			if(AdminRepository.DeleteDocumentType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string DocumentTypeAvailable(string DocumentType,int DocumentTypeId) {
			if(AdminRepository.DocumentTypeNameAvailable(DocumentType,DocumentTypeId))
				return "Document Type Name already exists.";
			else
				return string.Empty;
		}

		//
		// GET: /Admin/FindDocumentTypes
		[HttpGet]
		public JsonResult FindDocumentTypes(string term,int documentSectionId) {
			return Json(AdminRepository.FindDocumentTypes(term,documentSectionId),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region DealContact

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult DealContact() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="DealContact";
			return View();
		}

		//
		// GET: /Admin/DealContactList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult DealContactList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealContactList> dealContacts=AdminRepository.GetAllDealContacts(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var dealContact in dealContacts) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
					  dealContact.ContactId,
					  dealContact.ContactName,
					  dealContact.ContactTitle,
					  dealContact.ContactNotes,
					  dealContact.Email,
					  dealContact.Phone,
					  dealContact.WebAddress
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditDealContact
		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult EditDealContact(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			Contact dealContact=AdminRepository.FindContact(id);
			List<CommunicationDetailModel> communications=AdminRepository.GetContactCommunications(dealContact.ContactID);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
					  dealContact.ContactID,
					  dealContact.ContactName,
					  dealContact.Title,
					  dealContact.Notes,
					  AdminRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Email),
					  AdminRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.HomePhone),
					  AdminRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.WebAddress),
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateDealContact
		[HttpPost]
		[OtherEntityAuthorize]
		public ActionResult UpdateDealContact(FormCollection collection) {
			EditDealContactModel model=new EditDealContactModel();
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(model);
			string ErrorMessage=DealContactNameAvailable(model.ContactName,model.ContactId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("ContactName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Contact contact=AdminRepository.FindContact(model.ContactId);
				if(contact==null) {
					contact=new Contact();
					contact.CreatedBy=Authentication.CurrentUser.UserID;
					contact.CreatedDate=DateTime.Now;
				}
				contact.LastUpdatedBy=Authentication.CurrentUser.UserID;
				contact.LastUpdatedDate=DateTime.Now;
				contact.EntityID=Authentication.CurrentEntity.EntityID;
				contact.ContactName=model.ContactName;
				contact.LastName="n/a";
				contact.Title=model.ContactTitle;
				contact.Notes=model.ContactNotes;
				AddCommunication(contact,Models.Admin.Enums.CommunicationType.Email,model.Email);
				AddCommunication(contact,Models.Admin.Enums.CommunicationType.HomePhone,model.Phone);
				AddCommunication(contact,Models.Admin.Enums.CommunicationType.WebAddress,model.WebAddress);
				errorInfo=AdminRepository.SaveDealContact(contact);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result+="True||"+contact.ContactID.ToString();
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DealContactNameAvailable(string dealContactName,int contactID) {
			if(AdminRepository.DealContactNameAvailable(dealContactName,contactID))
				return "Contact Name already exists.";
			else
				return string.Empty;
		}

		private void AddCommunication(DeepBlue.Models.Entity.Contact contact,DeepBlue.Models.Admin.Enums.CommunicationType communicationType,string value) {
			ContactCommunication contactCommunication=contact.ContactCommunications.SingleOrDefault(communication => communication.Communication.CommunicationTypeID==(int)communicationType);
			if(contactCommunication==null) {
				contactCommunication=new ContactCommunication();
				contactCommunication.CreatedBy=Authentication.CurrentUser.UserID;
				contactCommunication.CreatedDate=DateTime.Now;
				contactCommunication.Communication=new Communication();
				contactCommunication.Communication.CreatedBy=Authentication.CurrentUser.UserID;
				contactCommunication.Communication.CreatedDate=DateTime.Now;
				contact.ContactCommunications.Add(contactCommunication);
			}
			contactCommunication.EntityID=Authentication.CurrentEntity.EntityID;
			contactCommunication.LastUpdatedBy=Authentication.CurrentUser.UserID;
			contactCommunication.LastUpdatedDate=DateTime.Now;
			contactCommunication.Communication.CommunicationTypeID=(int)communicationType;
			contactCommunication.Communication.CommunicationValue=(string.IsNullOrEmpty(value)==false?value:string.Empty);
			contactCommunication.Communication.LastUpdatedBy=Authentication.CurrentUser.UserID;
			contactCommunication.Communication.LastUpdatedDate=DateTime.Now;
			contactCommunication.Communication.EntityID=Authentication.CurrentEntity.EntityID;
		}

		[HttpGet]
		[OtherEntityAuthorize]
		public string DeleteDealContact(int id) {
			if(AdminRepository.DeleteDealContact(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region SelectList
		[HttpGet]
		[OtherEntityAuthorize]
		public JsonResult SelectList(string actionName) {
			List<SelectListItem> items=null;
			switch(actionName) {
			case "FundType":
				items=SelectListFactory.GetUnderlyingFundTypeSelectList(AdminRepository.GetAllUnderlyingFundTypes());
				break;
			case "ReportingFrequency":
				items=SelectListFactory.GetReportingFrequencySelectList(AdminRepository.GetAllReportingFrequencies());
				break;
			case "ReportingType":
				items=SelectListFactory.GetReportingTypeSelectList(AdminRepository.GetAllReportingTypes());
				break;
			case "Industry":
				items=SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
				break;
			case "Geography":
				items=SelectListFactory.GetGeographySelectList(AdminRepository.GetAllGeographies());
				break;
			case "InvestorType":
				items=SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
				break;
			case "ActivityType":
				items=SelectListFactory.GetActivityTypeSelectList(AdminRepository.GetAllActivityTypes());
				break;
			case "Currency":
				items=SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
				break;
			case "ShareClassType":
				items=SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
				break;
			case "EquityType":
				items=SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
				break;
			case "DataType":
				items=SelectListFactory.GetDataTypeSelectList(AdminRepository.GetAllDataTypes());
				break;
			case "Module":
				items=SelectListFactory.GetModuleSelectList(AdminRepository.GetAllModules());
				break;
			case "CommunicationGroup":
				items=SelectListFactory.GetCommunicationGroupingSelectList(AdminRepository.GetAllCommunicationGroupings());
				break;
			case "DocumentType":
				items=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
				break;
			case "PurchaseType":
				items=SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
				break;
			case "FixedIncomeType":
				items=SelectListFactory.GetFixedIncomeTypesSelectList(AdminRepository.GetAllFixedIncomeTypes());
				break;
			case "FundExpenseType":
				items=SelectListFactory.GetFundExpenseTypeSelectList(AdminRepository.GetAllFundExpenseTypes());
				break;
			}
			return Json(items,JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region FileUpload

		[HttpGet]
		[OtherEntityAuthorize]
		public ActionResult FileUpload() {
			return View();
		}

		//
		// POST: /Admin/Upload
		[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Upload(FormCollection collection) {
			ImportExcelFileModel model=new ImportExcelFileModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			if(model.UploadFile==null) {
				ModelState.AddModelError("UploadFile","File is required");
			}
			if(model.UploadFile!=null) {
				string ext=Path.GetExtension(model.UploadFile.FileName).ToLower();
				if(ext!=".xlsx") {
					ModelState.AddModelError("UploadFile",".xlsx files only allowed");
				}
			}
			if(ModelState.IsValid) {
				DeepBlue.Models.File.UploadFileModel uploadFileModel=UploadFileHelper.UploadTempFile(model.UploadFile);
				model.FileName=uploadFileModel.FileName;
				model.FilePath=uploadFileModel.FilePath;
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			resultModel.Result=JsonSerializer.ToJsonObject(new { Result=resultModel.Result,FilePath=model.FilePath,FileName=model.FileName }).ToString();
			return View("Result",resultModel);
		}

		//
		// POST: /Admin/UploadFile
		[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult UploadFile() {
			int index=0;
			string fileName=string.Empty;
			for(index=0;index<Request.Files.Count;index++) {
				DeepBlue.Models.File.UploadFileModel uploadFileModel=UploadFileHelper.Upload(Request.Files[index],"TempUploadPath",Request.Files[index].FileName);
				fileName=uploadFileModel.FileName;
			}
			return Json(new { Error=string.Empty,FileName=fileName });
		}

		#endregion

		#region Log
		public ActionResult Log() {
			return View();
		}

		//
		// GET: /Admin/DocumentTypeList
		[HttpGet]
		public JsonResult LogList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.Log> logss=AdminRepository.GetAllLogs(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var log in logss) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { 
						log.Controller,
						log.Action,
						log.View,
						log.QueryString,
						log.LogText,
						log.LogDetails.FirstOrDefault().Detail
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region SellerType

		public ActionResult SellerType() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="DealManagement";
			ViewData["PageName"]="SellerType";
			return View();
		}

		//
		// GET: /Admin/SellerTypeList
		[HttpGet]
		public JsonResult SellerTypeList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.SellerType> sellerTypes=AdminRepository.GetAllSellerTypes(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			if(Authentication.CurrentEntity.EntityID!=(int)ConfigUtil.SystemEntityID) {
				sellerTypes=sellerTypes.Where(e => e.EntityID==Authentication.CurrentEntity.EntityID).ToList();
			}
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var sellerType in sellerTypes) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {sellerType.SellerTypeID,
					  sellerType.SellerType1,
					  sellerType.Enabled 
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditSellerType
		[HttpGet]
		public ActionResult EditSellerType(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			DeepBlue.Models.Entity.SellerType sellerType=AdminRepository.FindSellerType(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {sellerType.SellerTypeID,
					  sellerType.SellerType1,
					  sellerType.Enabled}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateSellerType
		[HttpPost]
		public ActionResult UpdateSellerType(FormCollection collection) {
			EditSellerTypeModel model=new EditSellerTypeModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=SellerAvailable(model.SellerType,model.SellerTypeId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				SellerType sellerType=AdminRepository.FindSellerType(model.SellerTypeId);
				if(sellerType==null) {
					sellerType=new SellerType();
					sellerType.CreatedBy=Authentication.CurrentUser.UserID;
					sellerType.CreatedDate=DateTime.Now;
				}
				sellerType.SellerType1=model.SellerType;
				sellerType.Enabled=model.Enabled;
				sellerType.EntityID=Authentication.CurrentEntity.EntityID;
				sellerType.LastUpdatedBy=Authentication.CurrentUser.UserID;
				sellerType.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveSellerType(sellerType);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					resultModel.Result="True||"+sellerType.SellerTypeID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteSellerType(int id) {
			if(AdminRepository.DeleteSellerType(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public string SellerAvailable(string Seller,int SellerTypeId) {
			if(AdminRepository.SellerTypeNameAvailable(Seller,SellerTypeId))
				return "Seller Type already exists.";
			else
				return string.Empty;
		}

		//
		// GET: /Fund/FindSellerTypes
		[HttpGet]
		public JsonResult FindSellerTypes(string term) {
			return Json(AdminRepository.FindSellerTypes(term),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region ExportExcel

		[OtherEntityAuthorize]
		public ActionResult Export() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="ExportExcel";
			ViewData["PageName"]="ExportExcel";
			return View();
		}

		//public ActionResult ExportList(int pageIndex, int pageSize, string sortName, string sortOrder, string tableName) {
		//    int totalRows = 0;
		//    var rows = AdminRepository.GetAllTables(pageIndex, pageSize, sortName, sortOrder, ref totalRows, tableName);
		//    return Json(new { total = totalRows, page = pageIndex,  rows = rows
		//     }, JsonRequestBehavior.AllowGet);
		//}

		[OtherEntityAuthorize]
		public ActionResult ExportList() {
			FlexigridData data=new FlexigridData { page=1,total=4 };
			data.rows.Add(new FlexigridRow { cell=new List<object> { (int)DeepBlue.Models.Admin.Enums.ExportExcelType.Investor,"Investors" } });
			data.rows.Add(new FlexigridRow { cell=new List<object> { (int)DeepBlue.Models.Admin.Enums.ExportExcelType.AmberbrookFund,"Funds" } });
			data.rows.Add(new FlexigridRow { cell=new List<object> { (int)DeepBlue.Models.Admin.Enums.ExportExcelType.UnderlyingFund,"Underlying Funds" } });
			data.rows.Add(new FlexigridRow { cell=new List<object> { (int)DeepBlue.Models.Admin.Enums.ExportExcelType.UnderlyingDirect,"Underlying Directs" } });
			return Json(data,JsonRequestBehavior.AllowGet);
		}

		[OtherEntityAuthorize]
		public ActionResult ExportExcel(string tableName,int? excelExportTypeId) {
			if(excelExportTypeId>0) {
				string actionName=string.Empty;
				switch((DeepBlue.Models.Admin.Enums.ExportExcelType)excelExportTypeId) {
				case ExportExcelType.Investor:
					actionName="InvestorExportExcel";
					break;
				case ExportExcelType.AmberbrookFund:
					actionName="FundExportExcel";
					break;
				case ExportExcelType.UnderlyingFund:
					actionName="UnderlyingFundExportExcel";
					break;
				case ExportExcelType.UnderlyingDirect:
					actionName="UnderlyingDirectExportExcel";
					break;
				}
				return RedirectToAction(actionName);
			} else {
				return new ExportExcel { TableName=tableName,GridData=AdminRepository.FindTable(tableName) };
			}
		}

		[OtherEntityAuthorize]
		public ActionResult InvestorExportExcel() {
			return View(AdminRepository.GetAllInvestorExportList());
		}

		[OtherEntityAuthorize]
		public ActionResult FundExportExcel() {
			return View(AdminRepository.GetAllFundExportList());
		}

		[OtherEntityAuthorize]
		public ActionResult UnderlyingFundExportExcel() {
			return View(AdminRepository.GetAllUnderlyingFundExportList());
		}

		[OtherEntityAuthorize]
		public ActionResult UnderlyingDirectExportExcel() {
			return View(AdminRepository.GetAllUnderlyingDirectExportList());
		}

		#endregion

		#region Menu

		[HttpGet]
		[SystemEntityAuthorize]
		public ActionResult Menu() {
			ViewData["MenuName"]="AdminManagement";
			ViewData["SubmenuName"]="EntitySetup";
			ViewData["PageName"]="Menu";
			return View();
		}

		//
		// GET: /Admin/MenuList
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult MenuList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DeepBlue.Models.Entity.Menu> menus=AdminRepository.GetAllMenus(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var menu in menus) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {menu.MenuID,
					  menu.DisplayName,
					  menu.ParentMenuID,
					 (menu.Menu2 != null ? (menu.Menu2.Menu2 != null ? menu.Menu2.Menu2.DisplayName + " -> " : string.Empty) : string.Empty) + (menu.Menu2 != null ? menu.Menu2.DisplayName : string.Empty),
					  menu.Title,
					  menu.URL
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditMenu
		[HttpGet]
		[SystemEntityAuthorize]
		public JsonResult EditMenu(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			Menu menu=AdminRepository.FindMenu(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {menu.MenuID,
					  menu.DisplayName,
					  menu.ParentMenuID,
					  (menu.Menu2 != null ? (menu.Menu2.Menu2 != null ? menu.Menu2.Menu2.DisplayName + " -> " : string.Empty) : string.Empty) + (menu.Menu2 != null ? menu.Menu2.DisplayName : string.Empty),
					  menu.Title,
					  menu.URL
					}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateMenu
		[HttpPost]
		[SystemEntityAuthorize]
		public ActionResult UpdateMenu(FormCollection collection) {
			ICacheManager cacheManager=new MemoryCacheManager();
			EditMenuModel model=new EditMenuModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				Menu menu=AdminRepository.FindMenu(model.MenuID);
				if(menu==null) {
					menu=new Menu();
				}
				menu.DisplayName=model.DisplayName;
				menu.ParentMenuID=model.ParentMenuID;
				menu.URL=model.URL;
				menu.Title=model.Title;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveMenu(menu);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					// Remove entity menu cache
					cacheManager.RemoveByPattern(MenuHelper.ENTITYMENUKEY);
					resultModel.Result="True||"+menu.MenuID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string DeleteMenu(int id) {
			if(AdminRepository.DeleteMenu(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				// Remove entity menu cache
				ICacheManager cacheManager=new MemoryCacheManager();
				cacheManager.RemoveByPattern(MenuHelper.ENTITYMENUKEY);
				return string.Empty;
			}
		}

		[HttpGet]
		[SystemEntityAuthorize]
		public string MenuAvailable(string Menu,int MenuID) {
			if(AdminRepository.MenuNameAvailable(Menu,MenuID))
				return "Menu Name already exists.";
			else
				return string.Empty;
		}

		[HttpGet]
		public JsonResult FindMenus(string term,int? menuID) {
			return Json(AdminRepository.FindMenus(term,menuID),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region EntityMenu

		[HttpGet]
		public ActionResult EntityMenu() {
			ViewData["EntityMenuName"]="AdminManagement";
			ViewData["SubentityMenuName"]="EntitySetup";
			ViewData["PageName"]="EntityMenu";
			return View();
		}

		//
		// GET: /Admin/EntityMenuList
		[HttpGet]
		public JsonResult EntityMenuList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<EntityMenuModel> entityMenus=AdminRepository.GetAllEntityMenus(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var entityMenu in entityMenus) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {entityMenu.EntityMenuID,
					  entityMenu.DisplayName, 
					  entityMenu.MenuID, 
					  entityMenu.MenuName,
					  entityMenu.SortOrder
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/EditEntityMenu
		[HttpGet]
		public JsonResult EditEntityMenu(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			EntityMenuModel entityMenu=AdminRepository.GetEntityMenu(id);
			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {entityMenu.EntityMenuID,
					  entityMenu.DisplayName, 
					  entityMenu.MenuID, 
					  entityMenu.MenuName,
					  entityMenu.SortOrder
					}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/UpdateEntityMenu
		[HttpPost]
		public ActionResult UpdateEntityMenu(FormCollection collection) {
			ICacheManager cacheManager=new MemoryCacheManager();
			EditEntityMenuModel model=new EditEntityMenuModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			//string ErrorMessage = EntityMenuAvailable(model.DisplayName, model.EntityMenuID);
			//if (String.IsNullOrEmpty(ErrorMessage) == false) {
			//    ModelState.AddModelError("DisplayName", ErrorMessage);
			//}
			if(ModelState.IsValid) {
				EntityMenu entityMenu=AdminRepository.FindEntityMenu(model.EntityMenuID);
				if(entityMenu==null) {
					entityMenu=new EntityMenu();
				}
				entityMenu.DisplayName=model.DisplayName;
				entityMenu.MenuID=model.MenuID;
				entityMenu.EntityID=Authentication.CurrentEntity.EntityID;
				IEnumerable<ErrorInfo> errorInfo=AdminRepository.SaveEntityMenu(entityMenu);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					// Remove entity menu cache
					cacheManager.RemoveByPattern(string.Format(MenuHelper.ENTITYMENUKEY,entityMenu.EntityID));
					resultModel.Result="True||"+entityMenu.EntityMenuID;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Admin/UpdateEntityMenuSortOrder
		[HttpPost]
		public ActionResult UpdateEntityMenuSortOrder(FormCollection collection) {
			ICacheManager cacheManager=new MemoryCacheManager();
			ResultModel resultModel=new ResultModel();
			int entityMenuID=0;
			int alterEntityMenuID=0;
			int.TryParse(collection["entityMenuID"],out entityMenuID);
			int.TryParse(collection["alterEntityMenuID"],out alterEntityMenuID);
			EntityMenu entityMenu=AdminRepository.FindEntityMenu(entityMenuID);
			EntityMenu alterEntityMenu=AdminRepository.FindEntityMenu(alterEntityMenuID);
			IEnumerable<ErrorInfo> errorInfo=null;
			if(entityMenu!=null&&alterEntityMenu!=null) {
				int entityMenuSortOrder=entityMenu.SortOrder;
				int alterEntityMenuSortOrder=alterEntityMenu.SortOrder;
				entityMenu.SortOrder=alterEntityMenuSortOrder;
				alterEntityMenu.SortOrder=entityMenuSortOrder;
				AdminRepository.SaveEntityMenu(entityMenu);
				AdminRepository.SaveEntityMenu(alterEntityMenu);
			}
			if(errorInfo!=null) {
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
			} else {
				// Remove entity menu cache
				cacheManager.RemoveByPattern(string.Format(MenuHelper.ENTITYMENUKEY,entityMenu.EntityID));
				resultModel.Result="True||"+entityMenu.EntityMenuID;
			}
			return View("Result",resultModel);
		}

		[HttpGet]
		public string DeleteEntityMenu(int id) {
			if(AdminRepository.DeleteEntityMenu(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				// Remove entity menu cache
				ICacheManager cacheManager=new MemoryCacheManager();
				cacheManager.RemoveByPattern(string.Format(MenuHelper.ENTITYMENUKEY,id));
				return string.Empty;
			}
		}

		[HttpGet]
		public string EntityMenuAvailable(string EntityMenu,int EntityMenuID) {
			if(AdminRepository.EntityMenuNameAvailable(EntityMenu,EntityMenuID))
				return "EntityMenu Name already exists.";
			else
				return string.Empty;
		}

		#endregion

		#region ScheduleK1

		[OtherEntityAuthorize]
		[HttpGet]
		public ActionResult ScheduleK1() {
			return View(new ScheduleK1Model());
		}

		//
		// GET: /Admin/ScheduleK1List
		[OtherEntityAuthorize]
		[HttpGet]
		public ActionResult ScheduleK1List(int pageIndex,int pageSize,string sortName,string sortOrder,int? fundID,int? underlyingFundID) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<ScheduleK1ListModel> scheduleK1s=AdminRepository.GetAllScheduleK1s(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundID,underlyingFundID);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var schedule in scheduleK1s) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
							 schedule.PartnersShareFormID,
							 schedule.UnderlyingFundName,
							 schedule.PartnershipEIN,
							 schedule.FundName,
							 schedule.PartnerEIN
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/FindScheduleK1/1
		[OtherEntityAuthorize]
		[HttpGet]
		public JsonResult FindScheduleK1(int id) {
			ScheduleK1Model model=AdminRepository.FindScheduleK1Model(id);
			if(model==null) {
				model=new ScheduleK1Model();
			}
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CreateScheduleK1
		[HttpPost]
		public ActionResult CreateScheduleK1(FormCollection collection) {
			ScheduleK1Model model=new ScheduleK1Model();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo=null;
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				PartnersShareForm scheduleK1;
				if(model.PartnersShareFormID>0) {
					scheduleK1=AdminRepository.FindScheduleK1((model.PartnersShareFormID??0));
				} else {
					scheduleK1=new PartnersShareForm();
					scheduleK1.CreatedBy=Authentication.CurrentUser.UserID;
					scheduleK1.CreatedDate=DateTime.Now;
				}

				scheduleK1.LastUpdatedBy=Authentication.CurrentUser.UserID;
				scheduleK1.LastUpdatedDate=DateTime.Now;
				scheduleK1.EntityID=Authentication.CurrentEntity.EntityID;

				scheduleK1.AlternativeMinimumTax=model.AlternativeMinimumTax;
				scheduleK1.BeginingCapital=model.BeginingCapital;
				scheduleK1.BeginingLoss=model.BeginingLoss;
				scheduleK1.BeginingProfit=model.BeginingProfit;
				scheduleK1.BeginningCapitalAccount=model.BeginningCapitalAccount;
				scheduleK1.CapitalContributed=model.CapitalContributed;
				scheduleK1.Collectibles28GainLoss=model.Collectibles28GainLoss;
				scheduleK1.Credits=model.Credits;
				scheduleK1.CurrentYearIncrease=model.CurrentYearIncrease;
				scheduleK1.Distribution=model.Distribution;
				scheduleK1.EndingCapital=model.EndingCapital;
				scheduleK1.EndingCapitalAccount=model.EndingCapitalAccount;
				scheduleK1.EndingLoss=model.EndingLoss;
				scheduleK1.EndingProfit=model.EndingProfit;
				scheduleK1.ForeignTransaction=model.ForeignTransaction;
				scheduleK1.FundID=model.FundID;
				scheduleK1.GuaranteedPayment=model.GuaranteedPayment;
				scheduleK1.InterestIncome=model.InterestIncome;
				scheduleK1.IRSCenter=model.IRSCenter;
				scheduleK1.IsDomesticPartner=model.IsDomesticPartner;
				scheduleK1.IsForeignPartner=model.IsForeignPartner;
				scheduleK1.IsGAAP=model.IsGAAP;
				scheduleK1.IsGain=model.IsGain;
				scheduleK1.IsGeneralPartner=model.IsGeneralPartner;
				scheduleK1.IsLimitedPartner=model.IsLimitedPartner;
				scheduleK1.IsOther=model.IsOther;
				scheduleK1.IsPTP=model.IsPTP;
				scheduleK1.IsSection704=model.IsSection704;
				scheduleK1.IsTaxBasis=model.IsTaxBasis;
				scheduleK1.NetLongTermCapitalGainLoss=model.NetLongTermCapitalGainLoss;
				scheduleK1.NetRentalRealEstateIncome=model.NetRentalRealEstateIncome;
				scheduleK1.NetSection1231GainLoss=model.NetSection1231GainLoss;
				scheduleK1.NetShortTermCapitalGainLoss=model.NetShortTermCapitalGainLoss;
				scheduleK1.NonRecourse=model.NonRecourse;
				scheduleK1.OrdinaryBusinessIncome=model.OrdinaryBusinessIncome;
				scheduleK1.OrdinaryDividends=model.OrdinaryDividends;
				scheduleK1.OtherDeduction=model.OtherDeduction;
				scheduleK1.OtherIncomeLoss=model.OtherIncomeLoss;
				scheduleK1.OtherInformation=model.OtherInformation;
				scheduleK1.OtherNetRentalIncomeLoss=model.OtherNetRentalIncomeLoss;
				scheduleK1.PartnerEIN=model.PartnerEIN;
				scheduleK1.PartnershipEIN=model.PartnershipEIN;
				scheduleK1.PartnerType=model.PartnerType;
				scheduleK1.QualifiedDividends=model.QualifiedDividends;
				scheduleK1.QualifiedNonRecourseFinancing=model.QualifiedNonRecourseFinancing;
				scheduleK1.Recourse=model.Recourse;
				scheduleK1.Royalties=model.Royalties;
				scheduleK1.Section179Deduction=model.Section179Deduction;
				scheduleK1.SelfEmploymentEarningLoss=model.SelfEmploymentEarningLoss;
				scheduleK1.TaxExemptIncome=model.TaxExemptIncome;
				scheduleK1.UnderlyingFundID=model.UnderlyingFundID;
				scheduleK1.UnrecapturedSection1250Gain=model.UnrecapturedSection1250Gain;
				scheduleK1.WithdrawalsAndDistributions=model.WithdrawalsAndDistributions;

				Address partnerAddress=null;

				if(model.PartnerAddressID>0) {
					partnerAddress=AdminRepository.FindAddress((model.PartnerAddressID??0));
				}

				if(partnerAddress==null) {
					partnerAddress=new Address();
					partnerAddress.CreatedBy=Authentication.CurrentUser.UserID;
					partnerAddress.CreatedDate=DateTime.Now;
				}

				partnerAddress.LastUpdatedBy=Authentication.CurrentUser.UserID;
				partnerAddress.LastUpdatedDate=DateTime.Now;
				partnerAddress.EntityID=Authentication.CurrentEntity.EntityID;
				partnerAddress.Address1=model.PartnerAddress1;
				partnerAddress.Address2=model.PartnerAddress2;
				partnerAddress.AddressTypeID=(int)DeepBlue.Models.Admin.Enums.AddressType.Work;
				partnerAddress.City=model.PartnerCity;
				partnerAddress.Country=model.PartnerCountry;
				partnerAddress.State=model.PartnerState;
				partnerAddress.PostalCode=model.PartnerZip;
				errorInfo=AdminRepository.SaveAddress(partnerAddress);

				if(errorInfo==null) {
					scheduleK1.PartnerAddressID=partnerAddress.AddressID;
					errorInfo=AdminRepository.SaveScheduleK1(scheduleK1);
				}

				if(errorInfo!=null) {
					foreach(var err in errorInfo.ToList()) {
						resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
					}
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Admin/DeleteScheduleK1/1
		[OtherEntityAuthorize]
		[HttpGet]
		public string DeleteScheduleK1(int id) {
			if(AdminRepository.DeleteScheduleK1(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		//
		// GET: /Admin/ExportScheduleK1/1
		[OtherEntityAuthorize]
		[HttpGet]
		public ActionResult ExportScheduleK1Pdf(int id) {
			HtmlViewRenderer htmlViewRenderer=new HtmlViewRenderer();
			var model=AdminRepository.FindScheduleK1Model(id);
			// Render the view html to a string.
			string htmlText=htmlViewRenderer.RenderViewToString(this,"ExportScheduleK1",model);
			return new ExportToPdf("ScheduleK1.pdf",htmlText);
		}


		//
		// GET: /Admin/ExportScheduleK1/1
		[OtherEntityAuthorize]
		[HttpGet]
		public ActionResult ExportScheduleK1(int id) {
			return View(AdminRepository.FindScheduleK1Model(id));
		}

		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
