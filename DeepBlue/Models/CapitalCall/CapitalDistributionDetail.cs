using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {

	public class CapitalDistributionDetail : CapitalDistributionInvestorDetail {

		public int CapitalDistrubutionId { get; set; }

		public string Number { get; set; }
	
	}
}