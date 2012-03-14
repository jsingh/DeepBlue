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

namespace DeepBlue.ImportData {
    /// <summary>
    /// Currently, a Direct can be either Equity or Fixed Income. 4-20tblStockTable looks like it holds stocks, so we are going to import
    /// 4-20tblStockTable => Equity. So basically
    /// </summary>
    class DirectsImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C4_20tblStockTable, Exception>> Errors = new List<KeyValuePair<C4_20tblStockTable, Exception>>();
        public static List<KeyValuePair<Equity, Exception>> ImportErrors = new List<KeyValuePair<Equity, Exception>>();
        private static StringBuilder messageLog = new StringBuilder();
        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static NameValueCollection ImportEquities(CookieCollection cookies) {
            NameValueCollection values = new NameValueCollection();
            ImportErrors = new List<KeyValuePair<Equity, Exception>>();
            TotalImportRecords = 0;
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            List<Equity> equities = ConvertBlueToDeepBlue();
            LogErrors(Errors);
            TotalImportRecords = equities.Count;
            Util.Log(string.Format("Trying to import {0} equities", TotalImportRecords));
            foreach (Equity equity in equities) {
                string resp = string.Empty;
                int? newDirect = null;
                try {
                    EquityType eqType = Globals.EquityTypes.Where(x=>x.EquityTypeID == equity.EquityTypeID).FirstOrDefault();
                    if(eqType == null){
                        string err = "could not find equity type with id:" + equity.EquityTypeID;
                        Util.WriteError(err);
                        ImportErrors.Add(new KeyValuePair<Equity, Exception>(equity, new Exception(err)));
                    } else {
                        if (IsDirectAlreadyPresent(equity.IssuerID, eqType.Equity, equity.Symbol)) {
                            string eqAlreadyPresent = string.Format("Equity with IssuerID: {0}, Symbol: {1}, EquityType: {2} already present", equity.IssuerID, equity.Symbol, eqType.Equity);
                            Util.WriteWarning(eqAlreadyPresent);
                            messageLog.AppendLine(eqAlreadyPresent);
                        } else {
                            messageLog.AppendLine("Attemption to create equity: "+equity.Symbol);
                            newDirect = CreateDirect(equity, cookies, out resp);
                            if (newDirect.HasValue) {
                                RecordsImportedSuccessfully++;
                                string successMsg = "SUCCESS: created new equity. ID:  " + newDirect.Value;
                                messageLog.AppendLine(successMsg);
                                Util.WriteNewEntry(successMsg);
                            } else {
                                string error = string.Format("Direct {0} could not be created, response from the server: {1}", equity.Symbol, resp);
                                messageLog.AppendLine("FAILURE: cannot create equity");
                                Util.WriteError(error);
                                ImportErrors.Add(new KeyValuePair<Equity, Exception>(equity, new Exception(error)));
                            }
                        }
                    }
                } catch (Exception ex) {
                    Util.Log(string.Format("Direct {0} could not be created. Exception: {1}", equity.Symbol, ex.Message));
                    ImportErrors.Add(new KeyValuePair<Equity,Exception>( equity, new Exception(resp, ex)));
                }
            }
            Util.Log(string.Format("{0} equities were imported successfully", RecordsImportedSuccessfully));
            
            LogErrors(ImportErrors);
            LogMessages();
            return values;
        }
        
