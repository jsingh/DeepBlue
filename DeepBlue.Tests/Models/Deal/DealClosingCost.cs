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
    public class DealClosingCostTest : Base {
        public DeepBlue.Models.Entity.DealClosingCost DefaultDealClosingCost { get; set; }

        public Mock<IDealClosingCostService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealClosingCostService>();

			DefaultDealClosingCost = new DeepBlue.Models.Entity.DealClosingCost(MockService.Object);
            MockService.Setup(x => x.SaveDealClosingCost(It.IsAny<DeepBlue.Models.Entity.DealClosingCost>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealClosingCost dealClosingCost, bool ifValid) {
			RequiredFieldDataMissing(dealClosingCost, ifValid);
        }

        #region DealClosingCost
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealClosingCost dealClosingCost, bool ifValidData) {
            if (ifValidData) {
				dealClosingCost.DealID = 1;
				dealClosingCost.Amount = 1;
				dealClosingCost.Date = DateTime.MaxValue;
				dealClosingCost.DealClosingCostTypeID = 1;
            } else {
				dealClosingCost.DealID = 0;
				dealClosingCost.Amount = 0;
				dealClosingCost.Date = DateTime.MinValue;
				dealClosingCost.DealClosingCostTypeID = 0;
            }
        }
        #endregion

    }
}