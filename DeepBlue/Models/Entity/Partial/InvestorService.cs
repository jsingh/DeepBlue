using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

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
					// Update investor account,address,contact,communication values
					foreach (var investorAccount in investor.InvestorAccounts) {
						context.InvestorAccounts.Attach(new InvestorAccount { InvestorAccountID = investorAccount.InvestorAccountID });
						context.InvestorAccounts.ApplyCurrentValues(investorAccount);
					}
					foreach (var investorAddress in investor.InvestorAddresses) {
						context.Addresses.Attach(new Address { AddressID = investorAddress.Address.AddressID });
						context.Addresses.ApplyCurrentValues(investorAddress.Address);
						context.InvestorAddresses.Attach(new InvestorAddress { InvestorAddressID = investorAddress.InvestorAddressID });
						context.InvestorAddresses.ApplyCurrentValues(investorAddress);
					}
					foreach (var investorCommunication in investor.InvestorCommunications) {
						context.Communications.Attach(new Communication { CommunicationID = investorCommunication.Communication.CommunicationID });
						context.Communications.ApplyCurrentValues(investorCommunication.Communication);
						context.InvestorCommunications.Attach(new InvestorCommunication { InvestorCommunicationID = investorCommunication.InvestorCommunicationID });
						context.InvestorCommunications.ApplyCurrentValues(investorCommunication);
					}
					foreach (var investorContact in investor.InvestorContacts) {
						foreach (var contactAddress in investorContact.Contact.ContactAddresses) {
							context.Addresses.Attach(new Address { AddressID = contactAddress.Address.AddressID });
							context.Addresses.ApplyCurrentValues(contactAddress.Address);
							context.ContactAddresses.Attach(new ContactAddress { ContactAddressID = contactAddress.ContactAddressID });
							context.ContactAddresses.ApplyCurrentValues(contactAddress);
						}
						context.InvestorContacts.Attach(new InvestorContact { InvestorContactID = investorContact.InvestorContactID });
						context.InvestorContacts.ApplyCurrentValues(investorContact);
					}
					context.Investors.Attach(new Investor { InvestorID = investor.InvestorID });
					context.Investors.ApplyCurrentValues(investor);
					context.SaveChanges();
				}
				context.SaveChanges();
			}
		}
	}

 
}
