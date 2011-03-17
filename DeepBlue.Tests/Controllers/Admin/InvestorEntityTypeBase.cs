using System;
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
    public class InvestorEntityTypeBase : Base {
        public AdminController  DefaultController { get; set; }

        public Mock<IAdminRepository> MockRepository { get; set; }

		public Mock<ITransactionRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<IAdminRepository>();

			MockAdminRepository = new Mock<ITransactionRepository>();

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new AdminController(MockRepository.Object,MockAdminRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
       
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

    }
}
