using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeepBlue.Models.Transaction {
	public class CreateTransactionModel {

			public CreateTransactionModel(){
				FundNames = new List<SelectListItem>();
				FundClosings = new List<SelectListItem>(); 
				InvestorTypes = new List<SelectListItem>();
			}
				
			[Required(ErrorMessage="*")]
			[DisplayName("Investor:")]
			public int InvestorId { get; set; }

			[DisplayName("Investor Name:")]
			public string InvestorName { get; set; }

			[DisplayName("Display Name:")]
			public string DisplayName { get; set; }

			[Required(ErrorMessage = "*")]
			[DisplayName("Fund Name:")]
			public int FundId { get; set; }

			[Required(ErrorMessage = "*")]
			[DisplayName("Fund Close:")]
			public int FundClosingId { get; set; }

			[DisplayName("Investor Type:")]
			public int InvestorTypeId { get; set; }

			[Required(ErrorMessage = "*")]
			[DisplayName("Committed Amount:")]
			public decimal TotalCommitment { get; set; }

			[Required(ErrorMessage = "*")]
			[DisplayName("Committed Date:")]
			public string CommittedDate { get; set; }

			[DisplayName("Signed Agreement:")]
			public bool IsAgreementSigned { get; set; }

			public List<SelectListItem> FundNames { get; set; }

			public List<SelectListItem> FundClosings { get; set; }

			public List<SelectListItem> InvestorTypes { get; set; }
	}
}