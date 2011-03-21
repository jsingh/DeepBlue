using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {
	public class FundRateScheduleDetail {
	
		public int FundRateScheduleId { get; set; }

		public int FundId { get; set; }

		public int InvestorTypeId { get; set; }

		public int RateScheduleId { get; set; }

		public int RateScheduleTypeId { get; set; }
		
		public List<FundRateScheduleTier> FundRateScheduleTiers { get; set; }
	}

	public class FundRateScheduleTier{

		public string Notes { get; set; }

		public int ManagementFeeRateScheduleId { get; set; }

		public int ManagementFeeRateScheduleTierId { get; set; }
		
		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public Decimal Rate { get; set; }

		public Decimal FlatFee { get; set; }

		public int MultiplierTypeId { get; set; }
	}
}