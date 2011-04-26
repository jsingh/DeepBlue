using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealClosingCostTypeMD))]
	public partial class DealClosingCostType {
		public class DealClosingCostTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Deal Closing Cost Type Name is required")]
			[StringLength(50, ErrorMessage = "Deal Closing Cost Type Name must be under 50 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public DealClosingCostType(IDealClosingCostTypeService purchaseTypeService)
			: this() {
			this.DealClosingCostTypeService = purchaseTypeService;
		}

		public DealClosingCostType() {
		}

		private IDealClosingCostTypeService _purchaseTypeService;
		public IDealClosingCostTypeService DealClosingCostTypeService {
			get {
				if (_purchaseTypeService == null) {
					_purchaseTypeService = new DealClosingCostTypeService();
				}
				return _purchaseTypeService;
			}
			set {
				_purchaseTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var purchaseType = this;
			IEnumerable<ErrorInfo> errors = Validate(purchaseType);
			if (errors.Any()) {
				return errors;
			}
			DealClosingCostTypeService.SaveDealClosingCostType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealClosingCostType purchaseType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(purchaseType);
			return errors;
		}
	}
}