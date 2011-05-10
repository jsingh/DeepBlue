﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(IssuerMD))]
	public partial class Issuer {
		public class IssuerMD {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Name is required")]
			[StringLength(100, ErrorMessage = "Name must be under 100 characters")]
			public global::System.String Name {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Parent Name must be under 100 characters")]
			public global::System.String ParentName {
				get;
				set;
			}

			[Required(ErrorMessage = "Country is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
			public global::System.Int32 CountryID {
				get;
				set;
			}

			#endregion
		}

		public Issuer(IIssuerService issuerService)
			: this() {
			this.IssuerService = issuerService;
		}

		public Issuer() {
		}

		private IIssuerService _issuerService;
		public IIssuerService IssuerService {
			get {
				if (_issuerService == null) {
					_issuerService = new IssuerService();
				}
				return _issuerService;
			}
			set {
				_issuerService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var issuer = this;
			IEnumerable<ErrorInfo> errors = Validate(issuer);
			if (errors.Any()) {
				return errors;
			}
			IssuerService.SaveIssuer(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Issuer issuer) {
			return ValidationHelper.Validate(issuer);
		}
	}
}