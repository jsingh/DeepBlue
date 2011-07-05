using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundStockDistributionLineItemService {
		void SaveUnderlyingFundStockDistributionLineItem(UnderlyingFundStockDistributionLineItem underlyingFundStockDistributionLineItem);
	}
	public class UnderlyingFundStockDistributionLineItemService : IUnderlyingFundStockDistributionLineItemService {

		#region IUnderlyingFundStockDistributionLineItemService Members

		public void SaveUnderlyingFundStockDistributionLineItem(UnderlyingFundStockDistributionLineItem underlyingFundStockDistributionLineItem) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundStockDistributionLineItem.UnderlyingFundStockDistributionLineItemID == 0) {
					context.UnderlyingFundStockDistributionLineItems.AddObject(underlyingFundStockDistributionLineItem);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundStockDistributionLineItems", underlyingFundStockDistributionLineItem);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundStockDistributionLineItem);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}