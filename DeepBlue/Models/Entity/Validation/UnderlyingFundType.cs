﻿using System;
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

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundType(IUnderlyingFundTypeService underlyingFundTypeService)
			: this() {
				this.UnderlyingFundTypeService = underlyingFundTypeService;
		}

		public UnderlyingFundType() {
		}

		private IUnderlyingFundTypeService _UnderlyingFundTypeService;
		public IUnderlyingFundTypeService UnderlyingFundTypeService {
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
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundTypeService.SaveUnderlyingFundType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundType underlyingFundType) {
			return ValidationHelper.Validate(underlyingFundType);
		}
	}
}