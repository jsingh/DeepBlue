using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Deal {
    public class DealClosingCostInvalidDataTest : DealClosingCostTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultDealClosingCost, false);
			this.ServiceErrors = DefaultDealClosingCost.Save();
        }

		[Test]
		public void create_a_new_dealclosingcost_without_dealid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_dealclosingcost_without_amount_throws_error() {
			Assert.IsFalse(IsPropertyValid("Amount"));
		}

		[Test]
		public void create_a_new_dealclosingcost_without_date_throws_error() {
			Assert.IsFalse(IsPropertyValid("Date"));
		}

        [Test]
        public void create_a_new_dealclosingcost_without_typeid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealClosingCostTypeID"));
        }

    }
}