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

			[Required(ErrorMessage = "DealUnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealUnderlyingFundID is required")]
			public global::System.Int32 DealUnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "CommitmentAmount is required")]
            [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CommitmentAmount is required")]
			public global::System.Decimal CommitmentAmount {
				get;
				set;
			}

			[Required(ErrorMessage = "UnfundedAmount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "UnfundedAmount is required")]
			public global::System.Decimal UnfundedAmount {
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

		private IDealUnderlyingFundAdjustmentService _dealUnderlyingFundAdjustmentService;
		public IDealUnderlyingFundAdjustmentService DealUnderlyingFundAdjustmentService {
			get {
				if (_dealUnderlyingFundAdjustmentService == null) {
					_dealUnderlyingFundAdjustmentService = new DealUnderlyingFundAdjustmentService();
				}
				return _dealUnderlyingFundAdjustmentService;
			}
			set {
				_dealUnderlyingFundAdjustmentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var dealUnderlyingFundAdjustment = this;
			IEnumerable<ErrorInfo> errors = Validate(dealUnderlyingFundAdjustment);
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