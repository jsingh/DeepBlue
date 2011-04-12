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
    public class CommunicationTypeInvalidDataTest : CommunicationTypeTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCommunicationType,false);
			this.ServiceErrors = DefaultCommunicationType.Save();
        }

		[Test]
		public void create_a_new_communicationtype_without_communicationtype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationTypeName"));
		}

		[Test]
		public void create_a_new_communicationtype_without_too_long_communicationtype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationTypeName"));
		}

		[Test]
		public void create_a_new_communicationtype_without_communication_groupid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationGroupingID"));
		}

		[Test]
		public void create_a_new_communicationtype_without_too_long_communication_groupid_throws_error() {
			Assert.IsFalse(IsPropertyValid("CommunicationGroupingID"));
		}
    }
}