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

namespace DeepBlue.Controllers.Investor {

	public class InvestorController : Controller {

		public IInvestorRepository InvestorRepository { get; set; }

		public InvestorController()
			: this(new InvestorRepository()) {
		}

		public InvestorController(IInvestorRepository repository) {
			InvestorRepository = repository;
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
			ViewData["ShowRightPanelSearch"] = true;
			CreateModel model = new CreateModel();
			model.SelectList.States = SelectListFactory.GetStateSelectList(InvestorRepository.GetAllStates());
			model.SelectList.Countries = SelectListFactory.GetCountrySelectList(InvestorRepository.GetAllCountries());
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(InvestorRepository.GetAllInvestorEntityTypes());
			model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(InvestorRepository.GetAllAddressTypes());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.SelectList.Source = SelectListFactory.GetSourceList();
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
				investor.CreatedBy = 0;
				investor.CreatedDate = DateTime.Now;
				investor.EntityID = BaseController.CurrentEntityID;
				investor.FirstName = string.Empty;
				investor.IsDomestic = model.DomesticForeign;
				investor.LastName = string.Empty;
				investor.LastUpdatedBy = 0;
				investor.LastUpdatedDate = DateTime.Now;
				investor.ManagerName = string.Empty;
				investor.InvestorEntityTypeID = model.EntityType;
				investor.InvestorName = model.InvestorName;
				investor.FirstName = model.Alias;
				investor.MiddleName = string.Empty;
				investor.Notes = string.Empty;
				investor.PrevInvestorID = 0;
				investor.ResidencyState = model.StateOfResidency;
				investor.Social = model.SocialSecurityTaxId;
				investor.TaxExempt = false;
				investor.TaxID = 0;
				investor.Notes = model.Notes;

				/* Investor Address */
				InvestorAddress investorAddress = new InvestorAddress();
				investorAddress.CreatedBy = 0;
				investorAddress.CreatedDate = DateTime.Now;
				investorAddress.EntityID = BaseController.CurrentEntityID;
				investorAddress.LastUpdatedBy = 0;
				investorAddress.LastUpdatedDate = DateTime.Now;

				investorAddress.Address = new Address();
				investorAddress.Address.Address1 = model.Address1;
				investorAddress.Address.Address2 = model.Address2;
				investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
				investorAddress.Address.City = model.City;
				investorAddress.Address.Country = model.Country;
				investorAddress.Address.CreatedDate = DateTime.Now;
				investorAddress.Address.City = model.City;
				investorAddress.Address.Country = model.Country;
				investorAddress.Address.CreatedBy = 0;
				investorAddress.Address.CreatedDate = DateTime.Now;
				investorAddress.Address.EntityID = BaseController.CurrentEntityID;
				investorAddress.Address.IsPreferred = false;
				investorAddress.Address.LastUpdatedDate = DateTime.Now;
				investorAddress.Address.LastUpdatedBy = 0;
				investorAddress.Address.LastUpdatedDate = DateTime.Now;
				investorAddress.Address.Listed = false;
				investorAddress.Address.PostalCode = model.Zip;
				investorAddress.Address.State = model.State;
				investorAddress.Address.StProvince = string.Empty;
				/* Add New Investor Address */
				investor.InvestorAddresses.Add(investorAddress);

				/* Bank Account */
				InvestorAccount investorAccount;
				for (int index = 0; index < model.AccountLength; index++) {
					if (collection[(index + 1).ToString() + "_" + "AccountNumber"] != null) {
						investorAccount = new InvestorAccount();
						investorAccount.Account = collection[(index + 1).ToString() + "_" + "AccountNumber"];
						investorAccount.Attention = collection[(index + 1).ToString() + "_" + "Attention"];
						investorAccount.Comments = string.Empty;
						investorAccount.CreatedBy = 0;
						investorAccount.CreatedDate = DateTime.Now;
						investorAccount.EntityID = BaseController.CurrentEntityID;
						investorAccount.IsPrimary = false;
						investorAccount.LastUpdatedBy = 0;
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
					if (collection[(index + 1).ToString() + "_" + "ContactAddress1"] != null) {
						investorContact = new InvestorContact();
						investorContact.CreatedBy = 0;
						investorContact.CreatedDate = DateTime.Now;
						investorContact.EntityID = BaseController.CurrentEntityID;
						investorContact.LastUpdatedBy = 0;
						investorContact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact = new Contact();
						investorContact.Contact.ContactName = collection[(index + 1).ToString() + "_" + "ContactPerson"];
						investorContact.Contact.ContactType = string.Empty;
						investorContact.Contact.CreatedBy = 0;
						investorContact.Contact.CreatedDate = DateTime.Now;
						investorContact.Contact.FirstName = string.Empty;
						investorContact.Contact.LastName = string.Empty;
						investorContact.Contact.LastUpdatedBy = 0;
						investorContact.Contact.LastUpdatedDate = DateTime.Now;
						investorContact.Contact.MiddleName = string.Empty;
						investorContact.Contact.ReceivesDistributionNotices = collection[(index + 1).ToString() + "_" + "DistributionNotices"].Contains("true");
						investorContact.Contact.ReceivesFinancials = collection[(index + 1).ToString() + "_" + "Financials"].Contains("true");
						investorContact.Contact.ReceivesInvestorLetters = collection[(index + 1).ToString() + "_" + "InvestorLetters"].Contains("true");
						investorContact.Contact.ReceivesK1 = collection[(index + 1).ToString() + "_" + "K1"].Contains("true");

						contactAddress = new ContactAddress();
						contactAddress.CreatedBy = 0;
						contactAddress.CreatedDate = DateTime.Now;
						contactAddress.EntityID = BaseController.CurrentEntityID;
						contactAddress.LastUpdatedBy = 0;
						contactAddress.LastUpdatedDate = DateTime.Now;
						contactAddress.Address = new Address();
						contactAddress.Address.Address1 = collection[(index + 1).ToString() + "_" + "ContactAddress1"];
						contactAddress.Address.Address2 = collection[(index + 1).ToString() + "_" + "ContactAddress2"];
						contactAddress.Address.Address3 = string.Empty;
						contactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Investor.Enums.AddressType.Work;
						contactAddress.Address.City = collection[(index + 1).ToString() + "_" + "ContactCity"];
						contactAddress.Address.Country = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]);
						contactAddress.Address.County = string.Empty;
						contactAddress.Address.CreatedBy = 0;
						contactAddress.Address.CreatedDate = DateTime.Now;
						contactAddress.Address.EntityID = BaseController.CurrentEntityID;
						contactAddress.Address.LastUpdatedBy = 0;
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
				return RedirectToAction("ThankYou", "Investor");
			} else {
				ViewData["MenuName"] = "Investor";
				model.SelectList.States = SelectListFactory.GetStateSelectList(InvestorRepository.GetAllStates());
				model.SelectList.Countries = SelectListFactory.GetCountrySelectList(InvestorRepository.GetAllCountries());
				model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(InvestorRepository.GetAllInvestorEntityTypes());
				model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(InvestorRepository.GetAllAddressTypes());
				model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
				model.SelectList.Source = SelectListFactory.GetSourceList();
				return View(model);
			}
		}

