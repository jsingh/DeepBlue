using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IPurchaseTypeService {
		void SavePurchaseType(PurchaseType purchaseType);
	}
	public class PurchaseTypeService : IPurchaseTypeService {

		#region IPurchaseTypeService Members

		public void SavePurchaseType(PurchaseType purchaseType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (purchaseType.PurchaseTypeID == 0) {
					context.PurchaseTypes.AddObject(purchaseType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("PurchaseTypes", purchaseType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, purchaseType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}