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
    public class CashDistributionTypeTest : Base {
		public DeepBlue.Models.Entity.CashDistributionType DefaultCashDistributionType { get; set; }

        public Mock<ICashDistributionTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICashDistributionTypeService>();

			DefaultCashDistributionType = new DeepBlue.Models.Entity.CashDistributionType(MockService.Object);
            MockService.Setup(x => x.SaveCashDistributionType(It.IsAny<DeepBlue.Models.Entity.CashDistributionType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.CashDistributionType cashdistributiontype, bool ifValid) {
			RequiredFieldDataMissing(cashdistributiontype, ifValid);
			StringLengthInvalidData(cashdistributiontype, ifValid);			
		}

		#region CashDistributionType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CashDistributionType cashdistributiontype, bool ifValidData) {
			if (ifValidData) {
				cashdistributiontype.Name = "CashDistributionTypeName";
			}
			else{
				cashdistributiontype.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CashDistributionType cashdistributiontype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			cashdistributiontype.Name = GetString(100 + delta);
		}
		#endregion
    }
}