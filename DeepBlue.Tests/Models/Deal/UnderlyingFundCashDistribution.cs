using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Deal {
    public class UnderlyingFundCashDistributionTest : Base {
        public DeepBlue.Models.Entity.UnderlyingFundCashDistribution DefaultUnderlyingFundCashDistribution { get; set; }

        public Mock<IUnderlyingFundCashDistributionService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IUnderlyingFundCashDistributionService>();

			DefaultUnderlyingFundCashDistribution = new DeepBlue.Models.Entity.UnderlyingFundCashDistribution(MockService.Object);
            MockService.Setup(x => x.SaveUnderlyingFundCashDistribution(It.IsAny<DeepBlue.Models.Entity.UnderlyingFundCashDistribution>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.UnderlyingFundCashDistribution underlyingFundCashDistribution, bool ifValid) {
			RequiredFieldDataMissing(underlyingFundCashDistribution, ifValid);
        }

        #region UnderlyingFundCashDistribution
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingFundCashDistribution underlyingFundCashDistribution, bool ifValidData) {
            if (ifValidData) {
				underlyingFundCashDistribution.FundID = 1;
				underlyingFundCashDistribution.CreatedBy = 1;
				underlyingFundCashDistribution.CreatedDate = DateTime.MaxValue;
				underlyingFundCashDistribution.LastUpdatedBy = 1;
				underlyingFundCashDistribution.LastUpdatedDate = DateTime.MaxValue;
				underlyingFundCashDistribution.UnderlyingFundID = 1;
				underlyingFundCashDistribution.Amount = 1;
				underlyingFundCashDistribution.NoticeDate = DateTime.MaxValue;
				underlyingFundCashDistribution.ReceivedDate = DateTime.MaxValue;
            } else {
				underlyingFundCashDistribution.FundID = 0;
				underlyingFundCashDistribution.CreatedBy = 0;
				underlyingFundCashDistribution.CreatedDate = DateTime.MinValue;
				underlyingFundCashDistribution.LastUpdatedBy = 0;
				underlyingFundCashDistribution.LastUpdatedDate = DateTime.MinValue;
				underlyingFundCashDistribution.Amount = 0;
				underlyingFundCashDistribution.NoticeDate = DateTime.MinValue;
				underlyingFundCashDistribution.ReceivedDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}