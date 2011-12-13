using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class DealListModel {

		public int DealId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public string FundName { get; set; }

	}

	public class DealFundListModel {

		public int FundID  { get; set; }

		public string FundName { get; set; }

		public int? DealCount  { get; set; }
	}
}