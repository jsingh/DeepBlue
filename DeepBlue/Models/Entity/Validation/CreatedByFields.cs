using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	public class CreatedByFields {

		[Required(ErrorMessage = "CreatedDate is required")]
		[DateRange(ErrorMessage = "CreatedDate is required")]
		public global::System.DateTime CreatedDate {
			get;
			set;
		}


		[Required(ErrorMessage = "CreatedBy is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
		public global::System.Int32 CreatedBy {
			get;
			set;
		}


		[DateRange(ErrorMessage = "LastUpdatedDate is required")]
		public Nullable<global::System.DateTime> LastUpdatedDate {
			get;
			set;
		}


		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
		public Nullable<global::System.Int32> LastUpdatedBy {
			get;
			set;
		}

	}
}