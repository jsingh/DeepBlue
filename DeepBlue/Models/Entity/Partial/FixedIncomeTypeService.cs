using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFixedIncomeTypeService {
		void SaveFixedIncomeType(FixedIncomeType fixedIncomeType);
	}
	public class FixedIncomeTypeService : IFixedIncomeTypeService {

		#region IFixedIncomeTypeService Members

		public void SaveFixedIncomeType(FixedIncomeType fixedIncomeType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fixedIncomeType.FixedIncomeTypeID == 0) {
					context.FixedIncomeTypes.AddObject(fixedIncomeType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("FixedIncomeTypes", fixedIncomeType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, fixedIncomeType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}