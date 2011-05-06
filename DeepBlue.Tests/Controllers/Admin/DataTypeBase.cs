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

namespace DeepBlue.Tests.Controllers.Admin {
    public class DataTypeBase : Base {
        public AdminController  DefaultController { get; set; }

        public Mock<ITransactionRepository> MockRepository { get; set; }

		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<ITransactionRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new AdminController(MockAdminRepository.Object,MockRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			//MockAdminRepository.Setup(x => x.GetAllDataTypes()).Returns(new List<DataType>()); 
       
        }

        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

		#region FindDatatype
		[Test]
		public void valid_Find_DataType_sets_json_result_error() {
			Assert.IsTrue((DefaultController.DataTypeList(1, 1, "DataTypeID", "asc") != null));
		}
		#endregion

    }
}
