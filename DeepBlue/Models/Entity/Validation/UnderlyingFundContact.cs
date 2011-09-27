using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
 

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(UnderlyingFundContactMD))]
	public partial class UnderlyingFundContact {
		public class UnderlyingFundContactMD : CreatedByFields {
		}

		public UnderlyingFundContact(IUnderlyingFundContactService underlyingFundContactservice)
			: this() {
				this.UnderlyingFundContactservice = underlyingFundContactservice;
		}

		public UnderlyingFundContact() {
		}

		private IUnderlyingFundContactService _UnderlyingFundContactService;
		public IUnderlyingFundContactService UnderlyingFundContactservice {
			get {
				if (_UnderlyingFundContactService == null) {
					_UnderlyingFundContactService = new UnderlyingFundContactService();
				}
				return _UnderlyingFundContactService;
			}
			set {
				_UnderlyingFundContactService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundContactservice.SaveUnderlyingFundContact(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundContact underlyingFundContact) {
			return ValidationHelper.Validate(underlyingFundContact);
		}
	}
}