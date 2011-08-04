using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingDirectLastPriceHistoryMD))]
	public partial class UnderlyingDirectLastPriceHistory {
		public class UnderlyingDirectLastPriceHistoryMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "Underlying Direct Last Price is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Direct Last Price is required")]
			public global::System.Int32 UnderlyingDirectLastPriceID {
				get;
				set;
			}

			[Required(ErrorMessage = "Last Price is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Last Price is required")]
			public global::System.Decimal LastPrice {
				get;
				set;
			}

			[Required(ErrorMessage = "Last Price Date is required")]
			[DateRange()]
			public global::System.DateTime LastPriceDate {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingDirectLastPriceHistory(IUnderlyingDirectLastPriceHistoryService underlyingDirectLastPriceHistoryService)
			: this() {
			this.UnderlyingDirectLastPriceHistoryService = underlyingDirectLastPriceHistoryService;
		}

		public UnderlyingDirectLastPriceHistory() {
		}

		private IUnderlyingDirectLastPriceHistoryService _UnderlyingDirectLastPriceHistoryService;
		public IUnderlyingDirectLastPriceHistoryService UnderlyingDirectLastPriceHistoryService {
			get {
				if (_UnderlyingDirectLastPriceHistoryService == null) {
					_UnderlyingDirectLastPriceHistoryService = new UnderlyingDirectLastPriceHistoryService();
				}
				return _UnderlyingDirectLastPriceHistoryService;
			}
			set {
				_UnderlyingDirectLastPriceHistoryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingDirectLastPriceHistoryService.SaveUnderlyingDirectLastPriceHistory(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingDirectLastPriceHistory underlyingDirectLastPriceHistory) {
			return ValidationHelper.Validate(underlyingDirectLastPriceHistory);
		}
	}
}