using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Tests.Models.Deal;

namespace DeepBlue.Tests.Models.Deal {

	public class UnderlyingFundCapitalCallLineItemValidDataTest : UnderlyingFundCapitalCallLineItemTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundCapitalCallLineItem, true);
			this.ServiceErrors = DefaultUnderlyingFundCapitalCallLineItem.Save();
        }

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_underlyingfundid_passes() {
			Assert.IsTrue(IsPropertyValid("UnderlyingFundID"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_dealid_passes() {
			Assert.IsTrue(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}
 
    }
}