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
				this.CapitalCallLineItemservice = capitalCallLineItemservice;
		}

		public CapitalCallLineItem() {
		}

		private ICapitalCallLineItemService _CapitalCallLineItemService;
		public ICapitalCallLineItemService CapitalCallLineItemservice {
			get {
				if (_CapitalCallLineItemService == null) {
					_CapitalCallLineItemService = new CapitalCallLineItemService();
				}
				return _CapitalCallLineItemService;
			}
			set {
				_CapitalCallLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalCallLineItemservice.SaveCapitalCallLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalCallLineItem capitalCallLineItem) {
			return ValidationHelper.Validate(capitalCallLineItem);
		}
	}
}