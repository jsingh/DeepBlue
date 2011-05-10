﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Document;
using DeepBlue.Models.Issuer;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Helpers {
	public class SelectListFactory {
		
		public static List<SelectListItem> GetStateSelectList(List<STATE> states) {
			List<SelectListItem> stateList = new List<SelectListItem>();
			stateList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			stateList.Add(item);
			foreach (var state in states) {
				item = new SelectListItem();
				item.Text = state.Name.ToString();
				item.Value = state.StateID.ToString();
				stateList.Add(item);
			}
			return stateList;
		}

		public static List<SelectListItem> GetCountrySelectList(List<COUNTRY> countries) {
			List<SelectListItem> countryList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			countryList.Add(item);
			foreach (var country in countries) {
				item = new SelectListItem();
				item.Text = country.CountryName.ToString();
				item.Value = country.CountryID.ToString();
				countryList.Add(item);
			}
			return countryList;
		}

		public static List<SelectListItem> GetAddressTypeSelectList(List<AddressType> addressTypes) {
			List<SelectListItem> addressTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			addressTypeList.Add(item);
			foreach (var addressType in addressTypes) {
				item = new SelectListItem();
				item.Text = addressType.AddressTypeName.ToString();
				item.Value = addressType.AddressTypeID.ToString();
				addressTypeList.Add(item);
			}
			return addressTypeList;
		}

		public static List<SelectListItem> GetInvestorEntityTypesSelectList(List<InvestorEntityType> investorEntityTypes) {
			List<SelectListItem> investorEntityTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			investorEntityTypeList.Add(item);
			foreach (var investorEntityType in investorEntityTypes) {
				item = new SelectListItem();
				item.Text = investorEntityType.InvestorEntityTypeName.ToString();
				item.Value = investorEntityType.InvestorEntityTypeID.ToString();
				investorEntityTypeList.Add(item);
			}
			return investorEntityTypeList;
		}

		public static List<SelectListItem> GetDomesticForeignList() {
			List<SelectListItem> domasticForeigns = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "Domestic";
			item.Value = "true";
			domasticForeigns.Add(item);
			item = new SelectListItem();
			item.Text = "Foreign";
			item.Value = "false";
			domasticForeigns.Add(item);
			return domasticForeigns;
		}

		public static List<SelectListItem> GetSourceList() {
			List<SelectListItem> domasticForeigns = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			domasticForeigns.Add(item);
			item = new SelectListItem();
			item.Text = "FEG";
			item.Value = "1";
			domasticForeigns.Add(item);
			item = new SelectListItem();
			item.Text = "Cambridge";
			item.Value = "2";
			domasticForeigns.Add(item);
			item = new SelectListItem();
			item.Text = "NEPC";
			item.Value = "3";
			domasticForeigns.Add(item);
			return domasticForeigns;
		}

		public static List<SelectListItem> GetFundSelectList(List<Fund> funds) {
			List<SelectListItem> fundList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			fundList.Add(item);
			foreach (var fund in funds) {
				item = new SelectListItem();
				item.Text = fund.FundName.ToString();
				item.Value = fund.FundID.ToString();
				fundList.Add(item);
			}
			return fundList;
		}

		public static List<SelectListItem> GetInvestorTypeSelectList(List<InvestorType> investorTypes) {
			List<SelectListItem> investorTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			investorTypeList.Add(item);
			foreach (var investorType in investorTypes) {
				item = new SelectListItem();
				item.Text = investorType.InvestorTypeName.ToString();
				item.Value = investorType.InvestorTypeID.ToString();
				investorTypeList.Add(item);
			}
			return investorTypeList;
		}

		public static List<SelectListItem> GetDefaultSelectList() {
			List<SelectListItem> defaultSelectList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			defaultSelectList.Add(item);
			return defaultSelectList;
		}

		public static List<SelectListItem> GetDocumentTypeSelectList(List<DocumentType> documentTypes) {
			List<SelectListItem> documentTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			documentTypeList.Add(item);
			foreach (var documentType in documentTypes) {
				item = new SelectListItem();
				item.Text = documentType.DocumentTypeName.ToString();
				item.Value = documentType.DocumentTypeID.ToString();
				documentTypeList.Add(item);
			}
			return documentTypeList;
		}

		public static List<SelectListItem> GetDocumentStatusList() {
			List<SelectListItem> documentStatusList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = DocumentStatus.Investor.ToString();
			item.Value = ((int)DocumentStatus.Investor).ToString();
			documentStatusList.Add(item);
			item = new SelectListItem();
			item.Text = DocumentStatus.Fund.ToString();
			item.Value = ((int)DocumentStatus.Fund).ToString();
			documentStatusList.Add(item);
			return documentStatusList;
		}

		public static List<SelectListItem> GetModuleSelectList(List<MODULE> modules) {
			List<SelectListItem> moduleList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			moduleList.Add(item);
			foreach (var module in modules) {
				item = new SelectListItem();
				item.Text = module.ModuleName;
				item.Value = module.ModuleID.ToString();
				moduleList.Add(item);
			}
			return moduleList;
		}

		public static List<SelectListItem> GetDataTypeSelectList(List<DataType> dataTypes) {
			List<SelectListItem> dataTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			dataTypeList.Add(item);
			foreach (var dataType in dataTypes) {
				item = new SelectListItem();
				item.Text = dataType.DataTypeName;
				item.Value = dataType.DataTypeID.ToString();
				dataTypeList.Add(item);
			}
			return dataTypeList;
		}

		public static List<SelectListItem> GetUploadTypeSelectList() {
			List<SelectListItem> uploadTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = UploadType.Upload.ToString();
			item.Value = ((int)UploadType.Upload).ToString();
			uploadTypeList.Add(item);
			item = new SelectListItem();
			item.Text = UploadType.Link.ToString();
			item.Value = ((int)UploadType.Link).ToString();
			uploadTypeList.Add(item);
			return uploadTypeList;
		}

		public static List<SelectListItem> GetMultiplierTypeList(List<MultiplierType> multiplierTypes) {
			List<SelectListItem> multiplierTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			multiplierTypeList.Add(item);
			foreach (var type in multiplierTypes) {
				item = new SelectListItem();
				item.Text = type.Name;
				item.Value = type.MultiplierTypeID.ToString();
				multiplierTypeList.Add(item);
			}
			return multiplierTypeList;
		}

		public static List<SelectListItem> GetCommunicationGroupingSelectList(List<CommunicationGrouping> communicationGroupings) {
			List<SelectListItem> communicationGroupingList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			communicationGroupingList.Add(item);
			foreach (var communicationGrouping in communicationGroupings) {
				item = new SelectListItem();
				item.Text = communicationGrouping.CommunicationGroupingName.ToString();
				item.Value = communicationGrouping.CommunicationGroupingID.ToString();
				communicationGroupingList.Add(item);
			}
			return communicationGroupingList;
		}

		public static List<SelectListItem> GetPurchaseTypeSelectList(List<PurchaseType> purchaseTypes) {
			List<SelectListItem> purchaseTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			purchaseTypeList.Add(item);
			foreach (var purchaseType in purchaseTypes) {
				item = new SelectListItem();
				item.Text = purchaseType.Name.ToString();
				item.Value = purchaseType.PurchaseTypeID.ToString();
				purchaseTypeList.Add(item);
			}
			return purchaseTypeList;
		}

		public static List<SelectListItem> GetDealClosingCostTypeSelectList(List<DealClosingCostType> dealClosingCostTypes) {
			List<SelectListItem> dealClosingCostTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			dealClosingCostTypeList.Add(item);
			foreach (var dealClosingCostType in dealClosingCostTypes) {
				item = new SelectListItem();
				item.Text = dealClosingCostType.Name.ToString();
				item.Value = dealClosingCostType.DealClosingCostTypeID.ToString();
				dealClosingCostTypeList.Add(item);
			}
			return dealClosingCostTypeList;
		}

		public static List<SelectListItem> GetUnderlyingFundSelectList(List<UnderlyingFund> underlyingFunds) {
			List<SelectListItem> underlyingFundList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			underlyingFundList.Add(item);
			foreach (var underlyingFund in underlyingFunds) {
				item = new SelectListItem();
				item.Text = underlyingFund.FundName.ToString();
				item.Value = underlyingFund.UnderlyingtFundID.ToString();
				underlyingFundList.Add(item);
			}
			return underlyingFundList;
		}

		public static List<SelectListItem> GetIssuerSelectList(List<IssuerDetailModel> issuers) {
			List<SelectListItem> issuerList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			issuerList.Add(item);
			foreach (var issuer in issuers) {
				item = new SelectListItem();
				item.Text = issuer.Name.ToString();
				item.Value = issuer.IssuerId.ToString();
				issuerList.Add(item);
			}
			return issuerList;
		}

		public static List<SelectListItem> GetSecurityTypeSelectList(List<SecurityType> securityTypes) {
			List<SelectListItem> securityTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			securityTypeList.Add(item);
			foreach (var securityType in securityTypes) {
				item = new SelectListItem();
				item.Text = securityType.Name.ToString();
				item.Value = securityType.SecurityTypeID.ToString();
				securityTypeList.Add(item);
			}
			return securityTypeList;
		}

		public static List<SelectListItem> GetEquitySelectList(List<Equity> equities) {
			List<SelectListItem> equityList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			equityList.Add(item);
			foreach (var equity in equities) {
				item = new SelectListItem();
				item.Text = equity.Symbol.ToString();
				item.Value = equity.EquityID.ToString();
				equityList.Add(item);
			}
			return equityList;
		}

		public static List<SelectListItem> GetEquityTypeSelectList(List<EquityType> equityTypes) {
			List<SelectListItem> equityTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			equityTypeList.Add(item);
			foreach (var equityType in equityTypes) {
				item = new SelectListItem();
				item.Text = equityType.Equity.ToString();
				item.Value = equityType.EquityTypeID.ToString();
				equityTypeList.Add(item);
			}
			return equityTypeList;
		}

		public static List<SelectListItem> GetFixedIncomeSelectList(List<FixedIncome> fixedIncomes) {
			List<SelectListItem> fixedIncomeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			fixedIncomeList.Add(item);
			foreach (var fixedIncome in fixedIncomes) {
				item = new SelectListItem();
				item.Text = fixedIncome.Symbol.ToString();
				item.Value = fixedIncome.FixedIncomeID.ToString();
				fixedIncomeList.Add(item);
			}
			return fixedIncomeList;
		}

		public static List<SelectListItem> GetFixedIncomeTypesSelectList(List<FixedIncomeType> fixedIncomeTypes) {
			List<SelectListItem> fixedIncomeTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			fixedIncomeTypeList.Add(item);
			foreach (var fixedIncomeType in fixedIncomeTypes) {
				item = new SelectListItem();
				item.Text = fixedIncomeType.FixedIncomeType1.ToString();
				item.Value = fixedIncomeType.FixedIncomeTypeID.ToString();
				fixedIncomeTypeList.Add(item);
			}
			return fixedIncomeTypeList;
		}
		
		public static List<SelectListItem> GetUnderlyingFundTypeSelectList(List<UnderlyingFundType> underlyingFundTypes) {
			List<SelectListItem> underlyingFundTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			underlyingFundTypeList.Add(item);
			foreach (var underlyingFundType in underlyingFundTypes) {
				item = new SelectListItem();
				item.Text = underlyingFundType.Name.ToString();
				item.Value = underlyingFundType.UnderlyingFundTypeID.ToString();
				underlyingFundTypeList.Add(item);
			}
			return underlyingFundTypeList;
		}

		public static List<SelectListItem> GetReportingTypeSelectList(List<ReportingType> reportingTypes) {
			List<SelectListItem> reportingTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			reportingTypeList.Add(item);
			foreach (var reportingType in reportingTypes) {
				item = new SelectListItem();
				item.Text = reportingType.Reporting.ToString();
				item.Value = reportingType.ReportingTypeID.ToString();
				reportingTypeList.Add(item);
			}
			return reportingTypeList;
		}

		public static List<SelectListItem> GetReportingFrequencySelectList(List<ReportingFrequency> reportingFrequencys) {
			List<SelectListItem> reportingFrequencyList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			reportingFrequencyList.Add(item);
			foreach (var reportingFrequency in reportingFrequencys) {
				item = new SelectListItem();
				item.Text = reportingFrequency.ReportingFrequency1.ToString();
				item.Value = reportingFrequency.ReportingFrequencyID.ToString();
				reportingFrequencyList.Add(item);
			}
			return reportingFrequencyList;
		}

		public static List<SelectListItem> GetIndustrySelectList(List<Industry> industrys) {
			List<SelectListItem> industryList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			industryList.Add(item);
			foreach (var industry in industrys) {
				item = new SelectListItem();
				item.Text = industry.Industry1.ToString();
				item.Value = industry.IndustryID.ToString();
				industryList.Add(item);
			}
			return industryList;
		}

		public static List<SelectListItem> GetGeographySelectList(List<Geography> geographys) {
			List<SelectListItem> geographyList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			geographyList.Add(item);
			foreach (var geography in geographys) {
				item = new SelectListItem();
				item.Text = geography.Geography1.ToString();
				item.Value = geography.GeographyID.ToString();
				geographyList.Add(item);
			}
			return geographyList;
		}

		public static List<SelectListItem> GetCurrencySelectList(List<Currency> currencies) {
			List<SelectListItem> currencyList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			currencyList.Add(item);
			foreach (var currency in currencies) {
				item = new SelectListItem();
				item.Text = currency.Currency1.ToString();
				item.Value = currency.CurrencyID.ToString();
				currencyList.Add(item);
			}
			return currencyList;
		}

		public static List<SelectListItem> GetShareClassTypeSelectList(List<ShareClassType> shareClassTypes) {
			List<SelectListItem> shareClassTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			shareClassTypeList.Add(item);
			foreach (var shareClassType in shareClassTypes) {
				item = new SelectListItem();
				item.Text = shareClassType.ShareClass.ToString();
				item.Value = shareClassType.ShareClassTypeID.ToString();
				shareClassTypeList.Add(item);
			}
			return shareClassTypeList;
		}

		public static List<SelectListItem> GetEmptySelectList() {
			List<SelectListItem> lists = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			lists.Add(item);
			return lists;
		}

	}
}