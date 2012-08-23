using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Controllers.Accounting;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.Accounting {
	public class AccountingManager : IAccounting {

		public IAccountingRepository AccountingRepository { get; set; }

		public AccountingManager()
			: this(new AccountingRepository()) {
		}


		public AccountingManager(IAccountingRepository accountingRepository) {
			AccountingRepository = accountingRepository;
		}

		private void CreateAccountEntry(AccountingEntryTemplate accountingEntryTemplate, DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, decimal? amount, object record) {
			if (accountingEntryTemplate != null) {
				DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType accountingEntryAmountType = (Models.Accounting.Enums.AccountingEntryAmountType)accountingEntryTemplate.AccountingEntryAmountTypeID;
				decimal calculateAmount = 0;
				int? attributedTo = null;
				string attributedToName = string.Empty;
				string attributedToType = string.Empty;
				int? traceID = null;
				switch (accountingEntryAmountType) {
					case Models.Accounting.Enums.AccountingEntryAmountType.Amount:
						calculateAmount = (amount ?? 0);
						break;
					case Models.Accounting.Enums.AccountingEntryAmountType.Custom:
						break;
					case Models.Accounting.Enums.AccountingEntryAmountType.Field:
						switch (accountingTransactionType) {
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalCallLineItem:
								if (record.GetType().Equals(typeof(CapitalCallLineItem))) {
									CapitalCallLineItem item = (CapitalCallLineItem)record;
									calculateAmount = item.CapitalAmountCalled;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalCallReconcilationLineItem:
								if (record.GetType().Equals(typeof(CapitalCallLineItem))) {
									CapitalCallLineItem item = (CapitalCallLineItem)record;
									calculateAmount = item.CapitalAmountCalled;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalDistributionLineItem:
								if (record.GetType().Equals(typeof(CapitalDistributionLineItem))) {
									CapitalDistributionLineItem item = (CapitalDistributionLineItem)record;
									calculateAmount = item.DistributionAmount;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CashDistribution:
								if (record.GetType().Equals(typeof(CashDistribution))) {
									CashDistribution item = (CashDistribution)record;
									calculateAmount = item.Amount;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.DealClosingCost:
								if (record.GetType().Equals(typeof(DealClosingCost))) {
									DealClosingCost item = (DealClosingCost)record;
									calculateAmount = item.Amount;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.DealUnderlyingFund:
								if (record.GetType().Equals(typeof(DealUnderlyingFund))) {
									DealUnderlyingFund item = (DealUnderlyingFund)record;
									calculateAmount = (item.CommittedAmount ?? 0);
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.FundExpense:
								if (record.GetType().Equals(typeof(FundExpense))) {
									FundExpense item = (FundExpense)record;
									calculateAmount = item.Amount;
								}
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.SecuritySaleLineItem:
								break;
							case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.UnderlyingFundStockDistributionLineItem:
								if (record.GetType().Equals(typeof(UnderlyingFundStockDistributionLineItem))) {
									UnderlyingFundStockDistributionLineItem item = (UnderlyingFundStockDistributionLineItem)record;
									calculateAmount = (item.FMV ?? 0);
								}
								break;
						}
						break;
					case Models.Accounting.Enums.AccountingEntryAmountType.FixedAmount:
						calculateAmount = (accountingEntryTemplate.Amount ?? 0);
						break;
					case Models.Accounting.Enums.AccountingEntryAmountType.Percentage:
						decimal percentage = 0;
						decimal.TryParse(accountingEntryTemplate.AccountingEntryAmountTypeData.Replace("%", ""), out percentage);
						calculateAmount = decimal.Divide(
											(
											decimal.Multiply(percentage, (amount ?? 0))
											), 100);
						break;
				}
				AccountingEntry accountingEntry = new AccountingEntry {
					AccountingEntryTemplateID = accountingEntryTemplate.AccountingEntryTemplateID,
					Amount = calculateAmount,
					AttributedTo = attributedTo,
					AttributedToName = attributedToName,
					AttributedToType = attributedToType,
					CreatedBy = Authentication.CurrentUser.UserID,
					CreatedDate = DateTime.Now,
					EntityID = Authentication.CurrentEntity.EntityID,
					LastUpdatedBy = Authentication.CurrentUser.UserID,
					LastUpdatedDate = DateTime.Now,
					FundID = fundID,
					TraceID = traceID,
				};
				AccountingRepository.SaveAccountingEntry(accountingEntry);
			}
		}

		public void CreateEntry(DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, decimal? amount, object record) {
			AccountingEntryTemplate isCreditAccountingEntryTemplate = AccountingRepository.FindAccountingEntryTemplate(fundID, (int)accountingTransactionType, true);
			AccountingEntryTemplate isDebitAccountingEntryTemplate = AccountingRepository.FindAccountingEntryTemplate(fundID, (int)accountingTransactionType, false);
			CreateAccountEntry(isCreditAccountingEntryTemplate,accountingTransactionType, fundID, amount, record);
			CreateAccountEntry(isDebitAccountingEntryTemplate, accountingTransactionType, fundID, amount, record);
		}
	}
}