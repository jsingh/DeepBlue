using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Document {
	public class UploadModel {

		/* Investor Document Module */
		
		public int InvestorId { get; set; }
		
		public int FundId { get; set; }
		
		[DisplayName("Investor:")]
		public string InvestorName { get; set; }

		[DisplayName("Fund:")]
		public string FundName { get; set; }
	 
		// Reference to the uploaded file
		[Required(ErrorMessage = "File is required.")]
		[DisplayName("File:")]
		public HttpPostedFileBase FileName { get; set; }

		public int DocumentStatus { get; set; }

		[Required(ErrorMessage = "Document Type is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Type is required.")]
		[DisplayName("Document Type:")]
		public int DocumentTypeId { get; set; }

		[Required(ErrorMessage = "Document Date is required.")]
		[DisplayName("Document Date:")]
		public DateTime DocumentDate { get; set; }
		
		public string ModelErrorMessage { get; set; }

		/* Document Type */

		public List<SelectListItem> DocumentTypes { get; set; }

		/* Document Staus Type */
		public List<SelectListItem> DocumentStatusTypes { get; set; }

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