using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class ManagementFeeDetail {
		public ManagementFeeDetail() {
			ManagementFee = 0;
		}
		public decimal ManagementFee { get; set; }

		public FlexigridData Tiers { get; set; }
	}
	 
}