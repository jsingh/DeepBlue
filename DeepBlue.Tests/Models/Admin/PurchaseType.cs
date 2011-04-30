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
    public class PurchaseTypeTest : Base {
		public DeepBlue.Models.Entity.PurchaseType DefaultPurchaseType { get; set; }

        public Mock<IPurchaseTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IPurchaseTypeService>();

			DefaultPurchaseType = new DeepBlue.Models.Entity.PurchaseType(MockService.Object);
            MockService.Setup(x => x.SavePurchaseType(It.IsAny<DeepBlue.Models.Entity.PurchaseType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.PurchaseType purchaseType, bool ifValid) {
			RequiredFieldDataMissing(purchaseType, ifValid);
			StringLengthInvalidData(purchaseType, ifValid);			
		}

		#region PurchaseType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.PurchaseType purchaseType, bool ifValidData) {
			if (ifValidData) {
				purchaseType.Name = "PurchaseTypeName";
			}
			else{
				purchaseType.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.PurchaseType purchaseType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			purchaseType.Name = GetString(50 + delta);
		}
		#endregion
    }
}