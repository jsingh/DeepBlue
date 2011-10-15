using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.Web.Mvc;

namespace DeepBlue.Models.Transaction {
	public class EditModel {
		
		public EditModel(){
			InvestorTypes = new List<SelectListItem>();
			Date = DateTime.Now;
		}		
		
		public int InvestorId { get; set; }

		public int InvestorFundId { get; set; }

		public int FundId { get; set; }

		public int TransactionTypeId { get; set; }

		[DisplayName("Investor Name")]
		public string InvestorName { get; set; }

		[DisplayName("Original Commitment Amount")]
		public decimal OriginalCommitmentAmount { get; set; }

		[DisplayName("Unfunded Amount")]
		public decimal UnfundedAmount { get; set; }

		[Required(ErrorMessage = "Commitment Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Commitment Amount is required")]
		[DisplayName("Commitment Amount")]
		public decimal CommitmentAmount { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[DisplayName("Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DateRange()]
		public DateTime Date { get; set; }
				
		[DisplayName("Counterparty Investor")]
		public string CounterPartyInvestor { get; set; }

		[Required(ErrorMessage = "Investor Type is required")]
		[Range((int)ConfigUtil.IDStartRange, Double.MaxValue, ErrorMessage = "Investor Type is required")]
		[DisplayName("Investor Type")]
		public int InvestorTypeId { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }

		[Required(ErrorMessage = "Counterparty is required")]
		[Range((int)ConfigUtil.IDStartRange, Double.MaxValue, ErrorMessage = "Counterparty is required")]
		public int CounterPartyInvestorId { get; set; }

		public string CounterPartyInvestorName { get; set; }

		[DisplayName("Commitment Amount")]
		public decimal CounterPartyInvestorCommitmentAmount { get; set; }

		public List<SelectListItem> InvestorTypes { get; set; }
	}

	public class EditCommitmentAmountModel{
		
		public int InvestorFundId { get; set; }

		[Required(ErrorMessage = "Commitment Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Commitment Amount is required")]
		[DisplayName("Commitment Amount")]
		public decimal TotalCommitment { get; set; }

		public decimal UnfundedAmount { get; set; }
	}

	public class FundClosingDetail{
		public FundClosingDetail(){
			FundClosingId =0;
			Name = string.Empty;
		}
		public FundClosingDetail(int fundCloseId,string name) {
			FundClosingId = fundCloseId;
			Name = name;
		}
		public int FundClosingId { get; set; }
		public string Name { get; set; }
	}

	public class FundDetail{
		public FundDetail(){
			InvestorTypeId = 0;
			InvestorTypeName = string.Empty;
			FundClosingDetails = new List<FundClosingDetail>();
		}		
		
		public int InvestorTypeId { get; set; }

		public string InvestorTypeName { get; set; }

		public List<FundClosingDetail> FundClosingDetails { get; set; }
	}

	public class InvestorTypeDetail {
		public InvestorTypeDetail() {
			InvestorTypeId = 0;
			InvestorTypeName = string.Empty;
		}

		public int InvestorTypeId { get; set; }

		public string InvestorTypeName { get; set; }
	}
}