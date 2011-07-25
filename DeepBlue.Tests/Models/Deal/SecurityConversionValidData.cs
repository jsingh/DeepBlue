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

	public class SecurityConversionValidDataTest : SecurityConversionTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultSecurityConversion, true);
			this.ServiceErrors = DefaultSecurityConversion.Save();
		}

		[Test]
		public void create_a_new_securityconversion_with_oldsecurityid_passes() {
			Assert.IsTrue(IsPropertyValid("OldSecurityID"));
		}

		[Test]
		public void create_a_new_securityconversion_with_oldsecuritytypeid_passes() {
			Assert.IsTrue(IsPropertyValid("OldSecurityTypeID"));
		}

		[Test]
		public void create_a_new_securityconversion_with_newsecurityid_passes() {
			Assert.IsTrue(IsPropertyValid("NewSecurityID"));
		}

		[Test]
		public void create_a_new_securityconversion_with_newsecuritytypeid_passes() {
			Assert.IsTrue(IsPropertyValid("NewSecurityTypeID"));
		}

		[Test]
		public void create_a_new_securityconversion_with_splitfactor_passes() {
			Assert.IsTrue(IsPropertyValid("SplitFactor"));
		}

		[Test]
		public void create_a_new_securityconversion_with_conversiondate_passes() {
			Assert.IsTrue(IsPropertyValid("ConversionDate"));
		}

		[Test]
		public void create_a_new_securityconversion_with_createdby_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_securityconversion_with_createddate_passes() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_securityconversion_with_lastupdatedby_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_securityconversion_with_lastupdateddate_passes() {
			Assert.IsTrue(IsPropertyValid("LastUpdatedDate"));
		}

	}
}