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
	public class CashDistributionValidDataTest : CashDistributionTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCashDistribution, true);
			this.ServiceErrors = DefaultCashDistribution.Save();
		}

		[Test]
		public void create_a_new_cashdistributionsummary_with_fundid_passes() {
			Assert.IsFalse(IsPropertyValid("FundId"));
		}

		[Test]
		public void create_a_new_cashdistributionsummary_with_capitaldistributionid_passes() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionId"));
		}
		
    }
}