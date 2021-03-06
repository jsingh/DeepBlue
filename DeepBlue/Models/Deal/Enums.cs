﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal.Enums {

	public enum SecurityType {
		Equity = 1,
		FixedIncome = 2
	}

	public enum ActivityType {
		Split = 1,
		Conversion = 2,
		Merger = 3
	}

	public enum Currency {
		AUD = 2,
		CAD = 3,
		EUR = 4,
		JPY = 5,
		USD = 6
	}

	public enum ReconcileType {
		All = 0,
		UnderlyingFundCapitalCall = 1,
		UnderlyingFundCashDistribution = 2,
		CapitalCall = 3,
		CapitalDistribution = 4,
		DividendDistribution = 5,
		PostRecordCapitalCall = 6,
		PostRecordDistribution = 7,
		PostRecordDividendDistribution = 8
	}

	public enum EquitySecurityType {
		Private = 0,
		Public = 1
	}

	public enum UnderlyingFundType {
		Buyout = 1,
		BuyoutMezzanine = 2,
		BuyoutVenture = 3,
		Direct = 4,
		FundOfFunds = 5,
		Mezzanine = 6,
		Note = 7,
		RealEstate = 8,
		Venture = 9
	}
}