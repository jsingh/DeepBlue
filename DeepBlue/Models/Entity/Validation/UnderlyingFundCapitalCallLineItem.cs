using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundCapitalCallLineItemMD))]
	public partial class UnderlyingFundCapitalCallLineItem {
		public class UnderlyingFundCapitalCallLineItemMD {
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

		public UnderlyingFundCapitalCallLineItem(IUnderlyingFundCapitalCallLineItemService underlyingFundCapitalCallLineItemservice)
			: this() {
			this.underlyingFundCapitalCallLineItemservice = underlyingFundCapitalCallLineItemservice;
		}

		public UnderlyingFundCapitalCallLineItem() {
		}

		private IUnderlyingFundCapitalCallLineItemService _underlyingFundCapitalCallLineItemService;
		public IUnderlyingFundCapitalCallLineItemService underlyingFundCapitalCallLineItemservice {
			get {
				if (_underlyingFundCapitalCallLineItemService == null) {
					_underlyingFundCapitalCallLineItemService = new UnderlyingFundCapitalCallLineItemService();
				}
				return _underlyingFundCapitalCallLineItemService;
			}
			set {
				_underlyingFundCapitalCallLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundCapitalCallLineItem = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundCapitalCallLineItem);
			if (errors.Any()) {
				return errors;
			}
			underlyingFundCapitalCallLineItemservice.SaveUnderlyingFundCapitalCallLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem) {
			return ValidationHelper.Validate(underlyingFundCapitalCallLineItem);
		}
	}
}