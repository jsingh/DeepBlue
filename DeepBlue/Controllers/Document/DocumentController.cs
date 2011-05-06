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

namespace DeepBlue.Controllers.Document {
	public class DocumentController : Controller {

		public IDocumentRepository DocumentRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public DocumentController()
			: this(new DocumentRepository(), new AdminRepository()) {
		}

		public DocumentController(IDocumentRepository repository, IAdminRepository adminRepository) {
			DocumentRepository = repository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Document/New
		public ActionResult New() {
			ViewData["MenuName"] = "Document";
			ViewData["SubmenuName"] = "";
			ViewData["PageName"] = "NewDocument";
			CreateModel model = new CreateModel();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.DocumentStatusTypes = SelectListFactory.GetDocumentStatusList();
			model.DocumentStatus = (int)DocumentStatus.Investor;
			model.UploadTypes = SelectListFactory.GetUploadTypeSelectList();
			return View(model);
		}


		//
		// POST: /Document/Create
		[HttpPost]
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			this.TryUpdateModel(model);
			int fileTypeId = 0;
			string fileName = string.Empty;
			string ext = string.Empty;
			string filePath = string.Empty;
			UploadFile fileUpload = null;
			if (ModelState.IsValid) {
				int? investorId = null;
				int? fundId = null;
				switch (model.DocumentStatus) {
					case (int)DocumentStatus.Investor:
						if (model.InvestorId == 0) {
							ModelState.AddModelError("InvestorId", "Investor is required.");
						}
						else {
							investorId = model.InvestorId;
						}
						break;
					case (int)DocumentStatus.Fund:
						if (model.FundId == 0) {
							ModelState.AddModelError("FundId", "Fund is required.");
						}
						else {
							fundId = model.FundId;
						}
						break;
				}
				switch ((UploadType)model.UploadType) {
					case UploadType.Link:
						Regex regex = new Regex(
									@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)"
									+ @"*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$",
									RegexOptions.IgnoreCase
									| RegexOptions.Multiline
									| RegexOptions.IgnorePatternWhitespace
									| RegexOptions.Compiled
									);
						if (regex.IsMatch(model.FilePath) == false) {
							ModelState.AddModelError("FilePath", "Invalid Link.");
						}
						else {
							fileName = Path.GetFileName(model.FilePath);
							ext = Path.GetExtension(model.FilePath);
							filePath = model.FilePath.Replace(fileName, "");
							if (filePath.ToLower().StartsWith("http://") == false) {
								filePath = "http://" + filePath;
							}
						}
						model.File = null;
						break;
					case UploadType.Upload:
						if (model.File != null) {
							ext = Path.GetExtension(model.File.FileName).ToLower();
						}
						else {
							ModelState.AddModelError("File", "File is required");
						}
						break;
				}
				if (ModelState.IsValid) {
					string errorMessage = string.Empty;
					Models.Entity.FileType fileType = CheckFileExtension(ext, out errorMessage);
					if (fileType == null) {
						ModelState.AddModelError("File", errorMessage);
					}
					else {
						fileTypeId = fileType.FileTypeID;
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
						if (model.File != null) {
							fileUpload = new UploadFile(model.File, "DocumentUploadPath", (int)ConfigUtil.CurrentEntityID, (investorId != null ? investorId : fundId), model.DocumentTypeId, Path.GetFileName(model.File.FileName));
							fileUpload.Upload();
							investorFundDocument.File.FileName = fileUpload.FileName;
							investorFundDocument.File.FilePath = fileUpload.FilePath;
							investorFundDocument.File.Size = fileUpload.Size;
						}
						else {
							investorFundDocument.File.FilePath = filePath;
							investorFundDocument.File.FileName = fileName;
						}
						investorFundDocument.File.FileTypeID = fileTypeId;
						investorFundDocument.File.CreatedBy = AppSettings.CreatedByUserId;
						investorFundDocument.File.CreatedDate = DateTime.Now;
						investorFundDocument.File.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorFundDocument.File.LastUpdatedBy = AppSettings.CreatedByUserId;
						investorFundDocument.File.LastUpdatedDate = DateTime.Now;
						IEnumerable<ErrorInfo> errorInfo = investorFundDocument.Save();
						StringBuilder errors = new StringBuilder();
						if (errorInfo != null) {
							foreach (var err in errorInfo.ToList()) {
								errors.Append(err.PropertyName + " : " + err.ErrorMessage + "\n");
							}
							if (string.IsNullOrEmpty(errors.ToString()) == false) {
								ModelState.AddModelError("ModelErrorMessage", errors.ToString());
							}
						}
						else {
							model.InvestorId = 0;
							model.FundId = 0;
							model.DocumentTypeId = 0;
							model.DocumentStatus = (int)DocumentStatus.Investor;
							model.DocumentDate = DateTime.Now;
							model.ModelErrorMessage = string.Empty;
						}
					}
				}
			}
			ViewData["MenuName"] = "Document";
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
			model.DocumentStatusTypes = SelectListFactory.GetDocumentStatusList();
			model.UploadTypes = SelectListFactory.GetUploadTypeSelectList();
			if (ModelState.IsValid) {
				model.DocumentStatus = (int)DocumentStatus.Investor;
				return RedirectToAction("New");
			}
			else {
				return View("New", model);
			}
		}

		private Models.Entity.FileType CheckFileExtension(string extension, out string errorMessage) {
			List<Models.Entity.FileType> fileTypes = AdminRepository.GetAllFileTypes();
			Models.Entity.FileType fileType = null;
			errorMessage = "*.";
			foreach (var type in fileTypes) {
				errorMessage += type.FileExtension + ",";
				if (fileType == null) {
					var arrExtensions = type.FileExtension.Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					foreach (var ext in arrExtensions) {
						if (ext.ToLower() == extension.Replace(".", "").ToLower()) {
							fileType = type;
							break;
						}
					}
				}
				else {
					break;
				}
			}
			if (fileType == null) {
				errorMessage += "  files only allowed";
			}
			return fileType;
		}

		//
		// GET: /Document/Search
		[HttpGet]
		public ActionResult Search() {
			ViewData["MenuName"] = "Document";
			ViewData["SubmenuName"] = "";
			ViewData["PageName"] = "DocumentSearch";
			SearchModel model = new SearchModel();
			model.DocumentTypes = SelectListFactory.GetDocumentTypeSelectList(AdminRepository.GetAllDocumentTypes());
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

	}
}
