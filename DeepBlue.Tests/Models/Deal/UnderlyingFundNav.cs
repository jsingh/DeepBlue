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
    public class UnderlyingFundNAVTest : Base {
        public DeepBlue.Models.Entity.UnderlyingFundNAV DefaultUnderlyingFundNAV { get; set; }

        public Mock<IUnderlyingFundNAVService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IUnderlyingFundNAVService>();

			DefaultUnderlyingFundNAV = new DeepBlue.Models.Entity.UnderlyingFundNAV(MockService.Object);
            MockService.Setup(x => x.SaveUnderlyingFundNAV(It.IsAny<DeepBlue.Models.Entity.UnderlyingFundNAV>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.UnderlyingFundNAV underlyingFundNav, bool ifValid) {
			RequiredFieldDataMissing(underlyingFundNav, ifValid);
        }

        #region UnderlyingFundNAV
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingFundNAV underlyingFundNav, bool ifValidData) {
            if (ifValidData) {
				underlyingFundNav.FundID = 1;
				underlyingFundNav.CreatedBy = 1;
				underlyingFundNav.CreatedDate = DateTime.MaxValue;
				underlyingFundNav.LastUpdatedBy = 1;
				underlyingFundNav.LastUpdatedDate = DateTime.MaxValue;
				underlyingFundNav.UnderlyingFundID = 1;
				underlyingFundNav.FundNAV = 1;
				underlyingFundNav.FundNAVDate = DateTime.MaxValue;
            } else {
				underlyingFundNav.FundID = 0;
				underlyingFundNav.CreatedBy = 0;
				underlyingFundNav.CreatedDate = DateTime.MinValue;
				underlyingFundNav.LastUpdatedBy = 0;
				underlyingFundNav.LastUpdatedDate = DateTime.MinValue;
				underlyingFundNav.UnderlyingFundID = 0;
				underlyingFundNav.FundNAV = 0;
				underlyingFundNav.FundNAVDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}