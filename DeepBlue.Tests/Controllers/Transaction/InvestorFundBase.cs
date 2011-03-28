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
using DeepBlue.Controllers.Investor;
using DeepBlue.Controllers.Admin;
using DeepBlue.Controllers.Fund; 
using DeepBlue.Models.Fund; 

namespace DeepBlue.Tests.Controllers.Transaction {
    public class InvestorFundBase : Base {
        public TransactionController DefaultController { get; set; }

        public Mock<ITransactionRepository> MockRepository { get; set; }
		public Mock<IInvestorRepository> MockInvestorRepository { get; set; }
		public Mock<IAdminRepository> MockAdminRepository { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockRepository = new Mock<ITransactionRepository>();

			// Spin up mock repository and attach to controller
			MockInvestorRepository = new Mock<IInvestorRepository>();

			MockAdminRepository = new Mock<IAdminRepository>();

            // Spin up the controller with the mock http context, and the mock repository
			DefaultController = new TransactionController(MockRepository.Object, MockInvestorRepository.Object, MockAdminRepository.Object);
            DefaultController.ControllerContext = new ControllerContext(DeepBlue.Helpers.HttpContextFactory.GetHttpContext(), new RouteData(), new Mock<ControllerBase>().Object);
			MockRepository.Setup(x => x.GetAllFundNames()).Returns(GetMockAllFundNames());
			MockAdminRepository.Setup(x => x.GetAllInvestorTypes()).Returns(GetMockAllInvestorTypes());
        }
		
		private List<DeepBlue.Models.Entity.Fund> GetMockAllFundNames() {
			List<DeepBlue.Models.Entity.Fund> funds = new List<DeepBlue.Models.Entity.Fund>();
			return funds;
        }


		private List<FundClosing> GetMockAllFundClosings() {
			List<FundClosing> fundClosings = new List<FundClosing>();
			return fundClosings;
		}

		private List<InvestorType> GetMockAllInvestorTypes() {
			List<InvestorType> investorTypes = new List<InvestorType>();
			return investorTypes;
		}


        [TearDown]
        public override void TearDown() {
            base.TearDown();
            DefaultController.Dispose();
            DefaultController = null;
        }

    }
}
