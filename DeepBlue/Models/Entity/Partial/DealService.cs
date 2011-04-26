using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealService {
		void SaveDeal(Deal deal);
	}
	public class DealService : IDealService {

		#region IDealService Members

		public void SaveDeal(Deal deal) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (deal.DealID == 0) {
					context.Deals.AddObject(deal);
				}
				else {
					Deal updateDeal = context.Deals.SingleOrDefault(findDeal => findDeal.DealID == deal.DealID);
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;

					if (deal.Partner != null) {
						// Update partner entity
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("Partners", deal.Partner);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, deal.Partner);
						}
						else {
							updateDeal.Partner = new Partner {
								CreatedBy = deal.Partner.CreatedBy,
								CreatedDate = deal.Partner.CreatedDate,
								EntityID = deal.Partner.EntityID,
								LastUpdatedBy = deal.Partner.LastUpdatedBy,
								LastUpdatedDate = deal.Partner.LastUpdatedDate,
								PartnerID = deal.Partner.PartnerID,
								PartnerName = deal.Partner.PartnerName
							};
						}
					}

					if (deal.Contact != null) {
						// Update contact entity
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("Contacts", deal.Contact);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, deal.Contact);
						}
						else {
							updateDeal.Contact = new Contact {
								ContactID = deal.Contact.ContactID,
								ContactName = deal.Contact.ContactName,
								CreatedBy = deal.Contact.CreatedBy,
								CreatedDate = deal.Contact.CreatedDate,
								Designation = deal.Contact.Designation,
								EntityID = deal.Contact.EntityID,
								FirstName = deal.Contact.FirstName,
								LastName = deal.Contact.LastName,
								LastUpdatedBy = deal.Contact.LastUpdatedBy,
								LastUpdatedDate = deal.Contact.LastUpdatedDate,
								MiddleName = deal.Contact.MiddleName,
								ReceivesDistributionNotices = deal.Contact.ReceivesDistributionNotices,
								ReceivesFinancials = deal.Contact.ReceivesFinancials,
								ReceivesInvestorLetters = deal.Contact.ReceivesInvestorLetters,
								ReceivesK1 = deal.Contact.ReceivesK1
							};
						}
						// Update contact communication
						UpdateContactCommunication(context, deal.Contact, updateDeal.Contact);
					}

					if (deal.Contact1 != null) {
						// Update seller contact entity
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("Contacts", deal.Contact1);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, deal.Contact1);
						}
						else {
							updateDeal.Contact1 = new Contact {
								ContactID = deal.Contact1.ContactID,
								ContactName = deal.Contact1.ContactName,
								CreatedBy = deal.Contact1.CreatedBy,
								CreatedDate = deal.Contact1.CreatedDate,
								Designation = deal.Contact1.Designation,
								EntityID = deal.Contact1.EntityID,
								FirstName = deal.Contact1.FirstName,
								LastName = deal.Contact1.LastName,
								LastUpdatedBy = deal.Contact1.LastUpdatedBy,
								LastUpdatedDate = deal.Contact1.LastUpdatedDate,
								MiddleName = deal.Contact1.MiddleName,
								ReceivesDistributionNotices = deal.Contact1.ReceivesDistributionNotices,
								ReceivesFinancials = deal.Contact1.ReceivesFinancials,
								ReceivesInvestorLetters = deal.Contact1.ReceivesInvestorLetters,
								ReceivesK1 = deal.Contact1.ReceivesK1
							};
						}
						// Update seller contact communication
						UpdateContactCommunication(context, deal.Contact1, updateDeal.Contact1);
					}

					key = context.CreateEntityKey("Deals", deal);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, deal);
					}
				}
				context.SaveChanges();
			}
		}

		private void UpdateContactCommunication(DeepBlueEntities context, Contact dealContact, Contact newDealContact) {
			EntityKey key;
			object originalItem;
			// Update contact communication
			foreach (var contactCommunication in dealContact.ContactCommunications) {
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
					if (newDealContact != null) {
						contact = newDealContact;
					}
					else {
						contact = context.Contacts.SingleOrDefault(cont => cont.ContactID == contactCommunication.Contact.ContactID);
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
		}

		#endregion

	}


}