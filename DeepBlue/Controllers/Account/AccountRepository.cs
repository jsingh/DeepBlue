using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using System.Reflection;
using System.Linq.Expressions;
using DeepBlue.Models.Deal;
 

namespace DeepBlue.Controllers.Account {
	public class AccountRepository : IAccountRepository {

		public ENTITY FetchUserEntity(int entityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.ENTITies.Where(entity => entity.EntityID == entityId && entity.Enabled == true).SingleOrDefault();
			}
		}

		public USER FetchUserLogin(string userName, int entityId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.USERs.Where(user => user.Login == userName && user.EntityID == entityId && user.Enabled == true).SingleOrDefault();
			}
		}
	}
}