using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionLineItemMD))]
	public partial class CapitalDistributionLineItem {
		public class CapitalDistributionLineItemMD : CreatedByFields {
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

		public CapitalDistributionLineItem(ICapitalDistributionLineItemService capitalDistributionLineItemService)
			: this() {
				this.CapitalDistributionLineItemService = capitalDistributionLineItemService;
		}

		public CapitalDistributionLineItem() {
		}

		private ICapitalDistributionLineItemService _CapitalDistributionLineItemService;
		public ICapitalDistributionLineItemService CapitalDistributionLineItemService {
			get {
				if (_CapitalDistributionLineItemService == null) {
					_CapitalDistributionLineItemService = new CapitalDistributionLineItemService();
				}
				return _CapitalDistributionLineItemService;
			}
			set {
				_CapitalDistributionLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CapitalDistributionLineItemService.SaveCapitalDistributionLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalDistributionLineItem capitalDistributionLineItem) {
			return ValidationHelper.Validate(capitalDistributionLineItem);
		}
	}
}