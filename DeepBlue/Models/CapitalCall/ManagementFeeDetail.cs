using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class ManagementFeeDetail {

		public ManagementFeeDetail() {
			ManagementFee = 0;
			InvestorId = 0;
		}

		public int InvestorId { get; set; }

		public decimal ManagementFee { get; set; }

	}
	 
}