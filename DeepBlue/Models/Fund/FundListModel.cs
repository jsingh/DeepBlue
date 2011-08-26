using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {
	
	public class FundListModel {

		public int FundId { get; set; }

		public string FundName { get; set; }
		
		public string TaxId { get; set; }

		public decimal? CommitmentAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public DateTime? InceptionDate { get; set; }

		public DateTime? ScheduleTerminationDate { get; set; }
	}
 
}