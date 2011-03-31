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
    public class CapitalCallDistributionTest : Base {
        public DeepBlue.Models.Entity.CapitalDistribution   DefaultCapitalCallDistribution { get; set; }

		public Mock<ICapitalDistributionService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICapitalDistributionService>();

			DefaultCapitalCallDistribution = new DeepBlue.Models.Entity.CapitalDistribution(MockService.Object);
			MockService.Setup(x => x.SaveCapitalDistribution(It.IsAny<DeepBlue.Models.Entity.CapitalDistribution>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.CapitalDistribution capitaldistribution, bool ifValid) {
			RequiredFieldDataMissing(capitaldistribution, ifValid);
			StringLengthInvalidData(capitaldistribution, ifValid);
        }

        #region CapitalCallDistribution
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalDistribution capitaldistribution, bool ifValidData) {
            if (ifValidData) {
				capitaldistribution.FundID = 1;
				capitaldistribution.DistributionAmount= 10000;
				capitaldistribution.CapitalDistributionDate = DateTime.Now;
				capitaldistribution.CapitalDistributionDueDate  = DateTime.Now;
				capitaldistribution.DistributionNumber="1";
            } else {
				capitaldistribution.FundID = 0;
				capitaldistribution.DistributionAmount = 0;
				capitaldistribution.CapitalDistributionDate = DateTime.MinValue;
				capitaldistribution.CapitalDistributionDueDate = DateTime.MinValue;
				capitaldistribution.DistributionNumber = string.Empty;
            }
        }

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalDistribution capitaldistribution, bool ifValidData) {
        }
        #endregion      
    }
}