using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.CapitalCall {
    public class CapitalCallReceiveTest : Base {
        public DeepBlue.Models.Entity.CapitalCall  DefaultCapitalCallReceive { get; set; }

        public Mock<ICapitalCallService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<ICapitalCallService >();

			DefaultCapitalCallReceive = new DeepBlue.Models.Entity.CapitalCall(MockService.Object);
            MockService.Setup(x => x.SaveCapitalCall(It.IsAny<DeepBlue.Models.Entity.CapitalCall>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.CapitalCall capitalcall, bool ifValid) {
			RequiredFieldDataMissing(capitalcall, ifValid);
			StringLengthInvalidData(capitalcall, ifValid);
        }

        #region CapitalCallReceive
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalCall capitalcall, bool ifValidData) {
            if (ifValidData) {
				capitalcall.FundID = 1;
				capitalcall.CapitalAmountCalled = 10000;
				capitalcall.CapitalCallDate = DateTime.Now;
				capitalcall.CapitalCallDueDate = DateTime.Now;
            } else {
				capitalcall.FundID = 0;
				capitalcall.CapitalAmountCalled = 0;
				capitalcall.CapitalCallDate = DateTime.MinValue;
				capitalcall.CapitalCallDueDate = DateTime.MinValue;
            }
        }

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalCall capitalcall, bool ifValidData) {
        }
        #endregion      
    }
}