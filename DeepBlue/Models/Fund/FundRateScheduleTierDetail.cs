using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund {

	public class FundRateScheduleTierDetail {

		public string Notes { get; set; }

		public int ManagementFeeRateScheduleId { get; set; }

		public int ManagementFeeRateScheduleTierId { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public Decimal? Rate { get; set; }

		public Decimal? FlatFee { get; set; }

		public int MultiplierTypeId { get; set; }
	}

}