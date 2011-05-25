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
    public class ReportingFrequencyInvalidDataTest :ReportingFrequencyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultReportingFrequency, false);
			this.ServiceErrors = DefaultReportingFrequency.Save();
        }

		[Test]
		public void create_a_new_reportingfrequency_without_reportingfrequency_throws_error() {
			Assert.IsFalse(IsPropertyValid("ReportingFrequency1"));
		}

		[Test]
		public void create_a_new_reportingfrequency_without_too_long_reportingfrequency_throws_error() {
			Assert.IsFalse(IsPropertyValid("ReportingFrequency1"));
		}

    }
}