﻿using System;
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
using DeepBlue.Models.Admin;
using DeepBlue.Models.Excel;
using System.Configuration;
using System.IO;


namespace DeepBlue.Controllers.Investor {

	[OtherEntityAuthorize]
	public class InvestorController : BaseController {

		#region Constants
		private const string EXCELINVESTORERROR_BY_KEY = "Investor-Excel-{0}";
		private const string EXCELINVESTORBANKERROR_BY_KEY = "InvestorBank-Excel-{0}";
		private const string EXCELINVESTORCONTACTERROR_BY_KEY = "InvestorContact-Excel-{0}";
		#endregion

		public IInvestorRepository InvestorRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public InvestorController()
			: this(new InvestorRepository(), new AdminRepository()) {
		}

		public InvestorController(IInvestorRepository investorRepository, IAdminRepository adminRepository) {
			InvestorRepository = investorRepository;
			AdminRepository = adminRepository;
		}

		#region New Investor

		//
		// GET: /Investor/New
		public ActionResult New() {
			ViewData["MenuName"] = "InvestorManagement";
			ViewData["SubmenuName"] = "NewInvestorSetup";
			ViewData["PageName"] = "New Investor Setup";
			CreateModel model = new CreateModel();
			model.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			//model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(AdminRepository.GetAllAddressTypes());
			model.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.Sources = SelectListFactory.GetSourceList();
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
				investor.Source = model.Source;
				investor.FOIA = model.FOIA;
				investor.ERISA = model.ERISA;

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
					investorAccount.FFCNumber = Convert.ToString(collection[(index + 1).ToString() + "_" + "FFCNumber"]);
					investorAccount.IBAN = Convert.ToString(collection[(index + 1).ToString() + "_" + "IBAN"]);
					investorAccount.ByOrderOf = Convert.ToString(collection[(index + 1).ToString() + "_" + "ByOrderOf"]);
					investorAccount.SWIFT = Convert.ToString(collection[(index + 1).ToString() + "_" + "Swift"]);
					investorAccount.Account = Convert.ToString(collection[(index + 1).ToString() + "_" + "Account"]);
					investorAccount.AccountNumberCash = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountNumber"]);
					investorAccount.Attention = Convert.ToString(collection[(index + 1).ToString() + "_" + "Attention"]);
					investorAccount.BankName = Convert.ToString(collection[(index + 1).ToString() + "_" + "BankName"]);
					investorAccount.Phone = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountPhone"]);
					investorAccount.Fax = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountFax"]);

					if (string.IsNullOrEmpty(investorAccount.Comments) == false
					  || string.IsNullOrEmpty(investorAccount.Reference) == false
					  || string.IsNullOrEmpty(investorAccount.AccountOf) == false
					  || string.IsNullOrEmpty(investorAccount.FFC) == false
					  || string.IsNullOrEmpty(investorAccount.FFCNumber) == false
					  || string.IsNullOrEmpty(investorAccount.IBAN) == false
					  || string.IsNullOrEmpty(investorAccount.ByOrderOf) == false
					  || string.IsNullOrEmpty(investorAccount.SWIFT) == false
					  || string.IsNullOrEmpty(investorAccount.Account) == false
					  || string.IsNullOrEmpty(investorAccount.AccountNumberCash) == false
					  || string.IsNullOrEmpty(investorAccount.Attention) == false
					  || string.IsNullOrEmpty(investorAccount.BankName) == false
					  || string.IsNullOrEmpty(investorAccount.Phone) == false
					  || string.IsNullOrEmpty(investorAccount.Fax) == false
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
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result = "True||" + investor.InvestorID;
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
			EditModel model = null;
			if (id > 0) {
				model = InvestorRepository.FindInvestorDetail(id);
			}
			else {
				model = new EditModel();
				model.AddressInformations = new List<AddressInformation>();
				model.AccountInformations = new List<AccountInformation>();
				model.ContactInformations = new List<ContactInformation>();
				List<AddressInformation> addresses = (List<AddressInformation>)model.AddressInformations;
				addresses.Add(new AddressInformation { });
				List<AccountInformation> accounts = (List<AccountInformation>)model.AccountInformations;
				accounts.Add(new AccountInformation { });
				List<ContactInformation> contacts = (List<ContactInformation>)model.ContactInformations;
				contacts.Add(new ContactInformation { });
			}
			if (model != null) {
				InvestorInformation investorInformation = (InvestorInformation)model;
				LoadCustomFieldValues(ref investorInformation);
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/InvestmentDetailList
		[HttpGet]
		public JsonResult InvestmentDetailList(int pageIndex, int pageSize, string sortName, string sortOrder, int investorId) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<FundInformation> fundInformations = InvestorRepository.GetInvestmentDetails(pageIndex, pageSize, sortName, sortOrder, ref totalRows, investorId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fundInformation in fundInformations) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> { 
						fundInformation.FundName,
						fundInformation.InvestorType,
						FormatHelper.CurrencyFormat(fundInformation.TotalCommitment),
						FormatHelper.CurrencyFormat(fundInformation.UnfundedAmount),
						fundInformation.FundClose
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
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

		//
		// GET: /Investor/
		public ActionResult Index() {
			return View();
		}

		public ActionResult ThankYou() {
			ViewData["MenuName"] = "InvestorManagement";
			return View();
		}

		//
		// GET: /Investor/Details/5
		public ActionResult Details(int id) {
			return View();
		}

		public ActionResult Result() {
			return View();
		}

		#endregion

		#region Update Investor

		//
		// GET: /Investor/Edit/5
		[HttpGet]
		public ActionResult Edit(int? id) {
			EditModel model = new EditModel();
			ViewData["MenuName"] = "InvestorManagement";
			ViewData["SubmenuName"] = "UpdateInvestorInformation";
			ViewData["PageName"] = "UpdateInvestorInformation";
			if ((id ?? 0) == 0) {
				model.id = InvestorRepository.FindLastInvestorId();
			}
			else {
				model.id = id ?? 0;
			}
			model.InvestorId = model.id;
			model.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(AdminRepository.GetAllInvestorEntityTypes());
			model.Sources = SelectListFactory.GetSourceList();
			model.ContactInformations = new List<ContactInformation>();
			model.AccountInformations = new List<AccountInformation>();
			model.CustomField = new CustomFieldModel();
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Investor);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			model.CustomField.InitializeDatePicker = false;
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

		public JsonResult ContactInformationList(int pageIndex, int pageSize, string sortName, string sortOrder, int investorId) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<ContactInformation> contactInformations = InvestorRepository.ContactInformationList(pageIndex,
																										pageSize,
																										sortName,
																										sortOrder,
																										ref totalRows,
																										investorId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var contact in contactInformations) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
							contact.ContactId,
							contact.InvestorContactId,
							contact.AddressId,
							contact.ContactAddressId,
							contact.Person,
							contact.Designation,
							contact.Phone,
							contact.Fax,
							contact.Email,
							contact.WebAddress,
							contact.Address1,
							contact.Address2,
							contact.City,
							contact.Country,
							contact.CountryName,
							contact.State,
							contact.StateName,
							contact.Zip,
							contact.DistributionNotices,
							contact.Financials,
							contact.K1,
							contact.InvestorLetters
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Investor Bank Account

		public ActionResult UpdateInvestorBankDetail(FormCollection collection) {
			AccountInformation model = new AccountInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			if (string.IsNullOrEmpty(model.Account)) {
				ModelState.AddModelError("Account", "Account Name is required");
			}
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
				investorAccount.Account = model.Account;
				investorAccount.AccountNumberCash = model.AccountNumber;
				investorAccount.Attention = model.Attention;
				investorAccount.Reference = model.Reference;
				investorAccount.AccountOf = model.AccountOf;
				investorAccount.Routing = model.ABANumber;
				investorAccount.SWIFT = model.Swift;
				investorAccount.IBAN = model.IBAN;
				investorAccount.FFC = model.FFC;
				investorAccount.FFCNumber = model.FFCNumber;
				investorAccount.ByOrderOf = model.ByOrderOf;
				investorAccount.BankName = model.BankName;
				investorAccount.Phone = model.AccountPhone;
				investorAccount.Fax = model.AccountFax;
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

		public JsonResult BankAccountInformationList(int pageIndex, int pageSize, string sortName, string sortOrder, int investorId) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<AccountInformation> accountInformations = InvestorRepository.BankAccountInformationList(pageIndex,
																										pageSize,
																										sortName,
																										sortOrder,
																										ref totalRows,
																										investorId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var account in accountInformations) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
							account.AccountId,
							account.BankName,
							account.AccountNumber,
							account.ABANumber,
							account.AccountOf,
							account.FFC,
							account.FFCNumber,
							account.Attention,
							account.Swift,
							account.IBAN,
							account.Reference,
							account.ByOrderOf
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Investor Information
		public ActionResult UpdateInvestorInformation(FormCollection collection) {
			InvestorInformation model = new InvestorInformation();
			this.TryUpdateModel(model, collection);
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = InvestorNameAvailable(model.InvestorName, model.InvestorId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("InvestorName", ErrorMessage);
			}
			if (ModelState.IsValid) {
				// Attempt to update investor.
				DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(model.InvestorId);
				if (investor != null) {
					investor.InvestorName = model.InvestorName;
					investor.IsDomestic = model.DomesticForeign;
					investor.Alias = model.Alias;
					investor.Notes = model.Notes;
					investor.Source = model.Source;
					investor.FOIA = model.FOIA;
					investor.ERISA = model.ERISA;
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

		#region Investor Library

		[HttpGet]
		public ActionResult Library() {
			ViewData["MenuName"] = "InvestorManagement";
			ViewData["SubmenuName"] = "InvestorLibrary";
			ViewData["PageName"] = "InvestorLibrary";
			return View();
		}

		[HttpGet]
		public JsonResult InvestorLibraryList(int pageIndex, int pageSize, string sortName, string sortOrder, int? investorId, int? fundId) {
			ResultModel resultModel = new ResultModel();
			List<InvertorLibraryInformation> investorLibrary = null;
			int totalRows = 0;
			int leftPageIndex = 0;
			int rightPageIndex = 0;
			int totalPages = 0;

			FlexigridData flexgridData = new FlexigridData();
			List<InvestorList> investorLists = new List<InvestorList>();

			investorLibrary = InvestorRepository.GetInvestorLibraryList(pageIndex, pageSize, sortName, sortOrder, ref totalRows, investorId, fundId);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fund in investorLibrary) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
							new {
								 fund.FundName, fund.FundID, fund.TotalCommitted
							} 
						}
				});
				foreach (var fundInformation in fund.FundInformations) {
					InvestorList investor = investorLists.FirstOrDefault(fundInvestor => fundInvestor.InvestorID == fundInformation.InvestorID);
					if (investor == null) {
						investorLists.Add(new InvestorList { FundID = fund.FundID, InvestorID = fundInformation.InvestorID, InvestorName = fundInformation.InvestorName });

					}
				}
			}
			investorLists.Sort();
			totalPages = Convert.ToInt32(Math.Ceiling(decimal.Divide((decimal)totalRows, (decimal)pageSize)));
			if (totalPages > pageIndex) {
				rightPageIndex = pageIndex + 1;
			}
			if (pageIndex > 1) {
				leftPageIndex = pageIndex - 1;
			}
			return Json(new { FlexGridData = flexgridData, Investors = investorLists, LeftPageIndex = leftPageIndex, RightPageIndex = rightPageIndex, Library = investorLibrary }, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/FindInvestorFunds
		[HttpGet]
		public JsonResult FindInvestorFunds(string term) {
			return Json(InvestorRepository.FindInvestorFunds(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Investor/FindFundInvestors
		[HttpGet]
		public JsonResult FindFundInvestors(string term) {
			return Json(InvestorRepository.FindFundInvestors(term), JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Import Investor

		[HttpPost]
		public ActionResult ImportExcel(FormCollection collection) {
			DeepBlue.Models.Deal.ImportExcelFileModel model = new DeepBlue.Models.Deal.ImportExcelFileModel();
			ImportExcelModel importExcelModel = new DeepBlue.Models.Excel.ImportExcelModel();
			importExcelModel.Tables = new List<ImportExcelTableModel>();
			this.TryUpdateModel(model);
			if (string.IsNullOrEmpty(model.FileName)) {
				ModelState.AddModelError("FileName", "File Name is required");
			}
			if (ModelState.IsValid) {
				string rootPath = Server.MapPath("~/");
				string uploadFilePath = Path.Combine(model.FilePath, model.FileName);
				string errorMessage = string.Empty;
				string sessionKey = string.Empty;
				DataSet ds = ExcelConnection.GetDataSet(uploadFilePath, uploadFilePath, ref errorMessage, ref sessionKey);
				ImportExcelTableModel tableModel = null;
				if (string.IsNullOrEmpty(errorMessage)) {
					foreach (DataTable dt in ds.Tables) {
						tableModel = new ImportExcelTableModel();
						tableModel.TableName = dt.TableName;
						tableModel.TotalRows = dt.Rows.Count;
						tableModel.SessionKey = sessionKey;
						foreach (DataColumn column in dt.Columns) {
							tableModel.Columns.Add(column.ColumnName);
						}
						importExcelModel.Tables.Add(tableModel);
					}
				}
				else {
					importExcelModel.Result += errorMessage;
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							importExcelModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(importExcelModel);
		}

		#region ImportInvestorExcel

		[HttpPost]
		public ActionResult ImportInvestorExcel(FormCollection collection) {
			ImportInvestorExcelModel model = new ImportInvestorExcelModel();
			ResultModel resultModel = new ResultModel();
			MemoryCacheManager cacheManager = new MemoryCacheManager();
			int totalPages = 0;
			int totalRows = 0;
			int completedRows = 0;
			int? succssRows = 0;
			int? errorRows = 0;
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				string key = string.Format(EXCELINVESTORERROR_BY_KEY, model.SessionKey);
				List<DeepBlue.Models.Deal.ImportExcelError> errors = cacheManager.Get(key, () => {
					return new List<DeepBlue.Models.Deal.ImportExcelError>();
				});
				DataSet ds = ExcelConnection.ImportExcelDataset(model.SessionKey);
				if (ds != null) {
					PagingDataTable importExcelTable = null;
					if (ds.Tables[model.InvestorTableName] != null) {
						importExcelTable = (PagingDataTable)ds.Tables[model.InvestorTableName];
					}
					if (importExcelTable != null) {
						importExcelTable.PageSize = model.PageSize;
						PagingDataTable table = importExcelTable.Skip(model.PageIndex);
						totalPages = importExcelTable.TotalPages;
						totalRows = importExcelTable.TotalRows;
						if (totalPages > model.PageIndex) {
							completedRows = (model.PageIndex * importExcelTable.PageSize);
						}
						else {
							completedRows = totalRows;
						}

						int rowNumber = 0;

						string investorName = string.Empty;
						string displayName = string.Empty;
						string socialSecurityID = string.Empty;
						bool domesticForeign = false;
						string stateOfResidencyName = string.Empty;
						string entityType = string.Empty;
						string source = string.Empty;
						bool foia = false;
						bool erisa = false;
						string notes = string.Empty;
						string phone = string.Empty;
						string fax = string.Empty;
						string email = string.Empty;
						string webAddress = string.Empty;
						string address1 = string.Empty;
						string address2 = string.Empty;
						string city = string.Empty;
						string stateName = string.Empty;
						string zip = string.Empty;
						string countryName = string.Empty;

						Models.Entity.InvestorEntityType investorEntityType;
						Models.Entity.STATE stateOfResidency;
						Models.Entity.COUNTRY country;
						Models.Entity.STATE state;

						DeepBlue.Models.Deal.ImportExcelError error;


						EmailAttribute emailValidation = new EmailAttribute();
						ZipAttribute zipAttribute = new ZipAttribute();
						WebAddressAttribute webAttribute = new WebAddressAttribute();
						IEnumerable<ErrorInfo> errorInfo;

						StringBuilder rowErrors;
						foreach (DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"), out rowNumber);

							error = new DeepBlue.Models.Deal.ImportExcelError { RowNumber = rowNumber };
							rowErrors = new StringBuilder();

							investorName = row.GetValue(model.InvestorName);
							displayName = row.GetValue(model.DisplayName);
							socialSecurityID = row.GetValue(model.SocialSecurityID);
							domesticForeign = row.GetValue(model.DomesticForeign).ToLower() == "domestic";
							stateOfResidencyName = row.GetValue(model.StateOfResidency);
							entityType = row.GetValue(model.EntityType);
							source = row.GetValue(model.Source);
							bool.TryParse(row.GetValue(model.FOIA), out foia);
							bool.TryParse(row.GetValue(model.ERISA), out erisa);
							phone = row.GetValue(model.Phone);
							fax = row.GetValue(model.Fax);
							email = row.GetValue(model.Email);
							webAddress = row.GetValue(model.WebAddress);
							address1 = row.GetValue(model.Address1);
							address2 = row.GetValue(model.Address2);
							city = row.GetValue(model.City);
							stateName = row.GetValue(model.State);
							zip = row.GetValue(model.Zip);
							countryName = row.GetValue(model.Country);

							investorEntityType = null;
							stateOfResidency = null;
							state = null;
							country = null;

							DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(investorName);

							if (investor != null) {
								error.Errors.Add(new ErrorInfo(model.InvestorName, "Investor already exist"));
							}
							else {

								investor = new Models.Entity.Investor();

								if (string.IsNullOrEmpty(entityType) == false) {
									investorEntityType = AdminRepository.FindInvestorEntityType(entityType);
								}

								if (string.IsNullOrEmpty(stateOfResidencyName) == false) {
									stateOfResidency = AdminRepository.FindState(stateOfResidencyName);
								}

								if (string.IsNullOrEmpty(countryName) == false) {
									country = AdminRepository.FindCountry(countryName);
								}

								if (string.IsNullOrEmpty(stateName) == false) {
									state = AdminRepository.FindState(stateName);
								}

								investor.Alias = displayName;
								investor.IsDomestic = domesticForeign;
								investor.InvestorEntityTypeID = (investorEntityType != null ? investorEntityType.InvestorEntityTypeID : 0);
								investor.InvestorName = investorName;
								investor.FirstName = displayName;
								investor.ResidencyState = (stateOfResidency != null ? stateOfResidency.StateID : 0);
								investor.Social = socialSecurityID;
								investor.Notes = notes;

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
								investor.Source = source;
								investor.ERISA = erisa;
								investor.FOIA = foia;

								// Attempt to create new investor address.
								InvestorAddress investorAddress = new InvestorAddress();
								investorAddress.CreatedBy = Authentication.CurrentUser.UserID;
								investorAddress.CreatedDate = DateTime.Now;
								investorAddress.EntityID = Authentication.CurrentEntity.EntityID;
								investorAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
								investorAddress.LastUpdatedDate = DateTime.Now;

								investorAddress.Address = new Address();
								investorAddress.Address.Address1 = address1;
								investorAddress.Address.Address2 = address2;
								investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
								investorAddress.Address.City = city;
								investorAddress.Address.Country = (country != null ? country.CountryID : 0);
								investorAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
								investorAddress.Address.CreatedDate = DateTime.Now;
								investorAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
								investorAddress.Address.LastUpdatedDate = DateTime.Now;
								investorAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
								investorAddress.Address.PostalCode = zip;
								investorAddress.Address.State = (state != null ? state.StateID : 0);

								if (string.IsNullOrEmpty(investorAddress.Address.Address1) == false
									|| string.IsNullOrEmpty(investorAddress.Address.Address2) == false
									|| string.IsNullOrEmpty(investorAddress.Address.City) == false
									|| string.IsNullOrEmpty(investorAddress.Address.PostalCode) == false
									|| string.IsNullOrEmpty(investorAddress.Address.County) == false
									|| string.IsNullOrEmpty(phone) == false
									|| string.IsNullOrEmpty(email) == false
									|| string.IsNullOrEmpty(webAddress) == false
									|| string.IsNullOrEmpty(fax) == false
									) {

									errorInfo = ValidationHelper.Validate(investorAddress.Address);
									if (errorInfo.Any()) {
										error.Errors.Add(new ErrorInfo(model.Address1, ValidationHelper.GetErrorInfo(errorInfo)));
									}
									if (emailValidation.IsValid(email) == false)
										error.Errors.Add(new ErrorInfo(model.Email, "Invalid Email"));
									if (zipAttribute.IsValid(zip) == false)
										error.Errors.Add(new ErrorInfo(model.Email, "Invalid Zip"));
									if (webAttribute.IsValid(webAddress) == false)
										error.Errors.Add(new ErrorInfo(model.Email, "Invalid  Web Address"));

									if (error.Errors.Count() == 0) {
										/* Add New Investor Address */
										investor.InvestorAddresses.Add(investorAddress);
										/* Investor Communication Values */
										AddCommunication(investor, Models.Admin.Enums.CommunicationType.HomePhone, phone);
										AddCommunication(investor, Models.Admin.Enums.CommunicationType.Email, email);
										AddCommunication(investor, Models.Admin.Enums.CommunicationType.WebAddress, webAddress);
										AddCommunication(investor, Models.Admin.Enums.CommunicationType.Fax, fax);
									}
								}

								if (error.Errors.Count() == 0) {
									errorInfo = InvestorRepository.SaveInvestor(investor);
									if (errorInfo != null) {
										error.Errors.Add(new ErrorInfo(model.InvestorName, ValidationHelper.GetErrorInfo(errorInfo)));
									}
								}
							}

							StringBuilder sberror = new StringBuilder();
							foreach (var e in error.Errors) {
								sberror.AppendFormat("{0},", e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber - 1, sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if (errors != null) {
					succssRows = errors.Where(e => e.Errors.Count == 0).Count();
					errorRows = errors.Where(e => e.Errors.Count > 0).Count();
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
			return Json(new {
				Result = resultModel.Result,
				TotalRows = totalRows,
				CompletedRows = completedRows,
				TotalPages = totalPages,
				PageIndex = model.PageIndex,
				SuccessRows = succssRows,
				ErrorRows = errorRows
			});
		}

		#endregion

		#region ImportInvestorBankExcel

		[HttpPost]
		public ActionResult ImportInvestorBankExcel(FormCollection collection) {
			ImportInvestorBankExcelModel model = new ImportInvestorBankExcelModel();
			ResultModel resultModel = new ResultModel();
			MemoryCacheManager cacheManager = new MemoryCacheManager();
			int totalPages = 0;
			int totalRows = 0;
			int completedRows = 0;
			int? succssRows = 0;
			int? errorRows = 0;
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				string key = string.Format(EXCELINVESTORBANKERROR_BY_KEY, model.SessionKey);
				List<DeepBlue.Models.Deal.ImportExcelError> errors = cacheManager.Get(key, () => {
					return new List<DeepBlue.Models.Deal.ImportExcelError>();
				});
				DataSet ds = ExcelConnection.ImportExcelDataset(model.SessionKey);
				if (ds != null) {
					PagingDataTable importExcelTable = null;
					if (ds.Tables[model.InvestorBankTableName] != null) {
						importExcelTable = (PagingDataTable)ds.Tables[model.InvestorBankTableName];
					}
					if (importExcelTable != null) {
						importExcelTable.PageSize = model.PageSize;
						PagingDataTable table = importExcelTable.Skip(model.PageIndex);
						totalPages = importExcelTable.TotalPages;
						totalRows = importExcelTable.TotalRows;
						if (totalPages > model.PageIndex) {
							completedRows = (model.PageIndex * importExcelTable.PageSize);
						}
						else {
							completedRows = totalRows;
						}

						int rowNumber = 0;

						string investorName = string.Empty;
						string bankName = string.Empty;
						int abaNumber = 0;
						string accountName = string.Empty;
						string accountNumber = string.Empty;
						string ffcName = string.Empty;
						string ffcNumber = string.Empty;
						string reference = string.Empty;
						string swift = string.Empty;
						string iban = string.Empty;
						string phone = string.Empty;
						string fax = string.Empty;

						DeepBlue.Models.Deal.ImportExcelError error;


						EmailAttribute emailValidation = new EmailAttribute();
						ZipAttribute zipAttribute = new ZipAttribute();
						WebAddressAttribute webAttribute = new WebAddressAttribute();
						IEnumerable<ErrorInfo> errorInfo;

						StringBuilder rowErrors;
						foreach (DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"), out rowNumber);

							error = new DeepBlue.Models.Deal.ImportExcelError { RowNumber = rowNumber };
							rowErrors = new StringBuilder();

							investorName = row.GetValue(model.InvestorName);
							bankName = row.GetValue(model.BankName);
							int.TryParse(row.GetValue(model.ABANumber), out abaNumber);
							accountName = row.GetValue(model.AccountName);
							accountNumber = row.GetValue(model.AccountNumber);
							ffcName = row.GetValue(model.FFCName);
							ffcNumber = row.GetValue(model.FFCNumber);
							reference = row.GetValue(model.Reference);
							swift = row.GetValue(model.Swift);
							iban = row.GetValue(model.IBAN);
							phone = row.GetValue(model.Phone);
							fax = row.GetValue(model.Fax);

							DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(investorName);

							if (investor == null) {
								error.Errors.Add(new ErrorInfo(model.InvestorName, "Investor does not exist"));
							}
							else {

								// Attempt to create new investor account.
								InvestorAccount investorAccount = InvestorRepository.FindInvestorAccount(
										investor.InvestorID,
										bankName,
										abaNumber,
										accountName,
										accountNumber,
										ffcName,
										ffcNumber,
										reference,
										swift,
										iban,
										phone,
										fax
									);
								if (investorAccount == null) {
									investorAccount = new InvestorAccount();
									investorAccount.CreatedBy = Authentication.CurrentUser.UserID;
									investorAccount.CreatedDate = DateTime.Now;
								}
								else {
									error.Errors.Add(new ErrorInfo(model.InvestorName, "Investor Bank Account Information already exist"));
								}

								if (error.Errors.Count() == 0) {
									investorAccount.Comments = string.Empty;
									investorAccount.EntityID = Authentication.CurrentEntity.EntityID;
									investorAccount.IsPrimary = false;
									investorAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
									investorAccount.LastUpdatedDate = DateTime.Now;
									investorAccount.Routing = abaNumber;
									investorAccount.Reference = reference;
									investorAccount.FFC = ffcName;
									investorAccount.FFCNumber = ffcNumber;
									investorAccount.IBAN = iban;
									investorAccount.SWIFT = swift;
									investorAccount.Account = accountName;
									investorAccount.AccountNumberCash = accountNumber;
									investorAccount.BankName = bankName;
									investorAccount.Phone = phone;
									investorAccount.Fax = fax;
									investorAccount.InvestorID = investor.InvestorID;

									if (error.Errors.Count() == 0) {
										errorInfo = InvestorRepository.SaveInvestorAccount(investorAccount);
										if (errorInfo != null) {
											error.Errors.Add(new ErrorInfo(model.InvestorName, ValidationHelper.GetErrorInfo(errorInfo)));
										}
									}
								}
							}

							StringBuilder sberror = new StringBuilder();
							foreach (var e in error.Errors) {
								sberror.AppendFormat("{0},", e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber - 1, sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if (errors != null) {
					succssRows = errors.Where(e => e.Errors.Count == 0).Count();
					errorRows = errors.Where(e => e.Errors.Count > 0).Count();
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
			return Json(new {
				Result = resultModel.Result,
				TotalRows = totalRows,
				CompletedRows = completedRows,
				TotalPages = totalPages,
				PageIndex = model.PageIndex,
				SuccessRows = succssRows,
				ErrorRows = errorRows
			});
		}

		#endregion

		#region ImportInvestorContactExcel

		[HttpPost]
		public ActionResult ImportInvestorContactExcel(FormCollection collection) {
			ImportInvestorContactExcelModel model = new ImportInvestorContactExcelModel();
			ResultModel resultModel = new ResultModel();
			MemoryCacheManager cacheManager = new MemoryCacheManager();
			int totalPages = 0;
			int totalRows = 0;
			int completedRows = 0;
			int? succssRows = 0;
			int? errorRows = 0;
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				string key = string.Format(EXCELINVESTORCONTACTERROR_BY_KEY, model.SessionKey);
				List<DeepBlue.Models.Deal.ImportExcelError> errors = cacheManager.Get(key, () => {
					return new List<DeepBlue.Models.Deal.ImportExcelError>();
				});
				DataSet ds = ExcelConnection.ImportExcelDataset(model.SessionKey);
				if (ds != null) {
					PagingDataTable importExcelTable = null;
					if (ds.Tables[model.InvestorContactTableName] != null) {
						importExcelTable = (PagingDataTable)ds.Tables[model.InvestorContactTableName];
					}
					if (importExcelTable != null) {
						importExcelTable.PageSize = model.PageSize;
						PagingDataTable table = importExcelTable.Skip(model.PageIndex);
						totalPages = importExcelTable.TotalPages;
						totalRows = importExcelTable.TotalRows;
						if (totalPages > model.PageIndex) {
							completedRows = (model.PageIndex * importExcelTable.PageSize);
						}
						else {
							completedRows = totalRows;
						}

						int rowNumber = 0;

						string investorName = string.Empty;
						string contactPerson = string.Empty;
						string designation = string.Empty;
						string telephone = string.Empty;
						string fax = string.Empty;
						string email = string.Empty;
						string webAddress = string.Empty;
						string address = string.Empty;
						string city = string.Empty;
						string stateName = string.Empty;
						string zip = string.Empty;
						string countryName = string.Empty;
						bool receivesDistributionCapitalCallNotices = false;
						bool financials = false;
						bool k1 = false;
						bool investorLetters = false;

						COUNTRY country = null;
						STATE state = null;

						DeepBlue.Models.Deal.ImportExcelError error;

						DeepBlue.Models.Entity.Investor investor;
						EmailAttribute emailValidation = new EmailAttribute();
						ZipAttribute zipAttribute = new ZipAttribute();
						WebAddressAttribute webAttribute = new WebAddressAttribute();
						IEnumerable<ErrorInfo> errorInfo;

						StringBuilder rowErrors;
						foreach (DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"), out rowNumber);

							error = new DeepBlue.Models.Deal.ImportExcelError { RowNumber = rowNumber };
							rowErrors = new StringBuilder();

							investorName = row.GetValue(model.InvestorName);
							contactPerson = row.GetValue(model.ContactPerson);
							designation = row.GetValue(model.Designation);
							telephone = row.GetValue(model.Telephone);
							fax = row.GetValue(model.Fax);
							email = row.GetValue(model.Email);
							webAddress = row.GetValue(model.WebAddress);
							address = row.GetValue(model.Address);
							city = row.GetValue(model.City);
							stateName = row.GetValue(model.State);
							zip = row.GetValue(model.Zip);
							countryName = row.GetValue(model.Country);
							bool.TryParse(row.GetValue(model.ReceivesDistributionCapitalCallNotices), out receivesDistributionCapitalCallNotices);
							bool.TryParse(row.GetValue(model.Financials), out financials);
							bool.TryParse(row.GetValue(model.InvestorLetters), out investorLetters);
							bool.TryParse(row.GetValue(model.K1), out k1);
							investor = null;
							country = null;
							state = null;

							if (string.IsNullOrEmpty(investorName) == false) {
								investor = InvestorRepository.FindInvestor(investorName);
							}

							if (investor == null) {
								error.Errors.Add(new ErrorInfo(model.InvestorName, "Investor does not exist"));
							}
							else {

								if (string.IsNullOrEmpty(countryName) == false) {
									country = AdminRepository.FindCountry(countryName);
								}

								if (string.IsNullOrEmpty(stateName) == false) {
									state = AdminRepository.FindState(stateName);
								}

								// Attempt to create new investor contact.
								InvestorContact investorContact = InvestorRepository.FindInvestorContact(investor.InvestorID,
									  contactPerson,
									  designation,
									  receivesDistributionCapitalCallNotices,
									  financials,
									  k1,
									  investorLetters
									 );

								if (investorContact == null) {
									investorContact = new InvestorContact();
									investorContact.CreatedBy = Authentication.CurrentUser.UserID;
									investorContact.CreatedDate = DateTime.Now;
								}
								else {
									error.Errors.Add(new ErrorInfo(model.InvestorName, "Investor contact already exist"));
								}

								if (error.Errors.Count() == 0) {

									investorContact.InvestorID = investor.InvestorID;
									investorContact.EntityID = Authentication.CurrentEntity.EntityID;
									investorContact.LastUpdatedBy = Authentication.CurrentUser.UserID;
									investorContact.LastUpdatedDate = DateTime.Now;

									investorContact.Contact = new Contact();
									investorContact.Contact.CreatedBy = Authentication.CurrentUser.UserID;
									investorContact.Contact.CreatedDate = DateTime.Now;

									investorContact.Contact.ContactName = contactPerson;
									investorContact.Contact.FirstName = "n/a";
									investorContact.Contact.LastName = "n/a";
									investorContact.Contact.LastUpdatedBy = Authentication.CurrentUser.UserID;
									investorContact.Contact.LastUpdatedDate = DateTime.Now;
									investorContact.Contact.ReceivesDistributionNotices = receivesDistributionCapitalCallNotices;
									investorContact.Contact.ReceivesFinancials = financials;
									investorContact.Contact.ReceivesInvestorLetters = investorLetters;
									investorContact.Contact.ReceivesK1 = k1;
									investorContact.Contact.Designation = designation;
									investorContact.Contact.EntityID = Authentication.CurrentEntity.EntityID;

									// Attempt to create new investor contact address.
									ContactAddress contactAddress = null;

									contactAddress = new ContactAddress();
									contactAddress.CreatedBy = Authentication.CurrentUser.UserID;
									contactAddress.CreatedDate = DateTime.Now;
									contactAddress.EntityID = Authentication.CurrentEntity.EntityID;
									contactAddress.LastUpdatedBy = Authentication.CurrentUser.UserID;
									contactAddress.LastUpdatedDate = DateTime.Now;

									contactAddress.Address = new Address();
									contactAddress.Address.CreatedBy = Authentication.CurrentUser.UserID;
									contactAddress.Address.CreatedDate = DateTime.Now;
									contactAddress.Address.Address1 = address;
									contactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
									contactAddress.Address.City = city;
									contactAddress.Address.Country = (country != null ? country.CountryID : 0);
									contactAddress.Address.EntityID = Authentication.CurrentEntity.EntityID;
									contactAddress.Address.LastUpdatedBy = Authentication.CurrentUser.UserID;
									contactAddress.Address.LastUpdatedDate = DateTime.Now;
									contactAddress.Address.PostalCode = zip;
									contactAddress.Address.State = (state != null ? state.StateID : 0);

									/* Add Investor Contact Communication Values */


									if (string.IsNullOrEmpty(contactAddress.Address.Address1) == false
									   || string.IsNullOrEmpty(contactAddress.Address.Address2) == false
									   || string.IsNullOrEmpty(contactAddress.Address.City) == false
										|| string.IsNullOrEmpty(contactAddress.Address.PostalCode) == false
										|| string.IsNullOrEmpty(investorContact.Contact.ContactName) == false
										|| string.IsNullOrEmpty(fax) == false
										|| string.IsNullOrEmpty(email) == false
										|| string.IsNullOrEmpty(webAddress) == false
									 ) {
										errorInfo = ValidationHelper.Validate(contactAddress.Address);
										errorInfo = errorInfo.Union(ValidationHelper.Validate(investorContact.Contact));

										if (errorInfo.Any()) {
											error.Errors.Add(new ErrorInfo(model.InvestorName, ValidationHelper.GetErrorInfo(errorInfo)));
										}
										if (emailValidation.IsValid(email) == false)
											error.Errors.Add(new ErrorInfo(model.Email, "Invalid Email"));
										if (webAttribute.IsValid(webAddress) == false)
											error.Errors.Add(new ErrorInfo(model.WebAddress, "Invalid Web Address"));
										if (zipAttribute.IsValid(contactAddress.Address.PostalCode) == false)
											error.Errors.Add(new ErrorInfo(model.Zip, "Invalid Zip"));


										if (error.Errors.Count() == 0) {
											investorContact.Contact.ContactAddresses.Add(contactAddress);
											AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.HomePhone, telephone);
											AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Fax, fax);
											AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.Email, email);
											AddCommunication(investorContact.Contact, Models.Admin.Enums.CommunicationType.WebAddress, webAddress);
										}
									}
									
									if (error.Errors.Count() == 0) {
										errorInfo = InvestorRepository.SaveInvestorContact(investorContact);
										if (errorInfo != null) {
											error.Errors.Add(new ErrorInfo(model.InvestorName, ValidationHelper.GetErrorInfo(errorInfo)));
										}
									}
								}
							}

							StringBuilder sberror = new StringBuilder();
							foreach (var e in error.Errors) {
								sberror.AppendFormat("{0},", e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber - 1, sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if (errors != null) {
					succssRows = errors.Where(e => e.Errors.Count == 0).Count();
					errorRows = errors.Where(e => e.Errors.Count > 0).Count();
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
			return Json(new {
				Result = resultModel.Result,
				TotalRows = totalRows,
				CompletedRows = completedRows,
				TotalPages = totalPages,
				PageIndex = model.PageIndex,
				SuccessRows = succssRows,
				ErrorRows = errorRows
			});
		}

		#endregion

		#endregion

		public ActionResult GetImportErrorExcel(string sessionKey, string tableName) {
			MemoryCacheManager cacheManager = new MemoryCacheManager();
			ActionResult result = null;
			DataSet ds = ExcelConnection.ImportExcelDataset(sessionKey);
			if (ds != null) {
				PagingDataTable importExcelTable = null;
				if (ds.Tables[tableName] != null) {
					importExcelTable = (PagingDataTable)ds.Tables[tableName];
				}
				PagingDataTable errorTable = (PagingDataTable)importExcelTable.Clone();
				DataRow[] rows = importExcelTable.Select("ImportError<>''");
				foreach (var row in rows) {
					errorTable.ImportRow(row);
				}
				//if (importExcelTable != null) {
					result = new ExportExcel { TableName = string.Format("{0}_{1}", tableName, sessionKey), GridData = errorTable };
				//}
			}
			return result;
		}

		private string AppendErrorInfo(int rowNumber, DataTable excelDataTable, List<DeepBlue.Models.Deal.ImportExcelError> errors) {
			StringBuilder errorInfo = new StringBuilder();
			errorInfo.Append("<table cellpadding=0 cellspacing=0 border=0>");
			errorInfo.Append("<thead>");
			errorInfo.Append("<tr>");
			errorInfo.AppendFormat("<th>{0}</th>", "RowNumber");
			foreach (DataColumn col in excelDataTable.Columns) {
				errorInfo.AppendFormat("<th>{0}</th>", col.ColumnName);
			}
			errorInfo.Append("</tr>");
			errorInfo.Append("</thead>");
			errorInfo.Append("</tbody>");
			if (errors.Count() > 0) {
				foreach (var err in errors) {
					if (err.Errors.Count() > 0) {
						errorInfo.Append("<tr>");
						errorInfo.AppendFormat("<td>{0}</td>", err.RowNumber);
						foreach (DataColumn col in excelDataTable.Columns) {
							var errorColumn = err.Errors.Where(e => e.PropertyName == col.ColumnName).FirstOrDefault();
							if (errorColumn != null) {
								errorInfo.AppendFormat("<td>{0}</td>", errorColumn.ErrorMessage);
							}
							else {
								errorInfo.AppendFormat("<td>{0}</td>", string.Empty);
							}
						}
						errorInfo.Append("</tr>");
					}
				}
			}
			errorInfo.Append("</tbody>");
			errorInfo.Append("</table>");
			return errorInfo.ToString();
		}
	}
}
