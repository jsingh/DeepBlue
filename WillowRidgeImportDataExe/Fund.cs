using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

using DeepBlue.ImportData.SourceData;
using DeepBlue.Models.Entity;
using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using DeepBlue.Models.Fund;
using System.IO;
using System.Web.Script.Serialization;
using DeepBlue.Helpers;

namespace DeepBlue.ImportData {
	class FundImport {
		private static Hashtable StateAbbr = new Hashtable();
		public static List<KeyValuePair<C6_10AmberbrookFundInfo, Exception>> Errors = new List<KeyValuePair<C6_10AmberbrookFundInfo, Exception>>();
		public static List<KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>> ImportErrors = new List<KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>>();
		private static StringBuilder messageLog = new StringBuilder();

		public static int TotalConversionRecords = 0;
		public static int RecordsConvertedSuccessfully = 0;

		public static int TotalImportRecords = 0;
		public static int RecordsImportedSuccessfully = 0;

		public static NameValueCollection ImportFunds(CookieCollection cookies) {
			NameValueCollection values = new NameValueCollection();
			ImportErrors = new List<KeyValuePair<Models.Entity.Fund, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;
			List<DeepBlue.Models.Entity.Fund> dbFunds = ConvertBlueToDeepBlue();
			LogErrors(Errors);
			foreach (DeepBlue.Models.Entity.Fund fund in dbFunds) {
				if (!IsFundAlreadyPresent(fund.FundName)) {
					NameValueCollection formValues = new NameValueCollection();
					TotalImportRecords++;
					try {
						CreateModel model = new CreateModel();
						model.FundName = fund.FundName;
						StringBuilder sb = new StringBuilder();
						sb.Append("Importing fund: ").AppendLine(fund.FundName);
						// these fields are required. Although inception date is 
						// not required in the database, it is required by the app
						model.TaxId = fund.TaxID;
						if (fund.InceptionDate != null) {
							if (fund.InceptionDate.HasValue)
								model.InceptionDate = fund.InceptionDate.Value.Date;
						}
						else {
							model.InceptionDate = new DateTime(1900, 1, 1);
						}

						// The following fields are optional
						if (fund.ScheduleTerminationDate.HasValue)
							model.ScheduleTerminationDate = fund.ScheduleTerminationDate.Value.Date;
						if (fund.FinalTerminationDate.HasValue)
							model.FinalTerminationDate = fund.FinalTerminationDate.Value.Date;

						model.NumofAutoExtensions = fund.NumofAutoExtensions;
						model.DateClawbackTriggered = fund.DateClawbackTriggered;
						model.RecycleProvision = fund.RecycleProvision;
						if(fund.MgmtFeesCatchUpDate.HasValue)
							model.MgmtFeesCatchUpDate = fund.MgmtFeesCatchUpDate.Value.Date;

						model.Carry = fund.Carry;
						formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

						FundAccount fundAccount = fund.FundAccounts.First();
						FundBankDetail fundAccountModel = new FundBankDetail();
						if (!string.IsNullOrEmpty(fundAccount.BankName)) {
							if (fundAccount.BankName.Length > 50) {
								fundAccountModel.BankName = fundAccount.BankName.Substring(0, 50);
							}
						}

						if (!string.IsNullOrEmpty(fundAccount.Account)) {
							if (fundAccount.Account.Length > 50) {
								fundAccountModel.AccountNumber = fundAccount.Account.Substring(0, 50);
							}
						}
						fundAccountModel.ABANumber = fundAccount.Routing;
						fundAccountModel.Reference = fundAccount.Reference;
						fundAccountModel.AccountOf = fundAccount.AccountOf;
						fundAccountModel.Attention = fundAccount.Attention;
						fundAccountModel.AccountPhone = fundAccount.Phone;
						fundAccountModel.AccountFax = fundAccount.Fax;
						//WARNING: the following fields are not present in blue
						//fundAccount.SWIFT;
						//fundAccount.AccountNumberCash;
						//fundAccount.FFCNumber;
						//fundAccount.IBAN;
						formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty));

						// Send the request 
						string url = HttpWebRequestUtil.GetUrl("Fund/Create");
						string formData = HttpWebRequestUtil.ToFormValue(formValues);
						sb.Append("FormData: ").AppendLine(formData);
						byte[] postData = System.Text.Encoding.ASCII.GetBytes(formData);
						HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
						if (response.StatusCode == System.Net.HttpStatusCode.OK) {
							using (Stream receiveStream = response.GetResponseStream()) {
								// Pipes the stream to a higher level stream reader with the required encoding format. 
								using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
									string resp = readStream.ReadToEnd();
									sb.Append("Response: ").AppendLine(resp);
									if (string.IsNullOrEmpty(resp)) {
										RecordsImportedSuccessfully++;
										values = values.Combine(formValues);
										string msg = "Successfully imported" + model.FundName;
										Util.WriteNewEntry(msg);
										sb.AppendLine(msg);
									}
									else {
										string msg = "Failed to import" + model.FundName;
										Util.WriteError(msg);
										sb.AppendLine(msg);
										ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>(fund, new Exception(resp)));
									}
									response.Close();
									readStream.Close();
								}
							}

						}
						messageLog.Append(sb.ToString());
					}
					catch (Exception ex) {
						ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>(fund, ex));
					}
				}
				else {
					string msg = string.Format("Fund: {0} already exists", fund.FundName);
					Util.WriteWarning(msg);
					messageLog.AppendLine(msg);
				}
			}
			LogErrors(ImportErrors);
			LogMessages();
			return values;
		}

		private static bool IsFundAlreadyPresent(string fundName) {
			return GetFunds(Globals.CookieContainer, fundName).FirstOrDefault() != null;
		}

		public static void ConvertInvestorViaDB() {
			ImportErrors = new List<KeyValuePair<Models.Entity.Fund, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;
			List<DeepBlue.Models.Entity.Fund> dbFunds = ConvertBlueToDeepBlue();
			LogErrors(Errors);
			foreach (DeepBlue.Models.Entity.Fund fund in dbFunds) {
				TotalImportRecords++;
				using (DeepBlueEntities context = new DeepBlueEntities()) {
					try {
						context.Funds.AddObject(fund);
						context.SaveChanges();
						RecordsImportedSuccessfully++;
					}
					catch (Exception ex) {
						ImportErrors.Add(new KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>(fund, ex));
					}
				}
			}
			LogErrors(ImportErrors);
		}

		private static void LogMessages() {
			using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
				tw.WriteLine(Environment.NewLine + messageLog.ToString());
				tw.Flush();
				tw.Close();
			}
		}

		private static void LogErrors(List<KeyValuePair<C6_10AmberbrookFundInfo, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

				foreach (KeyValuePair<C6_10AmberbrookFundInfo, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.AmberbrookFundName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
					}
					catch (Exception ex) {
						Util.Log("Error logging exception: " + ex.Message);
					}
				}
				tw.Flush();
				tw.Close();
			}
		}

		private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.Fund, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
				foreach (KeyValuePair<DeepBlue.Models.Entity.Fund, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.FundName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
					}
					catch (Exception ex) {
						Util.Log("Error logging exception: " + ex.Message);
					}
				}
				tw.Flush();
				tw.Close();
			}
		}

		public static List<DeepBlue.Models.Entity.Fund> ConvertBlueToDeepBlue() {
			Errors = new List<KeyValuePair<C6_10AmberbrookFundInfo, Exception>>();
			TotalConversionRecords = 0;
			RecordsConvertedSuccessfully = 0;
			List<DeepBlue.Models.Entity.Fund> dbFunds = new List<DeepBlue.Models.Entity.Fund>();
			using (BlueEntities context = new BlueEntities()) {
				List<C6_10AmberbrookFundInfo> funds = context.C6_10AmberbrookFundInfo.ToList();
				foreach (C6_10AmberbrookFundInfo fund in funds) {
					try {
						TotalConversionRecords++;
						DeepBlue.Models.Entity.Fund deepBlueFund = GetFundFromBlue(fund);
						dbFunds.Add(deepBlueFund);
						RecordsConvertedSuccessfully++;
					}
					catch (Exception ex) {
						Errors.Add(new KeyValuePair<C6_10AmberbrookFundInfo, Exception>(fund, ex));
					}
				}
			}
			return dbFunds;
		}

		private static DeepBlue.Models.Entity.Fund GetFundFromBlue(C6_10AmberbrookFundInfo blueFund) {
			DeepBlue.Models.Entity.Fund fund = new DeepBlue.Models.Entity.Fund();
			fund.FundName = blueFund.AmberbrookFundName;
			
			if(blueFund.SchedulelTermDate.HasValue)
			fund.ScheduleTerminationDate = blueFund.SchedulelTermDate.Value.Date;

			if( blueFund.FinalTermDate.HasValue)
				fund.FinalTerminationDate = blueFund.FinalTermDate.Value.Date;
			// these fields are required. Although inception date is 
			// not required in the database, it is required by the app
			fund.TaxID = Guid.NewGuid().ToString("N").Substring(0, 25);
			if (blueFund.InceptionDate != null) {
				fund.InceptionDate = blueFund.InceptionDate.Date;
			}
			else {
				fund.InceptionDate = new DateTime(1900, 1, 1);
			}

			// WARNING: these fields are not present in Blue
			//fund.NumofAutoExtensions
			//fund.DateClawbackTriggered;
			//fund.RecycleProvision;
			//fund.MgmtFeesCatchUpDate;
			//fund.Carry;
			// WARNING: This field is present in blue but not present in Deepblue
			// blueFund.AmberbrookFundNo

			// Bank
			FundAccount account = new Models.Entity.FundAccount();
			account.BankName = blueFund.Bank;
			account.Routing = Convert.ToInt32(blueFund.ABANumber.Trim().Replace("-", string.Empty).Replace(" ", string.Empty));
			account.Account = blueFund.AccountNumber;
			account.AccountOf = blueFund.Accountof;
			account.Attention = blueFund.Attn;
			if (!string.IsNullOrEmpty(blueFund.AccountNumberCash)) {
				account.AccountNumberCash = blueFund.AccountNumberCash;
			}
			else {
				account.AccountNumberCash = string.Empty;
			}
			account.Fax = blueFund.Fax;
			account.Phone = blueFund.Phone;

			fund.FundAccounts.Add(account);

			return fund;
		}

		public static List<DeepBlue.Models.Entity.Fund> GetFunds(CookieCollection cookies, string term = null) {
			List<DeepBlue.Models.Entity.Fund> funds = new List<DeepBlue.Models.Entity.Fund>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Fund/FindFunds?term=" + (string.IsNullOrEmpty(term) ? string.Empty : term));
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
								DeepBlue.Models.Entity.Fund fund = new Fund();
								fund.FundID = alist.id;
								fund.FundName = alist.value;
								funds.Add(fund);
							}
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return funds;
		}

		public static DeepBlue.Models.CapitalCall.FundDetail GetFundDetail(CookieCollection cookies, int fundId) {
			List<DeepBlue.Models.Entity.Fund> funds = new List<DeepBlue.Models.Entity.Fund>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/FundDetail?id=" + fundId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						string resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							DeepBlue.Models.CapitalCall.FundDetail fundDetail = (DeepBlue.Models.CapitalCall.FundDetail)js.Deserialize(resp, typeof(DeepBlue.Models.CapitalCall.FundDetail));
							return fundDetail;
						}
					}
				}
			}
			return null;
		}

		private static Hashtable DeepBlueFunds = new Hashtable();
		public static DeepBlue.Models.Entity.Fund GetFund(string fundName, CookieCollection cookies) {
			if (DeepBlueFunds.ContainsKey(fundName)) {
				return (DeepBlue.Models.Entity.Fund)DeepBlueFunds[fundName];
			}
			else {
				DeepBlue.Models.Entity.Fund fund = FundImport.GetFunds(cookies, fundName).FirstOrDefault();
				if (fund != null) {
					DeepBlueFunds.Add(fundName, fund);
				}
				return fund;
			}
		}
	}
}
