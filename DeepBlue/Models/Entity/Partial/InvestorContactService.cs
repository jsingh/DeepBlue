using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity.Partial {
	public interface IInvestorContactService {
		void SaveInvestorContact(InvestorContact investorContact);
	}

	public class InvestorContactService : IInvestorContactService {
		public void SaveInvestorContact(InvestorContact investorContact) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorContact.InvestorContactID == 0) {
					context.InvestorContacts.AddObject(investorContact);
				}
				else {
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("InvestorContacts", investorContact);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorContact);
					}
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Contacts", investorContact.Contact);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investorContact.Contact);
					}
					foreach (var contactAddress in investorContact.Contact.ContactAddresses) {
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("ContactAddresses", contactAddress);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, contactAddress);
						}
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("Addresses", contactAddress.Address);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, contactAddress.Address);
						}
					}
					foreach (var contactCommunication in investorContact.Contact.ContactCommunications) {
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("ContactCommunications", contactCommunication);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, contactCommunication);
							key = default(EntityKey);
							originalItem = null;
							key = context.CreateEntityKey("Communications", contactCommunication.Communication);
							if (context.TryGetObjectByKey(key, out originalItem)) {
								context.ApplyCurrentValues(key.EntitySetName, contactCommunication.Communication);
							}
						}
					}
				}
				context.SaveChanges();
			}

		}
	}
}