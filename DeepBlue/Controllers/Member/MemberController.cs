using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models;
using DeepBlue.Helpers;
using DeepBlue.Models.Member;
using DeepBlue.Models.Entity;
using DeepBlue.Models.Member.Enums;

namespace DeepBlue.Controllers.Member {

	public class MemberController : Controller {

		public IMemberRepository MemberRepository { get; set; }

		public MemberController()
			: this(new MemberRepository()) {
		}

		public MemberController(IMemberRepository repository) {
			MemberRepository = repository;
		}

		//
		// GET: /Member/

		public ActionResult Index() {
			return View();
		}

		public ActionResult ThankYou() {
			ViewData["MenuName"] = "Member";
			return View();
		}

		//
		// GET: /Member/Details/5

		public ActionResult Details(int id) {
			return View();
		}

		//
		// GET: /Member/Create

		public ActionResult New() {
		//	ViewData["MenuName"] = "Member";
			CreateModel model = new CreateModel();
			model.SelectList.States = SelectListFactory.GetStateSelectList(MemberRepository.GetAllStates());
			model.SelectList.Countries = SelectListFactory.GetCountrySelectList(MemberRepository.GetAllCountries());
			model.SelectList.MemberEntityTypes = SelectListFactory.GetMemberEntityTypesSelectList(MemberRepository.GetAllMemberEntityTypes());
			model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(MemberRepository.GetAllAddressTypes());
			model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
			model.AddressType = (int)DeepBlue.Models.Member.Enums.AddressType.Work;
			model.ContactAddressType = (int)DeepBlue.Models.Member.Enums.AddressType.Work;
			model.DomesticForeign = true;
			model.AccountLength = 1;
			model.ContactLength = 1;
			return View(model);
		}

		//
		// POST: /Member/Create

