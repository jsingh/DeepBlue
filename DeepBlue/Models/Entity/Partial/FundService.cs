using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFundService {
		void SaveFund(Fund fund);
	}

	public class FundService : IFundService {
		public void SaveFund(Fund fund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fund.FundID == 0) {
					context.Funds.AddObject(fund);
				} else {
					Fund updateFund = context.Funds.SingleOrDefault(deepblueFund => deepblueFund.FundID == fund.FundID);
					//Update fund,fund account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					foreach (var fundAccount in fund.FundAccounts) {
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("FundAccounts", fundAccount);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, fundAccount);
						} else {
							updateFund.FundAccounts.Add(new FundAccount {
								Account = fundAccount.Account,
								AccountNumberCash = fundAccount.AccountNumberCash,
								AccountOf = fundAccount.AccountOf,
								Attention = fundAccount.Attention,
								BankName = fundAccount.BankName,
								CreatedBy = fundAccount.CreatedBy,
								CreatedDate = fundAccount.CreatedDate,
								EntityID = fundAccount.EntityID,
								Fax = fundAccount.Fax,
								FFCNumber = fundAccount.FFCNumber,
								IBAN = fundAccount.IBAN,
								IsPrimary = fundAccount.IsPrimary,
								LastUpdatedBy = fundAccount.LastUpdatedBy,
								LastUpdatedDate = fundAccount.LastUpdatedDate,
								Phone = fundAccount.Phone,
								Reference = fundAccount.Reference,
								Routing = fundAccount.Routing,
								SWIFT = fundAccount.SWIFT
							});
						}
					}
					foreach (var fundClosing in fund.FundClosings) {
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("FundClosings", fundClosing);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, fundClosing);
						} else {
							updateFund.FundClosings.Add(new FundClosing {
								FundClosingDate = fundClosing.FundClosingDate,
								IsFirstClosing = fundClosing.IsFirstClosing,
								Name = fundClosing.Name
							});
						}
					}
					foreach (var fundRateSchedule in fund.FundRateSchedules) {
						originalItem = null;
						key = default(EntityKey);
						key = context.CreateEntityKey("FundRateSchedules", fundRateSchedule);
						FundRateSchedule newFundRateSchdule = null;
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, fundRateSchedule);
						} else {
							newFundRateSchdule = new FundRateSchedule {
								CreatedBy = fundRateSchedule.CreatedBy,
								CreatedDate = fundRateSchedule.CreatedDate,
								InvestorTypeID = fundRateSchedule.InvestorTypeID,
								LastUpdatedBy = fundRateSchedule.LastUpdatedBy,
								LastUpdatedDate = fundRateSchedule.LastUpdatedDate,
								RateScheduleTypeID = fundRateSchedule.RateScheduleTypeID,
								RateScheduleID = fundRateSchedule.RateScheduleID
							};
							updateFund.FundRateSchedules.Add(newFundRateSchdule);
						}
						//originalItem = null;
						//key = default(EntityKey);
						//key = context.CreateEntityKey("ManagementFeeRateSchedules", fundRateSchedule.ManagementFeeRateSchedule);
						//if (context.TryGetObjectByKey(key, out originalItem)) {
						//    context.ApplyCurrentValues(key.EntitySetName, fundRateSchedule.ManagementFeeRateSchedule);
						//} else {
						//    if (newFundRateSchdule != null) {
						//        newFundRateSchdule.ManagementFeeRateSchedule = new ManagementFeeRateSchedule {
						//            CalculationFormatString = fundRateSchedule.ManagementFeeRateSchedule.CalculationFormatString,
						//            CreatedBy = fundRateSchedule.ManagementFeeRateSchedule.CreatedBy,
						//            CreatedDate = fundRateSchedule.ManagementFeeRateSchedule.CreatedDate,
						//            Description = fundRateSchedule.ManagementFeeRateSchedule.Description,
						//            EntityID = fundRateSchedule.ManagementFeeRateSchedule.EntityID,
						//            LastUpdatedBy = fundRateSchedule.ManagementFeeRateSchedule.LastUpdatedBy,
						//            LastUpdatedDate = fundRateSchedule.ManagementFeeRateSchedule.LastUpdatedDate,
						//            Name = fundRateSchedule.ManagementFeeRateSchedule.Name,
						//            RateScheduleTypeID = fundRateSchedule.ManagementFeeRateSchedule.RateScheduleTypeID
						//        };
						//    }
						//}
						//foreach (var tier in fundRateSchedule.ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers) {
						//    originalItem = null;
						//    key = default(EntityKey);
						//    key = context.CreateEntityKey("ManagementFeeRateScheduleTiers", tier);
						//    if (context.TryGetObjectByKey(key, out originalItem)) {
						//        context.ApplyCurrentValues(key.EntitySetName, tier);
						//    } else {
						//        if (newFundRateSchdule != null) {
						//            newFundRateSchdule.ManagementFeeRateSchedule.ManagementFeeRateScheduleTiers.Add(new ManagementFeeRateScheduleTier {
						//                CreatedBy = tier.CreatedBy,
						//                CreatedDate = tier.CreatedDate,
						//                EndDate = tier.EndDate,
						//                LastUpdatedBy = tier.LastUpdatedBy,
						//                LastUpdatedDate = tier.LastUpdatedDate,
						//                Multiplier = tier.Multiplier,
						//                MultiplierTypeID = tier.MultiplierTypeID,
						//                StartDate = tier.StartDate,
						//                Notes = tier.Notes
						//            });
						//        }
						//    }
						//}
					}
					originalItem = null;
					key = default(EntityKey);
					key = context.CreateEntityKey("Funds", fund);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, fund);
					}
				}
				context.SaveChanges();
			}
		}
	}
}