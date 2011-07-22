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
    public class UnderlyingFundCapitalCallInvalidDataTest : UnderlyingFundCapitalCallTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUnderlyingFundCapitalCall, false);
			this.ServiceErrors = DefaultUnderlyingFundCapitalCall.Save();
        }

		[Test]
		public void create_a_new_underlyingcapitalcall_without_fundid_passes() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_amount_passes() {
			Assert.IsFalse(IsPropertyValid("Amount"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_noticedate_passes() {
			Assert.IsFalse(IsPropertyValid("NoticeDate"));
		}

		[Test]
		public void create_a_new_underlyingcapitalcall_without_receiveddate_passes() {
			Assert.IsFalse(IsPropertyValid("ReceivedDate"));
		}
    }
}