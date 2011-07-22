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
    public class UnderlyingFundCapitalCallTest : Base {
        public DeepBlue.Models.Entity.UnderlyingFundCapitalCall DefaultUnderlyingFundCapitalCall { get; set; }

        public Mock<IUnderlyingFundCapitalCallService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IUnderlyingFundCapitalCallService>();

			DefaultUnderlyingFundCapitalCall = new DeepBlue.Models.Entity.UnderlyingFundCapitalCall(MockService.Object);
            MockService.Setup(x => x.SaveUnderlyingFundCapitalCall(It.IsAny<DeepBlue.Models.Entity.UnderlyingFundCapitalCall>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.UnderlyingFundCapitalCall underlyingFundCapitalCall, bool ifValid) {
			RequiredFieldDataMissing(underlyingFundCapitalCall, ifValid);
        }

        #region UnderlyingFundCapitalCall
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingFundCapitalCall underlyingFundCapitalCall, bool ifValidData) {
            if (ifValidData) {
				underlyingFundCapitalCall.FundID = 1;
				underlyingFundCapitalCall.CreatedBy = 1;
				underlyingFundCapitalCall.CreatedDate = DateTime.MaxValue;
				underlyingFundCapitalCall.LastUpdatedBy = 1;
				underlyingFundCapitalCall.LastUpdatedDate = DateTime.MaxValue;
				underlyingFundCapitalCall.UnderlyingFundID = 1;
				underlyingFundCapitalCall.Amount = 1;
				underlyingFundCapitalCall.NoticeDate = DateTime.MaxValue;
				underlyingFundCapitalCall.ReceivedDate = DateTime.MaxValue;
            } else {
				underlyingFundCapitalCall.FundID = 0;
				underlyingFundCapitalCall.CreatedBy = 0;
				underlyingFundCapitalCall.CreatedDate = DateTime.MinValue;
				underlyingFundCapitalCall.LastUpdatedBy = 0;
				underlyingFundCapitalCall.LastUpdatedDate = DateTime.MinValue;
				underlyingFundCapitalCall.Amount = 0;
				underlyingFundCapitalCall.NoticeDate = DateTime.MinValue;
				underlyingFundCapitalCall.ReceivedDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}