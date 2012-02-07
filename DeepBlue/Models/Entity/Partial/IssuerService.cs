using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IIssuerService {
		void SaveIssuer(Issuer issuer);
	}

	public class IssuerService : IIssuerService {
		public void SaveIssuer(Issuer issuer) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (issuer.IssuerID == 0) {
					context.Issuers.AddObject(issuer);
				}
				else {
					Issuer updateIssuer = context.IssuersTable.SingleOrDefault(deepblueIssuer => deepblueIssuer.IssuerID == issuer.IssuerID);
					//Update issuer,issuer account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					key = context.CreateEntityKey("Issuers", issuer);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, issuer);
					}
				}
				context.SaveChanges();
			}
		}
	}
}