using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IMenuService {
		void SaveMenu(Menu menu);
	}

	public class MenuService : IMenuService {

		#region IMenuService Members

		public void SaveMenu(Menu menu) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (menu.MenuID == 0) {
					context.Menus.AddObject(menu);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Menus", menu);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, menu);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}
