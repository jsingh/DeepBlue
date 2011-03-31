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
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required]
			[StringLength(12)]
			public global::System.String CapitalCallNumber {
				get;
				set;
			}


			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 CapitalCallTypeID {
				get;
				set;
			}

			[Required]
			[DateRange()]
			public global::System.DateTime CapitalCallDate {
				get;
				set;
			}

			[Required]
			[DateRange()]
			public global::System.DateTime CapitalCallDueDate {
				get;
				set;
			}

			[Required]
			[Range(1, (double)decimal.MaxValue)]
			public global::System.Decimal CapitalAmountCalled {
				get;
				set;
			}

			[Required]
			[Range(1, (double)decimal.MaxValue)]
			public global::System.Decimal NewInvestmentAmount {
				get;
				set;
			}
		
			#endregion
		}
																							
		public CapitalCall(ICapitalCallService capitalCallservice)
			: this() {
			this.capitalCallservice = capitalCallservice;
		}

		public CapitalCall() {
		}

		private ICapitalCallService _capitalCallService;
		public ICapitalCallService capitalCallservice {
			get {
				if (_capitalCallService == null) {
					_capitalCallService = new CapitalCallService();
				}
				return _capitalCallService;
			}
			set {
				_capitalCallService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var capitalCall = this;
			IEnumerable<ErrorInfo> errors = Validate(capitalCall);
			if (errors.Any()) {
				return errors;
			}
			capitalCallservice.SaveCapitalCall(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalCall capitalCallclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(capitalCallclosing);
			return errors;
		}
	}
}