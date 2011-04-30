using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Report {
	public class CapitalCallSummaryInvalidDataTest : CapitalCallSummaryTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCapitalCallSummary, false);
			this.ServiceErrors = DefaultCapitalCallSummary.Save();
		}

		[Test]
		public void create_a_new_capitalcallsummary_without_valid_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundId"));
		}

		[Test]
		public void create_a_new_capitalcallsummary_without_valid_capitalcallid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallId"));
		}

    }
}