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
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Controllers.Fund {
	public class FundBase : Base {
		public FundController DefaultController { get; set; }

		public Mock<IFundRepository> MockFundRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockFundRepository = new Mock<IFundRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

			// Spin up the controller with the mock http context, and the mock repository
			DefaultController = new FundController(MockFundRepository.Object, MockAdminRepository.Object);
			DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockFundRepository.Setup(x => x.GetAllMultiplierTypes()).Returns(new List<MultiplierType>());
			MockAdminRepository.Setup(x => x.GetAllInvestorTypes()).Returns(new List<InvestorType>());
			MockAdminRepository.Setup(x => x.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund)).Returns(new List<CustomFieldDetail>());
		}


		[TearDown]
		public override void TearDown() {
			base.TearDown();
			DefaultController.Dispose();
			DefaultController = null;
		}

	}
}
