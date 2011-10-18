using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {
	public class InvestorListModel {

		public string InvestorName { get; set; }

		public decimal? CommittedAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public DateTime? CloseDate { get; set; }
	}
}