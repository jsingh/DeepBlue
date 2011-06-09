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

		private ISecurityTypeService _securityTypeService;
		public ISecurityTypeService SecurityTypeService {
			get {
				if (_securityTypeService == null) {
					_securityTypeService = new SecurityTypeService();
				}
				return _securityTypeService;
			}
			set {
				_securityTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var securityType = this;
			IEnumerable<ErrorInfo> errors = Validate(securityType);
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