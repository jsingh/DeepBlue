using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingDirectLastPriceMD))]
	public partial class UnderlyingDirectLastPrice {
		public class UnderlyingDirectLastPriceMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Security Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
			public global::System.Int32 SecurityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Security is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security is required")]
			public global::System.Int32 SecurityID {
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

		public UnderlyingDirectLastPrice(IUnderlyingDirectLastPriceService underlyingDirectLastPriceService)
			: this() {
			this.UnderlyingDirectLastPriceService = underlyingDirectLastPriceService;
		}

		public UnderlyingDirectLastPrice() {
		}

		private IUnderlyingDirectLastPriceService _UnderlyingDirectLastPriceService;
		public IUnderlyingDirectLastPriceService UnderlyingDirectLastPriceService {
			get {
				if (_UnderlyingDirectLastPriceService == null) {
					_UnderlyingDirectLastPriceService = new UnderlyingDirectLastPriceService();
				}
				return _UnderlyingDirectLastPriceService;
			}
			set {
				_UnderlyingDirectLastPriceService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingDirectLastPriceService.SaveUnderlyingDirectLastPrice(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingDirectLastPrice underlyingDirectLastPrice) {
			return ValidationHelper.Validate(underlyingDirectLastPrice);
		}
	}
}