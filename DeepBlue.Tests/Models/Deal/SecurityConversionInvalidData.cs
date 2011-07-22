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

	public class SecurityConversionInvalidDataTest : SecurityConversionTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultSecurityConversion, false);
			this.ServiceErrors = DefaultSecurityConversion.Save();
        }

		[Test]
		public void create_a_new_securityconversion_without_oldsecurityid_passes() {
			Assert.IsFalse(IsPropertyValid("OldSecurityID"));
		}

		[Test]
		public void create_a_new_securityconversion_without_oldsecuritytypeid_passes() {
			Assert.IsFalse(IsPropertyValid("OldSecurityTypeID"));
		}

		[Test]
		public void create_a_new_securityconversion_without_newsecurityid_passes() {
			Assert.IsFalse(IsPropertyValid("NewSecurityID"));
		}

		[Test]
		public void create_a_new_securityconversion_without_newsecuritytypeid_passes() {
			Assert.IsFalse(IsPropertyValid("NewSecurityTypeID"));
		}

		[Test]
		public void create_a_new_securityconversion_without_splitfactor_passes() {
			Assert.IsFalse(IsPropertyValid("SplitFactor"));
		}

		[Test]
		public void create_a_new_securityconversion_without_conversiondate_passes() {
			Assert.IsFalse(IsPropertyValid("ConversionDate"));
		}

		[Test]
		public void create_a_new_securityconversion_without_createdby_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_securityconversion_without_createddate_passes() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_securityconversion_without_lastupdatedby_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedBy"));
		}

		[Test]
		public void create_a_new_securityconversion_without_lastupdateddate_passes() {
			Assert.IsFalse(IsPropertyValid("LastUpdatedDate"));
		}
 
    }
}