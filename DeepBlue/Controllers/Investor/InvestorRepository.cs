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
				return context.InvestorsTable.SingleOrDefault(investor => investor.InvestorID == investorId);
			}
		}

		public List<InvestorFund> FindInvestorFunds(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorFund in context.InvestorFunds
						.Include("Fund")
						.Include("InvestorFundTransactions")
						.EntityFilter()
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
						.EntityFilter()
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
						.EntityFilter()
						.SingleOrDefault(investorFund => investorFund.InvestorFundID == investorFundId);
			}
		}

		public InvestorFundTransaction FindInvestorFundTransaction(int transactionId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorFundTransactionsTable.SingleOrDefault(investorFundTransaction => investorFundTransaction.InvestorFundTransactionID == transactionId);
			}
		}

		public bool Delete(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				DeepBlue.Models.Entity.Investor deepBlueInvestor = context.InvestorsTable.SingleOrDefault(investor => investor.InvestorID == investorId);
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
				InvestorContact investorContact = context.InvestorContactsTable.SingleOrDefault(contact => contact.InvestorContactID == investorContactId);
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
				InvestorAccount investorAccount = context.InvestorAccountsTable.SingleOrDefault(account => account.InvestorAccountID == investorAccountId);
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
				return context.InvestorTypesTable.SingleOrDefault(investorType => investorType.InvestorTypeID == investorTypeId);
			}
		}

		public List<AutoCompleteList> FindInvestors(string investorName, int? fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var investors = context.InvestorsTable.AsQueryable();
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
				IQueryable<AutoCompleteList> query = (from investor in context.InvestorsTable
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
				return (from investor in context.InvestorsTable
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
				return (from investorFundTransaction in context.InvestorFundTransactionsTable
						where investorFundTransaction.InvestorFundID == investorFundId &&
							  investorFundTransaction.TransactionTypeID == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell
						select investorFundTransaction.Amount ?? 0).Sum();
			}
		}

		public bool InvestorNameAvailable(string invesorName, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from investor in context.InvestorsTable
						 where investor.InvestorName == invesorName && investor.InvestorID != investorId
						 select investor.InvestorID).Count()) > 0 ? true : false;
			}
		}

		public bool SocialSecurityTaxIdAvailable(string socialSecurityId, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return ((from investor in context.InvestorsTable
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
				return (from investor in context.InvestorsTable
						where investor.InvestorID == investorId
						select new EditModel {
							InvestorName = investor.InvestorName,
							Alias = investor.Alias,
							DomesticForeign = investor.IsDomestic,
							DomesticForeignName = (investor.IsDomestic ? "Domestic" : "Foreign"),
							EntityType = investor.InvestorEntityTypeID,
							EntityTypeName = investor.InvestorEntityType.InvestorEntityTypeName,
							SocialSecurityTaxId = investor.Social,
							StateOfResidency = investor.ResidencyState,
							StateOfResidencyName = investor.STATE.Name,
							Notes = investor.Notes,
							InvestorId = investor.InvestorID,
							AccountInformations = (from account in investor.InvestorAccounts
												   select new {
													   ABANumber = account.Routing,
													   Account = account.Account,
													   AccountNumber = account.AccountNumberCash,
													   Attention = account.Attention,
													   AccountOf = account.AccountOf,
													   BankName = account.BankName,
													   ByOrderOf = account.ByOrderOf,
													   FFC = account.FFC,
													   FFCNumber = account.FFCNumber,
													   AccountId = account.InvestorAccountID,
													   IBAN = account.IBAN,
													   Reference = account.Reference,
													   Swift = account.SWIFT,
													   InvestorId = investorId,
													   AccountPhone = account.Phone,
													   AccountFax = account.Fax
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

		public List<FundInformation> GetInvestmentDetails(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<FundInformation> query = (from investorFund in context.InvestorFundsTable
													 where investorFund.InvestorID == investorId
													 select new FundInformation {
														 FundName = investorFund.Fund.FundName,
														 TotalCommitment = investorFund.TotalCommitment,
														 UnfundedAmount = investorFund.UnfundedAmount,
														 InvestorType = investorFund.InvestorType.InvestorTypeName,
														 FundClose = (from transaction in investorFund.InvestorFundTransactions
																	  select new {
																		  FundClose = transaction.FundClosing.Name
																	  }).FirstOrDefault().FundClose,
														 FundId = investorFund.FundID,
														 InvestorTypeId = investorFund.InvestorTypeID,
														 InvestorFundId = investorFund.InvestorFundID,
														 InvestorId = investorFund.InvestorID
													 });
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<FundInformation> paginatedList = new PaginatedList<FundInformation>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion

		#region Investor Address

		public InvestorAddress FindInvestorAddress(int investorAddressId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorAddresses
					.Include("Address")
					.EntityFilter()
					.Where(investorAddress => investorAddress.InvestorAddressID == investorAddressId).SingleOrDefault();
			}
		}

		public object FindInvestorAddressModel(int investorAddressId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorAddress in context.InvestorAddressesTable
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
					.EntityFilter()
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
							  .EntityFilter()
					.Where(investorContact => investorContact.InvestorContactID == investorContactId).SingleOrDefault();
			}
		}

		public object FindInvestorContactModel(int investorContactId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investorContact in context.InvestorContactsTable
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

		public List<ContactInformation> ContactInformationList(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<ContactInformation> query = (from investorContact in context.InvestorContactsTable
														where investorContact.InvestorID == investorId
														select new ContactInformation {
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
																				   select new AddressInformation {
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
																					 select new ContactCommunicationInformation {
																						 ContactCommunicationId = contactCommunication.ContactCommunicationID,
																						 CommunicationTypeId = contactCommunication.Communication.CommunicationTypeID,
																						 CommunicationValue = contactCommunication.Communication.CommunicationValue
																					 }),
														});
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<ContactInformation> paginatedList = new PaginatedList<ContactInformation>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion

		#region Investor Bank Account
		public InvestorAccount FindInvestorAccount(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return context.InvestorAccountsTable
					   .Where(investorAccount => investorAccount.InvestorAccountID == investorAccountId).SingleOrDefault();
			}
		}

		public object FindInvestorAccountModel(int investorAccountId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from account in context.InvestorAccountsTable
						where account.InvestorAccountID == investorAccountId
						select new {
							ABANumber = account.Routing,
							AccountNumber = account.AccountNumberCash,
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
							InvestorId = account.InvestorID,
							Account = account.Account,
							AccountPhone = account.Phone,
							AccountFax = account.Fax
						}).FirstOrDefault();
			}
		}

		public IEnumerable<ErrorInfo> SaveInvestorAccount(InvestorAccount investorAccount) {
			return investorAccount.Save();
		}

		public List<AccountInformation> BankAccountInformationList(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AccountInformation> query = (from account in context.InvestorAccountsTable
														where account.InvestorID == investorId
														select new AccountInformation {
															ABANumber = account.Routing,
															AccountNumber = account.Account,
															Attention = account.Attention,
															AccountOf = account.AccountOf,
															BankName = account.BankName,
															ByOrderOf = account.ByOrderOf,
															FFC = account.FFC,
															FFCNumber = account.FFCNumber,
															AccountId = account.InvestorAccountID,
															IBAN = account.IBAN,
															Reference = account.Reference,
															Swift = account.SWIFT,
															InvestorId = account.InvestorID
														});
				query = query.OrderBy(sortName, (sortOrder == "asc"));
				PaginatedList<AccountInformation> paginatedList = new PaginatedList<AccountInformation>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		#endregion

		#region Investor Information
		public InvestorInformation FindInvestorInformation(int investorId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				return (from investor in context.InvestorsTable
						where investor.InvestorID == investorId
						select new InvestorInformation {
							InvestorName = investor.InvestorName,
							Alias = investor.Alias,
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

		public int FindLastInvestorId() {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				var lastInvestor = (from investor in context.InvestorsTable
									orderby investor.InvestorID descending
									select new {
										InvestorID = investor.InvestorID
									}).FirstOrDefault();
				return (lastInvestor != null ? lastInvestor.InvestorID : 0);
			}
		}
		#endregion

		#region Investor Library

		public List<InvertorLibraryInformation> GetInvestorLibraryList(int pageIndex, int pageSize, string sortName, string sortOrder, ref int totalRows, int? investorId, int? fundId) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<Models.Entity.Fund> fundQuery = context.FundsTable;
				if (fundId > 0) {
					fundQuery = fundQuery.Where(fund => fund.FundID == fundId);
				}
				var investorFundQuery = (from investorFund in context.InvestorFundsTable
										 where (investorId > 0 ? investorFund.InvestorID == investorId : investorFund.InvestorID > 0)
										 select new {
											 FundID = investorFund.FundID
										 }).Distinct();
				IQueryable<InvertorLibraryInformation> query = (from fund in fundQuery
																join fundInvestor in investorFundQuery on fund.FundID equals fundInvestor.FundID
																orderby fund.FundName
																select new InvertorLibraryInformation {
																	FundName = fund.FundName,
																	FundID = fund.FundID,
																	FundInformations = (from investorFund in fund.InvestorFunds
																						where (investorId > 0 ? investorFund.InvestorID == investorId : investorFund.InvestorID > 0)
																						select new FundInvestorInformation {
																							CommitmentAmount = investorFund.TotalCommitment,
																							CommittedDate = investorFund
																											.InvestorFundTransactions
																											.Where(transaction => transaction.FundClosingID > 0)
																											.FirstOrDefault().CommittedDate,
																							FundClose = investorFund
																											.InvestorFundTransactions
																											.Where(transaction => transaction.FundClosingID > 0)
																											.FirstOrDefault().FundClosing.Name,
																							CloseDate = investorFund
																											.InvestorFundTransactions
																											.Where(transaction => transaction.FundClosingID > 0)
																											.FirstOrDefault().FundClosing.FundClosingDate,
																							InvestorID = investorFund.InvestorID,
																							InvestorName = investorFund.Investor.InvestorName,
																							UnfundedAmount = investorFund.UnfundedAmount
																						})
																}
				);
				PaginatedList<InvertorLibraryInformation> paginatedList = new PaginatedList<InvertorLibraryInformation>(query, pageIndex, pageSize);
				totalRows = paginatedList.TotalCount;
				return paginatedList;
			}
		}

		public List<AutoCompleteList> FindInvestorFunds(string fundName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> fundListQuery = (from fund in context.FundsTable
															  where fund.InvestorFunds.Count() > 0
															  where fund.FundName.StartsWith(fundName)
															  orderby fund.FundName
															  select new AutoCompleteList {
																  id = fund.FundID,
																  label = fund.FundName,
																  value = fund.FundName
															  });
				return new PaginatedList<AutoCompleteList>(fundListQuery, 1, AutoCompleteOptions.RowsLength);
			}
		}

		public List<AutoCompleteList> FindFundInvestors(string investorName) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				IQueryable<AutoCompleteList> query = (from investor in context.InvestorsTable
													  where investor.InvestorName.StartsWith(investorName) && investor.InvestorFunds.Count() > 0
													  orderby investor.InvestorName
													  select new AutoCompleteList {
														  id = investor.InvestorID,
														  label = investor.InvestorName + " (" + investor.Social + ")",
														  value = investor.InvestorName
													  });
				return new PaginatedList<AutoCompleteList>(query, 1, AutoCompleteOptions.RowsLength);
			}
		}

		#endregion
	}
}