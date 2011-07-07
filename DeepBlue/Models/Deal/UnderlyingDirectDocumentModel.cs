using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class UnderlyingDirectDocumentModel {
		
		public int DocumentTypeId { get; set; }

		public DateTime DocumentDate { get; set; }

		public int UploadTypeId { get; set; }

		public string FilePath { get; set; }

		public HttpPostedFileBase File { get; set; }

		public int SecurityId { get; set; }

		public int SecurityTypeId { get; set; }
	}
}