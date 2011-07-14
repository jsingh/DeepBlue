using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionLineItemMD))]
	public partial class CapitalDistributionLineItem {
		public class CapitalDistributionLineItemMD {
			#region Primitive Properties
			[Required(ErrorMessage = "CapitalDistributionID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalDistributionID is required")]
			public global::System.Int32 CapitalDistributionID {
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

		public CapitalDistributionLineItem(ICapitalDistributionLineItemService capitalDistributionLineItemservice)
			: this() {
			this.capitalDistributionLineItemservice = capitalDistributionLineItemservice;
		}

		public CapitalDistributionLineItem() {
		}

		private ICapitalDistributionLineItemService _capitalDistributionLineItemService;
		public ICapitalDistributionLineItemService capitalDistributionLineItemservice {
			get {
				if (_capitalDistributionLineItemService == null) {
					_capitalDistributionLineItemService = new CapitalDistributionLineItemService();
				}
				return _capitalDistributionLineItemService;
			}
			set {
				_capitalDistributionLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var capitalDistributionLineItem = this;
			IEnumerable<ErrorInfo> errors = Validate(capitalDistributionLineItem);
			if (errors.Any()) {
				return errors;
			}
			capitalDistributionLineItemservice.SaveCapitalDistributionLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalDistributionLineItem capitalDistributionLineItem) {
			return ValidationHelper.Validate(capitalDistributionLineItem);
		}
	}
}