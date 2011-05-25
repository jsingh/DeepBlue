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
    public class ReportingFrequencyValidDataTest : ReportingFrequencyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultReportingFrequency, true);
			this.ServiceErrors = DefaultReportingFrequency.Save();
        }

		[Test]
		public void create_a_new_reportingfrequency_with_reportingfrequency_passes() {
			Assert.IsTrue(IsPropertyValid("ReportingFrequency1"));
		}

    }
}