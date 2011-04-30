using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Deal;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Tests.Controllers.Deal {
	public class DealClosingCostBase : Base {
		public DealController DefaultController { get; set; }

		public Mock<IDealRepository > MockDealRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockDealRepository = new Mock<IDealRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

			// Spin up the controller with the mock http context, and the mock repository
			DefaultController = new DealController(MockDealRepository.Object, MockAdminRepository.Object);
			DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x => x.GetAllDocumentTypes()).Returns(new List<DocumentType>());
			MockAdminRepository.Setup(x => x.GetAllPurchaseTypes()).Returns(new List<PurchaseType>());
			MockAdminRepository.Setup(x => x.GetAllDealClosingCostTypes()).Returns(new List<DealClosingCostType>());
			MockDealRepository.Setup(x => x.GetAllUnderlyingFunds()).Returns(new List<UnderlyingFund>());

		}


		[TearDown]
		public override void TearDown() {
			base.TearDown();
			DefaultController.Dispose();
			DefaultController = null;
		}

	}
}
