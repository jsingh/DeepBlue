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
    public class UnderlyingDirectLastPriceInvalidDataTest : UnderlyingDirectLastPriceTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingDirectLastPrice, false);
			this.ServiceErrors = DefaultUnderlyingDirectLastPrice.Save();
        }

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_fundid_passes() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_securitytypeid_passes() {
			Assert.IsFalse(IsPropertyValid("SecurityTypeID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_securityid_passes() {
			Assert.IsFalse(IsPropertyValid("SecurityID"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_lastprice_passes() {
			Assert.IsFalse(IsPropertyValid("LastPrice"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_lastpricedate_passes() {
			Assert.IsFalse(IsPropertyValid("LastPriceDate"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_dealunderlyinglastprice_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}
    }
}