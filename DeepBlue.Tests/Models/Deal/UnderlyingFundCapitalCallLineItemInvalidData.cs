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

	public class UnderlyingFundCapitalCallLineItemInvalidDataTest : UnderlyingFundCapitalCallLineItemTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundCapitalCallLineItem, false);
			this.ServiceErrors = DefaultUnderlyingFundCapitalCallLineItem.Save();
        }

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_underlyingfundid_passes() {
			Assert.IsFalse(IsPropertyValid("UnderlyingFundID"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_dealid_passes() {
			Assert.IsFalse(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcalllineitem_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}
 
    }
}