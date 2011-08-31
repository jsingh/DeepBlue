using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor;
using DeepBlue.Helpers;


namespace DeepBlue.Controllers.Investor {
	public class InvestorRepository : IInvestorRepository {

		#region Investors

		public Models.Entity.Investor FindInvestor(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.Investors.SingleOrDefault(investor => investor.InvestorID == investorId);
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
					if (deepBlueInvestor.CapitalCallLineItems.Count > 0 || deepBlueInvestor.CapitalDistributionLineItems.Count > 0) {
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
						List<ContactCommunication> contactCommunications = investorContact.Contact.ContactCommunications.ToList();
						foreach (var contactCommunication in contactCommunications) {
							context.Communications.DeleteObject(contactCommunication.Communication);
							context.ContactCommunications.DeleteObject(contactCommunication);
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

		public bool DeleteInvestorContact(int investorContactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorContact investorContact = context.InvestorContacts.SingleOrDefault(contact => contact.InvestorContactID == investorContactId);
				if (investorContact != null) {
					List<ContactAddress> investorContactAddresses = investorContact.Contact.ContactAddresses.ToList();
					foreach (var contactAddress in investorContactAddresses) {
						context.Addresses.DeleteObject(contactAddress.Address);
						context.ContactAddresses.DeleteObject(contactAddress);
					}
					List<ContactCommunication> contactCommunications = investorContact.Contact.ContactCommunications.ToList();
					foreach (var contactCommunication in contactCommunications) {
						context.Communications.DeleteObject(contactCommunication.Communication);
						context.ContactCommunications.DeleteObject(contactCommunication);
					}
					context.Contacts.DeleteObject(investorContact.Contact);
					context.InvestorContacts.DeleteObject(investorContact);
					context.SaveChanges();
					return true;
				}
				else {
					return false;
				}
			}
		}

		public bool DeleteInvestorAccount(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				InvestorAccount investorAccount = context.InvestorAccounts.SingleOrDefault(account => account.InvestorAccountID == investorAccountId);
				if (investorAccount != null) {
					context.InvestorAccounts.DeleteObject(investorAccount);
					context.SaveChanges();
					return true;
				}
				else {
					return false;
				}
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

		public InvestorDetail GetInvestorDetail(int investorId) {
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

		#region Investor Detail

		public EditModel FindInvestorDetail(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investor in context.Investors
						where investor.InvestorID == investorId
						select new EditModel {
							InvestorName = investor.InvestorName,
							DisplayName = investor.Alias,
							DomesticForeign = investor.IsDomestic,
							DomesticForeignName = (investor.IsDomestic ? "Domestic" : "Foreign"),
							EntityType = investor.InvestorEntityTypeID,
							EntityTypeName = investor.InvestorEntityType.InvestorEntityTypeName,
							SocialSecurityTaxId = investor.Social,
							StateOfResidency = investor.ResidencyState,
							StateOfResidencyName = investor.STATE.Name,
							Notes = investor.Notes,
							InvestorId = investor.InvestorID,
							FundInformations = (from investorFund in investor.InvestorFunds
												select new FundInformation {
													FundName = investorFund.Fund.FundName,
													TotalCommitment = investorFund.TotalCommitment,
													UnfundedAmount = investorFund.UnfundedAmount,
													InvestorType = investorFund.InvestorType.InvestorTypeName
												}),
							AccountInformations = (from account in investor.InvestorAccounts
												   select new {
													   ABANumber = account.Routing,
													   AccountNumber = account.Account,
													   Attention = account.Attention,
													   AccountOf = account.AccountOf,
													   BankName = account.BankName,
													   ByOrderOf = account.ByOrderOf,
													   FFC = account.FFC,
													   FFCNO = account.FFCNumber,
													   AccountId = account.InvestorAccountID,
													   IBAN = account.IBAN,
													   Reference = account.Reference,
													   Swift = account.SWIFT,
													   InvestorId = investorId
												   }),
							AddressInformations = (from investorAddress in investor.InvestorAddresses
												   select new {
													   Address1 = investorAddress.Address.Address1,
													   Address2 = investorAddress.Address.Address2,
													   AddressId = investorAddress.InvestorAddressID,
													   City = investorAddress.Address.City,
													   Country = investorAddress.Address.Country,
													   CountryName = investorAddress.Address.COUNTRY1.CountryName,
													   State = investorAddress.Address.State,
													   StateName = investorAddress.Address.STATE1.Name,
													   Zip = investorAddress.Address.PostalCode,
													   InvestorId = investorId,
													   InvestorCommunications = (from investorCommunication in investor.InvestorCommunications
																				 select new {
																					 InvestorCommunicationId = investorCommunication.InvestorCommunicationID,
																					 CommunicationTypeId = investorCommunication.Communication.CommunicationTypeID,
																					 CommunicationValue = investorCommunication.Communication.CommunicationValue
																				 }),
												   }),
							ContactInformations = (from investorContact in investor.InvestorContacts
												   select new {
													   Person = investorContact.Contact.ContactName,
													   DistributionNotices = investorContact.Contact.ReceivesDistributionNotices,
													   Financials = investorContact.Contact.ReceivesFinancials,
													   InvestorLetters = investorContact.Contact.ReceivesInvestorLetters,
													   K1 = investorContact.Contact.ReceivesK1,
													   Designation = investorContact.Contact.Designation,
													   ContactId = investorContact.ContactID,
													   InvestorId = investorId,
													   InvestorContactId = investorContact.InvestorContactID,
													   AddressInformations = (from contactAddress in investorContact.Contact.ContactAddresses
																			  select new {
																				  Address1 = contactAddress.Address.Address1,
																				  Address2 = contactAddress.Address.Address2,
																				  Country = contactAddress.Address.Country,
																				  CountryName = contactAddress.Address.COUNTRY1.CountryName,
																				  State = contactAddress.Address.State,
																				  StateName = contactAddress.Address.STATE1.Name,
																				  City = contactAddress.Address.City,
																				  Zip = contactAddress.Address.PostalCode,
																				  AddressId = contactAddress.AddressID,
																				  ContactAddressId = contactAddress.ContactAddressID
																			  }),
													   ContactCommunications = (from contactCommunication in investorContact.Contact.ContactCommunications
																				select new {
																					ContactCommunicationId = contactCommunication.ContactCommunicationID,
																					CommunicationTypeId = contactCommunication.Communication.CommunicationTypeID,
																					CommunicationValue = contactCommunication.Communication.CommunicationValue
																				}),
												   })
						}
						).SingleOrDefault();
			}
		}

		#endregion

		#region Investor Address

		public InvestorAddress FindInvestorAddress(int investorAddressId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorAddresses
					.Include("Address")
					.Where(investorAddress => investorAddress.InvestorAddressID == investorAddressId).SingleOrDefault();
			}
		}

		public object FindInvestorAddressModel(int investorAddressId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorAddress in context.InvestorAddresses
						where investorAddress.InvestorAddressID == investorAddressId
						select new {
							Address1 = investorAddress.Address.Address1,
							Address2 = investorAddress.Address.Address2,
							AddressId = investorAddress.InvestorAddressID,
							City = investorAddress.Address.City,
							Country = investorAddress.Address.Country,
							CountryName = investorAddress.Address.COUNTRY1.CountryName,
							State = investorAddress.Address.State,
							StateName = investorAddress.Address.STATE1.Name,
							Zip = investorAddress.Address.PostalCode,
							InvestorId = investorAddress.InvestorID,
							InvestorCommunications = (from investorCommunication in investorAddress.Investor.InvestorCommunications
													  select new {
														  InvestorCommunicationId = investorCommunication.InvestorCommunicationID,
														  CommunicationTypeId = investorCommunication.Communication.CommunicationTypeID,
														  CommunicationValue = investorCommunication.Communication.CommunicationValue
													  }),
						}).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorAddress(InvestorAddress investorAddress) {
			return investorAddress.Save();
		}

		#endregion

		#region Investor Communication

		public List<InvestorCommunication> FindInvestorCommunications(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorCommunications
					.Include("Communication")
					.Where(investorCommunication => investorCommunication.InvestorID == investorId).ToList();
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorCommunication(InvestorCommunication investorCommunication) {
			return investorCommunication.Save();
		}

		#endregion

		#region Investor Contact
		public InvestorContact FindInvestorContact(int investorContactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorContacts
				 .Include("Contact")
							  .Include("Contact.ContactAddresses")
							  .Include("Contact.ContactAddresses.Address")
							  .Include("Contact.ContactCommunications")
							  .Include("Contact.ContactCommunications.Communication")
					.Where(investorContact => investorContact.InvestorContactID == investorContactId).SingleOrDefault();
			}
		}

		public object FindInvestorContactModel(int investorContactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorContact in context.InvestorContacts
						where investorContact.InvestorContactID == investorContactId
						select new {
							Person = investorContact.Contact.ContactName,
							DistributionNotices = investorContact.Contact.ReceivesDistributionNotices,
							Financials = investorContact.Contact.ReceivesFinancials,
							InvestorLetters = investorContact.Contact.ReceivesInvestorLetters,
							K1 = investorContact.Contact.ReceivesK1,
							Designation = investorContact.Contact.Designation,
							ContactId = investorContact.ContactID,
							InvestorId = investorContact.InvestorID,
							InvestorContactId = investorContact.InvestorContactID,
							AddressInformations = (from contactAddress in investorContact.Contact.ContactAddresses
												   select new {
													   Address1 = contactAddress.Address.Address1,
													   Address2 = contactAddress.Address.Address2,
													   Country = contactAddress.Address.Country,
													   CountryName = contactAddress.Address.COUNTRY1.CountryName,
													   State = contactAddress.Address.State,
													   StateName = contactAddress.Address.STATE1.Name,
													   City = contactAddress.Address.City,
													   Zip = contactAddress.Address.PostalCode,
													   AddressId = contactAddress.AddressID,
													   ContactAddressId = contactAddress.ContactAddressID
												   }),
							ContactCommunications = (from contactCommunication in investorContact.Contact.ContactCommunications
													 select new {
														 ContactCommunicationId = contactCommunication.ContactCommunicationID,
														 CommunicationTypeId = contactCommunication.Communication.CommunicationTypeID,
														 CommunicationValue = contactCommunication.Communication.CommunicationValue
													 }),
						}).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorContact(InvestorContact investorContact) {
			return investorContact.Save();
		}
		#endregion

		#region Investor Bank Account
		public InvestorAccount FindInvestorAccount(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorAccounts
					   .Where(investorAccount => investorAccount.InvestorAccountID == investorAccountId).SingleOrDefault();
			}
		}

