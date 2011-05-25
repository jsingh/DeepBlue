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
    public class ReportingTypeInvalidDataTest :ReportingTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultReportingType, false);
			this.ServiceErrors = DefaultReportingType.Save();
        }

		[Test]
		public void create_a_new_reportingtype_without_reporting_throws_error() {
			Assert.IsFalse(IsPropertyValid("Reporting"));
		}

		[Test]
		public void create_a_new_reportingtype_without_too_long_reporting_throws_error() {
			Assert.IsFalse(IsPropertyValid("Reporting"));
		}

    }
}