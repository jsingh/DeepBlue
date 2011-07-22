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

    public class UnderlyingFundCapitalCallLineItemTest : Base {
        public DeepBlue.Models.Entity.UnderlyingFundCapitalCallLineItem DefaultUnderlyingFundCapitalCallLineItem { get; set; }

        public Mock<IUnderlyingFundCapitalCallLineItemService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IUnderlyingFundCapitalCallLineItemService>();

			DefaultUnderlyingFundCapitalCallLineItem = new DeepBlue.Models.Entity.UnderlyingFundCapitalCallLineItem(MockService.Object);
            MockService.Setup(x => x.SaveUnderlyingFundCapitalCallLineItem(It.IsAny<DeepBlue.Models.Entity.UnderlyingFundCapitalCallLineItem>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem, bool ifValid) {
			RequiredFieldDataMissing(underlyingFundCapitalCallLineItem, ifValid);
        }

        #region UnderlyingFundCapitalCallLineItem
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem, bool ifValidData) {
            if (ifValidData) {
				underlyingFundCapitalCallLineItem.CreatedBy = 1;
				underlyingFundCapitalCallLineItem.CreatedDate = DateTime.MaxValue;
				underlyingFundCapitalCallLineItem.LastUpdatedBy = 1;
				underlyingFundCapitalCallLineItem.LastUpdatedDate = DateTime.MaxValue;
				underlyingFundCapitalCallLineItem.UnderlyingFundID = 1;
				underlyingFundCapitalCallLineItem.DealID = 1;
            } else {
				underlyingFundCapitalCallLineItem.CreatedBy = 0;
				underlyingFundCapitalCallLineItem.CreatedDate = DateTime.MinValue;
				underlyingFundCapitalCallLineItem.LastUpdatedBy = 0;
				underlyingFundCapitalCallLineItem.LastUpdatedDate = DateTime.MinValue;
				underlyingFundCapitalCallLineItem.UnderlyingFundID = 0;
				underlyingFundCapitalCallLineItem.DealID = 0;
            }
        }
        #endregion

    }
}