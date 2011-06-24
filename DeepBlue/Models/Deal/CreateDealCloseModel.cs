using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class CreateDealCloseModel {
		public CreateDealCloseModel() {
			DealUnderlyingFunds = new List<DealUnderlyingFundModel>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectModel>();
		}

		public int DealClosingId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Deal Close Number is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Close Number is required")]
		[DisplayName("Deal Close #:")]
		public int? DealNumber { get; set; }

		[Required(ErrorMessage = "Close Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Close Date:")]
		public DateTime CloseDate { get; set; }

		[DisplayName("Final Close:")]
		public bool IsFinalClose { get; set; }

        public string DealName { get; set; }

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }

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

		#region FinalClose

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