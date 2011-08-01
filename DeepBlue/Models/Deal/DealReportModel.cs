using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class DealReportModel {

		public DealReportModel() {
			DealId = 0;
			DealNumber = 0;
			DealName = string.Empty;
			NetPurchasePrice = 0;
			CommittedAmount = 0;
			DealUnderlyingFunds = new List<DealUnderlyingFundModel>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectModel>();
		}

		public int DealId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public DateTime DealDate { get; set; }

		public decimal? NetPurchasePrice { get; set; }

		public decimal? GrossPurchasePrice { get; set; }

		public decimal? CommittedAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public decimal? TotalAmount { get; set; }

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }
	}

	public class DealUnderlyingDetail {

		public DealUnderlyingDetail() {
			DealUnderlyingFunds = new List<DealUnderlyingFundModel>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectModel>();
		}

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }

		public string TotalFundNAV { get { decimal totalFundNav = this.DealUnderlyingFunds.Sum(fund => fund.FundNAV) ?? 0; return (totalFundNav > 0 ? string.Format("{0:N2}",totalFundNav) : string.Empty); } }

		public string TotalCommitted { get { return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.CommittedAmount)); } }

		public string TotalUnfunded { get { return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.UnfundedAmount)); } }

		public string TotalPurchasePrice { get { return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Sum(direct => direct.PurchasePrice)); } }

		public string TotalFMV { get { return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Sum(direct => direct.FMV)); } }

	}


}