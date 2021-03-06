﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealUnderlyingDirectService {
		void SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect);
	}
	public class DealUnderlyingDirectService : IDealUnderlyingDirectService {

		#region IDealUnderlyingDirectService Members

		public void SaveDealUnderlyingDirect(DealUnderlyingDirect dealUnderlyingDirect) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealUnderlyingDirect.DealUnderlyingDirectID == 0) {
					context.DealUnderlyingDirects.AddObject(dealUnderlyingDirect);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealUnderlyingDirects", dealUnderlyingDirect);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealUnderlyingDirect);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}