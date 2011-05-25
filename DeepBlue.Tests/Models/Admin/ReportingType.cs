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
    public class ReportingTypeTest : Base {
		public DeepBlue.Models.Entity.ReportingType DefaultReportingType { get; set; }

        public Mock<IReportingTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IReportingTypeService>();

			DefaultReportingType = new DeepBlue.Models.Entity.ReportingType(MockService.Object);
            MockService.Setup(x => x.SaveReportingType(It.IsAny<DeepBlue.Models.Entity.ReportingType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.ReportingType reportingtype, bool ifValid) {
			RequiredFieldDataMissing(reportingtype, ifValid);
			StringLengthInvalidData(reportingtype, ifValid);			
		}

		#region ReportingType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.ReportingType reportingtype, bool ifValidData) {
			if (ifValidData) {
				reportingtype.Reporting = "Reporting";
			}
			else{
				reportingtype.Reporting = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.ReportingType reportingtype, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			reportingtype.Reporting = GetString(100 + delta);
		}
		#endregion
    }
}