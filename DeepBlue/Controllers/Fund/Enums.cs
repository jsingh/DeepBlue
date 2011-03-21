using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Fund.Enums {
	public enum RateScheduleType {
		TieredRateSchedule = 1
	}

	public enum MutiplierType {
		CapitalCommitted = 1,
		FlatFee = 2
	}
}