using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Document {
    public class DocumentTest : Base {
        public DeepBlue.Models.Entity.InvestorFundDocument DefaultFundDocument { get; set; }

        public Mock<IInvestorFundDocumentService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IInvestorFundDocumentService>();

			DefaultFundDocument = new DeepBlue.Models.Entity.InvestorFundDocument(MockService.Object);
			DefaultFundDocument.File = new File();
            MockService.Setup(x => x.SaveInvestorFundDocument(It.IsAny<DeepBlue.Models.Entity.InvestorFundDocument>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.InvestorFundDocument funddocument, bool ifValid) {
			RequiredFieldDataMissing(funddocument, ifValid);
        }

        #region Document
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.InvestorFundDocument funddocument, bool ifValidData) {
            if (ifValidData) {
				funddocument.DocumentTypeID  = 1;
				funddocument.DocumentDate = DateTime.Now;
				funddocument.InvestorID = 1;
            } else {
				funddocument.DocumentTypeID  = 0;
				funddocument.DocumentDate = DateTime.MinValue;
				funddocument.InvestorID = 0;
            }
        }
        #endregion
    }
}