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
	public class CapitalDistributionTest : Base {
		public DeepBlue.Models.Entity.CapitalDistribution DefaultCapitalDistribution { get; set; }

		public Mock<ICapitalDistributionService> MockService { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockService = new Mock<ICapitalDistributionService>();

			DefaultCapitalDistribution = new DeepBlue.Models.Entity.CapitalDistribution(MockService.Object);
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

		#region CapitalDistributionRequiredFieldData
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CapitalDistribution capitalDistribution, bool ifValidData) {
			if (ifValidData) {
				capitalDistribution.CapitalDistributionDate = DateTime.Now;
				capitalDistribution.CapitalDistributionDueDate = DateTime.Now;
				capitalDistribution.FundID = 1;
				capitalDistribution.DistributionAmount = 1;
				capitalDistribution.IsManual = true;
				capitalDistribution.CreatedDate = DateTime.Now;
				capitalDistribution.CreatedBy = 1;
				capitalDistribution.LastUpdatedDate = DateTime.Now;
				capitalDistribution.LastUpdatedBy = 1;
			}
			else {
				capitalDistribution.CapitalDistributionDate = DateTime.MinValue;
				capitalDistribution.CapitalDistributionDueDate = DateTime.MinValue;
				capitalDistribution.FundID = 0;
				capitalDistribution.DistributionAmount = 0;
				capitalDistribution.IsManual = false;
				capitalDistribution.CreatedDate = DateTime.MinValue;
				capitalDistribution.CreatedBy = 0;
				capitalDistribution.LastUpdatedDate = DateTime.MinValue;
				capitalDistribution.LastUpdatedBy = 0;

			}
		}
		#endregion

		#region CapitalDistributionStringLengthInvalidData
		private void StringLengthInvalidData(DeepBlue.Models.Entity.CapitalDistribution capitalDistribution, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			capitalDistribution.DistributionNumber = GetString(50 + delta);

		}
		#endregion
	}
}