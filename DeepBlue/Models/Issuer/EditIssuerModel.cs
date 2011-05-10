using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Issuer {
	public class EditIssuerModel {

		public EditIssuerModel() {
			Name = string.Empty;
			ParentName = string.Empty;
			CountryId = 0;
			Equities = new List<EquityDetailModel>();
			FixedIncomes = new List<FixedIncomeDetailModel>();
		}

		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
		[RemoteUID_(Action = "IssuerNameAvailable", Controller = "Issuer", ValidateParameterName = "IssuerName", Params = new string[] { "IssuerId" })]
		[DisplayName("Name:")]
		public string Name { get; set; }

		[StringLength(100, ErrorMessage = "Parent Name must be under 100 characters.")]
		[DisplayName("Parent Name:")]
		public string ParentName { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required.")]
		[DisplayName("Country:")]
		public int? CountryId { get; set; }

		public List<EquityDetailModel> Equities { get; set; }

		public List<FixedIncomeDetailModel> FixedIncomes { get; set; }

		public List<SelectListItem> Countries { get; set; }

		public List<SelectListItem> EquityTypes { get; set; }

		public List<SelectListItem> FixedIncomeTypes { get; set; }

		public List<SelectListItem> ShareClassTypes { get; set; }

		public List<SelectListItem> Industries { get; set; }

		public List<SelectListItem> Currencies { get; set; }


	}
}