		//
		// GET: /Investor/Edit/5
		[HttpGet]
		public ActionResult Edit(int id) {
			EditModel model = new EditModel();
			ViewData["MenuName"] = "Investor";
			model.id = id;
			model.SelectList.States = SelectListFactory.GetStateSelectList(InvestorRepository.GetAllStates());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.SelectList.Countries = SelectListFactory.GetCountrySelectList(InvestorRepository.GetAllCountries());
			model.SelectList.InvestorEntityTypes = SelectListFactory.GetInvestorEntityTypesSelectList(InvestorRepository.GetAllInvestorEntityTypes());
			model.ContactInformations = new List<ContactInformation>();
			model.AccountInformations = new List<AccountInformation>();
			return View(model);
		}

		//
		// POST: /Investor/Update
		[HttpPost]
		public bool Update(FormCollection collection) {
			int index = 0;
			int addressCount = Convert.ToInt32(collection["AddressInfoCount"]);
			int contactAddressCount = Convert.ToInt32(collection["ContactInfoCount"]);
			int accountCount = Convert.ToInt32(collection["AccountInfoCount"]);
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(Convert.ToInt32(collection["InvestorId"]));
			InvestorContact investorContact;
			ContactAddress investorContactAddress;
			InvestorAccount investorAccount;

			investor.IsDomestic = Convert.ToBoolean(collection["DomesticForeigns"]);
			if (string.IsNullOrEmpty(collection["StateOfResidency"]) == false)
				investor.ResidencyState = Convert.ToInt32(collection["StateOfResidency"]);
			if (string.IsNullOrEmpty(collection["EntityType"]) == false)
				investor.InvestorEntityTypeID = Convert.ToInt32(collection["EntityType"]);

			// Assign address details
			for (index = 1; index < addressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AddressId"]) == false) {
					InvestorAddress investorAddress = investor.InvestorAddresses.Single(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "AddressId"]));
					if (investorAddress != null) {
						investorAddress.LastUpdatedDate = DateTime.Now;

						investorAddress.Address.Address1 = collection[index.ToString() + "_" + "Address1"];
						investorAddress.Address.Address2 = collection[index.ToString() + "_" + "Address2"];
						investorAddress.Address.City = collection[index.ToString() + "_" + "City"];
						investorAddress.Address.LastUpdatedDate = DateTime.Now;
						investorAddress.Address.PostalCode = collection[index.ToString() + "_" + "PostalCode"];
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "Country"]) == false)
							investorAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "Country"]);
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "State"]) == false)
							investorAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "State"]);
					}
				}
			}
			// Assign contact address details
			for (index = 1; index < contactAddressCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactId"]) == false) {
					investorContact = investor.InvestorContacts.Single(contact => contact.ContactID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactId"]));
					if (investorContact != null) {
						investorContact.LastUpdatedDate = DateTime.Now;

						// Assign contact details
						investorContact.Contact.ContactName = collection[index.ToString() + "_" + "ContactPerson"];
						investorContact.Contact.ReceivesDistributionNotices = (collection[index.ToString() + "_" + "DistributionNotices"]).Contains("true");
						investorContact.Contact.ReceivesFinancials = (collection[index.ToString() + "_" + "Financials"]).Contains("true");
						investorContact.Contact.ReceivesInvestorLetters = (collection[index.ToString() + "_" + "InvestorLetters"]).Contains("true");
						investorContact.Contact.ReceivesK1 = (collection[index.ToString() + "_" + "K1"]).Contains("true");

						investorContactAddress = investorContact.Contact.ContactAddresses.Single(address => address.AddressID == Convert.ToInt32(collection[index.ToString() + "_" + "ContactAddressId"]));
						// Assign address details
						if (investorContactAddress != null) {
							investorContactAddress.Address.Address1 = collection[index.ToString() + "_" + "ContactAddress1"];
							investorContactAddress.Address.Address2 = collection[index.ToString() + "_" + "ContactAddress2"];
							investorContactAddress.Address.City = collection[index.ToString() + "_" + "ContactCity"];
							investorContactAddress.Address.PostalCode = collection[index.ToString() + "_" + "ContactPostalCode"];
							if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactCountry"]) == false)
								investorContactAddress.Address.Country = Convert.ToInt32(collection[index.ToString() + "_" + "ContactCountry"]);
							if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ContactState"]) == false)
								investorContactAddress.Address.State = Convert.ToInt32(collection[index.ToString() + "_" + "ContactState"]);
						}
					}
				}
			}
			// Assign account details
			for (index = 1; index < accountCount + 1; index++) {
				if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "AccountId"]) == false) {
					investorAccount = investor.InvestorAccounts.Single(account => account.InvestorAccountID == Convert.ToInt32(collection[index.ToString() + "_" + "AccountId"]));
					if (investorAccount != null) {
						investorAccount.Account = collection[index.ToString() + "_" + "AccountNumber"];
						investorAccount.Attention = collection[index.ToString() + "_" + "Attention"];
						investorAccount.Reference = collection[index.ToString() + "_" + "Reference"];
					}
				}
			}
			InvestorRepository.Save();
			return true;
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
		// GET: /Investor/Edit
		public JsonResult FindInvestor(int id) {
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(id);
			EditModel model = new EditModel();
			if (investor != null) {
				AddressInformation addressInfo;
				ContactInformation contactInfo;
				AccountInformation accountInfo;
				model.InvestorName = investor.InvestorName;
				model.DisplayName = investor.FirstName;
				model.DomesticForeign = investor.IsDomestic;
				model.EntityType = investor.InvestorEntityTypeID;
				model.SocialSecurityTaxId = investor.Social;
				model.StateOfResidency = (int)(investor.ResidencyState != null ? investor.ResidencyState : 0);
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
				model.FundInformations = new FlexigridObject();
				model.FundInformations.page = 1;
				model.FundInformations.total = investor.InvestorFunds.Count();
				foreach (var fund in investor.InvestorFunds) {
					FlexigridRow row = new FlexigridRow();
					row.id = fund.FundID.ToString();
					row.cell.Add(fund.Fund.FundName.ToString());
					row.cell.Add(string.Format("{0:C}",fund.TotalCommitment));
					row.cell.Add(string.Format("{0:C}",Convert.ToDecimal(fund.UnfundedAmount)));
					InvestorType investorType = InvestorRepository.FindInvestorType((int)fund.InvestorTypeId);
					if (investorType != null)
					   row.cell.Add(investorType.InvestorTypeName);
					else
					  row.cell.Add(string.Empty);
					model.FundInformations.rows.Add(row);
				}
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}
	}
}
