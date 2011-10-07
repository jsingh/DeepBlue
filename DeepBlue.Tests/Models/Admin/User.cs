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
    public class UserTest : Base {
		public DeepBlue.Models.Entity.USER DefaultUser { get; set; }

        public Mock<IUserService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
			MockService = new Mock<IUserService>();

            DefaultUser = new DeepBlue.Models.Entity.USER(MockService.Object);
            MockService.Setup(x => x.SaveUser(It.IsAny<DeepBlue.Models.Entity.USER>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.USER user, bool ifValid) {
            RequiredFieldDataMissing(user, ifValid);
            StringLengthInvalidData(user, ifValid);			
		}

		#region User
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.USER user, bool ifValidData) {
			if (ifValidData) {
                user.UserID = 1;
                user.FirstName = "FirstName";
                user.LastName = "LastName";
                user.PasswordHash = "Password";
                user.Email = "Email@emil.com";
			}
			else{
                user.UserID = 0;
                user.FirstName = string.Empty;
                user.LastName = string.Empty;
                user.PasswordHash = string.Empty;
                user.Email = string.Empty;
			}
		}

		private void StringLengthInvalidData(DeepBlue.Models.Entity.USER user, bool ifValidData) {
			int delta = 0;
			if (!ifValidData) {
				delta = 1;
			}
            user.FirstName = GetString(30 + delta);
            user.LastName = GetString(30 + delta);
            user.PasswordHash = GetString(50 + delta);
            user.Email = GetString(50 + delta);
		}
		#endregion
    }
}