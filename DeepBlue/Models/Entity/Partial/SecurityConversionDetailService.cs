using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ISecurityConversionDetailService {
		void SaveSecurityConversionDetail(SecurityConversionDetail securityConversionDetail);
	}
	public class SecurityConversionDetailService : ISecurityConversionDetailService {

		#region ISecurityConversionDetailService Members

		public void SaveSecurityConversionDetail(SecurityConversionDetail securityConversionDetail) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (securityConversionDetail.SecurityConversionDetailID == 0) {
					context.SecurityConversionDetails.AddObject(securityConversionDetail);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("SecurityConversionDetails", securityConversionDetail);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, securityConversionDetail);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}