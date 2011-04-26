using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Document;
using DeepBlue.Models.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Tests.Controllers.Document {
	public class DocumentUploadBase : Base {
		public DocumentController DefaultController { get; set; }

		public Mock<IDocumentRepository> MockRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockRepository = new Mock<IDocumentRepository>();
			MockAdminRepository = new Mock<IAdminRepository>();
			// Spin up the controller with the mock http context, and the mock repository
			DefaultController = new DocumentController(MockRepository.Object, MockAdminRepository.Object);
			DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x => x.GetAllDocumentTypes()).Returns(new List<DocumentType>());
		}


		[TearDown]
		public override void TearDown() {
			base.TearDown();
			DefaultController.Dispose();
			DefaultController = null;
		}

	}
}
