using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(SecurityTypeMD))]
	public partial class SecurityType {
		public class SecurityTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Security Type Name is required")]
			[StringLength(100, ErrorMessage = "Security Type Name must be under 100 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public SecurityType(ISecurityTypeService securityTypeService)
			: this() {
			this.SecurityTypeService = securityTypeService;
		}

		public SecurityType() {
		}

		private ISecurityTypeService _SecurityTypeService;
		public ISecurityTypeService SecurityTypeService {
			get {
				if (_SecurityTypeService == null) {
					_SecurityTypeService = new SecurityTypeService();
				}
				return _SecurityTypeService;
			}
			set {
				_SecurityTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			SecurityTypeService.SaveSecurityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(SecurityType securityType) {
			return ValidationHelper.Validate(securityType);
		}
	}
}