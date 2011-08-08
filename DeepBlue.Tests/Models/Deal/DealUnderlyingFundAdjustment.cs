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
    public class DealUnderlyingFundAdjustmentTest : Base {
        public DeepBlue.Models.Entity.DealUnderlyingFundAdjustment DefaultDealUnderlyingFundAdjustment { get; set; }

        public Mock<IDealUnderlyingFundAdjustmentService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IDealUnderlyingFundAdjustmentService>();

			DefaultDealUnderlyingFundAdjustment = new DeepBlue.Models.Entity.DealUnderlyingFundAdjustment(MockService.Object);
            MockService.Setup(x => x.SaveDealUnderlyingFundAdjustment(It.IsAny<DeepBlue.Models.Entity.DealUnderlyingFundAdjustment>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.DealUnderlyingFundAdjustment dealUnderlyingFund, bool ifValid) {
			RequiredFieldDataMissing(dealUnderlyingFund, ifValid);
        }


		#region DealUnderlyingFundAdjustmentRequiredFieldData
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealUnderlyingFundAdjustment dealUnderlyingFundAdjustment, bool ifValidData) {
			if (ifValidData) {
				dealUnderlyingFundAdjustment.DealUnderlyingFundID = 1;
				dealUnderlyingFundAdjustment.CommitmentAmount = 1;
				dealUnderlyingFundAdjustment.UnfundedAmount = 1;
				dealUnderlyingFundAdjustment.CreatedBy = 1;
				dealUnderlyingFundAdjustment.CreatedDate = DateTime.Now;
				dealUnderlyingFundAdjustment.LastUpdatedDate = DateTime.Now;
				dealUnderlyingFundAdjustment.LastUpdatedBy = 1;
			}
			else {
				dealUnderlyingFundAdjustment.DealUnderlyingFundID = 0;
				dealUnderlyingFundAdjustment.CommitmentAmount = 0;
				dealUnderlyingFundAdjustment.UnfundedAmount = 0;
				dealUnderlyingFundAdjustment.CreatedBy = 0;
				dealUnderlyingFundAdjustment.CreatedDate = DateTime.MinValue;
				dealUnderlyingFundAdjustment.LastUpdatedDate = DateTime.MinValue;
				dealUnderlyingFundAdjustment.LastUpdatedBy = 0;
			}
		}
		#endregion
    }
}