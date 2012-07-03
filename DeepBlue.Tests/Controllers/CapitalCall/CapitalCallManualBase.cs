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
using DeepBlue.Controllers.Investor;
using DeepBlue.Controllers.Admin;


namespace DeepBlue.Tests.Controllers.CapitalCall {
    public class CapitalCallManualBase : Base {
        public CapitalCallController  DefaultController { get; set; }

		public Mock<IInvestorRepository> MockInvestorRepository { get; set; }
        public Mock<ICapitalCallRepository> MockCapiticalCallRepository { get; set; }
		public Mock<IFundRepository> MockFundRepository { get; set; }
		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockInvestorRepository = new Mock<IInvestorRepository>();
			MockCapiticalCallRepository = new Mock<ICapitalCallRepository>();
			MockFundRepository= new Mock<IFundRepository>();
			MockAdminRepository = new Mock<IAdminRepository>();

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new CapitalCallController(MockFundRepository.Object, MockCapiticalCallRepository.Object, MockInvestorRepository.Object, MockAdminRepository.Object);
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
