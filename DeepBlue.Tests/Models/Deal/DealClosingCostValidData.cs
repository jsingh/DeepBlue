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
    public class DealClosingCostValidDataTest : DealClosingCostTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultDealClosingCost, true);
			this.ServiceErrors = DefaultDealClosingCost.Save();
        }

		[Test]
		public void create_a_new_dealclosingcost_with_dealclosingcosttypeid_throws_error() {
			Assert.IsTrue(IsPropertyValid("DealClosingCostTypeID"));
		}

		[Test]
		public void create_a_new_dealclosingcost_with_amount_throws_error() {
			Assert.IsTrue(IsPropertyValid("Amount"));
		}

		[Test]
		public void create_a_new_dealclosingcost_with_dealid_throws_error() {
			Assert.IsTrue(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_dealclosingcost_with_date_throws_error() {
			Assert.IsTrue(IsPropertyValid("Date"));
		}
 
    }
}