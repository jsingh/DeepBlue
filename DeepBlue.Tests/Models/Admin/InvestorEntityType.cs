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
    public class InvestorEntityTypeTest : Base {
		public DeepBlue.Models.Entity.InvestorEntityType DefaultInvestorEntityType { get; set; }

        public Mock<IInvestorEntityTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IInvestorEntityTypeService>();

			DefaultInvestorEntityType = new DeepBlue.Models.Entity.InvestorEntityType(MockService.Object);
            MockService.Setup(x => x.SaveInvestorEntityType(It.IsAny<DeepBlue.Models.Entity.InvestorEntityType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.InvestorEntityType investorEntityType, bool ifValid) {
			RequiredFieldDataMissing(investorEntityType, ifValid);
			StringLengthInvalidData(investorEntityType, ifValid);			
		}

		#region InvestorEntityType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.InvestorEntityType investorEntityType, bool ifValidData) {
			if (ifValidData) {
				investorEntityType.InvestorEntityTypeName = "";
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.InvestorEntityType investorEntityType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			investorEntityType.InvestorEntityTypeName = GetString(20 + delta);
		}
		#endregion
    }
}