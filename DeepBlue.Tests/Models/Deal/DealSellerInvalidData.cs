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
    public class DealSellerInvalidDataTest : DealSellerTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultDeal, false);
			this.ServiceErrors = DefaultDeal.Save();
        }

		[Test]
		public void create_a_new_dealseller_without_fundit_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_dealseller_without_dealnumber_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealNumber"));
		}

		[Test]
		public void create_a_new_dealseller_without_purchasetypeid_throws_error() {
			Assert.IsFalse(IsPropertyValid("PurchaseTypeID"));
		}


        [Test]
        public void create_a_new_dealseller_without_dealname_throws_error() {
            Assert.IsFalse(IsPropertyValid("DealName"));
        }

        [Test]
        public void create_a_new_dealseller_with_too_long_dealname_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealName"));
        }
    }
}