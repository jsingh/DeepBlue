using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IEquitySplitService {
		void SaveEquitySplit(EquitySplit equitySplit);
	}
	public class EquitySplitService : IEquitySplitService {

		#region IEquitySplitService Members

		public void SaveEquitySplit(EquitySplit equitySplit) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (equitySplit.EquiteSplitID == 0) {
					context.EquitySplits.AddObject(equitySplit);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("EquitySplits", equitySplit);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, equitySplit);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}