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

		[Required(ErrorMessage = "Deal Close Number is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Close Number is required")]
		[DisplayName("Deal Close #:")]
		public int? DealNumber { get; set; }

		[Required(ErrorMessage = "Close Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Close Date:")]
		public DateTime? CloseDate { get; set; }

		[DisplayName("Final Close:")]
		public bool IsFinalClose { get; set; }

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }

		public string TotalCA {
			get {
				return FormatHelper.CurrencyFormat(this.DealUnderlyingFunds.Sum(fund => fund.CommittedAmount));
			}
		}

		private decimal? TotalGrossPurchasePrice { get { return this.DealUnderlyingFunds.Sum(fund => fund.GrossPurchasePrice); } }

		private decimal? TotalPostRecordDateCapitalCall { get { return this.DealUnderlyingFunds.Sum(fund => fund.PostRecordDateCapitalCall); } }

		private decimal? TotalPostRecordDateDistribution { get { return this.DealUnderlyingFunds.Sum(fund => fund.PostRecordDateDistribution); } }

		public string TotalGPP {
			get {
				return FormatHelper.CurrencyFormat(this.TotalGrossPurchasePrice);
			}
		}

		public string TotalPRCC {
			get {
				return FormatHelper.CurrencyFormat(this.TotalPostRecordDateCapitalCall);
			}
		}

		public string TotalPRCD {
			get {
				return FormatHelper.CurrencyFormat(this.TotalPostRecordDateDistribution);
			}
		}

		public string TotalNPP {
			get {
				return FormatHelper.CurrencyFormat((this.TotalGrossPurchasePrice ?? 0) + ((this.TotalPostRecordDateCapitalCall ?? 0) - (this.TotalPostRecordDateDistribution ?? 0)));
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
	}

}