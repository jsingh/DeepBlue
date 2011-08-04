using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CurrencyMD))]
	public partial class Currency {
		public class CurrencyMD : CreatedByFields {

			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Currency is required")]
			[StringLength(100, ErrorMessage = "Currency Name must be under 100 characters.")]
			public global::System.String Currency1 {
				get;
				set;
			}

			#endregion
		}

		public Currency(ICurrencyService currencyService)
			: this() {
				this.CurrencyService = currencyService;
		}

		public Currency() {
		}

		private ICurrencyService _CurrencyService;
		public ICurrencyService CurrencyService {
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
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CurrencyService.SaveCurrency(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Currency currency) {
			return ValidationHelper.Validate(currency);
		}
	}
}