using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface ISellerTypeService {
		void SaveSellerType(SellerType sellerType);
	}

	public class SellerTypeService : ISellerTypeService {

		#region ISellerTypeService Members

		public void SaveSellerType(SellerType sellerType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (sellerType.SellerTypeID == 0) {
					context.SellerTypes.AddObject(sellerType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("SellerTypes", sellerType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, sellerType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}