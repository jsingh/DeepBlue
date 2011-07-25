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
    public class ShareClassTypeTest : Base {
		public DeepBlue.Models.Entity.ShareClassType DefaultShareClassType { get; set; }

        public Mock<IShareClassTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IShareClassTypeService>();

			DefaultShareClassType = new DeepBlue.Models.Entity.ShareClassType(MockService.Object);
            MockService.Setup(x => x.SaveShareClassType(It.IsAny<DeepBlue.Models.Entity.ShareClassType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.ShareClassType shareclasstype, bool ifValid) {
			RequiredFieldDataMissing(shareclasstype, ifValid);
			StringLengthInvalidData(shareclasstype, ifValid);			
		}

		#region ShareClassType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.ShareClassType shareclasstype, bool ifValidData) {
			if (ifValidData) {
				shareclasstype.ShareClass = "ShareClassType";
				shareclasstype.CreatedBy = 1;
				shareclasstype.CreatedDate = DateTime.MaxValue;
				shareclasstype.LastUpdatedBy = 1;
				shareclasstype.LastUpdatedDate = DateTime.MaxValue;
				shareclasstype.EntityID = 2;
			}
			else{
				shareclasstype.ShareClass = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.ShareClassType shareclasstype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			shareclasstype.ShareClass = GetString(100 + delta);
		}
		#endregion
    }
}