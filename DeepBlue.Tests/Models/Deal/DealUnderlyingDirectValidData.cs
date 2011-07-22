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
    public class DealUnderlyingDirectValidDataTest : DealUnderlyingDirectTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDealUnderlyingDirect, true);
			this.ServiceErrors = DefaultDealUnderlyingDirect.Save();
        }

		[Test]
		public void create_a_new_dealunderlyingdirect_with_dealid_passes() {
			Assert.IsTrue(IsPropertyValid("DealID"));
		}

        [Test]
		public void create_a_new_dealunderlyingdirect_with_securitytypeid_passes() {
			Assert.IsTrue(IsPropertyValid("SecurityTypeID"));
        }

		[Test]
		public void create_a_new_dealunderlyingdirect_with_securityid_passes() {
			Assert.IsTrue(IsPropertyValid("SecurityID"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_with_purchaseprice_passes() {
			Assert.IsTrue(IsPropertyValid("PurchasePrice"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_with_numberofshares_passes() {
			Assert.IsTrue(IsPropertyValid("NumberOfShares"));
		}

		[Test]
		public void create_a_new_dealunderlyingdirect_with_recorddate_passes() {
			Assert.IsTrue(IsPropertyValid("RecordDate"));
		}
    }
}