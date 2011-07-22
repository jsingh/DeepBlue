using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {

	public class FundRateScheduleDetail {

		public FundRateScheduleDetail() {
			FundRateScheduleTiers = new List<FundRateScheduleTierDetail>();
		}
	
		public int FundRateScheduleId { get; set; }

		public int FundId { get; set; }

		public int InvestorTypeId { get; set; }

		public int RateScheduleId { get; set; }

		public int RateScheduleTypeId { get; set; }

		public List<FundRateScheduleTierDetail> FundRateScheduleTiers { get; set; }
	}

}