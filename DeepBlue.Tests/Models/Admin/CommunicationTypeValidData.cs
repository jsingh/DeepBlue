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
    public class CommunicationTypeValidDataTest : CommunicationTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCommunicationType, true);
			this.ServiceErrors = DefaultCommunicationType.Save();
        }

		[Test]
		public void create_a_new_communicationtype_with_communicationtype_name_passes() {
			Assert.IsTrue(IsPropertyValid("CommunicationTypeName"));
		}

		[Test]
		public void create_a_new_communicationtype_with_communication_groupid_passes() {
			Assert.IsTrue(IsPropertyValid("CommunicationGroupId"));
		}
    }
}