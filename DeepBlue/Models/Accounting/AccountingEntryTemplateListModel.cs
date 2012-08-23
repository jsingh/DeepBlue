using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Accounting {
	public class AccountingEntryTemplateListModel {

		public int AccountingEntryTemplateID { get; set; }

		public string FundName { get; set; }

		public string VirtualAccountName { get; set; }

		public string AccountingTransactionTypeName { get; set; }

		public string AccountingEntryAmountTypeName { get; set; }

		public Boolean IsCredit { get; set; }
		 
	}
}