using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundRateScheduleMD))]
	public partial class FundRateSchedule {
		public class FundRateScheduleMD {

			#region Primitive Properties
			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "InvestorTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorTypeID is required")]
			public global::System.Int32 InvestorTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "RateScheduleID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "RateScheduleID is required")]
			public global::System.Int32 RateScheduleID {
				get;
				set;
			}

			[Required(ErrorMessage = "RateScheduleTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "RateScheduleTypeID is required")]
			public global::System.Int32 RateScheduleTypeID {
				get;
				set;
			}

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

			#endregion
		}
		 
	}
}