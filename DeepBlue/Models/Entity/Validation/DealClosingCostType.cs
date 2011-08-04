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

		public DealClosingCostType(IDealClosingCostTypeService dealClosingCostTypeService)
			: this() {
			this.DealClosingCostTypeService = dealClosingCostTypeService;
		}

		public DealClosingCostType() {
		}

		private IDealClosingCostTypeService _DealClosingCostTypeService;
		public IDealClosingCostTypeService DealClosingCostTypeService {
			get {
				if (_DealClosingCostTypeService == null) {
					_DealClosingCostTypeService = new DealClosingCostTypeService();
				}
				return _DealClosingCostTypeService;
			}
			set {
				_DealClosingCostTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealClosingCostTypeService.SaveDealClosingCostType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealClosingCostType dealClosingCostType) {
			return ValidationHelper.Validate(dealClosingCostType);
		}
	}
}