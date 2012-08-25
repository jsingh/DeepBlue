using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Accounting.Enums;
 


namespace DeepBlue.Controllers.Accounting {
	interface IAccounting {
		void CreateAccountingEntry(AccountingTransactionType accountingTransactionType, int fundID, int entityID, IAccountable accountableItem, decimal? amount = null, int? accountingTransactionSubTypeID = null);
	}

	public interface IAccountable {
		int FundID { get; set; }
		int EntityID { get; set; }
		void Audit();
		int? TraceID { get; }
		decimal? Amount { get; }
		int? AttributedTo { get; }
		string AttributedToName { get; }
		string AttributedToType { get; }
	}
}
