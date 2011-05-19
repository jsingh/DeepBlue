using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundCapitalCallLineItemService {
		void SaveUnderlyingFundCapitalCallLineItem(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem);
	}

	public class UnderlyingFundCapitalCallLineItemService : IUnderlyingFundCapitalCallLineItemService {
		public void SaveUnderlyingFundCapitalCallLineItem(UnderlyingFundCapitalCallLineItem underlyingFundCapitalCallLineItem) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundCapitalCallLineItem.UnderlyingFundCapitalCallLineItemID == 0) {
					context.UnderlyingFundCapitalCallLineItems.AddObject(underlyingFundCapitalCallLineItem);
				}
				else {
					//Update underlyingFundCapitalCallLineItem,underlyingFundCapitalCallLineItem account values
					//Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					key = context.CreateEntityKey("UnderlyingFundCapitalCallLineItems", underlyingFundCapitalCallLineItem);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundCapitalCallLineItem);
					}
				}
				context.SaveChanges();
			}
		}
	}
}