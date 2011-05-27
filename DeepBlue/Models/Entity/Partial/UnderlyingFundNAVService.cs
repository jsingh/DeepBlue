using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundNAVService {
		void SaveUnderlyingFundNAV(UnderlyingFundNAV underlyingFundNAV);
	}
	public class UnderlyingFundNAVService : IUnderlyingFundNAVService {

		#region IUnderlyingFundNAVService Members

		public void SaveUnderlyingFundNAV(UnderlyingFundNAV underlyingFundNAV) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundNAV.UnderlyingFundNAVID == 0) {
					context.UnderlyingFundNAVs.AddObject(underlyingFundNAV);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundNAVs", underlyingFundNAV);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundNAV);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}