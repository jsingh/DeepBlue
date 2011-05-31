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
    public class ActivityTypeTest : Base {
		public DeepBlue.Models.Entity.ActivityType DefaultActivityType { get; set; }

        public Mock<IActivityTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IActivityTypeService>();

			DefaultActivityType = new DeepBlue.Models.Entity.ActivityType(MockService.Object);
            MockService.Setup(x => x.SaveActivityType(It.IsAny<DeepBlue.Models.Entity.ActivityType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.ActivityType activitytype, bool ifValid) {
			RequiredFieldDataMissing(activitytype, ifValid);
			StringLengthInvalidData(activitytype, ifValid);			
		}

		#region ActivityType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.ActivityType activitytype, bool ifValidData) {
			if (ifValidData) {
				activitytype.Name = "ActivityTypeName";
			}
			else{
				activitytype.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.ActivityType activitytype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			activitytype.Name = GetString(100 + delta);
		}
		#endregion
    }
}