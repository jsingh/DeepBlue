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
    public class CommunicationGroupingTest : Base {
		public DeepBlue.Models.Entity.CommunicationGrouping DefaultCommunicationGrouping { get; set; }

        public Mock<ICommunicationGroupingService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICommunicationGroupingService>();

			DefaultCommunicationGrouping = new DeepBlue.Models.Entity.CommunicationGrouping (MockService.Object);
            MockService.Setup(x => x.SaveCommunicationGrouping(It.IsAny<DeepBlue.Models.Entity.CommunicationGrouping>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.CommunicationGrouping communicationGroup, bool ifValid) {
			RequiredFieldDataMissing(communicationGroup, ifValid);
			StringLengthInvalidData(communicationGroup, ifValid);			
		}

		#region CommunicationType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CommunicationGrouping communicationGroup, bool ifValidData) {
			if (ifValidData) {
				communicationGroup.CommunicationGroupingName  = "CommunicationGroupingName";
			}
			else{
				communicationGroup.CommunicationGroupingName = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CommunicationGrouping communicationGroup, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			communicationGroup.CommunicationGroupingName = GetString(20 + delta);
		}
		#endregion
    }
}