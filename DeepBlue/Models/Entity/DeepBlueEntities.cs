using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	public partial class DeepBlueEntities : ObjectContext {
		#region Entity Filters
		public IQueryable<Account> AccountsTable {
			get {
				return this.Accounts.EntityFilter();
			}
		}
		public IQueryable<ActivityType> ActivityTypesTable {
			get {
				return this.ActivityTypes.EntityFilter();
			}
		}
		public IQueryable<Address> AddressesTable {
			get {
				return this.Addresses.EntityFilter();
			}
		}
		public IQueryable<AddressType> AddressTypesTable {
			get {
				return this.AddressTypes.EntityFilter();
			}
		}
		public IQueryable<AnnualMeetingHistory> AnnualMeetingHistoriesTable {
			get {
				return this.AnnualMeetingHistories.EntityFilter();
			}
		}
		public IQueryable<CapitalCall> CapitalCallsTable {
			get {
				return this.CapitalCalls
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<CapitalCallLineItem> CapitalCallLineItemsTable {
			get {
				return this.CapitalCallLineItems
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<CapitalCallLineItemType> CapitalCallLineItemTypesTable {
			get {
				return this.CapitalCallLineItemTypes.EntityFilter();
			}
		}
		public IQueryable<CapitalCallType> CapitalCallTypesTable {
			get {
				return this.CapitalCallTypes.EntityFilter();
			}
		}
		public IQueryable<CapitalDistribution> CapitalDistributionsTable {
			get {
				return this.CapitalDistributions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<CapitalDistributionLineItem> CapitalDistributionLineItemsTable {
			get {
				return this.CapitalDistributionLineItems
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<CapitalDistributionProfit> CapitalDistributionProfitsTable {
			get {
				return this.CapitalDistributionProfits.EntityFilter();
			}
		}
		public IQueryable<CashDistribution> CashDistributionsTable {
			get {
				return this.CashDistributions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<CashDistributionType> CashDistributionTypesTable {
			get {
				return this.CashDistributionTypes.EntityFilter();
			}
		}
		public IQueryable<Communication> CommunicationsTable {
			get {
				return this.Communications.EntityFilter();
			}
		}
		public IQueryable<CommunicationGrouping> CommunicationGroupingsTable {
			get {
				return this.CommunicationGroupings.EntityFilter();
			}
		}
		public IQueryable<CommunicationType> CommunicationTypesTable {
			get {
				return this.CommunicationTypes.EntityFilter();
			}
		}
		public IQueryable<Contact> ContactsTable {
			get {
				return this.Contacts.EntityFilter();
			}
		}
		public IQueryable<ContactAddress> ContactAddressesTable {
			get {
				return this.ContactAddresses.EntityFilter();
			}
		}
		public IQueryable<ContactCommunication> ContactCommunicationsTable {
			get {
				return this.ContactCommunications.EntityFilter();
			}
		}
		public IQueryable<COUNTRY> COUNTRiesTable {
			get {
				return this.COUNTRies.EntityFilter();
			}
		}
		public IQueryable<Currency> CurrenciesTable {
			get {
				return this.Currencies.EntityFilter();
			}
		}
		public IQueryable<CustomField> CustomFieldsTable {
			get {
				return this.CustomFields.EntityFilter();
			}
		}
		public IQueryable<CustomFieldValue> CustomFieldValuesTable {
			get {
				return this.CustomFieldValues
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<DataType> DataTypesTable {
			get {
				return this.DataTypes.EntityFilter();
			}
		}
		public IQueryable<Deal> DealsTable {
			get {
				return this.Deals.EntityFilter();
			}
		}
		public IQueryable<DealClosing> DealClosingsTable {
			get {
				return this.DealClosings.EntityFilter();
			}
		}
		public IQueryable<DealClosingCost> DealClosingCostsTable {
			get {
				return this.DealClosingCosts.EntityFilter();
			}
		}
		public IQueryable<DealClosingCostType> DealClosingCostTypesTable {
			get {
				return this.DealClosingCostTypes.EntityFilter();
			}
		}
		public IQueryable<DealFundDocument> DealFundDocumentsTable {
			get {
				return this.DealFundDocuments.EntityFilter();
			}
		}
		public IQueryable<DealUnderlyingDirect> DealUnderlyingDirectsTable {
			get {
				return this.DealUnderlyingDirects.EntityFilter();
			}
		}
		public IQueryable<DealUnderlyingFund> DealUnderlyingFundsTable {
			get {
				return this.DealUnderlyingFunds.EntityFilter();
			}
		}
		public IQueryable<DealUnderlyingFundAdjustment> DealUnderlyingFundAdjustmentsTable {
			get {
				return this.DealUnderlyingFundAdjustments
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<DividendDistribution> DividendDistributionsTable {
			get {
				return this.DividendDistributions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<DocumentSection> DocumentSectionsTable {
			get {
				return this.DocumentSections.EntityFilter();
			}
		}
		public IQueryable<DocumentType> DocumentTypesTable {
			get {
				return this.DocumentTypes.EntityFilter();
			}
		}
		public IQueryable<ENTITY> ENTITiesTable {
			get {
				return this.ENTITies;
			}
		}
		public IQueryable<Equity> EquitiesTable {
			get {
				return this.Equities.EntityFilter();
			}
		}
		public IQueryable<EquitySplit> EquitySplitsTable {
			get {
				return this.EquitySplits
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<EquityType> EquityTypesTable {
			get {
				return this.EquityTypes.EntityFilter();
			}
		}
		public IQueryable<File> FilesTable {
			get {
				return this.Files.EntityFilter();
			}
		}
		public IQueryable<FileType> FileTypesTable {
			get {
				return this.FileTypes.EntityFilter();
			}
		}
		public IQueryable<FixedIncome> FixedIncomesTable {
			get {
				return this.FixedIncomes.EntityFilter();
			}
		}
		public IQueryable<FixedIncomeType> FixedIncomeTypesTable {
			get {
				return this.FixedIncomeTypes.EntityFilter();
			}
		}
		public IQueryable<Fund> FundsTable {
			get {
				return this.Funds.EntityFilter();
			}
		}
		public IQueryable<FundAccount> FundAccountsTable {
			get {
				return this.FundAccounts.EntityFilter();
			}
		}
		public IQueryable<FundActivityHistory> FundActivityHistoriesTable {
			get {
				return this.FundActivityHistories.EntityFilter();
			}
		}
		public IQueryable<FundClosing> FundClosingsTable {
			get {
				return this.FundClosings.EntityFilter();
			}
		}
		public IQueryable<FundExpense> FundExpensesTable {
			get {
				return this.FundExpenses
							.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
							.EntityFilter();
			}
		}
		public IQueryable<FundExpenseType> FundExpenseTypesTable {
			get {
				return this.FundExpenseTypes.EntityFilter();
			}
		}
		public IQueryable<FundRateSchedule> FundRateSchedulesTable {
			get {
				return this.FundRateSchedules
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<Geography> GeographiesTable {
			get {
				return this.Geographies.EntityFilter();
			}
		}
		public IQueryable<Industry> IndustriesTable {
			get {
				return this.Industries.EntityFilter();
			}
		}
		public IQueryable<InvestmentType> InvestmentTypesTable {
			get {
				return this.InvestmentTypes.EntityFilter();
			}
		}
		public IQueryable<Investor> InvestorsTable {
			get {
				return this.Investors.EntityFilter();
			}
		}
		public IQueryable<InvestorAccount> InvestorAccountsTable {
			get {
				return this.InvestorAccounts.EntityFilter();
			}
		}
		public IQueryable<InvestorAddress> InvestorAddressesTable {
			get {
				return this.InvestorAddresses.EntityFilter();
			}
		}
		public IQueryable<InvestorCommunication> InvestorCommunicationsTable {
			get {
				return this.InvestorCommunications.EntityFilter();
			}
		}
		public IQueryable<InvestorContact> InvestorContactsTable {
			get {
				return this.InvestorContacts.EntityFilter();
			}
		}
		public IQueryable<InvestorEntityType> InvestorEntityTypesTable {
			get {
				return this.InvestorEntityTypes.EntityFilter();
			}
		}
		public IQueryable<InvestorFund> InvestorFundsTable {
			get {
				return this.InvestorFunds
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<InvestorFundDocument> InvestorFundDocumentsTable {
			get {
				return this.InvestorFundDocuments.EntityFilter();
			}
		}
		public IQueryable<InvestorFundTransaction> InvestorFundTransactionsTable {
			get {
				return this.InvestorFundTransactions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<InvestorType> InvestorTypesTable {
			get {
				return this.InvestorTypes.EntityFilter();
			}
		}
		public IQueryable<Issuer> IssuersTable {
			get {
				return this.Issuers.EntityFilter();
			}
		}
		public IQueryable<Log> LogsTable {
			get {
				return this.Logs.EntityFilter();
			}
		}
		public IQueryable<LogDetail> LogDetailsTable {
			get {
				return this.LogDetails.EntityFilter();
			}
		}
		public IQueryable<LogType> LogTypesTable {
			get {
				return this.LogTypes.EntityFilter();
			}
		}
		public IQueryable<ManagementFeeRateSchedule> ManagementFeeRateSchedulesTable {
			get {
				return this.ManagementFeeRateSchedules.EntityFilter();
			}
		}
		public IQueryable<ManagementFeeRateScheduleTier> ManagementFeeRateScheduleTiersTable {
			get {
				return this.ManagementFeeRateScheduleTiers
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<MODULE> MODULEsTable {
			get {
				return this.MODULEs.EntityFilter();
			}
		}
		public IQueryable<MultiplierType> MultiplierTypesTable {
			get {
				return this.MultiplierTypes.EntityFilter();
			}
		}
		public IQueryable<OptionField> OptionFieldsTable {
			get {
				return this.OptionFields.EntityFilter();
			}
		}
		public IQueryable<OptionFieldValueList> OptionFieldValueListsTable {
			get {
				return this.OptionFieldValueLists.EntityFilter();
			}
		}
		public IQueryable<Partner> PartnersTable {
			get {
				return this.Partners.EntityFilter();
			}
		}
		public IQueryable<PurchaseType> PurchaseTypesTable {
			get {
				return this.PurchaseTypes.EntityFilter();
			}
		}
		public IQueryable<RateScheduleType> RateScheduleTypesTable {
			get {
				return this.RateScheduleTypes.EntityFilter();
			}
		}
		public IQueryable<ReportingFrequency> ReportingFrequenciesTable {
			get {
				return this.ReportingFrequencies.EntityFilter();
			}
		}
		public IQueryable<ReportingType> ReportingTypesTable {
			get {
				return this.ReportingTypes.EntityFilter();
			}
		}
		public IQueryable<SecurityConversion> SecurityConversionsTable {
			get {
				return this.SecurityConversions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<SecurityConversionDetail> SecurityConversionDetailsTable {
			get {
				return this.SecurityConversionDetails.EntityFilter();
			}
		}
		public IQueryable<SecurityType> SecurityTypesTable {
			get {
				return this.SecurityTypes.EntityFilter();
			}
		}
		public IQueryable<SellerType> SellerTypesTable {
			get {
				return this.SellerTypes.EntityFilter();
			}
		}
		public IQueryable<ShareClassType> ShareClassTypesTable {
			get {
				return this.ShareClassTypes.EntityFilter();
			}
		}
		public IQueryable<STATE> STATEsTable {
			get {
				return this.STATEs.EntityFilter();
			}
		}
		public IQueryable<TransactionType> TransactionTypesTable {
			get {
				return this.TransactionTypes.EntityFilter();
			}
		}
		public IQueryable<UnderlyingDirectDividendDistribution> UnderlyingDirectDividendDistributionsTable {
			get {
				return this.UnderlyingDirectDividendDistributions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingDirectDocument> UnderlyingDirectDocumentsTable {
			get {
				return this.UnderlyingDirectDocuments.EntityFilter();
			}
		}
		public IQueryable<UnderlyingDirectLastPrice> UnderlyingDirectLastPricesTable {
			get {
				return this.UnderlyingDirectLastPrices
							.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
							.EntityFilter();
			}
		}
		public IQueryable<UnderlyingDirectLastPriceHistory> UnderlyingDirectLastPriceHistoriesTable {
			get {
				return this.UnderlyingDirectLastPriceHistories
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFund> UnderlyingFundsTable {
			get {
				return this.UnderlyingFunds.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundCapitalCall> UnderlyingFundCapitalCallsTable {
			get {
				return this.UnderlyingFundCapitalCalls
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundCapitalCallLineItem> UnderlyingFundCapitalCallLineItemsTable {
			get {
				return this.UnderlyingFundCapitalCallLineItems
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundCashDistribution> UnderlyingFundCashDistributionsTable {
			get {
				return this.UnderlyingFundCashDistributions
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundContact> UnderlyingFundContactsTable {
			get {
				return this.UnderlyingFundContacts.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundDocument> UnderlyingFundDocumentsTable {
			get {
				return this.UnderlyingFundDocuments.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundNAV> UnderlyingFundNAVsTable {
			get {
				return this.UnderlyingFundNAVs
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundNAVHistory> UnderlyingFundNAVHistoriesTable {
			get {
				return this.UnderlyingFundNAVHistories
					.Join(this.USERsTable, source => source.CreatedBy, user => user.UserID, (source, user) => source)
					.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundStockDistribution> UnderlyingFundStockDistributionsTable {
			get {
				return this.UnderlyingFundStockDistributions.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundStockDistributionLineItem> UnderlyingFundStockDistributionLineItemsTable {
			get {
				return this.UnderlyingFundStockDistributionLineItems.EntityFilter();
			}
		}
		public IQueryable<UnderlyingFundType> UnderlyingFundTypesTable {
			get {
				return this.UnderlyingFundTypes.EntityFilter();
			}
		}
		public IQueryable<USER> USERsTable {
			get {
				return this.USERs.EntityFilter();
			}
		}
		public IQueryable<EntityMenu> EntityMenusTable {
			get {
				return this.EntityMenus.EntityFilter();
			}
		}
		public IQueryable<Menu> MenusTable {
			get {
				return this.Menus.EntityFilter();
			}
		}
		public IQueryable<PartnersShareForm> PartnersShareFormsTable {
			get {
				return this.PartnersShareForms.EntityFilter();
			}
		}
		#endregion
	}
}