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

		private IDealClosingService _DealClosingService;
		public IDealClosingService DealClosingService {
			get {
				if (_DealClosingService == null) {
					_DealClosingService = new DealClosingService();
				}
				return _DealClosingService;
			}
			set {
				_DealClosingService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealClosingService.SaveDealClosing(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealClosing dealClosing) {
			return ValidationHelper.Validate(dealClosing);
		}
	}
}