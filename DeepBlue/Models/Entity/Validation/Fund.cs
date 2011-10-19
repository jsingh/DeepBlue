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

			[StringLength(50, ErrorMessage = "TaxID must be under 50 characters.")]
			public global::System.String TaxID {
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

		private IFundService _FundService;
		public IFundService FundService {
			get {
				if (_FundService == null) {
					_FundService = new FundService();
				}
				return _FundService;
			}
			set {
				_FundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
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
	 
}