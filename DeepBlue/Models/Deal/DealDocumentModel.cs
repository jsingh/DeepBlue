using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class DealDocumentModel {
		[DisplayName("DocumentType-")]
		public int DocumentTypeID { get; set; }

		[DisplayName("Fund-")]
		public int FundID { get; set; }

		[DisplayName("Document Date-")]
		public DateTime DocumentDate { get; set; }

		[DisplayName("File:")]
		public HttpPostedFileBase File { get; set; }
	}
}