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

    public class CashDistributionTest : Base {
        public DeepBlue.Models.Entity.CashDistribution DefaultCashDistribution { get; set; }

        public Mock<ICashDistributionService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<ICashDistributionService>();

			DefaultCashDistribution = new DeepBlue.Models.Entity.CashDistribution(MockService.Object);
            MockService.Setup(x => x.SaveCashDistribution(It.IsAny<DeepBlue.Models.Entity.CashDistribution>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.CashDistribution cashDistribution, bool ifValid) {
			RequiredFieldDataMissing(cashDistribution, ifValid);
        }

        #region CashDistribution
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CashDistribution cashDistribution, bool ifValidData) {
            if (ifValidData) {
				cashDistribution.CreatedBy = 1;
				cashDistribution.CreatedDate = DateTime.MaxValue;
				cashDistribution.LastUpdatedBy = 1;
				cashDistribution.LastUpdatedDate = DateTime.MaxValue;
				cashDistribution.UnderlyingFundID = 1;
				cashDistribution.DealID = 1;
            } else {
				cashDistribution.CreatedBy = 0;
				cashDistribution.CreatedDate = DateTime.MinValue;
				cashDistribution.LastUpdatedBy = 0;
				cashDistribution.LastUpdatedDate = DateTime.MinValue;
				cashDistribution.UnderlyingFundID = 0;
				cashDistribution.DealID = 0;
            }
        }
        #endregion

    }
}