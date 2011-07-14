using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICapitalCallLineItemService {
		void SaveCapitalCallLineItem(CapitalCallLineItem capitalCallLineItem);
	}
	public class CapitalCallLineItemService : ICapitalCallLineItemService {

		#region ICapitalCallLineItemService Members

		public void SaveCapitalCallLineItem(CapitalCallLineItem capitalCallLineItem) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (capitalCallLineItem.CapitalCallLineItemID == 0) {
					context.CapitalCallLineItems.AddObject(capitalCallLineItem);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CapitalCallLineItems", capitalCallLineItem);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, capitalCallLineItem);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}