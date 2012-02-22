using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class EntityPermission {

		private string _tableName = string.Empty;

		public string URL { get; set; }

		public Table TableName {
			get {
				return (Table)Enum.Parse(typeof(Table),this._tableName);
			}
			set {
				this._tableName = value.ToString();
			}
		}

		public bool IsSystemEntity { get; set; }

		public bool IsOtherEntity { get; set; }
	}


	public enum Table {
		NULL,
		Account,
		Address,
		AddressType,
		CapitalCallLineItemType,
		CapitalCallType,
		CashDistributionType,
		Communication,
		CommunicationGrouping,
		CommunicationType,
		Contact,
		ContactAddress,
		ContactCommunication,
		Currency,
		CustomField,
		DataType,
		Deal,
		DealClosingCostType,
		DealFundDocument,
		DocumentType,
		ENTITY,
		Equity,
		EquityType,
		File,
		FileType,
		FixedIncome,
		FixedIncomeType,
		Fund,
		FundAccount,
		FundClosing,
		FundExpenseType,
		Geography,
		Industry,
		InvestmentType,
		Investor,
		InvestorAccount,
		InvestorAddress,
		InvestorCommunication,
		InvestorContact,
		InvestorEntityType,
		InvestorFundDocument,
		InvestorType,
		Issuer,
		Log,
		ManagementFeeRateSchedule,
		OptionField,
		Partner,
		PurchaseType,
		RateScheduleType,
		ReportingFrequency,
		ReportingType,
		SellerType,
		ShareClassType,
		TransactionType,
		UnderlyingDirectDocument,
		UnderlyingFund,
		UnderlyingFundDocument,
		UnderlyingFundType,
		USER,
		EntityMenu,
		Menu
	}
}