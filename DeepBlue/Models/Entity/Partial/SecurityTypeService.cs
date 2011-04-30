using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ISecurityTypeService {
		void SaveSecurityType(SecurityType securityType);
	}
	public class SecurityTypeService : ISecurityTypeService {

		#region ISecurityTypeService Members

		public void SaveSecurityType(SecurityType securityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (securityType.SecurityTypeID == 0) {
					context.SecurityTypes.AddObject(securityType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("SecurityTypes", securityType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, securityType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}