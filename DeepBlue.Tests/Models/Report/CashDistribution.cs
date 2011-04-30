using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Report {
    public class CashDistributionTest : Base {
        public DeepBlue.Models.Entity.CapitalDistribution  DefaultCashDistribution { get; set; }

		public Mock<ICapitalDistributionService> MockService { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockService = new Mock<ICapitalDistributionService>();

			DefaultCashDistribution = new DeepBlue.Models.Entity.CapitalDistribution(MockService.Object);
			MockService.Setup(x => x.SaveCapitalDistribution(It.IsAny<DeepBlue.Models.Entity.CapitalDistribution>()));
		}

		protected bool IsPropertyValid(string propertyName) {
			string errorMsg = string.Empty;
			int errorCount = 0;
			return IsModelValid(out errorMsg, out errorCount, propertyName);
		}

		protected void Create_Data(DeepBlue.Models.Entity.CapitalDistribution cashdistributionsummary, bool ifValid) {
			RequiredFieldDataMissing(cashdistributionsummary, ifValid);
			StringLengthInvalidData(cashdistributionsummary, ifValid);

		}

		#region CashDistributionSummary
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalDistribution cashdistributionsummary, bool ifValidData) {
			if (ifValidData) {
				cashdistributionsummary.FundID = 1;
				cashdistributionsummary.CapitalDistributionID  = 1;
			}
			else {
				cashdistributionsummary.FundID = 0;
				cashdistributionsummary.CapitalDistributionID = 0;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalDistribution cashdistributionsummary, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
		}
		#endregion

    }
}