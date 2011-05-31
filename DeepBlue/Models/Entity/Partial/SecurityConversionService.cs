using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ISecurityConversionService {
		void SaveSecurityConversion(SecurityConversion securityType);
	}
	public class SecurityConversionService : ISecurityConversionService {

		#region ISecurityConversionService Members

		public void SaveSecurityConversion(SecurityConversion securityConversion) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (securityConversion.SecurityConversionID == 0) {
					context.SecurityConversions.AddObject(securityConversion);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("SecurityConversions", securityConversion);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, securityConversion);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}