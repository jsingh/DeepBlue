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
    public class EquitySplitTest : Base {
        public DeepBlue.Models.Entity.EquitySplit DefaultEquitySplit { get; set; }

        public Mock<IEquitySplitService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IEquitySplitService>();

			DefaultEquitySplit = new DeepBlue.Models.Entity.EquitySplit(MockService.Object);
            MockService.Setup(x => x.SaveEquitySplit(It.IsAny<DeepBlue.Models.Entity.EquitySplit>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

        protected void Create_Data(DeepBlue.Models.Entity.EquitySplit equitySplit, bool ifValid) {
			RequiredFieldDataMissing(equitySplit, ifValid);
        }

        #region EquitySplit
        private void RequiredFieldDataMissing(DeepBlue.Models.Entity.EquitySplit equitySplit, bool ifValidData) {
            if (ifValidData) {
				equitySplit.CreatedBy = 1;
				equitySplit.CreatedDate = DateTime.MaxValue;
				equitySplit.LastUpdatedBy = 1;
				equitySplit.LastUpdatedDate = DateTime.MaxValue;
				equitySplit.EquityID = 1;
				equitySplit.SplitFactor = 1;
				equitySplit.SplitDate = DateTime.MaxValue;
            } else {
				equitySplit.CreatedBy = 0;
				equitySplit.CreatedDate = DateTime.MinValue;
				equitySplit.LastUpdatedBy = 0;
				equitySplit.LastUpdatedDate = DateTime.MinValue;
				equitySplit.EquityID = 0;
				equitySplit.SplitFactor = 0;
				equitySplit.SplitDate = DateTime.MinValue;
            }
        }
        #endregion

    }
}