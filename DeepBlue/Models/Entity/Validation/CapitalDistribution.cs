using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionMD))]
	public partial class CapitalDistribution {
		public class CapitalDistributionMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Capital Distribution Date is required")]
			[DateRange(ErrorMessage = "Capital Distribution Date is required")]
			public global::System.DateTime CapitalDistributionDate {
				get;
				set;
			}

			[Required(ErrorMessage = "Capital Distribution Due Date is required")]
			[DateRange(ErrorMessage = "Capital Distribution Due Date is required")]
			public global::System.DateTime CapitalDistributionDueDate {
				get;
				set;
			}


			[Required(ErrorMessage = "Distribution Amount is required")]
			[Range(1, (double)decimal.MaxValue, ErrorMessage = "Distribution Amount is required")]
			public global::System.Decimal DistributionAmount {
				get;
				set;
			}


			[Required(ErrorMessage = "Distribution Number is required")]
			[StringLength(12, ErrorMessage = "Distribution Number must be under 12 characters.")]
			public global::System.String DistributionNumber {
				get;
				set;
			}

			#endregion
		}

		public CapitalDistribution(ICapitalDistributionService capitalCallservice)
			: this() {
			this.capitalCallservice = capitalCallservice;
		}

		public CapitalDistribution() {
		}

		private ICapitalDistributionService _capitalCallService;
		public ICapitalDistributionService capitalCallservice {
			get {
				if (_capitalCallService == null) {
					_capitalCallService = new CapitalDistributionService();
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
			capitalCallservice.SaveCapitalDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalDistribution capitalCallclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(capitalCallclosing);
			return errors;
		}
	}
}