		public object FindInvestorAccountModel(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from account in context.InvestorAccounts
						where account.InvestorAccountID == investorAccountId
						select new {
							ABANumber = account.Routing,
							AccountNumber = account.Account,
							Attention = account.Attention,
							AccountOf = account.AccountOf,
							BankName = account.BankName,
							ByOrderOf = account.ByOrderOf,
							FFC = account.FFC,
							FFCNO = account.FFCNumber,
							AccountId = account.InvestorAccountID,
							IBAN = account.IBAN,
							Reference = account.Reference,
							Swift = account.SWIFT,
							InvestorId = account.InvestorID
						}).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorAccount(InvestorAccount investorAccount) {
			return investorAccount.Save();
		}
		#endregion

		#region Investor Information
		public InvestorInformation FindInvestorInformation(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investor in context.Investors
						where investor.InvestorID == investorId
						select new InvestorInformation {
							InvestorName = investor.InvestorName,
							DisplayName = investor.Alias,
							DomesticForeign = investor.IsDomestic,
							DomesticForeignName = (investor.IsDomestic ? "Domestic" : "Foreign"),
							EntityType = investor.InvestorEntityTypeID,
							EntityTypeName = investor.InvestorEntityType.InvestorEntityTypeName,
							SocialSecurityTaxId = investor.Social,
							StateOfResidency = investor.ResidencyState,
							StateOfResidencyName = investor.STATE.Name,
							Notes = investor.Notes,
							InvestorId = investor.InvestorID,
						}).SingleOrDefault();
			}
		}
		#endregion
	}
}