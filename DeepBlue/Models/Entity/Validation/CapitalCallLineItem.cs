using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalCallLineItemMD))]
	public partial class CapitalCallLineItem {
		public class CapitalCallLineItemMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "CapitalCallID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalCallID is required")]
			public global::System.Int32 CapitalCallID {
				get;
				set;
			}

			[Required(ErrorMessage = "InvestorID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestorID is required")]
			public global::System.Int32 InvestorID {
				get;
				set;
			}
			#endregion
		}

		public CapitalCallLineItem(ICapitalCallLineItemService capitalCallLineItemservice)
			: this() {
			this.capitalCallLineItemservice = capitalCallLineItemservice;
		}

		public CapitalCallLineItem() {
		}

		private ICapitalCallLineItemService _capitalCallLineItemService;
		public ICapitalCallLineItemService capitalCallLineItemservice {
			get {
				if (_capitalCallLineItemService == null) {
					_capitalCallLineItemService = new CapitalCallLineItemService();
				}
				return _capitalCallLineItemService;
			}
			set {
				_capitalCallLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var capitalCallLineItem = this;
			IEnumerable<ErrorInfo> errors = Validate(capitalCallLineItem);
			if (errors.Any()) {
				return errors;
			}
			capitalCallLineItemservice.SaveCapitalCallLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalCallLineItem capitalCallLineItemclosing) {
			return ValidationHelper.Validate(capitalCallLineItemclosing);
		}
	}
}