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
    public class UnderlyingDirectLastPriceValidDataTest : UnderlyingDirectLastPriceTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingDirectLastPrice, true);
			this.ServiceErrors = DefaultUnderlyingDirectLastPrice.Save();
        }

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_securitytypeid_passes() {
			Assert.IsTrue(IsPropertyValid("SecurityTypeID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_securityid_passes() {
			Assert.IsTrue(IsPropertyValid("SecurityID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_lastprice_passes() {
			Assert.IsTrue(IsPropertyValid("LastPrice"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_lastpricedate_passes() {
			Assert.IsTrue(IsPropertyValid("LastPriceDate"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}
    }
}