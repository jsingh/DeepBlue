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
			item.Value = "";
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
			item.Value = "";
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
			item.Value = "";
			addressTypeList.Add(item);
			foreach (var addressType in addressTypes) {
				item = new SelectListItem();
				item.Text = addressType.AddressTypeName.ToString();
				item.Value = addressType.AddressTypeID.ToString();
				addressTypeList.Add(item);
			}
			return addressTypeList;
		}

		public static List<SelectListItem> GetMemberEntityTypesSelectList(List<MemberEntityType> memberEntityTypes) {
			List<SelectListItem> memberEntityTypeList = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "--Select One--";
			item.Value = "";
			memberEntityTypeList.Add(item);
			foreach (var memberEntityType in memberEntityTypes) {
				item = new SelectListItem();
				item.Text = memberEntityType.MemberEntityTypeName.ToString();
				item.Value = memberEntityType.MemberEntityTypeID.ToString();
				memberEntityTypeList.Add(item);
			}
			return memberEntityTypeList;
		}

		public static List<SelectListItem> GetDomesticForeignList() {
			List<SelectListItem> domasticForeigns = new List<SelectListItem>();
			SelectListItem item = new SelectListItem();
			item.Text = "Yes";
			item.Value = "True";
			domasticForeigns.Add(item);
			item = new SelectListItem();
			item.Text = "No";
			item.Value  = "False";
			domasticForeigns.Add(item);
			return domasticForeigns;
		}

	}
}