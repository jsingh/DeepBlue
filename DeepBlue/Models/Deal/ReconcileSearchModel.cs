using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {

	public class ReconcileSearchModel {

		public ReconcileSearchModel(){
			PageIndex = 1;
			PageSize = 25;
		}

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? StartDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? EndDate { get; set; }

		public int? FundId { get; set; }

		public int? UnderlyingFundId { get; set; }

		public int ReconcileType { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }
		
	}
}