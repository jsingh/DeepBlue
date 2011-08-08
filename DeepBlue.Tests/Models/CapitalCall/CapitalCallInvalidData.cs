using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.CapitalCall {

    public class CapitalCallInvalidDataTest : CapitalCallTest  {

		#region Invalid Test Methods
		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCapitalCallReqular, false);
			this.ServiceErrors = DefaultCapitalCallReqular.Save();
		}


		[Test]
		public void create_a_new_capitalcall_without_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
		}
		[Test]
		public void create_a_new_capitalcall_without_capitalcalldate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_capitalcallduedate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallDueDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_capitalamountcalled_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalAmountCalled"));
		}
		[Test]
		public void create_a_new_capitalcall_without_investmentamount_throws_error() {
			Assert.IsFalse(IsPropertyValid("InvestmentAmount"));
		}
		[Test]
		public void create_a_new_capitalcall_without_managementfeestartdate_throws_error() {
			Assert.IsFalse(IsPropertyValid("ManagementFeeStartDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_managementfeeenddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("ManagementFeeEndDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_capitalcallnumber_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallNumber"));
		}
		[Test]
		public void create_a_new_capitalcall_without_capitalcalltypeid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CapitalCallTypeID"));
		}
		[Test]
		public void create_a_new_capitalcall_without_createddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_createdby_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}
		[Test]
		public void create_a_new_capitalcall_without_lastupdateddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}
		[Test]
		public void create_a_new_capitalcall_without_lastupdatedby_throws_error() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}
		#endregion

    }
}