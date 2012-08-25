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
		public class UnderlyingFundStockDistributionLineItemMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "Underlying Fund Stock Distribution is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund Stock Distribution is required")]
			public global::System.Int32 UnderlyingFundStockDistributionID {
				get;
				set;
			}
			
			#endregion
		}

		public int? TraceID {
			get {
				return this.UnderlyingFundStockDistributionLineItemID;
			}
		}
		public decimal? Amount {
			get {
				// get the purchase price from the parent record
				DeepBlueEntities context = new DeepBlueEntities();
				decimal purchasePrice = context.UnderlyingFundStockDistributions.Where(x => x.UnderlyingFundStockDistributionID == this.UnderlyingFundStockDistributionID).FirstOrDefault().PurchasePrice;
				return purchasePrice * this.NumberOfShares;
			}
		}

		/// <summary>
		/// Attribute this to the Security distributed
		/// </summary>
		public int? AttributedTo {
			get {
				DeepBlueEntities context = new DeepBlueEntities();
				return context.UnderlyingFundStockDistributions.Where(x => x.UnderlyingFundStockDistributionID == this.UnderlyingFundStockDistributionID).FirstOrDefault().SecurityID;
			}
		}
		public string AttributedToName {
			get {
				DeepBlueEntities context = new DeepBlueEntities();
				return context.Equities.Where(x => x.EquityID == AttributedTo.Value).FirstOrDefault().Symbol;
			}
		}

		public  string AttributedToType {
			get {
				return "Security";
			}
		}

		public UnderlyingFundStockDistributionLineItem(IUnderlyingFundStockDistributionLineItemService underlyingFundStockDistributionLineItemService)
			: this() {
				this.UnderlyingFundStockDistributionLineItemService = underlyingFundStockDistributionLineItemService;
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
			IEnumerable<ErrorInfo> errors = Validate(this);
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