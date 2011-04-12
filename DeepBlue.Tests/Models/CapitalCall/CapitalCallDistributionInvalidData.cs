using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.CapitalCall {
    public class CapitalCallDistributionInvalidDataTest : CapitalCallDistributionTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallDistribution, false);
			this.ServiceErrors = DefaultCapitalCallDistribution.Save();
        }

		[Test]
		public void create_a_new_capitalcalldistribution_without_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}


        [Test]
        public void create_a_new_capitalcalldistribution_without_distributionamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("DistributionAmount"));
        }

        [Test]
        public void create_a_new_capitalcalldistribution_without_distributiondate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionDate"));
        }


		[Test]
		public void create_a_new_capitalcalldistribution_without_distributionduedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalDistributionDueDate"));
		}

		[Test]
		public void create_a_new_capitalcalldistribution_without_distributionnumber_throws_error() {
			Assert.IsFalse(IsPropertyValid("DistributionNumber"));
		}
    }
}