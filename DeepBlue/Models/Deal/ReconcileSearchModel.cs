using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {

	public class ReconcileSearchModel {

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? StartDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? EndDate { get; set; }

		public int? FundId { get; set; }

		public int? UnderlyingFundId { get; set; }

		public int ReconcileType { get; set; }
		
	}
}