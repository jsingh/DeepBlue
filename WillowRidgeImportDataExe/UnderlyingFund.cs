using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using DeepBlue.ImportData.SourceData;
using DeepBlue.Models.Entity;
using System.Net;
using DeepBlue.Models.Deal;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using DeepBlue.Models.Fund;
using System.IO;
using System.Web.Script.Serialization;
using DeepBlue.Helpers;

// /Deal/UpdateUnderlyingFund
// /Deal/CreateUnderlyingFundAddress
// /Deal/CreateUnderlyingFundContact

// Create a default Issuer for all the conversion records
// Associate the address created to the Registered Office ID
namespace DeepBlue.ImportData {
    class UnderlyingFundImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>> Errors = new List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>>();
        public static List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>>();
        private static StringBuilder messageLog = new StringBuilder();

        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static NameValueCollection ImportFunds(CookieCollection cookies) {
            NameValueCollection values = new NameValueCollection();
            ImportErrors = new List<KeyValuePair<Models.Entity.UnderlyingFund, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Address>> dbFunds = ConvertBlueToDeepBlue();
            LogErrors(Errors);
            // Create a new issuer and assign all the underlying funds to it
            int? issuerId = 0;
            if (dbFunds.Count > 0) {
                issuerId = CreateIssuer(cookies);
            }

            if (issuerId != null) {
                string msg = "Created new issuer: " + issuerId;
                Util.WriteNewEntry(msg);
                messageLog.AppendLine(msg);
                foreach (KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Address> kvp in dbFunds) {
                    NameValueCollection formValues = new NameValueCollection();
                    DeepBlue.Models.Entity.UnderlyingFund underlyingFund = kvp.Key;
                    if (!IsUnderlyingFundAlreadyPresent(underlyingFund.FundName)) {
                        TotalImportRecords++;
                        try {
                            CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
                            model.IsFeesIncluded = underlyingFund.IsFeesIncluded;
                            model.FundName = underlyingFund.FundName;
                            model.FundTypeId = underlyingFund.FundTypeID;
                            model.IssuerId = issuerId.Value;
                            model.VintageYear = underlyingFund.VintageYear;
                            model.TotalSize = underlyingFund.TotalSize;
                            model.TerminationYear = underlyingFund.TerminationYear;
                            model.IncentiveFee = underlyingFund.IncentiveFee;
                            model.LegalFundName = underlyingFund.LegalFundName;
                            model.Description = underlyingFund.Description;
                            model.FiscalYearEnd = underlyingFund.FiscalYearEnd;
                            model.ManagementFee = underlyingFund.ManagementFee;
                            model.Taxable = underlyingFund.Taxable;
                            model.TaxRate = underlyingFund.TaxRate;
                            model.AuditorName = underlyingFund.AuditorName;
                            model.IsDomestic = underlyingFund.IsDomestic;
                            model.Exempt = underlyingFund.Exempt;

                            model.IndustryId = underlyingFund.IndustryID;
                            model.GeographyId = underlyingFund.GeographyID;
                            model.ReportingFrequencyId = underlyingFund.ReportingFrequencyID;
                            model.ReportingTypeId = underlyingFund.ReportingTypeID;

                            // We dont use the ShareClass Type
                            // model.ShareClassTypeId
                            // We also dont use the InvestmentTypeId
                            // model.InvestmentTypeId = underlyingFund.InvestmentTypeID;

                            model.Description = underlyingFund.Description;
                            model.FiscalYearEnd = underlyingFund.FiscalYearEnd;
                            model.IsDomestic = underlyingFund.IsDomestic;
                            model.FundStructureId = underlyingFund.FundStructureID;
                            model.Taxable = underlyingFund.Taxable;
                            model.Exempt = underlyingFund.Exempt;
                            model.ManagementFee = underlyingFund.ManagementFee;
                            model.IncentiveFee = underlyingFund.IncentiveFee;
                            model.TaxRate = underlyingFund.TaxRate;
                            model.AuditorName = underlyingFund.AuditorName;
                            model.WebUserName = underlyingFund.WebUserName;
                            model.WebPassword = underlyingFund.WebPassword;
                            if (underlyingFund.Account != null) {
                                model.BankName = underlyingFund.Account.BankName;
                                model.ABANumber = underlyingFund.Account.Routing;
                                model.AccountOf = underlyingFund.Account.AccountOf;
                                model.AccountNumber = underlyingFund.Account.Account1;
                                model.Account = underlyingFund.Account.Account1;
                                model.Attention = underlyingFund.Account.Attention;
                                model.Reference = underlyingFund.Account.Reference;
                            }
                            Address address = kvp.Value;
                            if (address != null) {
                                model.Address1 = address.Address1;
                                model.Address2 = address.Address2;
                                model.City = address.City;
                                model.Country = address.Country;
                                model.State = address.State;
                                model.Zip = address.PostalCode;
                            }
                            string resp = string.Empty;
                            int? underlyingFundId = CreateNewUnderlyingFund(cookies, model, out resp);
                            if (underlyingFundId != null) {
                                string newUFMsg = string.Format("created new UF. {0}, new UFID: {1}", model.FundName, underlyingFundId);
                                messageLog.AppendLine(newUFMsg);
                                Util.WriteNewEntry(newUFMsg);
                                RecordsImportedSuccessfully++;
                                string error = string.Empty;
                                // Create the Contact
                                // /Deal/CreateUnderlyingFundContact
                                List<UnderlyingFundContact> contacts = underlyingFund.UnderlyingFundContacts.ToList();
                                foreach (UnderlyingFundContact ufContact in contacts) {
                                    ufContact.UnderlyingtFundID = underlyingFundId.Value;
                                    int? ufContactID = CreateUnderlyingFundContact(ufContact, cookies, out resp);
                                    if (ufContactID == null) {
                                        messageLog.AppendLine("FAIL: UF.Contact Could not be created.");
                                        string newContactMsg = " Underlying Fund Contact could not be created. Response: " + resp;
                                        Util.WriteError(newContactMsg);
                                        error += newContactMsg;
                                    } else {
                                        string newContactMsg = string.Format("created new contact for UF: {0}, UFID: {1}, Contact: {2}, UFContactID: {3}", model.FundName, underlyingFundId, ufContact.Contact.ContactName, ufContactID);
                                        messageLog.AppendLine(newContactMsg);
                                        Util.WriteNewEntry(newContactMsg);
                                    }
                                }
                                // Create the Address
                                // /Deal/CreateUnderlyingFundAddress
                                //Address address = kvp.Value;
                                //if (address != null) {
                                //    CreateUnderlyingFundAddress(underlyingFundId.Value, address, cookies, out resp);
                                //    if (!string.IsNullOrEmpty(resp)) {
                                //        messageLog.AppendLine("FAIL: UF.FundAddress could not be created");
                                //        error += " Underlying Fund Address could not be created. Response: " + resp;
                                //    } else {
                                //        string newAddressMsg = string.Format("created new address for UF: {0}, UFID: {1}", model.FundName, underlyingFundId);
                                //        messageLog.AppendLine(newAddressMsg);
                                //        Util.WriteNewEntry(newAddressMsg);
                                //    }
                                //}

                                if (!string.IsNullOrEmpty(error)) {
                                    Util.WriteError(error);
                                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(underlyingFund, new Exception("Underlying Fund was created, but the following errors were encountered: " + error)));
                                }
                            } else {
                                Util.WriteError(string.Format("Error creating UF: {0}, Response: {1}" , underlyingFund.FundName, resp));
                                ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(underlyingFund, new Exception("Underlying Fund Could not be created. Response: " + resp)));
                            }


                        } catch (Exception ex) {
                            ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(underlyingFund, ex));
                            Util.WriteError(ex.Message);
                            Util.Log(ex.Message);
                        }
                    } else {
                        string alreadyPresent = string.Format("Underlying fund: {0} is already present", underlyingFund.FundName);
                        Util.WriteWarning(alreadyPresent);
                        messageLog.AppendLine(alreadyPresent);
                    }
                }
            } else {
                Util.WriteError("CANNOT CREATE ISSUER. BAILING.......");
                messageLog.AppendLine("Error creating issuer");
            }
            LogErrors(ImportErrors);
            LogMessages();
            return values;
        }

        private static bool IsUnderlyingFundAlreadyPresent(string ufName) {
            return GetUnderlyingFunds(Globals.CookieContainer).Where(x => x.FundName.Equals(ufName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null;
        }

        #region Convert Single Underlying Fund
        public static int? ImportUnderlyingFund(CookieCollection cookies, string blueUFName) {
            using (BlueEntities context = new BlueEntities()) {
                C7_10tblGPPaymentInstructions uFund = context.C7_10tblGPPaymentInstructions.Where(x => x.FundType != "Direct").Where(x => x.Fund.Equals(blueUFName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                Address address = null;
                DeepBlue.Models.Entity.UnderlyingFund deepBlueFund = GetUnderlyingFundFromBlue(uFund, out address);
                int RecordsImportedSuccessfully = 0;
                List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> ImportErrors = new List<KeyValuePair<UnderlyingFund, Exception>>();
                if (uFund != null) {
                    int? issuerId = CreateIssuer(cookies);
                    return CreateUnderlyingFund(cookies, deepBlueFund, address, issuerId, ref RecordsImportedSuccessfully, ref ImportErrors);
                }
            }
            return null;
        }

        private static int? CreateUnderlyingFund(CookieCollection cookies, DeepBlue.Models.Entity.UnderlyingFund underlyingFund, Address address, int? issuerId, ref int RecordsImportedSuccessfully, ref List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> ImportErrors) {
            CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
            model.IsFeesIncluded = underlyingFund.IsFeesIncluded;
            model.FundName = underlyingFund.FundName;
            model.FundTypeId = underlyingFund.FundTypeID;
            model.IssuerId = issuerId.Value;
            model.VintageYear = underlyingFund.VintageYear;
            model.TotalSize = underlyingFund.TotalSize;
            model.TerminationYear = underlyingFund.TerminationYear;
            model.IncentiveFee = underlyingFund.IncentiveFee;
            model.LegalFundName = underlyingFund.LegalFundName;
            model.Description = underlyingFund.Description;
            model.FiscalYearEnd = underlyingFund.FiscalYearEnd;
            model.ManagementFee = underlyingFund.ManagementFee;
            model.Taxable = underlyingFund.Taxable;
            model.TaxRate = underlyingFund.TaxRate;
            model.AuditorName = underlyingFund.AuditorName;
            model.IsDomestic = underlyingFund.IsDomestic;
            model.Exempt = underlyingFund.Exempt;

            model.IndustryId = underlyingFund.IndustryID;
            model.GeographyId = underlyingFund.GeographyID;
            model.ReportingFrequencyId = underlyingFund.ReportingFrequencyID;
            model.ReportingTypeId = underlyingFund.ReportingTypeID;

            // We dont use the ShareClass Type
            // model.ShareClassTypeId
            // We also dont use the InvestmentTypeId
            // model.InvestmentTypeId = underlyingFund.InvestmentTypeID;

            model.Description = underlyingFund.Description;
            model.FiscalYearEnd = underlyingFund.FiscalYearEnd;
            model.IsDomestic = underlyingFund.IsDomestic;
            model.FundStructureId = underlyingFund.FundStructureID;
            model.Taxable = underlyingFund.Taxable;
            model.Exempt = underlyingFund.Exempt;
            model.ManagementFee = underlyingFund.ManagementFee;
            model.IncentiveFee = underlyingFund.IncentiveFee;
            model.TaxRate = underlyingFund.TaxRate;
            model.AuditorName = underlyingFund.AuditorName;
            model.WebUserName = underlyingFund.WebUserName;
            model.WebPassword = underlyingFund.WebPassword;
            if (underlyingFund.Account != null) {
                model.BankName = underlyingFund.Account.BankName;
                model.ABANumber = underlyingFund.Account.Routing;
                model.AccountOf = underlyingFund.Account.AccountOf;
                model.AccountNumber = underlyingFund.Account.Account1;
                model.Account = underlyingFund.Account.Account1;
                model.Attention = underlyingFund.Account.Attention;
                model.Reference = underlyingFund.Account.Reference;
            }

            if (address != null) {
                model.Address1 = address.Address1;
                model.Address2 = address.Address2;
                model.City = address.City;
                model.Country = address.Country;
                model.State = address.State;
                model.Zip = address.PostalCode;
            } else {
                model.Address1 = Globals.DefaultStringValue;
                model.City = Globals.DefaultStringValue;
                model.Country = Globals.DefaultCountryID;
                model.State = Globals.DefaultStateID;
                model.Zip = Globals.DefaultZip;
            }
            
            string resp = string.Empty;
            int? underlyingFundId = CreateNewUnderlyingFund(cookies, model, out resp);
            if (underlyingFundId != null) {
                RecordsImportedSuccessfully++;
                string error = string.Empty;
                // Create the Contact
                // /Deal/CreateUnderlyingFundContact
                List<UnderlyingFundContact> contacts = underlyingFund.UnderlyingFundContacts.ToList();
                foreach (UnderlyingFundContact ufContact in contacts) {
                    ufContact.UnderlyingtFundID = underlyingFundId.Value;
                    int? ufContactID = CreateUnderlyingFundContact(ufContact, cookies, out resp);
                    if (ufContactID == null) {
                        error += " Underlying Fund Contact could not be created. Response: " + resp;
                    }
                }
                // Create the Address
                // /Deal/CreateUnderlyingFundAddress
                if (address != null) {
                    CreateUnderlyingFundAddress(underlyingFundId.Value, address, cookies, out resp);
                    if (!string.IsNullOrEmpty(resp)) {
                        error += " Underlying Fund Address could not be created. Response: " + resp;
                    }
                }

                if (!string.IsNullOrEmpty(error)) {
                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(underlyingFund, new Exception("Underlying Fund was created, but the following errors were encountered: " + error)));
                }
            } else {
                ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(underlyingFund, new Exception("Underlying Fund Could not be created. Response: " + resp)));
            }
            return underlyingFundId;
        }
        #endregion
        private static int? CreateNewUnderlyingFund(CookieCollection cookies, CreateUnderlyingFundModel model, out string resp) {
            resp = string.Empty;
            int? underlyingFundId = null;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Deal/UpdateUnderlyingFund");
            string data = HttpWebRequestUtil.ToFormValue(formValues);
            messageLog.Append("Form Data:").AppendLine(data);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (resp.ToLower().StartsWith("true")) {
                                int ufid = 0;
                                if (Int32.TryParse(resp.Substring(resp.LastIndexOf('|') + 1), out ufid)) {
                                    underlyingFundId = ufid;
                                }
                            }
                        }
                        messageLog.AppendLine("Response: " + resp);
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return underlyingFundId;
        }

        public static NameValueCollection CreateUnderlyingFundAddress(int underlyingFundId, Address address, CookieCollection cookies, out string resp) {
            resp = string.Empty;
            NameValueCollection values = new NameValueCollection();
            UnderlyingFundAddressInformation registeredAddress = new UnderlyingFundAddressInformation();
            registeredAddress.UnderlyingFundId = underlyingFundId;
            registeredAddress.Address1 = address.Address1;
            registeredAddress.Address2 = address.Address2;
            registeredAddress.City = address.City;
            registeredAddress.Country = address.Country;
            registeredAddress.State = address.State;
            registeredAddress.Zip = address.PostalCode;

            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(registeredAddress, string.Empty, string.Empty);
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundAddress");
            messageLog.AppendLine("Deal/CreateUnderlyingFundAddress");
            string data = HttpWebRequestUtil.ToFormValue(formValues);
            messageLog.AppendLine("Form Data: " + data);
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (string.IsNullOrEmpty(resp)) {
                           values = values.Combine(formValues);
                        } else {
                        }
                        messageLog.AppendLine("Response: "+resp);
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return values;
        }

        public static int? CreateUnderlyingFundContact(UnderlyingFundContact ufContact, CookieCollection cookies, out string resp) {
            resp = string.Empty;
            int? returnUFContactID = null;
            UnderlyingFundContactModel contactModel = new UnderlyingFundContactModel();
            contactModel.UnderlyingFundId = ufContact.UnderlyingtFundID.Value;
            contactModel.ContactName = ufContact.Contact.ContactName;
            contactModel.ContactTitle = ufContact.Contact.Title;
            contactModel.ContactNotes = ufContact.Contact.Notes;
            List<ContactCommunication> contactCommunications = ufContact.Contact.ContactCommunications.ToList();
            foreach (ContactCommunication comm in contactCommunications) {
                if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.Email) {
                    contactModel.Email = comm.Communication.CommunicationValue;
                }

                if (comm.Communication.CommunicationTypeID == (int)Models.Admin.Enums.CommunicationType.HomePhone) {
                    contactModel.Phone = comm.Communication.CommunicationValue;
                }
            }

            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(contactModel, string.Empty, string.Empty);
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundContact");
            string data = HttpWebRequestUtil.ToFormValue(formValues);
            messageLog.AppendLine("Deal/CreateUnderlyingFundContact. Form Data: ").AppendLine(data); ;
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        int retVal = 0;
                        if(Int32.TryParse(resp, out retVal)){
                            returnUFContactID = retVal;
                        }
                        messageLog.AppendLine("Response: " + retVal);
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return returnUFContactID;
        }

        public static List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Address>> ConvertBlueToDeepBlue() {
            List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Address>> dbFunds = new List<KeyValuePair<UnderlyingFund, Address>>();
            Errors = new List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>>();
            TotalConversionRecords = 0;
            RecordsConvertedSuccessfully = 0;
            //List<DeepBlue.Models.Entity.UnderlyingFund> dbFunds = new List<DeepBlue.Models.Entity.UnderlyingFund>();
            using (BlueEntities context = new BlueEntities()) {
                List<C7_10tblGPPaymentInstructions> uFunds = context.C7_10tblGPPaymentInstructions.ToList(); //.Where(x => x.FundType != "Direct").ToList();
                foreach (C7_10tblGPPaymentInstructions uf in uFunds) {
                    try {
                        TotalConversionRecords++;
                        Address address = null;
                        DeepBlue.Models.Entity.UnderlyingFund deepBlueFund = GetUnderlyingFundFromBlue(uf, out address);
                        dbFunds.Add(new KeyValuePair<UnderlyingFund, Address>(deepBlueFund, address));
                        RecordsConvertedSuccessfully++;
                    } catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C7_10tblGPPaymentInstructions, Exception>(uf, ex));
                        Util.Log("ConvertBlueToDeepBlue() " + ex);
                    }
                }
            }
            return dbFunds;
        }

        private static DeepBlue.Models.Entity.UnderlyingFund GetUnderlyingFundFromBlue(C7_10tblGPPaymentInstructions blueUFund, out Address fundRegisteredOffice) {
            DeepBlue.Models.Entity.UnderlyingFund uf = new DeepBlue.Models.Entity.UnderlyingFund();
            uf.EntityID = Globals.DefaultEntityID;
            // ToDO: IssuerID
            uf.FundName = blueUFund.Fund;
            uf.FundTypeID = GetFundType(blueUFund.FundType);
            uf.IsFeesIncluded = blueUFund.FeesInside.HasValue ? blueUFund.FeesInside.Value : false;
            uf.VintageYear = blueUFund.VintageYear.HasValue ? (short?)blueUFund.VintageYear : null;
            // TODO: Convert FundSize to money in DB and re-gen the model
            try {
                uf.TotalSize = Convert.ToInt32(blueUFund.FundSize);
            } catch {
                uf.TotalSize = Int32.MaxValue;
            }
            if (blueUFund.TerminationDate.HasValue) {
                uf.TerminationYear = Convert.ToInt16(blueUFund.TerminationDate.Value.Year);
            }


            // WARNING: these fields are present in blue but absent in deepblue
            // What are these fields used for in blue?
            //blueUFund.Website;
            uf.GeographyID = Globals.Geograpies.First().GeographyID;
            // WARNING: We dont use the InvestmentTypeID. Do we really need this?
            // uf.InvestmentTypeID
            // ShareClass type can be added via the admin screen. However, we dont ask for it when creating a new UF. Should we not ask for it?
            // uf.ShareClassType
            // We dont use this field,a s there is no UI element for it.
            // uf.FundStructureID
            uf.WebUserName = blueUFund.WebLogin;
            uf.WebPassword = blueUFund.WebPassword;

            uf.IndustryID = GetIndustryFocus(blueUFund.Industry_Focus);
            uf.ReportingFrequencyID = GetReportingFrequency(blueUFund.Reporting);
            uf.ReportingTypeID = GetReportingType(blueUFund.ReportingType);
            uf.Description = Globals.DefaultString + blueUFund.Comments ?? string.Empty;
            // description cannot be over 100 characters
            if (uf.Description.Length > 100) {
                uf.Description = uf.Description.Substring(0, 100);
            }

            Contact contact = new Contact();
            contact.ContactName = blueUFund.ContactName;
            if (!string.IsNullOrEmpty(blueUFund.Phone)) {
                Communication comm = new Communication() { CommunicationTypeID = (int)DeepBlue.Models.Admin.Enums.CommunicationType.WorkPhone, CommunicationValue = blueUFund.Phone };
                contact.ContactCommunications.Add(new ContactCommunication() { Communication = comm });
            }
            if (!string.IsNullOrEmpty(blueUFund.Email_address)) {
                Communication comm = new Communication() { CommunicationTypeID = (int)DeepBlue.Models.Admin.Enums.CommunicationType.Email, CommunicationValue = blueUFund.Email_address };
                contact.ContactCommunications.Add(new ContactCommunication() { Communication = comm });
            }
            if (!string.IsNullOrEmpty(contact.ContactName)) {
                uf.UnderlyingFundContacts.Add(new UnderlyingFundContact() { Contact = contact });
            }

            //uf.FundRegisteredOfficeID
            // TODO: use uf.FundRegisteredOfficeID to store the address
            // This is the Fund's registered office address, and not the contact's address
            Address address = null;
            if (!string.IsNullOrEmpty(blueUFund.MailingAddress1) || !string.IsNullOrEmpty(blueUFund.MailingAddress2)) {
                address = new Address();
                if (!string.IsNullOrEmpty(blueUFund.MailingAddress1)) {
                    address.Address1 = blueUFund.MailingAddress1;
                } else {
                    address.Address1 = Globals.DefaultStringValue;
                }

                // Address 1 has to be less than or equal to 40 characters
                if (address.Address1.Length > 40) {
                    address.Address1 = address.Address1.Substring(0, 40);
                }

                address.Address2 = blueUFund.MailingAddress2;
                // Address 2 has to be less than or equal to 40 characters
                if (!string.IsNullOrEmpty(address.Address2) && address.Address2.Length > 40) {
                    address.Address2 = address.Address2.Substring(0, 40);
                }
                //string[] parts = new string[3];
                //if (Util.ParseAddress(address.Address2, out parts)) {
                //    address.City = parts[0];
                //    address.State = Globals.States.Where(x => x.Abbr == parts[1].ToUpper().Trim()).First().StateID;
                //    address.PostalCode = parts[2];
                //}
                Util.SetAddress(address.Address2, address);

                address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
                address.Country = Globals.DefaultCountryID;
            } else {
                // We have to have the address.. The UI enforces us to have an address
                address = new Address();
                address.Address1 = Globals.DefaultAddress1;
                address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
                address.City = Globals.DefaultCity;
                address.State = Globals.DefaultStateID;
                address.PostalCode = Globals.DefaultZip;
                address.Country = Globals.DefaultCountryID;
                messageLog.AppendLine("WARNING: " + uf.FundName + " doesnt have an address, so using default address");
            }

            fundRegisteredOffice = address;
            // You have to have an account# for a successful account to be created
            if (!string.IsNullOrEmpty(blueUFund.AccountNumber)) {
                Account account = new Account();
                account.BankName = blueUFund.Bank;
                try {
                    account.Routing = Convert.ToInt32(blueUFund.ABANumber.Replace("-", string.Empty).Replace(" ", string.Empty));
                } catch {
                    account.Routing = 111000025;
                }
                account.AccountOf = blueUFund.Accountof;
                if (!string.IsNullOrEmpty(blueUFund.AccountNumber)) {
                    account.Account1 = blueUFund.AccountNumber;
                } else {
                    account.Account1 = "dummy_account";
                }
                account.Attention = blueUFund.Attn;
                account.Reference = blueUFund.Reference;
                uf.Account = account;
            }

            // WARNING: the following fields are present in DeepBlue, but are not present in Blue
            //uf.LegalFundName
            uf.LegalFundName = uf.FundName;
            //uf.FiscalYearEnd
            //uf.IsDomestic
            //uf.FundStructureId
            //uf.Taxable
            //uf.Exempt
            //uf.AddressID
            //uf.managementfee
            //uf.incentivefee
            //uf.taxrate
            //uf.auditorname
            //uf.managercontactid
            //uf.shareclasstypeid
            //uf.investmenttypeid
            //account.Phone;
            //account.Fax;
            //account.IBAN;
            //account.FFC;
            //account.FFCNumber;
            //account.SWIFT;
            //account.AccountNumberCash
            return uf;
        }

        private static int GetFundType(string fundType) {
            UnderlyingFundType ufType = Globals.UnderlyingFundTypes.Where(x => x.Name == fundType).FirstOrDefault();
            if (ufType == null) {
                ufType = Globals.UnderlyingFundTypes.First();
            }
            return ufType.UnderlyingFundTypeID;
        }

        private static int GetIndustryFocus(string industryType) {
            Industry industry = Globals.Industries.Where(x => x.Industry1 == industryType).FirstOrDefault();
            if (industry == null) {
                industry = Globals.Industries.First();
            }
            return industry.IndustryID;
        }

        private static int GetReportingType(string reportingType) {
            ReportingType rType = Globals.ReportingTypes.Where(x => x.Reporting == reportingType).FirstOrDefault();
            if (rType == null) {
                rType = Globals.ReportingTypes.First();
            }
            return rType.ReportingTypeID;
        }

        private static int GetReportingFrequency(string reportingFrequency) {
            ReportingFrequency frequency = Globals.ReportingFrequencies.Where(x => x.ReportingFrequency1 == reportingFrequency).FirstOrDefault();
            if (frequency == null) {
                frequency = Globals.ReportingFrequencies.First();
            }
            return frequency.ReportingFrequencyID;
        }

        private static int? CreateIssuer(CookieCollection cookies) {
            Issuer issuer = new Issuer();
            issuer.Name = Globals.DefaultString + " " + DateTime.Now;
            messageLog.AppendLine("Creating new issuer: "+issuer.Name);
            return UFIssuerImport.CreateIssuerViaWeb(new List<Issuer>() { issuer }, cookies);
        }

        private static void GetUnderlyingFund(string ufName, CookieCollection cookies) {
            
        }

        public static List<DeepBlue.Models.Deal.UnderlyingFundListModel> GetUnderlyingFunds(CookieCollection cookies) {
            List<DeepBlue.Models.Deal.UnderlyingFundListModel> ufs = new List<DeepBlue.Models.Deal.UnderlyingFundListModel>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingFundList?pageIndex=1&pageSize=5000&sortName=FundName&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Deal.UnderlyingFundListModel ufListModel = new DeepBlue.Models.Deal.UnderlyingFundListModel();
                                ufListModel.UnderlyingFundId = Convert.ToInt32(row.cell[0]);
                                ufListModel.FundName = Convert.ToString(row.cell[1]);
                                ufListModel.FundType = Convert.ToString(row.cell[2]);
                                ufListModel.Industry = Convert.ToString(row.cell[3]);
                                ufListModel.IssuerID = Convert.ToInt32(row.cell[4]);
                                ufs.Add(ufListModel);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return ufs;
        }

        private static void LogMessages() {
            using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
                tw.WriteLine(Environment.NewLine +messageLog.ToString());
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

                foreach (KeyValuePair<C7_10tblGPPaymentInstructions, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.Fund + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.FundName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }
    }

    public class UnderlyingFundTypeImport {
        public static List<string> GetUnderlyingFundTypesFromBlue() {
				using (BlueEntities context = new BlueEntities()) {
					return context.C8_50tblFundType.Select(x => x.FundType).ToList();
				}
        }

        public static List<DeepBlue.Models.Entity.UnderlyingFundType> GetUnderlyingFundTypesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.UnderlyingFundType> underlyingFundTypes = new List<DeepBlue.Models.Entity.UnderlyingFundType>();
            // GET: /Admin/UnderlyingList
            // Send the request 
            Util.Log("<=====================================Getting Underlying Fund Types from DeepBlue===============================================>");
            string url = HttpWebRequestUtil.GetUrl("Admin/UnderlyingFundTypeList?pageIndex=1&pageSize=50&sortName=Name&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Entity.UnderlyingFundType ufType = new DeepBlue.Models.Entity.UnderlyingFundType();
                                ufType.UnderlyingFundTypeID = Convert.ToInt32(row.cell[0]);
                                ufType.Name = Convert.ToString(row.cell[1]);
                                underlyingFundTypes.Add(ufType);
                                Util.Log(string.Format("Underlying Fund Type ID: {0}, Name: {1}", ufType.UnderlyingFundTypeID, ufType.Name));
                            }
                        } else {
                            Util.WriteError("No Underlying Fund Type found from Deep Blue");
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return underlyingFundTypes;
        }

        public static void SyncUnderlyingFundTypes(CookieCollection cookies) {
            // Make sure that all the UnderlyingFundTypes are present in Blue
            List<string> blueUFTypes = GetUnderlyingFundTypesFromBlue();
            List<DeepBlue.Models.Entity.UnderlyingFundType> deepblueUFTypes = GetUnderlyingFundTypesFromDeepBlue(cookies);
            List<DeepBlue.Models.Entity.UnderlyingFundType> new_deepblueUFTypes = new List<Models.Entity.UnderlyingFundType>();

            foreach (string blueUFType in blueUFTypes) {
                Util.Log("Blue UF:" + blueUFType);
                DeepBlue.Models.Entity.UnderlyingFundType deepBlueUFType = deepblueUFTypes.Where(x => x.Name == blueUFType).FirstOrDefault();
                if (deepBlueUFType == null) {
                    // Add the new UF Type
                    CreateNewUnderlyingFundType(cookies, blueUFType);
                }
            }
        }

        private static void CreateNewUnderlyingFundType(CookieCollection cookies, string underlyingFundTypeName) {
            DeepBlue.Models.Admin.EditUnderlyingFundTypeModel model = new DeepBlue.Models.Admin.EditUnderlyingFundTypeModel();
            model.Name = underlyingFundTypeName;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateUnderlyingFundType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (!resp.ToLower().StartsWith("true")) {
                                Util.Log(string.Format("Cannot create UnderlyingFundType: (0). Reason: {1}", underlyingFundTypeName, resp));
                            } else {
                                Util.Log(string.Format(" UnderlyingFundType: (0) created. Reason: {1}", underlyingFundTypeName, resp));
                            }
                        } else {
                            Util.Log(string.Format("Cannot create UnderlyingFundType: (0). Reason: response empty from server", underlyingFundTypeName));
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
        }
    }

    public class IndustryFocusImport {
        public static List<string> GetIndustriesFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C8_50tblIndustryFocus.Select(x => x.IndustryFocus).ToList();
            }
        }

        public static List<DeepBlue.Models.Entity.Industry> GetIndustriesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.Industry> industries = new List<DeepBlue.Models.Entity.Industry>();
            // GET: /Admin/UnderlyingList
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/IndustryList?pageIndex=1&pageSize=50&sortName=Industry1&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Entity.Industry industry = new DeepBlue.Models.Entity.Industry();
                                industry.IndustryID = Convert.ToInt32(row.cell[0]);
                                industry.Industry1 = Convert.ToString(row.cell[1]);
                                industries.Add(industry);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return industries;
        }

        public static void SyncIndustryFocuses(CookieCollection cookies) {
            // Make sure that all the UnderlyingFundTypes are present in Blue
            List<string> blueIndustries = GetIndustriesFromBlue();
            List<DeepBlue.Models.Entity.Industry> deepblueIndustries = GetIndustriesFromDeepBlue(cookies);
            List<DeepBlue.Models.Entity.Industry> new_deepblueIndustries = new List<Models.Entity.Industry>();

            foreach (string blueIndustry in blueIndustries) {
                DeepBlue.Models.Entity.Industry deepBlueIndustry = deepblueIndustries.Where(x => x.Industry1 == blueIndustry).FirstOrDefault();
                if (deepBlueIndustry == null) {
                    // Add the new UF Type
                    CreateNewIndustry(cookies, blueIndustry);
                }
            }
        }

        private static void CreateNewIndustry(CookieCollection cookies, string industryName) {
            DeepBlue.Models.Admin.EditIndustryModel model = new DeepBlue.Models.Admin.EditIndustryModel();
            model.Industry = industryName;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateIndustry");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (!resp.ToLower().StartsWith("true")) {
                                Util.Log(string.Format("Cannot create Industry: (0). Reason: {1}", industryName, resp));
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
        }
    }

    public class ReportingTypeImport {
        public static List<string> GetReportingTypesFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C8_50tblReportingType.Select(x => x.ReportingType).ToList();
            }
        }

        public static List<DeepBlue.Models.Entity.ReportingType> GetReportingTypesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.ReportingType> reportingTypes = new List<DeepBlue.Models.Entity.ReportingType>();
            // GET: /Admin/UnderlyingList
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/ReportingTypeList?pageIndex=1&pageSize=50&sortName=Reporting&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Entity.ReportingType rType = new DeepBlue.Models.Entity.ReportingType();
                                rType.ReportingTypeID = Convert.ToInt32(row.cell[0]);
                                rType.Reporting = Convert.ToString(row.cell[1]);
                                reportingTypes.Add(rType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return reportingTypes;
        }

        public static void SyncReportingTypes(CookieCollection cookies) {
            // Make sure that all the ReportingTypes are present in Blue
            List<string> blueReportingTypes = GetReportingTypesFromBlue();
            List<DeepBlue.Models.Entity.ReportingType> deepblueReportingTypes = GetReportingTypesFromDeepBlue(cookies);

            foreach (string blueReportingType in blueReportingTypes) {
                DeepBlue.Models.Entity.ReportingType deepBlueUFType = deepblueReportingTypes.Where(x => x.Reporting == blueReportingType).FirstOrDefault();
                if (deepBlueUFType == null) {
                    // Add the new UF Type
                    CreateNewReportingType(cookies, blueReportingType);
                }
            }
        }

        private static void CreateNewReportingType(CookieCollection cookies, string reportingTypeName) {
            DeepBlue.Models.Admin.EditReportingTypeModel model = new DeepBlue.Models.Admin.EditReportingTypeModel();
            model.Reporting = reportingTypeName;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateReportingType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (!resp.ToLower().StartsWith("true")) {
                                Util.Log(string.Format("Cannot create ReportingType: (0). Reason: {1}", reportingTypeName, resp));
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
        }
    }

    public class ReportingFrequencyImport {
        public static List<DeepBlue.Models.Entity.ReportingFrequency> GetReportingFrequenciesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.ReportingFrequency> reportingFrequencies = new List<DeepBlue.Models.Entity.ReportingFrequency>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/ReportingFrequencyList?pageIndex=1&pageSize=50&sortName=ReportingFrequency1&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Entity.ReportingFrequency rType = new DeepBlue.Models.Entity.ReportingFrequency();
                                rType.ReportingFrequencyID = Convert.ToInt32(row.cell[0]);
                                rType.ReportingFrequency1 = Convert.ToString(row.cell[1]);
                                reportingFrequencies.Add(rType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return reportingFrequencies;
        }
        private static void CreateNewReportingFrequency(CookieCollection cookies, string reportingFrequency) {
            DeepBlue.Models.Admin.EditReportingFrequencyModel model = new DeepBlue.Models.Admin.EditReportingFrequencyModel();
            model.ReportingFrequency = reportingFrequency;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateReportingFrequency");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (!resp.ToLower().StartsWith("true")) {
                                Util.Log(string.Format("Cannot create ReportingFrequency: (0). Reason: {1}", reportingFrequency, resp));
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
        }
    }

    public class GeographyImport {
        public static List<DeepBlue.Models.Entity.Geography> GetGeographiesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.Geography> geographies = new List<DeepBlue.Models.Entity.Geography>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/GeographyList?pageIndex=1&pageSize=50&sortName=Geography1&sortOrder=");
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Entity.Geography geography = new DeepBlue.Models.Entity.Geography();
                                geography.GeographyID = Convert.ToInt32(row.cell[0]);
                                geography.Geography1 = Convert.ToString(row.cell[1]);
                                geographies.Add(geography);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return geographies;
        }



        private static void CreateNewGeographyImport(CookieCollection cookies, string geography) {
            DeepBlue.Models.Admin.EditGeographyModel model = new DeepBlue.Models.Admin.EditGeographyModel();
            model.Geography = geography;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateReportingFrequency");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            if (!resp.ToLower().StartsWith("true")) {
                                Util.Log(string.Format("Cannot create Geography: (0). Reason: {1}", geography, resp));
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
        }
    }
}
