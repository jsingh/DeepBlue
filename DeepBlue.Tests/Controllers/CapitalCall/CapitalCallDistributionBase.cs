using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.CapitalCall;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.Entity;
using DeepBlue.Models.CapitalCall ;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;


namespace DeepBlue.Tests.Controllers.CapitalCall {
    public class CapitalCallDistributionBase : Base {
        public CapitalCallController  DefaultController { get; set; }

        public Mock<ICapitalCallRepository> MockCapiticalCallRepository { get; set; }
		public Mock<IFundRepository> MockFundRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockCapiticalCallRepository = new Mock<ICapitalCallRepository>();
			MockFundRepository= new Mock<IFundRepository>();

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new CapitalCallController(MockFundRepository.Object, MockCapiticalCallRepository.Object);
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
