﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Transaction;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CommunicationTypeBase : Base {
      
	    public AdminController DefaultController { get; set; }

		public Mock<ITransactionRepository> MockTransactionRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockTransactionRepository = new Mock<ITransactionRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

			int totalRows = 0;

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new AdminController(MockAdminRepository.Object, MockTransactionRepository.Object); 
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x=>x.GetAllCommunicationGroupings ()).Returns(new List<DeepBlue.Models.Entity.CommunicationGrouping>());
			MockAdminRepository.Setup(x => x.GetAllCommunicationTypes(1, 1, "CommunicationTypeName", "asc", ref totalRows)).Returns(new List<DeepBlue.Models.Entity.CommunicationType>());
						
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

		#region FindCommunicationType
		[Test]
		public void valid_Find_CommunicationType_sets_json_result_error() {
			Assert.IsTrue((DefaultController.CommunicationTypeList(1, 1, "CommunicationTypeName", "asc") != null));
		}
		#endregion

    }
}