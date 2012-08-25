using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Controllers.Accounting;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Reflection;

namespace DeepBlue.Controllers.Accounting {
	public class AccountingManager : IAccounting {

		public IAccountingRepository AccountingRepository { get; set; }

		public AccountingManager()
			: this(new AccountingRepository()) {
		}


		public AccountingManager(IAccountingRepository accountingRepository) {
			AccountingRepository = accountingRepository;
		}

		public void CreateAccountingEntry(DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, int entityID, IAccountable accountableItem, decimal? amount = null, int? accountingTransactionSubTypeID = null) {
			DeepBlueEntities context = new DeepBlueEntities();
			decimal amt = amount.HasValue ? amount.Value : (accountableItem.Amount.HasValue ? accountableItem.Amount.Value : 0);
			var query = from aet in context.AccountingEntryTemplates
						where aet.EntityID == entityID && aet.AccountingTransactionTypeID == (int)accountingTransactionType
						select aet;
			// See if there are any templates specific to the Fund
			List<AccountingEntryTemplate> templates = query.Where(x => x.FundID == fundID).ToList();
			if (templates.Count <= 0) {
				// No templates found for this fund.. try to see if there is an Entity level template available
				templates = query.Where(x => x.FundID == null).ToList();
			}

			if (templates.Count > 0) {
				// Filter on the sub type
				if (accountingTransactionSubTypeID.HasValue) {
					//var templatesWithSubType = templates.Where(x => x.AccountingTransactionSubTypeID == accountingTransactionSubTypeID.Value).ToList();
					//if (templatesWithSubType.Count > 0) {
					//    templates = templatesWithSubType;
					//}
				}

				List<AccountingEntry> accountingEntries = new List<AccountingEntry>();
				foreach (AccountingEntryTemplate template in templates) {
					// each template will result in an accounting entry
					AccountingEntry entry = new AccountingEntry();
					entry.AccountingEntryTemplateID = template.AccountingEntryTemplateID;
					DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType amountType = (DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType)template.AccountingEntryAmountTypeID;
					switch (amountType) {
						case DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType.FixedAmount:
							entry.Amount = Convert.ToDecimal(template.AccountingEntryAmountTypeData);
							break;
						case DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType.Percentage:
							decimal percent = Convert.ToDecimal(template.AccountingEntryAmountTypeData);
							entry.Amount = (percent * amt) / 100;
							break;
						case DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType.Field:
							// Use reflection to get the amount
							PropertyInfo property = accountableItem.GetType().GetProperties().Where(x => x.Name == template.AccountingEntryAmountTypeData).FirstOrDefault();
							object val = property.GetValue(accountableItem, null);
							if (val != null) {
								entry.Amount = Convert.ToDecimal(val);
							}
							break;
						case DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType.Custom:
							break;
						default:
							entry.Amount = amt;
							break;
					}

					entry.TraceID = accountableItem.TraceID;
					entry.AttributedTo = accountableItem.AttributedTo;
					entry.AttributedToName = accountableItem.AttributedToName;
					entry.AttributedToType = accountableItem.AttributedToType;
					entry.FundID = fundID;
					entry.EntityID = entityID;
					context.AccountingEntries.AddObject(entry);
					context.SaveChanges();
				}
			}
		}

