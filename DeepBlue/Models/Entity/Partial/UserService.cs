using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUserService {
		void SaveUser(USER user);
	}

	public class UserService : IUserService {
		public void SaveUser(USER user) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (user.UserID == 0) {
					context.USERs.AddObject(user);
				} else {
					//Update user,user account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					key = context.CreateEntityKey("USERs", user);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, user);
					}
				}
				context.SaveChanges();
			}
		}
	}
}