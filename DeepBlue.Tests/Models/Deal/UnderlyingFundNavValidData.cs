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
    public class UnderlyingFundNAVValidDataTest : UnderlyingFundNAVTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundNAV, true);
			this.ServiceErrors = DefaultUnderlyingFundNAV.Save();
        }

		[Test]
		public void create_a_new_underlyingfundnav_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_underlyingfundid_passes() {
			Assert.IsTrue(IsPropertyValid("UnderlyingFundID"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_fundnav_passes() {
			Assert.IsTrue(IsPropertyValid("FundNAV"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_fundnavdate_passes() {
			Assert.IsTrue(IsPropertyValid("FundNAVDate"));
		}
    }
}