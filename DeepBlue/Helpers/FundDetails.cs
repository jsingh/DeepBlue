using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {

	public class FundLists {

		public int TotalPages { get; set; }

		public int PageNo { get; set; }

		public List<FundList> FundDetails { get; set; }
	}

	public class FundList {

		public string FundName { get; set; }

		public int FundId { get; set; }
	}
}