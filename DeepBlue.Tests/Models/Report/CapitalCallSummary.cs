using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Report {
    public class CapitalCallSummaryTest : Base {
        public DeepBlue.Models.Entity.CapitalCall  DefaultCapitalCallSummary { get; set; }

		public Mock<ICapitalCallService> MockService { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockService = new Mock<ICapitalCallService>();

			DefaultCapitalCallSummary = new DeepBlue.Models.Entity.CapitalCall(MockService.Object);
			MockService.Setup(x => x.SaveCapitalCall(It.IsAny<DeepBlue.Models.Entity.CapitalCall >()));
		}

		protected bool IsPropertyValid(string propertyName) {
			string errorMsg = string.Empty;
			int errorCount = 0;
			return IsModelValid(out errorMsg, out errorCount, propertyName);
		}

		protected void Create_Data(DeepBlue.Models.Entity.CapitalCall capitalcallsummary, bool ifValid) {
			RequiredFieldDataMissing(capitalcallsummary, ifValid);
			StringLengthInvalidData(capitalcallsummary, ifValid);

		}

		#region CapitalCallSummary
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalCall capitalcallsummary, bool ifValidData) {
			if (ifValidData) {
				capitalcallsummary.FundID = 1;
				capitalcallsummary.CapitalCallID = 1;
			}
			else {
				capitalcallsummary.FundID = 0;
				capitalcallsummary.CapitalCallID  = 0;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalCall capitalcallsummary, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
		}
		#endregion

    }
}