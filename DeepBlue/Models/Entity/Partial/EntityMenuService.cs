using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace DeepBlue.Models.Entity {

	public interface IEntityMenuService {
		void SaveEntityMenu(EntityMenu entityMenu);
	}

	public class EntityMenuService : IEntityMenuService {

		#region IEntityMenuService Members

		public void SaveEntityMenu(EntityMenu entityMenu) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (entityMenu.EntityMenuID == 0) {
					if (context.EntityMenus.Count() > 0)
						entityMenu.SortOrder = context.EntityMenus.Select(em => em.EntityMenuID).Max() + 1;
					else
						entityMenu.SortOrder = 1;
					context.EntityMenus.AddObject(entityMenu);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("EntityMenus", entityMenu);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, entityMenu);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}
