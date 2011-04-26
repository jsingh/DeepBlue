﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Document;

namespace DeepBlue.Helpers {
	public class SelectListFactory {
		public static List<SelectListItem> GetStateSelectList(List<STATE> states) {
			List<SelectListItem> stateList = new List<SelectListItem>();
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