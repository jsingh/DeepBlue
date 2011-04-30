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
    public class SecurityTypeTest : Base {
		public DeepBlue.Models.Entity.SecurityType DefaultSecurityType { get; set; }

        public Mock<ISecurityTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ISecurityTypeService>();

			DefaultSecurityType = new DeepBlue.Models.Entity.SecurityType(MockService.Object);
            MockService.Setup(x => x.SaveSecurityType(It.IsAny<DeepBlue.Models.Entity.SecurityType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.SecurityType securityType, bool ifValid) {
			RequiredFieldDataMissing(securityType, ifValid);
			StringLengthInvalidData(securityType, ifValid);			
		}

		#region SecurityType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.SecurityType securityType, bool ifValidData) {
			if (ifValidData) {
				securityType.Name = "SecurityTypeName";
			}
			else{
				securityType.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.SecurityType securityType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			securityType.Name = GetString(100 + delta);
		}
		#endregion
    }
}