using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IShareClassTypeService {
		void SaveShareClassType(ShareClassType shareClassType);
	}
	public class ShareClassTypeService : IShareClassTypeService {

		#region IShareClassTypeService Members

		public void SaveShareClassType(ShareClassType shareClassType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (shareClassType.ShareClassTypeID == 0) {
					context.ShareClassTypes.AddObject(shareClassType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ShareClassTypes", shareClassType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, shareClassType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}