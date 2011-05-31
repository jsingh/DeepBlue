using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal.Enums {
    
	public enum SecurityType {
        Equity = 1,
        FixedIncome = 2
    }

	public enum ExportType {
		Word = 1,
		Pdf = 2,
		Print = 3
	}

	public enum ActivityType {
		Split = 1,
		Conversion = 2,
		Merger = 3
	}
}