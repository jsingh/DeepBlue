﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundManualCapitalCallModel : UnderlyingFundCapitalCallModel {

		public UnderlyingFundManualCapitalCallModel() {
			IsManualCapitalCall = true;
		}

		public IEnumerable<ActivityDealModel> Deals { get; set; }

	}
}