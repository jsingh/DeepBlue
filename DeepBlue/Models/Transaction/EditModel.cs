using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Transaction {
	public class EditModel {

		public int InvestorId { get; set; }

		public int InvestorFundId { get; set; }

		public int InvestorFundTransactionId { get; set; }

		public int TransactionTypeId { get; set; }

		[DisplayName("Investor Name:")]
		public string InvestorName { get; set; }

		[DisplayName("Original Commitment Amount:")]
		public decimal OriginalCommitmentAmount { get; set; }

		[DisplayName("Unfunded Amount:")]
		public decimal UnfundedAmount { get; set; }

		[Required(ErrorMessage = "Required")]
		[Range(1,Double.MaxValue,ErrorMessage="Required")]
		[DisplayName("Commitment Amount:")]
		public decimal CommitmentAmount { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Date:")]
		public DateTime Date { get; set; }
				
		[DisplayName("Counterparty Investor:")]
		public string CounterPartyInvestor { get; set; }

		[DisplayName("Notes:")]
		public string Notes { get; set; }

		[Required(ErrorMessage = "Required")]
		[Range((int)ConfigUtil.IDStartRange, Double.MaxValue, ErrorMessage = "Required")]
		public int CounterPartyInvestorId { get; set; }

		public string CounterPartyInvestorName { get; set; }

		[DisplayName("Commitment Amount:")]
		public decimal CounterPartyInvestorCommitmentAmount { get; set; }

		public bool TransactionSuccess { get; set; }

	}

	public class EditCommitmentAmountModel{
		
		public int InvestorFundId { get; set; }

		[Required(ErrorMessage = "Required")]
		[Range(1, Double.MaxValue, ErrorMessage = "Required")]
		[DisplayName("Commitment Amount:")]
		public decimal CommitmentAmount { get; set; }

		public decimal UnfundedAmount { get; set; }
	}
}