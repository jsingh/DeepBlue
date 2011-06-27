using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class FinalDealCloseModel {

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }

		#region CloseDetail

		public string TotalCA {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.CommittedAmount));
			}
		}

		public string TotalGPP {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.GrossPurchasePrice));
			}
		}

		public string TotalPRCC {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.PostRecordDateCapitalCall));
			}
		}

		public string TotalPRCD {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.PostRecordDateDistribution));
			}
		}

		public string TotalNPP {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.NetPurchasePrice));
			}
		}

		public string TotalNoOfShares {
			get {
				return FormatHelper.NumberFormat(this.DealUnderlyingDirects.Sum(direct => direct.NumberOfShares));
			}
		}

		public string TotalPurchasePrice {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Sum(direct => direct.PurchasePrice));
			}
		}

		public string TotalFMV {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Sum(direct => direct.FMV));
			}
		}

		#endregion

		#region FinalCloseDetail

		public string TotalRGPP {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Where(fund => fund.DealClosingId > 0).Sum(fund => fund.ReassignedGPP));
			}
		}

		public string TotalFinalPRCC {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Where(fund => fund.DealClosingId > 0).Sum(fund => fund.PostRecordDateCapitalCall));
			}
		}

		public string TotalFinalPRCD {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Where(fund => fund.DealClosingId > 0).Sum(fund => fund.PostRecordDateDistribution));
			}
		}

		public string TotalAJC {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Where(fund => fund.DealClosingId > 0).Sum(fund => fund.AdjustedCost));
			}
		}

		public string TotalFinalNoOfShares {
			get {
				return FormatHelper.NumberFormat(this.DealUnderlyingDirects.Where(fund => fund.DealClosingId > 0).Sum(direct => direct.NumberOfShares));
			}
		}

		public string TotalFinalPurchasePrice {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Where(fund => fund.DealClosingId > 0).Sum(direct => direct.PurchasePrice));
			}
		}

		public string TotalFinalFMV {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingDirects.Where(fund => fund.DealClosingId > 0).Sum(direct => direct.FMV));
			}
		}

		#endregion
	}

}