		//private void CreateAccountEntry(AccountingEntryTemplate accountingEntryTemplate, DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, decimal? amount, object record) {
		//    if (accountingEntryTemplate != null) {
		//        DeepBlue.Models.Accounting.Enums.AccountingEntryAmountType accountingEntryAmountType = (Models.Accounting.Enums.AccountingEntryAmountType)accountingEntryTemplate.AccountingEntryAmountTypeID;
		//        decimal calculateAmount = 0;
		//        int? attributedTo = null;
		//        string attributedToName = string.Empty;
		//        string attributedToType = string.Empty;
		//        int? traceID = null;
		//        switch (accountingEntryAmountType) {
		//            case Models.Accounting.Enums.AccountingEntryAmountType.Amount:
		//                calculateAmount = (amount ?? 0);
		//                break;
		//            case Models.Accounting.Enums.AccountingEntryAmountType.Custom:
		//                break;
		//            case Models.Accounting.Enums.AccountingEntryAmountType.Field:
		//                switch (accountingTransactionType) {
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalCallLineItem:
		//                        if (record.GetType().Equals(typeof(CapitalCallLineItem))) {
		//                            CapitalCallLineItem item = (CapitalCallLineItem)record;
		//                            calculateAmount = item.CapitalAmountCalled;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalCallReconcilationLineItem:
		//                        if (record.GetType().Equals(typeof(CapitalCallLineItem))) {
		//                            CapitalCallLineItem item = (CapitalCallLineItem)record;
		//                            calculateAmount = item.CapitalAmountCalled;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CapitalDistributionLineItem:
		//                        if (record.GetType().Equals(typeof(CapitalDistributionLineItem))) {
		//                            CapitalDistributionLineItem item = (CapitalDistributionLineItem)record;
		//                            calculateAmount = item.DistributionAmount;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.CashDistribution:
		//                        if (record.GetType().Equals(typeof(CashDistribution))) {
		//                            CashDistribution item = (CashDistribution)record;
		//                            calculateAmount = item.Amount;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.DealClosingCost:
		//                        if (record.GetType().Equals(typeof(DealClosingCost))) {
		//                            DealClosingCost item = (DealClosingCost)record;
		//                            calculateAmount = item.Amount;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.DealUnderlyingFund:
		//                        if (record.GetType().Equals(typeof(DealUnderlyingFund))) {
		//                            DealUnderlyingFund item = (DealUnderlyingFund)record;
		//                            calculateAmount = (item.CommittedAmount ?? 0);
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.FundExpense:
		//                        if (record.GetType().Equals(typeof(FundExpense))) {
		//                            FundExpense item = (FundExpense)record;
		//                            calculateAmount = item.Amount;
		//                        }
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.SecuritySaleLineItem:
		//                        break;
		//                    case DeepBlue.Models.Accounting.Enums.AccountingTransactionType.UnderlyingFundStockDistributionLineItem:
		//                        if (record.GetType().Equals(typeof(UnderlyingFundStockDistributionLineItem))) {
		//                            UnderlyingFundStockDistributionLineItem item = (UnderlyingFundStockDistributionLineItem)record;
		//                            calculateAmount = (item.FMV ?? 0);
		//                        }
		//                        break;
		//                }
		//                break;
		//            case Models.Accounting.Enums.AccountingEntryAmountType.FixedAmount:
		//                calculateAmount = (accountingEntryTemplate.Amount ?? 0);
		//                break;
		//            case Models.Accounting.Enums.AccountingEntryAmountType.Percentage:
		//                decimal percentage = 0;
		//                decimal.TryParse(accountingEntryTemplate.AccountingEntryAmountTypeData.Replace("%", ""), out percentage);
		//                calculateAmount = decimal.Divide(
		//                                    (
		//                                    decimal.Multiply(percentage, (amount ?? 0))
		//                                    ), 100);
		//                break;
		//        }
		//        AccountingEntry accountingEntry = new AccountingEntry {
		//            AccountingEntryTemplateID = accountingEntryTemplate.AccountingEntryTemplateID,
		//            Amount = calculateAmount,
		//            AttributedTo = attributedTo,
		//            AttributedToName = attributedToName,
		//            AttributedToType = attributedToType,
		//            CreatedBy = Authentication.CurrentUser.UserID,
		//            CreatedDate = DateTime.Now,
		//            EntityID = Authentication.CurrentEntity.EntityID,
		//            LastUpdatedBy = Authentication.CurrentUser.UserID,
		//            LastUpdatedDate = DateTime.Now,
		//            FundID = fundID,
		//            TraceID = traceID,
		//        };
		//        AccountingRepository.SaveAccountingEntry(accountingEntry);
		//    }
		//}

		//public void CreateEntry(DeepBlue.Models.Accounting.Enums.AccountingTransactionType accountingTransactionType, int fundID, decimal? amount, object record) {
		//    AccountingEntryTemplate isCreditAccountingEntryTemplate = AccountingRepository.FindAccountingEntryTemplate(fundID, (int)accountingTransactionType, true);
		//    AccountingEntryTemplate isDebitAccountingEntryTemplate = AccountingRepository.FindAccountingEntryTemplate(fundID, (int)accountingTransactionType, false);
		//    CreateAccountEntry(isCreditAccountingEntryTemplate,accountingTransactionType, fundID, amount, record);
		//    CreateAccountEntry(isDebitAccountingEntryTemplate, accountingTransactionType, fundID, amount, record);
		//}
	}
}