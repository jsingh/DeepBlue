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
    public class ReportingFrequencyTest : Base {
		public DeepBlue.Models.Entity.ReportingFrequency DefaultReportingFrequency { get; set; }

        public Mock<IReportingFrequencyService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IReportingFrequencyService>();

			DefaultReportingFrequency = new DeepBlue.Models.Entity.ReportingFrequency(MockService.Object);
            MockService.Setup(x => x.SaveReportingFrequency(It.IsAny<DeepBlue.Models.Entity.ReportingFrequency>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.ReportingFrequency reportingfrequency, bool ifValid) {
			RequiredFieldDataMissing(reportingfrequency, ifValid);
			StringLengthInvalidData(reportingfrequency, ifValid);			
		}

		#region ReportingFrequency
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.ReportingFrequency reportingfrequency, bool ifValidData) {
			if (ifValidData) {
				reportingfrequency.ReportingFrequency1  = "ReportingFrequency";
			}
			else{
				reportingfrequency.ReportingFrequency1 = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.ReportingFrequency reportingfrequency, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			reportingfrequency.ReportingFrequency1 = GetString(100 + delta);
		}
		#endregion
    }
}