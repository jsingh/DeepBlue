using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DividendDistributionMD))]
	public partial class DividendDistribution {
		public class DividendDistributionMD {

			#region Primitive Properties
			[Required(ErrorMessage = "SecurityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityID is required")]
			public global::System.Int32 SecurityID {
				get;
				set;
			}

			[Required(ErrorMessage = "SecurityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityTypeID is required")]
			public global::System.Int32 SecurityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealID is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingDirectDividendDistributionID is required")]
			public Nullable<global::System.Int32> UnderlyingDirectDividendDistributionID {
				get;
				set;
			}

			[Required(ErrorMessage = "Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
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

			[DateRange(ErrorMessage = "DistributionDate is required")]
			public Nullable<global::System.DateTime> DistributionDate {
				get;
				set;
			}

			#endregion
		}
		public DividendDistribution(IDividendDistributionService dividendDistributionService)
			: this() {
			this.DividendDistributionService = dividendDistributionService;
		}

		public DividendDistribution() {
		}

		private IDividendDistributionService _DividendDistributionService;
		public IDividendDistributionService DividendDistributionService {
			get {
				if (_DividendDistributionService == null) {
					_DividendDistributionService = new DividendDistributionService();
				}
				return _DividendDistributionService;
			}
			set {
				_DividendDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DividendDistributionService.SaveDividendDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DividendDistribution dividendDistribution) {
			return ValidationHelper.Validate(dividendDistribution);
		}
	}
}