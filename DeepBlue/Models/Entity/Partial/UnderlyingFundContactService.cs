using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundContactService {
		void SaveUnderlyingFundContact(UnderlyingFundContact underlyingFundContact);
	}
	public class UnderlyingFundContactService : IUnderlyingFundContactService {

		#region IUnderlyingFundContactService Members

		public void SaveUnderlyingFundContact(UnderlyingFundContact underlyingFundContact) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundContact.UnderlyingFundContactID  == 0) {
					context.UnderlyingFundContacts.AddObject(underlyingFundContact);
				}
				else {
					EntityKey key;
					object originalItem;
					UnderlyingFundContact updateUnderlyingFundContact = context.UnderlyingFundContactsTable.SingleOrDefault(deepblueUnderlyingFundContact => deepblueUnderlyingFundContact.UnderlyingFundContactID == underlyingFundContact.UnderlyingFundContactID);
					if (underlyingFundContact.Contact != null) {
						/* Contact & Communication */
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("Contacts", underlyingFundContact.Contact);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, underlyingFundContact.Contact);
						}
						else {
							updateUnderlyingFundContact.Contact = new Contact {
								ContactName = underlyingFundContact.Contact.ContactName,
								Designation = underlyingFundContact.Contact.Designation,
								CreatedBy = underlyingFundContact.Contact.CreatedBy,
								CreatedDate = underlyingFundContact.Contact.CreatedDate,
								EntityID = underlyingFundContact.Contact.EntityID,
								FirstName = underlyingFundContact.Contact.FirstName,
								LastName = underlyingFundContact.Contact.LastName,
								LastUpdatedBy = underlyingFundContact.Contact.LastUpdatedBy,
								LastUpdatedDate = underlyingFundContact.Contact.LastUpdatedDate,
								MiddleName = underlyingFundContact.Contact.MiddleName,
								ReceivesDistributionNotices = underlyingFundContact.Contact.ReceivesDistributionNotices,
								ReceivesFinancials = underlyingFundContact.Contact.ReceivesFinancials,
								ReceivesInvestorLetters = underlyingFundContact.Contact.ReceivesInvestorLetters,
								ReceivesK1 = underlyingFundContact.Contact.ReceivesK1
							};

						}
						foreach (var contactCommunication in underlyingFundContact.Contact.ContactCommunications) {
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
								Contact contact = null;
								if (updateUnderlyingFundContact.Contact != null) {
									contact = updateUnderlyingFundContact.Contact;
								}
								else {
									contact = context.ContactsTable.SingleOrDefault(cont => cont.ContactID == contactCommunication.Contact.ContactID);
								}
								if (contact != null) {
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
					}

					// Define an ObjectStateEntry and EntityKey for the current object. 
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundContacts", underlyingFundContact);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundContact);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}