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
    class InvestorFundImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C6_20tblLPCommitment, Exception>> Errors = new List<KeyValuePair<C6_20tblLPCommitment, Exception>>();
        public static List<KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>>();
        private static StringBuilder messageLog = new StringBuilder();

        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static NameValueCollection ImportInvestorFunds(CookieCollection cookies) {
            NameValueCollection values = new NameValueCollection();
            ImportErrors = new List<KeyValuePair<Models.Entity.InvestorFund, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<DeepBlue.Models.Entity.InvestorFund> investorFunds = new List<Models.Entity.InvestorFund>();
            using (BlueEntities context = new BlueEntities()) {
                List<C6_20tblLPCommitment> blueInvestorFunds = context.C6_20tblLPCommitment.ToList();
                foreach (C6_20tblLPCommitment blueInvestorFund in blueInvestorFunds) {
                    string err = string.Empty;
                    TotalConversionRecords++;
                    try {
                        Models.Entity.InvestorFund invFund = GetInvestorFundFromBlue(blueInvestorFund, out err);
                        messageLog.AppendLine(string.Format("C6_20tblLPCommitment => InvestorFund: blueInvestorFund.ID: {0}, blueInvestorFund.AmberbrookFundName: {1}, blueInvestorFund.AmberbrookFundNo: {2}, invFund.FundID: {3}, blueInvestorFund.LimitedPartner: {4}, invFund.InvestorID: {5}, blueInvestorFund.CommitmentAmount: {6}, invFund.TotalCommitment: {7}", blueInvestorFund.ID, blueInvestorFund.AmberbrookFundName, blueInvestorFund.AmberbrookFundNo, invFund.FundID, blueInvestorFund.LimitedPartner, invFund.InvestorID, blueInvestorFund.CommitmentAmount, invFund.TotalCommitment));
                        investorFunds.Add(invFund);
                        RecordsConvertedSuccessfully++;
                    } catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C6_20tblLPCommitment, Exception>(blueInvestorFund, ex));
                    }
                }
            }

            LogErrors(Errors);
            foreach (DeepBlue.Models.Entity.InvestorFund investorFund in investorFunds) {
                string msg = string.Empty;
                bool? investorFundAlreadyPresent = IsInvestorFundAlreadyPresent(investorFund.InvestorID, investorFund.FundID, investorFund.InvestorTypeId.Value, investorFund.TotalCommitment, out msg);
                if (investorFundAlreadyPresent.HasValue && !investorFundAlreadyPresent.Value) {
                    NameValueCollection formValues = new NameValueCollection();
                    TotalImportRecords++;
                    try {
                        DeepBlue.Models.Transaction.CreateModel model = new Models.Transaction.CreateModel();
                        int? fundClosingID = FundClosingImport.GetFundClosingID(investorFund.FundID);
                        if (fundClosingID.HasValue) {
                            model.FundClosingId = fundClosingID.Value;
                            model.FundId = investorFund.FundID;
                            model.InvestorId = investorFund.InvestorID;
                            model.InvestorTypeId = investorFund.InvestorTypeId.Value;
                            model.TotalCommitment = investorFund.TotalCommitment;

                            formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty, new string[] { "FundClosings", "InvestorTypes", "EditCommitmentAmountModel", "EditModel" }));

                            // Send the request 
                            string url = HttpWebRequestUtil.GetUrl("Transaction/CreateInvestorFund");
                            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
                            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                                using (Stream receiveStream = response.GetResponseStream()) {
                                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                                        string resp = readStream.ReadToEnd();
                                        if (string.IsNullOrEmpty(resp)) {
                                            RecordsImportedSuccessfully++;
                                            values = values.Combine(formValues);
                                        } else {
                                            ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>(investorFund, new Exception(resp)));
                                        }
                                        response.Close();
                                        readStream.Close();
                                    }
                                }
                            }
                        } else {
                            string errMsg = "Unable to create/get fundClosing ";
                            Util.WriteError(errMsg);
                            ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>(investorFund, new Exception(errMsg)));
                        }
                    } catch (Exception ex) {
                        ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>(investorFund, ex));
                    }
                } else {
                    string alreadyExistsMsg = string.Format("Investor already exists for InvestorId: {0}, FundId: {1}, CommitmentAmount: {2}, InvestorTypeId: {3}", investorFund.InvestorID, investorFund.FundID, investorFund.TotalCommitment, investorFund.InvestorTypeId);
                    Util.WriteWarning(alreadyExistsMsg);
                    messageLog.AppendLine(alreadyExistsMsg);
                }
            }
            LogErrors(ImportErrors);
            LogMessages();
            return values;
        }

        private static bool? IsInvestorFundAlreadyPresent(int investorId, int fundId, int investorTypeId, decimal commitmentAmount, out string resp) {
            List<DeepBlue.Models.Investor.FundInformation> investorCommitments = GetInvestmentDetailList(Globals.CookieContainer, investorId, out resp);
            return investorCommitments.Where(x => x.FundId == fundId).Where(x => x.InvestorTypeId == investorTypeId).Where(x => x.TotalCommitment == commitmentAmount).FirstOrDefault() != null;
        }

        private static void LogMessages() {
            using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
                tw.WriteLine(Environment.NewLine +messageLog.ToString());
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<C6_20tblLPCommitment, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

                foreach (KeyValuePair<C6_20tblLPCommitment, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.AmberbrookFundName + ":" + (kv.Key.LimitedPartner + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + kv.Value.StackTrace));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.InvestorFund, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.FundID + ":" + kv.Key.InvestorID + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static Hashtable _investors = new Hashtable();
        private static int? GetInvestorID(string investorName) {
            if (!_investors.ContainsKey(investorName)) {
                Models.Entity.Investor investor = InvestorImport.GetInvestors(Globals.CookieContainer, null, investorName).FirstOrDefault();
                if (investor != null) {
                    _investors.Add(investorName, investor.InvestorID);
                } else {
                    return null;
                }
            }
            return (int)_investors[investorName];
        }

        private static Hashtable _funds = new Hashtable();
        private static int? GetFundID(string fundName) {
            if (!_funds.ContainsKey(fundName)) {
                Models.Entity.Fund fund = FundImport.GetFund(fundName, Globals.CookieContainer);
                if (fund != null) {
                    _funds.Add(fund.FundName, fund.FundID);
                } else {
                    return null;
                }
            }
            return (int)_funds[fundName];
        }

        private static DeepBlue.Models.Entity.InvestorFund GetInvestorFundFromBlue(C6_20tblLPCommitment blueInvestorFund, out string resp) {
            resp = string.Empty;
            DeepBlue.Models.Entity.InvestorFund investorFund = new DeepBlue.Models.Entity.InvestorFund();
            int? investorID = GetInvestorID(blueInvestorFund.LimitedPartner);
            int? fundID = GetFundID(blueInvestorFund.AmberbrookFundName);
            if (investorID.HasValue) {
                investorFund.InvestorID = investorID.Value;
            } else {
                string err = "Unable to Find Investor:" + blueInvestorFund.LimitedPartner;
                resp += err;
                Util.WriteError(err);
            }
            if (fundID.HasValue) {
                investorFund.FundID = fundID.Value;
            } else {
                string err = "Unable to Find Fund:" + blueInvestorFund.AmberbrookFundName;
                resp += err;
                Util.WriteError(err);
            }
            investorFund.InvestorTypeId = !string.IsNullOrEmpty(blueInvestorFund.Designation) && blueInvestorFund.Designation.Contains("Managing") ? (int)Models.Investor.Enums.InvestorType.ManagingMember : (int)Models.Investor.Enums.InvestorType.ManagingMember;
            investorFund.TotalCommitment = blueInvestorFund.CommitmentAmount;
            investorFund.UnfundedAmount = blueInvestorFund.CommitmentAmount;

            return investorFund;
        }
        
        // Transaction/InvestmentDetailList?pageIndex=1&pageSize=5000&sortName=FundName&sortOrder=asc&investorId=27
        private static Hashtable _commitmentAmounts = new Hashtable();
        private static int? GetCommitmentAmount(string investorName) {
            if (!_investors.ContainsKey(investorName)) {
                Models.Entity.Investor investor = InvestorImport.GetInvestors(Globals.CookieContainer, null, investorName).FirstOrDefault();
                if (investor != null) {
                    _investors.Add(investorName, investor.InvestorID);
                } else {
                    return null;
                }
            }
            return (int)_investors[investorName];
        }

        public static List<DeepBlue.Models.Investor.FundInformation> GetInvestmentDetailList(CookieCollection cookies, int investorID, out string resp) {
            resp = string.Empty;
            List<DeepBlue.Models.Investor.FundInformation> fundCommitments = new List<DeepBlue.Models.Investor.FundInformation>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Transaction/InvestmentDetailList?pageIndex=1&pageSize=25&sortName=FundName&sortOrder=asc&investorId=" + investorID);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
                            foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
                                DeepBlue.Models.Investor.FundInformation fundInformation = new DeepBlue.Models.Investor.FundInformation();
                                fundInformation.InvestorFundTransactionId = Convert.ToInt32(row.cell[0]);
                                fundInformation.FundId = Convert.ToInt32(row.cell[1]);
                                fundInformation.FundName = Convert.ToString(row.cell[2]);
                                fundInformation.InvestorTypeId = Convert.ToInt32(row.cell[3]);
                                fundInformation.InvestorType = Convert.ToString(row.cell[4]);
                                fundInformation.TotalCommitment = Convert.ToDecimal(row.cell[5]);
                                fundInformation.UnfundedAmount = Convert.ToDecimal(row.cell[6]);
                                fundInformation.FundClosingId = Convert.ToInt32(row.cell[7]);
                                fundInformation.FundClose = Convert.ToString(row.cell[8]);
                                fundInformation.InvestorFundId = Convert.ToInt32(row.cell[9]);
                                fundInformation.InvestorId = Convert.ToInt32(row.cell[10]);
                                fundCommitments.Add(fundInformation);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return fundCommitments;
        }
    }

    class FundClosingImport {
        private static Hashtable _fundClosings = new Hashtable();
        public static int? GetFundClosingID(int fundID) {
            if (!_fundClosings.ContainsKey(fundID)) {
                int? fundClosingId = CreateFundClosing(Globals.CookieContainer, fundID);
                if (fundClosingId.HasValue) {
                    _fundClosings.Add(fundID, fundClosingId.Value);
                }
                return fundClosingId;
            }
            return (int)_fundClosings[fundID];
        }

        public static int? CreateFundClosing(CookieCollection cookies, int fundId) {
            int? fundClosingId = null;
            DeepBlue.Models.Admin.EditFundClosingModel fundClosing = new DeepBlue.Models.Admin.EditFundClosingModel();
            fundClosing.Name = "FC-" + DateTime.Now.ToString();
            fundClosing.FundClosingDate = DateTime.Now;
            fundClosing.FundId = fundId;
            fundClosing.IsFirstClosing = false;

            NameValueCollection values = HttpWebRequestUtil.SetUpForm(fundClosing, string.Empty, string.Empty);

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateFundClosing");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(values));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            fundClosingId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        }
                        response.Close();
                        readStream.Close();
                    }
                }

            }

            return fundClosingId;
        }

        //FindFundClosings
        public static List<DeepBlue.Models.Entity.FundClosing> GetFundClosings(CookieCollection cookies, int fundID, out string resp) {
            resp = string.Empty;
            List<DeepBlue.Models.Entity.FundClosing> fundClosings = new List<DeepBlue.Models.Entity.FundClosing>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Fund/FindFundClosings?term=&fundId=" + fundID);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            List<AutoCompleteList> jsonFunds = (List<AutoCompleteList>)js.Deserialize(resp, typeof(List<AutoCompleteList>));
                            foreach (AutoCompleteList alist in jsonFunds) {
                                DeepBlue.Models.Entity.FundClosing fc = new DeepBlue.Models.Entity.FundClosing();
                                fc.FundClosingID = alist.id;
                                fc.Name = alist.value;
                                fundClosings.Add(fc);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return fundClosings;
        }
    }
}
