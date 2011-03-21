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
    public class DataTypeTest : Base {
		public DeepBlue.Models.Entity.DataType  DefaultDataType { get; set; }

        public Mock<IDataTypeService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IDataTypeService>();

			DefaultDataType = new DeepBlue.Models.Entity.DataType(MockService.Object);
            MockService.Setup(x => x.SaveDataType (It.IsAny<DeepBlue.Models.Entity.DataType>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.DataType dataType, bool ifValid) {
			RequiredFieldDataMissing(dataType, ifValid);
			StringLengthInvalidData(dataType, ifValid);			
		}

		#region InvestorEntityType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.DataType dataType, bool ifValidData) {
			if (ifValidData) {
				dataType.DataTypeName = "";
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.DataType dataType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			dataType.DataTypeName = GetString(50 + delta);
		}
		#endregion
    }
}