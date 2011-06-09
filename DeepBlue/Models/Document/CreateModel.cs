using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Document {
	public class CreateModel {
		public CreateModel(){
			UploadType = (int)Document.UploadType.Upload;
		}	
		/* Investor Document Module */
		
		public int InvestorId { get; set; }
		
		public int FundId { get; set; }
		
		[DisplayName("Investor:")]
		public string InvestorName { get; set; }

		[DisplayName("Fund:")]
		public string FundName { get; set; }
	 
		[DisplayName("File:")]
		public HttpPostedFileBase File { get; set; }

		public int DocumentStatus { get; set; }

		[Required(ErrorMessage = "Document Type is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Type is required.")]
		[DisplayName("Document Type:")]
		public int DocumentTypeId { get; set; }

		[Required(ErrorMessage = "Document Date is required.")]
		[DisplayName("Document Date:")]
        [DateRange()]
		public DateTime DocumentDate { get; set; }

		public string ModelErrorMessage { get; set; }

		public int UploadType { get; set; }
		
		[DisplayName("Link:")]
		public string FilePath { get; set; }

		/* Document Type */ 

		public List<SelectListItem> DocumentTypes { get; set; }

		/* Document Staus Type */
		public List<SelectListItem> DocumentStatusTypes { get; set; }

		/* Update Type */
		public List<SelectListItem> UploadTypes { get; set; }

	}

	public class FileDetailModel {
		public FileDetailModel() {
			ErrorMessage = string.Empty;
			FileName = string.Empty;
			FilePath = string.Empty;
			Size = 0;
		}		

		public string ErrorMessage { get; set; }

		public string FileName { get; set; }

		public string FilePath { get; set; }

		public long Size { get; set; }
	}
}