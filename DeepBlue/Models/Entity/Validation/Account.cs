using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(AccountMD))]
	public partial class Account {

		public class AccountMD : CreatedByFields {

			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Account is required")]
			[StringLength(40, ErrorMessage = "Account must be under 40 characters.")]
			public global::System.String Account {
				get;
				set;
			}

			[Required(ErrorMessage = "AccountNumberCash is required")]
			[StringLength(50, ErrorMessage = "AccountNumberCash must be under 50 characters.")]
			public global::System.String AccountNumberCash {
				get;
				set;
			}

			[Required(ErrorMessage = "IsPrimary is required")]
			public global::System.Boolean IsPrimary {
				get;
				set;
			}

			[StringLength(105, ErrorMessage = "Comments must be under 40 characters.")]
			public global::System.String Comments {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "BankName must be under 50 characters.")]
			public global::System.String BankName {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "SWIFT must be under 50 characters.")]
			public global::System.String SWIFT {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "AccountOf must be under 50 characters.")]
			public global::System.String AccountOf {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "FFC must be under 50 characters.")]
			public global::System.String FFC {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "FFCNumber must be under 50 characters.")]
			public global::System.String FFCNumber {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "IBAN must be under 50 characters.")]
			public global::System.String IBAN {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "ByOrderOf must be under 50 characters.")]
			public global::System.String ByOrderOf {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Reference must be under 50 characters.")]
			public global::System.String Reference {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Attention must be under 50 characters.")]
			public global::System.String Attention {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Phone must be under 50 characters.")]
			public global::System.String Phone {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Fax must be under 50 characters.")]
			public global::System.String Fax {
				get;
				set;
			}

			#endregion
		}
	}
}