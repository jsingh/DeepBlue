using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CommunicationMD))]
	public partial class Communication {
		public class CommunicationMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Communication Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Communication Type is required")]
			public global::System.Int32 CommunicationTypeID {
				get;
				set;
			}

			[StringLength(200, ErrorMessage = "Communication Value must be under 50 characters.")]
			public global::System.String CommunicationValue {
				get;
				set;
			}

			[StringLength(4, ErrorMessage = "Last Four Phone Value must be under 50 characters.")]
			public global::System.String LastFourPhone {
				get;
				set;
			}

			[StringLength(200, ErrorMessage = "Communication Comment Value must be under 50 characters.")]
			public global::System.String CommunicationComment {
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
			#endregion
		}
	}

}