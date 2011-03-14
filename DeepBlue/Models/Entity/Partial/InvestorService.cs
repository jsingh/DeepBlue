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
				} else {
					context.Investors.Attach(new Investor { InvestorID = investor.InvestorID });
					context.Investors.ApplyCurrentValues(investor);
					// Update investor account,address,contact,communication values
					foreach (var investorAccount in investor.InvestorAccounts) {
						if (investorAccount.InvestorAccountID > 0) {
							context.InvestorAccounts.SingleOrDefault(account => account.InvestorAccountID == investorAccount.InvestorAccountID);
							context.InvestorAccounts.ApplyCurrentValues(investorAccount);
						} else {
							//	 context.ObjectStateManager.ChangeObjectState(investorAccount, System.Data.EntityState.Added);
							//	context.InvestorAccounts.AddObject(investorAccount);
							//updateInvestor.InvestorAccounts.Add(new InvestorAccount {
							//    Account = investorAccount.Account,
							//    Attention = investorAccount.Attention,
							//    Comments = investorAccount.Comments,
							//    CreatedBy = investorAccount.CreatedBy,
							//    EntityID = investorAccount.EntityID,
							//    InvestorAccountID = investorAccount.InvestorAccountID,
							//    InvestorID = investor.InvestorID,
							//    IsPrimary = investorAccount.IsPrimary,
							//    LastUpdatedBy = investorAccount.LastUpdatedBy,
							//    LastUpdatedDate = investorAccount.LastUpdatedDate,
							//    Reference = investorAccount.Reference,
							//    Routing = investorAccount.Routing,
							//    CreatedDate = investorAccount.CreatedDate
							//});
						}
					}

					foreach (var investorAddress in investor.InvestorAddresses) {
						if (investorAddress.InvestorAddressID > 0) {
							context.InvestorAddresses.Attach(new InvestorAddress { InvestorAddressID = investorAddress.InvestorAddressID });
							context.InvestorAddresses.ApplyCurrentValues(investorAddress);
						} else {
							//updateInvestor.InvestorAddresses.Add(new InvestorAddress {
							//    Address = new Address {
							//        Address1 = investorAddress.Address.Address1,
							//        Address2 = investorAddress.Address.Address2,
							//        Address3 = investorAddress.Address.Address3,
							//        AddressType = investorAddress.Address.AddressType,
							//        AddressTypeID = investorAddress.Address.AddressTypeID,
							//        City = investorAddress.Address.City,
							//        AddressID = investorAddress.Address.AddressID,
							//        Country = investorAddress.Address.Country,
							//        County = investorAddress.Address.County,
							//        CreatedBy = investorAddress.Address.CreatedBy,
							//        CreatedDate = investorAddress.Address.CreatedDate,
							//        EntityID = investorAddress.Address.EntityID,
							//        IsPreferred = investorAddress.Address.IsPreferred,
							//        LastUpdatedBy = investorAddress.Address.LastUpdatedBy,
							//        LastUpdatedDate = investorAddress.Address.LastUpdatedDate,
							//        Listed = investorAddress.Address.Listed,
							//        PostalCode = investorAddress.Address.PostalCode,
							//        State = investorAddress.Address.State,
							//        StProvince = investorAddress.Address.StProvince,
							//    },
							//    CreatedBy = investorAddress.CreatedBy,
							//    CreatedDate = investorAddress.CreatedDate,
							//    EntityID = investorAddress.EntityID,
							//    InvestorID = investor.InvestorID,
							//    LastUpdatedBy = investorAddress.LastUpdatedBy,
							//    LastUpdatedDate = investorAddress.LastUpdatedDate,
							//});
						}
						if (investorAddress.Address.AddressID > 0) {
							context.Addresses.Attach(new Address { AddressID = investorAddress.Address.AddressID });
							context.Addresses.ApplyCurrentValues(investorAddress.Address);
						}
					}
					//foreach (var investorCommunication in investor.InvestorCommunications) {
					//    if (investorCommunication.Communication.CommunicationID > 0) {
					//        context.Communications.Attach(new Communication { CommunicationID = investorCommunication.Communication.CommunicationID });
					//        context.Communications.ApplyCurrentValues(investorCommunication.Communication);
					//    }
					//    if (investorCommunication.InvestorCommunicationID > 0) {
					//        context.InvestorCommunications.Attach(new InvestorCommunication { InvestorCommunicationID = investorCommunication.InvestorCommunicationID });
					//        context.InvestorCommunications.ApplyCurrentValues(investorCommunication);
					//    }
					//}
					foreach (var investorContact in investor.InvestorContacts) {
						if (investorContact.InvestorContactID > 0) {
							context.InvestorContacts.Attach(new InvestorContact { InvestorContactID = investorContact.InvestorContactID });
							context.InvestorContacts.ApplyCurrentValues(investorContact);
						}
						foreach (var contactAddress in investorContact.Contact.ContactAddresses) {
							if (contactAddress.ContactAddressID > 0) {
								context.ContactAddresses.Attach(new ContactAddress { ContactAddressID = contactAddress.ContactAddressID });
								context.ContactAddresses.ApplyCurrentValues(contactAddress);
							}
							if (contactAddress.Address.AddressID > 0) {
								context.Addresses.Attach(new Address { AddressID = contactAddress.Address.AddressID });
								context.Addresses.ApplyCurrentValues(contactAddress.Address);
							}
						}
					}
				}
				context.SaveChanges();
			}

		}
	}

 
}