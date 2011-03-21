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
    public class CustomFieldTest : Base {
		public DeepBlue.Models.Entity.CustomField  DefaultCustomField { get; set; }

        public Mock<ICustomFieldService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICustomFieldService>();

			DefaultCustomField = new DeepBlue.Models.Entity.CustomField(MockService.Object);
            MockService.Setup(x => x.SaveCustomField(It.IsAny<DeepBlue.Models.Entity.CustomField>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.CustomField customfield, bool ifValid) {
			RequiredFieldDataMissing(customfield, ifValid);
			StringLengthInvalidData(customfield, ifValid);			
		}

		#region CustomField
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CustomField customfield, bool ifValidData) {
			if (ifValidData) {
			    customfield.ModuleID = 0;
				customfield.DataTypeID = 0;
				customfield.CustomFieldText  = "";
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CustomField customfield, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			customfield.ModuleID = 0 + delta;
			customfield.DataTypeID = 0 + delta;
			customfield.CustomFieldText  = GetString(50 + delta);
		}
		#endregion
    }
}