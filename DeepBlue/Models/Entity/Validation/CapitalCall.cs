using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalCallMD))]
	public partial class CapitalCall {
		public class CapitalCallMD {
			#region Primitive Properties

			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "CapitalCallDate is required")]
			[DateRange(ErrorMessage = "CapitalCallDate is required")]
			public global::System.DateTime CapitalCallDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CapitalCallDueDate is required")]
			[DateRange(ErrorMessage = "CapitalCallDueDate is required")]
			public global::System.DateTime CapitalCallDueDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CapitalAmountCalled is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CapitalAmountCalled is required")]
			public global::System.Decimal CapitalAmountCalled {
				get;
				set;
			}

			[Required(ErrorMessage = "InvestmentAmount is required")]
			public global::System.Decimal InvestmentAmount {
				get;
				set;
			}

			[DateRange(ErrorMessage = "ManagementFeeStartDate is required")]
			public Nullable<global::System.DateTime> ManagementFeeStartDate {
				get;
				set;
			}

			[DateRange(ErrorMessage = "ManagementFeeEndDate is required")]
			public Nullable<global::System.DateTime> ManagementFeeEndDate {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "CapitalCallNumber must be under 50 characters.")]
			public global::System.String CapitalCallNumber {
				get;
				set;
			}

			[Required(ErrorMessage = "CapitalCallTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalCallTypeID is required")]
			public global::System.Int32 CapitalCallTypeID {
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

		public CapitalCall(ICapitalCallService capitalCallservice)
			: this() {
				this.CapitalCallservice = capitalCallservice;
		}

		public CapitalCall() {
		}

		private ICapitalCallService _CapitalCallService;
		public ICapitalCallService CapitalCallservice {
			get {
				if (_CapitalCallService == null) {
					_CapitalCallService = new CapitalCallService();
				}
				return _CapitalCallService;
			}
			set {
				_CapitalCallService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalCallservice.SaveCapitalCall(this);
			return null;
		}

		public IEnumerable<ErrorInfo> SaveCapitalCallOnly() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalCallservice.SaveCapitalCallOnly(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalCall capitalCall) {
			return ValidationHelper.Validate(capitalCall);
		}
	}
}