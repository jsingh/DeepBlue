using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.CapitalCall {
	public class FundDetail {
		
		public int FundId { get; set; }

		public string FundName { get; set; }

		public string TotalCommitment { get; set; }

		public string UnfundedAmount { get; set; }

		public string CapitalCallNumber { get; set; }

	}
}