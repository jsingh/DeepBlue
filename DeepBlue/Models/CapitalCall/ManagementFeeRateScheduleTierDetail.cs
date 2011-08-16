using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.CapitalCall {
	public class ManagementFeeRateScheduleTierDetail {

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public decimal Multiplier { get; set; }

		public int MultiplierTypeId { get; set; }

	}
}