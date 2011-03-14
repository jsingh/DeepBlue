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

namespace DeepBlue.Controllers.Document {
	public class DocumentController : Controller {

		public IDocumentRepository DocumentRepository { get; set; }

		public DocumentController()
			: this(new DocumentRepository()) {
		}

		public DocumentController(IDocumentRepository repository) {
			DocumentRepository = repository;
		}

		//
		// GET: /Document/New

		public ActionResult New() {
			ViewData["MenuName"] = "Document";
			UploadModel model = new UploadModel();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(DocumentRepository.GetAllDocumentTypes());
			model.DocumentStatusTypes = SelectListFactory.GetDocumentStatusList();
			model.DocumentStatus = (int)DocumentStatus.Investor;
			return View(model);
		}

		//
		// POST: /Document/Create

		[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(FormCollection collection) {
			UploadModel model = new UploadModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				if (model.File != null) {
					string ext = Path.GetExtension(model.File.FileName).ToLower();
					int? investorId = null;
					int? fundId = null;
					bool validate = true;
					switch (model.DocumentStatus) {
						case (int)DocumentStatus.Investor:
							if (model.InvestorId == 0) {
								ModelState.AddModelError("InvestorId", "Investor is required.");
								validate = false;
							} else {
								investorId = model.InvestorId;
							}
							break;
						case (int)DocumentStatus.Fund:
							if (model.FundId == 0) {
								ModelState.AddModelError("FundId", "Fund is required.");
								validate = false;
							} else {
								fundId = model.FundId;
							}
							break;
					}
					if (ext != ".pdf" && ext != ".doc" && ext != ".docx" && ext != ".xls" && ext != ".xlsx") {
						ModelState.AddModelError("FileName", "*.pdf,doc,docx,xls,xlsx files only allowed");
						validate = false;
					}
					if (validate) {
						int fileTypeId = 0;
						switch (ext) {
							case ".pdf":
								fileTypeId = (int)DeepBlue.Models.Document.FileType.PDF;
								break;
							case ".doc":
								fileTypeId = (int)DeepBlue.Models.Document.FileType.Word;
								break;
							case ".docx":
								fileTypeId = (int)DeepBlue.Models.Document.FileType.Word;
								break;
							case ".xls":
								fileTypeId = (int)DeepBlue.Models.Document.FileType.Excel;
								break;
							case ".xlsx":
								fileTypeId = (int)DeepBlue.Models.Document.FileType.Excel;
								break;
						}
						UploadFile fileUpload = new UploadFile(model.File, "DocumentUploadPath", (int)ConfigUtil.CurrentEntityID, (investorId != null ? investorId : fundId), model.DocumentTypeId, Path.GetFileName(model.File.FileName));
						if (fileUpload.Upload()) {
							InvestorFundDocument investorFundDocument = new InvestorFundDocument();
							investorFundDocument.CreatedBy = AppSettings.CreatedByUserId;
							investorFundDocument.CreatedDate = DateTime.Now;
							investorFundDocument.DocumentDate = model.DocumentDate;
							investorFundDocument.DocumentTypeID = model.DocumentTypeId;
							investorFundDocument.EntityID = (int)ConfigUtil.CurrentEntityID;
							investorFundDocument.LastUpdatedBy = AppSettings.CreatedByUserId;
							investorFundDocument.LastUpdatedDate = DateTime.Now;
							investorFundDocument.InvestorID = investorId;
							investorFundDocument.FundID = fundId;

							investorFundDocument.File = new Models.Entity.File();
							investorFundDocument.File.FileName = fileUpload.FileName;
							investorFundDocument.File.FilePath = fileUpload.FilePath;
							investorFundDocument.File.FileTypeID = fileTypeId;
							investorFundDocument.File.CreatedBy = AppSettings.CreatedByUserId;
							investorFundDocument.File.CreatedDate = DateTime.Now;
							investorFundDocument.File.EntityID = (int)ConfigUtil.CurrentEntityID;
							investorFundDocument.File.LastUpdatedBy = AppSettings.CreatedByUserId;
							investorFundDocument.File.LastUpdatedDate = DateTime.Now;
							investorFundDocument.File.Size = fileUpload.Size;
							IEnumerable<ErrorInfo> errorInfo = investorFundDocument.Save();
							StringBuilder errors = new StringBuilder();
							if (errorInfo != null) {
								foreach (var err in errorInfo.ToList()) {
									errors.Append(err.PropertyName + " : " + err.ErrorMessage + "\n");
								}
								if (string.IsNullOrEmpty(errors.ToString()) == false) {
									ModelState.AddModelError("ModelErrorMessage", errors.ToString());
								}
							} else {
								model.InvestorId = 0;
								model.FundId = 0;
								model.DocumentTypeId = 0;
								model.DocumentStatus = (int)DocumentStatus.Investor;
								model.DocumentDate = DateTime.Now;
								model.ModelErrorMessage = string.Empty;
							}
						}
					}
				} else {
					ModelState.AddModelError("FileName", "File is required");
				}
			}
			ViewData["MenuName"] = "Document";
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(DocumentRepository.GetAllDocumentTypes());
			model.DocumentStatusTypes = SelectListFactory.GetDocumentStatusList();
			if (ModelState.IsValid) {
				model.DocumentStatus = (int)DocumentStatus.Investor;
				return RedirectToAction("New", model);
			} else {
				return View("New", model);
			}
		}

		//
		// GET: /Document/Search
		[HttpGet]
		public ActionResult Search() {
			ViewData["MenuName"] = "Document";
			SearchModel model = new SearchModel();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(DocumentRepository.GetAllDocumentTypes());
			model.DocumentStatusTypes = SelectListFactory.GetDocumentStatusList();
			model.DocumentStatus = (int)DocumentStatus.Investor;
			return View(model);
		}

		//
		// GET: /Document/Search
		[HttpGet]
		public ActionResult List(int pageIndex, int pageSize, string sortName, string sortOrder, string fromDate, string toDate, int investorId, int fundId, int documentTypeId, int documentStatusId) {
			DateTime documentFromDate;
			DateTime documentToDate;
			DocumentStatus documentStatus = (DocumentStatus)documentStatusId;
			int totalRows = 0;
			if (string.IsNullOrEmpty(fromDate) == false)
				documentFromDate = Convert.ToDateTime(fromDate);
			else
				documentFromDate = Convert.ToDateTime("01/01/1900");
			if (string.IsNullOrEmpty(toDate) == false)
				documentToDate = Convert.ToDateTime(toDate);
			else
				documentToDate = DateTime.Now;
			IList<DocumentDetail> documentDetails = DocumentRepository.FindDocuments(pageIndex, pageSize, sortName, sortOrder, documentFromDate, documentToDate, investorId, fundId, documentTypeId, documentStatus, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(documentDetails);
		}

		[HttpGet]
		public ActionResult DownloadDocument(string filePath, string fileName) {
			return new DownloadFile { VirtualPath = filePath, FileDownloadName = fileName };
		}
	}
}
