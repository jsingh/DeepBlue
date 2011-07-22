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
    public class DealSellerTest : Base {
        public DeepBlue.Models.Entity.Deal DefaultDeal { get; set; }

        public Mock<IDealService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealService>();

			DefaultDeal = new DeepBlue.Models.Entity.Deal(MockService.Object);
            MockService.Setup(x => x.SaveDeal(It.IsAny<DeepBlue.Models.Entity.Deal>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.Deal deal, bool ifValid) {
			RequiredFieldDataMissing(deal, ifValid);
			StringLengthInvalidData(deal, ifValid);

        }

        #region DealSeller
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Deal deal, bool ifValidData) {
            if (ifValidData) {
				deal.EntityID = 1;
				deal.FundID = 1;
				deal.DealNumber = 1;
				deal.PurchaseTypeID = 1;
			    deal.DealName = "Test";
				deal.CreatedDate = DateTime.Now;
				deal.CreatedBy = 1;  
            } else {
				deal.EntityID = 0;
				deal.FundID = 0;
				deal.DealNumber = 0;
				deal.PurchaseTypeID = 0;
				deal.DealName = string.Empty;
				deal.CreatedDate = DateTime.MinValue;
				deal.CreatedBy = 0;
            }
        }

        private void StringLengthInvalidData(DeepBlue.Models.Entity.Deal deal, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            deal.DealName = GetString(50 + delta);
			
        }
        #endregion

    }
}