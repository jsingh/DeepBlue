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
    public class GeographyTest : Base {
		public DeepBlue.Models.Entity.Geography DefaultGeography { get; set; }

        public Mock<IGeographyService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IGeographyService>();

			DefaultGeography = new DeepBlue.Models.Entity.Geography(MockService.Object);
            MockService.Setup(x => x.SaveGeography(It.IsAny<DeepBlue.Models.Entity.Geography>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.Geography geography, bool ifValid) {
			RequiredFieldDataMissing(geography, ifValid);
			StringLengthInvalidData(geography, ifValid);			
		}

		#region Geography
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Geography geography, bool ifValidData) {
			if (ifValidData) {
				geography.Geography1 = "Geography";
			}
			else{
				geography.Geography1 = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.Geography geography, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			geography.Geography1 = GetString(100 + delta);
		}
		#endregion
    }
}