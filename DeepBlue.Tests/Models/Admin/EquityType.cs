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
    public class EquityTypeTest : Base {
		public DeepBlue.Models.Entity.EquityType DefaultEquityType { get; set; }

        public Mock<IEquityTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IEquityTypeService>();

			DefaultEquityType = new DeepBlue.Models.Entity.EquityType(MockService.Object);
            MockService.Setup(x => x.SaveEquityType(It.IsAny<DeepBlue.Models.Entity.EquityType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.EquityType equitytype, bool ifValid) {
			RequiredFieldDataMissing(equitytype, ifValid);
			StringLengthInvalidData(equitytype, ifValid);			
		}

		#region EquityType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.EquityType equitytype, bool ifValidData) {
			if (ifValidData) {
				equitytype.Equity = "Equity";
			}
			else{
				equitytype.Equity = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.EquityType equitytype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			equitytype.Equity = GetString(100 + delta);
		}
		#endregion
    }
}