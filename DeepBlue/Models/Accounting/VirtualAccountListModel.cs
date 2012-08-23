using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Accounting {
	public class VirtualAccountListModel {

		public int VirtualAccountID { get; set; }

		public string FundName { get; set; }

		public string AccountName { get; set; }

		public string ParentVirtualAccountName { get; set; }

		public decimal LedgerBalance { get; set; }
	}
}