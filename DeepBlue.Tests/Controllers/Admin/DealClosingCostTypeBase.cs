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
    public class DealClosingCostTypeBase : Base {

		public ResultModel Model {
			get {
				return base.ViewResult.ViewData.Model as ResultModel;
			}
		}
      
	    public AdminController DefaultController { get; set; }

		public Mock<ITransactionRepository> MockTransactionRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockTransactionRepository = new Mock<ITransactionRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

			int totalRows = 0;

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new AdminController(MockAdminRepository.Object, MockTransactionRepository.Object); 
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockAdminRepository.Setup(x => x.GetAllDealClosingCostTypes(1, 1, "DealClosingCostTypeID", "asc", ref totalRows)).Returns(new List<DeepBlue.Models.Entity.DealClosingCostType>());
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

		#region FindDealClosingCostType
		[Test]
		public void valid_find_dealclosingcosttype_sets_json_result_error() {
			Assert.IsTrue((DefaultController.DealClosingCostTypeList(1, 1, "DealClosingCostTypeID", "asc") != null));
		}
		#endregion

    }
}
