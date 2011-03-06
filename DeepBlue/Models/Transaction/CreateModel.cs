﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Transaction {

	public class CreateModel {
		public CreateModel() {
			FundNames = new List<SelectListItem>();
			InvestorTypes = new List<SelectListItem>();
			EditCommitmentAmountModel = new EditCommitmentAmountModel();
			FundClosings = new List<SelectListItem>();
		}

		[DisplayName("Investor:")]
		public int InvestorId { get; set; }

		[DisplayName("Investor Name:")]
		public string InvestorName { get; set; }

		[DisplayName("Display Name:")]
		public string DisplayName { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Name is required")]
		[DisplayName("Fund Name:")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Fund Close is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Close is required")]
		[DisplayName("Fund Close:")]
		public int FundClosingId { get; set; }

		[Required(ErrorMessage = "Investor Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor Type is required")]
		[DisplayName("Investor Type:")]
		public int InvestorTypeId { get; set; }

		[Required(ErrorMessage = "Committed Amount is required")]
		[Range(1, Double.MaxValue, ErrorMessage = "Invalid Committed Amount")]
		[RegularExpression("^(\\+|-)?[0-9][0-9]*(\\.[0-9]*)?$", ErrorMessage = "Invalid Committed Amount")]
		[DisplayName("Committed Amount:")]
		public decimal TotalCommitment { get; set; }

		[Required(ErrorMessage = "Committed Date is required")]
		[DisplayName("Committed Date:")]
		public DateTime CommittedDate { get; set; }

		public List<SelectListItem> FundNames { get; set; }

		public List<SelectListItem> FundClosings { get; set; }

		public List<SelectListItem> InvestorTypes { get; set; }

		public EditCommitmentAmountModel EditCommitmentAmountModel { get; set; }
	}

}