using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;
using DeepBlue.Models.Investor;

namespace DeepBlue.Models.Deal {

	public class CreateUnderlyingFundModel : AccountInformationModel {

		public CreateUnderlyingFundModel() {
			Country = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			CountryName = "United States";
		}

		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[StringLength(100, ErrorMessage = "Fund Name must be under 100 characters.")]
		[DisplayName("Fund Name")]
		public string FundName { get; set; }

		[StringLength(100, ErrorMessage = "Legal Fund Name must be under 100 characters.")]
		[DisplayName("Fund Legal Name")]
		public string LegalFundName { get; set; }

		[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
		[DisplayName("Description")]
		public string Description { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Fiscal Year End")]
		public DateTime? FiscalYearEnd { get; set; }

		[Required(ErrorMessage = "Fund Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Type is required")]
		[DisplayName("Fund Type")]
		public int FundTypeId { get; set; }

		public string FundType { get; set; }

		[Required(ErrorMessage = "GP is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "GP is required")]
		[DisplayName("Issuer")]
		public int IssuerId { get; set; }

		public string IssuerName { get; set; }

		[DisplayName("Fees Included")]
		public bool IsFeesIncluded { get; set; }

		[Range(short.MinValue, short.MaxValue)]
		[DisplayName("Vintage Year")]
		public short? VintageYear { get; set; }

		[DisplayName("Total Size")]
		public int? TotalSize { get; set; }

		[Range(short.MinValue, short.MaxValue)]
		[DisplayName("Termination Year")]
		public short? TerminationYear { get; set; }

		[DisplayName("Industry")]
		public int? IndustryId { get; set; }

		public string Industry { get; set; }

		[DisplayName("Geography")]
		public int? GeographyId { get; set; }

		public string Geography { get; set; }

		[DisplayName("Reporting")]
		public int? ReportingFrequencyId { get; set; }

		public string ReportingFrequency { get; set; }

		[DisplayName("Reporting Type")]
		public int? ReportingTypeId { get; set; }

		public string ReportingType { get; set; }

		[DisplayName("Fund Structure")]
		public int? FundStructureId { get; set; }

		[DisplayName("Taxable")]
		public bool Taxable { get; set; }

		[DisplayName("Tax Rate")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		[Range(typeof(decimal), "0", "100", ErrorMessage = "Tax Rate must be under 100%.")]
		public decimal? TaxRate { get; set; }

		[DisplayName("Management Fee")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		[Range(typeof(decimal), "0", "100", ErrorMessage = "Management Fee must be under 100%.")]
		public decimal? ManagementFee { get; set; }

		[DisplayName("Incentive Fee")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		[Range(typeof(decimal), "0", "100", ErrorMessage = "Incentive Fee must be under 100%.")]
		public decimal? IncentiveFee { get; set; }

		[DisplayName("Exempt")]
		public bool Exempt { get; set; }

		[DisplayName("Auditor Name")]
		[StringLength(75, ErrorMessage = "Fund Name must be under 75 characters.")]
		public string AuditorName { get; set; }

		[DisplayName("Fund Registered Office")]
		public int? FundRegisteredOfficeId { get; set; }

		[DisplayName("Investment Type")]
		public int? InvestmentTypeId { get; set; }

		[DisplayName("Manager Contact")]
		public int? ManagerContactId { get; set; }

		[DisplayName("Domestic")]
		public bool IsDomestic { get; set; }

		[StringLength(50, ErrorMessage = "Web UserName must be under 50 characters.")]
		public string WebUserName { get; set; }

		[StringLength(50, ErrorMessage = "Web Password must be under 50 characters.")]
		public string WebPassword { get; set; }

		[StringLength(100, ErrorMessage = "Website must be under 100 characters.")]
		[DisplayName("Website")]
		[WebAddress(ErrorMessage = "Invalid Website")]
		public string Website { get; set; }

		/* Address Information */
 
		[StringLength(40, ErrorMessage = "Address1 must be under 40 characters.")]
		[DisplayName("Address1")]
		public string Address1 { get; set; }

		[DisplayName("Address2")]
		public string Address2 { get; set; }

		[StringLength(30, ErrorMessage = "City must be under 30 characters.")]
		public string City { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State is required")]
		[DisplayName("State")]
		public int? State { get; set; }

		public string StateName { get; set; }

		[DisplayName("Zip")]
		[Zip(ErrorMessage = "Invalid Zip")]
		public string Zip { get; set; }

		[DisplayName("Country")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
		public int Country { get; set; }

		public string CountryName { get; set; }

		/* End Address Information */
	 

		public List<SelectListItem> UnderlyingFundTypes { get; set; }

		public List<SelectListItem> Industries { get; set; }

		public List<SelectListItem> Geographyes { get; set; }

		public List<SelectListItem> ReportingTypes { get; set; }

		public List<SelectListItem> Reportings { get; set; }

		public List<SelectListItem> FundStructures { get; set; }

		public List<SelectListItem> FundRegisteredOffices { get; set; }

		public List<SelectListItem> InvestmentTypes { get; set; }

		public List<SelectListItem> ManagerContacts { get; set; }

		public List<SelectListItem> DocumentTypes { get; set; }

		public List<SelectListItem> UploadTypes { get; set; }

		public string DocumentFileExtensions { get; set; }

	}
}