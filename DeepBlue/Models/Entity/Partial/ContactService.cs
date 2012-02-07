using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IContactService {
		void SaveContact(Contact contact);
	}

	public class ContactService : IContactService {

		#region IContactService Members

		public void SaveContact(Contact contact) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (contact.ContactID == 0) {
					context.Contacts.AddObject(contact);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem = null;

					Contact updateContact = context.ContactsTable.SingleOrDefault(deepblueContact => deepblueContact.ContactID == contact.ContactID);

					foreach (var contactCommunication in contact.ContactCommunications) {
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
						else {
							if (updateContact != null) {
								contact.ContactCommunications.Add(new ContactCommunication {
									CreatedBy = contactCommunication.CreatedBy,
									CreatedDate = contactCommunication.CreatedDate,
									EntityID = contactCommunication.EntityID,
									LastUpdatedBy = contactCommunication.LastUpdatedBy,
									LastUpdatedDate = contactCommunication.LastUpdatedDate,
									Communication = new Communication {
										CommunicationComment = contactCommunication.Communication.CommunicationComment,
										CommunicationTypeID = contactCommunication.Communication.CommunicationTypeID,
										CommunicationValue = contactCommunication.Communication.CommunicationValue,
										CreatedBy = contactCommunication.Communication.CreatedBy,
										CreatedDate = contactCommunication.Communication.CreatedDate,
										EntityID = contactCommunication.Communication.EntityID,
										IsPreferred = contactCommunication.Communication.IsPreferred,
										LastFourPhone = contactCommunication.Communication.LastFourPhone,
										LastUpdatedBy = contactCommunication.Communication.LastUpdatedBy,
										LastUpdatedDate = contactCommunication.Communication.LastUpdatedDate,
										Listed = contactCommunication.Communication.Listed
									}
								});
							}
						}
					}

					/* End Contact & Communication */
					key = context.CreateEntityKey("Contacts", contact);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, contact);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}