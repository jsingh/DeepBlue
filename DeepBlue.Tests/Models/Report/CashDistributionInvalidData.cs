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
	public class CashDistributionInvalidDataTest : CashDistributionTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCashDistribution, false);
			this.ServiceErrors = DefaultCashDistribution.Save();
		}

		[Test]
		public void create_a_new_cashdistributionsummary_without_valid_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundId"));
		}

		[Test]
		public void create_a_new_cashdistribuionsummary_without_valid_capitaldistributionid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionId"));
		}
    }
}