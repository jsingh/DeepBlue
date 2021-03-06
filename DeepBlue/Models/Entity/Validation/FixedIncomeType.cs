﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FixedIncomeTypeMD))]
	public partial class FixedIncomeType {
		public class FixedIncomeTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "FixedIncomeType is required")]
			[StringLength(100, ErrorMessage = "FixedIncomeType must be under 100 characters.")]
			public global::System.String FixedIncomeType1 {
				get;
				set;
			}
			#endregion
		}

		public FixedIncomeType(IFixedIncomeTypeService fixedIncomeTypeService)
			: this() {
			this.FixedIncomeTypeService = fixedIncomeTypeService;
		}

		public FixedIncomeType() {
		}

		private IFixedIncomeTypeService _FixedIncomeTypeService;
		public IFixedIncomeTypeService FixedIncomeTypeService {
			get {
				if (_FixedIncomeTypeService == null) {
					_FixedIncomeTypeService = new FixedIncomeTypeService();
				}
				return _FixedIncomeTypeService;
			}
			set {
				_FixedIncomeTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FixedIncomeTypeService.SaveFixedIncomeType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FixedIncomeType fixedIncomeType) {
			return ValidationHelper.Validate(fixedIncomeType);
		}
	}
}