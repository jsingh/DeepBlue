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
    public class DealDocumentTest : Base {
        public DeepBlue.Models.Entity.DealClosingCost  DefaultDealClosingCost { get; set; }

        public Mock<IDealClosingCostService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IDealClosingCostService>();

			DefaultDealClosingCost = new DeepBlue.Models.Entity.DealClosingCost(MockService.Object);
            MockService.Setup(x => x.SaveDealClosingCost(It.IsAny<DeepBlue.Models.Entity.DealClosingCost >()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealClosingCost dealclosingcost, bool ifValid) {
			RequiredFieldDataMissing(dealclosingcost, ifValid);
			StringLengthInvalidData(dealclosingcost, ifValid);

        }

        #region DealClosingCost
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealClosingCost dealclosingcost, bool ifValidData) {
            if (ifValidData) {
				dealclosingcost.Amount = 1000;
				dealclosingcost.Date = DateTime.Now ;
				dealclosingcost.DealID = 1;
				dealclosingcost.DealClosingCostTypeID =1;  
            } else {
				dealclosingcost.Amount = 0;
				dealclosingcost.Date = DateTime.MinValue;
				dealclosingcost.DealID = 0;
				dealclosingcost.DealClosingCostTypeID = 0;  
            }
        }

		private void StringLengthInvalidData(DeepBlue.Models.Entity.DealClosingCost dealclosingcost, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }			
        }
        #endregion

    }
}