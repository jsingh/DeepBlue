using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {

	public class ActivityDealModel {

		public int DealId { get; set; }

		public int FundId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public decimal? CommitmentAmount { get; set; }

		public decimal? CallAmount { get; set; }
	}
}