using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealClosingMD))]
	public partial class DealClosing {
		public class DealClosingMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Deal is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[Required(ErrorMessage = "CloseDate is required")]
			[DateRange()]
			public global::System.DateTime CloseDate {
				get;
				set;
			}
			#endregion
		}

		public DealClosing(IDealClosingService dealClosingService)
			: this() {
			this.DealClosingService = dealClosingService;
		}

		public DealClosing() {
		}

		private IDealClosingService _dealClosingService;
		public IDealClosingService DealClosingService {
			get {
				if (_dealClosingService == null) {
					_dealClosingService = new DealClosingService();
				}
				return _dealClosingService;
			}
			set {
				_dealClosingService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var dealClosing = this;
			IEnumerable<ErrorInfo> errors = Validate(dealClosing);
			if (errors.Any()) {
				return errors;
			}
			DealClosingService.SaveDealClosing(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealClosing dealClosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(dealClosing);
			return errors;
		}
	}
}