using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(PartnersShareFormMD))]
	public partial class PartnersShareForm {
		public class PartnersShareFormMD {

			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}


			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "PartnershipEIN must be under 50 characters.")]
			public global::System.String PartnershipEIN {
				get;
				set;
			}


			[StringLength(100, ErrorMessage = "IRSCenter must be under 100 characters.")]
			public global::System.String IRSCenter {
				get;
				set;
			}

			[Required(ErrorMessage = "PartnerAddressID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PartnerAddressID is required")]
			public global::System.Int32 PartnerAddressID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "PartnerEIN must be under 50 characters.")]
			public global::System.String PartnerEIN {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "PartnerType must be under 50 characters.")]
			public global::System.String PartnerType {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingProfit is required")]
			public Nullable<global::System.Decimal> BeginingProfit {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingProfit is required")]
			public Nullable<global::System.Decimal> EndingProfit {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingLoss is required")]
			public Nullable<global::System.Decimal> BeginingLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingLoss is required")]
			public Nullable<global::System.Decimal> EndingLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginingCapital is required")]
			public Nullable<global::System.Decimal> BeginingCapital {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingCapital is required")]
			public Nullable<global::System.Decimal> EndingCapital {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NonRecourse is required")]
			public Nullable<global::System.Decimal> NonRecourse {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "QualifiedNonRecourseFinancing is required")]
			public Nullable<global::System.Decimal> QualifiedNonRecourseFinancing {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Recourse is required")]
			public Nullable<global::System.Decimal> Recourse {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "BeginningCapitalAccount is required")]
			public Nullable<global::System.Decimal> BeginningCapitalAccount {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CapitalContributed is required")]
			public Nullable<global::System.Decimal> CapitalContributed {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CurrentYearIncrease is required")]
			public Nullable<global::System.Decimal> CurrentYearIncrease {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "WithdrawalsAndDistributions is required")]
			public Nullable<global::System.Decimal> WithdrawalsAndDistributions {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "EndingCapitalAccount is required")]
			public Nullable<global::System.Decimal> EndingCapitalAccount {
				get;
				set;
			}

			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OrdinaryBusinessIncome is required")]
			public Nullable<global::System.Decimal> OrdinaryBusinessIncome {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetRentalRealEstateIncome is required")]
			public Nullable<global::System.Decimal> NetRentalRealEstateIncome {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherNetRentalIncomeLoss is required")]
			public Nullable<global::System.Decimal> OtherNetRentalIncomeLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "GuaranteedPayment is required")]
			public Nullable<global::System.Decimal> GuaranteedPayment {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "InterestIncome is required")]
			public Nullable<global::System.Decimal> InterestIncome {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OrdinaryDividends is required")]
			public Nullable<global::System.Decimal> OrdinaryDividends {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "QualifiedDividends is required")]
			public Nullable<global::System.Decimal> QualifiedDividends {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Royalties is required")]
			public Nullable<global::System.Decimal> Royalties {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetShortTermCapitalGainLoss is required")]
			public Nullable<global::System.Decimal> NetShortTermCapitalGainLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetLongTermCapitalGainLoss is required")]
			public Nullable<global::System.Decimal> NetLongTermCapitalGainLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Collectibles28GainLoss is required")]
			public Nullable<global::System.Decimal> Collectibles28GainLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "UnrecapturedSection1250Gain is required")]
			public Nullable<global::System.Decimal> UnrecapturedSection1250Gain {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NetSection1231GainLoss is required")]
			public Nullable<global::System.Decimal> NetSection1231GainLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherIncomeLoss is required")]
			public Nullable<global::System.Decimal> OtherIncomeLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Section179Deduction is required")]
			public Nullable<global::System.Decimal> Section179Deduction {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OtherDeduction is required")]
			public Nullable<global::System.Decimal> OtherDeduction {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "SelfEmploymentEarningLoss is required")]
			public Nullable<global::System.Decimal> SelfEmploymentEarningLoss {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Credits is required")]
			public Nullable<global::System.Decimal> Credits {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "ForeignTransaction is required")]
			public Nullable<global::System.Decimal> ForeignTransaction {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "AlternativeMinimumTax is required")]
			public Nullable<global::System.Decimal> AlternativeMinimumTax {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "TaxExemptIncome is required")]
			public Nullable<global::System.Decimal> TaxExemptIncome {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Distribution is required")]
			public Nullable<global::System.Decimal> Distribution {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedDate is required")]
			[DateRange(ErrorMessage = "CreatedDate is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[DateRange(ErrorMessage = "LastUpdatedDate is required")]
			public Nullable<global::System.DateTime> LastUpdatedDate {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public Nullable<global::System.Int32> LastUpdatedBy {
				get;
				set;
			}
 
			#endregion
		}
		public PartnersShareForm(IPartnersShareFormService partnersShareFormService)
			: this() {
			this.PartnersShareFormService = partnersShareFormService;
		}

		public PartnersShareForm() {
		}

		private IPartnersShareFormService _PartnersShareFormService;
		public IPartnersShareFormService PartnersShareFormService {
			get {
				if (_PartnersShareFormService == null) {
					_PartnersShareFormService = new PartnersShareFormService();
				}
				return _PartnersShareFormService;
			}
			set {
				_PartnersShareFormService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			PartnersShareFormService.SavePartnersShareForm(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(PartnersShareForm partnersShareForm) {
			return ValidationHelper.Validate(partnersShareForm);
		}
	}
}