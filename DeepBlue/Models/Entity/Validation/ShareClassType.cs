using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ShareClassTypeMD))]
	public partial class ShareClassType {
		public class ShareClassTypeMD {
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

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
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

			[Required(ErrorMessage = "Last Updated Date is required")]
			[DateRange(ErrorMessage = "Last Updated Date is required")]
			public global::System.DateTime LastUpdatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "LastUpdatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public global::System.Int32 LastUpdatedBy {
				get;
				set;
			}


			#endregion
		}

		public ShareClassType(IShareClassTypeService shareclasstypeservice)
			: this() {
				this.shareClasstypeService = shareclasstypeservice;
		}

		public ShareClassType() {
		}

		private IShareClassTypeService _ShareClassTypeService;
		public IShareClassTypeService shareClasstypeService {
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
			var createshareclasstype = this;
			IEnumerable<ErrorInfo> errors = Validate(createshareclasstype);
			if (errors.Any()) {
				return errors;
			}
			shareClasstypeService.SaveShareClassType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ShareClassType createshareclasstype) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(createshareclasstype);
			return errors;
		}
	}
}