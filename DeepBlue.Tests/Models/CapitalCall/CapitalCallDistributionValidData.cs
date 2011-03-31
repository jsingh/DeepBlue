﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.CapitalCall {
    public class CapitalCallDistributionValidData : CapitalCallDistributionTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCapitalCallDistribution, true);
			this.ServiceErrors = DefaultCapitalCallDistribution.Save();
        }

		[Test]
		public void create_a_new_capitalcalldistribution_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

        [Test]
        public void create_a_new_capitalcalldistribution_with_valid_capitalamount_passes() {
			Assert.IsTrue(IsPropertyValid("DistributionAmount"));
        }

        [Test]
        public void create_a_new_capitalcalldistribution_with_valid_capitalcalldate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalDistributionDate"));
        }

		[Test]
		public void create_a_new_capitalcalldistribution_with_valid_capitalcallduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalDistributionDueDate"));
		}

		[Test]
		public void create_a_new_capitalcalldistribution_with_valid_capitalcallnumber_passes() {
			Assert.IsTrue(IsPropertyValid("DistributionNumber"));
		}

    }
}