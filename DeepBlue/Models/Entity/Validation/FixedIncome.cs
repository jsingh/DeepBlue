using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FixedIncomeMD))]
	public partial class FixedIncome {
		public class FixedIncomeMD {

			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Company is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Company is required")]
			public global::System.Int32 IssuerID {
				get;
				set;
			}

			[Required(ErrorMessage = "Fixed Income Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fixed Income Type is required")]
			public global::System.Int32 FixedIncomeTypeID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
			public global::System.String Symbol {
				get;
				set;
			}

			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Face Value is required")]
			public Nullable<global::System.Decimal> FaceValue {
				get;
				set;
			}

			[DateRange(ErrorMessage = "Maturity is required")]
			public Nullable<global::System.DateTime> Maturity {
				get;
				set;
			}

			[DateRange(ErrorMessage = "Issued Date is required")]
			public Nullable<global::System.DateTime> IssuedDate {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Currency is required")]
			public Nullable<global::System.Int32> CurrencyID {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "Frequency is required")]
			public Nullable<global::System.Int32> Frequency {
				get;
				set;
			}

			[DateRange(ErrorMessage = "First Accrual Date is required")]
			public Nullable<global::System.DateTime> FirstAccrualDate {
				get;
				set;
			}

			[DateRange(ErrorMessage = "First Coupon Date is required")]
			public Nullable<global::System.DateTime> FirstCouponDate {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Industry is required")]
			public Nullable<global::System.Int32> IndustryID {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Coupon Information must be under 100 characters.")]
			public global::System.String CouponInformation {
				get;
				set;
			}

			[StringLength(105, ErrorMessage = "Comments must be under 105 characters.")]
			public global::System.String Comments {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "CUSIP NO must be under 50 characters.")]
			public global::System.String ISIN {
				get;
				set;
			}

			#endregion
		}

		public FixedIncome(IFixedIncomeService fixedIncomeService)
			: this() {
			this.FixedIncomeService = fixedIncomeService;
		}

		public FixedIncome() {
		}

		private IFixedIncomeService _FixedIncomeService;
		public IFixedIncomeService FixedIncomeService {
			get {
				if (_FixedIncomeService == null) {
					_FixedIncomeService = new FixedIncomeService();
				}
				return _FixedIncomeService;
			}
			set {
				_FixedIncomeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FixedIncomeService.SaveFixedIncome(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FixedIncome fixedIncome) {
			return ValidationHelper.Validate(fixedIncome);
		}
	}
}