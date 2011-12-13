using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {

	public class CreateActivityModel {

		public CreateActivityModel() {
			UnderlyingFundCapitalCallModel = new UnderlyingFundCapitalCallModel();
			UnderlyingFundPostRecordCapitalCallModel = new UnderlyingFundPostRecordCapitalCallModel();
			UnderlyingFundCashDistributionModel = new UnderlyingFundCashDistributionModel();
			UnderlyingFundPostRecordCashDistributionModel = new UnderlyingFundPostRecordCashDistributionModel();
			UnderlyingFundValuationModel = new UnderlyingFundValuationModel();
			EquitySplitModel = new EquitySplitModel();
			SecurityConversionModel = new SecurityConversionModel();
			FundLevelExpenseModel = new FundExpenseModel();
			UnderlyingDirectValuationModel = new UnderlyingDirectValuationModel();
			UnfundedAdjustmentModel = new UnfundedAdjustmentModel();
			DividendDistributionModel = new DividendDistributionModel();
			PostRecordDividendDistributionModel = new PostRecordDividendDistributionModel();
		}

		public UnderlyingFundCapitalCallModel UnderlyingFundCapitalCallModel { get; set; }

		public UnderlyingFundPostRecordCapitalCallModel UnderlyingFundPostRecordCapitalCallModel { get; set; }

		public UnderlyingFundCashDistributionModel UnderlyingFundCashDistributionModel { get; set; }

		public UnderlyingFundPostRecordCashDistributionModel UnderlyingFundPostRecordCashDistributionModel { get; set; }

		public UnderlyingFundValuationModel UnderlyingFundValuationModel { get; set; }

		public DividendDistributionModel DividendDistributionModel { get; set; }

		public PostRecordDividendDistributionModel PostRecordDividendDistributionModel { get; set; }

		public EquitySplitModel EquitySplitModel { get; set; }

		public SecurityConversionModel SecurityConversionModel { get; set; }

		public FundExpenseModel FundLevelExpenseModel { get; set; }

		public UnderlyingDirectValuationModel UnderlyingDirectValuationModel { get; set; }

		public UnfundedAdjustmentModel UnfundedAdjustmentModel { get; set; }

		public List<SelectListItem> ActivityTypes { get; set; }
	
	}
}