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

        protected void Create_Data(DeepBlue.Models.Entity.DealUnderlyingFund dealUnderlying, bool ifValid) {
			RequiredFieldDataMissing(dealUnderlying, ifValid);
			StringLengthInvalidData(dealUnderlying, ifValid);

        }

        #region DealSeller
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealUnderlyingFund dealUnderlying, bool ifValidData) {
            if (ifValidData) {
				dealUnderlying.UnderlyingFundID  = 1;
				dealUnderlying.DealID = 1;
				dealUnderlying.RecordDate = DateTime.Now;
            } else {
				dealUnderlying.UnderlyingFundID = 0;
				dealUnderlying.DealID = 0;
				dealUnderlying.RecordDate = DateTime.MinValue;
            }
        }

		private void StringLengthInvalidData(DeepBlue.Models.Entity.DealUnderlyingFund dealUnderlying, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
        }
        #endregion

    }
}