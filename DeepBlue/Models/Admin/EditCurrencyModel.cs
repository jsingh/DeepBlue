using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditCurrencyModel {
		public EditCurrencyModel() {
			CurrencyId = 0;
			Currency = string.Empty;
		}

		public int CurrencyId { get; set; }

		[Required(ErrorMessage = "Currency is required")]
		[StringLength(100, ErrorMessage = "Currency must be under 100 characters.")]
		[RemoteUID_(Action = "CurrencyAvailable", Controller = "Admin", ValidateParameterName = "CurrencyCurrency", Params = new string[] { "CurrencyId" })]
		[DisplayName("Currency:")]
		public string Currency { get; set; }


		public bool Enabled { get; set; }
	}
}