using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundCapitalCallLineItemMD))]
	public partial class UnderlyingFundCapitalCallLineItem {
		public class UnderlyingFundCapitalCallLineItemMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealID is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundCapitalCallLineItem(IUnderlyingFundCapitalCallLineItemService underlyingFundCapitalCallLineItemService)
			: this() {
				this.UnderlyingFundCapitalCallLineItemService = underlyingFundCapitalCallLineItemService;
		}

		public UnderlyingFundCapitalCallLineItem() {
		}

		private IUnderlyingFundCapitalCallLineItemService _UnderlyingFundCapitalCallLineItemService;
		public IUnderlyingFundCapitalCallLineItemService UnderlyingFundCapitalCallLineItemService {
			get {
				if (_UnderlyingFundCapitalCallLineItemService == null) {
					_UnderlyingFundCapitalCallLineItemService = new UnderlyingFundCapitalCallLineItemService();
				}
				return _UnderlyingFundCapitalCallLineItemService;
			}
			set {
				_UnderlyingFundCapitalCallLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundCapitalCallLineItemService.SaveUnderlyingFundCapitalCallLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem) {
			return ValidationHelper.Validate(underlyingFundCapitalCallLineItem);
		}
	}
}