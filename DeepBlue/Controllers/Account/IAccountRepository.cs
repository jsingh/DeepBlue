using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.DynamicData;
using DeepBlue.Models.Deal;

namespace DeepBlue.Controllers.Account {
	public interface IAccountRepository {
		ENTITY FetchUserEntity(int entityId);
		USER FetchUserLogin(string userName, int entityId);
	}
}
