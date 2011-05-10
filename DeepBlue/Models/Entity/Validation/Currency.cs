using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CurrencyMD))]
	public partial class Currency {
		public class CurrencyMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Currency is required")]
			[StringLength(100, ErrorMessage = "Currency Name must be under 100 characters.")]
			public global::System.String Currency {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required(ErrorMessage = "Last Updated Date is required")]
			[DateRange(ErrorMessage = "Last Updated Date is required")]
			public global::System.DateTime LastUpdatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "LastUpdatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public global::System.Int32 LastUpdatedBy {
				get;
				set;
			}


			#endregion
		}

		public Currency(ICurrencyService currencyservice)
			: this() {
			this.currencyService = currencyservice;
		}

		public Currency() {
		}

		private ICurrencyService _CurrencyService;
		public ICurrencyService currencyService {
			get {
				if (_CurrencyService == null) {
					_CurrencyService = new CurrencyService();
				}
				return _CurrencyService;
			}
			set {
				_CurrencyService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var createcurrency = this;
			IEnumerable<ErrorInfo> errors = Validate(createcurrency);
			if (errors.Any()) {
				return errors;
			}
			currencyService.SaveCurrency(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Currency currency) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(currency);
			return errors;
		}
	}
}