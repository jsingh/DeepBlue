using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IEntityService {
		void SaveEntity(ENTITY entity);
	}

	public class EntityService : IEntityService {

		#region IEntityService Members

		public void SaveEntity(ENTITY entity) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (entity.EntityID == 0) {
					context.ENTITies.AddObject(entity);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ENTITies", entity);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, entity);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}