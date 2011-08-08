using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(AddressMD))]
	public partial class Address {
		public class AddressMD {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "AddressTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AddressTypeID is required")]
			public global::System.Int32 AddressTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Address1 is required")]
			[StringLength(40, ErrorMessage = "Address1 must be under 40 characters.")]
			public global::System.String Address1 {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Address2 must be under 40 characters.")]
			public global::System.String Address2 {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Address3 must be under 40 characters.")]
			public global::System.String Address3 {
				get;
				set;
			}

			[Required(ErrorMessage = "City is required")]
			[StringLength(30, ErrorMessage = "City must be under 30 characters.")]
			public global::System.String City {
				get;
				set;
			}

			[StringLength(125, ErrorMessage = "StProvince must be under 125 characters.")]
			public global::System.String StProvince {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State is required")]
			public Nullable<global::System.Int32> State {
				get;
				set;
			}

			[StringLength(10, ErrorMessage = "Zip must be under 10 characters.")]
			public global::System.String PostalCode {
				get;
				set;
			}

			[Required(ErrorMessage = "Country is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
			public global::System.Int32 Country {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "County must be under 50 characters.")]
			public global::System.String County {
				get;
				set;
			}

			[Required(ErrorMessage = "Listed is required")]
			public global::System.Boolean Listed {
				get;
				set;
			}

			[Required(ErrorMessage = "IsPreferred is required")]
			public global::System.Boolean IsPreferred {
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