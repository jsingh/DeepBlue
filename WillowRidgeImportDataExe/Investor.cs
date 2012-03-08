using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using DeepBlue.Models.Investor;
using DeepBlue.Models.Entity;
using System.Collections;

using DeepBlue.ImportData.SourceData;
using DeepBlue.Models.Entity;
using System.Net;
using System.Web.Script.Serialization;
using DeepBlue.Helpers;

namespace DeepBlue.ImportData {
    class InvestorImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C7_20tblLPPaymentInstructions, Exception>> Errors = new List<KeyValuePair<C7_20tblLPPaymentInstructions, Exception>>();
        public static List<KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>>();

        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static void Create(CreateModel model, CookieCollection cookies) {
            // Investor.js
            // function name "save"
            // /Investor/Create
            string url = HttpWebRequestUtil.GetUrl("Investor/Create");
            byte[] postData = HttpWebRequestUtil.GetPostData(model, string.Empty, string.Empty);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);

        }

        public static void ConvertInvestorViaWeb() {
            List<CreateModel> models = new List<CreateModel>();
            using (BlueEntities context = new BlueEntities()) {
                List<C7_20tblLPPaymentInstructions> investors = context.C7_20tblLPPaymentInstructions.ToList();
                foreach (C7_20tblLPPaymentInstructions investor in investors) {
                    CreateModel model = GetCreateModelFromBlue(investor);

                    #region Investor Account
                    // Currently in blue, each investor has only one account
                    model.AccountLength = 1; // This is used as a prefix for each InvestorAccount passed in the form
                    //The following Key is used to determine if a particular Account has been deleted or not
                    // index_BankIndex
                    // index is 1 based
                    InvestorAccount account = new InvestorAccount();
                    // Server looks for index_ABANumber
                    account.Routing = Convert.ToInt32(investor.ABANumber);
                    // Server looks for index_AccountNumber
                    account.Account = investor.AccountNumber;
                    account.AccountOf = investor.Accountof;
                    account.Attention = investor.Attn;
                    account.BankName = investor.Bank;
                    account.Reference = investor.Reference;
                    // WARNING: The following values are present in our database, but not present in Blue, so setting those to NULL
                    // FFC
                    // FFCNO
                    // IBAN
                    // ByOrderOf
                    // Swift
                    #endregion

                    #region Contact Info
                    //The following Key is used to determine if a particular Contact has been deleted or not
                    // index_ContactIndex
                    model.ContactLength = 0;
                    foreach (C7_25LPContactinfo contactInfo in investor.C7_25LPContactinfo) {
                        model.ContactLength++;
                        Contact contact = new Contact();
                        contact.ContactCompany = contactInfo.ContactCompany;
                        contact.ContactName = contactInfo.ContactName;
                        // WARNING: Deepblue has consolidated CallNotices/Distribution notices into one field.
                        if (contactInfo.DistributionNotices != null) {
                            contact.ReceivesDistributionNotices = contactInfo.DistributionNotices.Value;
                        }
                        if (contactInfo.Financials != null) {
                            contact.ReceivesFinancials = contactInfo.Financials.Value;
                        }
                        if (contactInfo.InvestorLetters != null) {
                            contact.ReceivesInvestorLetters = contactInfo.InvestorLetters.Value;
                        }

                        // WARNING: We dont have the following values in our database
                        // contactInfo.Dear; // This seems to be the first name from Contact Name
                        Address contactAddress = new Address();
                        contactAddress.Address1 = contactInfo.ContactAddress;
                        //contactInfo.Comments;
                        contactAddress.Address2 = contactInfo.ContactAddress2;
                        // Contact Info(Access) doesnt have the values for these properties, so using default values
                        contactAddress.Country = Globals.DefaultCountryID;
                        try {
                            string[] parts = new string[3];
                            if (ParseAddress(contactInfo.ContactAddress2, out parts)) {
                                contactAddress.City = parts[0];
                                contactAddress.PostalCode = parts[2];
                                contactAddress.State = Globals.States.Where(x => x.Abbr == parts[1].ToUpper().Trim()).First().StateID;
                            }
                        }
                        catch {
                            contactAddress.City = Globals.DefaultCity;
                            contactAddress.State = Globals.DefaultStateID;
                            contactAddress.PostalCode = Globals.DefaultZip;
                        }

                        AddCommunication(contact, Models.Admin.Enums.CommunicationType.Email, contactInfo.ContactEmail);
                        AddCommunication(contact, Models.Admin.Enums.CommunicationType.HomePhone, contactInfo.ContactPhone);
                        AddCommunication(contact, Models.Admin.Enums.CommunicationType.Fax, contactInfo.ContactFax);
                    }
                    #endregion
                }
            }
        }

        public static NameValueCollection ImportInvestors(CookieCollection cookies) {
            NameValueCollection values = new NameValueCollection();
            ImportErrors = new List<KeyValuePair<Models.Entity.Investor, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<DeepBlue.Models.Entity.Investor> dbInvestors = ConvertBlueToDeepBlue();
            LogErrors(Errors);
            foreach (DeepBlue.Models.Entity.Investor investor in dbInvestors) {
                // make sure that the investor doesnt already exist
                List<Models.Entity.Investor> existingInvestors = GetInvestors(cookies, null, investor.InvestorName);
                if (existingInvestors.Count > 0) {
                    //make sure the name match exactly
                    Models.Entity.Investor inv = existingInvestors.Where(x => x.InvestorName.Equals(investor.InvestorName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (inv != null) {
                        // investor already exists
                        Util.WriteWarning(string.Format("Investor: {0} already exists. Found investor: Investor Name: {1}, InvestorID: {2}", investor.InvestorName, inv.InvestorName, inv.InvestorID));
                        continue;
                    }
                }

                NameValueCollection formValues = new NameValueCollection();
                TotalImportRecords++;
                try {
                    CreateModel model = new CreateModel();
                    model.Alias = investor.Alias;
                    model.DomesticForeign = investor.IsDomestic;
                    model.EntityType = investor.InvestorEntityTypeID;
                    model.InvestorName = investor.InvestorName;
                    model.Alias = investor.FirstName;
                    model.StateOfResidency = investor.ResidencyState.Value;
                    model.SocialSecurityTaxId = investor.Social;
                    model.Notes = investor.Notes;
					
                    #region Investor's Address
                    InvestorAddress investorAddress = investor.InvestorAddresses.First();
                    Address address = investorAddress.Address;
                    model.Address1 = address.Address1 ?? "";
                    model.Address2 = address.Address2 ?? "";
                    model.City = address.City ?? "";
                    model.Country = address.Country;
                    model.Zip = address.PostalCode;
                    model.State = address.State.Value;
                    #endregion

                    #region Investor's communication (email, fax etc)
                    List<InvestorCommunication> investorComms = investor.InvestorCommunications.ToList();
                    foreach (InvestorCommunication comm in investorComms) {
                        if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.HomePhone) {
                            model.Phone = comm.Communication.CommunicationValue;
                        } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Email) {
                            model.Email = comm.Communication.CommunicationValue;
                        } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.WebAddress) {
                            model.WebAddress = comm.Communication.CommunicationValue;
                        } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Fax) {
                            model.Fax = comm.Communication.CommunicationValue;
                        }
                    }
                    #endregion

                    #region Investor's Accounts
                    int counter = 0;
                    foreach (InvestorAccount investorAccount in investor.InvestorAccounts.ToList()) {
                        // if (DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "BankIndex"]) <= 0) continue;
                        formValues.Add(++counter + "_BankIndex", counter.ToString());

                        // Only the following form keys have name different from the object properties
                        //investorAccount.Routing = DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "ABANumber"]);
                        //investorAccount.FFCNumber = Convert.ToString(collection[(index + 1).ToString() + "_" + "FFCNO"]);
                        //investorAccount.Account = Convert.ToString(collection[(index + 1).ToString() + "_" + "AccountNumber"]);
                        if (investorAccount.Routing != null) {
                            formValues.Add(counter + "_ABANumber", investorAccount.Routing.ToString());
                        }
                        if (!string.IsNullOrEmpty(investorAccount.FFCNumber)) {
                            formValues.Add(counter + "_FFCNO", investorAccount.FFCNumber.ToString());
                        }
                        if (!string.IsNullOrEmpty(investorAccount.Account)) {
                            formValues.Add(counter + "_AccountNumber", investorAccount.Account.ToString());
                        }
                        formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(investorAccount, counter + "_", string.Empty));
                    }
                    model.AccountLength = counter;
                    #endregion

                    #region Investor's contact
                    counter = 0;
                    foreach (InvestorContact investorContact in investor.InvestorContacts) {
                        counter++;
                        //if (DataTypeHelper.ToInt32(collection[(index + 1).ToString() + "_" + "ContactIndex"]) <= 0) continue;
                        formValues.Add(counter + "_ContactIndex", counter.ToString());
                        Contact contact = investorContact.Contact;
                        // Only the following form keys have name different from the object properties
                        //investorContact.Contact.ContactName = Convert.ToString(collection[(index + 1).ToString() + "_" + "ContactPerson"]);
                        //investorContact.Contact.ReceivesDistributionNotices = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "DistributionNotices"]);
                        //investorContact.Contact.ReceivesFinancials = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "Financials"]);
                        //investorContact.Contact.ReceivesInvestorLetters = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "InvestorLetters"]);
                        //investorContact.Contact.ReceivesK1 = DataTypeHelper.CheckBoolean(collection[(index + 1).ToString() + "_" + "K1"]);
                        if (!string.IsNullOrEmpty(contact.ContactName)) {
                            formValues.Add(counter + "_ContactPerson", contact.ContactName);
                        }
                        formValues.Add(counter + "_DistributionNotices", contact.ReceivesDistributionNotices.ToString());
                        formValues.Add(counter + "_Financials", contact.ReceivesFinancials.ToString());
                        formValues.Add(counter + "_InvestorLetters", contact.ReceivesInvestorLetters.ToString());
                        formValues.Add(counter + "_K1", contact.ReceivesK1.ToString());
                        formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(contact, counter + "_", string.Empty));

                        #region Contact's address
                        foreach (ContactAddress contactAddress in contact.ContactAddresses) {
                            Address addr = contactAddress.Address;
                            // Only the following form keys have name different from the object properties
                            // contactAddress.Address.PostalCode = collection[(index + 1).ToString() + "_" + "ContactZip"];
                            if (!string.IsNullOrEmpty(addr.PostalCode)) {
                                formValues.Add(counter + "_ContactZip", addr.PostalCode);
                            }
                            formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(addr, counter + "_Contact", string.Empty));
                        }
                        #endregion

                        #region Contact's communication (email, fax etc)
                        foreach (ContactCommunication comm in contact.ContactCommunications) {
                            string commValue = comm.Communication.CommunicationValue;
                            if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.HomePhone) {
                                formValues.Add(counter + "_ContactPhoneNumber", commValue);
                            } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Email) {
                                formValues.Add(counter + "_ContactEmail", commValue);
                            } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.WebAddress) {
                                formValues.Add(counter + "_ContactWebAddress", commValue);
                            } else if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Fax) {
                                formValues.Add(counter + "_ContactFaxNumber", commValue);
                            }
                        }
                        #endregion
                    }
                    model.ContactLength = counter;
                    #endregion

                    formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

                    // Send the request 
                    string url = HttpWebRequestUtil.GetUrl("Investor/Create");
                    byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
                    HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        using (Stream receiveStream = response.GetResponseStream()) {
                            // Pipes the stream to a higher level stream reader with the required encoding format. 
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                                string resp = readStream.ReadToEnd();
                                if (!string.IsNullOrEmpty(resp)) {
                                    int? investorId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                                    if (investorId.HasValue) {
                                        RecordsImportedSuccessfully++;
                                        values = values.Combine(formValues);
                                        Util.WriteNewEntry(string.Format("Created new investor: {0}, InvestorId: {1}", model.InvestorName, investorId));
                                    } else {
                                        ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>(investor, new Exception(resp)));
                                    }
                                } else {
                                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>(investor, new Exception(resp)));
                                }
                                response.Close();
                                readStream.Close();
                            }
                        }

                    }
                } catch (Exception ex) {
                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>(investor, ex));
                }

            }
            LogErrors(ImportErrors);
            return values;
        }

        public static void ConvertInvestorViaDB() {
            ImportErrors = new List<KeyValuePair<Models.Entity.Investor, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<DeepBlue.Models.Entity.Investor> dbInvestors = ConvertBlueToDeepBlue();
            LogErrors(Errors);
            foreach (DeepBlue.Models.Entity.Investor investor in dbInvestors) {
                TotalImportRecords++;
                using (DeepBlueEntities context = new DeepBlueEntities()) {
                    try {
                        context.Investors.AddObject(investor);
                        context.SaveChanges();
                        RecordsImportedSuccessfully++;
                    }
                    catch (Exception ex) {
                        ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>(investor, ex));
                    }
                }
            }
            LogErrors(ImportErrors);
        }

        private static void LogErrors(List<KeyValuePair<C7_20tblLPPaymentInstructions, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));
            
                foreach (KeyValuePair<C7_20tblLPPaymentInstructions, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.FullName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + kv.Value.StackTrace);
                    }
                    catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.Investor, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.Investor, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.InvestorName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    }
                    catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        public static List<DeepBlue.Models.Entity.Investor> ConvertBlueToDeepBlue() {
            Errors = new List<KeyValuePair<C7_20tblLPPaymentInstructions, Exception>>();
            TotalConversionRecords = 0;
            RecordsConvertedSuccessfully = 0;
            List<DeepBlue.Models.Entity.Investor> dbInvestors = new List<DeepBlue.Models.Entity.Investor>();
            using (BlueEntities context = new BlueEntities()) {
                List<C7_20tblLPPaymentInstructions> investors = context.C7_20tblLPPaymentInstructions.ToList();
                foreach (C7_20tblLPPaymentInstructions investor in investors) {
                    try {
                        TotalConversionRecords++;
                        DeepBlue.Models.Entity.Investor deepBlueInvestor = GetInvestorFromBlue(investor);
                        #region Investor Account
                        // Blue has only 1 account for 1 investor
                        InvestorAccount account = new InvestorAccount();
                        if (!string.IsNullOrEmpty(investor.ABANumber)) {
                            account.Routing = Convert.ToInt32(investor.ABANumber.Trim().Replace(" ", string.Empty).Replace("-", string.Empty));
                        }
                        if (!string.IsNullOrEmpty(investor.AccountNumber)) {
                            account.Account = investor.AccountNumber;
                        }
                        else {
                            account.Account = Globals.DefaultStringValue;
                        }
                        account.AccountOf = investor.Accountof;
                        account.Attention = investor.Attn;
                        account.BankName = investor.Bank;
                        account.Reference = investor.Reference;
                        account.CreatedBy = Globals.CurrentUser.UserID;
                        account.CreatedDate = DateTime.Now;
                        account.EntityID = Globals.DefaultEntityID;
                        account.IsPrimary = false;
                        account.LastUpdatedBy = Globals.CurrentUser.UserID;
                        account.LastUpdatedDate = DateTime.Now;
                        // WARNING: The following values are present in our database, but not present in Blue, so setting those to NULL
                        // FFC
                        // FFCNO
                        // IBAN
                        // ByOrderOf
                        // Swift
                        #endregion
                        deepBlueInvestor.InvestorAccounts.Add(account);

                        #region Contact Info
                        foreach (C7_25LPContactinfo contactInfo in investor.C7_25LPContactinfo) {
                            InvestorContact investorContact = new InvestorContact();
                            investorContact.CreatedBy = Globals.CurrentUser.UserID;
                            investorContact.CreatedDate = DateTime.Now;
                            investorContact.EntityID = Globals.DefaultEntityID;
                            investorContact.LastUpdatedBy = Globals.CurrentUser.UserID;
                            investorContact.LastUpdatedDate = DateTime.Now;
                            Contact contact = new Contact();
                            contact.ContactCompany = contactInfo.ContactCompany;
                            contact.ContactName = contactInfo.ContactName;
                            // WARNING: Deepblue has consolidated CallNotices/Distribution notices into one field.
                            if (contactInfo.DistributionNotices != null) {
                                contact.ReceivesDistributionNotices = contactInfo.DistributionNotices.Value;
                            }
                            if (contactInfo.Financials != null) {
                                contact.ReceivesFinancials = contactInfo.Financials.Value;
                            }
                            if (contactInfo.InvestorLetters != null) {
                                contact.ReceivesInvestorLetters = contactInfo.InvestorLetters.Value;
                            }
                            contact.CreatedBy = Globals.CurrentUser.UserID;
                            contact.CreatedDate = DateTime.Now;
                            contact.FirstName = contactInfo.ContactName;
                            contact.LastName = "n/a";
                            contact.LastUpdatedBy = Globals.CurrentUser.UserID;
                            contact.LastUpdatedDate = DateTime.Now;
                            contact.EntityID = Globals.DefaultEntityID;
                            investorContact.Contact = contact;

                            // WARNING: We dont have the following values in our database
                            // contactInfo.Dear; // This seems to be the first name from Contact Name
                            Address contactAddress = new Address();
                            if (contactInfo.ContactAddress != null) {
                                if (contactInfo.ContactAddress.Length > 40) {
                                    contactAddress.Address1 = contactInfo.ContactAddress.Substring(0, 40);
                                } else {
                                    contactAddress.Address1 = contactInfo.ContactAddress;
                                }
                            }
                            else {
                                contactAddress.Address1 = Globals.DefaultStringValue;
                            }
                            //contactInfo.Comments;
                            contactAddress.Address2 = contactInfo.ContactAddress2;
                            // Contact Info(Access) doesnt have the values for these properties, so using default values
                            contactAddress.Country = Globals.DefaultCountryID;
                            contactAddress.City = Globals.DefaultCity;
                            contactAddress.State = Globals.DefaultStateID;
                            contactAddress.PostalCode = Globals.DefaultZip;
                            try {
                                string[] parts = new string[3];
                                if (ParseAddress(contactInfo.ContactAddress2, out parts)) {
                                    contactAddress.City = parts[0];
                                    contactAddress.PostalCode = parts[2];
                                    int postalCode = 0;
                                    bool validZip = true;
                                    if (Int32.TryParse(contactAddress.PostalCode, out postalCode)) {
                                        if (contactAddress.PostalCode.Length > 5) {
                                            validZip = false;
                                        }
                                    } else {
                                        validZip = false;
                                    }
                                    if (!validZip) {
                                        contactAddress.PostalCode = Globals.DefaultZip;
                                    }
                                    contactAddress.State = Globals.States.Where(x => x.Abbr == parts[1].ToUpper().Trim()).First().StateID;
                                }
                                else {
                                    contactAddress.City = "dataerror: " + contactInfo.ContactAddress2;
                                }
                            }
                            catch {
                               
                            }

                            AddCommunication(contact, Models.Admin.Enums.CommunicationType.Email, contactInfo.ContactEmail);
                            AddCommunication(contact, Models.Admin.Enums.CommunicationType.HomePhone, contactInfo.ContactPhone);
                            AddCommunication(contact, Models.Admin.Enums.CommunicationType.Fax, contactInfo.ContactFax);
                            contactAddress.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
                            contactAddress.CreatedBy = Globals.CurrentUser.UserID;
                            contactAddress.CreatedDate = DateTime.Now;
                            contactAddress.EntityID = Globals.DefaultEntityID;
                            contactAddress.LastUpdatedBy = Globals.CurrentUser.UserID;
                            contactAddress.LastUpdatedDate = DateTime.Now;

                            ContactAddress cntAddr = new ContactAddress();
                            cntAddr.CreatedBy = Globals.CurrentUser.UserID;
                            cntAddr.CreatedDate = DateTime.Now;
                            cntAddr.EntityID = Globals.DefaultEntityID;
                            cntAddr.LastUpdatedBy = Globals.CurrentUser.UserID;
                            cntAddr.LastUpdatedDate = DateTime.Now;
                            cntAddr.Address = contactAddress;

                            investorContact.Contact.ContactAddresses.Add(cntAddr);
                            deepBlueInvestor.InvestorContacts.Add(investorContact);
                        }
                        #endregion
                        dbInvestors.Add(deepBlueInvestor);
                        RecordsConvertedSuccessfully++;
                    }
                    catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C7_20tblLPPaymentInstructions, Exception>(investor, ex));
                    }
                }
            }
            return dbInvestors;
        }

        private static DeepBlue.Models.Entity.Investor GetInvestorFromBlue(C7_20tblLPPaymentInstructions blueInvestor) {
            DeepBlue.Models.Entity.Investor investor = new DeepBlue.Models.Entity.Investor();
            investor.InvestorName = blueInvestor.FullName;
            investor.Notes = blueInvestor.Comments;
            investor.Alias = blueInvestor.Nameidentifier;
            // WARNING Blue has the following properties, but DeepBlue doesnt have these
            // investor.Reference;

            // WARNING: DeepBlue has the following properties, but Blue doesnt have, so we are using the default values
            investor.IsDomestic = true;
            investor.InvestorEntityTypeID = Globals.DefaultInvestorEntityTypeID;
            investor.ResidencyState = Globals.DefaultStateID;
            investor.Social = Guid.NewGuid().ToString("N").Substring(0,25);
            investor.Notes = Globals.DefaultString;

            investor.TaxID = 0;
            investor.FirstName = string.Empty;
            investor.LastName = "n/a";
            investor.ManagerName = string.Empty;
            investor.MiddleName = string.Empty;
            investor.PrevInvestorID = 0;
            investor.CreatedBy = Globals.CurrentUser.UserID;
            investor.CreatedDate = DateTime.Now;
            investor.LastUpdatedBy = Globals.CurrentUser.UserID;
            investor.LastUpdatedDate = DateTime.Now;
            investor.EntityID = Globals.DefaultEntityID;
            investor.TaxExempt = false;

            // Attempt to create new investor address.
            // WARNING: In Blue, there is no concept of an Investor Address, so we are setting default values here
            InvestorAddress investorAddress = new InvestorAddress();
            investorAddress.CreatedBy = Globals.CurrentUser.UserID;
            investorAddress.CreatedDate = DateTime.Now;
            investorAddress.EntityID = Globals.DefaultEntityID;
            investorAddress.LastUpdatedBy = Globals.CurrentUser.UserID;
            investorAddress.LastUpdatedDate = DateTime.Now;
            investorAddress.Address = new Address();
            investorAddress.Address.Address1 = Globals.DefaultString;
            investorAddress.Address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
            investorAddress.Address.City = Globals.DefaultCity;
            investorAddress.Address.Country = Globals.DefaultCountryID;
            investorAddress.Address.CreatedBy = Globals.CurrentUser.UserID;
            investorAddress.Address.CreatedDate = DateTime.Now;
            investorAddress.Address.LastUpdatedBy = Globals.CurrentUser.UserID;
            investorAddress.Address.LastUpdatedDate = DateTime.Now;
            investorAddress.Address.EntityID = Globals.DefaultEntityID;
            investorAddress.Address.PostalCode = Globals.DefaultZip;
            investorAddress.Address.State = Globals.DefaultStateID;
            investor.InvestorAddresses.Add(investorAddress);
            return investor;
        }

        private static CreateModel GetCreateModelFromBlue(C7_20tblLPPaymentInstructions investor) {
            CreateModel model = new CreateModel();
            model.InvestorName = investor.FullName;
            model.Notes = investor.Comments;
            model.Alias = investor.Nameidentifier;
            // WARNING Blue has the following properties, but DeepBlue doesnt have these
            // investor.Reference;

            // WARNING: DeepBlue has the following properties, but Blue doesnt have, so we are using the default values
            model.DomesticForeign = true;
            model.EntityType = Globals.DefaultInvestorEntityTypeID;
            model.StateOfResidency = Globals.DefaultStateID;
            model.SocialSecurityTaxId = "123-23-1234";
            model.Notes = Globals.DefaultString;

            #region Investor Address
            // WARNING: In Blue, there is no concept of an Investor Address, so we are setting default values here
            Address investorAddress = new Address();
            investorAddress.Address1 = string.Empty;
            investorAddress.City = Globals.DefaultCity;
            investorAddress.Country = Globals.DefaultCountryID;
            investorAddress.PostalCode = Globals.DefaultZip;
            investorAddress.State = Globals.DefaultStateID;
            #endregion
            return model;
        }

        private static bool ParseAddress(string address, out string[] parts) {
            parts = new string[3];
            try {

                int lastIndex = 0;
                lastIndex = address.LastIndexOf(",");
                parts[0] = address.Substring(0, lastIndex);
                parts[1] = address.Substring(lastIndex + 1, 2);
                lastIndex = address.LastIndexOf(" ");
                parts[2] = address.Substring(lastIndex + 1);
            }
            catch {
                return false;
            }
            return true;
        }

        private static void AddCommunication(DeepBlue.Models.Entity.Contact contact, DeepBlue.Models.Admin.Enums.CommunicationType communicationType, string value) {
            if (!string.IsNullOrEmpty(value)) {
                ContactCommunication contactComm = new ContactCommunication();
                Communication comm = new Communication();
                comm.CommunicationTypeID = (int)communicationType;
                comm.CommunicationValue = value;
                contactComm.Communication = comm;
                contact.ContactCommunications.Add(contactComm);
            }
        }

       

        public static List<Investor> GetInvestors(CookieCollection cookies, int? fundID, string investor = null) {
            List<Investor> investors = new List<Investor>();
            // Send the request 
            string query = string.Empty;
            if (fundID.HasValue) {
                query = "&fundId=" + fundID.Value;
            }
            string url = HttpWebRequestUtil.GetUrl("Investor/FindInvestors?term=" + (string.IsNullOrEmpty(investor) ? string.Empty : System.Web.HttpUtility.UrlEncode(investor)) + query);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            List<AutoCompleteList> jsonFunds = (List<AutoCompleteList>)js.Deserialize(resp, typeof(List<AutoCompleteList>));
                            foreach (AutoCompleteList alist in jsonFunds) {
                                Investor inv = new Investor();
                                inv.InvestorID = alist.id;
                                inv.InvestorName = alist.value;
                                investors.Add(inv);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return investors;
        }
    }



    class CreateInvestorContainer {
        public CreateModel CreateModel;
        public List<InvestorAccount> InvestorAccounts;
        public List<Contact> InvestorContacts;
    }
}
