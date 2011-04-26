using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(PurchaseTypeMD))]
	public partial class PurchaseType {
		public class PurchaseTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Purchase Type Name is required")]
			[StringLength(50, ErrorMessage = "Purchase Type Name must be under 50 characters.")]
			public global::System.String PurchaseTypeName {
				get;
				set;
			}
			#endregion
		}

		public PurchaseType(IPurchaseTypeService purchaseTypeService)
			: this() {
			this.PurchaseTypeService = purchaseTypeService;
		}

		public PurchaseType() {
		}

		private IPurchaseTypeService _purchaseTypeService;
		public IPurchaseTypeService PurchaseTypeService {
			get {
				if (_purchaseTypeService == null) {
					_purchaseTypeService = new PurchaseTypeService();
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
			PurchaseTypeService.SavePurchaseType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(PurchaseType purchaseType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(purchaseType);
			return errors;
		}
	}
}