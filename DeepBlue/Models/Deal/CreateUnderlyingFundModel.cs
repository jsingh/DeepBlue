﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class CreateUnderlyingFundModel {

		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[StringLength(100, ErrorMessage = "Fund Name must be under 100 characters.")]
		[RemoteUID_(Action = "UnderlyingFundNameAvailable", Controller = "Deal", ValidateParameterName = "FundName", Params = new string[] { "UnderlyingFundId" })]
		[DisplayName("Fund Name:")]
		public string FundName { get; set; }

		[DisplayName("Fund Legal Name:")]
		public string LegalFundName { get; set; }

		[DisplayName("Description:")]
		public string Description { get; set; }

		[DisplayName("Incentive Fee:")]
		public decimal? IncentiveFee { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Fiscal Year End:")]
		public DateTime? FiscalYearEnd { get; set; }

		[Required(ErrorMessage = "Fund Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Type is required")]
		[DisplayName("Fund Type:")]
		public int FundTypeId { get; set; }

		[Required(ErrorMessage = "Issuer is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Issuer is required")]
		[DisplayName("Issuer:")]
		public int IssuerId { get; set; }

		[DisplayName("Fees Included:")]
		public bool IsFeesIncluded { get; set; }

		[Range(short.MinValue, short.MaxValue)]
		[DisplayName("Vintage Year:")]
		public short? VintageYear { get; set; }

		[DisplayName("Total Size:")]
		public int? TotalSize { get; set; }

		[Range(short.MinValue, short.MaxValue)]
		[DisplayName("Termination Year:")]
		public short? TerminationYear { get; set; }

		[DisplayName("Industry:")]
		public int? IndustryId { get; set; }

		[DisplayName("Geography:")]
		public int? GeographyId { get; set; }

		[DisplayName("Reporting:")]
		public int? ReportingFrequencyId { get; set; }

		[DisplayName("Reporting Type:")]
		public int? ReportingTypeId { get; set; }

		[DisplayName("Fund Structure:")]
		public int? FundStructureId { get; set; }

		[DisplayName("Tax Rate:")]
		public decimal? TaxRate { get; set; }

		[DisplayName("Taxable:")]
		public bool Taxable { get; set; }

		[DisplayName("Management Fee:")]
		public decimal? ManagementFee { get; set; }

		[DisplayName("Exempt:")]
		public bool Exempt { get; set; }

		[DisplayName("Auditor Name:")]
		[StringLength(75, ErrorMessage = "Fund Name must be under 75 characters.")]
		public string AuditorName { get; set; }

		[DisplayName("Fund Registered Office:")]
		public int? FundRegisteredOfficeId { get; set; }

		[DisplayName("Investment Type:")]
		public int? InvestmentTypeId { get; set; }

		[DisplayName("Manager Contact:")]
		public int? ManagerContactId { get; set; }

		[DisplayName("Domestic:")]
		public bool IsDomestic { get; set; }
 
		/* Contact Info */

		public int ContactId { get; set; }

		[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
		[DisplayName("Contact Name:")]
		public string ContactName { get; set; }

		[StringLength(200, ErrorMessage = "Web Address must be under 200 characters.")]
		[DisplayName("Web Address:")]
		public string WebAddress { get; set; }

		[StringLength(200, ErrorMessage = "Mailing Address must be under 200 characters.")]
		[DisplayName("Mailing Address:")]
		public string Address { get; set; }

		[StringLength(200, ErrorMessage = "Phone must be under 200 characters.")]
		[DisplayName("Phone:")]
		public string Phone { get; set; }

		[StringLength(200, ErrorMessage = "Email must be under 200 characters.")]
		[DisplayName("Email:")]
		[EmailAttribute(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		/* Bank Info */

		[Required(ErrorMessage = "BankName is required")]
		[StringLength(50, ErrorMessage = "Bank Name must be under 50 characters.")]
		[DisplayName("Bank Name:")]
		public string BankName { get; set; }

		[Required(ErrorMessage = "Account No is required")]
		[StringLength(40, ErrorMessage = "Account No must be under 40 characters.")]
		[DisplayName("Account No:")]
		public string Account { get; set; }

		[DisplayName("ABA#:")]
		public int? Routing { get; set; }

		[StringLength(50, ErrorMessage = "Account Of must be under 50 characters.")]
		[DisplayName("Account Of:")]
		public string AccountOf { get; set; }

		[StringLength(50, ErrorMessage = "Reference must be under 50 characters.")]
		[DisplayName("Reference:")]
		public string Reference { get; set; }

		[StringLength(50, ErrorMessage = "Attention must be under 50 characters.")]
		[DisplayName("Attention:")]
		public string Attention { get; set; }

		public List<SelectListItem> UnderlyingFundTypes { get; set; }

		public List<SelectListItem> Industries { get; set; }

		public List<SelectListItem> Geographyes { get; set; }

		public List<SelectListItem> ReportingTypes { get; set; }

		public List<SelectListItem> Reportings { get; set; }

		public List<SelectListItem> Issuers { get; set; }

		public List<SelectListItem> FundStructures { get; set; }

		public List<SelectListItem> FundRegisteredOffices { get; set; }

		public List<SelectListItem> InvestmentTypes { get; set; }

		public List<SelectListItem> ManagerContacts { get; set; }

	}
}