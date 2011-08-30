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

	public class InvestorController : BaseController {

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
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(AdminRepository.GetAllAddressTypes());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.SelectList.Source = SelectListFactory.GetSourceList();
			model.CustomField = new CustomFieldModel();
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			model.DomesticForeign = true;
			model.AccountLength = 0;
			model.ContactLength = 0;
			model.Country = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			model.CountryName = "United States";
			return View(model);
		}

		//
		// POST: /Investor/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			ResultModel resultModel = new ResultModel();
			IEnumerable<ErrorInfo> errorInfo = null;
			this.TryUpdateModel(model);
			string ErrorMessage = InvestorNameAvailable(model.InvestorName, model.InvestorId);
			StringBuilder errors;
			EmailAttribute emailValidation = new EmailAttribute();
			ZipAttribute zipAttribute = new ZipAttribute();
			WebAddressAttribute webAttribute = new WebAddressAttribute();
			int count = 0;
			string errorTitle = string.Empty;
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

				if (string.IsNullOrEmpty(investorAddress.Address.Address1) == false
					|| string.IsNullOrEmpty(investorAddress.Address.Address2) == false
					|| string.IsNullOrEmpty(investorAddress.Address.City) == false
					|| string.IsNullOrEmpty(investorAddress.Address.PostalCode) == false
					|| string.IsNullOrEmpty(investorAddress.Address.County) == false
					|| string.IsNullOrEmpty(model.Phone) == false
					|| string.IsNullOrEmpty(model.Email) == false
					|| string.IsNullOrEmpty(model.WebAddress) == false
					|| string.IsNullOrEmpty(model.Fax) == false
					) {

					errorTitle = "<b>Address Information:</b>";
					errors = new StringBuilder();
					errorInfo = ValidationHelper.Validate(investorAddress.Address);
					if (errorInfo.Any()) {
						errors.Append(ValidationHelper.GetErrorInfo(errorInfo));
					}
					if (emailValidation.IsValid(model.Email) == false)
						errors.Append("Invalid Email\n");
					if (zipAttribute.IsValid(model.Zip) == false)
						errors.Append("Invalid Zip\n");
					if (webAttribute.IsValid(model.WebAddress) == false)
						errors.Append("Invalid Web Address\n");
					if (string.IsNullOrEmpty(errors.ToString()) == false) {
						resultModel.Result = string.Format("{0}\n{1}\n", errorTitle, errors.ToString());
					}

					if (string.IsNullOrEmpty(resultModel.Result)) {
						/* Add New Investor Address */
						investor.InvestorAddresses.Add(investorAddress);
						/* Investor Communication Values */
						AddCommunication(investor, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
						AddCommunication(investor, Models.Admin.Enums.CommunicationType.Email, model.Email);
						AddCommunication(investor, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress);
						AddCommunication(investor, Models.Admin.Enums.CommunicationType.Fax, model.Fax);
					}
				}

				/* Bank Account */
				count = 0;
				InvestorAccount investorAccount;
				for (int index = 0; index < model.AccountLength; index++) {

					if (DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "BankIndex"]) <= 0) continue;

					count++;

					// Attempt to create new investor account.
					investorAccount = new InvestorAccount();
					investorAccount.Comments = string.Empty;
					investorAccount.CreatedBy = Authentication.CurrentUser.UserID;
					investorAccount.CreatedDate = DateTime.Now;
					investorAccount.EntityID = Authentication.CurrentEntity.EntityID;
					investorAccount.IsPrimary = false;
					investorAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investorAccount.LastUpdatedDate = DateTime.Now;
					investorAccount.Routing = DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "ABANumber"]);
					investorAccount.Reference = Convert.ToString(collection[(index + 1).ToString() + "_" + "Reference"]);
					investorAccount.AccountOf = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountOf"]);
					investorAccount.FFC = Convert.ToString(collection[(index + 1).ToString() + "_" + "FFC"]);
					investorAccount.FFCNumber = Convert.ToString(collection[(index + 1).ToString() + "_" + "FFCNO"]);
					investorAccount.IBAN = Convert.ToString(collection[(index + 1).ToString() + "_" + "IBAN"]);
					investorAccount.ByOrderOf = Convert.ToString(collection[(index + 1).ToString() + "_" + "ByOrderOf"]);
					investorAccount.SWIFT = Convert.ToString(collection[(index + 1).ToString() + "_" + "Swift"]);
					investorAccount.Account = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountNumber"]);
					investorAccount.Attention = Convert.ToString(collection[(index + 1).ToString() + "_" + "Attention"]);
					investorAccount.BankName = Convert.ToString(collection[(index + 1).ToString() + "_" + "BankName"]);

					if (string.IsNullOrEmpty(investorAccount.Comments) == false
					  || string.IsNullOrEmpty(investorAccount.Reference) == false
					  || string.IsNullOrEmpty(investorAccount.AccountOf) == false
					  || string.IsNullOrEmpty(investorAccount.FFC) == false
					  || string.IsNullOrEmpty(investorAccount.FFCNumber) == false
					  || string.IsNullOrEmpty(investorAccount.IBAN) == false
					  || string.IsNullOrEmpty(investorAccount.ByOrderOf) == false
					  || string.IsNullOrEmpty(investorAccount.SWIFT) == false
					  || string.IsNullOrEmpty(investorAccount.Account) == false
					  || string.IsNullOrEmpty(investorAccount.Attention) == false
					  || string.IsNullOrEmpty(investorAccount.BankName) == false
					  || investorAccount.Routing > 0) {
						errorInfo = ValidationHelper.Validate(investorAccount);
						if (errorInfo.Any()) {
							resultModel.Result += string.Format("<b>Bank Information {0}:</b>\n{1}\n", count.ToString(), ValidationHelper.GetErrorInfo(errorInfo));
						}
						if (string.IsNullOrEmpty(resultModel.Result)) {
							investor.InvestorAccounts.Add(investorAccount);
						}
					}
				}

				count = 0;
				/* Contact Address */
				InvestorContact investorContact;
				ContactAddress contactAddress;
				for (int index = 0; index < model.ContactLength; index++) {

					if (DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "ContactIndex"]) <= 0) continue;

					count++;

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
					investorContact.Contact.ReceivesDistributionNotices = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "DistributionNotices"]);
					investorContact.Contact.ReceivesFinancials = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "Financials"]);
					investorContact.Contact.ReceivesInvestorLetters = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "InvestorLetters"]);
					investorContact.Contact.ReceivesK1 = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "K1"]);
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
					string contactPhoneNo = collection[(index + 1).ToString() + "_" + "ContactPhoneNumber"];
					string contactFaxNo = collection[(index + 1).ToString() + "_" + "ContactFaxNumber"];
					string contactEmail = collection[(index + 1).ToString() + "_" + "ContactEmail"];
					string contactWebAddress = collection[(index + 1).ToString() + "_" + "ContactWebAddress"];

					if (string.IsNullOrEmpty(contactAddress.Address.Address1) == false
					   || string.IsNullOrEmpty(contactAddress.Address.Address2) == false
					   || string.IsNullOrEmpty(contactAddress.Address.City) == false
						|| string.IsNullOrEmpty(contactAddress.Address.PostalCode) == false
						|| string.IsNullOrEmpty(investorContact.Contact.ContactName) == false
						|| string.IsNullOrEmpty(contactPhoneNo) == false
						|| string.IsNullOrEmpty(contactFaxNo) == false
						|| string.IsNullOrEmpty(contactEmail) == false
						|| string.IsNullOrEmpty(contactWebAddress) == false
					 ) {
						errorInfo = ValidationHelper.Validate(contactAddress.Address);
						errorInfo = errorInfo.Union(ValidationHelper.Validate(investorContact.Contact));

						errorTitle = "<b>Contact Information {0}:</b>\n{1}\n";
						errors = new StringBuilder();
						if (errorInfo.Any()) {
							errors.Append(ValidationHelper.GetErrorInfo(errorInfo));
						}
						if (emailValidation.IsValid(contactEmail) == false)
							errors.Append("Invalid Email\n");
						if (webAttribute.IsValid(contactWebAddress) == false)
							errors.Append("Invalid Web Address\n");
						if (zipAttribute.IsValid(contactAddress.Address.PostalCode) == false)
							errors.Append("Invalid Zip\n");

						if (string.IsNullOrEmpty(errors.ToString()) == false) {
							resultModel.Result += string.Format(errorTitle, count.ToString(), errors.ToString());
						}

						if (string.IsNullOrEmpty(resultModel.Result)) {
							investorContact.Contact.ContactAddresses.Add(contactAddress);
						}
						if (string.IsNullOrEmpty(resultModel.Result)) {
							investor.InvestorContacts.Add(investorContact);
							AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.HomePhone, contactPhoneNo);
							AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Fax, contactFaxNo);
							AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Email, contactEmail);
							AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.WebAddress, contactWebAddress);
						}
					}

				}

				if (string.IsNullOrEmpty(resultModel.Result)) {
					errorInfo = InvestorRepository.SaveInvestor(investor);
					if (errorInfo != null) {
						resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
					}
					else {
						resultModel.Result += SaveCustomValues(collection, investor.InvestorID);
					}
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
						value.CreatedBy = Authentication.CurrentUser.UserID;
						value.CreatedDate = DateTime.Now;
					}
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
					errorInfo = AdminRepository.SaveCustomFieldValue(value);
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
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			model.SelectList.Source = SelectListFactory.GetSourceList();
			model.ContactInformations = new List<ContactInformation>();
			model.AccountInformations = new List<AccountInformation>();
			model.CustomField = new CustomFieldModel();
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			model.CustomField.InitializeDatePicker = false;
			model.CustomField.IsDisplayMode = true;
			return View(model);
		}

		#endregion

		#region Delete Investor Details

		//
		// GET: /Investor/Delete
		[HttpGet]
		public string Delete(int id) {
			if (InvestorRepository.Delete(id) == false) {
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
			return InvestorRepository.DeleteInvestorContact(id);
		}

		//
		// GET: /Investor/DeleteInvestorAccount
		[HttpGet]
		public bool DeleteInvestorAccount(int id) {
			return InvestorRepository.DeleteInvestorAccount(id);
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
			return Json(InvestorRepository.FindInvestors(term, fundId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/FindOtherInvestors
		public JsonResult FindOtherInvestors(int investorId, string term) {
			return Json(InvestorRepository.FindOtherInvestors(term, investorId), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/InvestorDetail
		public JsonResult InvestorDetail(int id) {
			return Json(InvestorRepository.GetInvestorDetail(id), JsonRequestBehavior.AllowGet);
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

		public JsonResult FindInvestorDetail(int id) {
			EditModel model = InvestorRepository.FindInvestorDetail(id);
			if (model != null) {
				InvestorInformation investorInformation = (InvestorInformation)model;
				LoadCustomFieldValues(ref investorInformation);
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public void LoadCustomFieldValues(ref InvestorInformation model) {
			model.CustomField = new CustomFieldModel();
			List<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(model.InvestorId);
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			if (model.CustomField.Fields != null) {
				foreach (var field in model.CustomField.Fields) {
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
							Key = model.InvestorId
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
							Key = model.InvestorId
						});
					}
				}
			}
		}

		#region Investor Address
		public ActionResult UpdateInvestorAddress(FormCollection collection) {
			AddressInformation model = new AddressInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				InvestorAddress investorAddress = null;
				if ((model.AddressId ?? 0) > 0)
					investorAddress = InvestorRepository.FindInvestorAddress(model.AddressId ?? 0);
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

				if (investorAddress.Address == null) {
					investorAddress.Address = new Address {
						CreatedBy = Authentication.CurrentUser.UserID,
						CreatedDate = DateTime.Now,
					};
				}

				investorAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
				investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
				investorAddress.Address.Address1 = model.Address1;
				investorAddress.Address.Address2 = model.Address2;
				investorAddress.Address.City = model.City;
				investorAddress.Address.PostalCode = model.Zip;
				investorAddress.Address.Country = model.Country;
				investorAddress.Address.State = model.State;
				investorAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorAddress.Address.LastUpdatedDate = DateTime.Now;
				investorAddress.InvestorID = model.InvestorId;

				IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestorAddress(investorAddress);

				if (errorInfo == null) {
					List<InvestorCommunication> investorCommunications = InvestorRepository.FindInvestorCommunications(model.InvestorId);

					// Assign communication values

					InvestorCommunication investorCommunication = investorCommunications.Where(communication => communication.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.HomePhone).FirstOrDefault();
					AddCommunication(ref investorCommunication, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone, model.InvestorId);
					errorInfo = InvestorRepository.SaveInvestorCommunication(investorCommunication);

					if (errorInfo == null) {
						investorCommunication = investorCommunications.Where(communication => communication.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Email).FirstOrDefault();
						AddCommunication(ref investorCommunication, Models.Admin.Enums.CommunicationType.Email, model.Email, model.InvestorId);
						errorInfo = InvestorRepository.SaveInvestorCommunication(investorCommunication);
					}
					if (errorInfo == null) {
						investorCommunication = investorCommunications.Where(communication => communication.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.WebAddress).FirstOrDefault();
						AddCommunication(ref investorCommunication, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress, model.InvestorId);
						errorInfo = InvestorRepository.SaveInvestorCommunication(investorCommunication);
					}
					if (errorInfo == null) {
						investorCommunication = investorCommunications.Where(communication => communication.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Fax).FirstOrDefault();
						AddCommunication(ref investorCommunication, Models.Admin.Enums.CommunicationType.Fax, model.Fax, model.InvestorId);
						errorInfo = InvestorRepository.SaveInvestorCommunication(investorCommunication);
					}
				}
				resultModel.Result = ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result += "True||" + investorAddress.InvestorAddressID;
				}
			}
			else {
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

		public JsonResult FindInvestorAddress(int id) {
			return Json(InvestorRepository.FindInvestorAddressModel(id), JsonRequestBehavior.AllowGet);
		}

		private void AddCommunication(ref InvestorCommunication investorCommunication, DeepBlue.Models.Admin.Enums.CommunicationType communicationType, string value, int investorId) {
			if (investorCommunication == null) {
				investorCommunication = new InvestorCommunication();
				investorCommunication.CreatedBy = Authentication.CurrentUser.UserID;
				investorCommunication.CreatedDate = DateTime.Now;
				investorCommunication.Communication = new Communication();
				investorCommunication.Communication.CreatedBy = Authentication.CurrentUser.UserID;
				investorCommunication.Communication.CreatedDate = DateTime.Now;
			}
			investorCommunication.InvestorID = investorId;
			investorCommunication.EntityID = Authentication.CurrentEntity.EntityID;
			investorCommunication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			investorCommunication.LastUpdatedDate = DateTime.Now;
			investorCommunication.Communication.CommunicationTypeID = (int)communicationType;
			investorCommunication.Communication.CommunicationValue = (string.IsNullOrEmpty(value) == false ? value : string.Empty);
			investorCommunication.Communication.LastUpdatedBy = Authentication.CurrentUser.UserID;
			investorCommunication.Communication.LastUpdatedDate = DateTime.Now;
			investorCommunication.Communication.EntityID = Authentication.CurrentEntity.EntityID;
		}


		#endregion

		#region Investor Contact

		public ActionResult UpdateInvestorContact(FormCollection collection) {
			ContactInformation model = new ContactInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				InvestorContact investorContact = null;
				if ((model.InvestorContactId ?? 0) > 0)
					investorContact = InvestorRepository.FindInvestorContact(model.InvestorContactId ?? 0);
				if (investorContact == null) {
					investorContact = new InvestorContact();
					investorContact.CreatedBy = Authentication.CurrentUser.UserID;
					investorContact.CreatedDate = DateTime.Now;
					investorContact.Contact = new Contact();
					investorContact.Contact.CreatedBy = Authentication.CurrentUser.UserID;
					investorContact.Contact.CreatedDate = DateTime.Now;
				}
				investorContact.InvestorID = model.InvestorId;
				investorContact.EntityID = Authentication.CurrentEntity.EntityID;
				investorContact.LastUpdatedDate = DateTime.Now;
				investorContact.LastUpdatedBy = Authentication.CurrentUser.UserID;
				// Assign contact details
				investorContact.Contact.EntityID = Authentication.CurrentEntity.EntityID;
				investorContact.Contact.ContactName = model.Person;
				if (string.IsNullOrEmpty(investorContact.Contact.FirstName)) investorContact.Contact.FirstName = "n/a";
				if (string.IsNullOrEmpty(investorContact.Contact.LastName)) investorContact.Contact.LastName = "n/a";
				investorContact.Contact.ReceivesDistributionNotices = model.DistributionNotices;
				investorContact.Contact.ReceivesFinancials = model.Financials;
				investorContact.Contact.ReceivesInvestorLetters = model.InvestorLetters;
				investorContact.Contact.ReceivesK1 = model.K1;
				investorContact.Contact.Designation = model.Designation;
				investorContact.Contact.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorContact.Contact.LastUpdatedDate = DateTime.Now;

				ContactAddress investorContactAddress = investorContact.Contact.ContactAddresses.SingleOrDefault(address => address.ContactAddressID == model.ContactAddressId);
				// Assign address details
				if (investorContactAddress == null) {
					investorContactAddress = new ContactAddress();
					investorContactAddress.CreatedBy = Authentication.CurrentUser.UserID;
					investorContactAddress.CreatedDate = DateTime.Now;
					investorContactAddress.Address = new Address();
					investorContactAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
					investorContactAddress.Address.CreatedDate = DateTime.Now;
					investorContact.Contact.ContactAddresses.Add(investorContactAddress);
				}
				investorContactAddress.EntityID = Authentication.CurrentEntity.EntityID;
				investorContactAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorContactAddress.LastUpdatedDate = DateTime.Now;
				investorContactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
				investorContactAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
				investorContactAddress.Address.Address1 = model.Address1;
				investorContactAddress.Address.Address2 = model.Address2;
				investorContactAddress.Address.City = model.City;
				investorContactAddress.Address.PostalCode = model.Zip;
				investorContactAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorContactAddress.Address.LastUpdatedDate = DateTime.Now;
				investorContactAddress.Address.Country = model.Country;
				investorContactAddress.Address.State = model.State;

				/* Add Communication Values */
				AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.HomePhone, model.Phone);
				AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Fax, model.Fax);
				AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Email, model.Email);
				AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.WebAddress, model.WebAddress);

				IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestorContact(investorContact);
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result))
					resultModel.Result += "True||" + investorContact.InvestorContactID;
			}
			else {
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

		public JsonResult FindInvestorContact(int id) {
			return Json(InvestorRepository.FindInvestorContactModel(id), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Investor Bank Account

		public ActionResult UpdateInvestorBankDetail(FormCollection collection) {
			AccountInformation model = new AccountInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				InvestorAccount investorAccount = null;
				if ((model.AccountId ?? 0) > 0)
					investorAccount = InvestorRepository.FindInvestorAccount(model.AccountId ?? 0);
				if (investorAccount == null) {
					investorAccount = new InvestorAccount();
					investorAccount.CreatedBy = Authentication.CurrentUser.UserID;
					investorAccount.CreatedDate = DateTime.Now;
				}
				investorAccount.InvestorID = model.InvestorId;
				investorAccount.EntityID = Authentication.CurrentEntity.EntityID;
				investorAccount.Account = model.AccountNumber;
				investorAccount.Attention = model.Attention;
				investorAccount.Reference = model.Reference;
				investorAccount.AccountOf = model.AccountOf;
				investorAccount.Routing = model.ABANumber;
				investorAccount.SWIFT = model.Swift;
				investorAccount.IBAN = model.IBAN;
				investorAccount.FFC = model.FFC;
				investorAccount.FFCNumber = model.FFCNO;
				investorAccount.ByOrderOf = model.ByOrderOf;
				investorAccount.BankName = model.BankName;
				investorAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorAccount.LastUpdatedDate = DateTime.Now;

				IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestorAccount(investorAccount);
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result))
					resultModel.Result += "True||" + investorAccount.InvestorAccountID;
			}
			else {
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

		public JsonResult FindInvestorAccount(int id) {
			return Json(InvestorRepository.FindInvestorAccountModel(id), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Investor Information
		public ActionResult UpdateInvestorInformation(FormCollection collection) {
			InvestorInformation model = new InvestorInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				// Attempt to update investor.
				DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(model.InvestorId);
				if (investor != null) {
					investor.IsDomestic = model.DomesticForeign;
					investor.Alias = model.DisplayName;
					investor.Notes = model.Notes;
					investor.ResidencyState = model.StateOfResidency;
					investor.InvestorEntityTypeID = model.EntityType;
					investor.LastUpdatedBy = Authentication.CurrentUser.UserID;
					investor.LastUpdatedDate = DateTime.Now;
					if (string.IsNullOrEmpty(investor.FirstName))
						investor.FirstName = "n/a";
					if (string.IsNullOrEmpty(investor.LastName))
						investor.LastName = "n/a";
					IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestor(investor);
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
					else {
						// Update custom field Values
						resultModel.Result += SaveCustomValues(collection, investor.InvestorID);
					}
					if (string.IsNullOrEmpty(resultModel.Result))
						resultModel.Result += "True||" + investor.InvestorID;
				}
			}
			else {
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

		public JsonResult FindInvestorInformation(int id) {
			InvestorInformation investorInformation = InvestorRepository.FindInvestorInformation(id);
			LoadCustomFieldValues(ref investorInformation);
			return Json(investorInformation, JsonRequestBehavior.AllowGet);
		}
		#endregion

		public ActionResult Result() {
			return View();
		}
	}
}
