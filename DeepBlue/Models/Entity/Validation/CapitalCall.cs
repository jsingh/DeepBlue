using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalCallMD))]
	public partial class CapitalCall {
		public class CapitalCallMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Capital Call Number is required")]
			[StringLength(12, ErrorMessage = "Capital Call Number must be under 12 characters.")]
			public global::System.String CapitalCallNumber {
				get;
				set;
			}

			[Required(ErrorMessage = "Capital Call Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Capital Call Type is required")]
			public global::System.Int32 CapitalCallTypeID {
				get;
				set;
			}

			[Required(ErrorMessage="Capital Call Date is required")]
			[DateRange(ErrorMessage = "Capital Call Date is required")]
			public global::System.DateTime CapitalCallDate {
				get;
				set;
			}

			[Required(ErrorMessage = "Capital Call Due Date is required")]
			[DateRange(ErrorMessage = "Capital Call Due Date is required")]
			public global::System.DateTime CapitalCallDueDate {
				get;
				set;
			}

			[Required(ErrorMessage="Capital Amount Called is required")]
			[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Capital Amount Called is required")]
			public global::System.Decimal CapitalAmountCalled {
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

		private IEnumerable<ErrorInfo> Validate(CapitalCall capitalCall) {
			return ValidationHelper.Validate(capitalCall);
		}
	}
}