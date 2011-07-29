using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {

	public class DealDocumentModel {

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Document Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Type is required")]
		public int DocumentTypeId { get; set; }

		[Required(ErrorMessage = "Document For is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document For is required")]
		public int DocumentStatusId { get; set; }

		[Required(ErrorMessage = "Upload Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Upload Type is required")]
		public int DocumentUploadTypeId { get; set; }

		public int? DealFundDocumentId { get; set; }

		public int? DocumentInvestorId { get; set; }

		public int? DocumentFundId { get; set; }

		[Required(ErrorMessage = "Document Date is required")]
		[DateRange()]
		public DateTime DocumentDate { get; set; }
		
		public HttpPostedFileBase DocumentFile { get; set; }
		
		public string DocumentFilePath { get; set; }

	}
}