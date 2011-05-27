﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorTypeMD))]
	public partial class InvestorType {
		public class InvestorTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Type Name is required")]
			[StringLength(20, ErrorMessage = "Investor Type Name must be under 20 characters.")]
			public global::System.String InvestorTypeName {
				get;
				set;
			}
			#endregion
		}

		public InvestorType(IInvestorTypeService investorService)
			: this() {
			this.InvestorTypeService = investorService;
		}

		public InvestorType() {
		}

		private IInvestorTypeService _investorService;
		public IInvestorTypeService InvestorTypeService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorTypeService();
				}
				return _investorService;
			}
			set {
				_investorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investor = this;
			IEnumerable<ErrorInfo> errors = Validate(investor);
			if (errors.Any()) {
				return errors;
			}
			InvestorTypeService.SaveInvestorType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorType investorEntityType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorEntityType);
			return errors;
		}
	}
}