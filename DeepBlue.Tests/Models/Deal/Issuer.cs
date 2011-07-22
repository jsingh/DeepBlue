using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Deal {
    public class IssuerTest : Base {
        public DeepBlue.Models.Entity.Issuer DefaultIssuer { get; set; }

        public Mock<IIssuerService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IIssuerService>();

			DefaultIssuer = new DeepBlue.Models.Entity.Issuer(MockService.Object);
            MockService.Setup(x => x.SaveIssuer(It.IsAny<DeepBlue.Models.Entity.Issuer>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.Issuer issuer, bool ifValid) {
			RequiredFieldDataMissing(issuer, ifValid);
        }

        #region Issuer
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Issuer issuer, bool ifValidData) {
            if (ifValidData) {
				issuer.Name = "test";
				issuer.CountryID = 1;
            } else {
				issuer.Name = string.Empty;
				issuer.CountryID = 0;
            }
        }
        #endregion

    }
}