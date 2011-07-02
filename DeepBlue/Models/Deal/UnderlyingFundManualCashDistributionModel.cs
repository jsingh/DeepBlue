using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundManualCashDistributionModel : UnderlyingFundCashDistributionModel {

		public UnderlyingFundManualCashDistributionModel() {
			IsManualCapitalCall = true;
		}

		public IEnumerable<ActivityDealModel> Deals { get; set; }

	}

	 
}