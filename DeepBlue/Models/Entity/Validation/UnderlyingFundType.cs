using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundTypeMD))]
	public partial class UnderlyingFundType {
		public class UnderlyingFundTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Name is required")]
			[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundType(IUnderlyingFundTypeService underlyingfundtypeservice)
			: this() {
				this.underlyingfundtypeService = underlyingfundtypeService;
		}

		public UnderlyingFundType() {
		}

		private IUnderlyingFundTypeService _UnderlyingFundTypeService;
		public IUnderlyingFundTypeService underlyingfundtypeService {
			get {
				if (_UnderlyingFundTypeService == null) {
					_UnderlyingFundTypeService = new UnderlyingFundTypeService();
				}
				return _UnderlyingFundTypeService;
			}
			set {
				_UnderlyingFundTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var createunderlyingfundtype = this;
			IEnumerable<ErrorInfo> errors = Validate(createunderlyingfundtype);
			if (errors.Any()) {
				return errors;
			}
			underlyingfundtypeService.SaveUnderlyingFundType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundType createunderlyingfundtype) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(createunderlyingfundtype);
			return errors;
		}
	}
}