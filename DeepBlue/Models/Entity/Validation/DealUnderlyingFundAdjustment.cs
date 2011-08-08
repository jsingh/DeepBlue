using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealUnderlyingFundAdjustmentMD))]
	public partial class DealUnderlyingFundAdjustment {
		public class DealUnderlyingFundAdjustmentMD : CreatedByFields {
			#region Primitive Properties

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealUnderlyingFundID is required")]
			public Nullable<global::System.Int32> DealUnderlyingFundID {
				get;
				set;
			}

			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CommitmentAmount is required")]
			public Nullable<global::System.Decimal> CommitmentAmount {
				get;
				set;
			}

			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "UnfundedAmount is required")]
			public Nullable<global::System.Decimal> UnfundedAmount {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedDate is required")]
			[DateRange(ErrorMessage = "CreatedDate is required")]
			public global::System.DateTime CreatedDate {
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

		public DealUnderlyingFundAdjustment(IDealUnderlyingFundAdjustmentService dealUnderlyingFundAdjustmentService)
			: this() {
			this.DealUnderlyingFundAdjustmentService = dealUnderlyingFundAdjustmentService;
		}

		public DealUnderlyingFundAdjustment() {
		}

		private IDealUnderlyingFundAdjustmentService _DealUnderlyingFundAdjustmentService;
		public IDealUnderlyingFundAdjustmentService DealUnderlyingFundAdjustmentService {
			get {
				if (_DealUnderlyingFundAdjustmentService == null) {
					_DealUnderlyingFundAdjustmentService = new DealUnderlyingFundAdjustmentService();
				}
				return _DealUnderlyingFundAdjustmentService;
			}
			set {
				_DealUnderlyingFundAdjustmentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingFundAdjustmentService.SaveDealUnderlyingFundAdjustment(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment) {
			return ValidationHelper.Validate(dealUnderlyingFundAdjustment);
		}
	}
}