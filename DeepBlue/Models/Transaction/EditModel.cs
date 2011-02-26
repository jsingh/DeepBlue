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

		[Required(ErrorMessage = "Required")]
		[Range(1,Double.MaxValue,ErrorMessage="Required")]
		[DisplayName("Commitment Amount:")]
		public decimal CommitmentAmount { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Date:")]
		public string Date { get; set; }

		[DisplayName("Counter Party:")]
		public string CounterParty { get; set; }

		[DisplayName("Notes:")]
		public string Notes { get; set; }
		
		public int OtherInvestorId { get; set; }

		public string OtherInvestorName { get; set; }

		[DisplayName("Commitment Amount:")]
		public decimal OtherInvestorCommitmentAmount { get; set; }
	}

	public class EditCommitmentAmountModel{
		
		public int InvestorFundId { get; set; }

		[Required(ErrorMessage = "Required")]
		[Range(1, Double.MaxValue, ErrorMessage = "Required")]
		[DisplayName("Commitment Amount:")]
		public decimal CommitmentAmount { get; set; }
	}
}