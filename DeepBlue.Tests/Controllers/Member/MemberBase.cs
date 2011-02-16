using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Controllers.Member;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MbUnit.Framework;

namespace DeepBlue.Tests.Controllers.Member {
    public class MemberBase : Base {
        public MemberController DefaultController { get; set; }

        public Mock<IMemberControllerRepository> MockRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up the controller with the mock http context
            DefaultController = new MemberController();
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<IMemberControllerRepository>();
            DefaultController.MemberControllerRepository = MockRepository.Object;
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }
    }
}
