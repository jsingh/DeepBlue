using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {

	public class CapitalCallDetail : CapitalCallInvestorDetail {

		public int CapitalCallId { get; set; }

		public string Number { get; set; }

	}
}