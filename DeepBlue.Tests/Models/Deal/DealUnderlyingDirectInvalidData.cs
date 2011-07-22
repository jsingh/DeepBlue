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
    public class DealUnderlyingDirectInvalidDataTest : DealUnderlyingDirectTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDealUnderlyingDirect, false);
			this.ServiceErrors = DefaultDealUnderlyingDirect.Save();
        }

		[Test]
		public void create_a_new_dealunderlyingdirect_without_dealid_passes() {
			Assert.IsFalse(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_without_securitytypeid_passes() {
			Assert.IsFalse(IsPropertyValid("SecurityTypeID"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_without_securityid_passes() {
			Assert.IsFalse(IsPropertyValid("SecurityID"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_without_purchaseprice_passes() {
			Assert.IsFalse(IsPropertyValid("PurchasePrice"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_without_numberofshares_passes() {
			Assert.IsFalse(IsPropertyValid("NumberOfShares"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_without_recorddate_passes() {
			Assert.IsFalse(IsPropertyValid("RecordDate"));
		}
    }
}