using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealClosingCostMD))]
	public partial class DealClosingCost {
		public class DealClosingCostMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Deal Closing Cost Type Name is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Closing Cost Type Name is required")]
			public global::System.Int32 DealClosingCostTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Amount is required")]
			[Range((double)1, (double)decimal.MaxValue, ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}

			[Required(ErrorMessage = "Deal is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[Required(ErrorMessage = "Date is required")]
			[DateRange()]
			public global::System.DateTime Date {
				get;
				set;
			}
			#endregion
		}

		public DealClosingCost(IDealClosingCostService dealClosingCostService)
			: this() {
			this.DealClosingCostService = dealClosingCostService;
		}

		public DealClosingCost() {
		}

		private IDealClosingCostService _dealClosingCostService;
		public IDealClosingCostService DealClosingCostService {
			get {
				if (_dealClosingCostService == null) {
					_dealClosingCostService = new DealClosingCostService();
				}
				return _dealClosingCostService;
			}
			set {
				_dealClosingCostService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var dealClosingCost = this;
			IEnumerable<ErrorInfo> errors = Validate(dealClosingCost);
			if (errors.Any()) {
				return errors;
			}
			DealClosingCostService.SaveDealClosingCost(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealClosingCost dealClosingCost) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(dealClosingCost);
			return errors;
		}
	}
}