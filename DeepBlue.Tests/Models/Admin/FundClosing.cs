using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class FundClosingTest : Base {
		public DeepBlue.Models.Entity.FundClosing  DefaultFundClosing { get; set; }

        public Mock<IFundClosingService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IFundClosingService>();

			DefaultFundClosing = new DeepBlue.Models.Entity.FundClosing(MockService.Object);
            MockService.Setup(x => x.SaveFundClose(It.IsAny<DeepBlue.Models.Entity.FundClosing>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.FundClosing fundClosing, bool ifValid) {
			RequiredFieldDataMissing(fundClosing, ifValid);
			StringLengthInvalidData(fundClosing, ifValid);			
		}

		#region FundClosing
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.FundClosing fundClosing, bool ifValidData) {
			if (ifValidData) {
				fundClosing.Name = "FundClosing";
				fundClosing.FundID = 1;
				fundClosing.FundClosingDate = DateTime.Now ; 
			}
			else{
				fundClosing.Name = string.Empty ;
				fundClosing.FundID = 0;
				fundClosing.FundClosingDate = DateTime.MinValue ; 
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.FundClosing fundClosing, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			fundClosing.Name = GetString(50 + delta);
			//fundClosing.FundID = 0 + delta ;
			//fundClosing.FundClosingDate = DateTime.Now + delta;
		}
		#endregion
    }
}