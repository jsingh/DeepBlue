using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundDocumentModel {

		public int UnderlyingFundDocumetId { get; set; }
		
		[Required(ErrorMessage = "Underlying Fund is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required.")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Document Type is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Type is required.")]
		public int DocumentTypeId { get; set; }

		[Required(ErrorMessage = "Document Date is required.")]
		[DateRange(ErrorMessage = "Document Date is required.")]
		public DateTime DocumentDate { get; set; }

		[Required(ErrorMessage = "Upload Type is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Upload Type is required.")]
		public int UploadTypeId { get; set; }

		public string FilePath { get; set; }

	}
}