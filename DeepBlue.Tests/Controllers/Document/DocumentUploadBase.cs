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

namespace DeepBlue.Tests.Controllers.Document {
    public class DocumentUploadBase : Base {
        public DocumentController DefaultController { get; set; }

        public Mock<IDocumentRepository > MockRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<IDocumentRepository>();
            // Spin up the controller with the mock http context, and the mock repository
            DefaultController = new DocumentController(MockRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockRepository.Setup(x => x.GetAllDocumentTypes()).Returns(new List<DocumentType>());
			   
        }

		 
        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

    }
}
