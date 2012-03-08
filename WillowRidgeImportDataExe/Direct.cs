using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using QueryBuilder.Fixture;
using DeepBlue.ImportData.SourceData;
using DeepBlue.Models.Entity;
using System.Net;
using DeepBlue.Models.Deal;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using DeepBlue.Models.Fund;
using System.IO;

// /Deal/UpdateIssuer
// IssuerDetailModel
// EquityDetailModel
// FixedIncomeDetailModel
namespace DeepBlue.ImportData {
    class DirectImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>> Errors = new List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>>();
        public static List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>>();

        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static NameValueCollection ConvertUnderlyingFundViaWeb(CookieCollection cookies) {
            NameValueCollection values = new NameValueCollection();
            ImportErrors = new List<KeyValuePair<Models.Entity.UnderlyingFund, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<DeepBlue.Models.Entity.UnderlyingFund> dbFunds = ConvertBlueToDeepBlue();
            LogErrors(Errors);
            foreach (DeepBlue.Models.Entity.UnderlyingFund fund in dbFunds) {
                NameValueCollection formValues = new NameValueCollection();
                TotalImportRecords++;
                try {
                    CreateUnderlyingFundModel model = new CreateUnderlyingFundModel();
                    //model.FundName = fund.FundName;
                    //// these fields are required. Although inception date is 
                    //// not required in the database, it is required by the app
                    //model.TaxId = fund.TaxID;
                    //if (fund.InceptionDate != null) {
                    //    model.InceptionDate = fund.InceptionDate;
                    //}
                    //else {
                    //    model.InceptionDate = new DateTime(1900, 1, 1);
                    //}

                    //// The following fields are optional
                    //model.ScheduleTerminationDate = fund.ScheduleTerminationDate;
                    //model.FinalTerminationDate = fund.FinalTerminationDate;
                    //model.NumofAutoExtensions = fund.NumofAutoExtensions;
                    //model.DateClawbackTriggered = fund.DateClawbackTriggered;
                    //model.RecycleProvision = fund.RecycleProvision;
                    //model.MgmtFeesCatchUpDate = fund.MgmtFeesCatchUpDate;
                    //model.Carry = fund.Carry;
                    //formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

                    //FundAccount fundAccount = fund.FundAccounts.First();
                    //FundBankDetail fundAccountModel = new FundBankDetail();
                    //if (!string.IsNullOrEmpty(fundAccount.BankName)) {
                    //    if (fundAccount.BankName.Length > 50) {
                    //        fundAccountModel.BankName = fundAccount.BankName.Substring(0, 50);
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(fundAccount.Account)) {
                    //    if (fundAccount.Account.Length > 50) {
                    //        fundAccountModel.AccountNo = fundAccount.Account.Substring(0, 50);
                    //    }
                    //}
                    //fundAccountModel.ABANumber = fundAccount.Routing;
                    //fundAccountModel.Reference = fundAccount.Reference;
                    //fundAccountModel.AccountOf = fundAccount.AccountOf;
                    //fundAccountModel.Attention = fundAccount.Attention;
                    //fundAccountModel.Telephone = fundAccount.Phone;
                    //fundAccountModel.Fax = fundAccount.Fax;
                    ////WARNING: the following fields are not present in blue
                    ////fundAccount.SWIFT;
                    ////fundAccount.AccountNumberCash;
                    ////fundAccount.FFCNumber;
                    ////fundAccount.IBAN;
                    //formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

                    //// Send the request 
                    //string url = HttpWebRequestUtil.GetUrl("Fund/Create");
                    //byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
                    //HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                    //if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    //    using (Stream receiveStream = response.GetResponseStream()) {
                    //        // Pipes the stream to a higher level stream reader with the required encoding format. 
                    //        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                    //            string resp = readStream.ReadToEnd();
                    //            if (string.IsNullOrEmpty(resp)) {
                    //                RecordsImportedSuccessfully++;
                    //                values = values.Combine(formValues);
                    //            }
                    //            else {
                    //                ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>(fund, new Exception(resp)));
                    //            }
                    //            response.Close();
                    //            readStream.Close();
                    //        }
                    //    }

                    //}
                }
                catch (Exception ex) {
                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>(fund, ex));
                }
            }
            LogErrors(ImportErrors);
            return values;
        }

        public static List<DeepBlue.Models.Entity.UnderlyingFund> ConvertBlueToDeepBlue() {
            Errors = new List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>>();
            TotalConversionRecords = 0;
            RecordsConvertedSuccessfully = 0;
            List<DeepBlue.Models.Entity.UnderlyingFund> dbFunds = new List<DeepBlue.Models.Entity.UnderlyingFund>();
            using (BlueEntities context = new BlueEntities()) {
                List<C7_10tblGPPaymentInstructions> uFunds = context.C7_10tblGPPaymentInstructions.Where(x=>x.FundType == "Direct").ToList();
                foreach (C7_10tblGPPaymentInstructions uf in uFunds) {
                    try {
                        TotalConversionRecords++;
                        DeepBlue.Models.Entity.UnderlyingFund deepBlueFund = GetUnderlyingFundFromBlue(uf);
                        dbFunds.Add(deepBlueFund);
                        RecordsConvertedSuccessfully++;
                    }
                    catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C7_10tblGPPaymentInstructions, Exception>(uf, ex));
                    }
                }
            }
            return dbFunds;
        }

        private static DeepBlue.Models.Entity.UnderlyingFund GetUnderlyingFundFromBlue(C7_10tblGPPaymentInstructions blueUFund) {
            DeepBlue.Models.Entity.UnderlyingFund uf = new DeepBlue.Models.Entity.UnderlyingFund();
            uf.EntityID = Globals.DefaultEntityID;
            // ToDO: IssuerID
            uf.FundName = blueUFund.Fund;
            uf.FundTypeID = GetFundType(blueUFund.FundType);
            uf.IsFeesIncluded = blueUFund.FeesInside.HasValue ? blueUFund.FeesInside.Value : false;
            uf.VintageYear = blueUFund.VintageYear.HasValue ? (short?)blueUFund.VintageYear : null;
            // TODO: Convert FundSize to money in DB and re-gen the model
            uf.TotalSize = Convert.ToInt32(blueUFund.FundSize);
            if (blueUFund.TerminationDate.HasValue) {
                uf.TerminationYear = Convert.ToInt16(blueUFund.TerminationDate.Value.Year);
            }

            // WARNING: these fields are present in blue but absent in deepblue
            // What are these fields used for in blue?
            //blueUFund.Website;
            //blueUFund.WebLogin;
            //blueUFund.WebPassword;

            uf.IndustryID = GetIndustryFocus(blueUFund.Industry_Focus);
            uf.GeographyID = Globals.Geograpies.First().GeographyID;
            uf.ReportingFrequencyID = GetReportingFrequency(blueUFund.Reporting);
            uf.ReportingTypeID = GetReportingType(blueUFund.ReportingType);
            uf.Description = Globals.DefaultString;

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

            //uf.FundRegisteredOfficeID
            // TODO: use uf.FundRegisteredOfficeID to store the address
            Address address = new Address();
            address.Address1 = blueUFund.MailingAddress1;
            address.Address2 = blueUFund.MailingAddress2;
            string[] parts = new string[3];
            if (Util.ParseAddress(address.Address2, out parts)) {
                address.City = parts[0];
                address.State = Globals.States.Where(x => x.Abbr == parts[1].ToUpper().Trim()).First().StateID;
                address.PostalCode = parts[2];
            }

            address.AddressTypeID = (int)DeepBlue.Models.Admin.Enums.AddressType.Work;
            address.Country = Globals.DefaultCountryID;
            //ContactAddress contactAddress = new ContactAddress();
            //     contactAddress.Address = address;
            // contact.ContactAddresses.Add(contactAddress);

            Account account = new Account();
            account.BankName = blueUFund.Bank;
            account.Routing = Convert.ToInt32(blueUFund.ABANumber.Replace("-", string.Empty).Replace(" ", string.Empty));
            account.AccountOf = blueUFund.Accountof;
            account.Account1 = blueUFund.AccountNumber;

            account.Attention = blueUFund.Attn;
            account.Reference = blueUFund.Reference;
            // WARNING: the following fields are present in DeepBlue, but are not present in Blue
            //uf.LegalFundName
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

        private static void LogErrors(List<KeyValuePair<C7_10tblGPPaymentInstructions, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", Investor.TotalConversionRecords, Investor.RecordsConvertedSuccessfully, Investor.Errors.Count));

                foreach (KeyValuePair<C7_10tblGPPaymentInstructions, Exception> kv in errors) {
                    try {
                        tw.WriteLine(kv.Key.Fund + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", Investor.TotalImportRecords, Investor.RecordsImportedSuccessfully, Investor.ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.UnderlyingFund, Exception> kv in errors) {
                    try {
                        tw.WriteLine(kv.Key.FundName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }
    }
}
