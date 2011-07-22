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
    public class IssuerInvalidDataTest : IssuerTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultIssuer, false);
			this.ServiceErrors = DefaultIssuer.Save();
        }

		[Test]
		public void create_a_new_underlyingfundnav_without_name_passes() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_without_countryid_passes() {
			Assert.IsFalse(IsPropertyValid("CountryID"));
		}
		 
    }
}