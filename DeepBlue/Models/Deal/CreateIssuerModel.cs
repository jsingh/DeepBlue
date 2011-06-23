using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class CreateIssuerModel  {

		public CreateIssuerModel() {
			EquityDetailModel = new EquityDetailModel();
			FixedIncomeDetailModel = new FixedIncomeDetailModel();
			IssuerDetailModel = new IssuerDetailModel();
		}

		public IssuerDetailModel IssuerDetailModel { get; set; }

		public EquityDetailModel EquityDetailModel { get; set; }

		public FixedIncomeDetailModel FixedIncomeDetailModel { get; set; }
		
	}
}