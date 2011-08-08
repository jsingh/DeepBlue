using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalCallLineItemMD))]
	public partial class CapitalCallLineItem {
		public class CapitalCallLineItemMD {
			#region Primitive Properties

			[Required(ErrorMessage = "CapitalCallID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalCallID is required")]
			public global::System.Int32 CapitalCallID {
				get;
				set;
			}

			[Required(ErrorMessage = "InvestorID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorID is required")]
			public global::System.Int32 InvestorID {
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
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "InvestmentAmount is required")]
			public global::System.Decimal InvestmentAmount {
				get;
				set;
			}

			[DateRange(ErrorMessage = "ReceivedDate is required")]
			public Nullable<global::System.DateTime> ReceivedDate {
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

			[Required(ErrorMessage = "IsReconciled is required")]
			public global::System.Boolean IsReconciled {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "ReconciliationMethod must be under 100 characters.")]
			public global::System.String ReconciliationMethod {
				get;
				set;
			}

			[DateRange(ErrorMessage = "PaidON is required")]
			public Nullable<global::System.DateTime> PaidON {
				get;
				set;
			}

			#endregion
		}

		public CapitalCallLineItem(ICapitalCallLineItemService capitalCallLineItemservice)
			: this() {
				this.CapitalCallLineItemservice = capitalCallLineItemservice;
		}

		public CapitalCallLineItem() {
		}

		private ICapitalCallLineItemService _CapitalCallLineItemService;
		public ICapitalCallLineItemService CapitalCallLineItemservice {
			get {
				if (_CapitalCallLineItemService == null) {
					_CapitalCallLineItemService = new CapitalCallLineItemService();
				}
				return _CapitalCallLineItemService;
			}
			set {
				_CapitalCallLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalCallLineItemservice.SaveCapitalCallLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalCallLineItem capitalCallLineItem) {
			return ValidationHelper.Validate(capitalCallLineItem);
		}
	}
}