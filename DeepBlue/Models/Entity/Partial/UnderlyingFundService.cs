using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity.Partial {
	public interface IUnderlyingFundService {
		void SaveUnderlyingFund(UnderlyingFund underlyingFund);
	}

	public class UnderlyingFundService : IUnderlyingFundService {
		public void SaveUnderlyingFund(UnderlyingFund underlyingFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFund.UnderlyingtFundID == 0) {
					context.UnderlyingFunds.AddObject(underlyingFund);
				}
				else {
					UnderlyingFund updateUnderlyingFund = context.UnderlyingFunds.SingleOrDefault(deepblueUnderlyingFund => deepblueUnderlyingFund.UnderlyingtFundID == underlyingFund.UnderlyingtFundID);
					//Update underlyingFund,underlyingFund account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;

					if (underlyingFund.Account != null) {
						/* Account */
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("Accounts", underlyingFund.Account);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, underlyingFund.Account);
						}
						else {
							updateUnderlyingFund.Account = new Account {
								Account1 = underlyingFund.Account.Account1,
								AccountNumberCash = underlyingFund.Account.AccountNumberCash,
								AccountOf = underlyingFund.Account.AccountOf,
								Attention = underlyingFund.Account.Attention,
								BankName = underlyingFund.Account.BankName,
								CreatedBy = underlyingFund.Account.CreatedBy,
								CreatedDate = underlyingFund.Account.CreatedDate,
								EntityID = underlyingFund.Account.EntityID,
								Fax = underlyingFund.Account.Fax,
								FFCNumber = underlyingFund.Account.FFCNumber,
								IBAN = underlyingFund.Account.IBAN,
								IsPrimary = underlyingFund.Account.IsPrimary,
								LastUpdatedBy = underlyingFund.Account.LastUpdatedBy,
								LastUpdatedDate = underlyingFund.Account.LastUpdatedDate,
								Phone = underlyingFund.Account.Phone,
								Reference = underlyingFund.Account.Reference,
								Routing = underlyingFund.Account.Routing,
								SWIFT = underlyingFund.Account.SWIFT,
								Comments = underlyingFund.Account.Comments,
								FFC = underlyingFund.Account.FFC,
								ByOrderOf = underlyingFund.Account.ByOrderOf
							};
						}
						/* End Account */
					}
					if (underlyingFund.Contact != null) {
						/* Contact & Communication */
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("Contacts", underlyingFund.Contact);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, underlyingFund.Contact);
						}
						else {
							updateUnderlyingFund.Contact = new Contact {
								ContactName = underlyingFund.Contact.ContactName,
								Designation = underlyingFund.Contact.Designation,
								CreatedBy = underlyingFund.Contact.CreatedBy,
								CreatedDate = underlyingFund.Contact.CreatedDate,
								EntityID = underlyingFund.Contact.EntityID,
								FirstName = underlyingFund.Contact.FirstName,
								LastName = underlyingFund.Contact.LastName,
								LastUpdatedBy = underlyingFund.Contact.LastUpdatedBy,
								LastUpdatedDate = underlyingFund.Contact.LastUpdatedDate,
								MiddleName = underlyingFund.Contact.MiddleName,
								ReceivesDistributionNotices = underlyingFund.Contact.ReceivesDistributionNotices,
								ReceivesFinancials = underlyingFund.Contact.ReceivesFinancials,
								ReceivesInvestorLetters = underlyingFund.Contact.ReceivesInvestorLetters,
								ReceivesK1 = underlyingFund.Contact.ReceivesK1
							};

						}
						foreach (var contactCommunication in underlyingFund.Contact.ContactCommunications) {
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
								if (updateUnderlyingFund.Contact != null) {
									contact = updateUnderlyingFund.Contact;
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
						/* End Contact & Communication */
					}
					originalItem = null;
					key = default(EntityKey);
					key = context.CreateEntityKey("UnderlyingFunds", underlyingFund);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, underlyingFund);
					}
				}
				context.SaveChanges();
			}
		}
	}
}