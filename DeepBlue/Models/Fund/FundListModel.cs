using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {
	
	public class FundListModel {

		public int FundId { get; set; }

		public string FundName { get; set; }
		
		public string TaxId { get; set; }

		public DateTime? FundStartDate { get; set; }

		public DateTime? ScheduleTerminationDate { get; set; }
	}
	

}