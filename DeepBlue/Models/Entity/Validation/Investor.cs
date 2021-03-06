﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorMD))]
	public partial class Investor {
		public class InvestorMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Social is required")]
			[StringLength(25, ErrorMessage = "Social must be under 25 characters.")]
			public global::System.String Social {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Name is required")]
			[StringLength(100, ErrorMessage = "Investor Name must be under 100 characters.")]
			public global::System.String InvestorName {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Entity Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor Entity Type is required")]
			public global::System.Int32 InvestorEntityTypeID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "Display Name must be under 50 characters.")]
			public global::System.String Alias {
				get;
				set;
			}

			[Range(0, int.MaxValue, ErrorMessage = "Prev Investor is required")]
			public Nullable<global::System.Int32> PrevInvestorID {
				get;
				set;
			}

			[StringLength(40, ErrorMessage = "Manager Name must be under 40 characters.")]
			public global::System.String ManagerName {
				get;
				set;
			}

			[Required]
			[StringLength(30, ErrorMessage = "Last Name must be under 30 characters.")]
			public global::System.String LastName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "First Name must be under 30 characters.")]
			public global::System.String FirstName {
				get;
				set;
			}

			[StringLength(30, ErrorMessage = "Middle Name must be under 30 characters.")]
			public global::System.String MiddleName {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Residency State is required")]
			public Nullable<global::System.Int32> ResidencyState {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
			public global::System.String Notes {
				get;
				set;
			}

			#endregion
		}

		public Investor(IInvestorService investorService)
			: this() {
			this.InvestorService = investorService;
		}

		public Investor() {
		}
		private IInvestorService _InvestorService;
		public IInvestorService InvestorService {
			get {
				if (_InvestorService == null) {
					_InvestorService = new InvestorService();
				}
				return _InvestorService;
			}
			set {
				_InvestorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorService.SaveInvestor(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Investor investor) {
			return ValidationHelper.Validate(investor);
		}
	}
}