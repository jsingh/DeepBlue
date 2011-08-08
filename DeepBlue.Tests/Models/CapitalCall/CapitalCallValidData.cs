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
	public class CapitalCallValidDataTest : CapitalCallTest {

		#region Valid Test Methods
		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultCapitalCallReqular, true);
			this.ServiceErrors = DefaultCapitalCallReqular.Save();
		}


		[Test]
		public void create_a_new_capitalcall_with_valid_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_capitalcalldate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_capitalcallduedate_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallDueDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_capitalamountcalled_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalAmountCalled"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_investmentamount_passes() {
			Assert.IsTrue(IsPropertyValid("InvestmentAmount"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_managementfeestartdate_passes() {
			Assert.IsTrue(IsPropertyValid("ManagementFeeStartDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_managementfeeenddate_passes() {
			Assert.IsTrue(IsPropertyValid("ManagementFeeEndDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_capitalcallnumber_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallNumber"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_capitalcalltypeid_passes() {
			Assert.IsTrue(IsPropertyValid("CapitalCallTypeID"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}
		[Test]
		public void create_a_new_capitalcall_with_valid_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}
		#endregion
	}
}