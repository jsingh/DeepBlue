using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(PartnerMD))]
	public partial class Partner {
		public class PartnerMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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

			[Required(ErrorMessage = "Partner Name is required")]
			[StringLength(100, ErrorMessage = "Partner Name  must be under 100 characters.")]
			public global::System.String PartnerName {
				get;
				set;
			}
			#endregion
		}
	}
}