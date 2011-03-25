using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Tests.Controllers.FundSetup {
    public class FundBase : Base {
        public FundController DefaultController { get; set; }

        public Mock<IFundRepository > MockRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<IFundRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

            // Spin up the controller with the mock http context, and the mock repository
            DefaultController = new FundController(MockRepository.Object, MockAdminRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockRepository.Setup(x=>x.GetAllMultiplierTypes()).Returns(new List<MultiplierType>()); 
			 
			MockAdminRepository.Setup(x=>x.GetAllInvestorTypes()).Returns(new List<InvestorType>());  
        }

		 
        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

    }
}
