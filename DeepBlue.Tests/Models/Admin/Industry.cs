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
    public class IndustryTest : Base {
		public DeepBlue.Models.Entity.Industry DefaultIndustry { get; set; }

        public Mock<IIndustryService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IIndustryService>();

			DefaultIndustry = new DeepBlue.Models.Entity.Industry(MockService.Object);
            MockService.Setup(x => x.SaveIndustry(It.IsAny<DeepBlue.Models.Entity.Industry>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.Industry industry, bool ifValid) {
			RequiredFieldDataMissing(industry, ifValid);
			StringLengthInvalidData(industry, ifValid);			
		}

		#region Industry
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Industry industry, bool ifValidData) {
			if (ifValidData) {
				industry.Industry1 = "Industry1";
			}
			else{
				industry.Industry1 = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.Industry industry, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			industry.Industry1 = GetString(100 + delta);
		}
		#endregion
    }
}