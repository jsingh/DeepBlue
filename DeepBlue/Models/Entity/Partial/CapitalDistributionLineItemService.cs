﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICapitalDistributionLineItemService {
		void SaveCapitalDistributionLineItem(CapitalDistributionLineItem capitalDistributionLineItem);
	}
	public class CapitalDistributionLineItemService : ICapitalDistributionLineItemService {

		#region ICapitalDistributionLineItemService Members

		public void SaveCapitalDistributionLineItem(CapitalDistributionLineItem capitalCallLineItem) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (capitalCallLineItem.CapitalDistributionLineItemID == 0) {
					context.CapitalDistributionLineItems.AddObject(capitalCallLineItem);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CapitalDistributionLineItems", capitalCallLineItem);
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