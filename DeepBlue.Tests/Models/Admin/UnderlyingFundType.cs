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
	public class UnderlyingFundTypeTest : Base {
		public DeepBlue.Models.Entity.UnderlyingFundType DefaultUnderlyingFundType { get; set; }

		public Mock<IUnderlyingFundTypeService> MockService { get; set; }

		[SetUp]
		public override void Setup() {
			base.Setup();

			// Spin up mock repository and attach to controller
			MockService = new Mock<IUnderlyingFundTypeService>();

			DefaultUnderlyingFundType = new DeepBlue.Models.Entity.UnderlyingFundType(MockService.Object);
			MockService.Setup(x => x.SaveUnderlyingFundType(It.IsAny<DeepBlue.Models.Entity.UnderlyingFundType>()));
		}

		protected bool IsPropertyValid(string propertyName) {
			string errorMsg = string.Empty;
			int errorCount = 0;
			return IsModelValid(out errorMsg, out errorCount, propertyName);
		}

		protected void Create_Data(DeepBlue.Models.Entity.UnderlyingFundType underlyingFundType, bool ifValid) {
			RequiredFieldDataMissing(underlyingFundType, ifValid);
			StringLengthInvalidData(underlyingFundType, ifValid);
		}

		#region UnderlyingFundType
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.UnderlyingFundType underlyingFundType, bool ifValidData) {
			if (ifValidData) {
				underlyingFundType.Name = "UnderlyingFundType";
			}
			else {
				underlyingFundType.Name = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.UnderlyingFundType underlyingFundType, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
			underlyingFundType.Name = GetString(50 + delta);
		}
		#endregion
	}
}