using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundStockDistributionLineItemMD))]
	public partial class UnderlyingFundStockDistributionLineItem {
		public class UnderlyingFundStockDistributionLineItemMD {
			#region Primitive Properties

			[Required(ErrorMessage = "Underlying Fund Stock Distribution is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund Stock Distribution is required")]
			public global::System.Int32 UnderlyingFundStockDistributionID {
				get;
				set;
			}
			
			#endregion
		}

		public UnderlyingFundStockDistributionLineItem(IUnderlyingFundStockDistributionLineItemService underlyingFundStockDistributionLineItemservice)
			: this() {
				this.UnderlyingFundStockDistributionLineItemService = underlyingFundStockDistributionLineItemservice;
		}

		public UnderlyingFundStockDistributionLineItem() {
		}

		private IUnderlyingFundStockDistributionLineItemService _UnderlyingFundStockDistributionLineItemService;
		public IUnderlyingFundStockDistributionLineItemService UnderlyingFundStockDistributionLineItemService {
			get {
				if (_UnderlyingFundStockDistributionLineItemService == null) {
					_UnderlyingFundStockDistributionLineItemService = new UnderlyingFundStockDistributionLineItemService();
				}
				return _UnderlyingFundStockDistributionLineItemService;
			}
			set {
				_UnderlyingFundStockDistributionLineItemService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundStockDistributionLineItem = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundStockDistributionLineItem);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundStockDistributionLineItemService.SaveUnderlyingFundStockDistributionLineItem(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundStockDistributionLineItem underlyingFundStockDistributionLineItem) {
			return ValidationHelper.Validate(underlyingFundStockDistributionLineItem);
		}
	}
}