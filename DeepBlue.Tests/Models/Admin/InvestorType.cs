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
    public class InvestorTypeTest : Base {
		public DeepBlue.Models.Entity.InvestorType DefaultInvestorType { get; set; }

        public Mock<IInvestorTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IInvestorTypeService>();

			DefaultInvestorType = new DeepBlue.Models.Entity.InvestorType(MockService.Object);
            MockService.Setup(x => x.SaveInvestorType(It.IsAny<DeepBlue.Models.Entity.InvestorType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.InvestorType investorType, bool ifValid) {
			RequiredFieldDataMissing(investorType, ifValid);
			StringLengthInvalidData(investorType, ifValid);			
		}

		#region InvestorType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.InvestorType investorType, bool ifValidData) {
			if (ifValidData) {
				investorType.InvestorTypeName = "InvestorTypeName";
			}
			else{
				investorType.InvestorTypeName = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.InvestorType investorType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			investorType.InvestorTypeName = GetString(20 + delta);
		}
		#endregion
    }
}