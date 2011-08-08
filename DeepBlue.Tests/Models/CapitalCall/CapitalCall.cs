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
	public class CapitalCallTest : Base {
		public DeepBlue.Models.Entity.CapitalCall DefaultCapitalCallReqular { get; set; }

		public Mock<ICapitalCallService> MockService { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockService = new Mock<ICapitalCallService>();

			DefaultCapitalCallReqular = new DeepBlue.Models.Entity.CapitalCall(MockService.Object);
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

		#region CapitalCallRequiredFieldData
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalCall capitalcall, bool ifValidData) {
			if (ifValidData) {
				capitalcall.FundID = 1;
				capitalcall.CapitalCallDate = DateTime.Now;
				capitalcall.CapitalCallDueDate = DateTime.Now;
				capitalcall.CapitalAmountCalled = 1;
				capitalcall.InvestmentAmount = 1;
				capitalcall.ManagementFeeStartDate = DateTime.Now;
				capitalcall.ManagementFeeEndDate = DateTime.Now;
				capitalcall.CapitalCallTypeID = 1;
				capitalcall.CreatedDate = DateTime.Now;
				capitalcall.CreatedBy = 1;
				capitalcall.LastUpdatedDate = DateTime.Now;
				capitalcall.LastUpdatedBy = 1;

			}
			else {
				capitalcall.FundID = 0;
				capitalcall.CapitalCallDate = DateTime.MinValue;
				capitalcall.CapitalCallDueDate = DateTime.MinValue;
				capitalcall.CapitalAmountCalled = 0;
				capitalcall.InvestmentAmount = 0;
				capitalcall.ManagementFeeStartDate = DateTime.MinValue;
				capitalcall.ManagementFeeEndDate = DateTime.MinValue;
				capitalcall.CapitalCallTypeID = 0;
				capitalcall.CreatedDate = DateTime.MinValue;
				capitalcall.CreatedBy = 0;
				capitalcall.LastUpdatedDate = DateTime.MinValue;
				capitalcall.LastUpdatedBy = 0;

			}
		}
		#endregion

		#region CapitalCallStringLengthInvalidData
		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalCall capitalcall, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			capitalcall.CapitalCallNumber = GetString(50 + delta);

		}
		#endregion
	}
}