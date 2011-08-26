﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor;
using DeepBlue.Helpers;


namespace DeepBlue.Controllers.Investor {
	public class InvestorRepository : IInvestorRepository {

		#region IInvestorRepository Investors

		public Models.Entity.Investor FindInvestor(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				Models.Entity.Investor deepBlueinvestor = context.Investors
							  .Include("InvestorAddresses")
							  .Include("InvestorAddresses.Address")
							  .Include("InvestorContacts")
							  .Include("InvestorContacts.Contact")
							  .Include("InvestorContacts.Contact.ContactAddresses")
							  .Include("InvestorContacts.Contact.ContactAddresses.Address")
							  .Include("InvestorContacts.Contact.ContactCommunications")
							  .Include("InvestorContacts.Contact.ContactCommunications.Communication")
							  .Include("InvestorAccounts")
							  .Include("InvestorCommunications")
							  .Include("InvestorCommunications.Communication")
							  .Include("InvestorFunds")
							  .Include("InvestorFunds.Fund")
							  .SingleOrDefault(investor => investor.InvestorID == investorId);
				return deepBlueinvestor;
			}
		}

		public List<InvestorFund> FindInvestorFunds(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorFund in context.InvestorFunds
						.Include("Fund")
						.Include("InvestorFundTransactions")
						where investorFund.InvestorID == investorId
						select investorFund)
						.ToList();
			}
		}

		public InvestorFund FindInvestorFund(int investorId, int fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFunds
						.Include("Fund")
						.Include("Investor")
						.Include("InvestorType")
						.Include("InvestorFundTransactions")
						.SingleOrDefault(investorFund => investorFund.InvestorID == investorId && investorFund.FundID == fundId);
			}
		}

		public InvestorFund FindInvestorFund(int investorFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFunds
						.Include("Fund")
						.Include("Investor")
						.Include("InvestorType")
						.Include("InvestorFundTransactions")
						.SingleOrDefault(investorFund => investorFund.InvestorFundID == investorFundId);
			}
		}


		public InvestorFundTransaction FindInvestorFundTransaction(int transactionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFundTransactions.SingleOrDefault(investorFundTransaction => investorFundTransaction.InvestorFundTransactionID == transactionId);
			}
		}

		public bool Delete(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DeepBlue.Models.Entity.Investor deepBlueInvestor = context.Investors.SingleOrDefault(investor => investor.InvestorID == investorId);
				if (deepBlueInvestor != null) {
					if (deepBlueInvestor.CapitalCallLineItems.Count > 0 && deepBlueInvestor.CapitalDistributionLineItems.Count > 0) {
						return false;
					}
					List<InvestorAddress> investorAddresses = deepBlueInvestor.InvestorAddresses.ToList();
					foreach (var investorAddress in investorAddresses) {
						context.Addresses.DeleteObject(investorAddress.Address);
						context.InvestorAddresses.DeleteObject(investorAddress);
					}
					List<InvestorAccount> investorAccounts = deepBlueInvestor.InvestorAccounts.ToList();
					foreach (var investorAccount in investorAccounts) {
						context.InvestorAccounts.DeleteObject(investorAccount);
					}
					List<InvestorContact> investorContacts = deepBlueInvestor.InvestorContacts.ToList();
					foreach (var investorContact in investorContacts) {
						List<ContactAddress> contactAddresses = investorContact.Contact.ContactAddresses.ToList();
						foreach (var contactAddress in contactAddresses) {
							context.Addresses.DeleteObject(contactAddress.Address);
							context.ContactAddresses.DeleteObject(contactAddress);
						}
						context.Contacts.DeleteObject(investorContact.Contact);
						context.InvestorContacts.DeleteObject(investorContact);
					}
					List<InvestorCommunication> investorCommunications = deepBlueInvestor.InvestorCommunications.ToList();
					foreach (var investorCommunication in investorCommunications) {
						context.Communications.DeleteObject(investorCommunication.Communication);
						context.InvestorCommunications.DeleteObject(investorCommunication);
					}
					List<InvestorFund> investorFunds = deepBlueInvestor.InvestorFunds.ToList();
					foreach (var investorFund in investorFunds) {
						List<InvestorFundTransaction> investorFundTransactions = investorFund.InvestorFundTransactions.ToList();
						foreach (var investorFundTransaction in investorFundTransactions) {
							context.InvestorFundTransactions.DeleteObject(investorFundTransaction);
						}
						context.InvestorFunds.DeleteObject(investorFund);
					}
					List<InvestorFundDocument> investorFundDocuments = deepBlueInvestor.InvestorFundDocuments.ToList();
					foreach (var document in investorFundDocuments) {
						context.InvestorFundDocuments.DeleteObject(document);
					}
					context.Investors.DeleteObject(deepBlueInvestor);
					context.SaveChanges();
				}
				return true;
			}
		}

		public void DeleteInvestorContact(int investorContactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorContact investorContact = context.InvestorContacts.SingleOrDefault(contact => contact.ContactID == investorContactId);
				if (investorContact != null) {
					List<ContactAddress> investorContactAddresses = investorContact.Contact.ContactAddresses.ToList();
					foreach (var contactAddress in investorContactAddresses) {
						context.Addresses.DeleteObject(contactAddress.Address);
						context.ContactAddresses.DeleteObject(contactAddress);
					}
					context.Contacts.DeleteObject(investorContact.Contact);
				}
				context.InvestorContacts.DeleteObject(investorContact);
				context.SaveChanges();
			}
		}

		public void DeleteInvestorAccount(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorAccount investorAccount = context.InvestorAccounts.SingleOrDefault(account => account.InvestorAccountID == investorAccountId);
				if (investorAccount != null) {
					context.InvestorAccounts.DeleteObject(investorAccount);
				}
				context.SaveChanges();
			}
		}

		public IEnumerable<Helpers.ErrorInfo> SaveInvestorFund(InvestorFund investorFund) {
			return investorFund.Save();
		}

		public IEnumerable<Helpers.ErrorInfo> SaveInvestor(Models.Entity.Investor investor) {
			return investor.Save();
		}

		public InvestorType FindInvestorType(int investorTypeId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorTypes.SingleOrDefault(investorType => investorType.InvestorTypeID == investorTypeId);
			}
		}

		public List<AutoCompleteList> FindInvestors(string investorName, int? fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var investors = context.Investors.AsQueryable();
				investors = investors.Where(investor => investor.InvestorName.StartsWith(investorName));
				if (fundId.HasValue)
					investors = investors.Where(investor => investor.InvestorFunds.Where(investorFund => investorFund.FundID == fundId).Count() > 0);

				IQueryable<AutoCompleteList> query = (from investor in investors
													  where investor.InvestorName.StartsWith(investorName)
													  orderby investor.InvestorName
													  select new AutoCompleteList {
														  id = investor.InvestorID,
														  label = investor.InvestorName + " (" + investor.Social + ")",
														  value = investor.InvestorName
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindOtherInvestors(string investorName, int excludeInvestorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> query = (from investor in context.Investors
													  where investor.InvestorName.StartsWith(investorName) && investor.InvestorID != excludeInvestorId
													  orderby investor.InvestorName
													  select new AutoCompleteList {
														  id = investor.InvestorID,
														  label = investor.InvestorName + " (" + investor.Social + ")",
														  value = investor.InvestorName
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}


		public InvestorDetail FindInvestorDetail(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investor in context.Investors
						where investor.InvestorID == investorId
						select new InvestorDetail {
							InvestorName = investor.InvestorName,
							DisplayName = investor.Alias,
							InvestorId = investor.InvestorID,
							Social = investor.Social
						}).SingleOrDefault();
			}
		}

		public decimal FindSumOfSellAmount(int investorFundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorFundTransaction in context.InvestorFundTransactions
						where investorFundTransaction.InvestorFundID == investorFundId &&
							  investorFundTransaction.TransactionTypeID == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell
						select investorFundTransaction.Amount ?? 0).Sum();
			}
		}

		public bool InvestorNameAvailable(string invesorName, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from investor in context.Investors
						 where investor.InvestorName == invesorName && investor.InvestorID != investorId
						 select investor.InvestorID).Count()) > 0 ? true : false;
			}
		}

		public bool SocialSecurityTaxIdAvailable(string socialSecurityId, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from investor in context.Investors
						 where investor.Social == socialSecurityId && investor.InvestorID != investorId
						 select investor.InvestorID).Count()) > 0 ? true : false;
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorFundTransaction(InvestorFundTransaction investorFundTransaction) {
			return investorFundTransaction.Save();
		}

		#endregion

		#region Investor Address
		public InvestorAddress FindInvestorAddress(int investorAddressId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorAddresses.Where(investorAddress => investorAddress.InvestorAddressID == investorAddressId).SingleOrDefault();
			}
		}
		public IEnumerable<ErrorInfo> SaveInvestorAddress(InvestorAddress investorAddress) {
			return investorAddress.Save();
		}

		#endregion

		#region Investor Communication
		public InvestorCommunication FindInvestorCommunication(int investorCommunicationId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorCommunications.Where(investorCommunication => investorCommunication.InvestorCommunicationID == investorCommunicationId).SingleOrDefault();
			}
		}
		public IEnumerable<ErrorInfo> SaveInvestorCommunication(InvestorCommunication investorCommunication) {
			return investorCommunication.Save();
		}
		#endregion


	
	}
}