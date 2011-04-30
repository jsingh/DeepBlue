using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class DealDetailModel {

		public DealDetailModel() {
			DealId = 0;
			FundId = 0;
			FundName = string.Empty;
			DealName = string.Empty;
			DealNumber = 0;
			ContactId = 0;
			PurchaseTypeId = 0;
			IsPartnered = false;
			PartnerName = string.Empty;
			SellerInfo = new DealSellerDetailModel();
			DealExpenses = new List<DealClosingCostModel>();
			DealUnderlyingFunds = new List<DealUnderlyingFundModel>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectModel>();
		}

		public int DealId { get; set; }
		
		[Required(ErrorMessage="Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund Name -")]
		public int FundId { get; set; }
		
		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		[StringLength(50, ErrorMessage = "Deal Name must be under 50 characters.")]
		[DisplayName("Deal Name -")]
		public string DealName { get; set; }

		[Required(ErrorMessage = "Deal Number is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Number is required")]
		[DisplayName("Deal No.-")]
		public int DealNumber { get; set; }

		[DisplayName("Contact-")]
		public int? ContactId { get; set; }

		[Required(ErrorMessage = "Purchase Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Purchase Type is required")]
		[DisplayName("Purchase Type-")]
		public int PurchaseTypeId { get; set; }

		[DisplayName("Partnered-")]
		public bool IsPartnered { get; set; }

		[DisplayName("Partner Name-")]
		public string PartnerName { get; set; }

		public DealSellerDetailModel SellerInfo { get; set; }

		public List<DealClosingCostModel> DealExpenses { get; set; }

		public List<DealUnderlyingFundModel> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectModel> DealUnderlyingDirects { get; set; }

	}
}