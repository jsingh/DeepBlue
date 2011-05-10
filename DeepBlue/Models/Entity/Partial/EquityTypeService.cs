using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IEquityTypeService {
		void SaveEquityType(EquityType equityType);
	}
	public class EquityTypeService : IEquityTypeService {

		#region IEquityTypeService Members

		public void SaveEquityType(EquityType equityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (equityType.EquityTypeID == 0) {
					context.EquityTypes.AddObject(equityType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("EquityTypes", equityType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, equityType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}