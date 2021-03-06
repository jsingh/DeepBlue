﻿using System;
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
using DeepBlue.Models.Admin;
using System.Configuration;
using System.Data;
using DeepBlue.Models.Excel;
using System.Reflection;

namespace DeepBlue.Controllers.Deal {

	[OtherEntityAuthorize]
	public class DealController:BaseController {

		#region Constants
		private const string EXCELDEALERROR_BY_KEY="Deal-Excel-{0}";
		private const string EXCELDEALEXPENSEERROR_BY_KEY="Deal-Expense-Excel-{0}";
		private const string EXCELDEALUFERROR_BY_KEY="Deal-UnderlyingFund-Excel-{0}";
		private const string EXCELDEALUDERROR_BY_KEY="Deal-UnderlyingDirect-Excel-{0}";

		private const string EXCELDEALCLOSEUFERROR_BY_KEY="DealClose-UnderlyingFund-Excel-{0}";
		private const string EXCELDEALCLOSEUDERROR_BY_KEY="DealClose-UnderlyingDirect-Excel-{0}";
		#endregion

		public IDealRepository DealRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public IFundRepository FundRepository { get; set; }

		public DealController()
			: this(new DealRepository(),new AdminRepository(),new CapitalCallRepository(),new FundRepository()) {
		}

		public DealController(IDealRepository dealRepository,IAdminRepository adminRepository,ICapitalCallRepository capitalCallRepository,IFundRepository fundRepository) {
			DealRepository=dealRepository;
			AdminRepository=adminRepository;
			CapitalCallRepository=capitalCallRepository;
			FundRepository=fundRepository;
		}

		#region Deal

		//
		// GET: /Deal/New
		public ActionResult New() {
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="CreateNewDeal";
			ViewData["PageName"]="CreateNewDeal";
			return View(GetNewDealModel(0));
		}

		//
		// GET: /Deal/Edit
		public ActionResult Edit(int? id) {
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="DealList";
			ViewData["PageName"]="DealList";
			return View("New",GetNewDealModel((id??0)));
		}

		//
		// GET: /Deal/List
		public ActionResult List(bool? isCloseDeal) {
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="DealList";
			ViewData["PageName"]="DealList";
			ViewData["CloseDeal"]=isCloseDeal;
			if((isCloseDeal??false)==true) {
				ViewData["MenuName"]="DealManagement";
				ViewData["SubmenuName"]="CloseDeal";
				ViewData["PageName"]="CloseDeal";
			}
			return View();
		}