        public static int? CreateDirect(Equity equity, CookieCollection cookies, out string resp) {
            resp = string.Empty;
            int? id = null;
            EquityDetailModel eqDetail = new EquityDetailModel();
            eqDetail.EquitySymbol = equity.Symbol;
            eqDetail.EquityComments = equity.Comments;
            eqDetail.IssuerId = equity.IssuerID;
            eqDetail.EquityTypeId = equity.EquityTypeID;
            // For some reason, the UI lets the user also update the issuer..So we have to include those fields also.. We should probably change that so
            // the the issuer should not be allowed to be updated
            IssuerDetailModel issuerModel = new IssuerDetailModel();
            Issuer issuer = IssuerImport.GetIssuer(equity.IssuerID, cookies);
            NameValueCollection formValues = new NameValueCollection();
            if (issuer != null) {
                issuerModel.IssuerId = issuer.IssuerID;
                issuerModel.CountryId = issuer.CountryID;
                issuerModel.Name = issuer.Name;
                formValues = HttpWebRequestUtil.SetUpForm(issuerModel, string.Empty, string.Empty);


                formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(eqDetail, string.Empty, string.Empty));
                // Send the request 
                string url = HttpWebRequestUtil.GetUrl("Deal/UpdateIssuer");
                string data = HttpWebRequestUtil.ToFormValue(formValues);
                messageLog.AppendLine("Form Data: " + data);
                byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
                HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                    using (Stream receiveStream = response.GetResponseStream()) {
                        // Pipes the stream to a higher level stream reader with the required encoding format. 
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                            resp = readStream.ReadToEnd();
                            messageLog.AppendLine("Response: " + resp);
                            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
                            object obj = jsonSerialize.DeserializeObject(resp);
                            dynamic dataObj = obj;
                            string result = dataObj["data"];
                            if (result.ToLower().StartsWith("true")) {
                                id = HttpWebRequestUtil.GetNewKeyFromResponse(result);
                            }


                            response.Close();
                            readStream.Close();
                        }
                    }
                }
            } else {
                resp = "Cannot create equity. Could not find find issuer with issuerid: " + equity.IssuerID;
                Util.Log(resp);
            }
            return id;
        }

        public static List<Equity> ConvertBlueToDeepBlue() {
            List<Equity> retEquities = new List<Equity>();
            Errors = new List<KeyValuePair<C4_20tblStockTable, Exception>>();
            TotalConversionRecords = 0;
            RecordsConvertedSuccessfully = 0;
            Util.Log("Fetching Directs from Blue............");
            using (BlueEntities context = new BlueEntities()) {
                List<C4_20tblStockTable> equities = context.C4_20tblStockTable.ToList();
                foreach (C4_20tblStockTable equity in equities) {
                    try {
                        TotalConversionRecords++;
                        Equity eq = new Equity();
                        DeepBlue.Models.Deal.DirectListModel model = Globals.Issuers.Where(x => x.DirectName.Equals(equity.Company,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (model != null) {
                            eq.IssuerID = model.DirectId;
                            EquityType eqType = Globals.EquityTypes.Where(x => x.Equity.Equals(equity.Security, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (eqType != null) {
                                eq.EquityTypeID = eqType.EquityTypeID;
                                // Blue has StockSymbol, Ticker, StockName
                                eq.Symbol = equity.StockSymbol;
                                eq.Public = equity.PrivateStock.HasValue ? !equity.PrivateStock.Value : true;
                                retEquities.Add(eq);
                                RecordsConvertedSuccessfully++;
                            } else {
                                string error = string.Format("Couldn't find equity type: {0} for equity.Company: {1}, PrivateStock: {2}, StockName: {3}, StockSymbol: {4}", equity.Security, equity.Company, equity.PrivateStock, equity.StockName, equity.StockSymbol);
                                Errors.Add(new KeyValuePair<C4_20tblStockTable, Exception>(equity, new Exception(error)));
                                Util.WriteError(error);
                            }
                            // WARNING: These fields are present in DeepBlue but absent in blue
                            //eq.ShareClassTypeID;
                            //eq.IndustryID;
                            //eq.CurrencyID;
                            //eq.ISIN;
                            //eq.Comments;
                        } else {
                            string error = string.Format("Couldn't find issuer: {0}", equity.Company);
                            Errors.Add(new KeyValuePair<C4_20tblStockTable, Exception>(equity, new Exception(error)));
                            Util.Log("ConvertBlueToDeepBlue() " + error);
                        }
                    } catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C4_20tblStockTable, Exception>(equity, ex));
                        Util.WriteError(ex.Message);
                        Util.Log("ConvertBlueToDeepBlue() " + ex);
                    }
                }
            }
            Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalConversionRecords, RecordsConvertedSuccessfully));
            
           
            return retEquities;
        }

        public static List<AutoCompleteListExtend> GetDirects(CookieCollection cookies, string directName = null) {
            // Deal/FindEquityFixedIncomeIssuers?term=
            List<AutoCompleteListExtend> directs = new List<AutoCompleteListExtend>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/FindEquityFixedIncomeIssuers?term=" + (string.IsNullOrEmpty(directName) ? string.Empty : directName));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            List<AutoCompleteListExtend> jsonFunds = (List<AutoCompleteListExtend>)js.Deserialize(resp, typeof(List<AutoCompleteListExtend>));
                            directs = jsonFunds;
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return directs;
        }

        private static bool IsDirectAlreadyPresent(int issuerId, string equityType, string symbol) {
            return GetDirectsForIssuer(Globals.CookieContainer, issuerId).Where(x => x.EquityType.Equals(equityType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null;
        }

        public static List<DirectList> GetDirectsForIssuer(CookieCollection cookies, int issuerId) {
            List<DirectList> directs = new List<DirectList>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingDirectList?pageIndex=1&pageSize=5000&sortName=Symbol&sortOrder=asc&companyId=" + issuerId);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            GridData flexiGrid = (GridData)js.Deserialize(resp, typeof(GridData));
                            List<object> rows = (List<object>)flexiGrid.rows;
                            foreach (var obj in rows) {
                                dynamic data = obj;
                                DirectList list = new DirectList();
                                list.ID = data["ID"];
                                list.Industry = data["Industry"];
								list.SecurityType = data["SecurityType"];
								list.EquityType = string.Empty;
								list.FixedIncomeType = string.Empty;
								if (list.SecurityType == "Equity") {
									list.EquityType = data["Security"];
								}
								if (list.SecurityType == "FixedIncome") {
									list.FixedIncomeType = data["Security"];
								}
                                list.Symbol = data["Symbol"];
                                directs.Add(list);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return directs;
        }

        public class DirectList {
            public int ID { get; set; }
            public string SecurityType { get; set; }
            public string Symbol { get; set; }
            public string Industry { get; set; }
            public string EquityType { get; set; }
            public string FixedIncomeType { get; set; }
        }

        private static void LogMessages() {
            using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
                tw.WriteLine(Environment.NewLine +messageLog.ToString());
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<C4_20tblStockTable, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

                foreach (KeyValuePair<C4_20tblStockTable, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.Company + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<Equity, Exception>>  errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<Equity, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.Symbol + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }
    }

    /// <summary>
    /// Distinct 4-20tblStockTable.Company => Issuer (in DeepBlue)
    /// </summary>
    public class IssuerImport {
        public static void SyncIssuers(CookieCollection cookies) {
            List<string> blueIssuers = GetIssuersFromBlue();
            List<DeepBlue.Models.Deal.DirectListModel> deepblueIssuers = GetIssuersFromDeepBlue(cookies);
            //foreach (DeepBlue.Models.Deal.DirectListModel model in deepblueIssuers) {
            //    Util.Log(model.DirectName);
            //}
            
            Util.Log("Issuer import. Total Records to be imported: " + blueIssuers.Count);
            int totalFailures = 0;
            
            foreach (string blueIssuer in blueIssuers) {
                DeepBlue.Models.Deal.DirectListModel deepBlueIssuer = deepblueIssuers.Where(x => x.DirectName == blueIssuer).FirstOrDefault();
                if (deepBlueIssuer == null) {
                    // Add the new issuer

					Util.Log("Create new issuer start");
                    int? issuer = CreateNewIssuer(cookies, blueIssuer);
                    if (!issuer.HasValue) {
                        totalFailures++;
                    }
                }
            }

            Util.Log("Total import failures: " + totalFailures);
        }

        public static List<string> GetIssuersFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C4_20tblStockTable.Select(x => x.Company).Distinct().ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="isDirect"></param>
        /// <returns></returns>
        public static List<DeepBlue.Models.Deal.DirectListModel> GetIssuersFromDeepBlue(CookieCollection cookies, bool isDirect = false) {
            //from issuer in context.Issuers
            //    where issuer.IsGP == isGP && (companyId > 0 ? issuer.IssuerID == companyId : issuer.IssuerID > 0)
            //    select new DirectListModel {
            //                DirectId = issuer.IssuerID,
            //                DirectName = issuer.Name
            //    });
            List<DeepBlue.Models.Deal.DirectListModel> issuers = new List<DeepBlue.Models.Deal.DirectListModel>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/DirectList?pageIndex=1&pageSize=5000&sortName=DirectName&sortOrder=asc&companyId=0&isGP="+isDirect);
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
                                DeepBlue.Models.Deal.DirectListModel directListModel = new DeepBlue.Models.Deal.DirectListModel();
                                directListModel.DirectId = Convert.ToInt32(row.cell[0]);
                                directListModel.DirectName = Convert.ToString(row.cell[1]);
                                issuers.Add(directListModel);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return issuers;
        }

        private static int? CreateNewIssuer(CookieCollection cookies, string issuerName) {
            int? issuerId = null;
			Util.Log("CreateNewIssuer: cookie start " + issuerName);
			DeepBlue.Models.Deal.IssuerDetailModel model = new DeepBlue.Models.Deal.IssuerDetailModel();
            model.Name = issuerName;
            // Issuers are of 2 types, GP and nonGP. GP => Underlying funds, nonGP => directs
            model.IsUnderlyingFundModel = false;
            model.CountryId = Globals.DefaultCountryID;
			Util.Log("CreateNewIssuer: cookie start SetUpForm " + issuerName);
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
			Util.Log("CreateNewIssuer: " + issuerName);
            string url = HttpWebRequestUtil.GetUrl("Deal/CreateIssuer");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        issuerId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                        if (!issuerId.HasValue || issuerId.Value <= 0) {
                            Util.Log("Unable to create issuer: " + issuerName + ". Response from server: " + resp);
                        }
                    }
                }
            }
			Util.Log("CreateNewIssuer: " + issuerId);
            return issuerId;
        }

        public static Issuer GetIssuer(int issuerId, CookieCollection cookies) {
            Issuer issuer = null;
            string url = HttpWebRequestUtil.GetUrl("Deal/FindIssuer?id=" + issuerId);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            CreateIssuerModel model = (CreateIssuerModel)js.Deserialize(resp, typeof(CreateIssuerModel));
                            issuer =  new Issuer();
                            issuer.IssuerID = issuerId;
							if(model.IssuerDetailModel.AnnualMeetingDate.HasValue)
								issuer.AnnualMeetingDate = model.IssuerDetailModel.AnnualMeetingDate.Value.Date;

                            issuer.CountryID = model.IssuerDetailModel.CountryId;
                            issuer.IsGP = model.IssuerDetailModel.IsUnderlyingFundModel;
                            issuer.Name = model.IssuerDetailModel.Name;
                            issuer.ParentName = model.IssuerDetailModel.ParentName;
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return issuer;
        }
    }

    /// <summary>
    /// Currently, in Blue, there is no distinguishing between EquityType and ShareClassType. Since ShareClassType is optional,
    /// we will create all the distinct 4-20tblStockTable.Security => EquityType (in DeepBlue)
    /// </summary>
    public class EquityTypeImport {
        public static void SyncEquityTypes(CookieCollection cookies) {
            // Make sure that all the UnderlyingFundTypes are present in Blue
            List<string> blueEquityTypes = GetEquityTypesFromBlue();
            List<DeepBlue.Models.Entity.EquityType> deepblueEquityTypes = GetEquityTypesFromDeepBlue(cookies);
            //foreach (DeepBlue.Models.Entity.EquityType eqType in deepblueEquityTypes) {
            //    Util.Log(eqType.Equity);
            //}
            Util.Log("Equity Type import. Total Records to be imported: " + deepblueEquityTypes.Count);
            int totalFailures = 0;
            foreach (string blueEquityTyoe in blueEquityTypes) {
                if (blueEquityTyoe == "common") {
                    int i = 0;
                }
                DeepBlue.Models.Entity.EquityType deepEquityType = deepblueEquityTypes.Where(x => x.Equity == blueEquityTyoe.Trim()).FirstOrDefault();
                if (deepEquityType == null) {
                    // Add the new issuer
                    int? equityType = CreateNewEquityType(cookies, blueEquityTyoe);
                    if (!equityType.HasValue) {
                        totalFailures++;
                    }
                }
            }
            Util.Log("Total import failures: " + totalFailures);
        }

        public static List<string> GetEquityTypesFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C4_20tblStockTable.Select(x => x.Security).Distinct().ToList();
            }
        }

        public static List<DeepBlue.Models.Entity.EquityType> GetEquityTypesFromDeepBlue(CookieCollection cookies) {
            List<DeepBlue.Models.Entity.EquityType> equityTypes = new List<DeepBlue.Models.Entity.EquityType>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/EquityTypeList?pageIndex=1&pageSize=5000&sortName=Equity&sortOrder=");
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
                                DeepBlue.Models.Entity.EquityType equityType = new DeepBlue.Models.Entity.EquityType();
                                equityType.EquityTypeID = Convert.ToInt32(row.cell[0]);
                                equityType.Equity = Convert.ToString(row.cell[1]);
                                equityType.Enabled = Convert.ToBoolean(row.cell[2]);
                                equityTypes.Add(equityType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return equityTypes;
        }

        private static int? CreateNewEquityType(CookieCollection cookies, string equityType) {
            int? equityTypeId = null;
            DeepBlue.Models.Admin.EditEquityTypeModel model = new DeepBlue.Models.Admin.EditEquityTypeModel();
            model.EquityType = equityType;
            model.Enabled = true;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateEquityType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        equityTypeId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                        if (!equityTypeId.HasValue || equityTypeId.Value <= 0) {
                            Util.Log("Unable to create equity type: " + equityType + " Response from server: " + resp);
                        }
                    }
                }
            }
            return equityTypeId;
        }
    }
}
