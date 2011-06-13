using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ContactMD))]
	public partial class Contact {
		public class ContactMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
			public global::System.String ContactName {
				get;
				set;
			}

			[StringLength(20, ErrorMessage = "Contact Type must be under 20 characters.")]
			public global::System.String ContactType {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Last Name must be under 30 characters."), Required]
			public global::System.String LastName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "First Name must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Middle Name must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}
		 
			#endregion
		}
	}
}