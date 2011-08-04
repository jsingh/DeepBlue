using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionMD))]
	public partial class CapitalDistribution {
		public class CapitalDistributionMD : CreatedByFields {
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
			[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Distribution Amount is required")]
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

		public CapitalDistribution(ICapitalDistributionService capitalDistributionService)
			: this() {
				this.CapitalDistributionService = capitalDistributionService;
		}

		public CapitalDistribution() {
		}

		private ICapitalDistributionService _CapitalDistributionService;
		public ICapitalDistributionService CapitalDistributionService {
			get {
				if (_CapitalDistributionService == null) {
					_CapitalDistributionService = new CapitalDistributionService();
				}
				return _CapitalDistributionService;
			}
			set {
				_CapitalDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalDistributionService.SaveCapitalDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalDistribution capitalDistribution) {
			return ValidationHelper.Validate(capitalDistribution);
		}
	}
}