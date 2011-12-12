using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingDirectDividendDistributionMD))]
	public partial class UnderlyingDirectDividendDistribution {
		public class UnderlyingDirectDividendDistributionMD {

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

			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}

			[DateRange(ErrorMessage = "DistributionDate is required")]
			public Nullable<global::System.DateTime> DistributionDate {
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

			[Required(ErrorMessage = "IsPostRecordDateTransaction is required")]
			public global::System.Boolean IsPostRecordDateTransaction {
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

			[StringLength(100, ErrorMessage = "ChequeNumber must be under 100 characters.")]
			public global::System.String ChequeNumber {
				get;
				set;
			}

			#endregion
		}
		public UnderlyingDirectDividendDistribution(IUnderlyingDirectDividendDistributionService underlyingDirectDividendDistributionService)
			: this() {
			this.UnderlyingDirectDividendDistributionService = underlyingDirectDividendDistributionService;
		}

		public UnderlyingDirectDividendDistribution() {
		}

		private IUnderlyingDirectDividendDistributionService _UnderlyingDirectDividendDistributionService;
		public IUnderlyingDirectDividendDistributionService UnderlyingDirectDividendDistributionService {
			get {
				if (_UnderlyingDirectDividendDistributionService == null) {
					_UnderlyingDirectDividendDistributionService = new UnderlyingDirectDividendDistributionService();
				}
				return _UnderlyingDirectDividendDistributionService;
			}
			set {
				_UnderlyingDirectDividendDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingDirectDividendDistributionService.SaveUnderlyingDirectDividendDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingDirectDividendDistribution underlyingDirectDividendDistribution) {
			return ValidationHelper.Validate(underlyingDirectDividendDistribution);
		}
	}
}
