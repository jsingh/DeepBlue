using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Accounting.Enums;
 


namespace DeepBlue.Controllers.Accounting {
	interface IAccounting {
		void CreateEntry(DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, decimal? amount, object record);
	}
}
