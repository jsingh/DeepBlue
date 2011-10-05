using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Investor
{
    public class InvestorLibraryModel  
    {
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor is required")]
		public int? InvestorID { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int? FundID { get; set; }

    }
 
	public class InvertorLibraryInformation {

		public string FundName { get; set; }

		public int FundID { get; set; }

		public IEnumerable<FundInvestorInformation> FundInformations { get; set; }

		public decimal? TotalCommitted {
			get {
				return FundInformations.Sum(investorFund => investorFund.CommitmentAmount);
			}
		}

	}

	public class InvestorList {

		public int InvestorID { get; set; }

		public string InvestorName { get; set; }
	}

	public class FundInvestorInformation : InvestorList  {

		public decimal? CommitmentAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public string FundClose { get; set; }

		public DateTime? CommittedDate { get; set; }

	}
}
