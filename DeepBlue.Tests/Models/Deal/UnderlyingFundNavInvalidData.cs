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
    public class UnderlyingFundNAVInvalidDataTest : UnderlyingFundNAVTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundNAV, false);
			this.ServiceErrors = DefaultUnderlyingFundNAV.Save();
        }

		[Test]
		public void create_a_new_underlyingfundnav_without_fundid_passes() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_underlyingfundid_passes() {
			Assert.IsFalse(IsPropertyValid("UnderlyingFundID"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_fundnav_passes() {
			Assert.IsFalse(IsPropertyValid("FundNAV"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_fundnavdate_passes() {
			Assert.IsFalse(IsPropertyValid("FundNAVDate"));
		}
    }
}