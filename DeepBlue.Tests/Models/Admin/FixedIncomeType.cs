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
    public class FixedIncomeTypeTest : Base {
		public DeepBlue.Models.Entity.FixedIncomeType DefaultFixedIncomeType { get; set; }

        public Mock<IFixedIncomeTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IFixedIncomeTypeService>();

			DefaultFixedIncomeType = new DeepBlue.Models.Entity.FixedIncomeType(MockService.Object);
            MockService.Setup(x => x.SaveFixedIncomeType(It.IsAny<DeepBlue.Models.Entity.FixedIncomeType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.FixedIncomeType fixedincometype, bool ifValid) {
			RequiredFieldDataMissing(fixedincometype, ifValid);
			StringLengthInvalidData(fixedincometype, ifValid);			
		}

		#region FixedIncomeType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.FixedIncomeType fixedincometype, bool ifValidData) {
			if (ifValidData) {
				fixedincometype.FixedIncomeType1 = "FixedIncomeType1";
			}
			else{
				fixedincometype.FixedIncomeType1 = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.FixedIncomeType fixedincometype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			fixedincometype.FixedIncomeType1 = GetString(100 + delta);
		}
		#endregion
    }
}