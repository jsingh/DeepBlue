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
			public global::System.String Name {
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

		private IPurchaseTypeService _PurchaseTypeService;
		public IPurchaseTypeService PurchaseTypeService {
			get {
				if (_PurchaseTypeService == null) {
					_PurchaseTypeService = new PurchaseTypeService();
				}
				return _PurchaseTypeService;
			}
			set {
				_PurchaseTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			PurchaseTypeService.SavePurchaseType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(PurchaseType purchaseType) {
			return ValidationHelper.Validate(purchaseType);
		}
	}
}