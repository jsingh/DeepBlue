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
    public class DealClosingTest : Base {
        public DeepBlue.Models.Entity.DealClosing DefaultDealClosing { get; set; }

        public Mock<IDealClosingService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealClosingService>();

			DefaultDealClosing = new DeepBlue.Models.Entity.DealClosing(MockService.Object);
            MockService.Setup(x => x.SaveDealClosing(It.IsAny<DeepBlue.Models.Entity.DealClosing>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealClosing dealClosing, bool ifValid) {
			RequiredFieldDataMissing(dealClosing, ifValid);
        }

        #region DealClosing
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealClosing dealClosing, bool ifValidData) {
            if (ifValidData) {
				dealClosing.DealID = 1;
				dealClosing.CloseDate = DateTime.MaxValue;
            } else {
				dealClosing.DealID = 0;
				dealClosing.CloseDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}