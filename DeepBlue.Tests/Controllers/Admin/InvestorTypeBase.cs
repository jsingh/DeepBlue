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
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class InvestorTypeBase : Base {

		public ResultModel Model {
			get {
				return base.ViewResult.ViewData.Model as ResultModel;
			}
		}

        public AdminController  DefaultController { get; set; }

        public Mock<ITransactionRepository> MockRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<ITransactionRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

			int totalRows = 0;

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new AdminController(MockAdminRepository.Object,MockRepository.Object);
			MockAdminRepository.Setup(x=>x.GetAllInvestorTypes()).Returns(new List<InvestorType>()); 
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x => x.GetAllInvestorTypes(1, 1, "InvestorTypeName", "asc", ref totalRows)).Returns(new List<DeepBlue.Models.Entity.InvestorType>());
       
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

		#region FindInvestorType
		[Test]
		public void valid_find_investortype_sets_json_result_error() {
			Assert.IsTrue((DefaultController.InvestorTypeList(1, 1, "InvestorTypeName", "asc") != null));
		}
		#endregion

    }
}
