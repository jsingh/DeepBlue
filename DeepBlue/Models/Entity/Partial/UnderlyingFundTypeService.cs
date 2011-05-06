using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundTypeService {
		void SaveUnderlyingFundType(UnderlyingFundType underlyingFundType);
	}
	public class UnderlyingFundTypeService : IUnderlyingFundTypeService {

		#region IUnderlyingFundTypeService Members

		public void SaveUnderlyingFundType(UnderlyingFundType underlyingFundType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundType.UnderlyingFundTypeID == 0) {
					context.UnderlyingFundTypes.AddObject(underlyingFundType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundTypes", underlyingFundType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}