using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Admin {
	public class ScheduleK1Model {
		 

		public string FundName { get; set; }

		public string UnderlyingFundName { get; set; }

		#region Primitive Properties

		public int? PartnersShareFormID {
			get;
			set;
		}

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundID {
			get;
			set;
		}


		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundID {
			get;
			set;
		}


		[StringLength(50, ErrorMessage = "PartnershipEIN must be under 50 characters.")]
		public string PartnershipEIN {
			get;
			set;
		}


		[StringLength(100, ErrorMessage = "IRSCenter must be under 100 characters.")]
		public string IRSCenter {
			get;
			set;
		}

	 
		public bool IsPTP {
			get;
			set;
		}

		public int? PartnerAddressID {
			get;
			set;
		}

		[StringLength(50, ErrorMessage = "PartnerEIN must be under 50 characters.")]
		public string PartnerEIN {
			get;
			set;
		}


		public bool IsGeneralPartner {
			get;
			set;
		}


		public bool IsLimitedPartner {
			get;
			set;
		}


		public bool IsDomesticPartner {
			get;
			set;
		}


		public bool IsForeignPartner {
			get;
			set;
		}


		[StringLength(50, ErrorMessage = "PartnerType must be under 50 characters.")]
		public string PartnerType {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingProfit is required")]
		public decimal? BeginingProfit {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingProfit is required")]
		public decimal? EndingProfit {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingLoss is required")]
		public decimal? BeginingLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingLoss is required")]
		public decimal? EndingLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingCapital is required")]
		public decimal? BeginingCapital {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingCapital is required")]
		public decimal? EndingCapital {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NonRecourse is required")]
		public decimal? NonRecourse {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "QualifiedNonRecourseFinancing is required")]
		public decimal? QualifiedNonRecourseFinancing {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Recourse is required")]
		public decimal? Recourse {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginningCapitalAccount is required")]
		public decimal? BeginningCapitalAccount {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CapitalContributed is required")]
		public decimal? CapitalContributed {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CurrentYearIncrease is required")]
		public decimal? CurrentYearIncrease {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "WithdrawalsAndDistributions is required")]
		public decimal? WithdrawalsAndDistributions {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingCapitalAccount is required")]
		public decimal? EndingCapitalAccount {
			get;
			set;
		}

		public bool IsTaxBasis {
			get;
			set;
		}


		public bool IsGAAP {
			get;
			set;
		}


		public bool IsSection704 {
			get;
			set;
		}


		public bool IsOther {
			get;
			set;
		}


		public bool IsGain {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OrdinaryBusinessIncome is required")]
		public decimal? OrdinaryBusinessIncome {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetRentalRealEstateIncome is required")]
		public decimal? NetRentalRealEstateIncome {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherNetRentalIncomeLoss is required")]
		public decimal? OtherNetRentalIncomeLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "GuaranteedPayment is required")]
		public decimal? GuaranteedPayment {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "InterestIncome is required")]
		public decimal? InterestIncome {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OrdinaryDividends is required")]
		public decimal? OrdinaryDividends {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "QualifiedDividends is required")]
		public decimal? QualifiedDividends {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Royalties is required")]
		public decimal? Royalties {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetShortTermCapitalGainLoss is required")]
		public decimal? NetShortTermCapitalGainLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetLongTermCapitalGainLoss is required")]
		public decimal? NetLongTermCapitalGainLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Collectibles28GainLoss is required")]
		public decimal? Collectibles28GainLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "UnrecapturedSection1250Gain is required")]
		public decimal? UnrecapturedSection1250Gain {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetSection1231GainLoss is required")]
		public decimal? NetSection1231GainLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherIncomeLoss is required")]
		public decimal? OtherIncomeLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Section179Deduction is required")]
		public decimal? Section179Deduction {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherDeduction is required")]
		public decimal? OtherDeduction {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "SelfEmploymentEarningLoss is required")]
		public decimal? SelfEmploymentEarningLoss {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Credits is required")]
		public decimal? Credits {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "ForeignTransaction is required")]
		public decimal? ForeignTransaction {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "AlternativeMinimumTax is required")]
		public decimal? AlternativeMinimumTax {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "TaxExemptIncome is required")]
		public decimal? TaxExemptIncome {
			get;
			set;
		}


		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Distribution is required")]
		public decimal? Distribution {
			get;
			set;
		}


		public string OtherInformation {
			get;
			set;
		}

		#endregion


		#region Partnership Address

		public string PartnershipAddress1 { get; set; }

		public string PartnershipAddress2 { get; set; }

		public string PartnershipCity { get; set; }

		public string PartnershipStateName { get; set; }

		public string PartnershipZip { get; set; }

		public string PartnershipCountryName { get; set; }

		#endregion

		#region Partner Address

		[StringLength(40, ErrorMessage = "Partner Address1 must be under 40 characters.")]
		[DisplayName("Address1")]
		public string PartnerAddress1 { get; set; }

		[DisplayName("Address2")]
		public string PartnerAddress2 { get; set; }

		[Required(ErrorMessage = "Partner City is required")]
		[StringLength(30, ErrorMessage = "Partner City must be under 30 characters.")]
		public string PartnerCity { get; set; }

		[Required(ErrorMessage = "Partner State is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Partner State is required")]
		[DisplayName("State")]
		public int? PartnerState { get; set; }

		public string PartnerStateName { get; set; }

		[DisplayName("Zip")]
		[Zip(ErrorMessage = "Invalid Partner Zip")]
		public string PartnerZip { get; set; }

		[DisplayName("Country")]
		[Required(ErrorMessage = "Partner Country is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Partner Country is required")]
		public int PartnerCountry { get; set; }

		public string PartnerCountryName { get; set; }

		#endregion

	}
}