		[HttpPost]
		public ActionResult Create(CreateModel model, FormCollection collection) {
			if (ModelState.IsValid) {
				DeepBlue.Models.Entity.Member member = new DeepBlue.Models.Entity.Member();
				/*Member*/
				member.Alias = model.Alias;
				member.CreatedBy = 0;
				member.CreatedDate = DateTime.Now;
				member.EntityID = 0;
				member.FirstName = string.Empty;
				member.IsDomestic = model.DomesticForeign;
				member.LastName = string.Empty;
				member.LastUpdatedBy = 0;
				member.LastUpdatedDate = DateTime.Now;
				member.ManagerName = string.Empty;
				member.MemberEntityTypeID = model.EntityType;
				member.MemberName = model.MemberName;
				member.MiddleName = string.Empty;
				member.Notes = string.Empty;
				member.PrevMemberID = 0;
				member.ResidencyState = model.StateOfResidency;
				member.Social = model.SocialSecurityTaxId;
				member.TaxExempt = false;
				member.TaxID = 0;
				member.Notes = model.Notes;

				/* Member Address */
				MemberAddress memberAddress = new MemberAddress();
				memberAddress.CreatedBy = 0;
				memberAddress.CreatedDate = DateTime.Now;
				memberAddress.EntityID = 0;
				memberAddress.LastUpdatedBy = 0;
				memberAddress.LastUpdatedDate = DateTime.Now;

				memberAddress.Address = new Address();
				memberAddress.Address.Address1 = model.Address1;
				memberAddress.Address.Address2 = model.Address2;
				memberAddress.Address.AddressTypeID = model.AddressType;
				memberAddress.Address.City = model.City;
				memberAddress.Address.Country = model.Country;
				memberAddress.Address.CreatedDate = DateTime.Now;
				memberAddress.Address.City = model.City;
				memberAddress.Address.Country = model.Country;
				memberAddress.Address.CreatedBy = 0;
				memberAddress.Address.CreatedDate = DateTime.Now;
				memberAddress.Address.EntityID = 0;
				memberAddress.Address.IsPreferred = false;
				memberAddress.Address.LastUpdatedDate = DateTime.Now;
				memberAddress.Address.LastUpdatedBy = 0;
				memberAddress.Address.LastUpdatedDate = DateTime.Now;
				memberAddress.Address.Listed = false;
				memberAddress.Address.PostalCode = model.Zip;
				memberAddress.Address.State = model.State;
				memberAddress.Address.StProvince = string.Empty;
				/* Add New Member Address */
				member.MemberAddresses.Add(memberAddress);

				/* Bank Account */
				MemberAccount memberAccount;
				for (int index = 0; index < model.AccountLength; index++) {
					if (collection[(index + 1).ToString() + "_" + "AccountNumber"] != null) {
						memberAccount = new MemberAccount();
						memberAccount.Account = collection[(index + 1).ToString() + "_" + "AccountNumber"];
						memberAccount.Attention = collection[(index + 1).ToString() + "_" + "Attention"];
						memberAccount.Comments = string.Empty;
						memberAccount.CreatedBy = 0;
						memberAccount.CreatedDate = DateTime.Now;
						memberAccount.EntityID = 0;
						memberAccount.IsPrimary = false;
						memberAccount.LastUpdatedBy = 0;
						memberAccount.LastUpdatedDate = DateTime.Now;
						memberAccount.Routing = 0;
						memberAccount.Reference = collection[(index + 1).ToString() + "_" + "Reference"];
						member.MemberAccounts.Add(memberAccount);
					}
				}

				/* Contact Address */
				MemberContact memberContact;
				ContactAddress contactAddress;
				for (int index = 0; index < model.ContactLength; index++) {
					if (collection[(index + 1).ToString() + "_" + "ContactAddress1"] != null) {
						memberContact = new MemberContact();
						memberContact.CreatedBy = 0;
						memberContact.CreatedDate = DateTime.Now;
						memberContact.EntityID = 0;
						memberContact.LastUpdatedBy = 0;
						memberContact.LastUpdatedDate = DateTime.Now;
						memberContact.Contact = new Contact();
						memberContact.Contact.ContactName = collection[(index + 1).ToString() + "_" + "ContactPerson"];
						memberContact.Contact.ContactType = string.Empty;
						memberContact.Contact.CreatedBy = 0;
						memberContact.Contact.CreatedDate = DateTime.Now;
						memberContact.Contact.FirstName = string.Empty;
						memberContact.Contact.LastName = string.Empty;
						memberContact.Contact.LastUpdatedBy = 0;
						memberContact.Contact.LastUpdatedDate = DateTime.Now;
						memberContact.Contact.MiddleName = string.Empty;
						memberContact.Contact.ReceivesDistributionNotices = collection[(index + 1).ToString() + "_" + "DistributionNotices"].Contains("true");
						memberContact.Contact.ReceivesFinancials = collection[(index + 1).ToString() + "_" + "Financials"].Contains("true");
						memberContact.Contact.ReceivesInvestorLetters = collection[(index + 1).ToString() + "_" + "InvestorLetters"].Contains("true");
						memberContact.Contact.ReceivesK1 = collection[(index + 1).ToString() + "_" + "K1"].Contains("true");

						contactAddress = new ContactAddress();
						contactAddress.CreatedBy = 0;
						contactAddress.CreatedDate = DateTime.Now;
						contactAddress.EntityID = 0;
						contactAddress.LastUpdatedBy = 0;
						contactAddress.LastUpdatedDate = DateTime.Now;
						contactAddress.Address = new Address();
						contactAddress.Address.Address1 = collection[(index + 1).ToString() + "_" + "ContactAddress1"];
						contactAddress.Address.Address2 = collection[(index + 1).ToString() + "_" + "ContactAddress2"];
						contactAddress.Address.Address3 = string.Empty;
						contactAddress.Address.AddressTypeID = (int)DeepBlue.Models.Member.Enums.AddressType.Work;
						contactAddress.Address.City = collection[(index + 1).ToString() + "_" + "ContactCity"];
						contactAddress.Address.Country = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactCountry"]);
						contactAddress.Address.County = string.Empty;
						contactAddress.Address.CreatedBy = 0;
						contactAddress.Address.CreatedDate = DateTime.Now;
						contactAddress.Address.EntityID = 0;
						contactAddress.Address.LastUpdatedBy = 0;
						contactAddress.Address.LastUpdatedDate = DateTime.Now;
						contactAddress.Address.Listed = false;
						contactAddress.Address.PostalCode = collection[(index + 1).ToString() + "_" + "ContactZip"];
						contactAddress.Address.State = Convert.ToInt32(collection[(index + 1).ToString() + "_" + "ContactState"]);
						contactAddress.Address.StProvince = string.Empty;

						memberContact.Contact.ContactAddresses.Add(contactAddress);
						member.MemberContacts.Add(memberContact);
					}
				}
				MemberRepository.Add(member);
				MemberRepository.Save();
				return RedirectToAction("ThankYou", "Member");
			} else {
				ViewData["MenuName"] = "Member";
				model.SelectList.States = SelectListFactory.GetStateSelectList(MemberRepository.GetAllStates());
				model.SelectList.Countries = SelectListFactory.GetCountrySelectList(MemberRepository.GetAllCountries());
				model.SelectList.MemberEntityTypes = SelectListFactory.GetMemberEntityTypesSelectList(MemberRepository.GetAllMemberEntityTypes());
				model.SelectList.AddressTypes = SelectListFactory.GetAddressTypeSelectList(MemberRepository.GetAllAddressTypes());
				model.SelectList.DomesticForeigns = SelectListFactory.GetDomesticForeignList();
				return View(model);
			}
		}
	}
}
