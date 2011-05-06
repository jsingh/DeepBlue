using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IGeographyService {
		void SaveGeography(Geography geography);
	}
	public class GeographyService : IGeographyService {

		#region IGeographyService Members

		public void SaveGeography(Geography geography) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (geography.GeographyID == 0) {
					context.Geographies.AddObject(geography);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Geographies", geography);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, geography);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}