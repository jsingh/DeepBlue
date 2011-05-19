using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IInvestorService {
		void SaveInvestor(Investor investor);
	}

	public class InvestorService : IInvestorService {
		public void SaveInvestor(Investor investor) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investor.InvestorID == 0) {
					context.Investors.AddObject(investor);
				}
				else {
					EntityKey key;
					object originalItem;
					Investor updateInvestor = context.Investors.SingleOrDefault(deepblueInvestor => deepblueInvestor.InvestorID == investor.InvestorID);
					// Update investor account,address,contact,communication values
					foreach (var investorAccount in investor.InvestorAccounts) {
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("InvestorAccounts", investorAccount);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorAccount);
						}
						else {
							updateInvestor.InvestorAccounts.Add(new InvestorAccount {
								Account = investorAccount.Account,
								Attention = investorAccount.Attention,
								Comments = investorAccount.Comments,
								CreatedBy = investorAccount.CreatedBy,
								CreatedDate = investorAccount.CreatedDate,
								EntityID = investorAccount.EntityID,
								IsPrimary = investorAccount.IsPrimary,
								LastUpdatedBy = investorAccount.LastUpdatedBy,
								LastUpdatedDate = investorAccount.LastUpdatedDate,
								Reference = investorAccount.Reference,
								Routing = investorAccount.Routing,
								AccountOf = investorAccount.AccountOf,
								ByOrderOf = investorAccount.ByOrderOf,
								FFC = investorAccount.FFC,
								IBAN = investorAccount.IBAN,
								BankName = investorAccount.BankName,
								FFCNumber = investorAccount.FFCNumber,
								SWIFT = investorAccount.SWIFT
							});
						}
					}
					foreach (var investorAddress in investor.InvestorAddresses) {
						key = default(EntityKey);
						originalItem = null;
						key = default(EntityKey);
						originalItem = null;
						InvestorAddress newInvestorAddress = null;
						key = context.CreateEntityKey("InvestorAddresses", investorAddress);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorAddress);
						}
						else {
							newInvestorAddress = new InvestorAddress {
								CreatedBy = investorAddress.CreatedBy,
								CreatedDate = investorAddress.CreatedDate,
								EntityID = investorAddress.EntityID,
								LastUpdatedBy = investorAddress.LastUpdatedBy,
								LastUpdatedDate = investorAddress.LastUpdatedDate
							};
						}
						key = context.CreateEntityKey("Addresses", investorAddress.Address);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorAddress.Address);
						}
						else {
							if (newInvestorAddress != null) {
								newInvestorAddress.Address = new Address {
									Address1 = investorAddress.Address.Address1,
									Address2 = investorAddress.Address.Address2,
									Address3 = investorAddress.Address.Address3,
									AddressTypeID = investorAddress.Address.AddressTypeID,
									City = investorAddress.Address.City,
									Country = investorAddress.Address.Country,
									County = investorAddress.Address.County,
									CreatedBy = investorAddress.Address.CreatedBy,
									CreatedDate = investorAddress.Address.CreatedDate,
									EntityID = investorAddress.Address.EntityID,
									IsPreferred = investorAddress.Address.IsPreferred,
									LastUpdatedBy = investorAddress.Address.LastUpdatedBy,
									LastUpdatedDate = investorAddress.Address.LastUpdatedDate,
									Listed = investorAddress.Address.Listed,
									PostalCode = investorAddress.Address.PostalCode,
									State = investorAddress.Address.State,
									StProvince = investorAddress.Address.StProvince
								};
								updateInvestor.InvestorAddresses.Add(newInvestorAddress);
							}
						}
					}
					foreach (var investorCommunication in investor.InvestorCommunications) {
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("InvestorCommunications", investorCommunication);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorCommunication);
							key = default(EntityKey);
							originalItem = null;
							key = context.CreateEntityKey("Communications", investorCommunication.Communication);
							if (context.TryGetObjectByKey(key, out originalItem)) {
								context.ApplyCurrentValues(key.EntitySetName, investorCommunication.Communication);
							}
						}
						else {
							if (updateInvestor != null) {
								updateInvestor.InvestorCommunications.Add(new InvestorCommunication {
									CreatedBy = investorCommunication.CreatedBy,
									CreatedDate = investorCommunication.CreatedDate,
									LastUpdatedBy = investorCommunication.LastUpdatedBy,
									LastUpdatedDate = investorCommunication.LastUpdatedDate,
									EntityID = investorCommunication.EntityID,
									Communication = new Communication {
										CommunicationComment = investorCommunication.Communication.CommunicationComment,
										CommunicationTypeID = investorCommunication.Communication.CommunicationTypeID,
										CommunicationValue = investorCommunication.Communication.CommunicationValue,
										CreatedBy = investorCommunication.Communication.CreatedBy,
										CreatedDate = investorCommunication.Communication.CreatedDate,
										EntityID = investorCommunication.Communication.EntityID,
										IsPreferred = investorCommunication.Communication.IsPreferred,
										LastFourPhone = investorCommunication.Communication.LastFourPhone,
										LastUpdatedBy = investorCommunication.Communication.LastUpdatedBy,
										LastUpdatedDate = investorCommunication.Communication.LastUpdatedDate,
										Listed = investorCommunication.Communication.Listed
									}
								});
							}
						}
					}
					foreach (var investorContact in investor.InvestorContacts) {
						key = default(EntityKey);
						originalItem = null;
						InvestorContact newInvestorContact = null;
						key = context.CreateEntityKey("InvestorContacts", investorContact);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorContact);
						}
						else {
							newInvestorContact = new InvestorContact {
								CreatedBy = investorContact.CreatedBy,
								CreatedDate = investorContact.CreatedDate,
								EntityID = investorContact.EntityID,
								LastUpdatedBy = investorContact.LastUpdatedBy,
								LastUpdatedDate = investorContact.LastUpdatedDate
							};
						}
						key = default(EntityKey);
						originalItem = null;
						key = context.CreateEntityKey("Contacts", investorContact.Contact);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, investorContact.Contact);
						}
						else {
							if (newInvestorContact != null) {
								newInvestorContact.Contact = new Contact {
									ContactName = investorContact.Contact.ContactName,
									Designation = investorContact.Contact.Designation,
									CreatedBy = investorContact.Contact.CreatedBy,
									CreatedDate = investorContact.Contact.CreatedDate,
									EntityID = investorContact.Contact.EntityID,
									FirstName = investorContact.Contact.FirstName,
									LastName = investorContact.Contact.LastName,
									LastUpdatedBy = investorContact.Contact.LastUpdatedBy,
									LastUpdatedDate = investorContact.Contact.LastUpdatedDate,
									MiddleName = investorContact.Contact.MiddleName,
									ReceivesDistributionNotices = investorContact.Contact.ReceivesDistributionNotices,
									ReceivesFinancials = investorContact.Contact.ReceivesFinancials,
									ReceivesInvestorLetters = investorContact.Contact.ReceivesInvestorLetters,
									ReceivesK1 = investorContact.Contact.ReceivesK1
								};
							}
						}
						foreach (var contactAddress in investorContact.Contact.ContactAddresses) {
							key = default(EntityKey);
							originalItem = null;
							ContactAddress newContactAddress = null;
							key = context.CreateEntityKey("ContactAddresses", contactAddress);
							if (context.TryGetObjectByKey(key, out originalItem)) {
								context.ApplyCurrentValues(key.EntitySetName, contactAddress);
							}
							else {
								if (newInvestorContact != null) {
									if (newInvestorContact.Contact != null) {
										newContactAddress = new ContactAddress {
											CreatedBy = contactAddress.CreatedBy,
											CreatedDate = contactAddress.CreatedDate,
											EntityID = contactAddress.EntityID,
											LastUpdatedBy = contactAddress.LastUpdatedBy,
											LastUpdatedDate = contactAddress.LastUpdatedDate
										};
									}
								}
							}
							key = default(EntityKey);
							originalItem = null;
							key = context.CreateEntityKey("Addresses", contactAddress.Address);
							if (context.TryGetObjectByKey(key, out originalItem)) {
								context.ApplyCurrentValues(key.EntitySetName, contactAddress.Address);
							}
							else {
								if (newContactAddress != null) {
									newContactAddress.Address = new Address {
										Address1 = contactAddress.Address.Address1,
										Address2 = contactAddress.Address.Address2,
										Address3 = contactAddress.Address.Address3,
										AddressTypeID = contactAddress.Address.AddressTypeID,
										City = contactAddress.Address.City,
										Country = contactAddress.Address.Country,
										County = contactAddress.Address.County,
										CreatedBy = contactAddress.Address.CreatedBy,
										CreatedDate = contactAddress.Address.CreatedDate,
										EntityID = contactAddress.Address.EntityID,
										IsPreferred = contactAddress.Address.IsPreferred,
										LastUpdatedBy = contactAddress.Address.LastUpdatedBy,
										LastUpdatedDate = contactAddress.Address.LastUpdatedDate,
										Listed = contactAddress.Address.Listed,
										PostalCode = contactAddress.Address.PostalCode,
										State = contactAddress.Address.State,
										StProvince = contactAddress.Address.StProvince
									};
									if (newInvestorContact != null) {
										newInvestorContact.Contact.ContactAddresses.Add(newContactAddress);
										updateInvestor.InvestorContacts.Add(newInvestorContact);
									}
								}
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
							else {
								Contact contact = null;
								if (newInvestorContact != null) {
									contact = newInvestorContact.Contact;
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
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Investors", investor);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, investor);
					}
				}
				context.SaveChanges();
			}

		}
	}

	public partial class Investor {

		public List<InvestorAccount> InvestorAccountList { get; set; }
	}
}