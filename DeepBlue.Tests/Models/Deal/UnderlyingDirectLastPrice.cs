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
    public class UnderlyingDirectLastPriceTest : Base {
        public DeepBlue.Models.Entity.UnderlyingDirectLastPrice DefaultUnderlyingDirectLastPrice { get; set; }

        public Mock<IUnderlyingDirectLastPriceService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IUnderlyingDirectLastPriceService>();

			DefaultUnderlyingDirectLastPrice = new DeepBlue.Models.Entity.UnderlyingDirectLastPrice(MockService.Object);
            MockService.Setup(x => x.SaveUnderlyingDirectLastPrice(It.IsAny<DeepBlue.Models.Entity.UnderlyingDirectLastPrice>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.UnderlyingDirectLastPrice underlyingDirectLastPrice, bool ifValid) {
			RequiredFieldDataMissing(underlyingDirectLastPrice, ifValid);
        }

        #region UnderlyingDirectLastPrice
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingDirectLastPrice underlyingDirectLastPrice, bool ifValidData) {
            if (ifValidData) {
				underlyingDirectLastPrice.FundID = 1;
				underlyingDirectLastPrice.SecurityID = 1;
				underlyingDirectLastPrice.SecurityTypeID = 1;
				underlyingDirectLastPrice.LastPrice = 1;
				underlyingDirectLastPrice.LastPriceDate = DateTime.MaxValue;
				underlyingDirectLastPrice.CreatedBy = 1;
				underlyingDirectLastPrice.CreatedDate = DateTime.MaxValue;
				underlyingDirectLastPrice.LastUpdatedBy = 1;
				underlyingDirectLastPrice.LastUpdatedDate = DateTime.MaxValue;
            } else {
				underlyingDirectLastPrice.FundID = 0;
				underlyingDirectLastPrice.SecurityID = 0;
				underlyingDirectLastPrice.SecurityTypeID = 0;
				underlyingDirectLastPrice.LastPrice = 0;
				underlyingDirectLastPrice.LastPriceDate = DateTime.MinValue;
				underlyingDirectLastPrice.CreatedBy = 0;
				underlyingDirectLastPrice.CreatedDate = DateTime.MinValue;
				underlyingDirectLastPrice.LastUpdatedBy = 0;
				underlyingDirectLastPrice.LastUpdatedDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}