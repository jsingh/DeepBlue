using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Transaction.Enums {
	public enum TransactionType {
		OriginalCommitment = 1,
		SubsequentCommitment = 2,
		Buy = 3,
		Sell = 4,
		Split = 5
	}
}