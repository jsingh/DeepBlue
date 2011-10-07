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
    public class CurrencyTest : Base {
		public DeepBlue.Models.Entity.Currency DefaultCurrency { get; set; }

        public Mock<ICurrencyService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<ICurrencyService>();

            DefaultCurrency = new DeepBlue.Models.Entity.Currency(MockService.Object);
            MockService.Setup(x => x.SaveCurrency(It.IsAny<DeepBlue.Models.Entity.Currency>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.Currency currency, bool ifValid) {
            RequiredFieldDataMissing(currency, ifValid);
            StringLengthInvalidData(currency, ifValid);			
		}

		#region Currency
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Currency currency, bool ifValidData) {
			if (ifValidData) {
                currency.Currency1 = "Currency";
			}
			else{
			    currency.Currency1 = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.Currency currency, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
            currency.Currency1 = GetString(100 + delta);
		}
		#endregion
    }
}