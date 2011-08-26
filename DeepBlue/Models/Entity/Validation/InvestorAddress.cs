using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(InvestorAddressMD))]
	public partial class InvestorAddress {
		public class InvestorAddressMD {

			#region Primitive Properties

			[Required(ErrorMessage = "InvestorID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorID is required")]
			public global::System.Int32 InvestorID {
				get;
				set;
			}


			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "AddressID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AddressID is required")]
			public global::System.Int32 AddressID {
				get;
				set;
			}


			[DateRange(ErrorMessage = "CreatedDate is required")]
			public Nullable<global::System.DateTime> CreatedDate {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public Nullable<global::System.Int32> CreatedBy {
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

		public InvestorAddress(IInvestorAddressService investoraddressService)
			: this() {
			this.InvestorAddressService = InvestorAddressService;
		}

		public InvestorAddress() {
		}

		private IInvestorAddressService _InvestorAddressService;
		public IInvestorAddressService InvestorAddressService {
			get {
				if (_InvestorAddressService == null) {
					_InvestorAddressService = new InvestorAddressService();
				}
				return _InvestorAddressService;
			}
			set {
				_InvestorAddressService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			errors = errors.Union(ValidationHelper.Validate(this.Address));
			if (errors.Any()) {
				return errors;
			}
			InvestorAddressService.SaveInvestorAddress(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorAddress investoraddress) {
			return ValidationHelper.Validate(investoraddress);
		}
	}
}