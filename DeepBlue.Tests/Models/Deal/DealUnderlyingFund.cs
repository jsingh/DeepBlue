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
    public class DealUnderlyingFundTest : Base {
        public DeepBlue.Models.Entity.DealUnderlyingFund DefaultDealUnderlyingFund { get; set; }

        public Mock<IDealUnderlyingFundService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealUnderlyingFundService>();

			DefaultDealUnderlyingFund = new DeepBlue.Models.Entity.DealUnderlyingFund(MockService.Object);
            MockService.Setup(x => x.SaveDealUnderlyingFund(It.IsAny<DeepBlue.Models.Entity.DealUnderlyingFund>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealUnderlyingFund dealUnderlyingFund, bool ifValid) {
			RequiredFieldDataMissing(dealUnderlyingFund, ifValid);
        }

        #region DealSeller
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealUnderlyingFund dealUnderlyingFund, bool ifValidData) {
            if (ifValidData) {
				dealUnderlyingFund.UnderlyingFundID  = 1;
				dealUnderlyingFund.DealID = 1;
				dealUnderlyingFund.RecordDate = DateTime.Now;
            } else {
				dealUnderlyingFund.UnderlyingFundID = 0;
				dealUnderlyingFund.DealID = 0;
				dealUnderlyingFund.RecordDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}