using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundListModel {

		public int UnderlyingFundId { get; set; }

		public string FundName { get; set; }

		public string FundType { get; set; }

		public string IssuerName { get; set; }
	}
}