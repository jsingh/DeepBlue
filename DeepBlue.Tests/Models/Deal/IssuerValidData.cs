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
    public class IssuerValidDataTest : IssuerTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultIssuer, true);
			this.ServiceErrors = DefaultIssuer.Save();
        }

		[Test]
		public void create_a_new_underlyingfundnav_with_name_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_underlyingfundnav_with_countryid_passes() {
			Assert.IsTrue(IsPropertyValid("CountryID"));
		}
		 
    }
}