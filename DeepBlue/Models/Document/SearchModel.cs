using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Document {
	public class SearchModel {

		[DisplayName("Date:")]
		public DateTime FromDate { get; set; }

		[DisplayName("To")]
		public DateTime ToDate { get; set; }

		public int DocumentStatus { get; set; }

		public string Search { get; set; }

		public int InvestorId { get; set; }

		public int FundId { get; set; }

		[DisplayName("Investor:")]
		public string InvestorName { get; set; }

		[DisplayName("Fund:")]
		public string FundName { get; set; }
		
		/* Document Type */

		public List<SelectListItem> DocumentTypes { get; set; }

		/* Document Staus Type */

		public List<SelectListItem> DocumentStatusTypes { get; set; }
	}
}