using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Fund {
    public class FundTest : Base {
        public DeepBlue.Models.Entity.Fund DefaultFund { get; set; }

        public Mock<IFundService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IFundService>();

			DefaultFund = new DeepBlue.Models.Entity.Fund(MockService.Object);
            MockService.Setup(x => x.SaveFund(It.IsAny<DeepBlue.Models.Entity.Fund>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.Fund fund, bool ifValid) {
			RequiredFieldDataMissing(fund, ifValid);
			StringLengthInvalidData(fund, ifValid);

        }

        #region Fund
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.Fund fund, bool ifValidData) {
            if (ifValidData) {
				fund.EntityID = 1 ;
				fund.FundName= "FundName";
				fund.TaxID = "1";
				fund.InceptionDate = DateTime.Now;
				fund.FundAccounts.Add(new FundAccount {
					BankName="Bank Name",
					Account ="AC0034300349304"
				});

            } else {
				fund.EntityID = 0;
				fund.FundName = string.Empty;
				fund.TaxID = string.Empty;
				fund.InceptionDate = DateTime.MinValue;
				fund.FundAccounts.Add(new FundAccount{
					BankName = string.Empty,
					Account = string.Empty
				});
            }
        }

        private void StringLengthInvalidData(DeepBlue.Models.Entity.Fund fund, bool ifValidData) {
            int delta = 0;
            if (!ifValidData) {
                delta = 1;
            }
            fund.FundName = GetString(50 + delta);
			fund.FundAccounts.Add(new FundAccount {
				BankName = GetString(50 + delta),
				Account = GetString(40 + delta)
			});
        }
        #endregion

    }
}