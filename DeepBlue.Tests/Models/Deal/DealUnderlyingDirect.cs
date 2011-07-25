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
    public class DealUnderlyingDirectTest : Base {
        public DeepBlue.Models.Entity.DealUnderlyingDirect DefaultDealUnderlyingDirect { get; set; }

        public Mock<IDealUnderlyingDirectService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealUnderlyingDirectService>();

			DefaultDealUnderlyingDirect = new DeepBlue.Models.Entity.DealUnderlyingDirect(MockService.Object);
            MockService.Setup(x => x.SaveDealUnderlyingDirect(It.IsAny<DeepBlue.Models.Entity.DealUnderlyingDirect>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealUnderlyingDirect dealUnderlyingDirect, bool ifValid) {
			RequiredFieldDataMissing(dealUnderlyingDirect, ifValid);
        }

        #region DealSeller
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealUnderlyingDirect dealUnderlyingDirect, bool ifValidData) {
            if (ifValidData) {
				dealUnderlyingDirect.DealID = 1;
				dealUnderlyingDirect.RecordDate = DateTime.Now;
				dealUnderlyingDirect.SecurityID = 1;
				dealUnderlyingDirect.SecurityTypeID = 1;
				dealUnderlyingDirect.FMV = 1;
				dealUnderlyingDirect.PurchasePrice = 1;
				dealUnderlyingDirect.NumberOfShares = 1;
            } else {
				dealUnderlyingDirect.DealID = 0;
				dealUnderlyingDirect.RecordDate = DateTime.MinValue;
				dealUnderlyingDirect.SecurityID = 0;
				dealUnderlyingDirect.SecurityTypeID = 0;
				dealUnderlyingDirect.FMV = 0;
				dealUnderlyingDirect.PurchasePrice = 0;
				dealUnderlyingDirect.NumberOfShares = 0;
            }
        }
        #endregion

    }
}