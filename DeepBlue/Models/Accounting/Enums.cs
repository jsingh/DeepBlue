using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Accounting.Enums {
	public enum AccountingTransactionType {
		CapitalCallLineItem = 1,
		CapitalCallReconcilationLineItem = 2,
		CapitalDistributionLineItem = 3,
		DealUnderlyingFund = 4,
		CashDistribution = 5,
		UnderlyingFundStockDistributionLineItem = 6,
		FundExpense = 7,
		DealClosingCost = 8,
		SecuritySaleLineItem = 9
	}

	public enum AccountingEntryAmountType {
		Amount = 1,
		FixedAmount = 2,
		Percentage = 3,
		Field = 4,
		Custom = 5
	}
}