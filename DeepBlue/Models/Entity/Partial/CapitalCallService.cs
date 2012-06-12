using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICapitalCallService {
		void SaveCapitalCall(CapitalCall capitalCall);
		void SaveCapitalCallOnly(CapitalCall capitalCall);
	}

	public class CapitalCallService : ICapitalCallService {
		public void SaveCapitalCall(CapitalCall capitalCall) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (capitalCall.CapitalCallID == 0) {
					context.CapitalCalls.AddObject(capitalCall);
				} else {
					//Update capitalCall,capitalCall account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					foreach (var item in capitalCall.CapitalCallLineItems) {
						key = default(EntityKey);
						key = context.CreateEntityKey("CapitalCallLineItems", item);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, item);
						}
					}
					key = default(EntityKey);
					key = context.CreateEntityKey("CapitalCalls", capitalCall);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, capitalCall);
					}
				}
				context.SaveChanges();
			}
		}

		public void SaveCapitalCallOnly(CapitalCall capitalCall) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (capitalCall.CapitalCallID == 0) {
					context.CapitalCalls.AddObject(capitalCall);
				}
				else {
					//Update capitalCall,capitalCall account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					key = context.CreateEntityKey("CapitalCalls", capitalCall);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, capitalCall);
					}
				}
				context.SaveChanges();
			}
		}
	}
}