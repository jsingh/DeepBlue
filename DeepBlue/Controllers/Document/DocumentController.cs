using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Document;
using DeepBlue.Helpers;
using System.IO;
using DeepBlue.Models.Entity;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using DeepBlue.Controllers.Admin;
using DeepBlue.Models.Admin;

namespace DeepBlue.Controllers.Document {

	[OtherEntityAuthorize]
	public class DocumentController:BaseController {

		public IDocumentRepository DocumentRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public DocumentController()
			: this(new DocumentRepository(),new AdminRepository()) {
		}

		public DocumentController(IDocumentRepository repository,IAdminRepository adminRepository) {
			DocumentRepository=repository;
			AdminRepository=adminRepository;
		}

		//
		// GET: /Document/New
		public ActionResult New() {
			ViewData["MenuName"]="Document";
			ViewData["SubmenuName"]="";
			ViewData["PageName"]="NewDocument";
			CreateModel model=new CreateModel();
			model.DocumentTypes=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
			model.DocumentStatusTypes=SelectListFactory.GetDocumentStatusList();
			model.DocumentStatus=(int)DocumentStatus.Investor;
			model.UploadTypes=SelectListFactory.GetUploadTypeSelectList();
			return View(model);
		}

		//
		// POST: /Document/Create
		[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(FormCollection collection) {
			CreateModel model=new CreateModel();
			this.TryUpdateModel(model);
			int fileTypeId=0;
			string fileName=string.Empty;
			string ext=string.Empty;
			string filePath=string.Empty;
			ResultModel resultModel=new ResultModel();
			int? investorId=null;
			int? fundId=null;
			switch(model.DocumentStatus) {
			case (int)DocumentStatus.Investor:
				if(model.InvestorId==0)
					ModelState.AddModelError("InvestorId","Investor is required.");
				else
					investorId=model.InvestorId;
				break;
			case (int)DocumentStatus.Fund:
				if(model.FundId==0)
					ModelState.AddModelError("FundId","Fund is required.");
				else
					fundId=model.FundId;
				break;
			}
			switch((UploadType)model.UploadType) {
			case UploadType.Link:
				if(string.IsNullOrEmpty(model.FilePath)) {
					ModelState.AddModelError("FilePath","Link is required.");
				} else {
					if(UploadFileHelper.CheckFilePath(model.FilePath)==false) {
						ModelState.AddModelError("FilePath","Invalid Link.");
					} else {
						fileName=Path.GetFileName(model.FilePath);
						ext=Path.GetExtension(model.FilePath);
						filePath=model.FilePath.Replace(fileName,"");
						if((filePath.ToLower().StartsWith("http://")==false)&&
						   (filePath.ToLower().StartsWith("https://")==false)) {
							filePath="http://"+filePath;
						}
					}
					model.File=null;
				}
				break;
			case UploadType.Upload:
				if(model.File!=null) {
					ext=Path.GetExtension(model.File.FileName).ToLower();
				} else {
					ModelState.AddModelError("File","File is required");
				}
				break;
			}
			Models.Entity.FileType fileType=null;
			if(string.IsNullOrEmpty(ext)==false) {
				string errorMessage=string.Empty;
				fileType=UploadFileHelper.CheckFileExtension(AdminRepository.GetAllFileTypes(),ext,out errorMessage);
				if(fileType==null) {
					ModelState.AddModelError("File",errorMessage);
				}
			}
			if(ModelState.IsValid&&fileType!=null) {

				fileTypeId=fileType.FileTypeID;
				InvestorFundDocument investorFundDocument=new InvestorFundDocument();
				investorFundDocument.CreatedBy=Authentication.CurrentUser.UserID;
				investorFundDocument.CreatedDate=DateTime.Now;
				investorFundDocument.DocumentDate=DateTime.Now;
				investorFundDocument.DocumentTypeID=model.DocumentTypeId;
				investorFundDocument.EntityID=Authentication.CurrentEntity.EntityID;
				investorFundDocument.LastUpdatedBy=Authentication.CurrentUser.UserID;
				investorFundDocument.LastUpdatedDate=DateTime.Now;
				investorFundDocument.InvestorID=investorId;
				investorFundDocument.FundID=fundId;

				investorFundDocument.File=new Models.Entity.File();
				if(model.File!=null) {
					string appSettingName=string.Empty;
					int? key=0;
					if(investorId!=null) {
						key=investorId;
						appSettingName="InvestorDocumentUploadPath";
					} else {
						key=fundId;
						appSettingName="FundDocumentUploadPath";
					}
					DeepBlue.Models.File.UploadFileModel uploadFileModel=UploadFileHelper.Upload(model.File,appSettingName,Authentication.CurrentEntity.EntityID,key,model.DocumentTypeId,Path.GetFileName(model.File.FileName));
					investorFundDocument.File.FileName=uploadFileModel.FileName;
					investorFundDocument.File.FilePath=uploadFileModel.FilePath;
					investorFundDocument.File.Size=uploadFileModel.Size;
				} else {
					investorFundDocument.File.FilePath=filePath;
					investorFundDocument.File.FileName=fileName;
				}
				investorFundDocument.File.FileTypeID=fileTypeId;
				investorFundDocument.File.CreatedBy=Authentication.CurrentUser.UserID;
				investorFundDocument.File.CreatedDate=DateTime.Now;
				investorFundDocument.File.EntityID=Authentication.CurrentEntity.EntityID;
				investorFundDocument.File.LastUpdatedBy=Authentication.CurrentUser.UserID;
				investorFundDocument.File.LastUpdatedDate=DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo=investorFundDocument.Save();
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			resultModel.Result=JsonSerializer.ToJsonObject(new { error=string.Empty,data=resultModel.Result }).ToString();
			return View("Result",resultModel);
		}



		//
		// GET: /Document/Search
		[HttpGet]
		public ActionResult Search() {
			ViewData["MenuName"]="Document";
			ViewData["SubmenuName"]="";
			ViewData["PageName"]="DocumentSearch";
			SearchModel model=new SearchModel();
			model.DocumentTypes=SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes((int)DeepBlue.Models.Admin.Enums.DocumentSection.Investor));
			model.DocumentStatusTypes=SelectListFactory.GetDocumentStatusList();
			model.DocumentStatus=(int)DocumentStatus.Investor;
			return View(model);
		}

		//
		// GET: /Document/Search
		[HttpGet]
		public ActionResult List(int pageIndex,int pageSize,string sortName,string sortOrder,string fromDate,string toDate,int investorId,int fundId,int documentTypeId,int documentStatusId) {
			DateTime documentFromDate;
			DateTime documentToDate;
			DocumentStatus documentStatus=(DocumentStatus)documentStatusId;
			int totalRows=0;
			if(string.IsNullOrEmpty(fromDate)==false)
				documentFromDate=Convert.ToDateTime(fromDate);
			else
				documentFromDate=Convert.ToDateTime("01/01/1900");
			if(string.IsNullOrEmpty(toDate)==false)
				documentToDate=Convert.ToDateTime(toDate);
			else
				documentToDate=DateTime.MaxValue;
			List<DocumentDetail> documentDetails=DocumentRepository.FindDocuments(pageIndex,pageSize,sortName,sortOrder,documentFromDate,documentToDate,investorId,fundId,documentTypeId,documentStatus,ref totalRows);
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(documentDetails);
		}

		public ActionResult Result() {
			return View();
		}
	}
}
