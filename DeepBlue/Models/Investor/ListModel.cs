using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Investor {
	public class ListModel {
		public List<InvestorDetail> InvestorDetails = new List<InvestorDetail>();
	}
	public class InvestorDetail {
		public string InvestorName { get; set; }
		public int InvestorId { get; set; }
        public string Social { get; set; }
	}
}