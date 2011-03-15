using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin.Enums {
	public enum ControllerType {
		EntityType = 1,
		FOIA = 2,
		ERISA = 3
	}

	public enum Module {
		Investor = 1,
		Fund = 2
	}

	public enum CustomFieldDataType {
		Integer = 1,
		Text = 2,
		Boolean = 3,
		DateTime = 4,
		Currency = 5,
		MultiSelectOpiton = 6,
		SingleSelectOption = 7
	}
}