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
    public class FundExpenseTypeTest : Base {
		public DeepBlue.Models.Entity.FundExpenseType DefaultFundExpenseType { get; set; }

        public Mock<IFundExpenseTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IFundExpenseTypeService>();

            DefaultFundExpenseType = new DeepBlue.Models.Entity.FundExpenseType(MockService.Object);
            MockService.Setup(x => x.SaveFundExpenseType(It.IsAny<DeepBlue.Models.Entity.FundExpenseType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.FundExpenseType fundExpenseType, bool ifValid) {
            RequiredFieldDataMissing(fundExpenseType, ifValid);
            StringLengthInvalidData(fundExpenseType, ifValid);			
		}

		#region FundExpenseType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.FundExpenseType fundExpenseType, bool ifValidData) {
			if (ifValidData) {
                fundExpenseType.Name = "FundExpenseTypeName";
			}
			else{
                fundExpenseType.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.FundExpenseType fundExpenseType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
            fundExpenseType.Name = GetString(50 + delta);
		}
		#endregion
    }
}