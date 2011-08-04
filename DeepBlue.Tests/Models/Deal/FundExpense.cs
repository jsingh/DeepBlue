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
    public class FundExpenseTest : Base {
        public DeepBlue.Models.Entity.FundExpense DefaultFundExpense { get; set; }

        public Mock<IFundExpenseService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IFundExpenseService>();

			DefaultFundExpense = new DeepBlue.Models.Entity.FundExpense(MockService.Object);
            MockService.Setup(x => x.SaveFundExpense(It.IsAny<DeepBlue.Models.Entity.FundExpense>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.FundExpense fundExpense, bool ifValid) {
			RequiredFieldDataMissing(fundExpense, ifValid);
        }

        #region FundExpense
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.FundExpense fundExpense, bool ifValidData) {
            if (ifValidData) {
				fundExpense.FundID = 1;
				fundExpense.FundExpenseTypeID = 1;
				fundExpense.CreatedBy = 1;
				fundExpense.CreatedDate = DateTime.MaxValue;
				fundExpense.LastUpdatedBy = 1;
				fundExpense.LastUpdatedDate = DateTime.MaxValue;
				fundExpense.Amount = 1;
            } else {
				fundExpense.FundID = 0;
				fundExpense.FundExpenseTypeID = 0;
				fundExpense.CreatedBy = 0;
				fundExpense.CreatedDate = DateTime.MinValue;
				fundExpense.LastUpdatedBy = 0;
				fundExpense.LastUpdatedDate = DateTime.MinValue;
				fundExpense.Amount = 0;
            }
        }
        #endregion

    }
}