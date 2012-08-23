using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IVirtualAccountService {
		void SaveVirtualAccount(VirtualAccount virtualAccount);
	}

	public class VirtualAccountService : IVirtualAccountService {
		public void SaveVirtualAccount(VirtualAccount virtualAccount) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (virtualAccount.VirtualAccountID == 0) {
					context.VirtualAccounts.AddObject(virtualAccount);
				} else {
					EntityKey key;
					object originalItem;
					originalItem = null;
					key = default(EntityKey);
					key = context.CreateEntityKey("VirtualAccounts", virtualAccount);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, virtualAccount);
					}
				}
				context.SaveChanges();
			}
		}
	}
}