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
    public class CommunicationTypeTest : Base {
		public DeepBlue.Models.Entity.CommunicationType DefaultCommunicationType { get; set; }

        public Mock<ICommunicationTypeService > MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICommunicationTypeService>();

			DefaultCommunicationType = new DeepBlue.Models.Entity.CommunicationType(MockService.Object);
            MockService.Setup(x => x.SaveCommunicationType(It.IsAny<DeepBlue.Models.Entity.CommunicationType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.CommunicationType communicationType, bool ifValid) {
			RequiredFieldDataMissing(communicationType, ifValid);
			StringLengthInvalidData(communicationType, ifValid);			
		}

		#region CommunicationType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.CommunicationType communicationType, bool ifValidData) {
			if (ifValidData) {
				communicationType.CommunicationTypeName = "CommunicationTypeName";
				communicationType.CommunicationGroupingID = 1;
			}
			else{
				communicationType.CommunicationTypeName = string.Empty;
				communicationType.CommunicationGroupingID =0;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.CommunicationType communicationType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			communicationType.CommunicationTypeName = GetString(20 + delta);
		}
		#endregion
    }
}