using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DeepBlue.Models.CapitalCall {
	public class ListModel {
		
		public int FundId { get; set; }

		[DisplayName("Fund")]
		public string FundName { get; set; }
	}
}