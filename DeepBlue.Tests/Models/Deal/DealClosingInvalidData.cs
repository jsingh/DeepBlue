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
    public class DealClosingInvalidDataTest : DealClosingTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultDealClosing, false);
			this.ServiceErrors = DefaultDealClosing.Save();
        }

		[Test]
		public void create_a_new_dealClosing_without_dealid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_dealClosing_without_closedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CloseDate"));
		}
 
    }
}