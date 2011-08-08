using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionMD))]
	public partial class CapitalDistribution {
		public class CapitalDistributionMD  {
			#region Primitive Properties

			[Required(ErrorMessage = "CapitalDistributionDate is required")]
			[DateRange(ErrorMessage = "CapitalDistributionDate is required")]
			public global::System.DateTime CapitalDistributionDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CapitalDistributionDueDate is required")]
			[DateRange(ErrorMessage = "CapitalDistributionDueDate is required")]
			public global::System.DateTime CapitalDistributionDueDate {
				get;
				set;
			}

			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "DistributionAmount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "DistributionAmount is required")]
			public global::System.Decimal DistributionAmount {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "DistributionNumber must be under 50 characters.")]
			public global::System.String DistributionNumber {
				get;
				set;
			}

			[Required(ErrorMessage = "IsManual is required")]
			public global::System.Boolean IsManual {
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