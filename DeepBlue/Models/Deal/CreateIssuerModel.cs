using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class CreateIssuerModel {

		public CreateIssuerModel() {
			EquityDetailModel = new EquityDetailModel();
			FixedIncomeDetailModel = new FixedIncomeDetailModel();
		}

		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
		[RemoteUID_(Action = "IssuerNameAvailable", Controller = "Issuer", ValidateParameterName = "IssuerName", Params = new string[] { "IssuerId" })]
		[DisplayName("Issuer Name")]
		public string Name { get; set; }

		[StringLength(100, ErrorMessage = "Parent Name must be under 100 characters.")]
		[DisplayName("Parent Name")]
		public string ParentName { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required.")]
		[DisplayName("Country")]
		public int? CountryId { get; set; }

		public EquityDetailModel EquityDetailModel { get; set; }

		public FixedIncomeDetailModel FixedIncomeDetailModel { get; set; }

		public List<SelectListItem> Countries { get; set; }

		public List<SelectListItem> EquityTypes { get; set; }

		public List<SelectListItem> FixedIncomeTypes { get; set; }

		public List<SelectListItem> ShareClassTypes { get; set; }

		public List<SelectListItem> Industries { get; set; }

		public List<SelectListItem> Currencies { get; set; }
	}
}