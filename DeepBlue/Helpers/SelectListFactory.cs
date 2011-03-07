using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models;
using DeepBlue.Models.Entity;

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

		public static List<SelectListItem> GetDefaultSelectList(){
			List<SelectListItem> defaultSelectList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "0";
			defaultSelectList.Add(item);
			return defaultSelectList;
		}

	}
}