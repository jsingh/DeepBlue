using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models;
using DeepBlue.Helpers;
using DeepBlue.Models.Investor;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Investor.Enums;
using System.Data;
using DeepBlue.Controllers.Admin;
using DeepBlue.Models.Admin.Enums;
using System.Text;

namespace DeepBlue.Controllers.Investor {

	public class InvestorController : Controller {

		public IInvestorRepository InvestorRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public InvestorController()
			: this(new InvestorRepository(), new AdminRepository()) {
		}

		public InvestorController(IInvestorRepository investorRepository, IAdminRepository adminRepository) {
			InvestorRepository = investorRepository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Investor/

		public ActionResult Index() {
			return View();
		}

		public ActionResult ThankYou() {
			ViewData["MenuName"] = "Investor";
			return View();
		}

		//
		// GET: /Investor/Details/5

		public ActionResult Details(int id) {
			return View();
		}

		//
		// GET: /Investor/New

		public ActionResult New() {
			ViewData["MenuName"] = "Investor";
			CreateModel model = new CreateModel();
			model.SelectList.States = SelectListFactory.GetStateSelectList(AdminRepository.GetAllStates());
			model.SelectList.Countries = SelectListFactory.GetCountrySelectList(AdminRepository.GetAllCountries());
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(AdminRepository.GetAllAddressTypes());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.SelectList.Source = SelectListFactory.GetSourceList();
			model.CustomField = new CustomFieldModel();
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			model.DomesticForeign = true;
			model.AccountLength = 1;
			model.ContactLength = 1;
			model.Country = (int)DeepBlue.Models.Investor.Enums.DefaultCountry.USA;
			return View(model);
		}

		//
		// POST: /Investor/Create

		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				DeepBlue.Models.Entity.Investor investor = new DeepBlue.Models.Entity.Investor();
				/*Investor*/
				investor.Alias = model.Alias;
				investor.CreatedBy = AppSettings.CreatedByUserId;
				investor.CreatedDate = DateTime.Now;
				investor.EntityID = (int)ConfigUtil.CurrentEntityID;
				investor.FirstName = string.Empty;
				investor.IsDomestic = model.DomesticForeign;
				investor.LastName = "n/a";
				investor.LastUpdatedBy = AppSettings.CreatedByUserId;
				investor.LastUpdatedDate = DateTime.Now;
				investor.ManagerName = string.Empty;
				investor.InvestorEntityTypeID = model.EntityType;
				investor.InvestorName = model.InvestorName;
				investor.FirstName = model.Alias;
				investor.MiddleName = string.Empty;
				investor.Notes = string.Empty;
				investor.PrevInvestorID = 0;
				investor.ResidencyState = model.StateOfResidency;
				investor.Social = model.SocialSecurityTaxId ?? "";
				investor.TaxExempt = false;
				investor.CreatedBy = AppSettings.CreatedByUserId;
				investor.TaxID = 0;
				investor.Notes = model.Notes;

				if (model.Country > 0 && model.State > 0) {
					/* Investor Address */
					InvestorAddress investorAddress = new InvestorAddress();
					investorAddress.CreatedBy = AppSettings.CreatedByUserId;
					investorAddress.CreatedDate = DateTime.Now;
					investorAddress.EntityID = (int)ConfigUtil.CurrentEntityID;
					investorAddress.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorAddress.LastUpdatedDate = DateTime.Now;

					investorAddress.Address = new Address();
					investorAddress.Address.Address1 = model.Address1 ?? "";
					investorAddress.Address.Address2 = model.Address2 ?? "";
					investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
					investorAddress.Address.City = model.City ?? "";
					investorAddress.Address.Country = model.Country;
					investorAddress.Address.CreatedDate = DateTime.Now;
					investorAddress.Address.Country = model.Country;
					investorAddress.Address.CreatedBy = AppSettings.CreatedByUserId;
					investorAddress.Address.CreatedDate = DateTime.Now;
					investorAddress.Address.EntityID = (int)ConfigUtil.CurrentEntityID;
					investorAddress.Address.IsPreferred = false;
					investorAddress.Address.LastUpdatedDate = DateTime.Now;
					investorAddress.Address.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorAddress.Address.LastUpdatedDate = DateTime.Now;
					investorAddress.Address.Listed = false;
					investorAddress.Address.PostalCode = model.Zip;
					investorAddress.Address.State = model.State;
					investorAddress.Address.StProvince = string.Empty;
					/* Add New Investor Address */
					investor.InvestorAddresses.Add(investorAddress);
				}

				/* Bank Account */
				InvestorAccount investorAccount;
				for (int index = 0; index < model.AccountLength; index++) {
					if (string.IsNullOrEmpty(collection[(index + 1).ToString() + "_" + "AccountNumber"]) == false) {
						investorAccount = new InvestorAccount();
						investorAccount.Account = collection[(index + 1).ToString() + "_" + "AccountNumber"];
						investorAccount.Attention = collection[(index + 1).ToString() + "_" + "Attention"];
						investorAccount.Comments = string.Empty;
						investorAccount.CreatedBy = AppSettings.CreatedByUserId;
						investorAccount.CreatedDate = DateTime.Now;
						investorAccount.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorAccount.IsPrimary = false;
						investorAccount.LastUpdatedBy = AppSettings.CreatedByUserId;
						investorAccount.LastUpdatedDate = DateTime.Now;
						investorAccount.Routing = 0;
						investorAccount.Reference = collection[(index + 1).ToString() + "_" + "Reference"];
						investor.InvestorAccounts.Add(investorAccount);
					}
				}

				/* Contact Address */
				InvestorContact investorContact;
				ContactAddress contactAddress;
				for (int index = 0; index < model.ContactLength; index++) {
					if (Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactState"]) > 0 &&
						Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]) > 0) {
						investorContact = new InvestorContact();
						investorContact.CreatedBy = AppSettings.CreatedByUserId;
						investorContact.CreatedDate = DateTime.Now;
						investorContact.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorContact.LastUpdatedBy = AppSettings.CreatedByUserId;
						investorContact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact = new Contact();
						investorContact.Contact.ContactName = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactPerson"]);
						investorContact.Contact.ContactType = string.Empty;
						investorContact.Contact.CreatedBy = AppSettings.CreatedByUserId;
						investorContact.Contact.CreatedDate = DateTime.Now;
						investorContact.Contact.FirstName = string.Empty;
						investorContact.Contact.LastName = string.Empty;
						investorContact.Contact.LastUpdatedBy = AppSettings.CreatedByUserId;
						investorContact.Contact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact.MiddleName = string.Empty;
						investorContact.Contact.ReceivesDistributionNotices = collection[(index + 1).ToString() + "_" + "DistributionNotices"].Contains("true");
						investorContact.Contact.ReceivesFinancials = collection[(index + 1).ToString() + "_" + "Financials"].Contains("true");
						investorContact.Contact.ReceivesInvestorLetters = collection[(index + 1).ToString() + "_" + "InvestorLetters"].Contains("true");
						investorContact.Contact.ReceivesK1 = collection[(index + 1).ToString() + "_" + "K1"].Contains("true");
						investorContact.Contact.EntityID = (int)ConfigUtil.CurrentEntityID;

						contactAddress = new ContactAddress();
						contactAddress.CreatedBy = AppSettings.CreatedByUserId;
						contactAddress.CreatedDate = DateTime.Now;
						contactAddress.EntityID = (int)ConfigUtil.CurrentEntityID;
						contactAddress.LastUpdatedBy = AppSettings.CreatedByUserId;
						contactAddress.LastUpdatedDate = DateTime.Now;
						contactAddress.Address = new Address();
						contactAddress.Address.Address1 = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactAddress1"]);
						contactAddress.Address.Address2 = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactAddress2"]);
						contactAddress.Address.Address3 = string.Empty;
						contactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
						contactAddress.Address.City = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactCity"]);
						contactAddress.Address.Country = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]);
						contactAddress.Address.County = string.Empty;
						contactAddress.Address.CreatedBy = AppSettings.CreatedByUserId;
						contactAddress.Address.CreatedDate = DateTime.Now;
						contactAddress.Address.EntityID = (int)ConfigUtil.CurrentEntityID;
						contactAddress.Address.LastUpdatedBy = AppSettings.CreatedByUserId;
						contactAddress.Address.LastUpdatedDate = DateTime.Now;
						contactAddress.Address.Listed = false;
						contactAddress.Address.PostalCode = collection[(index + 1).ToString() + "_" + "ContactZip"];
						contactAddress.Address.State = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactState"]);
						contactAddress.Address.StProvince = string.Empty;
						investorContact.Contact.ContactAddresses.Add(contactAddress);

