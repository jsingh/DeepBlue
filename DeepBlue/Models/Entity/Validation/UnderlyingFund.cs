﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundMD))]
	public partial class UnderlyingFund {
		public class UnderlyingFundMD {
			#region Primitive Properties

			[StringLength(75, ErrorMessage = "AuditorName must be under 75 characters.")]
			public global::System.String AuditorName {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
			public global::System.String Description {
				get;
				set;
			}
		 
			[Required(ErrorMessage="FundName is required")]
			[StringLength(100, ErrorMessage = "FundName must be under 100 characters.")]
			public global::System.String FundName {
				get;
				set;
			}


			[Required(ErrorMessage = "IssuerID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "IssuerID is required")]
			public global::System.Int32 IssuerID {
				get;
				set;
			}

			[Required(ErrorMessage = "FundTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundTypeID is required")]
			public global::System.Int32 FundTypeID {
				get;
				set;
			}
						
			#endregion
		}

		public UnderlyingFund(IUnderlyingFundService underlyingFundTypeservice)
			: this() {
			this.underlyingFundTypeService = underlyingFundTypeService;
		}

		public UnderlyingFund() {
		}

		private IUnderlyingFundService _UnderlyingFundService;
		public IUnderlyingFundService underlyingFundTypeService {
			get {
				if (_UnderlyingFundService == null) {
					_UnderlyingFundService = new UnderlyingFundService();
				}
				return _UnderlyingFundService;
			}
			set {
				_UnderlyingFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundType = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundType);
			if (errors.Any()) {
				return errors;
			}
			underlyingFundTypeService.SaveUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFund underlyingFundType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingFundType);
			errors = errors.Union(ValidationHelper.Validate(underlyingFundType.Account));
			errors = errors.Union(ValidationHelper.Validate(underlyingFundType.Contact));
			foreach (ContactCommunication comm in underlyingFundType.Contact.ContactCommunications) {
				errors = errors.Union(ValidationHelper.Validate(comm.Communication));
			}
			return errors;
		}
	}
}