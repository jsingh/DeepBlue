using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class DealClosingCostTypeTest : Base {
		public DeepBlue.Models.Entity.DealClosingCostType  DefaultDealClosingCostType { get; set; }

        public Mock<IDealClosingCostTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IDealClosingCostTypeService>();

			DefaultDealClosingCostType = new DeepBlue.Models.Entity.DealClosingCostType(MockService.Object);
            MockService.Setup(x => x.SaveDealClosingCostType(It.IsAny<DeepBlue.Models.Entity.DealClosingCostType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.DealClosingCostType dealClosingCostType, bool ifValid) {
			RequiredFieldDataMissing(dealClosingCostType, ifValid);
			StringLengthInvalidData(dealClosingCostType, ifValid);			
		}

		#region DealClosingCostType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DealClosingCostType dealClosingCostType, bool ifValidData) {
			if (ifValidData) {
				dealClosingCostType.Name = "DealClosingCostTypeName";
			}
			else{
				dealClosingCostType.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.DealClosingCostType dealClosingCostType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			dealClosingCostType.Name = GetString(50 + delta);
		}
		#endregion
    }
}