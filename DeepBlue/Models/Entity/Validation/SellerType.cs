using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
	namespace DeepBlue.Models.Entity {
		[MetadataType(typeof(SellerTypeMD))]
		public partial class SellerType {
			public class SellerTypeMD {

				#region Primitive Properties
				[Required(ErrorMessage = "EntityID is required")]
				[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
				public global::System.Int32 EntityID {
					get;
					set;
				}

				[StringLength(100, ErrorMessage = "SellerType1 must be under 100 characters.")]
				public global::System.String SellerType1 {
					get;
					set;
				}

				[Required(ErrorMessage = "Enabled is required")]
				public global::System.Boolean Enabled {
					get;
					set;
				}

				[Required(ErrorMessage = "CreatedDate is required")]
				[DateRange(ErrorMessage = "CreatedDate is required")]
				public global::System.DateTime CreatedDate {
					get;
					set;
				}

				[Required(ErrorMessage = "CreatedBy is required")]
				[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
				public global::System.Int32 CreatedBy {
					get;
					set;
				}

				[DateRange(ErrorMessage = "LastUpdatedDate is required")]
				public Nullable<global::System.DateTime> LastUpdatedDate {
					get;
					set;
				}

				[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
				public Nullable<global::System.Int32> LastUpdatedBy {
					get;
					set;
				}

				#endregion
			}
			public SellerType(ISellerTypeService sellerTypeService)
				: this() {
				this.SellerTypeService = sellerTypeService;
			}

			public SellerType() {
			}

			private ISellerTypeService _SellerTypeService;
			public ISellerTypeService SellerTypeService {
				get {
					if (_SellerTypeService == null) {
						_SellerTypeService = new SellerTypeService();
					}
					return _SellerTypeService;
				}
				set {
					_SellerTypeService = value;
				}
			}

			public IEnumerable<ErrorInfo> Save() {
				IEnumerable<ErrorInfo> errors = Validate(this);
				if (errors.Any()) {
					return errors;
				}
				SellerTypeService.SaveSellerType(this);
				return null;
			}

			private IEnumerable<ErrorInfo> Validate(SellerType sellerType) {
				return ValidationHelper.Validate(sellerType);
			}
		}
	}
 