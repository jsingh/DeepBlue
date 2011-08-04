using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ShareClassTypeMD))]
	public partial class ShareClassType {
		public class ShareClassTypeMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "ShareClass is required")]
			[StringLength(100, ErrorMessage = "ShareClass must be under 100 characters.")]
			public global::System.String ShareClass {
				get;
				set;
			}

			#endregion
		}

		public ShareClassType(IShareClassTypeService shareClassTypeService)
			: this() {
				this.ShareClassTypeService = shareClassTypeService;
		}

		public ShareClassType() {
		}

		private IShareClassTypeService _ShareClassTypeService;
		public IShareClassTypeService ShareClassTypeService {
			get {
				if (_ShareClassTypeService == null) {
					_ShareClassTypeService = new ShareClassTypeService();
				}
				return _ShareClassTypeService;
			}
			set {
				_ShareClassTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			ShareClassTypeService.SaveShareClassType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ShareClassType shareClassType) {
			return ValidationHelper.Validate(shareClassType);
		}
	}
}