						investor.InvestorContacts.Add(investorContact);
					}
				}
				InvestorRepository.SaveInvestor(investor);
				// Update custom field Values
				SaveCustomValues(collection, investor.InvestorID);
				return RedirectToAction("New", "Investor");
			} else {
				ViewData["MenuName"] = "Investor";
				model.SelectList.States = SelectListFactory.GetStateSelectList(AdminRepository.GetAllStates());
				model.SelectList.Countries = SelectListFactory.GetCountrySelectList(AdminRepository.GetAllCountries());
				model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
				model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(AdminRepository.GetAllAddressTypes());
				model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
				model.SelectList.Source = SelectListFactory.GetSourceList();
				return View(model);
			}
		}

		//
		// GET: /Investor/Edit/5
		[HttpGet]
		public ActionResult Edit(int? id) {
			EditModel model = new EditModel();
			ViewData["MenuName"] = "Investor";
			model.id = id ?? 0;
			model.SelectList.States = SelectListFactory.GetStateSelectList(AdminRepository.GetAllStates());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.SelectList.Countries = SelectListFactory.GetCountrySelectList(AdminRepository.GetAllCountries());
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			model.ContactInformations = new List<ContactInformation>();
			model.AccountInformations = new List<AccountInformation>();
			model.CustomField = new CustomFieldModel();
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			model.CustomField.InitializeDatePicker = false;
			return View(model);
		}

		//
		// POST: /Investor/Update
		[HttpPost]
		public ActionResult Update(FormCollection collection) {
			int index = 0;
			int addressCount = Convert.ToInt32(collection["AddressInfoCount"]);
			int contactAddressCount = Convert.ToInt32(collection["ContactInfoCount"]);
			int accountCount = Convert.ToInt32(collection["AccountInfoCount"]);
			ResultModel resultModel = new ResultModel();
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(Convert.ToInt32(collection["InvestorId"]));

			InvestorContact investorContact;
			ContactAddress investorContactAddress;
			InvestorAccount investorAccount;

			investor.IsDomestic = Convert.ToBoolean(collection["DomesticForeigns"]);
			investor.Alias = collection["DisplayName"];
			investor.FirstName = collection["DisplayName"];
			investor.Notes = collection["Notes"];
			if (string.IsNullOrEmpty(investor.LastName))
				investor.LastName = "n/a";
			if (investor.CreatedBy == 0)
				investor.CreatedBy = AppSettings.CreatedByUserId;
			if (string.IsNullOrEmpty(collection["StateOfResidency"]) == false)
				investor.ResidencyState = Convert.ToInt32(collection["StateOfResidency"]);
			if (string.IsNullOrEmpty(collection["EntityType"]) == false)
				investor.InvestorEntityTypeID = Convert.ToInt32(collection["EntityType"]);

			// Assign address details
			for (index = 1; index < addressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AddressId"]) == false) {
					InvestorAddress investorAddress = investor.InvestorAddresses.SingleOrDefault(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "AddressId"]));
					if (investorAddress == null) {
						investorAddress = new InvestorAddress();
						investorAddress.CreatedBy = AppSettings.CreatedByUserId;
						investorAddress.CreatedDate = DateTime.Now;
						investorAddress.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorAddress.Address = new Address();
						investorAddress.Address.CreatedDate = DateTime.Now;
						investorAddress.Address.CreatedBy = AppSettings.CreatedByUserId;
						investorAddress.Address.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
						investorAddress.Address.StProvince = string.Empty;
						investorAddress.Address.Listed = false;
						investorAddress.Address.IsPreferred = false;
						investorAddress.Address.City = string.Empty;
						investorAddress.Address.Address3 = string.Empty;
						investorAddress.Address.County = string.Empty;
					}
					investorAddress.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorAddress.LastUpdatedDate = DateTime.Now;

					investorAddress.Address.Address1 = collection[index.ToString() + "_" + "Address1"];
					investorAddress.Address.Address2 = collection[index.ToString() + "_" + "Address2"];
					investorAddress.Address.City = collection[index.ToString() + "_" + "City"];
					investorAddress.Address.LastUpdatedDate = DateTime.Now;
					investorAddress.Address.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorAddress.Address.PostalCode = collection[index.ToString() + "_" + "PostalCode"];
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "Country"]) == false)
						investorAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "Country"]);
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "State"]) == false)
						investorAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "State"]);
					if (investorAddress.InvestorAddressID == 0 && investorAddress.Address.Country > 0 && investorAddress.Address.State > 0) {
						investor.InvestorAddresses.Add(investorAddress);
					}
				}
			}
			// Assign contact address details
			for (index = 1; index < contactAddressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactId"]) == false) {
					investorContact = investor.InvestorContacts.SingleOrDefault(contact => contact.ContactID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactId"]));
					if (investorContact == null) {
						investorContact = new InvestorContact();
						investorContact.CreatedBy = AppSettings.CreatedByUserId;
						investorContact.CreatedDate = DateTime.Now;
						investorContact.EntityID = (int)ConfigUtil.CurrentEntityID;

						investorContact.Contact = new Contact();
						investorContact.Contact.CreatedBy = AppSettings.CreatedByUserId;
						investorContact.Contact.CreatedDate = DateTime.Now;
						investorContact.Contact.LastName = string.Empty;
						investorContact.Contact.FirstName = string.Empty;
						investorContact.Contact.MiddleName = string.Empty;
						investorContact.Contact.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorContact.Contact.ContactType = string.Empty;
					}
					investorContact.LastUpdatedDate = DateTime.Now;
					investorContact.LastUpdatedBy = AppSettings.CreatedByUserId;
					// Assign contact details
					investorContact.Contact.ContactName = collection[index.ToString() + "_" + "ContactPerson"];
					investorContact.Contact.ReceivesDistributionNotices = (collection[index.ToString() + "_" + "DistributionNotices"]).Contains("true");
					investorContact.Contact.ReceivesFinancials = (collection[index.ToString() + "_" + "Financials"]).Contains("true");
					investorContact.Contact.ReceivesInvestorLetters = (collection[index.ToString() + "_" + "InvestorLetters"]).Contains("true");
					investorContact.Contact.ReceivesK1 = (collection[index.ToString() + "_" + "K1"]).Contains("true");
					investorContact.Contact.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorContact.Contact.LastUpdatedDate = DateTime.Now;

					investorContactAddress = investorContact.Contact.ContactAddresses.SingleOrDefault(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactAddressId"]));
					// Assign address details
					if (investorContactAddress == null) {
						investorContactAddress = new ContactAddress();
						investorContactAddress.CreatedBy = AppSettings.CreatedByUserId;
						investorContactAddress.CreatedDate = DateTime.Now;
						investorContactAddress.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorContactAddress.Address = new Address();
						investorContactAddress.Address.CreatedBy = AppSettings.CreatedByUserId;
						investorContactAddress.Address.CreatedDate = DateTime.Now;
						investorContactAddress.Address.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorContactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
						investorContactAddress.Address.StProvince = string.Empty;
						investorContactAddress.Address.Listed = false;
						investorContactAddress.Address.IsPreferred = false;
						investorContactAddress.Address.City = string.Empty;
						investorContactAddress.Address.Address3 = string.Empty;
						investorContactAddress.Address.County = string.Empty;
					}
					investorContactAddress.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorContactAddress.LastUpdatedDate = DateTime.Now;
					investorContactAddress.Address.Address1 = collection[index.ToString() + "_" + "ContactAddress1"];
					investorContactAddress.Address.Address2 = collection[index.ToString() + "_" + "ContactAddress2"];
					investorContactAddress.Address.City = collection[index.ToString() + "_" + "ContactCity"];
					investorContactAddress.Address.PostalCode = collection[index.ToString() + "_" + "ContactPostalCode"];
					investorContactAddress.Address.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorContactAddress.Address.LastUpdatedDate = DateTime.Now;
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactCountry"]) == false)
						investorContactAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "ContactCountry"]);
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactState"]) == false)
						investorContactAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "ContactState"]);
					if (investorContactAddress.ContactAddressID == 0 && investorContactAddress.Address.Country > 0 && investorContactAddress.Address.State > 0) {
						investorContact.Contact.ContactAddresses.Add(investorContactAddress);
						investor.InvestorContacts.Add(investorContact);
					}
				}
			}
			// Assign account details
			for (index = 1; index < accountCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AccountId"]) == false) {
					investorAccount = investor.InvestorAccounts.SingleOrDefault(account => account.InvestorAccountID == Convert.ToInt32(collection[index.ToString() + "_" + "AccountId"]));
					if (investorAccount == null) {
						investorAccount = new InvestorAccount();
						investorAccount.CreatedBy = AppSettings.CreatedByUserId;
						investorAccount.CreatedDate = DateTime.Now;
						investorAccount.EntityID = (int)ConfigUtil.CurrentEntityID;
						investorAccount.Comments = string.Empty;
						investorAccount.Routing = 0;
						investorAccount.IsPrimary = false;
						investorAccount.Investor = investor;
					}
					investorAccount.Account = collection[index.ToString() + "_" + "AccountNumber"];
					investorAccount.Attention = collection[index.ToString() + "_" + "Attention"];
					investorAccount.Reference = collection[index.ToString() + "_" + "Reference"];
					investorAccount.LastUpdatedBy = AppSettings.CreatedByUserId;
					investorAccount.LastUpdatedDate = DateTime.Now;
					if (investorAccount.InvestorAccountID == 0) {
						investor.InvestorAccounts.Add(investorAccount);
					}
				}
			}
			IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestor(investor);
			if (errorInfo != null) {
				foreach (var err in errorInfo.ToList()) {
					resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
				}
			} else {
				// Update custom field Values
				resultModel.Result += SaveCustomValues(collection, investor.InvestorID);
			}
			return View("Result", resultModel);
		}

		private string SaveCustomValues(FormCollection collection, int key) {
			System.Text.StringBuilder result = new StringBuilder();
			IEnumerable<ErrorInfo> errorInfo;
			IList<CustomField> customFields = AdminRepository.GetAllCustomFields((int)Models.Admin.Enums.Module.Investor);
			foreach (var field in customFields) {
				var customFieldValue = collection["CustomField_" + field.CustomFieldID.ToString()];
				if (customFieldValue != null) {
					CustomFieldValue value = AdminRepository.FindCustomFieldValue(field.CustomFieldID, key);
					if (value == null) {
						value = new CustomFieldValue();
					}
					value.CreatedBy = AppSettings.CreatedByUserId;
					value.CreatedDate = DateTime.Now;
					value.CustomFieldID = field.CustomFieldID;
					value.Key = key;
					value.LastUpdatedBy = AppSettings.CreatedByUserId;
					value.LastUpdatedDate = DateTime.Now;
					switch ((CustomFieldDataType)field.DataTypeID) {
						case CustomFieldDataType.Integer:
							value.IntegerValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToInt32(customFieldValue));
							break;
						case CustomFieldDataType.DateTime:
							value.DateValue = (string.IsNullOrEmpty(customFieldValue) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(customFieldValue));
							break;
						case CustomFieldDataType.Text:
							value.TextValue = customFieldValue;
							break;
						case CustomFieldDataType.Currency:
							value.CurrencyValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToDecimal(customFieldValue));
							break;
						case CustomFieldDataType.Boolean:
							value.BooleanValue = (customFieldValue.Contains("true") ? true : false);
							break;
					}
					errorInfo = value.Save();
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							result.Append(err.PropertyName + " : " + err.ErrorMessage + "\n");
						}
					}
				}
			}
			return result.ToString();
		}

		//
		// GET: /Investor/Delete
		[HttpGet]
		public bool Delete(int id) {
			//InvestorRepository.Delete(id);
			return true;
		}

		//
		// GET: /Investor/DeleteContactAddress
		[HttpGet]
		public bool DeleteContactAddress(int id) {
			InvestorRepository.DeleteInvestorContact(id);
			return true;
		}

		//
		// GET: /Investor/DeleteInvestorAccount
		[HttpGet]
		public bool DeleteInvestorAccount(int id) {
			InvestorRepository.DeleteInvestorAccount(id);
			return true;
		}

		//
		// GET: /Investor/InvestorNameAvailable
		[HttpGet]
		public string InvestorNameAvailable(string InvestorName, int InvestorId) {
			if (InvestorRepository.InvestorNameAvailable(InvestorName, InvestorId))
				return "Investor Name is already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Investor/FindInvestors
		public JsonResult FindInvestors() {
			List<InvestorDetail> investorDetails = InvestorRepository.FindInvestors(Request.QueryString["term"]);
			List<AutoCompleteList> autoCompleteLists = new List<AutoCompleteList>();
			foreach (var detail in investorDetails) {
				autoCompleteLists.Add(new AutoCompleteList { id = detail.InvestorId.ToString(), label = detail.InvestorName + "  (" + detail.Social.ToString() + ")", value = detail.InvestorName.ToString() });
			}
			return Json(autoCompleteLists, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/FindOtherInvestors
		public JsonResult FindOtherInvestors() {
			List<InvestorDetail> investorDetails = InvestorRepository.FindOtherInvestors(Request.QueryString["term"], Convert.ToInt32(Request.QueryString["investorid"]));
			List<AutoCompleteList> autoCompleteLists = new List<AutoCompleteList>();
			foreach (var detail in investorDetails) {
				autoCompleteLists.Add(new AutoCompleteList { id = detail.InvestorId.ToString(), label = detail.InvestorName + "  (" + detail.Social.ToString() + ")", value = detail.InvestorName.ToString() });
			}
			return Json(autoCompleteLists, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/InvestorDetail
		public JsonResult InvestorDetail(int id) {
			return Json(InvestorRepository.FindInvestorDetail(id), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/Edit
		public JsonResult FindInvestor(int id) {
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(id);
			EditModel model = new EditModel();
			if (investor != null) {
				AddressInformation addressInfo;
				ContactInformation contactInfo;
				AccountInformation accountInfo;
				model.InvestorName = investor.InvestorName;
				model.DisplayName = investor.Alias;
				model.DomesticForeign = investor.IsDomestic;
				model.EntityType = investor.InvestorEntityTypeID;
				model.SocialSecurityTaxId = investor.Social;
				model.StateOfResidency = (int)(investor.ResidencyState != null ? investor.ResidencyState : 0);
				model.Notes = investor.Notes;
				foreach (var address in investor.InvestorAddresses) {
					if (address.Address != null) {
						addressInfo = new AddressInformation();
						addressInfo.Address1 = address.Address.Address1;
						addressInfo.Address2 = address.Address.Address2;
						addressInfo.City = address.Address.City;
						addressInfo.Country = address.Address.Country;
						addressInfo.Email = string.Empty;
						addressInfo.Fax = string.Empty;
						addressInfo.Phone = string.Empty;
						addressInfo.WebAddress = string.Empty;
						addressInfo.Zip = address.Address.PostalCode;
						addressInfo.State = (int)address.Address.State;
						addressInfo.AddressId = address.Address.AddressID;
						model.AddressInformations.Add(addressInfo);
					}
				}
				foreach (var investorContact in investor.InvestorContacts) {
					contactInfo = new ContactInformation();
					contactInfo.ContactPerson = investorContact.Contact.ContactName;
					contactInfo.DistributionNotices = investorContact.Contact.ReceivesDistributionNotices;
					contactInfo.Financials = investorContact.Contact.ReceivesFinancials;
					contactInfo.InvestorLetters = investorContact.Contact.ReceivesInvestorLetters;
					contactInfo.K1 = investorContact.Contact.ReceivesK1;
					contactInfo.Designation = string.Empty;
					contactInfo.ContactId = investorContact.ContactID;
					foreach (var contact in investorContact.Contact.ContactAddresses) {
						contactInfo.ContactAddressId = contact.Address.AddressID;
						contactInfo.ContactZip = contact.Address.PostalCode;
						contactInfo.ContactWebAddress = string.Empty;
						contactInfo.ContactState = (int)contact.Address.State;
						contactInfo.ContactPhoneNumber = string.Empty;
						contactInfo.ContactAddress1 = contact.Address.Address1;
						contactInfo.ContactAddress2 = contact.Address.Address2;
						contactInfo.ContactCity = contact.Address.City;
						contactInfo.ContactCountry = contact.Address.Country;
						contactInfo.ContactEmail = string.Empty;
						contactInfo.ContactFaxNumber = string.Empty;
					}
					model.ContactInformations.Add(contactInfo);
				}
				foreach (var investorAccount in investor.InvestorAccounts) {
					accountInfo = new AccountInformation();
					accountInfo.AccountId = investorAccount.InvestorAccountID;
					accountInfo.AccountNumber = investorAccount.Account;
					accountInfo.Attention = investorAccount.Attention;
					accountInfo.Reference = investorAccount.Reference;
					accountInfo.ABANumber = string.Empty;
					accountInfo.AccountOf = string.Empty;
					accountInfo.BankName = string.Empty;
					accountInfo.ByOrderOf = string.Empty;
					accountInfo.FFC = string.Empty;
					accountInfo.FFCNO = string.Empty;
					accountInfo.IBAN = string.Empty;
					accountInfo.Swift = string.Empty;
					model.AccountInformations.Add(accountInfo);
				}
				if (model.AddressInformations.Count == 0)
					model.AddressInformations.Add(new AddressInformation());
				if (model.ContactInformations.Count == 0)
					model.ContactInformations.Add(new ContactInformation());
				if (model.AccountInformations.Count == 0)
					model.AccountInformations.Add(new AccountInformation());
				model.FundInformations = new FlexigridData();
				model.FundInformations.page = 1;
				model.FundInformations.total = investor.InvestorFunds.Count();
				foreach (var fund in investor.InvestorFunds) {
					FlexigridRow row = new FlexigridRow();
					row.cell.Add(fund.Fund.FundName.ToString());
					row.cell.Add(string.Format("{0:C}", fund.TotalCommitment));
					row.cell.Add(string.Format("{0:C}", Convert.ToDecimal(fund.UnfundedAmount)));
					Models.Entity.InvestorType investorType = InvestorRepository.FindInvestorType((int)fund.InvestorTypeId);
					if (investorType != null)
						row.cell.Add(investorType.InvestorTypeName);
					else
						row.cell.Add(string.Empty);
					model.FundInformations.rows.Add(row);
				}
				/* Load Custom Fields */
				model.CustomField = new CustomFieldModel();
				IList<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(id);
				var customFields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
				model.CustomField.Values = new List<CustomFieldValueDetail>();
				foreach (var field in customFields) {
					var value = customFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == field.CustomFieldID);
					if (value != null) {
						model.CustomField.Values.Add(new CustomFieldValueDetail {
							CustomFieldId = value.CustomFieldID,
							CustomFieldValueId = value.CustomFieldID,
							DataTypeId = value.CustomField.DataTypeID,
							BooleanValue = value.BooleanValue ?? false,
							CurrencyValue = (value.CurrencyValue ?? 0),
							DateValue = ((value.DateValue ?? Convert.ToDateTime("01/01/1900")).Year == 1900 ? string.Empty : (value.DateValue ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")),
							IntegerValue = value.IntegerValue ?? 0,
							TextValue = value.TextValue,
							Key = id
						});
					} else {
						model.CustomField.Values.Add(new CustomFieldValueDetail {
							CustomFieldId = field.CustomFieldID,
							CustomFieldValueId = 0,
							DataTypeId = field.DataTypeID,
							BooleanValue = false,
							CurrencyValue = 0,
							DateValue = string.Empty,
							IntegerValue = 0,
							TextValue = string.Empty,
							Key = id
						});
					}
				}

			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Result() {
			return View();
		}
	}
}
