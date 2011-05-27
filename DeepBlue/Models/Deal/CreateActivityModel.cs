using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {

	public class CreateActivityModel {

		public CreateActivityModel() {
			UnderlyingFundCapitalCallModel = new UnderlyingFundCapitalCallModel();
			UnderlyingFundPostRecordCapitalCallModel = new UnderlyingFundPostRecordCapitalCallModel();
			UnderlyingFundCashDistributionModel = new UnderlyingFundCashDistributionModel();
			UnderlyingFundPostRecordCashDistributionModel = new UnderlyingFundPostRecordCashDistributionModel();
			UnderlyingFundValuationModel = new UnderlyingFundValuationModel();
		}

		public UnderlyingFundCapitalCallModel UnderlyingFundCapitalCallModel { get; set; }

		public UnderlyingFundPostRecordCapitalCallModel UnderlyingFundPostRecordCapitalCallModel { get; set; }

		public UnderlyingFundCashDistributionModel UnderlyingFundCashDistributionModel { get; set; }

		public UnderlyingFundPostRecordCashDistributionModel UnderlyingFundPostRecordCashDistributionModel { get; set; }

		public UnderlyingFundValuationModel UnderlyingFundValuationModel { get; set; }
	
	}
}