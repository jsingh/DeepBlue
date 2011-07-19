﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {

	public class CapitalCallDetail {

		public int CapitalCallId { get; set; }

		public string Number { get; set; }

		public decimal? CapitalCallAmount { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? FundExpenses { get; set; }

		public DateTime CapitalCallDate { get; set; }

		public DateTime CapitalCallDueDate { get; set; }
	}
}