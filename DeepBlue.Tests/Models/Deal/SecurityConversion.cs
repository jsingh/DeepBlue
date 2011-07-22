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
    public class SecurityConversionTest : Base {
        public DeepBlue.Models.Entity.SecurityConversion DefaultSecurityConversion { get; set; }

        public Mock<ISecurityConversionService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<ISecurityConversionService>();

			DefaultSecurityConversion = new DeepBlue.Models.Entity.SecurityConversion(MockService.Object);
            MockService.Setup(x => x.SaveSecurityConversion(It.IsAny<DeepBlue.Models.Entity.SecurityConversion>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.SecurityConversion securityConversion, bool ifValid) {
			RequiredFieldDataMissing(securityConversion, ifValid);
        }

        #region SecurityConversion
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.SecurityConversion securityConversion, bool ifValidData) {
            if (ifValidData) {
				securityConversion.CreatedBy = 1;
				securityConversion.CreatedDate = DateTime.MaxValue;
				securityConversion.LastUpdatedBy = 1;
				securityConversion.LastUpdatedDate = DateTime.MaxValue;
				securityConversion.OldSecurityID = 1;
				securityConversion.OldSecurityTypeID = 1;
				securityConversion.NewSecurityID = 1;
				securityConversion.NewSecurityTypeID = 1;
				securityConversion.SplitFactor = 1;
				securityConversion.ConversionDate = DateTime.MaxValue;
            } else {
				securityConversion.CreatedBy = 0;
				securityConversion.CreatedDate = DateTime.MinValue;
				securityConversion.LastUpdatedBy = 0;
				securityConversion.LastUpdatedDate = DateTime.MinValue;
				securityConversion.OldSecurityID = 0;
				securityConversion.OldSecurityTypeID = 0;
				securityConversion.NewSecurityID = 0;
				securityConversion.NewSecurityTypeID = 0;
				securityConversion.SplitFactor = 0;
				securityConversion.ConversionDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}