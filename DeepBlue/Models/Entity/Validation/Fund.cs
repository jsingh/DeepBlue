using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;


namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundMD))]
	public partial class Fund {
		public class FundMD {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Fund Name is required")]
			[StringLength(50, ErrorMessage = "Fund Name must be under 50 characters")]
			public global::System.String FundName {
				get;
				set;
			}

			[Required(ErrorMessage="Tax Id is required")]
			[StringLength(50, ErrorMessage="Tax Id must be under 50 characters.")]
			public global::System.String TaxId {
				get;
				set;
			}

			[Required(ErrorMessage="Inception Date is required")]
			[DateRange(ErrorMessage = "Inception Date is required")]
			public global::System.DateTime InceptionDate {
				get;
				set;
			}


			#endregion
		}

		public Fund(IFundService fundService)
			: this() {
			this.FundService = fundService;
		}

		public Fund() {
		}

		private IFundService _fundService;
		public IFundService FundService {
			get {
				if (_fundService == null) {
					_fundService = new FundService();
				}
				return _fundService;
			}
			set {
				_fundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fund = this;
			IEnumerable<ErrorInfo> errors = Validate(fund);
			if (errors.Any()) {
				return errors;
			}
			FundService.SaveFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Fund fund) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fund);
			foreach (FundAccount account in fund.FundAccounts) {
				errors = errors.Union(ValidationHelper.Validate(account));
			}
			foreach (FundRateSchedule schedule in fund.FundRateSchedules) {
				errors = errors.Union(ValidationHelper.Validate(schedule));
			}
			return errors;
		}
	}

	[MetadataType(typeof(FundAccountMD))]
	public partial class FundAccount {
		public class FundAccountMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage="Bank Name is required")]
			[StringLength(50, ErrorMessage="Bank Name must be under 50 characters.")]
			public global::System.String BankName {
				get;
				set;
			}

			[Required(ErrorMessage="Account is required")]
			[StringLength(40, ErrorMessage="Account must be under 40 characters.")]
			public global::System.String Account {
				get;
				set;
			}
			#endregion
		}
	}

	[MetadataType(typeof(FundRateScheduleMD))]
	public partial class FundRateSchedule {
		public class FundRateScheduleMD {
			#region Primitive Properties
			
			[Required(ErrorMessage="Investor Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorType is required")]
			public global::System.Int32 InvestorTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Rate Schedule Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "RateScheduleType is required")]
			public global::System.Int32 RateScheduleTypeID {
				get;
				set;
			}
			#endregion
		}
	}
}