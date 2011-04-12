using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class CommunicationGroupingInvalidDataTest : CommunicationGroupingTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCommunicationGrouping, false);
			this.ServiceErrors = DefaultCommunicationGrouping.Save();
        }

		[Test]
		public void create_a_new_communicationgrouping_without_communicationgrouping_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationGroupingName"));
		}

		[Test]
		public void create_a_new_communicationgrouping_without_too_long_communicationgrouping_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationGroupingName"));
		}

    }
}