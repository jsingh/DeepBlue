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
// To Find all Issuers
// /Deal/FindGPs
namespace DeepBlue.ImportData {
    class UFIssuerImport {
        public static List<KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception>>();

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static int? CreateIssuerViaWeb(List<DeepBlue.Models.Entity.Issuer> dbIssuers, CookieCollection cookies) {
            int? issuerId = null;
            ImportErrors = new List<KeyValuePair<Models.Entity.Issuer, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            foreach (DeepBlue.Models.Entity.Issuer issuer in dbIssuers) {
                NameValueCollection formValues = new NameValueCollection();
                TotalImportRecords++;
                try {
                    IssuerDetailModel model = new IssuerDetailModel();
                    model.CountryId = Globals.DefaultCountryID;
                    model.AnnualMeetingDate = DateTime.Now.Date;
                    model.Name = issuer.Name;

                    formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

                    // Send the request 
                    string url = HttpWebRequestUtil.GetUrl("Deal/CreateIssuer");
                    byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
                    HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        using (Stream receiveStream = response.GetResponseStream()) {
                            // Pipes the stream to a higher level stream reader with the required encoding format. 
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                                string resp = readStream.ReadToEnd();
                                issuerId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                                if (issuerId != null) {
                                    RecordsImportedSuccessfully++;
                                } else {
                                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception>(issuer, new Exception(resp)));
                                }
                                response.Close();
                                readStream.Close();
                            }
                        }

                    }
                } catch (Exception ex) {
                    ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception>(issuer, ex));
                }
            }
            LogErrors(ImportErrors);
            return issuerId;
        }

        public static string GetIssuers(string term, CookieCollection cookies) {
            string resp = string.Empty;
            string url = HttpWebRequestUtil.GetUrl("Deal/FindGPs");
            url = url + "?term=" + term;
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return resp;
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.Issuer, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.Name + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    }
                    catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }
    }
}