		private CreateModel GetNewDealModel(int id) {
			CreateModel model=new CreateModel();
			model.Contacts=SelectListFactory.GetEmptySelectList();
			model.PurchaseTypes=SelectListFactory.GetPurchaseTypeSelectList(AdminRepository.GetAllPurchaseTypes());
			model.DealClosingCostTypes=SelectListFactory.GetDealClosingCostTypeSelectList(AdminRepository.GetAllDealClosingCostTypes());
			model.UnderlyingFunds=SelectListFactory.GetUnderlyingFundSelectList(DealRepository.GetAllUnderlyingFunds());
			model.Issuers=SelectListFactory.GetIssuerSelectList(DealRepository.GetAllIssuers());
			model.SecurityTypes=SelectListFactory.GetSecurityTypeSelectList(AdminRepository.GetAllSecurityTypes());
			model.Securities=SelectListFactory.GetEmptySelectList();
			model.DocumentTypes=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
			model.UploadTypes=SelectListFactory.GetUploadTypeSelectList();
			model.DocumentStatusTypes=SelectListFactory.GetDealDocumentStatusList();
			if(id<=0) {
				model.DealId=DealRepository.FindLastDealId();
				Models.Fund.FundDetail dealFundDetail=FundRepository.FindLastFundDetail();
				if(dealFundDetail!=null) {
					model.FundId=dealFundDetail.FundId;
					model.FundName=dealFundDetail.FundName;
				}
			} else {
				model.DealId=id;
			}
			List<Models.Entity.FileType> fileTypes=AdminRepository.GetAllFileTypes();
			string fileExtensions=string.Empty;
			foreach(var type in fileTypes) {
				var arrExtensions=type.FileExtension.Split((",").ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
				foreach(var ext in arrExtensions) {
					fileExtensions+=string.Format("\"{0}\",",ext);
				}
			}
			if(string.IsNullOrEmpty(fileExtensions)==false) {
				model.DocumentFileExtensions=string.Format("[{0}]",fileExtensions.Substring(0,fileExtensions.Length-1));
			}
			return model;
		}

		//
		// GET: /Deal/DealList
		[HttpGet]
		public ActionResult DealList(int pageIndex,int pageSize,string sortName,string sortOrder,bool? isNotClose,int? fundId,int? dealId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealListModel> deals=DealRepository.GetAllDeals(pageIndex,pageSize,sortName,sortOrder,ref totalRows,isNotClose,fundId,dealId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { deal.DealId,deal.DealName,deal.DealNumber }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealFundList
		[HttpGet]
		public ActionResult DealFundList(int pageIndex,int pageSize,string sortName,string sortOrder,bool? isNotClose,int? fundId,int? dealId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealListModel> deals=DealRepository.GetAllDeals(pageIndex,pageSize,sortName,sortOrder,ref totalRows,isNotClose,fundId,dealId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { deal.DealId,deal.DealName,deal.FundName }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealCloseList
		[HttpGet]
		public ActionResult DealCloseList(int pageIndex,int pageSize,string sortName,string sortOrder,int? fundID,int? dealID) {
			int totalRows=0;
			List<DealFundListModel> funds=DealRepository.GetAllCloseDeals(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundID,dealID);
			return Json(new {
				page=pageIndex, total=totalRows, rows=funds
			},JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			DealDetailModel model=new DealDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel=new ResultModel();
			string ErrorMessage=DealdNameAvailable(model.DealName,model.DealId,model.FundId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("DealName",ErrorMessage);
			}
			if(ModelState.IsValid) {

				// Attempt to create deal.

				Models.Entity.Deal deal=DealRepository.FindDeal(model.DealId);
				if(deal==null) {
					deal=new Models.Entity.Deal();
					deal.CreatedBy=Authentication.CurrentUser.UserID;
					deal.CreatedDate=DateTime.Now;
					deal.DealNumber=DealRepository.GetMaxDealNumber(model.FundId);
				}

				deal.EntityID=Authentication.CurrentEntity.EntityID;
				deal.LastUpdatedBy=Authentication.CurrentUser.UserID;
				deal.LastUpdatedDate=DateTime.Now;
				deal.DealName=model.DealName;
				deal.FundID=model.FundId;
				deal.IsPartnered=model.IsPartnered;
				deal.PurchaseTypeID=model.PurchaseTypeId;
				deal.ContactID=null;
				if((model.ContactId??0)>0)
					deal.ContactID=model.ContactId;

				if(deal.IsPartnered) {

					// Attempt to create deal partner.

					if(deal.Partner==null) {
						deal.Partner=new Partner();
						deal.Partner.CreatedBy=Authentication.CurrentUser.UserID;
						deal.Partner.CreatedDate=DateTime.Now;
					}
					deal.Partner.EntityID=Authentication.CurrentEntity.EntityID;
					deal.Partner.LastUpdatedBy=Authentication.CurrentUser.UserID;
					deal.Partner.LastUpdatedDate=DateTime.Now;
					deal.Partner.PartnerName=model.PartnerName;
				} else {
					deal.Partner=null;
				}

				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDeal(deal);
				if(errorInfo!=null) {
					foreach(var err in errorInfo.ToList()) {
						resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+deal.DealID;
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

		[HttpPost]
		public ActionResult ImportDeal(FormCollection collection) {
			DealDetailModel model=new DealDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel=new ResultModel();
			string ErrorMessage=DealdNameAvailable(model.DealName,model.DealId,model.FundId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("DealName",ErrorMessage);
			}
			if(ModelState.IsValid) {
				// Attempt to create deal.
				Models.Entity.Deal deal=DealRepository.FindDeal(model.DealId);
				if(deal==null) {
					deal=new Models.Entity.Deal();
					deal.CreatedBy=Authentication.CurrentUser.UserID;
					deal.CreatedDate=DateTime.Now;
				}
				deal.DealNumber=model.DealNumber;
				deal.EntityID=Authentication.CurrentEntity.EntityID;
				deal.LastUpdatedBy=Authentication.CurrentUser.UserID;
				deal.LastUpdatedDate=DateTime.Now;
				deal.DealName=model.DealName;
				deal.FundID=model.FundId;
				deal.IsPartnered=model.IsPartnered;
				deal.PurchaseTypeID=model.PurchaseTypeId;
				deal.ContactID=null;
				if((model.ContactId??0)>0)
					deal.ContactID=model.ContactId;

				if(deal.IsPartnered) {

					// Attempt to create deal partner.

					if(deal.Partner==null) {
						deal.Partner=new Partner();
						deal.Partner.CreatedBy=Authentication.CurrentUser.UserID;
						deal.Partner.CreatedDate=DateTime.Now;
					}
					deal.Partner.EntityID=Authentication.CurrentEntity.EntityID;
					deal.Partner.LastUpdatedBy=Authentication.CurrentUser.UserID;
					deal.Partner.LastUpdatedDate=DateTime.Now;
					deal.Partner.PartnerName=model.PartnerName;
				} else {
					deal.Partner=null;
				}

				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDeal(deal);
				if(errorInfo!=null) {
					foreach(var err in errorInfo.ToList()) {
						resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+deal.DealID;
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
		// GET: /Deal/DealdNameAvailable
		[HttpGet]
		public string DealdNameAvailable(string DealName,int DealId,int FundId) {
			if(DealRepository.DealNameAvailable(DealName,DealId,FundId))
				return "Deal Name already exist";
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

		//
		// GET: /Deal/Edit/5
		public JsonResult FindFund(int fundId) {
			DealDetailModel deal=new DealDetailModel();
			deal.DealNumber=DealRepository.GetMaxDealNumber(fundId);
			return Json(deal,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeal/5
		public JsonResult FindDeal(int dealId) {
			DealDetailModel dealDetail=DealRepository.FindDealDetail(dealId);
			foreach(var dealUnderlyingDirect in dealDetail.DealUnderlyingDirects) {
				dealUnderlyingDirect.Equities=SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(dealUnderlyingDirect.IssuerId));
				dealUnderlyingDirect.FixedIncomes=SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(dealUnderlyingDirect.IssuerId));
			}
			return Json(dealDetail,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDealCloseModel
		[HttpGet]
		public JsonResult FindDealCloseModel(int dealID) {
			Models.Entity.Deal deal=DealRepository.FindDeal(dealID);
			CreateDealCloseModel model=null;
			if(deal!=null) {
				model=new CreateDealCloseModel {
					DealId=deal.DealID, DealName=deal.DealName, DealNumber=deal.DealNumber
				};
			} else {
				model=new CreateDealCloseModel();
			}
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDeals
		[HttpGet]
		public JsonResult FindDeals(string term,int? fundId) {
			return Json(DealRepository.FindDeals(term,fundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/GetDealDetail
		[HttpGet]
		public JsonResult GetDealDetail(int id) {
			return Json(DealRepository.GetDealDetail(id),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/GetDealDetail
		[HttpGet]
		public string Delete(int id) {
			if(DealRepository.DeleteDeal(id)) {
				return "true";
			}
			return "false";
		}

		[HttpGet]
		public ActionResult CashDistributionAddTemplate() {
			UnderlyingFundCashDistributionModel model=new UnderlyingFundCashDistributionModel();
			model.CashDistributionTypes=SelectListFactory.GetCashDistributionTypeSelectList(AdminRepository.GetAllCashDistributionTypes());
			return View(model);
		}

		[HttpGet]
		public ActionResult UnderlyingFundPostRecordCashDistribution() {
			return View(new UnderlyingFundPostRecordCashDistributionModel());
		}

		[HttpGet]
		public ActionResult CapitalCallAddTemplate() {
			return View();
		}

		[HttpGet]
		public ActionResult UnderlyingFundPostRecordCapitalCall() {
			return View(new UnderlyingFundPostRecordCapitalCallModel());
		}

		[HttpGet]
		public ActionResult StockDistributionAddTemplate() {
			return View();
		}

		public ActionResult ManualUnderlyingFundStockDistribution() {
			return View();
		}

		public ActionResult UnderlyingFundValuation() {
			return View(new UnderlyingFundValuationModel());
		}
		#endregion

		#region DealExpense

		//
		// GET: /Deal/CreateDealExpense
		[HttpPost]
		public ActionResult CreateDealExpense(FormCollection collection) {
			DealClosingCostModel model=new DealClosingCostModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {

				// Attempt to create deal expense.

				DealClosingCost dealClosingCost=DealRepository.FindDealClosingCost(model.DealClosingCostId??0);
				if(dealClosingCost==null) {
					dealClosingCost=new DealClosingCost();
				}
				dealClosingCost.DealClosingCostTypeID=model.DealClosingCostTypeId;
				dealClosingCost.Amount=model.Amount;
				dealClosingCost.Date=model.Date;
				dealClosingCost.DealID=model.DealId;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealClosingCost(dealClosingCost);
				if(errorInfo!=null) {
					foreach(var err in errorInfo.ToList()) {
						resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
					}
				} else {
					resultModel.Result="True||"+dealClosingCost.DealClosingCostID;
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
		// GET: /Deal/FindDealClosingCost/1
		[HttpGet]
		public JsonResult FindDealClosingCost(int dealClosingCostId) {
			return Json(DealRepository.FindDealClosingCostModel(dealClosingCostId),JsonRequestBehavior.AllowGet);
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
			DealSellerDetailModel model=new DealSellerDetailModel();
			this.TryUpdateModel(model);
			if(model.SellerTypeId==0) {
				model.SellerTypeId=null;
			}
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {

				// Attempt to create deal seller information.

				Models.Entity.Deal deal=DealRepository.FindDeal(model.DealId);
				if(deal!=null) {
					deal.SellerTypeID=model.SellerTypeId;

					if(deal.Contact1==null) {
						deal.Contact1=new Contact();
						deal.Contact1.CreatedBy=Authentication.CurrentUser.UserID;
						deal.Contact1.CreatedDate=DateTime.Now;
					}

					deal.Contact1.EntityID=Authentication.CurrentEntity.EntityID;
					deal.Contact1.ContactName=model.ContactName;
					deal.Contact1.FirstName=model.SellerName;
					deal.Contact1.LastName="n/a";
					deal.Contact1.LastUpdatedBy=Authentication.CurrentUser.UserID;
					deal.Contact1.LastUpdatedDate=DateTime.Now;

					// Attempt to create communication values.

					AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.Email,model.Email);
					AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.Fax,model.Fax);
					AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.HomePhone,model.Phone);
					AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.Company,model.CompanyName);

					IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDeal(deal);
					if(errorInfo!=null) {
						foreach(var err in errorInfo.ToList()) {
							resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
						}
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

		#endregion

		#region DealDocument

		//
		// POST: /Document/CreateDealFundDocument
		[HttpPost]
		public string CreateDealFundDocument(FormCollection collection) {
			DealDocumentModel model=new DealDocumentModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			string documentUploadPath="DealDocumentUploadPath";
			int key=0;
			if((model.DocumentFundId??0)>0) {
				documentUploadPath="DealFundDocumentUploadPath";
				key=(model.DocumentFundId??0);
			}
			if(ModelState.IsValid) {
				DealFundDocument dealFundDocument=DealRepository.FindDealFundDocument(model.DealFundDocumentId??0);
				if(dealFundDocument==null) {
					dealFundDocument=new DealFundDocument();
					dealFundDocument.CreatedBy=Authentication.CurrentUser.UserID;
					dealFundDocument.CreatedDate=DateTime.Now;
				}
				dealFundDocument.LastUpdatedBy=Authentication.CurrentUser.UserID;
				dealFundDocument.LastUpdatedDate=DateTime.Now;
				dealFundDocument.EntityID=Authentication.CurrentEntity.EntityID;
				dealFundDocument.DocumentTypeID=model.DocumentTypeId;
				dealFundDocument.DocumentDate=DateTime.Now;
				if(model.DocumentFundId<=0) {
					model.DocumentFundId=null;
				}
				dealFundDocument.FundID=model.DocumentFundId;
				dealFundDocument.DealID=model.DealId;
				if(dealFundDocument.File==null) {
					dealFundDocument.File=new Models.Entity.File();
					dealFundDocument.File.CreatedBy=Authentication.CurrentUser.UserID;
					dealFundDocument.File.CreatedDate=DateTime.Now;
				}
				dealFundDocument.File.LastUpdatedBy=Authentication.CurrentUser.UserID;
				dealFundDocument.File.LastUpdatedDate=DateTime.Now;
				dealFundDocument.File.EntityID=Authentication.CurrentEntity.EntityID;

				DeepBlue.Models.Document.UploadType uploadType=(DeepBlue.Models.Document.UploadType)model.DocumentUploadTypeId;

				Models.File.UploadFileModel uploadFileModel=null;
				resultModel.Result+=CheckDocumentFile(uploadType,model.DocumentFilePath,Request.Files[0],ref uploadFileModel);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					if(uploadType==Models.Document.UploadType.Upload) {
						if(key>0)
							uploadFileModel=UploadFileHelper.Upload(Request.Files[0],documentUploadPath,Authentication.CurrentEntity.EntityID,dealFundDocument.DealID,key,dealFundDocument.DocumentTypeID,uploadFileModel.FileName);
						else
							uploadFileModel=UploadFileHelper.Upload(Request.Files[0],documentUploadPath,Authentication.CurrentEntity.EntityID,dealFundDocument.DealID,dealFundDocument.DocumentTypeID,uploadFileModel.FileName);

						dealFundDocument.File.Size=uploadFileModel.Size;
					}
					if(uploadFileModel!=null) {
						string ext=Path.GetExtension(uploadFileModel.FileName).ToLower();
						string fileTypeError=string.Empty;
						Models.Entity.FileType fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out fileTypeError);
						if(fileType==null) {
							resultModel.Result+=fileTypeError;
						} else {
							dealFundDocument.File.FileTypeID=fileType.FileTypeID;
							dealFundDocument.File.FileName=uploadFileModel.FileName;
							dealFundDocument.File.FilePath=uploadFileModel.FilePath;
						}
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)) {
					errorInfo=DealRepository.SaveDealFundDocument(dealFundDocument);
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
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
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		//
		// GET: /Document/DealFundDocumentList
		[HttpGet]
		public ActionResult DealFundDocumentList(int pageIndex,int pageSize,string sortName,string sortOrder,int dealId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealFundDocumentList> dealFundDocuments=DealRepository.GetAllDealFundDocuments(pageIndex,pageSize,sortName,sortOrder,ref totalRows,dealId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(dealFundDocuments);
		}

		//
		// GET: /Document/DeleteDealFundDocumentFile
		[HttpGet]
		public string DeleteDealFundDocumentFile(int id) {
			if(DealRepository.DeleteDealFundDocument(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region DealUnderlyingFund
		//
		// GET: /Deal/CreateDealUnderlyingFund
		[HttpPost]
		public ActionResult CreateDealUnderlyingFund(FormCollection collection) {
			DealUnderlyingFundModel model=new DealUnderlyingFundModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				DealUnderlyingFund dealUnderlyingFund=null;
				IEnumerable<ErrorInfo> errorInfo=SaveDealUnderlyingFund(model,out dealUnderlyingFund);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+dealUnderlyingFund.DealUnderlyingtFundID;
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


		public string FindDealID(string fundName,int dealNumber) {
			return DealRepository.FindDealID(fundName,dealNumber).ToString();
		}


		//
		// GET: /Deal/ImportUpdateDealClosingDealUnderlyingFund
		[HttpPost]
		public ActionResult ImportUpdateDealClosingDealUnderlyingFund(FormCollection collection) {
			DealUnderlyingFundModel model=new DealUnderlyingFundModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			DealUnderlyingFund dealUnderlyingFund=DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId??0);
			if(dealUnderlyingFund!=null) {
				dealUnderlyingFund.DealClosingID=model.DealClosingId;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
				if(errorInfo==null) {
					resultModel.Result="True||"+dealUnderlyingFund.DealUnderlyingtFundID;
				} else {
					resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Deal/ImportDealUnderlyingFund
		[HttpPost]
		public ActionResult ImportDealUnderlyingFund(FormCollection collection) {
			DealUnderlyingFundModel model=new DealUnderlyingFundModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			DealUnderlyingFund dealUnderlyingFund=DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId??0);
			if(dealUnderlyingFund==null) {
				dealUnderlyingFund=new DealUnderlyingFund();
			}
			dealUnderlyingFund.CommittedAmount=model.CommittedAmount;
			dealUnderlyingFund.DealClosingID=model.DealClosingId;
			dealUnderlyingFund.DealID=model.DealId;
			dealUnderlyingFund.Percent=model.Percent;
			dealUnderlyingFund.GrossPurchasePrice=model.GrossPurchasePrice;
			dealUnderlyingFund.PostRecordDateCapitalCall=model.PostRecordDateCapitalCall;
			dealUnderlyingFund.PostRecordDateDistribution=model.PostRecordDateDistribution;
			dealUnderlyingFund.ReassignedGPP=model.ReassignedGPP;
			dealUnderlyingFund.RecordDate=model.RecordDate;
			dealUnderlyingFund.UnderlyingFundID=model.UnderlyingFundId;
			dealUnderlyingFund.UnfundedAmount=model.UnfundedAmount;
			dealUnderlyingFund.EffectiveDate=model.EffectiveDate;

			dealUnderlyingFund.NetPurchasePrice=(dealUnderlyingFund.GrossPurchasePrice??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
			dealUnderlyingFund.AdjustedCost=(dealUnderlyingFund.ReassignedGPP??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);

			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);

			if(errorInfo==null) {
				if((model.FundNAV??0)>0) {
					// Check the user changes the fund navigation.
					UnderlyingFundNAV underlyingFundNAV=DealRepository.FindUnderlyingFundNAV(model.UnderlyingFundId,model.FundId,model.EffectiveDate);
					bool isCreateFundNAV=false;
					if(underlyingFundNAV==null)
						isCreateFundNAV=true;		// Create new underlying fund valuation.
					else if(underlyingFundNAV.FundNAV!=model.FundNAV)
						isCreateFundNAV=true; 	// If the user changes the fund navigation then update underlying fund valuation.

					if(isCreateFundNAV)
						CreateUnderlyingFundValuation(dealUnderlyingFund.UnderlyingFundID,model.FundId,(model.FundNAV??0),DateTime.Now,model.EffectiveDate,out errorInfo);
				}
				resultModel.Result="True||"+dealUnderlyingFund.DealUnderlyingtFundID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveDealUnderlyingFund(DealUnderlyingFundModel model,out DealUnderlyingFund dealUnderlyingFund) {
			// Attempt to create deal underlying fund.

			dealUnderlyingFund=DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId??0);
			if(dealUnderlyingFund==null) {
				dealUnderlyingFund=new DealUnderlyingFund();
			}

			dealUnderlyingFund.CommittedAmount=model.CommittedAmount;
			dealUnderlyingFund.DealClosingID=model.DealClosingId;
			dealUnderlyingFund.DealID=model.DealId;
			dealUnderlyingFund.Percent=model.Percent;
			dealUnderlyingFund.GrossPurchasePrice=model.GrossPurchasePrice;
			dealUnderlyingFund.PostRecordDateCapitalCall=model.PostRecordDateCapitalCall;
			dealUnderlyingFund.PostRecordDateDistribution=model.PostRecordDateDistribution;
			dealUnderlyingFund.ReassignedGPP=model.ReassignedGPP;
			dealUnderlyingFund.RecordDate=model.RecordDate;
			dealUnderlyingFund.UnderlyingFundID=model.UnderlyingFundId;
			dealUnderlyingFund.UnfundedAmount=model.UnfundedAmount;
			dealUnderlyingFund.EffectiveDate=model.EffectiveDate;

			dealUnderlyingFund.NetPurchasePrice=(dealUnderlyingFund.GrossPurchasePrice??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
			dealUnderlyingFund.AdjustedCost=(dealUnderlyingFund.ReassignedGPP??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);

			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);

			if(errorInfo==null) {
				if((model.FundNAV??0)>0) {
					// Check the user changes the fund navigation.
					UnderlyingFundNAV underlyingFundNAV=DealRepository.FindUnderlyingFundNAV(model.UnderlyingFundId,model.FundId,model.EffectiveDate);
					bool isCreateFundNAV=false;
					if(underlyingFundNAV==null)
						isCreateFundNAV=true;		// Create new underlying fund valuation.
					else if(underlyingFundNAV.FundNAV!=model.FundNAV)
						isCreateFundNAV=true; 	// If the user changes the fund navigation then update underlying fund valuation.

					if(isCreateFundNAV)
						CreateUnderlyingFundValuation(dealUnderlyingFund.UnderlyingFundID,model.FundId,(model.FundNAV??0),DateTime.Now,model.EffectiveDate,out errorInfo);
				}
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindDealUnderlyingFund
		[HttpGet]
		public JsonResult FindDealUnderlyingFund(int dealUnderlyingFundId) {
			return Json(DealRepository.FindDealUnderlyingFundModel(dealUnderlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteDealUnderlyingFund/1
		[HttpGet]
		public string DeleteDealUnderlyingFund(int id) {
			if(DealRepository.DeleteDealUnderlyingFund(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region DealUnderlyingDirect

		//
		// POST: /Deal/CreateDealUnderlyingDirect
		[HttpPost]
		public ActionResult CreateDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model=new DealUnderlyingDirectModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {

				DealUnderlyingDirect dealUnderlyingDirect=null;

				IEnumerable<ErrorInfo> errorInfo=SaveDealUnderlyingDirect(model,out dealUnderlyingDirect);

				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+dealUnderlyingDirect.DealUnderlyingDirectID;
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

		[HttpPost]
		public ActionResult ImportUpdateDealClosingDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model=new DealUnderlyingDirectModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			// Attempt to create deal underlying direct
			DealUnderlyingDirect dealUnderlyingDirect=DealRepository.FindDealUnderlyingDirect(model.DealUnderlyingDirectId??0);
			if(dealUnderlyingDirect!=null) {
				dealUnderlyingDirect.DealClosingID=model.DealClosingId;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
				if(errorInfo==null) {
					resultModel.Result="True||"+dealUnderlyingDirect.DealUnderlyingDirectID;
				} else {
					resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				}
			}
			return View("Result",resultModel);
		}

		[HttpPost]
		public ActionResult ImportDealUnderlyingDirect(FormCollection collection) {
			DealUnderlyingDirectModel model=new DealUnderlyingDirectModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			// Attempt to create deal underlying direct

			DealUnderlyingDirect dealUnderlyingDirect=DealRepository.FindDealUnderlyingDirect(model.DealUnderlyingDirectId??0);
			if(dealUnderlyingDirect==null) {
				dealUnderlyingDirect=new DealUnderlyingDirect();
			}

			dealUnderlyingDirect.AdjustedFMV=model.AdjustedFMV;
			dealUnderlyingDirect.DealClosingID=model.DealClosingId;
			dealUnderlyingDirect.DealID=model.DealId;
			dealUnderlyingDirect.FMV=model.FMV;
			dealUnderlyingDirect.NumberOfShares=model.NumberOfShares;
			dealUnderlyingDirect.Percent=model.Percent;
			dealUnderlyingDirect.PurchasePrice=model.PurchasePrice;
			dealUnderlyingDirect.RecordDate=model.RecordDate;
			dealUnderlyingDirect.SecurityID=model.SecurityId;
			dealUnderlyingDirect.SecurityTypeID=model.SecurityTypeId;
			dealUnderlyingDirect.TaxCostBase=model.TaxCostBase;
			dealUnderlyingDirect.TaxCostDate=model.TaxCostDate;

			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
			if(errorInfo==null) {
				if(model.PurchasePrice>0) {
					// Check the user changes the purchase price.
					UnderlyingDirectLastPrice underlyingDirectLastPrice=DealRepository.FindUnderlyingDirectLastPrice(model.FundId,model.SecurityId,model.SecurityTypeId);
					bool isCreateLastPrice=false;
					if(underlyingDirectLastPrice==null)
						isCreateLastPrice=true;
					else if(underlyingDirectLastPrice.LastPrice!=model.PurchasePrice)
						isCreateLastPrice=true;
					// If the user changes the purchase price then create underlying direct valuation.
					if(isCreateLastPrice)
						errorInfo=SaveUnderlyingDirectValuation(model.FundId,model.SecurityId,model.SecurityTypeId,model.PurchasePrice,DateTime.Now,out underlyingDirectLastPrice);
				}
				resultModel.Result="True||"+dealUnderlyingDirect.DealUnderlyingDirectID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveDealUnderlyingDirect(DealUnderlyingDirectModel model,out DealUnderlyingDirect dealUnderlyingDirect) {
			// Attempt to create deal underlying direct

			dealUnderlyingDirect=DealRepository.FindDealUnderlyingDirect(model.DealUnderlyingDirectId??0);
			if(dealUnderlyingDirect==null) {
				dealUnderlyingDirect=new DealUnderlyingDirect();
			}

			dealUnderlyingDirect.AdjustedFMV=model.AdjustedFMV;
			dealUnderlyingDirect.DealClosingID=model.DealClosingId;
			dealUnderlyingDirect.DealID=model.DealId;
			dealUnderlyingDirect.FMV=model.FMV;
			dealUnderlyingDirect.NumberOfShares=model.NumberOfShares;
			dealUnderlyingDirect.Percent=model.Percent;
			dealUnderlyingDirect.PurchasePrice=model.PurchasePrice;
			dealUnderlyingDirect.RecordDate=model.RecordDate;
			dealUnderlyingDirect.SecurityID=model.SecurityId;
			dealUnderlyingDirect.SecurityTypeID=model.SecurityTypeId;
			dealUnderlyingDirect.TaxCostBase=model.TaxCostBase;
			dealUnderlyingDirect.TaxCostDate=model.TaxCostDate;

			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
			if(errorInfo==null) {
				if(model.PurchasePrice>0) {
					// Check the user changes the purchase price.
					UnderlyingDirectLastPrice underlyingDirectLastPrice=DealRepository.FindUnderlyingDirectLastPrice(model.FundId,model.SecurityId,model.SecurityTypeId);
					bool isCreateLastPrice=false;
					if(underlyingDirectLastPrice==null)
						isCreateLastPrice=true;
					else if(underlyingDirectLastPrice.LastPrice!=model.PurchasePrice)
						isCreateLastPrice=true;
					// If the user changes the purchase price then create underlying direct valuation.
					if(isCreateLastPrice)
						errorInfo=SaveUnderlyingDirectValuation(model.FundId,model.SecurityId,model.SecurityTypeId,model.PurchasePrice,DateTime.Now,out underlyingDirectLastPrice);
				}
			}

			return errorInfo;
		}

		//
		// GET: /Deal/FindDealUnderlyingDirect/1
		[HttpGet]
		public JsonResult FindDealUnderlyingDirect(int dealUnderlyingDirectId) {
			DealUnderlyingDirectModel model=DealRepository.FindDealUnderlyingDirectModel(dealUnderlyingDirectId);
			model.Equities=SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(model.IssuerId));
			model.FixedIncomes=SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(model.IssuerId));
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindDealUnderlyingDirects
		[HttpGet]
		public JsonResult FindDealUnderlyingDirects(string term) {
			return Json(DealRepository.FindDealUnderlyingDirects(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteDealUnderlyingDirect/1
		[HttpGet]
		public string DeleteDealUnderlyingDirect(int id) {
			if(DealRepository.DeleteDealUnderlyingDirect(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region Security

		//
		// GET: /Deal/GetSecurity
		[HttpGet]
		public JsonResult GetSecurity(int issuerId,int securityTypeId) {
			List<SelectListItem> securityLists=null;
			switch((DeepBlue.Models.Deal.Enums.SecurityType)securityTypeId) {
			case Models.Deal.Enums.SecurityType.Equity:
				securityLists=SelectListFactory.GetEquitySelectList(DealRepository.GetAllEquity(issuerId));
				break;
			case Models.Deal.Enums.SecurityType.FixedIncome:
				securityLists=SelectListFactory.GetFixedIncomeSelectList(DealRepository.GetAllFixedIncome(issuerId));
				break;
			}
			return Json(securityLists,JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region DealClosing

		//
		// GET: /Deal/Close
		[HttpGet]
		public ActionResult Close(int? id) {
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="CloseDeal";
			ViewData["PageName"]="CloseDeal";
			CreateDealCloseModel model=new CreateDealCloseModel();
			if((id??0)>0) {
				model.DealId=(id??0);
			} else {
				model.DealId=DealRepository.FindLastDealId();
			}
			return View(model);
		}

		//
		// GET: /Deal/GetDealCloseDetails
		[HttpGet]
		public JsonResult GetDealCloseDetails(int id,int dealId) {
			return Json(DealRepository.FindDealClosingModel(id,dealId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/GetDealCloseAssetDetails
		[HttpGet]
		public JsonResult GetDealCloseAssetDetails(int dealId) {
			return Json(DealRepository.FindDealClosingModel(dealId),JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Deal/GetFianlDealCloseDetails
		[HttpGet]
		public JsonResult GetFianlDealCloseDetails(int dealId) {
			return Json(DealRepository.GetFinalDealClosingModel(dealId),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetFinalDealDetail(int dealId) {
			return Json(new {
				page=1, total=1, data=DealRepository.GetFinalDealDetail(dealId)
			},JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/CreateClosingDealUnderlyingFund
		[HttpPost]
		public ActionResult CreateClosingDealUnderlyingFund(FormCollection collection) {
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			DealUnderlyingFundModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;
			// Validate add new row.
			resultModel.Result=string.Empty;
			rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DealUnderlyingFundModel),"_");
			model=new DealUnderlyingFundModel();
			this.TryUpdateModel(model,rowCollection);
			model.RecordDate=DateTime.Now;
			model.DealClosingId=null;
			errorInfo=ValidationHelper.Validate(model);
			DealUnderlyingFund dealUnderlyingFund=null;
			if(errorInfo.Any()==false) {
				errorInfo=SaveDealUnderlyingFund(model,out dealUnderlyingFund);
			}
			if(errorInfo==null) {
				if(dealUnderlyingFund!=null) {
					resultModel.Result="True||"+dealUnderlyingFund.DealUnderlyingtFundID;
				}
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Deal/CreateClosingDealUnderlyingDirect
		[HttpPost]
		public ActionResult CreateClosingDealUnderlyingDirect(FormCollection collection) {
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			DealUnderlyingDirectModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;
			// Validate add new row.
			resultModel.Result=string.Empty;
			rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DealUnderlyingDirectModel),"$");
			model=new DealUnderlyingDirectModel();
			this.TryUpdateModel(model,rowCollection);
			model.RecordDate=DateTime.Now;
			model.DealClosingId=null;
			errorInfo=ValidationHelper.Validate(model);
			DealUnderlyingDirect dealUnderlyingDirect=null;
			if(errorInfo.Any()==false) {
				errorInfo=SaveDealUnderlyingDirect(model,out dealUnderlyingDirect);
			}
			if(errorInfo==null) {
				if(dealUnderlyingDirect!=null) {
					resultModel.Result="True||"+dealUnderlyingDirect.DealUnderlyingDirectID;
				}
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Deal/UpdateDealClosing
		[HttpPost]
		public ActionResult UpdateDealClosing(FormCollection collection) {
			CreateDealCloseModel model=new CreateDealCloseModel();
			this.TryUpdateModel(model);
			int totalUnderlyingFunds;
			int totalUnderlyingDirects;
			int.TryParse(collection["TotalUnderlyingFunds"],out totalUnderlyingFunds);
			int.TryParse(collection["TotalUnderlyingDirects"],out totalUnderlyingDirects);
			bool isNoErrors=false;
			ResultModel resultModel=new ResultModel();
			if(model.DealNumber==0) {
				ModelState.AddModelError("DealNumber","Deal Close Number is required");
			}
			if(ModelState.IsValid) {

				// Check error on deal underlying directs and underlying funds.
				IEnumerable<ErrorInfo> errorInfo=UpdateDealClosingFundsAndDirects(collection,totalUnderlyingFunds,totalUnderlyingDirects,0,false,model.CloseDate,true);
				if(errorInfo!=null)
					isNoErrors=!errorInfo.Any();
				else
					isNoErrors=true;

				if(isNoErrors) {

					// Attempt to create deal closing.

					DealClosing dealClosing=DealRepository.FindDealClosing(model.DealClosingId);

					if(dealClosing==null) {
						dealClosing=new DealClosing();
						dealClosing.DealNumber=DealRepository.GetMaxDealClosingNumber(model.DealId);
					}

					dealClosing.CloseDate=model.CloseDate;
					dealClosing.DealID=model.DealId;
					dealClosing.IsFinalClose=model.IsFinalClose;
					dealClosing.DiscountNAV=model.DiscountNAV;

					errorInfo=DealRepository.SaveDealClosing(dealClosing);

					if(errorInfo==null) {
						errorInfo=UpdateDealClosingFundsAndDirects(collection
																		,totalUnderlyingFunds
																		,totalUnderlyingDirects
																		,dealClosing.DealClosingID,false,model.CloseDate,false);
					}

					if(errorInfo==null) {
						resultModel.Result="True||"+dealClosing.DealClosingID;
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
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
		// GET: /Deal/ImportDealClosing
		[HttpPost]
		public ActionResult ImportDealClosing(FormCollection collection) {
			CreateDealCloseModel model=new CreateDealCloseModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			ResultModel resultModel=new ResultModel();
			if(model.DealNumber==0) {
				ModelState.AddModelError("DealNumber","Deal Close Number is required");
			}
			// Attempt to create deal closing.
			DealClosing dealClosing=DealRepository.FindDealClosing(model.DealClosingId);

			if(dealClosing==null) {
				dealClosing=new DealClosing();
				dealClosing.DealNumber=DealRepository.GetMaxDealClosingNumber(model.DealId);
			}

			dealClosing.CloseDate=model.CloseDate;
			dealClosing.DealID=model.DealId;
			dealClosing.IsFinalClose=model.IsFinalClose;

			errorInfo=DealRepository.SaveDealClosing(dealClosing);

			if(errorInfo==null) {
				resultModel.Result="True||"+dealClosing.DealClosingID;
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
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
		// GET: /Deal/UpdateFinalDealClosing
		[HttpPost]
		public ActionResult UpdateFinalDealClosing(FormCollection collection) {
			CreateDealCloseModel model=new CreateDealCloseModel();
			this.TryUpdateModel(model,collection);
			int totalUnderlyingFunds;
			int totalUnderlyingDirects;
			int.TryParse(collection["TotalUnderlyingFunds"],out totalUnderlyingFunds);
			int.TryParse(collection["TotalUnderlyingDirects"],out totalUnderlyingDirects);
			bool isNoErrors=false;
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=UpdateDealClosingFundsAndDirects(collection,
				totalUnderlyingFunds,
				totalUnderlyingDirects,0,true,model.CloseDate,true);
			if(errorInfo!=null)
				isNoErrors=!errorInfo.Any();
			else
				isNoErrors=true;

			if(isNoErrors) {
				UpdateDealClosingFundsAndDirects(collection,totalUnderlyingFunds,totalUnderlyingDirects,0,true,model.CloseDate,false);
			}
			resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> UpdateDealClosingFundsAndDirects(FormCollection collection,
			int totalUnderlyingFunds,
			int totalUnderlyingDirects,
			int dealClosingId,
			bool isFinalClose,
			DateTime finalCloseDate,
			bool isValidateOnly) {

			IEnumerable<ErrorInfo> errorInfo=null;

			int rowIndex=0;
			FormCollection rowCollection;
			DealUnderlyingFundModel dealUnderlyingFundModel=null;


			// Save each deal underlying fund rows.
			for(rowIndex=1;rowIndex<totalUnderlyingFunds+1;rowIndex++) {
				if(errorInfo!=null) {
					if(errorInfo.Any())
						break;
				}
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DealUnderlyingFundModel),"_");
				dealUnderlyingFundModel=new DealUnderlyingFundModel();
				this.TryUpdateModel(dealUnderlyingFundModel,rowCollection);
				if(dealUnderlyingFundModel.RecordDate==null) {
					dealUnderlyingFundModel.RecordDate=DateTime.Now;
				}
				errorInfo=ValidationHelper.Validate(dealUnderlyingFundModel);
				if(isValidateOnly==false) {
					if(errorInfo.Any()==false) {
						if(isFinalClose==false) {
							dealUnderlyingFundModel.DealClosingId=null;
							if(dealUnderlyingFundModel.IsClose)
								dealUnderlyingFundModel.DealClosingId=dealClosingId;

							errorInfo=null;
						} else {
							errorInfo=SaveFinalDealClosing(dealUnderlyingFundModel.DealClosingId??0,finalCloseDate);
						}
						if(errorInfo==null) {
							DealUnderlyingFund dealUnderlyingFund=null;
							errorInfo=SaveDealUnderlyingFund(dealUnderlyingFundModel,out dealUnderlyingFund);
							if(errorInfo!=null)
								break;
						}
					}
				}
			}

			if(errorInfo!=null) {
				if(errorInfo.Any()==false)
					errorInfo=null;
			}

			if(errorInfo==null) {
				rowIndex=0;
				rowCollection=null;
				DealUnderlyingDirectModel dealUnderlyingDirectModel=null;

				// Save each deal underlying direct rows.
				for(rowIndex=1;rowIndex<totalUnderlyingDirects+1;rowIndex++) {
					if(errorInfo!=null) {
						if(errorInfo.Any()) {
							break;
						}
					}
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DealUnderlyingDirectModel),"$");
					dealUnderlyingDirectModel=new DealUnderlyingDirectModel();
					this.TryUpdateModel(dealUnderlyingDirectModel,rowCollection);
					if(dealUnderlyingDirectModel.RecordDate==null) {
						dealUnderlyingDirectModel.RecordDate=DateTime.Now;
					}
					errorInfo=ValidationHelper.Validate(dealUnderlyingDirectModel);
					if(isValidateOnly==false) {
						if(errorInfo.Any()==false) {
							if(isFinalClose==false) {
								dealUnderlyingDirectModel.DealClosingId=null;
								if(dealUnderlyingDirectModel.IsClose)
									dealUnderlyingDirectModel.DealClosingId=dealClosingId;

								errorInfo=null;
							} else {
								errorInfo=SaveFinalDealClosing(dealUnderlyingDirectModel.DealClosingId??0,finalCloseDate);
							}
							DealUnderlyingDirect dealUnderlyingDirect=null;
							errorInfo=SaveDealUnderlyingDirect(dealUnderlyingDirectModel,out dealUnderlyingDirect);
							if(errorInfo!=null)
								break;
						}
					}
				}
			}
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveFinalDealClosing(int dealClosingId,DateTime finalCloseDate) {
			IEnumerable<ErrorInfo> errorInfo=null;
			DealClosing dealClosing=DealRepository.FindDealClosing(dealClosingId);
			if(dealClosing!=null) {
				dealClosing.IsFinalClose=true;
				dealClosing.CloseDate=finalCloseDate;
				errorInfo=DealRepository.SaveDealClosing(dealClosing);
			}
			return errorInfo;
		}

		//
		// GET: /Deal/DealCloseDateAvailable
		[HttpGet]
		public string DealCloseDateAvailable(DateTime dealCloseDate,int dealId,int dealCloseId) {
			if(DealRepository.DealCloseDateAvailable(dealCloseDate,dealId,dealCloseId))
				return "Close Date already exists.";
			else
				return string.Empty;
		}

		//
		// GET: /Deal/DealClosingList
		[HttpGet]
		public JsonResult DealClosingList(int pageIndex,int pageSize,string sortName,string sortOrder,int dealId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealCloseListModel> dealClosings=DealRepository.GetAllDealClosingLists(pageIndex,pageSize,sortName,sortOrder,ref totalRows,dealId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var dealClose in dealClosings) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { dealClose.DealClosingId,dealClose.DealNumber,dealClose.DealCloseName,dealClose.CloseDate.ToString("MM/dd/yyyy"),FormatHelper.CurrencyFormat(dealClose.TotalNetPurchasePrice) }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region DealReport

		//
		// GET: /Deal/Report
		[HttpGet]
		public ActionResult Report() {
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="DealReport";
			ViewData["PageName"]="DealReport";
			Models.Fund.FundDetail dealFundDetail=FundRepository.FindLastFundDetail();
			if(dealFundDetail==null)
				dealFundDetail=new Models.Fund.FundDetail();
			return View(dealFundDetail);
		}

		//
		// GET: /Deal/DealReportList
		[HttpGet]
		public JsonResult DealReportList(int pageIndex,int pageSize,string sortName,string sortOrder,int fundId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealReportModel> deals=DealRepository.GetAllReportDeals(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { 
						deal.DealId
						, deal.DealNumber
						, deal.DealName
						, deal.DealDate.ToString("MM/dd/yyyy")
						, FormatHelper.CurrencyFormat(deal.NetPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.GrossPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.CommittedAmount)
						, FormatHelper.NumberFormat(deal.NoOfShares)
						, FormatHelper.CurrencyFormat(deal.FMV)
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealUnderlyingDetails
		[HttpGet]
		public JsonResult DealUnderlyingDetails(int dealId) {
			DealUnderlyingDetail underlyingDetail=new DealUnderlyingDetail();
			underlyingDetail.DealUnderlyingFunds=DealRepository.GetAllDealUnderlyingFundDetails(dealId);
			underlyingDetail.DealUnderlyingDirects=DealRepository.GetAllDealUnderlyingDirects(dealId);
			return Json(underlyingDetail,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DealUnderlyingDetails
		[HttpGet]
		public ActionResult Export(int fundId,int exportTypeId,string sortName,string sortOrder) {
			int totalRows=0;
			DealExportModel model=new DealExportModel();
			model.Deals=DealRepository.GetAllReportDeals(1,200,sortName,sortOrder,ref totalRows,fundId);
			ViewData["ExportTypeId"]=exportTypeId;
			return View("ExportDetail",model);
		}

		//
		// GET: /Deal/ExportDetail
		[HttpGet]
		public ActionResult ExportDetail(FormCollection collection) {
			DealExportModel model=new DealExportModel();
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				model.Deals=DealRepository.GetAllExportDeals(model.SortName,model.SortOrder,model.FundId);
			}
			return View(model);
		}

		#endregion

		#region UnderlyingFund

		[HttpGet]
		public ActionResult UnderlyingFunds() {
			var collection=new FormCollection(Request.QueryString);
			ViewData["MenuName"]="AssetManagement";
			ViewData["SubmenuName"]="UnderlyingFundsLibrary";
			ViewData["PageName"]="UnderlyingFunds";
			ViewData["Mode"]=collection["mode"];
			CreateUnderlyingFundModel model=new CreateUnderlyingFundModel();
			model.UnderlyingFundTypes=SelectListFactory.GetUnderlyingFundTypeSelectList(AdminRepository.GetAllUnderlyingFundTypes());
			model.ReportingTypes=SelectListFactory.GetReportingTypeSelectList(AdminRepository.GetAllReportingTypes());
			model.Reportings=SelectListFactory.GetReportingFrequencySelectList(AdminRepository.GetAllReportingFrequencies());
			model.Industries=SelectListFactory.GetIndustrySelectList(AdminRepository.GetAllIndusties());
			model.Geographyes=SelectListFactory.GetGeographySelectList(AdminRepository.GetAllGeographies());
			model.FundStructures=SelectListFactory.GetEmptySelectList();
			model.InvestmentTypes=SelectListFactory.GetInvestmentTypeSelectList(AdminRepository.GetAllInvestmentTypes());
			model.ManagerContacts=SelectListFactory.GetEmptySelectList();
			model.FundRegisteredOffices=SelectListFactory.GetEmptySelectList();
			model.DocumentTypes=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
			model.UploadTypes=SelectListFactory.GetUploadTypeSelectList();
			List<Models.Entity.FileType> fileTypes=AdminRepository.GetAllFileTypes();
			string fileExtensions=string.Empty;
			foreach(var type in fileTypes) {
				var arrExtensions=type.FileExtension.Split((",").ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
				foreach(var ext in arrExtensions) {
					fileExtensions+=string.Format("\"{0}\",",ext);
				}
			}
			if(string.IsNullOrEmpty(fileExtensions)==false) {
				model.DocumentFileExtensions=string.Format("[{0}]",fileExtensions.Substring(0,fileExtensions.Length-1));
			}
			return View(model);
		}

		[HttpGet]
		public ActionResult FindUnderlyingFund(int underlyingFundId,int issuerId) {
			CreateUnderlyingFundModel model=DealRepository.FindUnderlyingFundModel(underlyingFundId,issuerId);
			if(model==null)
				model=new CreateUnderlyingFundModel();
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult UpdateUnderlyingFund(FormCollection collection) {
			CreateUnderlyingFundModel model=new CreateUnderlyingFundModel();
			UnderlyingFundAddressInformation addressInformationModel=null;
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			string ErrorMessage=UnderlyingFundNameAvailable(model.FundName,model.UnderlyingFundId);
			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("FundName",ErrorMessage);
			}
			if(string.IsNullOrEmpty(model.BankName)==false
			   ||string.IsNullOrEmpty(model.Account)==false
				||string.IsNullOrEmpty(model.AccountNumber)==false
				||string.IsNullOrEmpty(model.FFC)==false
				||string.IsNullOrEmpty(model.FFCNumber)==false
				||string.IsNullOrEmpty(model.Reference)==false
				||string.IsNullOrEmpty(model.Swift)==false
				||string.IsNullOrEmpty(model.IBAN)==false
				||string.IsNullOrEmpty(model.AccountPhone)==false
				||string.IsNullOrEmpty(model.AccountFax)==false
				||model.ABANumber>0

			) {

				if(string.IsNullOrEmpty(model.BankName)) {
					ModelState.AddModelError("BankName","Bank Name is required");
				}

				if(string.IsNullOrEmpty(model.Account)) {
					ModelState.AddModelError("Account","Account Name is required");
				}

				if(string.IsNullOrEmpty(model.AccountNumber)) {
					ModelState.AddModelError("AccountNumber","Account Number is required");
				}

			}
			if(model.UnderlyingFundId==0) {
				addressInformationModel=new UnderlyingFundAddressInformation();
				this.TryUpdateModel(addressInformationModel,collection);
				IEnumerable<ErrorInfo> errors=ValidationHelper.Validate(addressInformationModel);

			}
			if(ModelState.IsValid) {
				UnderlyingFund underlyingFund=DealRepository.FindUnderlyingFund(model.UnderlyingFundId);
				if(underlyingFund==null) {
					underlyingFund=new UnderlyingFund();
				}
				underlyingFund.EntityID=Authentication.CurrentEntity.EntityID;
				underlyingFund.IsFeesIncluded=model.IsFeesIncluded;
				underlyingFund.FundName=model.FundName;
				underlyingFund.FundTypeID=model.FundTypeId;
				underlyingFund.IssuerID=model.IssuerId;
				underlyingFund.VintageYear=model.VintageYear;
				underlyingFund.TotalSize=model.TotalSize;
				underlyingFund.TerminationYear=model.TerminationYear;
				underlyingFund.IncentiveFee=model.IncentiveFee;
				underlyingFund.LegalFundName=model.LegalFundName;
				underlyingFund.Description=model.Description;
				underlyingFund.FiscalYearEnd=model.FiscalYearEnd;
				underlyingFund.ManagementFee=model.ManagementFee;
				underlyingFund.Taxable=model.Taxable;
				underlyingFund.TaxRate=model.TaxRate;
				underlyingFund.AuditorName=model.AuditorName;
				underlyingFund.IsDomestic=model.IsDomestic;
				underlyingFund.Exempt=model.Exempt;
				underlyingFund.GeographyID=((model.GeographyId??0)>0?model.GeographyId:null);
				underlyingFund.IndustryID=((model.IndustryId??0)>0?model.IndustryId:null);
				underlyingFund.ReportingFrequencyID=((model.ReportingFrequencyId??0)>0?model.ReportingFrequencyId:null);
				underlyingFund.ReportingTypeID=((model.ReportingTypeId??0)>0?model.ReportingTypeId:null);
				underlyingFund.InvestmentTypeID=((model.InvestmentTypeId??0)>0?model.InvestmentTypeId:null);
				underlyingFund.FundRegisteredOfficeID=((model.FundRegisteredOfficeId??0)>0?model.FundRegisteredOfficeId:null);
				underlyingFund.FundStructureID=((model.FundStructureId??0)>0?model.FundStructureId:null);
				underlyingFund.ManagerContactID=((model.ManagerContactId??0)>0?model.ManagerContactId:null);
				underlyingFund.WebUserName=model.WebUserName;
				underlyingFund.WebPassword=model.WebPassword;
				underlyingFund.Website=model.Website;

				if(string.IsNullOrEmpty(model.BankName)==false
					&&string.IsNullOrEmpty(model.Account)==false) {
					if(underlyingFund.Account==null) {
						underlyingFund.Account=new Models.Entity.Account();
						underlyingFund.Account.CreatedBy=Authentication.CurrentUser.UserID;
						underlyingFund.Account.CreatedDate=DateTime.Now;
					}
					underlyingFund.Account.LastUpdatedBy=Authentication.CurrentUser.UserID;
					underlyingFund.Account.LastUpdatedDate=DateTime.Now;

					underlyingFund.Account.BankName=model.BankName;
					underlyingFund.Account.AccountNumberCash=model.AccountNumber;
					underlyingFund.Account.Account1=model.Account;
					underlyingFund.Account.AccountOf=model.AccountOf;
					underlyingFund.Account.Attention=model.Attention;
					underlyingFund.Account.EntityID=Authentication.CurrentEntity.EntityID;
					underlyingFund.Account.Reference=model.Reference;
					underlyingFund.Account.Routing=model.ABANumber;
					underlyingFund.Account.LastUpdatedBy=Authentication.CurrentUser.UserID;
					underlyingFund.Account.LastUpdatedDate=DateTime.Now;
					underlyingFund.Account.FFC=model.FFC;
					underlyingFund.Account.FFCNumber=model.FFCNumber;
					underlyingFund.Account.SWIFT=model.Swift;
					underlyingFund.Account.Phone=model.AccountPhone;
					underlyingFund.Account.IBAN=model.IBAN;
					underlyingFund.Account.Fax=model.AccountFax;
				}

				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveUnderlyingFund(underlyingFund);
				if(errorInfo!=null) {
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				} else {
					if(addressInformationModel!=null) {
						// Save underlying fund address
						Address address=null;
						addressInformationModel.UnderlyingFundId=underlyingFund.UnderlyingtFundID;
						errorInfo=SaveUnderlyingFundAddress(addressInformationModel,ref address);
						if(errorInfo==null) {
							underlyingFund.FundRegisteredOfficeID=address.AddressID;
							DealRepository.SaveUnderlyingFund(underlyingFund);
						}
					}
					resultModel.Result="True||"+underlyingFund.UnderlyingtFundID;
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

		private IEnumerable<ErrorInfo> SaveUnderlyingFundAddress(UnderlyingFundAddressInformation model,ref Address address) {

			address=DealRepository.FindUnderlyingFundAddress((model.UnderlyingFundId??0));

			if(address==null) {
				address=new Address();
				address.CreatedBy=Authentication.CurrentUser.UserID;
				address.CreatedDate=DateTime.Now;
			}

			address.EntityID=Authentication.CurrentEntity.EntityID;
			address.LastUpdatedBy=Authentication.CurrentUser.UserID;
			address.LastUpdatedDate=DateTime.Now;

			address.Address1=model.Address1;
			address.Address2=model.Address2;
			address.City=model.City;
			address.Country=model.Country;
			address.State=model.State;
			address.PostalCode=model.Zip;

			address.AddressTypeID=(int)DeepBlue.Models.Admin.Enums.AddressType.Work;

			return DealRepository.SaveUnderlyingFundAddress(address);
		}

		[HttpPost]
		public ActionResult CreateUnderlyingFundAddress(FormCollection collection) {
			UnderlyingFundAddressInformation model=new UnderlyingFundAddressInformation();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if((model.UnderlyingFundId??0)<=0) {
				ModelState.AddModelError("UnderlyingFundId","Underlying Fund is required");
			}
			if(ModelState.IsValid) {
				Address address=null;
				IEnumerable<ErrorInfo> errorInfo=SaveUnderlyingFundAddress(model,ref address);
				if(errorInfo==null) {
					UnderlyingFund underlyingFund=DealRepository.FindUnderlyingFund((model.UnderlyingFundId??0));
					underlyingFund.FundRegisteredOfficeID=address.AddressID;
					errorInfo=DealRepository.SaveUnderlyingFund(underlyingFund);
				}
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
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
		public string UnderlyingFundNameAvailable(string FundName,int UnderlyingFundId) {
			if(DealRepository.UnderlyingFundNameAvailable(FundName,UnderlyingFundId))
				return "Fund Name already exist";
			else
				return string.Empty;
		}

		[HttpGet]
		public JsonResult UnderlyingFundList(int pageIndex,int pageSize,string sortName,string sortOrder,int? gpId,int? underlyingFundID) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<UnderlyingFundListModel> underlyingFunds=DealRepository.GetAllUnderlyingFunds(pageIndex,pageSize,sortName,sortOrder,ref totalRows,gpId,underlyingFundID);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var underlyingFund in underlyingFunds) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { underlyingFund.UnderlyingFundId
						, underlyingFund.FundName
						, underlyingFund.FundType
						, underlyingFund.Industry
						, underlyingFund.IssuerID
						, underlyingFund.GP
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public string CreateUnderlyingFundDocument(FormCollection collection) {
			UnderlyingFundDocumentModel model=new UnderlyingFundDocumentModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			if(ModelState.IsValid) {
				UnderlyingFundDocument underlyingFundDocument=DealRepository.FindUnderlyingFundDocument(model.UnderlyingFundDocumetId);
				if(underlyingFundDocument==null) {
					underlyingFundDocument=new UnderlyingFundDocument();
					underlyingFundDocument.CreatedBy=Authentication.CurrentUser.UserID;
					underlyingFundDocument.CreatedDate=DateTime.Now;
				}
				underlyingFundDocument.LastUpdatedBy=Authentication.CurrentUser.UserID;
				underlyingFundDocument.LastUpdatedDate=DateTime.Now;
				underlyingFundDocument.EntityID=Authentication.CurrentEntity.EntityID;
				underlyingFundDocument.DocumentTypeID=model.DocumentTypeId;
				underlyingFundDocument.DocumentDate=model.DocumentDate;
				underlyingFundDocument.UnderlyingFundID=model.UnderlyingFundId;
				DeepBlue.Models.Document.UploadType uploadType=(DeepBlue.Models.Document.UploadType)model.UploadTypeId;
				DeepBlue.Models.File.UploadFileModel uploadFileModel=null;
				resultModel.Result+=CheckDocumentFile(uploadType,model.FilePath,Request.Files[0],ref uploadFileModel);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					underlyingFundDocument.File=new Models.Entity.File();
					underlyingFundDocument.File.CreatedBy=Authentication.CurrentUser.UserID;
					underlyingFundDocument.File.CreatedDate=DateTime.Now;
					underlyingFundDocument.File.LastUpdatedBy=Authentication.CurrentUser.UserID;
					underlyingFundDocument.File.LastUpdatedDate=DateTime.Now;
					underlyingFundDocument.File.EntityID=Authentication.CurrentEntity.EntityID;
					if(uploadType==Models.Document.UploadType.Upload) {
						uploadFileModel=UploadFileHelper.Upload(Request.Files[0],"UnderlyingFundDocumentUploadPath",Authentication.CurrentEntity.EntityID,underlyingFundDocument.UnderlyingFundID,underlyingFundDocument.DocumentTypeID,uploadFileModel.FileName);
						underlyingFundDocument.File.Size=uploadFileModel.Size;
					}
					if(uploadFileModel!=null) {
						string ext=Path.GetExtension(uploadFileModel.FileName).ToLower();
						string fileTypeError=string.Empty;
						Models.Entity.FileType fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out fileTypeError);
						if(fileType==null) {
							resultModel.Result+=fileTypeError;
						} else {
							underlyingFundDocument.File.FileTypeID=fileType.FileTypeID;
							underlyingFundDocument.File.FileName=uploadFileModel.FileName;
							underlyingFundDocument.File.FilePath=uploadFileModel.FilePath;
						}
					}
					errorInfo=DealRepository.SaveUnderlyingFundDocument(underlyingFundDocument);
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
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
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		[HttpGet]
		public string DeleteUnderlyingFundDocumentFile(int id) {
			if(DealRepository.DeleteUnderlyingFundDocument(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		[HttpGet]
		public ActionResult UnderlyingFundDocumentList(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundId) {
			int totalRows=0;
			List<UnderlyingFundDocumentList> underlyingFundDocuments=DealRepository.GetAllUnderlyingFundDocuments(underlyingFundId,pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(underlyingFundDocuments);
		}

		[HttpGet]
		public JsonResult FindUnderlyingFunds(string term) {
			return Json(DealRepository.FindUnderlyingFunds(term),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult FindReconcileUnderlyingFunds(string term,int? fundId) {
			return Json(DealRepository.FindReconcileUnderlyingFunds(term,fundId),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult UnderlyingFundContactList(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<UnderlyingFundContactList> underlyingFundContacts=DealRepository.GetAllUnderlyingFundContacts(underlyingFundId,pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var underlyingFundContact in underlyingFundContacts) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
					  underlyingFundContact.UnderlyingFundContactId,
					  underlyingFundContact.UnderlyingFundId,
					  underlyingFundContact.ContactId,
					  underlyingFundContact.ContactName,
					  underlyingFundContact.ContactTitle,
					  underlyingFundContact.ContactNotes,
					  underlyingFundContact.Email,
					  underlyingFundContact.Phone,
					  underlyingFundContact.WebAddress,
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		// GET: /Admin/EditUser
		[HttpGet]
		public ActionResult EditUnderlyingFundContact(int id) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			UnderlyingFundContact underlyingFundContact=DealRepository.FindUnderlyingFundContact(id);
			List<CommunicationDetailModel> communications=DealRepository.GetContactCommunications(underlyingFundContact.ContactID);

			flexgridData.total=totalRows;
			flexgridData.page=0;
			flexgridData.rows.Add(new FlexigridRow {
				cell=new List<object> {
					  underlyingFundContact.UnderlyingFundContactID,
					  underlyingFundContact.UnderlyingtFundID,
					  underlyingFundContact.ContactID,
					  underlyingFundContact.Contact.ContactName ,
					  underlyingFundContact.Contact.Title,
					  underlyingFundContact.Contact.Notes,
					  DealRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.Email),
					  DealRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.HomePhone),
					  DealRepository.GetCommunicationValue(communications, Models.Admin.Enums.CommunicationType.WebAddress),
				}
			});
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		// GET: /Deal/CreateUnderlyingFundContact
		[HttpPost]
		public string CreateUnderlyingFundContact(FormCollection collection) {
			UnderlyingFundContactModel model=new UnderlyingFundContactModel();
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				UnderlyingFundContact underlyingFundContact=DealRepository.FindUnderlyingFundContact(model.UnderlyingFundContactId);
				if(underlyingFundContact==null) {
					underlyingFundContact=new UnderlyingFundContact();
					underlyingFundContact.UnderlyingtFundID=model.UnderlyingFundId;
				}
				if(underlyingFundContact.Contact==null) {
					underlyingFundContact.Contact=new Contact();
					underlyingFundContact.Contact.CreatedBy=Authentication.CurrentUser.UserID;
					underlyingFundContact.Contact.CreatedDate=DateTime.Now;
				}
				underlyingFundContact.Contact.LastUpdatedBy=Authentication.CurrentUser.UserID;
				underlyingFundContact.Contact.LastUpdatedDate=DateTime.Now;
				underlyingFundContact.Contact.EntityID=Authentication.CurrentEntity.EntityID;
				underlyingFundContact.Contact.ContactName=model.ContactName;
				underlyingFundContact.Contact.LastName="n/a";
				underlyingFundContact.Contact.Title=model.ContactTitle;
				underlyingFundContact.Contact.Notes=model.ContactNotes;
				AddCommunication(underlyingFundContact.Contact,Models.Admin.Enums.CommunicationType.Email,model.Email);
				AddCommunication(underlyingFundContact.Contact,Models.Admin.Enums.CommunicationType.HomePhone,model.Phone);
				AddCommunication(underlyingFundContact.Contact,Models.Admin.Enums.CommunicationType.WebAddress,model.WebAddress);
				errorInfo=DealRepository.SaveUnderlyingFundContact(underlyingFundContact);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result=underlyingFundContact.UnderlyingFundContactID.ToString();
				} else {
					resultModel.Result="Error||"+resultModel.Result;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)==false) {
					resultModel.Result="Error||"+resultModel.Result;
				}
			}
			return resultModel.Result.ToString();
		}

		[HttpGet]
		public string DeleteUnderlyingFundContact(int id) {
			if(DealRepository.DeleteUnderlyingFundContact(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}


		#endregion

		#region Activities

		public ActionResult Activities() {
			ViewData["MenuName"]="ActivitiesManagement";
			ViewData["SubmenuName"]="Activities";
			ViewData["PageName"]="Activities";
			CreateActivityModel model=new CreateActivityModel();
			model.UnderlyingFundCashDistributionModel.CashDistributionTypes=SelectListFactory.GetCashDistributionTypeSelectList(AdminRepository.GetAllCashDistributionTypes());
			model.ActivityTypes=SelectListFactory.GetActivityTypeSelectList(AdminRepository.GetAllActivityTypes());
			List<Models.Entity.SecurityType> securityTypes=AdminRepository.GetAllSecurityTypes();
			model.EquitySplitModel.SecurityTypes=SelectListFactory.GetSecurityTypeSelectList(securityTypes);
			model.SecurityConversionModel.SecurityTypes=SelectListFactory.GetSecurityTypeSelectList(securityTypes);
			model.FundLevelExpenseModel.FundExpenseTypes=SelectListFactory.GetFundExpenseTypeSelectList(AdminRepository.GetAllFundExpenseTypes());
			return View(model);
		}

		#region View Activities

		public ActionResult ViewActivities() {
			return View();
		}

		//
		// GET: /Deal/ActivityDealsList
		[HttpGet]
		public JsonResult ActivityDealsList(int pageIndex,int pageSize,string sortName,string sortOrder,int fundId,int? dealID) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DealReportModel> deals=DealRepository.GetAllActivitiesDeals(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundId,dealID);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var deal in deals) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { 
						deal.DealId
						, deal.DealNumber
						, deal.DealName
						, deal.DealDate.ToString("MM/dd/yyyy")
						, FormatHelper.CurrencyFormat(deal.NetPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.GrossPurchasePrice)
						, FormatHelper.CurrencyFormat(deal.CommittedAmount)
						, FormatHelper.NumberFormat(deal.NoOfShares)
						, FormatHelper.CurrencyFormat(deal.FMV)
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundCapitalCalls(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundCapitalCalls(pageIndex,pageSize,sortName,sortOrder,ref totalRows,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundCashDistributions(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundCashDistributions(pageIndex,pageSize,sortName,sortOrder,ref totalRows,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundPostRecordCapitalCalls(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundPostRecordCapitalCalls(pageIndex,pageSize,sortName,sortOrder,ref totalRows,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundPostRecordCashDistributions(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundPostRecordCashDistributions(pageIndex,pageSize,sortName,sortOrder,ref totalRows,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundStockDistributions(int pageIndex,int pageSize,string sortName,string sortOrder,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundStockDistributions(pageIndex,pageSize,sortName,sortOrder,ref totalRows,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundAdjustments(int pageIndex,int pageSize,string sortName,string sortOrder,int dealUnderlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundAdjustments(pageIndex,pageSize,sortName,sortOrder,ref totalRows,dealUnderlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundValuations(int pageIndex,int pageSize,string sortName,string sortOrder,int dealUnderlyingFundID,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundValuations(pageIndex,pageSize,sortName,sortOrder,ref totalRows,dealUnderlyingFundID,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetUnderlyingFundValuationHistories(int pageIndex,int pageSize,string sortName,string sortOrder,int dealUnderlyingFundID,int underlyingFundID,int dealID) {
			int totalRows=0;
			var rows=DealRepository.GetUnderlyingFundValuationHistories(pageIndex,pageSize,sortName,sortOrder,ref totalRows,dealUnderlyingFundID,underlyingFundID,dealID);
			return Json(new {
				total=totalRows, rows=rows
			},JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region SecurityActivities

		//
		// GET: /Deal/FindEquityDirects
		[HttpGet]
		public JsonResult FindEquityDirects(string term) {
			return Json(DealRepository.FindEquityDirects(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeDirects
		[HttpGet]
		public JsonResult FindFixedIncomeDirects(string term) {
			return Json(DealRepository.FindFixedIncomeDirects(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindFixedIncomeSecurityConversionModel
		[HttpGet]
		public JsonResult FindFixedIncomeSecurityConversionModel(int fixedIncomeId) {
			return Json(DealRepository.FindFixedIncomeSecurityConversionModel(fixedIncomeId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquitySecurityConversionModel
		[HttpGet]
		public JsonResult FindEquitySecurityConversionModel(int equityId) {
			return Json(DealRepository.FindEquitySecurityConversionModel(equityId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateSplitActivity
		[HttpPost]
		public ActionResult CreateSplitActivity(FormCollection collection) {
			EquitySplitModel model=new EquitySplitModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			int securityTypeId=(int)Models.Deal.Enums.SecurityType.Equity;
			if(ModelState.IsValid) {
				EquitySplit equitySplit=DealRepository.FindEquitySplit(model.EquityId);
				if(equitySplit==null) {
					equitySplit=new EquitySplit();
					equitySplit.CreatedBy=Authentication.CurrentUser.UserID;
					equitySplit.CreatedDate=DateTime.Now;
				}
				equitySplit.LastUpdatedBy=Authentication.CurrentUser.UserID;
				equitySplit.LastUpdatedDate=DateTime.Now;
				equitySplit.EquityID=model.EquityId;
				equitySplit.SplitFactor=model.SplitFactor??0;
				equitySplit.SplitDate=model.SplitDate;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveEquitySplit(equitySplit);

				if(errorInfo==null) {

					// Get All Deal Underlying Directs
					List<DealUnderlyingDirect> dealUnderlyingDirects=DealRepository.GetAllDealUnderlyingDirects(securityTypeId,equitySplit.EquityID);

					foreach(var dealUnderlyingDirect in dealUnderlyingDirects) {

						// Update Deal Underlying Direct number of shares and fmv.

						dealUnderlyingDirect.NumberOfShares=dealUnderlyingDirect.NumberOfShares*equitySplit.SplitFactor;
						dealUnderlyingDirect.FMV=dealUnderlyingDirect.NumberOfShares*dealUnderlyingDirect.PurchasePrice;

						errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
						if(errorInfo!=null)
							break;
					}

					List<NewHoldingPatternModel> newHoldingPatterns=DealRepository.NewHoldingPatternList(model.ActivityTypeId,
						equitySplit.EquiteSplitID,
						securityTypeId,
						equitySplit.EquityID);

					foreach(var pattern in newHoldingPatterns) {
						errorInfo=CreateFundActivityHistory(pattern.FundId,
							pattern.OldNoOfShares,
							pattern.NewNoOfShares,
							equitySplit.EquiteSplitID,
							model.ActivityTypeId);
						if(errorInfo!=null)
							break;
					}
				}
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
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


		private IEnumerable<ErrorInfo> CreateFundActivityHistory(int fundId,int? oldNoOfShares,int? newNoOfShares,int activityId,int activityTypeId) {
			FundActivityHistory fundActivityHistory=new FundActivityHistory();
			fundActivityHistory.ActivityID=activityId;
			fundActivityHistory.ActivityTypeID=activityTypeId;
			fundActivityHistory.FundID=fundId;
			fundActivityHistory.OldNumberOfShares=oldNoOfShares??0;
			fundActivityHistory.NewNumberOfShares=newNoOfShares??0;
			return DealRepository.SaveFundActivityHistory(fundActivityHistory);
		}

		//
		// POST: /Deal/CreateConversionActivity
		[HttpPost]
		public ActionResult CreateConversionActivity(FormCollection collection) {
			SecurityConversionModel model=new SecurityConversionModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				SecurityConversion securityConversion=DealRepository.FindSecurityConversion(model.NewSecurityId,model.NewSecurityTypeId);
				if(securityConversion==null) {
					securityConversion=new SecurityConversion();
					securityConversion.CreatedBy=Authentication.CurrentUser.UserID;
					securityConversion.CreatedDate=DateTime.Now;
				}
				securityConversion.OldSecurityID=model.OldSecurityId;
				securityConversion.OldSecurityTypeID=model.OldSecurityTypeId;
				securityConversion.NewSecurityID=model.NewSecurityId;
				securityConversion.NewSecurityTypeID=model.NewSecurityTypeId;
				securityConversion.LastUpdatedBy=Authentication.CurrentUser.UserID;
				securityConversion.LastUpdatedDate=DateTime.Now;
				securityConversion.SplitFactor=model.SplitFactor??0;
				securityConversion.ConversionDate=model.ConversionDate;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveSecurityConversion(securityConversion);
				if(errorInfo==null) {

					// Get All Deal Underlying Directs

					List<DealUnderlyingDirect> dealUnderlyingDirects=DealRepository.GetAllDealUnderlyingDirects(securityConversion.OldSecurityTypeID,securityConversion.OldSecurityID);
					foreach(var dealUnderlyingDirect in dealUnderlyingDirects) {

						SecurityConversionDetail securityConversionDetail=new SecurityConversionDetail();
						securityConversionDetail.SecurityConversionID=securityConversion.SecurityConversionID;

						// Set old number of shares and fmv.
						securityConversionDetail.OldNumberOfShares=dealUnderlyingDirect.NumberOfShares;
						securityConversionDetail.OldFMV=dealUnderlyingDirect.FMV;

						// Update Deal Underlying Direct number of shares and fmv.
						dealUnderlyingDirect.NumberOfShares=securityConversionDetail.OldNumberOfShares*securityConversion.SplitFactor;
						dealUnderlyingDirect.FMV=dealUnderlyingDirect.NumberOfShares*dealUnderlyingDirect.PurchasePrice;
						dealUnderlyingDirect.SecurityTypeID=securityConversion.NewSecurityTypeID;
						dealUnderlyingDirect.SecurityID=securityConversion.NewSecurityID;

						// Set new number of shares and fmv.
						securityConversionDetail.NewNumberOfShares=dealUnderlyingDirect.NumberOfShares;
						securityConversionDetail.NewFMV=dealUnderlyingDirect.FMV;

						securityConversionDetail.DealUnderlyingDirectID=dealUnderlyingDirect.DealUnderlyingDirectID;

						errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
						if(errorInfo==null)
							errorInfo=DealRepository.SaveSecurityConversionDetail(securityConversionDetail);
						else
							break;
					}

					if(errorInfo==null) {
						List<NewHoldingPatternModel> newHoldingPatterns=DealRepository.NewHoldingPatternList(model.ActivityTypeId,securityConversion.SecurityConversionID,model.NewSecurityTypeId,model.NewSecurityId);
						foreach(var pattern in newHoldingPatterns) {
							errorInfo=CreateFundActivityHistory(pattern.FundId,pattern.OldNoOfShares,pattern.NewNoOfShares,securityConversion.SecurityConversionID,model.ActivityTypeId);
							if(errorInfo!=null)
								break;
						}
					}

				}
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+securityConversion.SecurityConversionID;
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


		#endregion

		#region UnderlyingFundCashDistribution

		//
		// POST : /Deal/CreateUnderlyingFundCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundCashDistribution(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundCashDistributionModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundCashDistributionModel),"_");
				model=new UnderlyingFundCashDistributionModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundCashDistributionModel),"_");
					model=new UnderlyingFundCashDistributionModel();
					this.TryUpdateModel(model,rowCollection);
					bool isManualCashDistribution=false;
					Boolean.TryParse(collection["IsManualCashDistribution"],out isManualCashDistribution);
					model.IsManualCashDistribution=isManualCashDistribution;
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingFundCashDistribution(model,collection);
					}
				}
			}
			return View("Result",resultModel);
		}

		#region Import Underlying Fund Cash Distribution
		[HttpPost]
		public ActionResult ImportUnderlyingFundCashDistribution(FormCollection collection) {
			UnderlyingFundCashDistribution underlyingFundCashDistribution=new UnderlyingFundCashDistribution();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(underlyingFundCashDistribution,collection);
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create underlying fund cash distribution.
			underlyingFundCashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCashDistribution.CreatedDate=DateTime.Now;
			underlyingFundCashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCashDistribution.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
			if(errorInfo==null) {
				resultModel.Result="True||"+underlyingFundCashDistribution.UnderlyingFundCashDistributionID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		public ActionResult ImportUnderlyingFundCashDistributionLineItem(FormCollection collection) {
			CashDistribution cashDistribution=new CashDistribution();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(cashDistribution,collection);
			IEnumerable<ErrorInfo> errorInfo=null;
			cashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
			cashDistribution.CreatedDate=DateTime.Now;
			cashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			cashDistribution.LastUpdatedDate=DateTime.Now;
			// Attempt to create underlying fund cash distribution.
			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
			if(errorInfo==null) {
				resultModel.Result="True||"+cashDistribution.CashDistributionID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}
		#endregion

		private IEnumerable<ErrorInfo> SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistributionModel model,FormCollection collection) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create underlying fund cash distribution.

			UnderlyingFundCashDistribution underlyingFundCashDistribution=null;
			if(model.UnderlyingFundCashDistributionId>0)
				underlyingFundCashDistribution=DealRepository.FindUnderlyingFundCashDistribution(model.UnderlyingFundCashDistributionId);
			if(underlyingFundCashDistribution==null) {
				underlyingFundCashDistribution=new UnderlyingFundCashDistribution();
				underlyingFundCashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingFundCashDistribution.CreatedDate=DateTime.Now;
			}
			underlyingFundCashDistribution.UnderlyingFundID=model.UnderlyingFundId;
			underlyingFundCashDistribution.Amount=model.Amount??0;
			underlyingFundCashDistribution.CashDistributionTypeID=model.CashDistributionTypeId??0;
			underlyingFundCashDistribution.IsPostRecordDateTransaction=false;
			underlyingFundCashDistribution.FundID=model.FundId;
			underlyingFundCashDistribution.NoticeDate=model.NoticeDate;
			underlyingFundCashDistribution.PaidDate=model.PaidDate;
			underlyingFundCashDistribution.ReceivedDate=DateTime.Now;
			underlyingFundCashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCashDistribution.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
			if(errorInfo==null) {

				// Attempt to create cash distribution to each deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllClosingDealUnderlyingFunds(underlyingFundCashDistribution.UnderlyingFundID,underlyingFundCashDistribution.FundID);
				CashDistribution cashDistribution;
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					cashDistribution=DealRepository.FindUnderlyingFundPostRecordCashDistribution(underlyingFundCashDistribution.UnderlyingFundCashDistributionID,
																			underlyingFundCashDistribution.UnderlyingFundID,
																			dealUnderlyingFund.DealID);
					if(cashDistribution==null) {
						cashDistribution=new CashDistribution();
						cashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
						cashDistribution.CreatedDate=DateTime.Now;
					}
					cashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
					cashDistribution.LastUpdatedDate=DateTime.Now;

					// Assign Underlying Fund Cash Distribution.
					cashDistribution.UnderluingFundCashDistributionID=underlyingFundCashDistribution.UnderlyingFundCashDistributionID;

					// Calculate distribution amount = (Deal Underlying Fund Committed Amount) / (Total Deal Underlying Fund Committed Amount) * Total Cash Distribution Amount.
					if(model.IsManualCashDistribution==true&&dealUnderlyingFunds.Count>1) {
						cashDistribution.Amount=DataTypeHelper.ToDecimal(collection[underlyingFundCashDistribution.FundID.ToString()+"_"+dealUnderlyingFund.DealID.ToString()+"_"+"CallAmount"]);
					} else {
						cashDistribution.Amount=((dealUnderlyingFund.CommittedAmount??0)/(dealUnderlyingFunds.Sum(fund => fund.CommittedAmount??0)))*underlyingFundCashDistribution.Amount;
					}

					cashDistribution.UnderlyingFundID=underlyingFundCashDistribution.UnderlyingFundID;
					cashDistribution.DealID=dealUnderlyingFund.DealID;
					errorInfo=DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
				}
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindUnderlyingFundCashDistribution
		[HttpGet]
		public JsonResult FindUnderlyingFundCashDistribution(int fundID,decimal amount,DateTime noticeDate,int underlyingFundID) {
			return Json(DealRepository.FindUnderlyingFundCashDistribution(fundID,amount,noticeDate,underlyingFundID),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/UnderlyingFundCashDistributionList
		[HttpGet]
		public JsonResult UnderlyingFundCashDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundCashDistributions(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingFundCashDistribution
		[HttpGet]
		public string DeleteUnderlyingFundCashDistribution(int id) {
			if(DealRepository.DeleteUnderlyingFundCashDistribution(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundPostRecordCashDistribution

		//
		// GET: /Deal/UnderlyingFundPostRecordCashDistributionList
		[HttpGet]
		public ActionResult UnderlyingFundPostRecordCashDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundPostRecordCashDistributions(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundPostRecordCashDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCashDistribution(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundPostRecordCashDistributionModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundPostRecordCashDistributionModel),"_");
				model=new UnderlyingFundPostRecordCashDistributionModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundPostRecordCashDistributionModel),"_");
					model=new UnderlyingFundPostRecordCashDistributionModel();
					this.TryUpdateModel(model,rowCollection);
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingFundPostRecordCashDistribution(model);
					}
				}
			}

			return View("Result",resultModel);
		}

		#region Import Underlying Fund Post Record Cash Distribution
		//
		// POST : /Deal/CreateUnderlyingFundPostRecordCashDistribution
		[HttpPost]
		public ActionResult ImportUnderlyingFundPostRecordCashDistribution(FormCollection collection) {
			UnderlyingFundPostRecordCashDistributionModel model=new UnderlyingFundPostRecordCashDistributionModel();
			this.TryUpdateModel(model,collection);
			IEnumerable<ErrorInfo> errorInfo=null;
			ResultModel resultModel=new ResultModel();

			CashDistribution cashDistribution=null;
			if(model.CashDistributionId>0)
				cashDistribution=DealRepository.FindUnderlyingFundPostRecordCashDistribution(model.CashDistributionId);
			if(cashDistribution==null) {
				cashDistribution=new CashDistribution();
				cashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				cashDistribution.CreatedDate=DateTime.Now;
			}
			cashDistribution.UnderlyingFundID=model.UnderlyingFundId;
			cashDistribution.Amount=model.Amount??0;
			cashDistribution.DealID=model.DealId;
			cashDistribution.DistributionDate=model.DistributionDate;
			cashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			cashDistribution.LastUpdatedDate=DateTime.Now;

			//Assign null value in Underlying Fund Cash Distribution.
			cashDistribution.UnderluingFundCashDistributionID=null;

			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
			if(errorInfo==null) {
				//resultModel.Result = "True||" + cashDistribution.CashDistributionID;
				// Update post record date capital call amount to deal underlying fund and reduce unfunded amount.
				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllNotClosingDealUnderlyingFunds(cashDistribution.UnderlyingFundID,cashDistribution.DealID);
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					if(dealUnderlyingFund.DealClosingID==null&&dealUnderlyingFund.DealID==cashDistribution.DealID) {
						dealUnderlyingFund.NetPurchasePrice=(dealUnderlyingFund.GrossPurchasePrice??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
						dealUnderlyingFund.AdjustedCost=(dealUnderlyingFund.ReassignedGPP??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
						errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if(errorInfo!=null)
							break;
					}
				}
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}

			return View("Result",resultModel);
		}
		#endregion

		private IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCashDistribution(UnderlyingFundPostRecordCashDistributionModel model) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create post record cash distribution.

			CashDistribution cashDistribution=null;
			if(model.CashDistributionId>0)
				cashDistribution=DealRepository.FindUnderlyingFundPostRecordCashDistribution(model.CashDistributionId);
			if(cashDistribution==null) {
				cashDistribution=new CashDistribution();
				cashDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				cashDistribution.CreatedDate=DateTime.Now;
			}
			cashDistribution.UnderlyingFundID=model.UnderlyingFundId;
			cashDistribution.Amount=model.Amount??0;
			cashDistribution.DealID=model.DealId;
			cashDistribution.DistributionDate=model.DistributionDate;
			cashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			cashDistribution.LastUpdatedDate=DateTime.Now;

			//Assign null value in Underlying Fund Cash Distribution.
			cashDistribution.UnderluingFundCashDistributionID=null;

			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
			if(errorInfo==null) {
				// Update post record date distribution amount to deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllNotClosingDealUnderlyingFunds(cashDistribution.UnderlyingFundID,cashDistribution.DealID);
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					if(dealUnderlyingFund.DealClosingID==null) {
						dealUnderlyingFund.PostRecordDateDistribution=DealRepository.GetSumOfCashDistribution(cashDistribution.UnderlyingFundID,cashDistribution.DealID);
						errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if(errorInfo!=null)
							break;
					}
				}
			}
			return errorInfo;
		}

		[HttpGet]
		public object FindUnderlyingFundPostRecordCashDistribution(int underlyingFundId,int dealId,decimal amount,DateTime distributionDate) {
			return Json(DealRepository.FindUnderlyingFundPostRecordCashDistribution(underlyingFundId,dealId,amount,distributionDate),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingFundPostRecordCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundPostRecordCashDistribution(int id) {
			if(DealRepository.DeleteUnderlyingFundPostRecordCashDistribution(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundCapitalCall

		//
		// POST : /Deal/CreateUnderlyingFundCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundCapitalCall(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundCapitalCallModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;
			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundCapitalCallModel),"_");
				model=new UnderlyingFundCapitalCallModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}
			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundCapitalCallModel),"_");
					model=new UnderlyingFundCapitalCallModel();
					this.TryUpdateModel(model,rowCollection);
					bool isManualCapitalCall=false;
					Boolean.TryParse(collection["IsManualCapitalCall"],out isManualCapitalCall);
					model.IsManualCapitalCall=isManualCapitalCall;
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingFundCapitalCall(model,collection);
						if(errorInfo!=null)
							resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return View("Result",resultModel);
		}


		//
		// POST : /Deal/ImportUnderlyingFundCapitalCall
		[HttpPost]
		public ActionResult ImportUnderlyingFundCapitalCall(FormCollection collection) {
			ResultModel resultModel=new ResultModel();
			UnderlyingFundCapitalCall underlyingFundCapitalCall=new UnderlyingFundCapitalCall();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(underlyingFundCapitalCall,collection);
			underlyingFundCapitalCall.CreatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCapitalCall.CreatedDate=DateTime.Now;
			underlyingFundCapitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCapitalCall.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);
			if(errorInfo==null) {
				resultModel.Result="True||"+underlyingFundCapitalCall.UnderlyingFundCapitalCallID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		//
		// POST : /Deal/ImportUnderlyingFundCapitalCallLineItem
		[HttpPost]
		public ActionResult ImportUnderlyingFundCapitalCallLineItem(FormCollection collection) {
			ResultModel resultModel=new ResultModel();
			UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem=new UnderlyingFundCapitalCallLineItem();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(underlyingFundCapitalCallLineItem,collection);

			underlyingFundCapitalCallLineItem.CreatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCapitalCallLineItem.CreatedDate=DateTime.Now;
			underlyingFundCapitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCapitalCallLineItem.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCallLineItem);
			if(errorInfo==null) {
				resultModel.Result="True||"+underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCallID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		//
		// GET: /Deal/FindUnderlyingFundCapitalCall
		[HttpGet]
		public JsonResult FindUnderlyingFundCapitalCall(int fundID,decimal amount,DateTime noticeDate,DateTime dueDate,int underlyingFundID) {
			return Json(DealRepository.FindUnderlyingFundCapitalCall(fundID,amount,noticeDate,dueDate,underlyingFundID),JsonRequestBehavior.AllowGet);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundCapitalCall(UnderlyingFundCapitalCallModel model,FormCollection collection) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create underlying fund capital call.
			UnderlyingFundCapitalCall underlyingFundCapitalCall=null;
			if(model.UnderlyingFundCapitalCallId>0)
				underlyingFundCapitalCall=DealRepository.FindUnderlyingFundCapitalCall(model.UnderlyingFundCapitalCallId);
			if(underlyingFundCapitalCall==null) {
				underlyingFundCapitalCall=new UnderlyingFundCapitalCall();
				underlyingFundCapitalCall.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingFundCapitalCall.CreatedDate=DateTime.Now;
			}
			underlyingFundCapitalCall.UnderlyingFundID=model.UnderlyingFundId;
			underlyingFundCapitalCall.Amount=model.Amount??0;
			underlyingFundCapitalCall.IsPostRecordDateTransaction=model.IsPostRecordDateTransaction??false;
			underlyingFundCapitalCall.IsDeemedCapitalCall=model.IsDeemedCapitalCall??false;
			underlyingFundCapitalCall.FundID=model.FundId;
			underlyingFundCapitalCall.NoticeDate=model.NoticeDate;
			underlyingFundCapitalCall.DueDate=model.NoticeDate;
			underlyingFundCapitalCall.ReceivedDate=DateTime.Now;
			underlyingFundCapitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundCapitalCall.LastUpdatedDate=DateTime.Now;
			underlyingFundCapitalCall.ManagementFee=model.ManagementFee;

			errorInfo=DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);

			if(errorInfo==null) {

				// Attempt to create capital call line item to each deal underlying fund.

				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllClosingDealUnderlyingFunds(underlyingFundCapitalCall.UnderlyingFundID,underlyingFundCapitalCall.FundID);
				UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem;
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					underlyingFundCapitalCallLineItem=DealRepository.FindUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCall.UnderlyingFundCapitalCallID,
																			underlyingFundCapitalCall.UnderlyingFundID,
																			dealUnderlyingFund.DealID);
					if(underlyingFundCapitalCallLineItem==null) {
						underlyingFundCapitalCallLineItem=new UnderlyingFundCapitalCallLineItem();
						underlyingFundCapitalCallLineItem.CreatedBy=Authentication.CurrentUser.UserID;
						underlyingFundCapitalCallLineItem.CreatedDate=DateTime.Now;
					}
					underlyingFundCapitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
					underlyingFundCapitalCallLineItem.LastUpdatedDate=DateTime.Now;

					// Assign underlying fund capital call.
					underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCallID=underlyingFundCapitalCall.UnderlyingFundCapitalCallID;

					// Calculate capital call amount = (Deal Underlying Fund Committed Amount) / (Total Deal Underlying Fund Committed Amount) * Total Capital Call Amount.
					if(model.IsManualCapitalCall&&dealUnderlyingFunds.Count>1) {
						underlyingFundCapitalCallLineItem.Amount=DataTypeHelper.ToDecimal(collection[underlyingFundCapitalCall.FundID.ToString()+"_"+dealUnderlyingFund.DealID.ToString()+"_"+"CallAmount"]);
					} else {
						underlyingFundCapitalCallLineItem.Amount=((dealUnderlyingFund.CommittedAmount??0)/(dealUnderlyingFunds.Sum(fund => fund.CommittedAmount??0)))*underlyingFundCapitalCall.Amount;
					}
					underlyingFundCapitalCallLineItem.ManagementFee=((dealUnderlyingFund.CommittedAmount??0)/(dealUnderlyingFunds.Sum(fund => fund.CommittedAmount??0)))*(underlyingFundCapitalCall.ManagementFee??0);
					underlyingFundCapitalCallLineItem.ReceivedDate=underlyingFundCapitalCall.ReceivedDate;
					underlyingFundCapitalCallLineItem.UnderlyingFundID=underlyingFundCapitalCall.UnderlyingFundID;
					underlyingFundCapitalCallLineItem.DealID=dealUnderlyingFund.DealID;
					underlyingFundCapitalCallLineItem.DueDate=underlyingFundCapitalCall.DueDate;
					underlyingFundCapitalCallLineItem.NoticeDate=underlyingFundCapitalCall.NoticeDate;
					// Update capital call amount to deal underlying fund and reduce unfunded amount.

					dealUnderlyingFund.UnfundedAmount=dealUnderlyingFund.UnfundedAmount-underlyingFundCapitalCallLineItem.Amount;

					errorInfo=DealRepository.SaveUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCallLineItem);

					if(errorInfo==null)
						errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);

					if(errorInfo!=null)
						break;
				}

			}
			return errorInfo;
		}

		//
		// GET: /Deal/UnderlyingFundCapitalCallList
		[HttpGet]
		public JsonResult UnderlyingFundCapitalCallList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundCapitalCalls(underlyingFundId),JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Deal/DeleteUnderlyingFundCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundCapitalCall(int id) {
			if(DealRepository.DeleteUnderlyingFundCapitalCall(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundPostRecordCapitalCall
		//
		// GET: /Deal/UnderlyingFundPostRecordCapitalCallList
		[HttpGet]
		public JsonResult UnderlyingFundPostRecordCapitalCallList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundPostRecordCapitalCalls(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundPostRecordCapitalCall
		[HttpPost]
		public ActionResult CreateUnderlyingFundPostRecordCapitalCall(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundPostRecordCapitalCallModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundPostRecordCapitalCallModel),"_");
				model=new UnderlyingFundPostRecordCapitalCallModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundPostRecordCapitalCallModel),"_");
					model=new UnderlyingFundPostRecordCapitalCallModel();
					this.TryUpdateModel(model,rowCollection);
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingFundPostRecordCapitalCall(model);
					}
				}
			}
			return View("Result",resultModel);
		}

		[HttpPost]
		public ActionResult ImportUnderlyingFundPostRecordCapitalCall(FormCollection collection) {
			UnderlyingFundPostRecordCapitalCallModel model=new UnderlyingFundPostRecordCapitalCallModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model,collection);
			IEnumerable<ErrorInfo> errorInfo=null;
			UnderlyingFundCapitalCallLineItem capitalCallLineItem=null;
			if(model.UnderlyingFundCapitalCallLineItemId>0) {
				capitalCallLineItem=DealRepository.FindUnderlyingFundPostRecordCapitalCall(model.UnderlyingFundCapitalCallLineItemId);
			}
			if(capitalCallLineItem==null) {
				capitalCallLineItem=new UnderlyingFundCapitalCallLineItem();
				capitalCallLineItem.CreatedBy=Authentication.CurrentUser.UserID;
				capitalCallLineItem.CreatedDate=DateTime.Now;
			}
			capitalCallLineItem.UnderlyingFundID=model.UnderlyingFundId;
			capitalCallLineItem.Amount=model.Amount??0;
			capitalCallLineItem.CapitalCallDate=model.CapitalCallDate;
			capitalCallLineItem.DealID=model.DealId;
			capitalCallLineItem.ReceivedDate=DateTime.Now;
			capitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
			capitalCallLineItem.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCapitalCall(capitalCallLineItem);
			if(errorInfo==null) {
				// Update post record date capital call amount to deal underlying fund and reduce unfunded amount.
				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllNotClosingDealUnderlyingFunds(capitalCallLineItem.UnderlyingFundID,capitalCallLineItem.DealID);
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					if(dealUnderlyingFund.DealClosingID==null&&dealUnderlyingFund.DealID==capitalCallLineItem.DealID) {
						dealUnderlyingFund.NetPurchasePrice=(dealUnderlyingFund.GrossPurchasePrice??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
						dealUnderlyingFund.AdjustedCost=(dealUnderlyingFund.ReassignedGPP??0)+(dealUnderlyingFund.PostRecordDateCapitalCall??0)-(dealUnderlyingFund.PostRecordDateDistribution??0);
						errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if(errorInfo!=null)
							break;
					}
				}

			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundPostRecordCapitalCall(UnderlyingFundPostRecordCapitalCallModel model) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create post record capital call.

			UnderlyingFundCapitalCallLineItem capitalCallLineItem=null;
			if(model.UnderlyingFundCapitalCallLineItemId>0) {
				capitalCallLineItem=DealRepository.FindUnderlyingFundPostRecordCapitalCall(model.UnderlyingFundCapitalCallLineItemId);
			}
			if(capitalCallLineItem==null) {
				capitalCallLineItem=new UnderlyingFundCapitalCallLineItem();
				capitalCallLineItem.CreatedBy=Authentication.CurrentUser.UserID;
				capitalCallLineItem.CreatedDate=DateTime.Now;
			}
			capitalCallLineItem.UnderlyingFundID=model.UnderlyingFundId;
			capitalCallLineItem.Amount=model.Amount??0;
			capitalCallLineItem.CapitalCallDate=model.CapitalCallDate;
			capitalCallLineItem.DealID=model.DealId;
			capitalCallLineItem.ReceivedDate=DateTime.Now;
			capitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
			capitalCallLineItem.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundPostRecordCapitalCall(capitalCallLineItem);
			if(errorInfo==null) {

				// Update post record date capital call amount to deal underlying fund and reduce unfunded amount.
				List<DealUnderlyingFund> dealUnderlyingFunds=DealRepository.GetAllNotClosingDealUnderlyingFunds(capitalCallLineItem.UnderlyingFundID,capitalCallLineItem.DealID);
				foreach(var dealUnderlyingFund in dealUnderlyingFunds) {
					if(dealUnderlyingFund.DealClosingID==null&&dealUnderlyingFund.DealID==capitalCallLineItem.DealID) {
						dealUnderlyingFund.PostRecordDateCapitalCall=DealRepository.GetSumOfUnderlyingFundCapitalCallLineItem(dealUnderlyingFund.UnderlyingFundID,dealUnderlyingFund.DealID);
						dealUnderlyingFund.UnfundedAmount=dealUnderlyingFund.UnfundedAmount-capitalCallLineItem.Amount;
						errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
						if(errorInfo!=null)
							break;
					}
				}

			}
			return errorInfo;
		}

		[HttpGet]
		public JsonResult FindUnderlyingFundPostRecordCapitalCall(int underlyingFundId,int dealId,decimal amount,DateTime capitalCallDate) {
			return Json(DealRepository.FindUnderlyingFundPostRecordCapitalCall(underlyingFundId,dealId,amount,capitalCallDate),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingFundPostRecordCapitalCall
		[HttpGet]
		public string DeleteUnderlyingFundPostRecordCapitalCall(int id) {
			if(DealRepository.DeleteUnderlyingFundPostRecordCapitalCall(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region UnderlyingFundStockDistribution

		//
		// POST : /Deal/CreateUnderlyingFundStockDistribution
		[HttpPost]
		public ActionResult CreateUnderlyingFundStockDistribution(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			UnderlyingFundStockDistributionModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundStockDistributionModel),"_");
				model=new UnderlyingFundStockDistributionModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.SecurityId>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(UnderlyingFundStockDistributionModel),"_");
					model=new UnderlyingFundStockDistributionModel();
					this.TryUpdateModel(model,rowCollection);
					bool isManualStockDistribution=false;
					Boolean.TryParse(collection["IsManualStockDistribution"],out isManualStockDistribution);
					model.IsManualStockDistribution=isManualStockDistribution;
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingFundStockDistribution(model,collection);
						if(errorInfo!=null)
							resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingFundStockDistribution(UnderlyingFundStockDistributionModel model,FormCollection collection) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create underlying fund cash distribution.

			UnderlyingFundStockDistribution underlyingFundStockDistribution=null;
			if(model.UnderlyingFundStockDistributionId>0)
				underlyingFundStockDistribution=DealRepository.FindUnderlyingFundStockDistribution(model.UnderlyingFundStockDistributionId);
			if(underlyingFundStockDistribution==null) {
				underlyingFundStockDistribution=new UnderlyingFundStockDistribution();
				underlyingFundStockDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingFundStockDistribution.CreatedDate=DateTime.Now;
			}

			underlyingFundStockDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundStockDistribution.LastUpdatedDate=DateTime.Now;

			underlyingFundStockDistribution.UnderlyingFundID=model.UnderlyingFundId;
			underlyingFundStockDistribution.FundID=model.FundId;
			underlyingFundStockDistribution.DistributionDate=model.DistributionDate;
			underlyingFundStockDistribution.NoticeDate=model.NoticeDate;

			underlyingFundStockDistribution.NumberOfShares=model.NumberOfShares;
			underlyingFundStockDistribution.PurchasePrice=model.PurchasePrice;

			underlyingFundStockDistribution.FMV=decimal.Multiply((decimal)(underlyingFundStockDistribution.NumberOfShares??0)
																   ,underlyingFundStockDistribution.PurchasePrice);

			underlyingFundStockDistribution.SecurityID=model.SecurityId;
			underlyingFundStockDistribution.SecurityTypeID=model.SecurityTypeId;
			underlyingFundStockDistribution.TaxCostBase=model.TaxCostBase;
			underlyingFundStockDistribution.TaxCostDate=model.TaxCostDate;
			if((model.BrokerID??0)>0) {
				underlyingFundStockDistribution.BrokerID=model.BrokerID;
			}
			underlyingFundStockDistribution.Notes=model.Notes;

			errorInfo=DealRepository.SaveUnderlyingFundStockDistribution(underlyingFundStockDistribution);
			if(errorInfo==null) {

				// Attempt to create stock distribution to each deal.
				List<StockDistributionLineItemModel> deals=DealRepository.GetAllStockDistributionDeals(underlyingFundStockDistribution.FundID,
																						underlyingFundStockDistribution.UnderlyingFundID);

				decimal? totalDealCommitment=deals.Sum(d => d.CommitmentAmount);
				foreach(var deal in deals) {
					UnderlyingFundStockDistributionLineItem stockDistributionItem=DealRepository.FindUnderlyingFundStockDistributionLineItem(
						underlyingFundStockDistribution.UnderlyingFundStockDistributionID
						,underlyingFundStockDistribution.UnderlyingFundID
						,deal.DealId);
					if(stockDistributionItem==null) {
						stockDistributionItem=new UnderlyingFundStockDistributionLineItem();
						stockDistributionItem.CreatedBy=Authentication.CurrentUser.UserID;
						stockDistributionItem.CreatedDate=DateTime.Now;
					}
					stockDistributionItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
					stockDistributionItem.LastUpdatedDate=DateTime.Now;
					stockDistributionItem.DealID=deal.DealId;
					stockDistributionItem.UnderlyingFundID=underlyingFundStockDistribution.UnderlyingFundID;

					if(model.IsManualStockDistribution) {
						stockDistributionItem.NumberOfShares=DataTypeHelper.ToDecimal(collection[underlyingFundStockDistribution.FundID.ToString()+"_"+deal.DealId.ToString()+"_"+"NumberOfShares"]);
						stockDistributionItem.FMV=DataTypeHelper.ToDecimal(collection[underlyingFundStockDistribution.FundID.ToString()+"_"+deal.DealId.ToString()+"_"+"FMV"]);
					} else {
						stockDistributionItem.NumberOfShares=decimal.Multiply(
															   decimal.Divide((decimal)(underlyingFundStockDistribution.NumberOfShares??0),(totalDealCommitment??0))
															   ,(deal.CommitmentAmount??0));
						stockDistributionItem.FMV=decimal.Multiply((stockDistributionItem.NumberOfShares??0),underlyingFundStockDistribution.PurchasePrice);
					}

					stockDistributionItem.UnderlyingFundStockDistributionID=underlyingFundStockDistribution.UnderlyingFundStockDistributionID;
					errorInfo=DealRepository.SaveUnderlyingFundStockDistributionLineItem(stockDistributionItem);
					if(errorInfo!=null)
						break;
				}

			}
			return errorInfo;
		}

		//
		// GET: /Deal/UnderlyingFundStockDistributionList
		[HttpGet]
		public JsonResult UnderlyingFundStockDistributionList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundStockDistributions(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindStockDistributionIssuers
		[HttpGet]
		public JsonResult FindStockDistributionIssuers(string term,int underlyingFundId) {
			return Json(DealRepository.FindStockIssuers(term,underlyingFundId),JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Deal/FindStockIssuers
		[HttpGet]
		public JsonResult FindStockIssuers(int underlyingFundId,int fundId,int issuerId,string term) {
			return Json(DealRepository.FindStockIssuers(underlyingFundId,fundId,issuerId,term),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/ImportUnderlyingStockDistribution
		[HttpPost]
		public ActionResult ImportUnderlyingStockDistribution(FormCollection collection) {
			ResultModel resultModel=new ResultModel();
			UnderlyingFundStockDistribution underlyingFundStockDistribution=new UnderlyingFundStockDistribution();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(underlyingFundStockDistribution,collection);
			underlyingFundStockDistribution.CreatedBy=Authentication.CurrentUser.UserID;
			underlyingFundStockDistribution.CreatedDate=DateTime.Now;
			underlyingFundStockDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundStockDistribution.LastUpdatedDate=DateTime.Now;

			string brokerName=collection["BrokerName"];
			if(string.IsNullOrEmpty(brokerName)==false) {
				Broker broker=AdminRepository.FindBroker(brokerName);
				if(broker==null) {
					broker=new Broker {
						BrokerName=brokerName,
						EntityID=Authentication.CurrentEntity.EntityID,
						CreatedDate=DateTime.Now,
						CreatedBy=Authentication.CurrentUser.UserID,
						LastUpdatedDate=DateTime.Now,
						LastUpdatedBy=Authentication.CurrentUser.UserID,
					};
					AdminRepository.SaveBroker(broker);
				}
				underlyingFundStockDistribution.BrokerID=broker.BrokerID;
			}

			if((underlyingFundStockDistribution.DistributionDate??Convert.ToDateTime("01/01/1900")).Year<=1900)
				underlyingFundStockDistribution.DistributionDate=null;

			if((underlyingFundStockDistribution.NoticeDate??Convert.ToDateTime("01/01/1900")).Year<=1900)
				underlyingFundStockDistribution.NoticeDate=null;

			if((underlyingFundStockDistribution.TaxCostDate??Convert.ToDateTime("01/01/1900")).Year<=1900)
				underlyingFundStockDistribution.TaxCostDate=null;

			errorInfo=DealRepository.SaveUnderlyingFundStockDistribution(underlyingFundStockDistribution);
			if(errorInfo==null) {
				resultModel.Result="True||"+underlyingFundStockDistribution.UnderlyingFundStockDistributionID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		//
		// POST : /Deal/ImportUnderlyingFundStockDistributionLineItem
		[HttpPost]
		public ActionResult ImportUnderlyingFundStockDistributionLineItem(FormCollection collection) {
			ResultModel resultModel=new ResultModel();
			UnderlyingFundStockDistributionLineItem underlyingFundStockDistributionLineItem=new UnderlyingFundStockDistributionLineItem();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(underlyingFundStockDistributionLineItem,collection);
			underlyingFundStockDistributionLineItem.CreatedBy=Authentication.CurrentUser.UserID;
			underlyingFundStockDistributionLineItem.CreatedDate=DateTime.Now;
			underlyingFundStockDistributionLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundStockDistributionLineItem.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundStockDistributionLineItem(underlyingFundStockDistributionLineItem);
			if(errorInfo==null) {
				resultModel.Result="True||"+underlyingFundStockDistributionLineItem.UnderlyingFundStockDistributionLineItemID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}


		#endregion

		#region UnderlyingFundValuation

		//
		// GET: /Deal/UnderlyingFundValuationList
		[HttpGet]
		public JsonResult UnderlyingFundValuationList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnderlyingFundValuations(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindUnderlyingFundValuation
		[HttpGet]
		public JsonResult FindUnderlyingFundValuation(int underlyingFundId,int underlyingFundNAVId) {
			UnderlyingFundValuationModel model=DealRepository.FindUnderlyingFundValuationModel(underlyingFundId,underlyingFundNAVId);
			if(model==null) {
				model=new UnderlyingFundValuationModel();
			}
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingFundValuation
		[HttpPost]
		public ActionResult CreateUnderlyingFundValuation(FormCollection collection) {
			UnderlyingFundValuationModel model=new UnderlyingFundValuationModel();
			this.TryUpdateModel(model);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo;
				UnderlyingFundNAV underlyingFundNAV=CreateUnderlyingFundValuation(model.UnderlyingFundId,model.FundId,(model.UpdateNAV??0),model.UpdateDate,model.EffectiveDate,out errorInfo);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+underlyingFundNAV.UnderlyingFundNAVID+"||"+underlyingFundNAV.FundID;
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

		private UnderlyingFundNAV CreateUnderlyingFundValuation(int underlyingFundId,
																int fundId,
																decimal fundNAV,
																DateTime fundNAVDate,
																DateTime? effectiveDate,
																out IEnumerable<ErrorInfo> errorInfo) {

			// Attempt to create deal underlying fund valuation.

			UnderlyingFundNAV underlyingFundNAV=DealRepository.FindUnderlyingFundNAV(underlyingFundId,fundId,effectiveDate);
			decimal existingFundNAV=0;
			DateTime existingFundNAVDate=Convert.ToDateTime("01/01/1900");
			//bool isExisting = false;
			if(underlyingFundNAV==null) {
				underlyingFundNAV=new UnderlyingFundNAV();
				underlyingFundNAV.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingFundNAV.CreatedDate=DateTime.Now;
			} else {
				existingFundNAV=underlyingFundNAV.FundNAV??0;
				existingFundNAVDate=underlyingFundNAV.FundNAVDate;
				//isExisting = true;
			}
			underlyingFundNAV.UnderlyingFundID=underlyingFundId;
			underlyingFundNAV.FundID=fundId;
			underlyingFundNAV.FundNAV=fundNAV;
			underlyingFundNAV.FundNAVDate=fundNAVDate;
			underlyingFundNAV.EffectiveDate=effectiveDate;
			underlyingFundNAV.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingFundNAV.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingFundNAV(underlyingFundNAV);
			if(errorInfo==null) {

				// Attempt to create underlying fund navigation history.

				UnderlyingFundNAVHistory underlyingFundNAVHistory=new UnderlyingFundNAVHistory();
				underlyingFundNAVHistory.UnderlyingFundNAVID=underlyingFundNAV.UnderlyingFundNAVID;
				underlyingFundNAVHistory.FundNAV=underlyingFundNAV.FundNAV;
				underlyingFundNAVHistory.FundNAVDate=underlyingFundNAV.FundNAVDate;
				underlyingFundNAVHistory.Calculation=null;
				underlyingFundNAVHistory.IsAudited=false;
				underlyingFundNAVHistory.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingFundNAVHistory.CreatedDate=DateTime.Now;
				underlyingFundNAVHistory.LastUpdatedBy=Authentication.CurrentUser.UserID;
				underlyingFundNAVHistory.LastUpdatedDate=DateTime.Now;

				if(existingFundNAV==underlyingFundNAV.FundNAV
					&&existingFundNAVDate==underlyingFundNAV.FundNAVDate) {
					underlyingFundNAVHistory.Calculation=underlyingFundNAV.FundNAV+":"+DealRepository.SumOfTotalCapitalCalls(underlyingFundNAV.UnderlyingFundID,underlyingFundNAV.FundID)+":"+DealRepository.SumOfTotalDistributions(underlyingFundNAV.UnderlyingFundID,underlyingFundNAV.FundID);
				}
				errorInfo=DealRepository.SaveUnderlyingFundNAVHistory(underlyingFundNAVHistory);
			}
			return underlyingFundNAV;
		}

		//
		// GET: /Deal/DeleteUnderlyingFundValuation
		[HttpGet]
		public string DeleteUnderlyingFundValuation(int id) {
			if(DealRepository.DeleteUnderlyingFundValuation(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		//
		// GET: /Deal/FindFundNAV
		[HttpGet]
		public decimal FindFundNAV(int underlyingFundId,int fundId) {
			return DealRepository.FindFundNAV(underlyingFundId,fundId);
		}

		#endregion

		#region FundLevelExpense

		//
		// POST : /Deal/FundExpenseList
		[HttpGet]
		public JsonResult FundExpenseList(int fundId) {
			return Json(DealRepository.GetAllFundExpenses(fundId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/FundExpense
		[HttpPost]
		public ActionResult CreateFundExpense(FormCollection collection) {
			FundExpenseModel model=new FundExpenseModel();
			this.TryUpdateModel(model);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {

				// Attempt to create fund expense.

				FundExpense fundExpense=DealRepository.FindFundExpense(model.FundExpenseId);
				if(fundExpense==null) {
					fundExpense=new FundExpense();
					fundExpense.CreatedBy=Authentication.CurrentUser.UserID;
					fundExpense.CreatedDate=DateTime.Now;
				}
				fundExpense.FundID=model.FundId;
				fundExpense.FundExpenseTypeID=model.FundExpenseTypeId;
				fundExpense.Amount=model.Amount;
				fundExpense.Date=model.Date;
				fundExpense.LastUpdatedBy=Authentication.CurrentUser.UserID;
				fundExpense.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveFundExpense(fundExpense);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+fundExpense.FundExpenseID;
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
		// POST : /Deal/FindFundExpense
		[HttpGet]
		public JsonResult FindFundExpense(int fundExpenseId) {
			return Json(DealRepository.FindFundExpenseModel(fundExpenseId),JsonRequestBehavior.AllowGet);
		}


		//
		// POST: /Deal/ReconcileList
		[HttpPost]
		public JsonResult ReconcileFundExpenseList(FormCollection collection) {
			ReconcileSearchModel model=new ReconcileSearchModel();
			this.TryUpdateModel(model,collection);
			string error=string.Empty;
			DateTime startDate=(model.StartDate??Convert.ToDateTime("01/01/1900"));
			DateTime endDate=(model.EndDate??DateTime.MaxValue);
			object allFundExpenses=null;
			int totalRows=0;
			if(ModelState.IsValid) {
				allFundExpenses=DealRepository.GetAllFundExpenses(model.FundId,startDate,endDate,model.PageIndex,model.PageSize,ref totalRows);
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							error+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return Json(new {
				Error=error, Results=allFundExpenses, page=model.PageIndex, total=totalRows
			},JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region NewHoldingPattern
		//
		// GET: /Deal/NewHoldingPatternList
		public ActionResult NewHoldingPatternList(int activityTypeId,int activityId,int securityTypeId,int securityId) {
			List<NewHoldingPatternModel> newHoldingPatterns=DealRepository.NewHoldingPatternList(activityTypeId,activityId,securityTypeId,securityId);
			FlexigridData flexgridData=new FlexigridData();
			flexgridData.total=newHoldingPatterns.Count();
			flexgridData.page=1;
			foreach(var pattern in newHoldingPatterns) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { pattern.FundName,(pattern.OldNoOfShares>0?FormatHelper.NumberFormat(pattern.OldNoOfShares):string.Empty),(pattern.NewNoOfShares>0?FormatHelper.NumberFormat(pattern.NewNoOfShares):string.Empty) }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
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
			return Json(DealRepository.UnderlyingDirectValuationList(issuerId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnderlyingDirectValuation
		[HttpPost]
		public ActionResult CreateUnderlyingDirectValuation(FormCollection collection) {
			FormCollection rowCollection;
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int index=0;
			string[] values;
			string value;
			ResultModel resultModel=new ResultModel();
			for(index=0;index<totalRows;index++) {
				rowCollection=new FormCollection();
				foreach(string key in collection.Keys) {
					values=collection[key].Split((",").ToCharArray());
					if(values.Length>index)
						value=values[index];
					else
						value=string.Empty;
					rowCollection.Add(key,value);
				}
				SaveUnderlyingDirectValuation(rowCollection);
			}
			return View("Result",resultModel);
		}

		private ResultModel SaveUnderlyingDirectValuation(FormCollection collection) {
			UnderlyingDirectValuationModel model=new UnderlyingDirectValuationModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo;
				UnderlyingDirectLastPrice underlyingDirectLastPrice;
				errorInfo=SaveUnderlyingDirectValuation(model.FundId,model.SecurityId,model.SecurityTypeId,(model.NewPrice??0),model.NewPriceDate,out underlyingDirectLastPrice);
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+underlyingDirectLastPrice.UnderlyingDirectLastPriceID;
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
			return resultModel;
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingDirectValuation(int fundId,int securityId,int securityTypeId,decimal newPrice,DateTime newPriceDate,
			out UnderlyingDirectLastPrice underlyingDirectLastPrice) {
			// Attempt to create underlying direct valuation.
			underlyingDirectLastPrice=DealRepository.FindUnderlyingDirectLastPrice(fundId,securityId,securityTypeId);
			IEnumerable<ErrorInfo> errorInfo;
			if(underlyingDirectLastPrice==null) {
				underlyingDirectLastPrice=new UnderlyingDirectLastPrice();
				underlyingDirectLastPrice.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingDirectLastPrice.CreatedDate=DateTime.Now;
			}
			underlyingDirectLastPrice.FundID=fundId;
			underlyingDirectLastPrice.SecurityID=securityId;
			underlyingDirectLastPrice.SecurityTypeID=securityTypeId;
			underlyingDirectLastPrice.LastPrice=newPrice;
			underlyingDirectLastPrice.LastPriceDate=newPriceDate;
			underlyingDirectLastPrice.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingDirectLastPrice.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingDirectValuation(underlyingDirectLastPrice);
			if(errorInfo==null) {
				// Attempt to create underlying direct valuation history.
				UnderlyingDirectLastPriceHistory lastPricehistory=new UnderlyingDirectLastPriceHistory();
				lastPricehistory.UnderlyingDirectLastPriceID=underlyingDirectLastPrice.UnderlyingDirectLastPriceID;
				lastPricehistory.LastPrice=underlyingDirectLastPrice.LastPrice;
				lastPricehistory.LastPriceDate=underlyingDirectLastPrice.LastPriceDate;
				lastPricehistory.LastUpdatedBy=Authentication.CurrentUser.UserID;
				lastPricehistory.LastUpdatedDate=DateTime.Now;
				lastPricehistory.CreatedBy=Authentication.CurrentUser.UserID;
				lastPricehistory.CreatedDate=DateTime.Now;
				errorInfo=DealRepository.SaveUnderlyingDirectValuationHistory(lastPricehistory);
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindUnderlyingDirectValuation
		[HttpGet]
		public JsonResult FindUnderlyingDirectValuation(int id) {
			UnderlyingDirectValuationModel model=DealRepository.FindUnderlyingDirectValuationModel(id);
			if(model==null) {
				model=new UnderlyingDirectValuationModel();
			}
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindLastPurchasePrice
		[HttpGet]
		public decimal FindLastPurchasePrice(int fundId,int securityId,int securityTypeId) {
			return DealRepository.FindLastPurchasePrice(fundId,securityId,securityTypeId);
		}

		[HttpGet]
		public string DeleteUnderlyingDirectValuation(int id) {
			if(DealRepository.DeleteUnderlyingDirectValuation(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region UnfundedAdjustment

		//
		// GET: /Deal/UnfundedAdjustmentList
		[HttpGet]
		public JsonResult UnfundedAdjustmentList(int underlyingFundId) {
			return Json(DealRepository.GetAllUnfundedAdjustments(underlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindUnfundedAdjustment
		[HttpGet]
		public JsonResult FindUnfundedAdjustment(int dealUnderlyingFundId) {
			return Json(DealRepository.FindUnfundedAdjustmentModel(dealUnderlyingFundId),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreateUnfundedAdjustment
		[HttpPost]
		public ActionResult UpdateUnfundedAdjustment(FormCollection collection) {
			UnfundedAdjustmentModel model=new UnfundedAdjustmentModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(ModelState.IsValid) {
				IEnumerable<ErrorInfo> errorInfo=null;
				DealUnderlyingFund dealUnderlyingFund=DealRepository.FindDealUnderlyingFund(model.DealUnderlyingFundId);
				if(dealUnderlyingFund!=null) {
					dealUnderlyingFund.CommittedAmount=model.CommitmentAmount;
					dealUnderlyingFund.UnfundedAmount=model.UnfundedAmount;
					errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
					if(errorInfo==null) {
						DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment=new DealUnderlyingFundAdjustment();
						dealUnderlyingFundAdjustment.CreatedBy=Authentication.CurrentUser.UserID;
						dealUnderlyingFundAdjustment.CreatedDate=DateTime.Now;
						dealUnderlyingFundAdjustment.LastUpdatedBy=Authentication.CurrentUser.UserID;
						dealUnderlyingFundAdjustment.LastUpdatedDate=DateTime.Now;
						dealUnderlyingFundAdjustment.Notes=model.Notes;
						dealUnderlyingFundAdjustment.CommitmentAmount=model.CommitmentAmount;
						dealUnderlyingFundAdjustment.UnfundedAmount=model.UnfundedAmount;
						dealUnderlyingFundAdjustment.DealUnderlyingFundID=model.DealUnderlyingFundId;
						errorInfo=DealRepository.SaveDealUnderlyingFundAdjustment(dealUnderlyingFundAdjustment);
					}
					resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
					if(string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result="True||"+dealUnderlyingFund.DealUnderlyingtFundID;
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

		#endregion

		#region UnderlyingDirectDividendDistribution

		//
		// POST : /Deal/CreateDirectDividendDistribution
		[HttpPost]
		public ActionResult CreateDirectDividendDistribution(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			DividendDistributionModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DividendDistributionModel),"_");
				model=new DividendDistributionModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(DividendDistributionModel),"_");
					model=new DividendDistributionModel();
					this.TryUpdateModel(model,rowCollection);
					bool isManualDividendDistribution=false;
					Boolean.TryParse(collection["IsManualDividendDistribution"],out isManualDividendDistribution);
					model.IsManualDividendDistribution=isManualDividendDistribution;
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SaveUnderlyingDirectDividendDistribution(model,collection);
					}
				}
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveUnderlyingDirectDividendDistribution(DividendDistributionModel model,FormCollection collection) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create underlying fund dividend distribution.

			UnderlyingDirectDividendDistribution underlyingDirectDividendDistribution=null;
			if(model.UnderlyingDirectDividendDistributionId>0)
				underlyingDirectDividendDistribution=DealRepository.FindUnderlyingDirectDividendDistribution(model.UnderlyingDirectDividendDistributionId);
			if(underlyingDirectDividendDistribution==null) {
				underlyingDirectDividendDistribution=new UnderlyingDirectDividendDistribution();
				underlyingDirectDividendDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				underlyingDirectDividendDistribution.CreatedDate=DateTime.Now;
			}
			underlyingDirectDividendDistribution.SecurityID=model.SecurityID;
			underlyingDirectDividendDistribution.SecurityTypeID=model.SecurityTypeID;
			underlyingDirectDividendDistribution.Amount=model.Amount??0;
			underlyingDirectDividendDistribution.IsPostRecordDateTransaction=false;
			underlyingDirectDividendDistribution.FundID=model.FundId;
			underlyingDirectDividendDistribution.DistributionDate=model.DistributionDate;
			underlyingDirectDividendDistribution.ReceivedDate=DateTime.Now;
			underlyingDirectDividendDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			underlyingDirectDividendDistribution.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SaveUnderlyingDirectDividendDistribution(underlyingDirectDividendDistribution);
			if(errorInfo==null) {
				// Attempt to create dividend distribution to each deal underlying direct.
				List<DealUnderlyingDirect> dealUnderlyingDirects=DealRepository.GetAllDealClosingUnderlyingDirects(underlyingDirectDividendDistribution.SecurityTypeID
																													 ,underlyingDirectDividendDistribution.SecurityID
																													 ,underlyingDirectDividendDistribution.FundID);
				decimal? totalShares=dealUnderlyingDirects.Sum(direct => direct.NumberOfShares);
				DividendDistribution dividendDistribution;
				foreach(var dealUnderlyingDirect in dealUnderlyingDirects) {
					dividendDistribution=DealRepository.FindDividendDistribution(
																			underlyingDirectDividendDistribution.UnderlyingDirectDividendDistributionID,
																			underlyingDirectDividendDistribution.SecurityTypeID,
																			underlyingDirectDividendDistribution.SecurityID,
																			dealUnderlyingDirect.DealID
																			);
					if(dividendDistribution==null) {
						dividendDistribution=new DividendDistribution();
						dividendDistribution.CreatedBy=Authentication.CurrentUser.UserID;
						dividendDistribution.CreatedDate=DateTime.Now;
					}
					dividendDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
					dividendDistribution.LastUpdatedDate=DateTime.Now;

					// Assign Underlying Fund Dividend Distribution.
					dividendDistribution.UnderlyingDirectDividendDistributionID=underlyingDirectDividendDistribution.UnderlyingDirectDividendDistributionID;

					// Calculate distribution amount = (Deal Underlying Direct NumberOfShares) * (Distribution Amount) * Total Number Of Shares.
					if(model.IsManualDividendDistribution==true&&dealUnderlyingDirects.Count>1) {
						dividendDistribution.Amount=DataTypeHelper.ToDecimal(collection[underlyingDirectDividendDistribution.FundID.ToString()+"_"+dealUnderlyingDirect.DealID.ToString()+"_"+"CallAmount"]);
					} else {
						dividendDistribution.Amount=decimal.Divide(
													  decimal.Multiply((dealUnderlyingDirect.NumberOfShares??0),underlyingDirectDividendDistribution.Amount)
													  ,(totalShares??0));
					}

					dividendDistribution.SecurityTypeID=underlyingDirectDividendDistribution.SecurityTypeID;
					dividendDistribution.SecurityID=underlyingDirectDividendDistribution.SecurityID;
					dividendDistribution.DealID=dealUnderlyingDirect.DealID;
					dividendDistribution.DistributionDate=underlyingDirectDividendDistribution.DistributionDate;
					errorInfo=DealRepository.SaveDividendDistribution(dividendDistribution);
				}
			}
			return errorInfo;
		}

		//
		// GET: /Deal/DirectDividendDistributionList
		[HttpGet]
		public JsonResult DirectDividendDistributionList(int securityTypeID,int securityID) {
			return Json(DealRepository.GetAllUnderlyingDirectDividendDistributions(securityTypeID,securityID),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/DeleteUnderlyingDirectDividendDistribution
		[HttpGet]
		public string DeleteUnderlyingDirectDividendDistribution(int id) {
			if(DealRepository.DeleteUnderlyingDirectDividendDistribution(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region PostRecordDividendDistribution
		//
		// GET: /Deal/PostRecordDividendDistributionList
		[HttpGet]
		public JsonResult PostRecordDividendDistributionList(int securityTypeID,int securityID) {
			return Json(DealRepository.GetAllPostRecordDividendDistributions(securityTypeID,securityID),JsonRequestBehavior.AllowGet);
		}

		//
		// POST : /Deal/CreatePostRecordDividendDistribution
		[HttpPost]
		public ActionResult CreatePostRecordDividendDistribution(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			PostRecordDividendDistributionModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;

			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				resultModel.Result=string.Empty;
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(PostRecordDividendDistributionModel),"_");
				model=new PostRecordDividendDistributionModel();
				this.TryUpdateModel(model,rowCollection);
				if(model.Amount>0) {
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()) {
						foreach(var err in errorInfo) {
							if(string.IsNullOrEmpty(err.ErrorMessage)==false)
								resultModel.Result+=rowIndex+"_"+err.PropertyName+"||"+err.ErrorMessage+"\n";
						}
						break;
					}
				}
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
					resultModel.Result=string.Empty;
					rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(PostRecordDividendDistributionModel),"_");
					model=new PostRecordDividendDistributionModel();
					this.TryUpdateModel(model,rowCollection);
					errorInfo=ValidationHelper.Validate(model);
					if(errorInfo.Any()==false) {
						errorInfo=SavePostRecordDividendDistribution(model);
					}
				}
			}
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SavePostRecordDividendDistribution(PostRecordDividendDistributionModel model) {
			IEnumerable<ErrorInfo> errorInfo=null;
			// Attempt to create post record capital call.

			DividendDistribution dividendDistribution=null;
			if(model.DividendDistributionID>0) {
				dividendDistribution=DealRepository.FindPostRecordDividendDistribution(model.DividendDistributionID);
			}
			if(dividendDistribution==null) {
				dividendDistribution=new DividendDistribution();
				dividendDistribution.CreatedBy=Authentication.CurrentUser.UserID;
				dividendDistribution.CreatedDate=DateTime.Now;
			}
			dividendDistribution.SecurityID=model.SecurityID;
			dividendDistribution.SecurityTypeID=model.SecurityTypeID;
			dividendDistribution.Amount=model.Amount??0;
			dividendDistribution.DistributionDate=model.DistributionDate;
			dividendDistribution.DealID=model.DealId;
			dividendDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			dividendDistribution.LastUpdatedDate=DateTime.Now;
			errorInfo=DealRepository.SavePostRecordDividendDistribution(dividendDistribution);
			return errorInfo;
		}

		//
		// GET: /Deal/DeletePostRecordDividendDistribution
		[HttpGet]
		public string DeletePostRecordDividendDistribution(int id) {
			if(DealRepository.DeletePostRecordDividendDistribution(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
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
			ViewData["MenuName"]="DealManagement";
			ViewData["SubmenuName"]="Reconcile";
			ViewData["PageName"]="Reconcile";
			return View(new ReconcileModel());
		}

		//
		// POST: /Deal/ReconcileList
		[HttpPost]
		public JsonResult ReconcileList(FormCollection collection) {
			ReconcileSearchModel model=new ReconcileSearchModel();
			this.TryUpdateModel(model,collection);
			string error=string.Empty;
			List<ReconcileReportModel> allReconciles=null;
			DateTime startDate=(model.StartDate??Convert.ToDateTime("01/01/1900"));
			DateTime endDate=(model.EndDate??DateTime.MaxValue);
			object allFundExpenses=null;
			string sortName=collection["sortName"];
			string sortOrder=collection["sortOrder"];
			if(string.IsNullOrEmpty(sortName))
				sortName="CounterParty";
			if(string.IsNullOrEmpty(sortOrder))
				sortOrder="asc";
			int[] totalRowArrays=new int[9];
			int totalRows=0;
			int totalFundExpenses=0;
			if(ModelState.IsValid) {
				DeepBlue.Models.Deal.Enums.ReconcileType reconcileType=(DeepBlue.Models.Deal.Enums.ReconcileType)model.ReconcileType;
				if(reconcileType==ReconcileType.All) {
					allReconciles=DealRepository.GetAllReconciles(startDate,
																	endDate,
																	model.FundId,
																	model.UnderlyingFundId,
																	model.IsReconcile,
																	model.PageIndex,
																	model.PageSize,
																	sortName,
																	sortOrder,
																	ref totalRowArrays);
					allFundExpenses=DealRepository.GetAllFundExpenses(model.FundId,
																		startDate,
																		endDate,
																		model.PageIndex,
																		model.PageSize,
																		ref totalFundExpenses);
				} else {
					allReconciles=DealRepository.GetAllReconciles(startDate,
																	endDate,
																	model.FundId,
																	model.UnderlyingFundId,
																	model.IsReconcile,
																	model.PageIndex,
																	model.PageSize,
																	sortName,
																	sortOrder,
																	ref totalRows,
																	reconcileType);
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							error+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			totalRowArrays[model.ReconcileType]=totalRows;
			return Json(new {
				Error=error,
				Results=allReconciles,
				FundExpenses=allFundExpenses,
				totalFundExpenses=totalFundExpenses,
				page=model.PageIndex,
				TotalRows=totalRowArrays
			},JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateReconcile
		[HttpPost]
		public ActionResult CreateReconcile(FormCollection collection) {
			int totalRows=0;
			int.TryParse(collection["TotalRows"],out totalRows);
			int rowIndex=0;
			ResultModel resultModel=new ResultModel();
			FormCollection rowCollection;
			ReconcileModel model=null;
			IEnumerable<ErrorInfo> errorInfo=null;
			// Validate each rows.
			for(rowIndex=0;rowIndex<totalRows;rowIndex++) {
				rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(ReconcileModel),"_");
				model=new ReconcileModel();
				this.TryUpdateModel(model,rowCollection);
				IEnumerable<ErrorInfo> validateErrorInfo=ValidationHelper.Validate(model);
				if(validateErrorInfo.Any()==false) {
					errorInfo=SaveReconcile(model);
					if(errorInfo!=null)
						break;
				}
			}
			resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			return View("Result",resultModel);
		}

		private IEnumerable<ErrorInfo> SaveReconcile(ReconcileModel model) {
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=ValidationHelper.Validate(model);
			if(errorInfo.Any()==false) {
				switch((ReconcileType)model.ReconcileTypeId) {
				case ReconcileType.CapitalCall:
					CapitalCallLineItem capitalCallLineItem=CapitalCallRepository.FindCapitalCallLineItem(model.Id);
					if(capitalCallLineItem!=null) {
						capitalCallLineItem.IsReconciled=model.IsReconciled;
						capitalCallLineItem.PaidON=model.PaidOn;
						capitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
						capitalCallLineItem.LastUpdatedDate=DateTime.Now;
						capitalCallLineItem.ChequeNumber=model.ChequeNumber;
						errorInfo=CapitalCallRepository.SaveCapitalCallLineItem(capitalCallLineItem);
						if(errorInfo==null) {
							DeepBlue.Models.Entity.CapitalCall capitalCall=CapitalCallRepository.FindCapitalCall(capitalCallLineItem.CapitalCallID);
							capitalCall.CapitalCallDueDate=model.PaymentDate;
							errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
						}
					}
					break;
				case ReconcileType.CapitalDistribution:
					CapitalDistributionLineItem capitalDistributionLineItem=CapitalCallRepository.FindCapitalDistributionLineItem(model.Id);
					if(capitalDistributionLineItem!=null) {
						capitalDistributionLineItem.IsReconciled=model.IsReconciled;
						capitalDistributionLineItem.PaidON=model.PaidOn;
						capitalDistributionLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
						capitalDistributionLineItem.LastUpdatedDate=DateTime.Now;
						capitalDistributionLineItem.ChequeNumber=model.ChequeNumber;
						errorInfo=CapitalCallRepository.SaveCapitalDistributionLineItem(capitalDistributionLineItem);
						if(errorInfo==null) {
							DeepBlue.Models.Entity.CapitalDistribution capitalDistribution=CapitalCallRepository.FindCapitalDistribution(capitalDistributionLineItem.CapitalDistributionID);
							capitalDistribution.CapitalDistributionDueDate=model.PaymentDate;
							errorInfo=CapitalCallRepository.SaveCapitalDistribution(capitalDistribution);
						}
					}
					break;
				case ReconcileType.UnderlyingFundCapitalCall:
					UnderlyingFundCapitalCall underlyingFundCapitalCall=DealRepository.FindUnderlyingFundCapitalCall(model.Id);
					if(underlyingFundCapitalCall!=null) {
						underlyingFundCapitalCall.IsReconciled=model.IsReconciled;
						underlyingFundCapitalCall.PaidON=model.PaidOn;
						underlyingFundCapitalCall.ReceivedDate=model.PaymentDate;
						underlyingFundCapitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
						underlyingFundCapitalCall.LastUpdatedDate=DateTime.Now;
						underlyingFundCapitalCall.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SaveUnderlyingFundCapitalCall(underlyingFundCapitalCall);
					}
					break;
				case ReconcileType.UnderlyingFundCashDistribution:
					UnderlyingFundCashDistribution underlyingFundCashDistribution=DealRepository.FindUnderlyingFundCashDistribution(model.Id);
					if(underlyingFundCashDistribution!=null) {
						underlyingFundCashDistribution.IsReconciled=model.IsReconciled;
						underlyingFundCashDistribution.PaidON=model.PaidOn;
						underlyingFundCashDistribution.ReceivedDate=model.PaymentDate;
						underlyingFundCashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
						underlyingFundCashDistribution.LastUpdatedDate=DateTime.Now;
						underlyingFundCashDistribution.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SaveUnderlyingFundCashDistribution(underlyingFundCashDistribution);
					}
					break;
				case ReconcileType.DividendDistribution:
					UnderlyingDirectDividendDistribution underlyingDirectDividendDistribution=DealRepository.FindUnderlyingDirectDividendDistribution(model.Id);
					if(underlyingDirectDividendDistribution!=null) {
						underlyingDirectDividendDistribution.IsReconciled=model.IsReconciled;
						underlyingDirectDividendDistribution.PaidON=model.PaidOn;
						underlyingDirectDividendDistribution.ReceivedDate=model.PaymentDate;
						underlyingDirectDividendDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
						underlyingDirectDividendDistribution.LastUpdatedDate=DateTime.Now;
						underlyingDirectDividendDistribution.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SaveUnderlyingDirectDividendDistribution(underlyingDirectDividendDistribution);
					}
					break;
				case ReconcileType.PostRecordCapitalCall:
					UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem=DealRepository.FindUnderlyingFundPostRecordCapitalCall(model.Id);
					if(underlyingFundCapitalCallLineItem!=null) {
						underlyingFundCapitalCallLineItem.IsReconciled=model.IsReconciled;
						underlyingFundCapitalCallLineItem.PaidON=model.PaidOn;
						underlyingFundCapitalCallLineItem.CapitalCallDate=model.PaymentDate;
						underlyingFundCapitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
						underlyingFundCapitalCallLineItem.LastUpdatedDate=DateTime.Now;
						underlyingFundCapitalCallLineItem.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SaveUnderlyingFundPostRecordCapitalCall(underlyingFundCapitalCallLineItem);
					}
					break;
				case ReconcileType.PostRecordDistribution:
					CashDistribution cashDistribution=DealRepository.FindUnderlyingFundPostRecordCashDistribution(model.Id);
					if(cashDistribution!=null) {
						cashDistribution.IsReconciled=model.IsReconciled;
						cashDistribution.PaidON=model.PaidOn;
						cashDistribution.DistributionDate=model.PaymentDate;
						cashDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
						cashDistribution.LastUpdatedDate=DateTime.Now;
						cashDistribution.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SaveUnderlyingFundPostRecordCashDistribution(cashDistribution);
					}
					break;
				case ReconcileType.PostRecordDividendDistribution:
					DividendDistribution dividendDistribution=DealRepository.FindPostRecordDividendDistribution(model.Id);
					if(dividendDistribution!=null) {
						dividendDistribution.IsReconciled=model.IsReconciled;
						dividendDistribution.PaidON=model.PaidOn;
						dividendDistribution.DistributionDate=model.PaymentDate;
						dividendDistribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
						dividendDistribution.LastUpdatedDate=DateTime.Now;
						dividendDistribution.ChequeNumber=model.ChequeNumber;
						errorInfo=DealRepository.SavePostRecordDividendDistribution(dividendDistribution);
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
			var collection=new FormCollection(Request.QueryString);
			ViewData["MenuName"]="AssetManagement";
			ViewData["SubmenuName"]="DirectLibrary";
			ViewData["PageName"]="AddDirects";
			ViewData["Mode"]=collection["mode"];
			CreateIssuerModel model=new CreateIssuerModel();

			model.IssuerDetailModel.IssuerRatings=SelectListFactory.GetEmptySelectList();

			model.EquityDetailModel.Currencies=SelectListFactory.GetCurrencySelectList(AdminRepository.GetAllCurrencies());
			model.EquityDetailModel.DocumentTypes=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
			model.EquityDetailModel.ShareClassTypes=SelectListFactory.GetShareClassTypeSelectList(AdminRepository.GetAllShareClassTypes());
			model.EquityDetailModel.EquityTypes=SelectListFactory.GetEquityTypeSelectList(AdminRepository.GetAllEquityTypes());
			model.EquityDetailModel.UploadTypes=SelectListFactory.GetUploadTypeSelectList();
			model.EquityDetailModel.EquitySecurityTypes=SelectListFactory.GetEquitySecurityTypeList();

			model.FixedIncomeDetailModel.FixedIncomeTypes=SelectListFactory.GetFixedIncomeTypesSelectList(AdminRepository.GetAllFixedIncomeTypes());
			model.FixedIncomeDetailModel.Currencies=model.EquityDetailModel.Currencies;
			model.FixedIncomeDetailModel.DocumentTypes=model.EquityDetailModel.DocumentTypes;
			model.FixedIncomeDetailModel.UploadTypes=model.EquityDetailModel.UploadTypes;

			model.EquityDetailModel.EquityCurrencyId=(int)DeepBlue.Models.Deal.Enums.Currency.USD;
			model.FixedIncomeDetailModel.FixedIncomeCurrencyId=(int)DeepBlue.Models.Deal.Enums.Currency.USD;
			List<Models.Entity.FileType> fileTypes=AdminRepository.GetAllFileTypes();
			string fileExtensions=string.Empty;
			foreach(var type in fileTypes) {
				var arrExtensions=type.FileExtension.Split((",").ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
				foreach(var ext in arrExtensions) {
					fileExtensions+=string.Format("\"{0}\",",ext);
				}
			}
			if(string.IsNullOrEmpty(fileExtensions)==false) {
				model.DocumentFileExtensions=string.Format("[{0}]",fileExtensions.Substring(0,fileExtensions.Length-1));
			}
			return View(model);
		}

		//
		// GET: /Deal/FindIssuer
		[HttpGet]
		public JsonResult FindIssuer(int id) {
			CreateIssuerModel model=DealRepository.FindIssuerModel(id);
			if(model==null)
				model=new CreateIssuerModel();
			if(model.IssuerDetailModel==null)
				model.IssuerDetailModel=new IssuerDetailModel();
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		public JsonResult DirectList(int pageIndex,int pageSize,string sortName,string sortOrder,bool isGP,int? companyId) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<DirectListModel> directs=DealRepository.GetAllDirects(pageIndex,pageSize,sortName,sortOrder,ref totalRows,isGP,companyId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var direct in directs) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { direct.DirectId,direct.DirectName }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		public JsonResult EquityList(int issuerId,int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<EquityListModel> equities=DealRepository.GetAllEquity(issuerId,pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var equity in equities) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> { equity.EquityId,equity.Symbol,equity.Industry,equity.EquityType }
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquityFixedIncomeIssuers
		[HttpGet]
		public JsonResult FindEquityFixedIncomeIssuers(string term) {
			return Json(DealRepository.FindEquityFixedIncomeIssuers(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindEquities
		[HttpGet]
		public JsonResult FindEquities() {
			return Json(DealRepository.FindEquities(),JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/CreateIssuer
		[HttpPost]
		public ActionResult CreateIssuer(FormCollection collection) {
			IssuerDetailModel model=new IssuerDetailModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage=IssuerNameAvailable(model.Name,model.IssuerId,model.IsUnderlyingFundModel);

			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				ModelState.AddModelError("Name",ErrorMessage);
			}
			if(ModelState.IsValid) {
				Models.Entity.Issuer issuer=null;
				IEnumerable<ErrorInfo> errorInfo=SaveIssuer(model,out issuer);
				if(errorInfo!=null) {
					foreach(var err in errorInfo.ToList()) {
						resultModel.Result+=err.PropertyName+" : "+err.ErrorMessage+"\n";
					}
				} else {
					resultModel.Result="True||"+issuer.IssuerID+"||"+issuer.Name;
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
			if(model.IsUnderlyingFundModel)
				resultModel.Result=resultModel.Result.Replace("Issuer Name","GP").Replace("Parent Name","GP Parent");
			else
				resultModel.Result=resultModel.Result.Replace("Issuer Name","Company").Replace("Parent Name","Company Parent");
			return View("Result",resultModel);
		}

		//
		// POST: /Deal/UpdateIssuer
		[HttpPost]
		public string UpdateIssuer(FormCollection collection) {
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;

			IssuerDetailModel model=new IssuerDetailModel();
			this.TryUpdateModel(model);

			errorInfo=ValidationHelper.Validate(model);
			resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
			resultModel.Result=resultModel.Result.Replace("Issuer Name","Company").Replace("Parent Name","Company Parent");

			string ErrorMessage=IssuerNameAvailable(model.Name,model.IssuerId,model.IsUnderlyingFundModel);

			if(String.IsNullOrEmpty(ErrorMessage)==false) {
				resultModel.Result+="Company name already exist"+"\n";
			}

			if(string.IsNullOrEmpty(resultModel.Result)) {
				Models.Entity.Issuer issuer=null;
				errorInfo=SaveIssuer(model,out issuer);
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				if(errorInfo==null) {

					EquityDetailModel equityDetailModel=new EquityDetailModel();
					bool isValidDocument=false;
					string documentError=string.Empty;
					this.TryUpdateModel(equityDetailModel);

					UnderlyingDirectDocumentModel equityUnderlyingDirectDocumentModel=new UnderlyingDirectDocumentModel {
						DocumentDate=DateTime.Now,
						DocumentTypeId=equityDetailModel.EquityDocumentTypeId,
						File=equityDetailModel.EquityFile,
						SecurityId=equityDetailModel.EquityId,
						SecurityTypeId=(int)Models.Deal.Enums.SecurityType.Equity,
						FilePath=equityDetailModel.EquityFilePath,
						UploadTypeId=equityDetailModel.EquityUploadTypeId
					};

					if(string.IsNullOrEmpty(equityDetailModel.EquitySymbol)==false||
						equityDetailModel.EquityIndustryId>0||
						equityDetailModel.ShareClassTypeId>0||
						string.IsNullOrEmpty(equityDetailModel.EquityISINO)==false||
						string.IsNullOrEmpty(equityDetailModel.EquityComments)==false||
						equityDetailModel.EquityDocumentTypeId>0||
						((equityDetailModel.EquityUploadTypeId==(int)Models.Document.UploadType.Upload)&&equityDetailModel.EquityFile!=null)||
						((equityDetailModel.EquityUploadTypeId==(int)Models.Document.UploadType.Link)&&string.IsNullOrEmpty(equityDetailModel.EquityFilePath)==false)||
						equityDetailModel.EquityTypeId>0) {
						errorInfo=ValidationHelper.Validate(equityDetailModel);
						resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
					}

					//documentError = ValidateUnderlyingDirectDocument(equityUnderlyingDirectDocumentModel, ref isValidDocument);
					//if (isValidDocument) {
					//	resultModel.Result += documentError;
					//}

					FixedIncomeDetailModel fixedIncomeDetailModel=new FixedIncomeDetailModel();
					this.TryUpdateModel(fixedIncomeDetailModel);

					UnderlyingDirectDocumentModel fixedIncomeUnderlyingDirectDocumentModel=new UnderlyingDirectDocumentModel {
						DocumentDate=DateTime.Now,
						DocumentTypeId=fixedIncomeDetailModel.FixedIncomeDocumentTypeId,
						File=fixedIncomeDetailModel.FixedIncomeFile,
						SecurityId=fixedIncomeDetailModel.FixedIncomeId,
						SecurityTypeId=(int)Models.Deal.Enums.SecurityType.FixedIncome,
						FilePath=fixedIncomeDetailModel.FixedIncomeFilePath,
						UploadTypeId=fixedIncomeDetailModel.FixedIncomeUploadTypeId
					};

					if(fixedIncomeDetailModel.FaceValue>0||
						fixedIncomeDetailModel.Maturity.HasValue||
						fixedIncomeDetailModel.IssuedDate.HasValue||
						string.IsNullOrEmpty(fixedIncomeDetailModel.CouponInformation)==false||
						fixedIncomeDetailModel.Frequency>0||
						fixedIncomeDetailModel.FirstCouponDate.HasValue||
						fixedIncomeDetailModel.FirstAccrualDate.HasValue||
						fixedIncomeDetailModel.FixedIncomeTypeId>0||
						fixedIncomeDetailModel.FixedIncomeIndustryId>0||
						fixedIncomeDetailModel.FixedIncomeDocumentTypeId>0||
						((fixedIncomeDetailModel.FixedIncomeUploadTypeId==(int)Models.Document.UploadType.Upload)&&fixedIncomeDetailModel.FixedIncomeFile!=null)||
						((fixedIncomeDetailModel.FixedIncomeUploadTypeId==(int)Models.Document.UploadType.Link)&&string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeFilePath)==false)||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeISINO)==false||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeComments)==false||
						string.IsNullOrEmpty(fixedIncomeDetailModel.FixedIncomeSymbol)==false) {
						errorInfo=ValidationHelper.Validate(fixedIncomeDetailModel);
						resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
					}

					documentError=ValidateUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel,ref isValidDocument);
					if(isValidDocument) {
						resultModel.Result+=documentError;
					}

					if(string.IsNullOrEmpty(resultModel.Result)) {
						if(fixedIncomeDetailModel.FixedIncomeTypeId>0) {
							errorInfo=SaveFixedIncome(ref fixedIncomeDetailModel);
							if(errorInfo==null) {
								fixedIncomeUnderlyingDirectDocumentModel.SecurityId=fixedIncomeDetailModel.FixedIncomeId;
								resultModel.Result+=SaveUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel);
							}
							resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
						}
						if(equityDetailModel.EquityTypeId>0) {
							errorInfo=SaveEquity(ref equityDetailModel);
							if(errorInfo==null) {
								equityUnderlyingDirectDocumentModel.SecurityId=equityDetailModel.EquityId;
								resultModel.Result+=SaveUnderlyingDirectDocument(equityUnderlyingDirectDocumentModel);
							}
							resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
						}
					}
				}
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result="True||"+issuer.IssuerID+"||"+issuer.Name;
				}
			}
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		private string ValidateUnderlyingDirectDocument(UnderlyingDirectDocumentModel model,ref bool isValidDocument) {
			ResultModel resultModel=new ResultModel();
			Models.Document.UploadType uploadType=(Models.Document.UploadType)model.UploadTypeId;
			Models.Deal.Enums.SecurityType securityType=(Models.Deal.Enums.SecurityType)model.SecurityTypeId;
			string securityTypeName=(securityType==Models.Deal.Enums.SecurityType.Equity?"Equity":"Fixed Income");
			string fileName=string.Empty;
			string ext=string.Empty;
			if(model.File!=null) {
				fileName=model.File.FileName;
			}
			if(string.IsNullOrEmpty(model.FilePath)==false) {
				if((model.FilePath.ToLower().StartsWith("http://")==false)&&
				   (model.FilePath.ToLower().StartsWith("https://")==false))
					model.FilePath="http://"+model.FilePath;
			}
			string filePath=model.FilePath;
			if(model.DocumentTypeId>0||
				(uploadType==Models.Document.UploadType.Upload&&string.IsNullOrEmpty(fileName)==false)||
				(uploadType==Models.Document.UploadType.Link&&string.IsNullOrEmpty(filePath)==false)
				) {

				isValidDocument=true;

				if(model.DocumentTypeId==0)
					resultModel.Result+=securityTypeName+" Document Type required\n";

				if(model.DocumentDate.Year<1900)
					resultModel.Result+=securityTypeName+" Document Date required\n";

				switch(uploadType) {
				case Models.Document.UploadType.Link:
					if(string.IsNullOrEmpty(filePath)) {
						resultModel.Result+="Link is required";
					} else {
						if(UploadFileHelper.CheckFilePath(filePath)==false) {
							ModelState.AddModelError("FilePath","Invalid Link.");
						} else {
							fileName=Path.GetFileName(filePath);
							ext=Path.GetExtension(filePath);
							if(string.IsNullOrEmpty(fileName)==false) {
								filePath=filePath.Replace(fileName,"");
							}
							break;
						}
					}
					break;
				case Models.Document.UploadType.Upload:
					if(model.File==null) {
						resultModel.Result+="File is required";
					}
					if(model.File!=null) {
						if(string.IsNullOrEmpty(model.File.FileName)) {
							resultModel.Result+="File is required";
						} else {
							ext=Path.GetExtension(model.File.FileName).ToLower();
						}
					}
					break;
				}

				if(string.IsNullOrEmpty(ext)==false) {
					string fileTypeError=string.Empty;
					FileType fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out fileTypeError);
					if(fileType==null) {
						resultModel.Result+=fileTypeError;
					}
				}
			}
			return resultModel.Result;
		}

		private string SaveUnderlyingDirectDocument(UnderlyingDirectDocumentModel model) {
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			string uploadPath=(model.SecurityTypeId==(int)Models.Deal.Enums.SecurityType.Equity?"EquityDocumentUploadPath":"FixedIncomeDocumentUploadPath");
			Models.Document.UploadType uploadType=(Models.Document.UploadType)model.UploadTypeId;
			Models.Deal.Enums.SecurityType securityType=(Models.Deal.Enums.SecurityType)model.SecurityTypeId;
			bool isValidDocument=false;
			resultModel.Result=ValidateUnderlyingDirectDocument(model,ref isValidDocument);
			if(isValidDocument) {
				if(string.IsNullOrEmpty(resultModel.Result)) {
					UnderlyingDirectDocument underlyingDirectDocument=new UnderlyingDirectDocument();
					underlyingDirectDocument.CreatedBy=Authentication.CurrentUser.UserID;
					underlyingDirectDocument.CreatedDate=DateTime.Now;
					underlyingDirectDocument.LastUpdatedBy=Authentication.CurrentUser.UserID;
					underlyingDirectDocument.LastUpdatedDate=DateTime.Now;
					underlyingDirectDocument.EntityID=Authentication.CurrentEntity.EntityID;
					underlyingDirectDocument.DocumentDate=model.DocumentDate;
					underlyingDirectDocument.DocumentTypeID=model.DocumentTypeId;
					underlyingDirectDocument.SecurityTypeID=model.SecurityTypeId;
					underlyingDirectDocument.SecurityID=model.SecurityId;
					DeepBlue.Models.File.UploadFileModel uploadFileModel=null;
					resultModel.Result+=CheckDocumentFile(uploadType,model.FilePath,model.File,ref uploadFileModel);
					if(string.IsNullOrEmpty(resultModel.Result)) {
						underlyingDirectDocument.File=new Models.Entity.File();
						underlyingDirectDocument.File.CreatedBy=Authentication.CurrentUser.UserID;
						underlyingDirectDocument.File.CreatedDate=DateTime.Now;
						underlyingDirectDocument.File.LastUpdatedBy=Authentication.CurrentUser.UserID;
						underlyingDirectDocument.File.LastUpdatedDate=DateTime.Now;
						underlyingDirectDocument.File.EntityID=Authentication.CurrentEntity.EntityID;
						if(uploadType==Models.Document.UploadType.Upload) {
							uploadFileModel=UploadFileHelper.Upload(model.File,uploadPath,Authentication.CurrentEntity.EntityID,underlyingDirectDocument.SecurityID,underlyingDirectDocument.DocumentTypeID,uploadFileModel.FileName);
							underlyingDirectDocument.File.Size=uploadFileModel.Size;
						}
						if(uploadFileModel!=null) {
							string ext=Path.GetExtension(uploadFileModel.FileName).ToLower();
							string fileTypeError=string.Empty;
							Models.Entity.FileType fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out fileTypeError);
							if(fileType==null) {
								resultModel.Result+=fileTypeError;
							} else {
								underlyingDirectDocument.File.FileTypeID=fileType.FileTypeID;

								underlyingDirectDocument.File.FileName=uploadFileModel.FileName;
								underlyingDirectDocument.File.FilePath=uploadFileModel.FilePath;
							}
						}
						errorInfo=DealRepository.SaveUnderlyingDirectDocument(underlyingDirectDocument);
						resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
					}
				}
			}
			return resultModel.Result;
		}

		private IEnumerable<ErrorInfo> SaveEquity(ref EquityDetailModel model) {
			Models.Entity.Equity equity=DealRepository.FindEquity(model.EquityId);
			EquitySecurityType equitySecurityType=(EquitySecurityType)model.EquitySecurityTypeId;
			if(equity==null) {
				equity=new Models.Entity.Equity();
			}
			equity.CurrencyID=((model.EquityCurrencyId??0)>0?model.EquityCurrencyId:null);
			equity.EquityTypeID=model.EquityTypeId;
			equity.IndustryID=((model.EquityIndustryId??0)>0?model.EquityIndustryId:null);
			equity.IssuerID=model.IssuerId;
			equity.Public=(equitySecurityType==EquitySecurityType.Public?true:false);
			equity.ShareClassTypeID=((model.ShareClassTypeId??0)>0?model.ShareClassTypeId:null);
			equity.Symbol=model.EquitySymbol;
			equity.EntityID=Authentication.CurrentEntity.EntityID;
			equity.Comments=model.EquityComments;
			equity.ISIN=model.EquityISINO;
			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveEquity(equity);
			model.EquityId=equity.EquityID;
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveFixedIncome(ref FixedIncomeDetailModel model) {
			Models.Entity.FixedIncome fixedIncome=DealRepository.FindFixedIncome(model.FixedIncomeId);
			if(fixedIncome==null) {
				fixedIncome=new Models.Entity.FixedIncome();
			}
			fixedIncome.CurrencyID=((model.FixedIncomeCurrencyId??0)>0?model.FixedIncomeCurrencyId:null);
			fixedIncome.FixedIncomeTypeID=model.FixedIncomeTypeId;
			fixedIncome.IndustryID=((model.FixedIncomeIndustryId??0)>0?model.FixedIncomeIndustryId:null);
			fixedIncome.IssuerID=model.IssuerId;
			fixedIncome.ISIN=model.FixedIncomeISINO;
			fixedIncome.Symbol=model.FixedIncomeSymbol;
			fixedIncome.EntityID=Authentication.CurrentEntity.EntityID;
			fixedIncome.Maturity=model.Maturity;
			fixedIncome.IssuedDate=model.IssuedDate;
			fixedIncome.Frequency=model.Frequency;
			fixedIncome.FirstCouponDate=model.FirstCouponDate;
			fixedIncome.FirstAccrualDate=model.FirstAccrualDate;
			fixedIncome.FaceValue=model.FaceValue;
			fixedIncome.CouponInformation=model.CouponInformation;
			fixedIncome.Comments=model.FixedIncomeComments;
			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveFixedIncome(fixedIncome);
			model.FixedIncomeId=fixedIncome.FixedIncomeID;
			return errorInfo;
		}

		private IEnumerable<ErrorInfo> SaveIssuer(IssuerDetailModel model,out Models.Entity.Issuer issuer) {
			issuer=DealRepository.FindIssuer(model.IssuerId);
			if(issuer==null) {
				issuer=new Models.Entity.Issuer();
			}
			issuer.Name=model.Name;
			issuer.ParentName=model.ParentName==null?string.Empty:model.ParentName;
			issuer.CountryID=model.CountryId;
			issuer.EntityID=Authentication.CurrentEntity.EntityID;
			issuer.IsGP=model.IsUnderlyingFundModel;
			issuer.AnnualMeetingDate=model.AnnualMeetingDate;
			IEnumerable<ErrorInfo> errorInfo=DealRepository.SaveIssuer(issuer);
			if(errorInfo==null) {
				if(model.AnnualMeetingDate.HasValue) {
					if(DealRepository.FindAnnualMeetingDateHistory(model.IssuerId,(model.AnnualMeetingDate??Convert.ToDateTime("01/01/1900")))==false) {
						AnnualMeetingHistory annualMeetingHistory=new AnnualMeetingHistory {
							AnnualMeetingDate=model.AnnualMeetingDate,
							IssuerID=issuer.IssuerID
						};
						errorInfo=DealRepository.SaveAnnualMeetingHistory(annualMeetingHistory);
					}
				}
			}
			return errorInfo;
		}

		//
		// GET: /Deal/FindIssuers
		[HttpGet]
		public JsonResult FindIssuers(string term) {
			return Json(DealRepository.FindIssuers(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindCompanys
		[HttpGet]
		public JsonResult FindCompanys(string term) {
			return Json(DealRepository.FindCompanys(term),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/FindGPs
		[HttpGet]
		public JsonResult FindGPs(string term) {
			return Json(DealRepository.FindGPs(term),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public string IssuerNameAvailable(string name,int issuerID,bool isGP) {
			if(DealRepository.IssuerNameAvailable(name,issuerID,isGP))
				return "Name already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Deal/AnnualMeetingDateList
		[HttpGet]
		public JsonResult AnnualMeetingDateList(int pageIndex,int pageSize,string sortName,string sortOrder) {
			FlexigridData flexgridData=new FlexigridData();
			int totalRows=0;
			List<AnnualMeetingHistory> annualMeetingDates=DealRepository.GetAllAnnualMettingDates(pageIndex,pageSize,sortName,sortOrder,ref totalRows);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			foreach(var annualMeetingDate in annualMeetingDates) {
				flexgridData.rows.Add(new FlexigridRow {
					cell=new List<object> {
						(annualMeetingDate.AnnualMeetingDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")
					}
				});
			}
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/UnderlyingDirectList
		[HttpGet]
		public JsonResult UnderlyingDirectList(int pageIndex,int pageSize,string sortName,string sortOrder,int companyId) {
			GridData flexgridData=new GridData();
			int totalRows=0;
			flexgridData.rows=DealRepository.UnderlyingDirectList(pageIndex,pageSize,sortName,sortOrder,ref totalRows,companyId);
			flexgridData.total=totalRows;
			flexgridData.page=pageIndex;
			return Json(flexgridData,JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult UnderlyingDirectDocumentList(int pageIndex,int pageSize,string sortName,string sortOrder,int securityID,int securityTypeID) {
			int totalRows=0;
			List<UnderlyingDirectDocumentList> underlyingDirectDocuments=DealRepository.GetAllUnderlyingDirectDocuments(pageIndex,pageSize,sortName,sortOrder,ref totalRows,securityID,securityTypeID);
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(underlyingDirectDocuments);
		}

		[HttpGet]
		public string DeleteUnderlyingDirectDocumentFile(int id) {
			if(DealRepository.DeleteUnderlyingDirectDocument(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}


		//
		// GET: /Deal/FindEquity
		[HttpGet]
		public JsonResult FindEquity(int id) {
			return Json(DealRepository.FindEquityModel(id),JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/UpdateEquity
		[HttpPost]
		public string UpdateEquity(FormCollection collection) {
			EquityDetailModel model=new EquityDetailModel();
			this.TryUpdateModel(model);
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo;
			if(model.EquityId<0) {
				ModelState.AddModelError("EquityId","EquityId is required");
			}
			if(ModelState.IsValid) {
				errorInfo=SaveEquity(ref model);
				if(errorInfo==null) {
					UnderlyingDirectDocumentModel equityUnderlyingDirectDocumentModel=new UnderlyingDirectDocumentModel {
						DocumentDate=DateTime.Now,
						DocumentTypeId=model.EquityDocumentTypeId,
						File=model.EquityFile,
						SecurityId=model.EquityId,
						SecurityTypeId=(int)Models.Deal.Enums.SecurityType.Equity,
						FilePath=model.EquityFilePath,
						UploadTypeId=model.EquityUploadTypeId
					};
					equityUnderlyingDirectDocumentModel.SecurityId=model.EquityId;
					resultModel.Result+=SaveUnderlyingDirectDocument(equityUnderlyingDirectDocumentModel);
				}
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		//
		// GET: /Deal/FindFixedIncome
		[HttpGet]
		public JsonResult FindFixedIncome(int id) {
			return Json(DealRepository.FindFixedIncomeModel(id),JsonRequestBehavior.AllowGet);
		}

		//
		// POST: /Deal/UpdateFixedIncome
		[HttpPost]
		public string UpdateFixedIncome(FormCollection collection) {
			FixedIncomeDetailModel model=new FixedIncomeDetailModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo;
			if(model.FixedIncomeId<0) {
				ModelState.AddModelError("FixedIncomeId","FixedIncomeId is required");
			}
			if(ModelState.IsValid) {
				errorInfo=SaveFixedIncome(ref model);
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result+=UploadFixedIncomeDocument(model);
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
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		private string UploadFixedIncomeDocument(FixedIncomeDetailModel model) {
			ResultModel resultModel=new ResultModel();
			UnderlyingDirectDocumentModel fixedIncomeUnderlyingDirectDocumentModel=new UnderlyingDirectDocumentModel {
				DocumentDate=DateTime.Now,
				DocumentTypeId=model.FixedIncomeDocumentTypeId,
				File=model.FixedIncomeFile,
				SecurityId=model.FixedIncomeId,
				SecurityTypeId=(int)Models.Deal.Enums.SecurityType.FixedIncome,
				FilePath=model.FixedIncomeFilePath,
				UploadTypeId=model.FixedIncomeUploadTypeId
			};
			bool isValidDocument=false;
			resultModel.Result+=ValidateUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel,ref isValidDocument);
			if(string.IsNullOrEmpty(resultModel.Result)) {
				fixedIncomeUnderlyingDirectDocumentModel.SecurityId=model.FixedIncomeId;
				resultModel.Result+=SaveUnderlyingDirectDocument(fixedIncomeUnderlyingDirectDocumentModel);
			}
			return resultModel.Result;
		}

		public string CreateFixedIncomeDocument(FormCollection collection) {
			FixedIncomeDetailModel model=new FixedIncomeDetailModel();
			this.TryUpdateModel(model,collection);
			ResultModel resultModel=new ResultModel();
			if(model.FixedIncomeId<0) {
				ModelState.AddModelError("FixedIncomeId","FixedIncomeId is required");
			}
			if(ModelState.IsValid) {
				resultModel.Result+=UploadFixedIncomeDocument(model);
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return JsonSerializer.ToJsonObject(new {
				error=string.Empty, data=resultModel.Result
			}).ToString();
		}

		//
		// GET: /Deal/DeleteEquity
		[HttpGet]
		public string DeleteEquity(int id) {
			if(DealRepository.DeleteEquity(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		//
		// GET: /Deal/DeleteFixedIncome
		[HttpGet]
		public string DeleteFixedIncome(int id) {
			if(DealRepository.DeleteFixedIncome(id)==false) {
				return "Cann't Delete! Child record found!";
			} else {
				return string.Empty;
			}
		}

		#endregion

		#region Check Document
		private string CheckDocumentFile(
			DeepBlue.Models.Document.UploadType uploadType,
			string filePath,
			HttpPostedFileBase postFile,
			ref DeepBlue.Models.File.UploadFileModel uploadFileModel) {

			string fileName=string.Empty;
			string ext=string.Empty;
			string errorMessage=string.Empty;
			Models.Entity.FileType fileType=null;
			errorMessage=string.Empty;
			ResultModel resultModel=new ResultModel();

			switch(uploadType) {
			case Models.Document.UploadType.Link:
				if(string.IsNullOrEmpty(filePath)) {
					resultModel.Result+="Link is required";
				} else {
					if((filePath.ToLower().StartsWith("http://")==false)&&
						(filePath.ToLower().StartsWith("https://")==false))
						filePath="http://"+filePath;
					if(UploadFileHelper.CheckFilePath(filePath)==false) {
						ModelState.AddModelError("FilePath","Invalid Link.");
					} else {
						fileName=Path.GetFileName(filePath);
						ext=Path.GetExtension(filePath);
						if(string.IsNullOrEmpty(fileName)) {
							ModelState.AddModelError("FilePath","Invalid Link.");
						} else {
							filePath=filePath.Replace(fileName,"");
						}
						break;
					}
				}
				break;
			case Models.Document.UploadType.Upload:
				if(postFile==null) {
					resultModel.Result+="File is required\n";
				}
				if(postFile!=null) {
					if(string.IsNullOrEmpty(postFile.FileName)) {
						resultModel.Result+="File is required\n";
					} else {
						fileName=Path.GetFileName(postFile.FileName);
						if(String.IsNullOrEmpty(fileName)==false) {
							fileName=fileName.Replace(" ","_");
						}
						ext=Path.GetExtension(postFile.FileName).ToLower();
					}
				}
				break;
			}
			if(string.IsNullOrEmpty(ext)==false) {
				string fileTypeError=string.Empty;
				fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out fileTypeError);
				if(fileType==null) {
					resultModel.Result+=fileTypeError;
				}
			}
			uploadFileModel=new Models.File.UploadFileModel { FileName=fileName,FilePath=filePath };
			return resultModel.Result;

		}
		#endregion

		#region Import Underlying Fund Valuation

		[HttpPost]
		public ActionResult ImportExcel(FormCollection collection) {
			ImportExcelFileModel model=new ImportExcelFileModel();
			ImportExcelModel importExcelModel=new ImportExcelModel();
			importExcelModel.Tables=new List<ImportExcelTableModel>();
			this.TryUpdateModel(model);
			if(string.IsNullOrEmpty(model.FileName)) {
				ModelState.AddModelError("FileName","File Name is required");
			}
			if(ModelState.IsValid) {
				string rootPath=Server.MapPath("~/");
				string uploadFilePath=Path.Combine(model.FilePath,model.FileName);
				string errorMessage=string.Empty;
				string sessionKey=string.Empty;
				DataSet ds=ExcelConnection.GetDataSet(uploadFilePath,uploadFilePath,ref errorMessage,ref sessionKey);
				ImportExcelTableModel tableModel=null;
				if(string.IsNullOrEmpty(errorMessage)) {
					foreach(DataTable dt in ds.Tables) {
						tableModel=new ImportExcelTableModel();
						tableModel.TableName=dt.TableName;
						tableModel.TotalRows=dt.Rows.Count;
						tableModel.SessionKey=sessionKey;
						foreach(DataColumn column in dt.Columns) {
							tableModel.Columns.Add(column.ColumnName);
						}
						importExcelModel.Tables.Add(tableModel);
					}
				} else {
					importExcelModel.Result+=errorMessage;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							importExcelModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return Json(importExcelModel);
		}

		[HttpPost]
		public ActionResult ImportUnderlyingFundValuation(FormCollection collection) {
			ImportUnderlyingFundValuationModel model=new ImportUnderlyingFundValuationModel();
			ResultModel resultModel=new ResultModel();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=(PagingDataTable)ds.Tables[0];
					PagingDataTable table=importExcelTable.Skip(model.PageIndex);
					totalPages=importExcelTable.TotalPages;
					totalRows=importExcelTable.TotalRows;
					if(totalPages>model.PageIndex) {
						completedRows=(model.PageIndex*importExcelTable.PageSize);
					} else {
						completedRows=totalRows;
					}
					string underlyingFundName=string.Empty;
					string amberbrookFundName=string.Empty;
					decimal updateNAV;
					DateTime updateDate;
					DateTime effectiveDate;
					int? underlyingFundId;
					int? fundId;
					IEnumerable<ErrorInfo> errorInfo;
					foreach(DataRow row in table.Rows) {
						underlyingFundName=row.GetValue(model.UnderlyingFund);
						amberbrookFundName=row.GetValue(model.AmberbrookFund);
						decimal.TryParse(row.GetValue(model.UpdateNAV),out updateNAV);
						DateTime.TryParse(row.GetValue(model.UpdateDate),out updateDate);
						DateTime.TryParse(row.GetValue(model.EffectiveDate),out effectiveDate);
						if(string.IsNullOrEmpty(amberbrookFundName)==false
							&&string.IsNullOrEmpty(underlyingFundName)==false) {
							underlyingFundId=DealRepository.FindUnderlyingFundID(underlyingFundName);
							fundId=DealRepository.FindFundID(amberbrookFundName);
							if(underlyingFundId>0&&fundId>0) {
								CreateUnderlyingFundValuation((underlyingFundId??0),(fundId??0),updateNAV,updateDate,effectiveDate,out errorInfo);
							}
						}
					}
					resultModel.Result=string.Empty;
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
			return Json(new {
				Result=resultModel.Result, TotalRows=totalRows, CompletedRows=completedRows, TotalPages=totalPages, PageIndex=model.PageIndex
			});
		}

		#endregion

		#region Import Deal

		#region ImportDealExcel

		[HttpPost]
		public ActionResult ImportDealExcel(FormCollection collection) {
			ImportDealExcelModel model=new ImportDealExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALEXPENSEERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;

						string partnerName=string.Empty;
						string purchaseTypeName=string.Empty;

						string contactName=string.Empty;
						string contactTitle=string.Empty;
						string contactPhoneNumber=string.Empty;
						string contactEmail=string.Empty;
						string contactWebAddress=string.Empty;
						string contactNotes=string.Empty;

						string sellerTypeName=string.Empty;
						string sellerName=string.Empty;
						string sellerContactName=string.Empty;
						string sellerPhoneNumber=string.Empty;
						string sellerEmail=string.Empty;
						string sellerFax=string.Empty;
						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						Models.Entity.Contact contact=null;
						Models.Entity.SellerType sellerType=null;
						Models.Entity.PurchaseType purchaseType=null;

						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;
							contact=null;
							purchaseType=null;
							sellerType=null;


							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							partnerName=row.GetValue(model.PartnerName);
							purchaseTypeName=row.GetValue(model.PurchaseType);

							contactName=row.GetValue(model.ContactName);
							contactTitle=row.GetValue(model.ContactTitle);
							contactPhoneNumber=row.GetValue(model.ContactPhoneNumber);
							contactEmail=row.GetValue(model.ContactEmail);
							contactWebAddress=row.GetValue(model.ContactWebAddress);
							contactNotes=row.GetValue(model.ContactNotes);

							sellerTypeName=row.GetValue(model.SellerType);
							sellerName=row.GetValue(model.SellerName);
							sellerContactName=row.GetValue(model.SellerContactName);
							sellerPhoneNumber=row.GetValue(model.SellerPhoneNumber);
							sellerEmail=row.GetValue(model.SellerEmail);
							sellerFax=row.GetValue(model.SellerFax);

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							}

							if(fund==null)
								error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Fund does not exist",fundName)));

							if(error.Errors.Count()==0) {

								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal!=null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal already exist",dealName)));
										}
									}
								}

								if(string.IsNullOrEmpty(contactName)==false) {
									contact=AdminRepository.FindDealContact(contactName);
									if(contact==null) {
										contact=new Contact();
										contact.CreatedBy=Authentication.CurrentUser.UserID;
										contact.CreatedDate=DateTime.Now;
										contact.LastUpdatedBy=Authentication.CurrentUser.UserID;
										contact.LastUpdatedDate=DateTime.Now;
										contact.EntityID=Authentication.CurrentEntity.EntityID;
										contact.ContactName=contactName;
										contact.LastName="n/a";
										contact.Title=contactTitle;
										contact.Notes=contactNotes;
										AddCommunication(contact,Models.Admin.Enums.CommunicationType.Email,contactEmail);
										AddCommunication(contact,Models.Admin.Enums.CommunicationType.HomePhone,contactPhoneNumber);
										AddCommunication(contact,Models.Admin.Enums.CommunicationType.WebAddress,contactWebAddress);
										IEnumerable<ErrorInfo> contactError=AdminRepository.SaveDealContact(contact);
										if(contactError!=null) {
											contact=null;
											error.Errors.Add(new ErrorInfo(model.ContactName,ValidationHelper.GetErrorInfo(contactError)));
										}
									}
								}

								if(string.IsNullOrEmpty(purchaseTypeName)==false) {
									purchaseType=AdminRepository.FindPurchaseType(purchaseTypeName);
									if(purchaseType==null) {
										AdminRepository.SavePurchaseType(new PurchaseType {
											Name=purchaseTypeName,
											EntityID=Authentication.CurrentEntity.EntityID,
										});
										purchaseType=AdminRepository.FindPurchaseType(purchaseTypeName);
									}
								}

								if(string.IsNullOrEmpty(sellerTypeName)==false) {
									sellerType=AdminRepository.FindSellerType(sellerTypeName);
									if(sellerType==null) {
										AdminRepository.SaveSellerType(new SellerType {
											SellerType1=sellerTypeName,
											CreatedBy=Authentication.CurrentUser.UserID,
											CreatedDate=DateTime.Now,
											LastUpdatedBy=Authentication.CurrentUser.UserID,
											LastUpdatedDate=DateTime.Now,
											Enabled=true,
											EntityID=Authentication.CurrentEntity.EntityID,
										});
										sellerType=AdminRepository.FindSellerType(sellerTypeName);
									}
								}
							}

							if(error.Errors.Count()==0) {

								int? purchaseTypeID=null;
								int? contactID=null;
								int? sellerTypeID=null;
								if(purchaseType!=null)
									purchaseTypeID=purchaseType.PurchaseTypeID;
								if(contact!=null)
									contactID=contact.ContactID;
								if(sellerType!=null)
									sellerTypeID=sellerType.SellerTypeID;

								deal=new Models.Entity.Deal {
									CreatedBy=Authentication.CurrentUser.UserID,
									CreatedDate=DateTime.Now,
									DealNumber=DealRepository.GetMaxDealNumber(fund.FundID),
									EntityID=Authentication.CurrentEntity.EntityID,
									LastUpdatedBy=Authentication.CurrentUser.UserID,
									LastUpdatedDate=DateTime.Now,
									DealName=dealName,
									PurchaseTypeID=purchaseTypeID,
									IsPartnered=string.IsNullOrEmpty(partnerName),
									ContactID=contactID,
									FundID=fund.FundID,
									Partner=new Partner {
										CreatedBy=Authentication.CurrentUser.UserID,
										CreatedDate=DateTime.Now,
										EntityID=Authentication.CurrentEntity.EntityID,
										LastUpdatedBy=Authentication.CurrentUser.UserID,
										LastUpdatedDate=DateTime.Now,
										PartnerName=partnerName,
									},
								};

								if(string.IsNullOrEmpty(sellerContactName)==false) {
									deal.SellerTypeID=sellerTypeID;
									if(deal.Contact1==null) {
										deal.Contact1=new Contact();
										deal.Contact1.CreatedBy=Authentication.CurrentUser.UserID;
										deal.Contact1.CreatedDate=DateTime.Now;
									}

									deal.Contact1.EntityID=Authentication.CurrentEntity.EntityID;
									deal.Contact1.ContactName=sellerContactName;
									deal.Contact1.FirstName=sellerName;
									deal.Contact1.LastName="n/a";
									deal.Contact1.LastUpdatedBy=Authentication.CurrentUser.UserID;
									deal.Contact1.LastUpdatedDate=DateTime.Now;
									// Attempt to create communication values.

									AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.Email,sellerEmail);
									AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.Fax,sellerFax);
									AddCommunication(deal.Contact1,Models.Admin.Enums.CommunicationType.HomePhone,sellerPhoneNumber);
								}

								IEnumerable<ErrorInfo> dealError=DealRepository.SaveDeal(deal);
								if(dealError!=null) {
									error.Errors.Add(new ErrorInfo(model.DealName,ValidationHelper.GetErrorInfo(dealError)));
								}
							}
							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#region ImportDealExpenseExcel

		[HttpPost]
		public ActionResult ImportDealExpenseExcel(FormCollection collection) {
			ImportDealExpenseExcelModel model=new ImportDealExpenseExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALEXPENSEERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealExpenseTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealExpenseTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;


						decimal amount=0;
						DateTime dealExpenseDate;
						string description=string.Empty;
						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						DealClosingCostType dealClosingCostType=null;
						IEnumerable<ErrorInfo> errorInfo;
						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;
							dealClosingCostType=null;
							amount=0;
							dealExpenseDate=Convert.ToDateTime("01/01/1900");
							description=string.Empty;

							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							Decimal.TryParse(row.GetValue(model.Amount),out amount);
							DateTime.TryParse(row.GetValue(model.Date),out dealExpenseDate);
							description=row.GetValue(model.Description);

							if(string.IsNullOrEmpty(description)==false) {
								dealClosingCostType=DealRepository.FindDealClosingCostType(description);
								if(dealClosingCostType==null) {
									errorInfo=AdminRepository.SaveDealClosingCostType(new DealClosingCostType {
										Name=description,
										EntityID=Authentication.CurrentEntity.EntityID,
									});
									if(errorInfo==null) {
										dealClosingCostType=DealRepository.FindDealClosingCostType(description);
									} else {
										error.Errors.Add(new ErrorInfo(model.Description,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								}
							} else {
								error.Errors.Add(new ErrorInfo(model.Description,"Description is required"));
							}

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.Description,"Fund is required"));
							}

							if(error.Errors.Count()==0) {

								if(fund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Fund does not exist"));

								if(dealClosingCostType==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Description does not exist"));
							}

							if(error.Errors.Count()==0) {

								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal==null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal does not exist",fundName)));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								DealClosingCost dealClosingCost=DealRepository.FindDealClosingCost((deal!=null?deal.DealID:0),amount,(dealClosingCostType!=null?dealClosingCostType.DealClosingCostTypeID:0),dealExpenseDate);

								if(dealClosingCost==null) {
									// Attempt to create deal expense.
									dealClosingCost=new DealClosingCost {
										Amount=amount,
										Date=dealExpenseDate,
										DealClosingCostTypeID=(dealClosingCostType!=null?dealClosingCostType.DealClosingCostTypeID:0),
										DealID=(deal!=null?deal.DealID:0),
									};
									errorInfo=DealRepository.SaveDealClosingCost(dealClosingCost);

									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.DealName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								} else {
									error.Errors.Add(new ErrorInfo(model.Description,"Deal Expense already exist"));
								}
							}
							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#region ImportDealUnderlyingFundExcel

		[HttpPost]
		public ActionResult ImportDealUnderlyingFundExcel(FormCollection collection) {
			ImportDealUnderlyingFundExcelModel model=new ImportDealUnderlyingFundExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALUFERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealUnderlyingFundTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealUnderlyingFundTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;

						string underlyingFundName=string.Empty;
						decimal grossPurchasePrice=0;
						decimal fundNav=0;
						DateTime effectiveDate;
						decimal capitalCommitment=0;
						decimal unfundedAmount=0;
						DateTime recordDate;

						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						Models.Entity.UnderlyingFund underlyingFund=null;
						IEnumerable<ErrorInfo> errorInfo;
						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;
							underlyingFund=null;

							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							underlyingFundName=row.GetValue(model.UnderlyingFundName);
							decimal.TryParse(row.GetValue(model.GrossPurchasePrice),out grossPurchasePrice);
							decimal.TryParse(row.GetValue(model.FundNav),out fundNav);
							decimal.TryParse(row.GetValue(model.CapitalCommitment),out capitalCommitment);
							decimal.TryParse(row.GetValue(model.UnfundedAmount),out unfundedAmount);
							DateTime.TryParse(row.GetValue(model.EffectiveDate),out effectiveDate);
							DateTime.TryParse(row.GetValue(model.RecordDate),out recordDate);

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.FundName,"Fund is required"));
							}

							if(string.IsNullOrEmpty(underlyingFundName)==false) {
								underlyingFund=DealRepository.FindUnderlyingFund(underlyingFundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.UnderlyingFundName,"Underlying Fund is required"));
							}

							if(error.Errors.Count()==0) {

								if(fund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Fund does not exist"));

								if(underlyingFund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Underlying Fund does not exist"));
							}

							if(error.Errors.Count()==0) {

								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal==null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal does not exist",fundName)));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								DealUnderlyingFund dealUnderlyingFund=DealRepository
									.FindDealUnderlyingFund((deal!=null?deal.DealID:0)
									,(underlyingFund!=null?underlyingFund.UnderlyingtFundID:0)
								,grossPurchasePrice
								,effectiveDate
								,capitalCommitment
								,unfundedAmount
								,recordDate);

								if(dealUnderlyingFund==null) {

									errorInfo=SaveDealUnderlyingFund(new DealUnderlyingFundModel {
										CommittedAmount=capitalCommitment,
										DealId=(deal!=null?deal.DealID:0),
										EffectiveDate=effectiveDate,
										FundId=(fund!=null?fund.FundID:0),
										FundNAV=fundNav,
										GrossPurchasePrice=grossPurchasePrice,
										UnfundedAmount=unfundedAmount,
										RecordDate=recordDate,
										UnderlyingFundId=(underlyingFund!=null?underlyingFund.UnderlyingtFundID:0),
									},out dealUnderlyingFund);

									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.DealName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								} else {
									error.Errors.Add(new ErrorInfo(model.UnderlyingFundName,"Deal Underlying Fund already exist"));
								}
							}
							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#region ImportDealUnderlyingDirectExcel

		[HttpPost]
		public ActionResult ImportDealUnderlyingDirectExcel(FormCollection collection) {
			ImportDealUnderlyingDirectExcelModel model=new ImportDealUnderlyingDirectExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALUDERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealUnderlyingDirectTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealUnderlyingDirectTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;

						string companyName=string.Empty;
						string symbol=string.Empty;
						string securityTypeName=string.Empty;
						int noOfShares=0;
						decimal purchasePrice=0;
						decimal fairMarketValue=0;
						decimal taxCostBasisPerShare=0;
						DateTime taxCostDate;
						DateTime recordDate;


						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						Models.Entity.Issuer issuer=null;
						Models.Entity.SecurityType securityType=null;
						Models.Entity.Equity equity=null;
						Models.Entity.FixedIncome fixedIncome=null;

						IEnumerable<ErrorInfo> errorInfo;
						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;

							issuer=null;
							securityType=null;
							equity=null;
							fixedIncome=null;

							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							companyName=row.GetValue(model.CompanyName);
							symbol=row.GetValue(model.Symbol);
							securityTypeName=row.GetValue(model.SecurityType);
							int.TryParse(row.GetValue(model.NoOfShares),out noOfShares);
							decimal.TryParse(row.GetValue(model.PurchasePrice),out purchasePrice);
							decimal.TryParse(row.GetValue(model.FairMarketValue),out fairMarketValue);
							decimal.TryParse(row.GetValue(model.TaxCostBasisPerShare),out taxCostBasisPerShare);
							DateTime.TryParse(row.GetValue(model.TaxCostDate),out taxCostDate);
							DateTime.TryParse(row.GetValue(model.RecordDate),out recordDate);

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.FundName,"Fund is required"));
							}

							if(string.IsNullOrEmpty(companyName)==false) {
								issuer=DealRepository.FindIssuer(companyName);
							} else {
								error.Errors.Add(new ErrorInfo(model.CompanyName,"Company Name is required"));
							}


							if(string.IsNullOrEmpty(securityTypeName)==false) {
								securityType=AdminRepository.FindSecurityType(securityTypeName);
							} else {
								error.Errors.Add(new ErrorInfo(model.SecurityType,"SecurityType is required"));
							}

							if(string.IsNullOrEmpty(symbol)) {
								error.Errors.Add(new ErrorInfo(model.SecurityType,"SecurityType is required"));
							} else {
								if(issuer!=null&&securityType!=null) {
									if(securityType.SecurityTypeID==(int)Models.Deal.Enums.SecurityType.Equity) {
										equity=DealRepository.FindEquity(issuer.IssuerID,symbol);
										if(equity==null) {
											error.Errors.Add(new ErrorInfo(model.Symbol,"Equity does not exist"));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								if(fund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Fund does not exist"));
							}

							if(error.Errors.Count()==0) {

								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal==null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal does not exist",fundName)));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								DealUnderlyingDirect dealUnderlyingDirect=DealRepository
									.FindDealUnderlyingDirect((deal!=null?deal.DealID:0),
									(securityType!=null?(equity!=null?equity.EquityID:0):(fixedIncome!=null?fixedIncome.FixedIncomeID:0)),
									(securityType!=null?securityType.SecurityTypeID:0),
									recordDate,
									noOfShares,
									fairMarketValue,
									purchasePrice,
									taxCostBasisPerShare,
									taxCostDate
									);

								if(dealUnderlyingDirect==null) {

									errorInfo=SaveDealUnderlyingDirect(new DealUnderlyingDirectModel {
										DealId=(deal!=null?deal.DealID:0),
										FundId=(fund!=null?fund.FundID:0),
										FMV=fairMarketValue,
										IssuerId=(issuer!=null?issuer.IssuerID:0),
										NumberOfShares=noOfShares,
										PurchasePrice=purchasePrice,
										SecurityId=(securityType!=null?(equity!=null?equity.EquityID:0):(fixedIncome!=null?fixedIncome.FixedIncomeID:0)),
										SecurityTypeId=(securityType!=null?securityType.SecurityTypeID:0),
										TaxCostBase=taxCostBasisPerShare,
										TaxCostDate=taxCostDate,
										RecordDate=recordDate,
									},out dealUnderlyingDirect);

									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.CompanyName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								} else {
									error.Errors.Add(new ErrorInfo(model.CompanyName,"Deal Underlying Direct already exist"));
								}
							}
							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#endregion

		#region Import Deal Close

		#region ImportDealCloseUnderlyingFundExcel

		[HttpPost]
		public ActionResult ImportDealCloseUnderlyingFundExcel(FormCollection collection) {
			ImportDealCloseUnderlyingFundExcelModel model=new ImportDealCloseUnderlyingFundExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALCLOSEUFERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealCloseUnderlyingFundTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealCloseUnderlyingFundTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;

						string underlyingFundName=string.Empty;
						DateTime closeDate;
						decimal grossPurchasePrice=0;
						decimal netPurchasePrice=0;
						decimal capitalCommitment=0;
						decimal unfundedAmount=0;
						DateTime effectiveDate;
						DateTime recordDate;

						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						DealClosing dealClosing=null;
						Models.Entity.UnderlyingFund underlyingFund=null;
						IEnumerable<ErrorInfo> errorInfo;
						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;
							underlyingFund=null;
							dealClosing=null;

							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							underlyingFundName=row.GetValue(model.UnderlyingFundName);
							decimal.TryParse(row.GetValue(model.GrossPurchasePrice),out grossPurchasePrice);
							decimal.TryParse(row.GetValue(model.NetPurchasePrice),out netPurchasePrice);
							decimal.TryParse(row.GetValue(model.CapitalCommitment),out capitalCommitment);
							decimal.TryParse(row.GetValue(model.UnfundedAmount),out unfundedAmount);
							DateTime.TryParse(row.GetValue(model.CloseDate),out closeDate);
							DateTime.TryParse(row.GetValue(model.EffectiveDate),out effectiveDate);
							DateTime.TryParse(row.GetValue(model.RecordDate),out recordDate);

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.FundName,"Fund is required"));
							}

							if(string.IsNullOrEmpty(underlyingFundName)==false) {
								underlyingFund=DealRepository.FindUnderlyingFund(underlyingFundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.UnderlyingFundName,"Underlying Fund is required"));
							}

							if(error.Errors.Count()==0) {

								if(fund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Fund does not exist"));

								if(underlyingFund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Underlying Fund does not exist"));
							}

							if(error.Errors.Count()==0) {
								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal==null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal does not exist",fundName)));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								DealUnderlyingFund dealUnderlyingFund=DealRepository
								.FindDealUnderlyingFund((deal!=null?deal.DealID:0)
								,(underlyingFund!=null?underlyingFund.UnderlyingtFundID:0)
								,grossPurchasePrice
								,effectiveDate
								,capitalCommitment
								,unfundedAmount
								,recordDate
								);

								if(dealUnderlyingFund==null) {
									error.Errors.Add(new ErrorInfo(model.UnderlyingFundName,"Deal Underlying Fund does not exist"));
								} else {

									dealClosing=DealRepository.FindDealClosing((deal!=null?deal.DealID:0),(fund!=null?fund.FundID:0),closeDate);
									if(dealClosing==null) {
										dealClosing=new DealClosing {
											CloseDate=closeDate,
											DealID=(deal!=null?deal.DealID:0),
											DealNumber=DealRepository.GetMaxDealClosingNumber((deal!=null?deal.DealID:0)),
											IsFinalClose=false,
										};
										errorInfo=DealRepository.SaveDealClosing(dealClosing);
									}

									if(dealClosing!=null) {
										dealUnderlyingFund.DealClosingID=dealClosing.DealClosingID;
										dealUnderlyingFund.CommittedAmount=capitalCommitment;
										dealUnderlyingFund.UnfundedAmount=unfundedAmount;
										dealUnderlyingFund.GrossPurchasePrice=grossPurchasePrice;
										dealUnderlyingFund.NetPurchasePrice=netPurchasePrice;
										errorInfo=DealRepository.SaveDealUnderlyingFund(dealUnderlyingFund);
										if(errorInfo!=null) {
											error.Errors.Add(new ErrorInfo(model.DealName,ValidationHelper.GetErrorInfo(errorInfo)));
										}
									}
								}
							}

							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#region ImportDealCloseUnderlyingDirectExcel

		[HttpPost]
		public ActionResult ImportDealCloseUnderlyingDirectExcel(FormCollection collection) {
			ImportDealCloseUnderlyingDirectExcelModel model=new ImportDealCloseUnderlyingDirectExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELDEALCLOSEUDERROR_BY_KEY,model.SessionKey);
				List<ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.DealCloseUnderlyingDirectTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.DealCloseUnderlyingDirectTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}
						string fundName=string.Empty;
						string dealName=string.Empty;

						string companyName=string.Empty;
						string symbol=string.Empty;
						string securityTypeName=string.Empty;
						int noOfShares=0;
						decimal purchasePrice=0;
						decimal fairMarketValue=0;
						decimal taxCostBasisPerShare=0;
						DateTime closeDate;
						DateTime taxCostDate;
						DateTime recordDate;


						int rowNumber=0;

						Models.Entity.Fund fund=null;
						Models.Entity.Deal deal=null;
						DealClosing dealClosing=null;
						Models.Entity.Issuer issuer=null;
						Models.Entity.SecurityType securityType=null;
						Models.Entity.Equity equity=null;
						Models.Entity.FixedIncome fixedIncome=null;

						IEnumerable<ErrorInfo> errorInfo;
						ImportExcelError error;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);
							fund=null;
							deal=null;

							issuer=null;
							securityType=null;
							equity=null;
							fixedIncome=null;

							error=new ImportExcelError {
								RowNumber=rowNumber
							};
							rowErrors=new StringBuilder();
							fundName=row.GetValue(model.FundName);
							dealName=row.GetValue(model.DealName);

							companyName=row.GetValue(model.CompanyName);
							symbol=row.GetValue(model.Symbol);
							securityTypeName=row.GetValue(model.SecurityType);
							int.TryParse(row.GetValue(model.NoOfShares),out noOfShares);
							decimal.TryParse(row.GetValue(model.PurchasePrice),out purchasePrice);
							decimal.TryParse(row.GetValue(model.FairMarketValue),out fairMarketValue);
							decimal.TryParse(row.GetValue(model.TaxCostBasisPerShare),out taxCostBasisPerShare);
							DateTime.TryParse(row.GetValue(model.TaxCostDate),out taxCostDate);
							DateTime.TryParse(row.GetValue(model.RecordDate),out recordDate);
							DateTime.TryParse(row.GetValue(model.CloseDate),out closeDate);

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.FundName,"Fund is required"));
							}

							if(string.IsNullOrEmpty(companyName)==false) {
								issuer=DealRepository.FindIssuer(companyName);
							} else {
								error.Errors.Add(new ErrorInfo(model.CompanyName,"Company Name is required"));
							}


							if(string.IsNullOrEmpty(securityTypeName)==false) {
								securityType=AdminRepository.FindSecurityType(securityTypeName);
							} else {
								error.Errors.Add(new ErrorInfo(model.SecurityType,"SecurityType is required"));
							}

							if(string.IsNullOrEmpty(symbol)) {
								error.Errors.Add(new ErrorInfo(model.SecurityType,"SecurityType is required"));
							} else {
								if(issuer!=null&&securityType!=null) {
									if(securityType.SecurityTypeID==(int)Models.Deal.Enums.SecurityType.Equity) {
										equity=DealRepository.FindEquity(issuer.IssuerID,symbol);
										if(equity==null) {
											error.Errors.Add(new ErrorInfo(model.Symbol,"Equity does not exist"));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								if(fund==null)
									error.Errors.Add(new ErrorInfo(model.FundName,"Fund does not exist"));
							}

							if(error.Errors.Count()==0) {

								if(fund!=null) {
									if(string.IsNullOrEmpty(dealName)==false) {
										deal=DealRepository.FindDeal(dealName,fund.FundID);
										if(deal==null) {
											error.Errors.Add(new ErrorInfo(model.FundName,string.Format("Deal does not exist",fundName)));
										}
									}
								}
							}

							if(error.Errors.Count()==0) {

								DealUnderlyingDirect dealUnderlyingDirect=DealRepository
									.FindDealUnderlyingDirect((deal!=null?deal.DealID:0),
									(securityType!=null?(equity!=null?equity.EquityID:0):(fixedIncome!=null?fixedIncome.FixedIncomeID:0)),
									(securityType!=null?securityType.SecurityTypeID:0),
									recordDate,
									noOfShares,
									fairMarketValue,
									purchasePrice,
									taxCostBasisPerShare,
									taxCostDate
									);

								if(dealUnderlyingDirect==null) {
									error.Errors.Add(new ErrorInfo(model.CompanyName,"Deal Underlying Direct already exist"));
								} else {

									dealClosing=DealRepository.FindDealClosing((deal!=null?deal.DealID:0),(fund!=null?fund.FundID:0),closeDate);
									if(dealClosing==null) {

										dealClosing=new DealClosing {
											CloseDate=closeDate,
											DealID=(deal!=null?deal.DealID:0),
											DealNumber=DealRepository.GetMaxDealClosingNumber((deal!=null?deal.DealID:0)),
											IsFinalClose=false,
										};

										errorInfo=DealRepository.SaveDealClosing(dealClosing);
									}

									if(dealClosing!=null) {
										dealUnderlyingDirect.DealClosingID=dealClosing.DealClosingID;
										dealUnderlyingDirect.NumberOfShares=noOfShares;
										dealUnderlyingDirect.PurchasePrice=purchasePrice;
										dealUnderlyingDirect.FMV=fairMarketValue;
										errorInfo=DealRepository.SaveDealUnderlyingDirect(dealUnderlyingDirect);
										if(errorInfo!=null) {
											error.Errors.Add(new ErrorInfo(model.DealName,ValidationHelper.GetErrorInfo(errorInfo)));
										}
									}
								}
							}
							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
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
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#endregion

		public ActionResult GetImportErrorExcel(string sessionKey,string tableName) {
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			ActionResult result=null;
			DataSet ds=ExcelConnection.ImportExcelDataset(sessionKey);
			if(ds!=null) {
				PagingDataTable importExcelTable=null;
				if(ds.Tables[tableName]!=null) {
					importExcelTable=(PagingDataTable)ds.Tables[tableName];
				}
				PagingDataTable errorTable=(PagingDataTable)importExcelTable.Clone();
				DataRow[] rows=importExcelTable.Select("ImportError<>''");
				foreach(var row in rows) {
					errorTable.ImportRow(row);
				}
				//if(importExcelTable!=null) {
					result=new ExportExcel {
						TableName=string.Format("{0}_{1}",tableName,sessionKey), GridData=errorTable
					};
				//}
			}
			return result;
		}

		private string AppendErrorInfo(int rowNumber,DataTable excelDataTable,List<ImportExcelError> errors) {
			StringBuilder errorInfo=new StringBuilder();
			errorInfo.Append("<table cellpadding=0 cellspacing=0 border=0>");
			errorInfo.Append("<thead>");
			errorInfo.Append("<tr>");
			errorInfo.AppendFormat("<th>{0}</th>","RowNumber");
			foreach(DataColumn col in excelDataTable.Columns) {
				errorInfo.AppendFormat("<th>{0}</th>",col.ColumnName);
			}
			errorInfo.Append("</tr>");
			errorInfo.Append("</thead>");
			errorInfo.Append("</tbody>");
			if(errors.Count()>0) {
				foreach(var err in errors) {
					if(err.Errors.Count()>0) {
						errorInfo.Append("<tr>");
						errorInfo.AppendFormat("<td>{0}</td>",err.RowNumber);
						foreach(DataColumn col in excelDataTable.Columns) {
							var errorColumn=err.Errors.Where(e => e.PropertyName==col.ColumnName).FirstOrDefault();
							if(errorColumn!=null) {
								errorInfo.AppendFormat("<td>{0}</td>",errorColumn.ErrorMessage);
							} else {
								errorInfo.AppendFormat("<td>{0}</td>",string.Empty);
							}
						}
						errorInfo.Append("</tr>");
					}
				}
			}
			errorInfo.Append("</tbody>");
			errorInfo.Append("</table>");
			return errorInfo.ToString();
		}

		#region Broker
		//
		// GET: /Deal/FindBrokers
		[HttpGet]
		public JsonResult FindBrokers(string term) {
			return Json(AdminRepository.FindBrokers(term),JsonRequestBehavior.AllowGet);
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}

