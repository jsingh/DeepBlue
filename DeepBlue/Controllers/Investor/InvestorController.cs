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

		#region New Investor

		//
		// GET: /Investor/New
		public ActionResult New() {
			ViewData["MenuName"] = "Investor";
			ViewData["PageName"] = "New Investor Setup";
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
			model.Country = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			return View(model);
		}

		//
		// POST: /Investor/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			string ErrorMessage = InvestorNameAvailable(model.InvestorName, model.InvestorId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("InvestorName", ErrorMessage);
			}
			ErrorMessage = SocialSecurityTaxIdAvailable(model.SocialSecurityTaxId, model.InvestorId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("SocialSecurityTaxId", ErrorMessage);
			}
			if (ModelState.IsValid) {
				// Attempt to create new deal.
				DeepBlue.Models.Entity.Investor investor = new DeepBlue.Models.Entity.Investor();
				investor.Alias = model.Alias;
				investor.IsDomestic = model.DomesticForeign;
				investor.InvestorEntityTypeID = model.EntityType;
				investor.InvestorName = model.InvestorName;
				investor.FirstName = model.Alias;
				investor.ResidencyState = model.StateOfResidency;
				investor.Social = model.SocialSecurityTaxId ?? "";
				investor.Notes = model.Notes;

				investor.TaxID = 0;
				investor.FirstName = string.Empty;
				investor.LastName = "n/a";
				investor.ManagerName = string.Empty;
				investor.MiddleName = string.Empty;
				investor.PrevInvestorID = 0;
				investor.CreatedBy = Authentication.CurrentUser.UserID;
				investor.CreatedDate = DateTime.Now;
				investor.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investor.LastUpdatedDate = DateTime.Now;
				investor.EntityID = Authentication.CurrentEntity.EntityID;
				investor.TaxExempt = false;

				// Check investor country and state is valid.
				if (model.Country > 0 && model.State > 0) {
					// Attempt to create new investor address.
					InvestorAddress investorAddress = new InvestorAddress();
					investorAddress.CreatedBy = Authentication.CurrentUser.UserID;
					investorAddress.CreatedDate = DateTime.Now;
					investorAddress.EntityID = Authentication.CurrentEntity.EntityID;
					investorAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorAddress.LastUpdatedDate = DateTime.Now;

					investorAddress.Address = new Address();
					investorAddress.Address.Address1 = model.Address1 ?? "";
					investorAddress.Address.Address2 = model.Address2 ?? "";
					investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
					investorAddress.Address.City = model.City ?? "";
					investorAddress.Address.Country = model.Country;
					investorAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
					investorAddress.Address.CreatedDate = DateTime.Now;
					investorAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorAddress.Address.LastUpdatedDate = DateTime.Now;
					investorAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
					investorAddress.Address.PostalCode = model.Zip;
					investorAddress.Address.State = model.State;
					/* Add New Investor Address */
					investor.InvestorAddresses.Add(investorAddress);
				}

				/* Bank Account */
				InvestorAccount investorAccount;
				for (int index = 0; index < model.AccountLength; index++) {
					if (string.IsNullOrEmpty(collection[(index + 1).ToString() + "_" + "AccountNumber"]) == false) {
						// Attempt to create new investor account.
						investorAccount = new InvestorAccount();
						investorAccount.Comments = string.Empty;
						investorAccount.CreatedBy = Authentication.CurrentUser.UserID;
						investorAccount.CreatedDate = DateTime.Now;
						investorAccount.EntityID = Authentication.CurrentEntity.EntityID;
						investorAccount.IsPrimary = false;
						investorAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
						investorAccount.LastUpdatedDate = DateTime.Now;
						if (string.IsNullOrEmpty(collection[(index + 1).ToString() + "_" + "ABANumber"]) == false) {
							investorAccount.Routing = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ABANumber"]);
						}
						investorAccount.Reference = collection[(index + 1).ToString() + "_" + "Reference"];
						investorAccount.AccountOf = collection[(index + 1).ToString() + "_" + "AccountOf"];
						investorAccount.FFC = collection[(index + 1).ToString() + "_" + "FFC"];
						investorAccount.FFCNumber = collection[(index + 1).ToString() + "_" + "FFCNO"];
						investorAccount.IBAN = collection[(index + 1).ToString() + "_" + "IBAN"];
						investorAccount.ByOrderOf = collection[(index + 1).ToString() + "_" + "ByOrderOf"];
						investorAccount.SWIFT = collection[(index + 1).ToString() + "_" + "Swift"];
						investorAccount.Account = collection[(index + 1).ToString() + "_" + "AccountNumber"];
						investorAccount.Attention = collection[(index + 1).ToString() + "_" + "Attention"];
						investorAccount.BankName = collection[(index + 1).ToString() + "_" + "BankName"];
						investor.InvestorAccounts.Add(investorAccount);
					}
				}

				/* Contact Address */
				InvestorContact investorContact;
				ContactAddress contactAddress;
				for (int index = 0; index < model.ContactLength; index++) {
					if (Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactState"]) > 0 &&
						Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]) > 0) {
						// Attempt to create new investor contact.
						investorContact = new InvestorContact();
						investorContact.CreatedBy = Authentication.CurrentUser.UserID;
						investorContact.CreatedDate = DateTime.Now;
						investorContact.EntityID = Authentication.CurrentEntity.EntityID;
						investorContact.LastUpdatedBy = Authentication.CurrentUser.UserID;
						investorContact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact = new Contact();
						investorContact.Contact.ContactName = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactPerson"]);
						investorContact.Contact.CreatedBy = Authentication.CurrentUser.UserID;
						investorContact.Contact.CreatedDate = DateTime.Now;
						investorContact.Contact.FirstName = "n/a";
						investorContact.Contact.LastName = "n/a";
						investorContact.Contact.LastUpdatedBy = Authentication.CurrentUser.UserID;
						investorContact.Contact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact.ReceivesDistributionNotices = collection[(index + 1).ToString() + "_" + "DistributionNotices"].Contains("true");
						investorContact.Contact.ReceivesFinancials = collection[(index + 1).ToString() + "_" + "Financials"].Contains("true");
						investorContact.Contact.ReceivesInvestorLetters = collection[(index + 1).ToString() + "_" + "InvestorLetters"].Contains("true");
						investorContact.Contact.ReceivesK1 = collection[(index + 1).ToString() + "_" + "K1"].Contains("true");
						investorContact.Contact.Designation = collection[(index + 1).ToString() + "_" + "Designation"];
						investorContact.Contact.EntityID = Authentication.CurrentEntity.EntityID;

						// Attempt to create new investor contact address.
						contactAddress = new ContactAddress();
						contactAddress.CreatedBy = Authentication.CurrentUser.UserID;
						contactAddress.CreatedDate = DateTime.Now;
						contactAddress.EntityID = Authentication.CurrentEntity.EntityID;
						contactAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
						contactAddress.LastUpdatedDate = DateTime.Now;
						contactAddress.Address = new Address();
						contactAddress.Address.Address1 = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactAddress1"]);
						contactAddress.Address.Address2 = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactAddress2"]);
						contactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
						contactAddress.Address.City = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactCity"]);
						contactAddress.Address.Country = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]);
						contactAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
						contactAddress.Address.CreatedDate = DateTime.Now;
						contactAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
						contactAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
						contactAddress.Address.LastUpdatedDate = DateTime.Now;
						contactAddress.Address.PostalCode = collection[(index + 1).ToString() + "_" + "ContactZip"];
						contactAddress.Address.State = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactState"]);

						/* Add Investor Contact Communication Values */
						AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.HomePhone, collection[(index + 1).ToString() + "_" + "ContactPhoneNumber"]);
						AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Fax, collection[(index + 1).ToString() + "_" + "ContactFaxNumber"]);
						AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Email, collection[(index + 1).ToString() + "_" + "ContactEmail"]);
						AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.WebAddress, collection[(index + 1).ToString() + "_" + "ContactWebAddress"]);

						investorContact.Contact.ContactAddresses.Add(contactAddress);
						investor.InvestorContacts.Add(investorContact);
					}
				}
				/* Investor Communication Values */
				AddCommunication(investor, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
				AddCommunication(investor, Models.Admin.Enums.CommunicationType.Email, model.Email);
				AddCommunication(investor, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress);
				AddCommunication(investor, Models.Admin.Enums.CommunicationType.Fax, model.Fax);
				IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestor(investor);
				if (errorInfo != null) {
					resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				}
				else {
					resultModel.Result += SaveCustomValues(collection, investor.InvestorID);
				}
			}
			if (ModelState.IsValid == false) {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}

		private string SaveCustomValues(FormCollection collection, int key) {
			System.Text.StringBuilder result = new StringBuilder();
			IEnumerable<ErrorInfo> errorInfo;
			// Get all investor custom fields.
			List<CustomFieldDetail> customFields = AdminRepository.GetAllCustomFields((int)Models.Admin.Enums.Module.Investor);
			foreach (var field in customFields) {
				var customFieldValue = collection["CustomField_" + field.CustomFieldId.ToString()];
				if (customFieldValue != null) {
					// Attempt to create new custom field value.
					CustomFieldValue value = AdminRepository.FindCustomFieldValue(field.CustomFieldId, key);
					if (value == null) {
						value = new CustomFieldValue();
					}
					value.CreatedBy = Authentication.CurrentUser.UserID;
					value.CreatedDate = DateTime.Now;
					value.CustomFieldID = field.CustomFieldId;
					value.Key = key;
					value.LastUpdatedBy = Authentication.CurrentUser.UserID;
					value.LastUpdatedDate = DateTime.Now;
					switch ((CustomFieldDataType)field.DataTypeId) {
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
						result.Append(ValidationHelper.GetErrorInfo(errorInfo));
					}
				}
			}
			return result.ToString();
		}

		private void AddCommunication(DeepBlue.Models.Entity.Investor investor, DeepBlue.Models.Admin.Enums.CommunicationType communicationType, string value) {
			// Attempt to create investor communication.
			InvestorCommunication investorCommunication = investor.InvestorCommunications.SingleOrDefault(communication => communication.Communication.CommunicationTypeID == (int)communicationType);
			if (investorCommunication == null) {
				investorCommunication = new InvestorCommunication();
				investorCommunication.CreatedBy = Authentication.CurrentUser.UserID;
				investorCommunication.CreatedDate = DateTime.Now;
				investorCommunication.Communication = new Communication();
				investorCommunication.Communication.CreatedBy = Authentication.CurrentUser.UserID;
				investorCommunication.Communication.CreatedDate = DateTime.Now;
				investor.InvestorCommunications.Add(investorCommunication);
			}
			investorCommunication.EntityID = Authentication.CurrentEntity.EntityID;
			investorCommunication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			investorCommunication.LastUpdatedDate = DateTime.Now;
			investorCommunication.Communication.CommunicationTypeID = (int)communicationType;
			investorCommunication.Communication.CommunicationValue = (string.IsNullOrEmpty(value) == false ? value : string.Empty);
			investorCommunication.Communication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			investorCommunication.Communication.LastUpdatedDate = DateTime.Now;
			investorCommunication.Communication.EntityID = Authentication.CurrentEntity.EntityID;
		}

		private void AddCommunication(DeepBlue.Models.Entity.Contact contact, DeepBlue.Models.Admin.Enums.CommunicationType communicationType, string value) {
			// Attempt to create contact communication.
			ContactCommunication contactCommunication = contact.ContactCommunications.SingleOrDefault(communication => communication.Communication.CommunicationTypeID == (int)communicationType);
			if (contactCommunication == null) {
				contactCommunication = new ContactCommunication();
				contactCommunication.CreatedBy = Authentication.CurrentUser.UserID;
				contactCommunication.CreatedDate = DateTime.Now;
				contactCommunication.Communication = new Communication();
				contactCommunication.Communication.CreatedBy = Authentication.CurrentUser.UserID;
				contactCommunication.Communication.CreatedDate = DateTime.Now;
				contact.ContactCommunications.Add(contactCommunication);
			}
			contactCommunication.EntityID = Authentication.CurrentEntity.EntityID;
			contactCommunication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			contactCommunication.LastUpdatedDate = DateTime.Now;
			contactCommunication.Communication.CommunicationTypeID = (int)communicationType;
			contactCommunication.Communication.CommunicationValue = (string.IsNullOrEmpty(value) == false ? value : string.Empty);
			contactCommunication.Communication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			contactCommunication.Communication.LastUpdatedDate = DateTime.Now;
			contactCommunication.Communication.EntityID = Authentication.CurrentEntity.EntityID;
		}

		#endregion

		#region Update Investor

		//
		// GET: /Investor/Edit/5
		[HttpGet]
		public ActionResult Edit(int? id) {
			EditModel model = new EditModel();
			ViewData["MenuName"] = "Investor";
			ViewData["PageName"] = "Update Investor Information";
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

			// Attempt to update investor.
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
			if (string.IsNullOrEmpty(collection["StateOfResidency"]) == false)
				investor.ResidencyState = Convert.ToInt32(collection["StateOfResidency"]);
			if (string.IsNullOrEmpty(collection["EntityType"]) == false)
				investor.InvestorEntityTypeID = Convert.ToInt32(collection["EntityType"]);

			investor.LastUpdatedBy = Authentication.CurrentUser.UserID;
			investor.LastUpdatedDate = DateTime.Now;
			// Assign address details
			for (index = 1; index < addressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AddressId"]) == false) {
					InvestorAddress investorAddress = investor.InvestorAddresses.SingleOrDefault(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "AddressId"]));
					if (investorAddress == null) {
						investorAddress = new InvestorAddress();
						investorAddress.CreatedBy = Authentication.CurrentUser.UserID;
						investorAddress.CreatedDate = DateTime.Now;
						investorAddress.Address = new Address();
						investorAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
						investorAddress.Address.CreatedDate = DateTime.Now;
					}
					investorAddress.EntityID = Authentication.CurrentEntity.EntityID;
					investorAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorAddress.LastUpdatedDate = DateTime.Now;
					investorAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
					investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
					investorAddress.Address.Address1 = collection[index.ToString() + "_" + "Address1"];
					investorAddress.Address.Address2 = collection[index.ToString() + "_" + "Address2"];
					investorAddress.Address.City = collection[index.ToString() + "_" + "City"];
					investorAddress.Address.PostalCode = collection[index.ToString() + "_" + "PostalCode"];
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "Country"]) == false)
						investorAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "Country"]);
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "State"]) == false)
						investorAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "State"]);
					if (investorAddress.InvestorAddressID == 0 && investorAddress.Address.Country > 0 && investorAddress.Address.State > 0)
						investor.InvestorAddresses.Add(investorAddress);
					// Assign communication values
					AddCommunication(investor, Models.Admin.Enums.CommunicationType.HomePhone, collection[index.ToString() + "_" + "Phone"]);
					AddCommunication(investor, Models.Admin.Enums.CommunicationType.Email, collection[index.ToString() + "_" + "Email"]);
					AddCommunication(investor, Models.Admin.Enums.CommunicationType.WebAddress, collection[index.ToString() + "_" + "WebAddress"]);
					AddCommunication(investor, Models.Admin.Enums.CommunicationType.Fax, collection[index.ToString() + "_" + "Fax"]);
				}
			}
			// Assign contact address details
			for (index = 1; index < contactAddressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactId"]) == false) {
					investorContact = investor.InvestorContacts.SingleOrDefault(contact => contact.ContactID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactId"]));
					if (investorContact == null) {
						investorContact = new InvestorContact();
						investorContact.CreatedBy = Authentication.CurrentUser.UserID;
						investorContact.CreatedDate = DateTime.Now;
						investorContact.Contact = new Contact();
						investorContact.Contact.CreatedBy = Authentication.CurrentUser.UserID;
						investorContact.Contact.CreatedDate = DateTime.Now;
					}
					investorContact.EntityID = Authentication.CurrentEntity.EntityID;
					investorContact.LastUpdatedDate = DateTime.Now;
					investorContact.LastUpdatedBy = Authentication.CurrentUser.UserID;
					// Assign contact details
					investorContact.Contact.EntityID = Authentication.CurrentEntity.EntityID;
					investorContact.Contact.ContactName = collection[index.ToString() + "_" + "ContactPerson"];
					if (string.IsNullOrEmpty(investorContact.Contact.FirstName)) investorContact.Contact.FirstName = "n/a";
					if (string.IsNullOrEmpty(investorContact.Contact.LastName)) investorContact.Contact.LastName = "n/a";
					investorContact.Contact.ReceivesDistributionNotices = (collection[index.ToString() + "_" + "DistributionNotices"]).Contains("true");
					investorContact.Contact.ReceivesFinancials = (collection[index.ToString() + "_" + "Financials"]).Contains("true");
					investorContact.Contact.ReceivesInvestorLetters = (collection[index.ToString() + "_" + "InvestorLetters"]).Contains("true");
					investorContact.Contact.ReceivesK1 = (collection[index.ToString() + "_" + "K1"]).Contains("true");
					investorContact.Contact.Designation = collection[index.ToString() + "_" + "Designation"];
					investorContact.Contact.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorContact.Contact.LastUpdatedDate = DateTime.Now;

					investorContactAddress = investorContact.Contact.ContactAddresses.SingleOrDefault(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactAddressId"]));
					// Assign address details
					if (investorContactAddress == null) {
						investorContactAddress = new ContactAddress();
						investorContactAddress.CreatedBy = Authentication.CurrentUser.UserID;
						investorContactAddress.CreatedDate = DateTime.Now;
						investorContactAddress.Address = new Address();
						investorContactAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
						investorContactAddress.Address.CreatedDate = DateTime.Now;
					}
					investorContactAddress.EntityID = Authentication.CurrentEntity.EntityID;
					investorContactAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorContactAddress.LastUpdatedDate = DateTime.Now;
					investorContactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
					investorContactAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
					investorContactAddress.Address.Address1 = collection[index.ToString() + "_" + "ContactAddress1"];
					investorContactAddress.Address.Address2 = collection[index.ToString() + "_" + "ContactAddress2"];
					investorContactAddress.Address.City = collection[index.ToString() + "_" + "ContactCity"];
					investorContactAddress.Address.PostalCode = collection[index.ToString() + "_" + "ContactPostalCode"];
					investorContactAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorContactAddress.Address.LastUpdatedDate = DateTime.Now;
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactCountry"]) == false)
						investorContactAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "ContactCountry"]);
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactState"]) == false)
						investorContactAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "ContactState"]);
					if (investorContactAddress.ContactAddressID == 0 && investorContactAddress.Address.Country > 0 && investorContactAddress.Address.State > 0) {
						investorContact.Contact.ContactAddresses.Add(investorContactAddress);
						investor.InvestorContacts.Add(investorContact);
					}
					/* Add Communication Values */
					AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.HomePhone, collection[index.ToString() + "_" + "ContactPhoneNumber"]);
					AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Fax, collection[index.ToString() + "_" + "ContactFaxNumber"]);
					AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Email, collection[index.ToString() + "_" + "ContactEmail"]);
					AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.WebAddress, collection[index.ToString() + "_" + "ContactWebAddress"]);
				}
			}
			// Assign account details
			for (index = 1; index < accountCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AccountId"]) == false) {
					investorAccount = investor.InvestorAccounts.SingleOrDefault(account => account.InvestorAccountID == Convert.ToInt32(collection[index.ToString() + "_" + "AccountId"]));
					if (investorAccount == null) {
						investorAccount = new InvestorAccount();
						investorAccount.CreatedBy = Authentication.CurrentUser.UserID;
						investorAccount.CreatedDate = DateTime.Now;
						investor.InvestorAccounts.Add(investorAccount);
					}
					investorAccount.EntityID = Authentication.CurrentEntity.EntityID;
					investorAccount.Account = collection[index.ToString() + "_" + "AccountNumber"];
					investorAccount.Attention = collection[index.ToString() + "_" + "Attention"];
					investorAccount.Reference = collection[index.ToString() + "_" + "Reference"];
					investorAccount.AccountOf = collection[index.ToString() + "_" + "AccountOf"];
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ABANumber"]) == false) {
						investorAccount.Routing = Convert.ToInt32(collection[index.ToString() + "_" + "ABANumber"]);
					}
					investorAccount.SWIFT = collection[index.ToString() + "_" + "Swift"];
					investorAccount.IBAN = collection[index.ToString() + "_" + "IBAN"];
					investorAccount.FFC = collection[index.ToString() + "_" + "FFC"];
					investorAccount.FFCNumber = collection[index.ToString() + "_" + "FFCNO"];
					investorAccount.ByOrderOf = collection[index.ToString() + "_" + "ByOrderOf"];
					investorAccount.BankName = collection[index.ToString() + "_" + "BankName"];
					investorAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorAccount.LastUpdatedDate = DateTime.Now;
				}
			}
			IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestor(investor);
			if (errorInfo != null) {
				foreach (var err in errorInfo.ToList()) {
					resultModel.Result +=  err.ErrorMessage + "\n";
				}
			}
			else {
				// Update custom field Values
				resultModel.Result += SaveCustomValues(collection, investor.InvestorID);
			}
			return View("Result", resultModel);
		}

		#endregion

		#region Delete Investor Details

		//
		// GET: /Investor/Delete
		[HttpGet]
		public string Delete(int id) {
			if (AdminRepository.DeleteModule(id) == false) {
				return "Cann't Delete! Child record found!";
			}
			else {
				return string.Empty;
			}
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

		#endregion

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
		// GET: /Investor/SocialSecurityTaxIdAvailable
		[HttpGet]
		public string SocialSecurityTaxIdAvailable(string SocialSecurityTaxId, int InvestorId) {
			if (InvestorRepository.SocialSecurityTaxIdAvailable(SocialSecurityTaxId, InvestorId))
				return "Social Security / Tax Id is already exist";
			else
				return string.Empty;
		}

		//
		// GET: /Investor/FindInvestors
		public JsonResult FindInvestors(string term, int? fundId) {
			return Json(InvestorRepository.FindInvestors(term,fundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/FindOtherInvestors
		public JsonResult FindOtherInvestors(int investorId, string term) {
			return Json(InvestorRepository.FindOtherInvestors(term, investorId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/InvestorDetail
		public JsonResult InvestorDetail(int id) {
			return Json(InvestorRepository.FindInvestorDetail(id), JsonRequestBehavior.AllowGet);
		}

		private string GetInvestorCommunicationValue(DeepBlue.Models.Entity.Investor investor, DeepBlue.Models.Admin.Enums.CommunicationType communicationType) {
			return (from investorCommunication in investor.InvestorCommunications
					where investorCommunication.Communication.CommunicationTypeID == (int)communicationType
					select investorCommunication.Communication.CommunicationValue).SingleOrDefault();
		}

		private string GetContactCommunicationValue(InvestorContact investorContact, DeepBlue.Models.Admin.Enums.CommunicationType communicationType) {
			return (from contactCommunication in investorContact.Contact.ContactCommunications
					where contactCommunication.Communication.CommunicationTypeID == (int)communicationType
					select contactCommunication.Communication.CommunicationValue).SingleOrDefault();
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
						addressInfo.Zip = address.Address.PostalCode;
						addressInfo.State = address.Address.State ?? 0;
						addressInfo.AddressId = address.Address.AddressID;
						addressInfo.Email = GetInvestorCommunicationValue(investor, Models.Admin.Enums.CommunicationType.Email);
						addressInfo.Fax = GetInvestorCommunicationValue(investor, Models.Admin.Enums.CommunicationType.Fax);
						addressInfo.Phone = GetInvestorCommunicationValue(investor, Models.Admin.Enums.CommunicationType.HomePhone);
						addressInfo.WebAddress = GetInvestorCommunicationValue(investor, Models.Admin.Enums.CommunicationType.WebAddress);
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
					contactInfo.Designation = investorContact.Contact.Designation;
					contactInfo.ContactId = investorContact.ContactID;
					foreach (var contact in investorContact.Contact.ContactAddresses) {
						contactInfo.ContactAddressId = contact.Address.AddressID;
						contactInfo.ContactZip = contact.Address.PostalCode;
						contactInfo.ContactState = contact.Address.State ?? 0;
						contactInfo.ContactAddress1 = contact.Address.Address1;
						contactInfo.ContactAddress2 = contact.Address.Address2;
						contactInfo.ContactCity = contact.Address.City;
						contactInfo.ContactCountry = contact.Address.Country;
					}
					contactInfo.ContactEmail = GetContactCommunicationValue(investorContact, Models.Admin.Enums.CommunicationType.Email);
					contactInfo.ContactWebAddress = GetContactCommunicationValue(investorContact, Models.Admin.Enums.CommunicationType.WebAddress);
					contactInfo.ContactPhoneNumber = GetContactCommunicationValue(investorContact, Models.Admin.Enums.CommunicationType.HomePhone);
					contactInfo.ContactFaxNumber = GetContactCommunicationValue(investorContact, Models.Admin.Enums.CommunicationType.Fax);
					model.ContactInformations.Add(contactInfo);
				}
				foreach (var investorAccount in investor.InvestorAccounts) {
					accountInfo = new AccountInformation();
					accountInfo.AccountId = investorAccount.InvestorAccountID;
					accountInfo.AccountNumber = investorAccount.Account;
					accountInfo.Attention = investorAccount.Attention;
					accountInfo.Reference = investorAccount.Reference;
					accountInfo.AccountOf = investorAccount.AccountOf;
					accountInfo.ByOrderOf = investorAccount.ByOrderOf;
					accountInfo.FFC = investorAccount.FFC;
					accountInfo.FFCNO = investorAccount.FFCNumber;
					accountInfo.IBAN = investorAccount.IBAN;
					accountInfo.Swift = investorAccount.SWIFT;
					accountInfo.ABANumber = (investorAccount.Routing > 0 ? investorAccount.Routing.ToString() : string.Empty);
					accountInfo.BankName = investorAccount.BankName;
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
					row.cell.Add(FormatHelper.CurrencyFormat(fund.TotalCommitment));
					row.cell.Add(FormatHelper.CurrencyFormat(Convert.ToDecimal(fund.UnfundedAmount)));
					Models.Entity.InvestorType investorType = InvestorRepository.FindInvestorType((int)fund.InvestorTypeId);
					if (investorType != null)
						row.cell.Add(investorType.InvestorTypeName);
					else
						row.cell.Add(string.Empty);
					model.FundInformations.rows.Add(row);
				}
				/* Load Custom Fields */
				model.CustomField = new CustomFieldModel();
				List<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(id);
				var customFields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
				model.CustomField.Values = new List<CustomFieldValueDetail>();
				foreach (var field in customFields) {
					var value = customFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == field.CustomFieldId);
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
					}
					else {
						model.CustomField.Values.Add(new CustomFieldValueDetail {
							CustomFieldId = field.CustomFieldId,
							CustomFieldValueId = 0,
							DataTypeId = field.DataTypeId,
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
