using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorAccountMD))]
	public partial class InvestorAccount {
		public class InvestorAccountMD: CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "Routing is required")]
			public global::System.Int32 Routing {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Account must be under 40 characters.")]
			public global::System.String Account {
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

			[StringLength(105, ErrorMessage = "Comments must be under 105 characters.")]
			public global::System.String Comments {
				get;
				set;
			}

		 
			[StringLength(50, ErrorMessage = "BankName must be under 50 characters.")]
			public global::System.String BankName {
				get;
				set;
			}
			#endregion
		}
	}
}