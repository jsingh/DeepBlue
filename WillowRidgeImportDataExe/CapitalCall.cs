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
using DeepBlue.Models.CapitalCall;

namespace DeepBlue.ImportData {
	/// <summary>
	/// Activity=>Amberbrook funds=>Capital Call
	/// AMB Fund => Investors
	/// </summary>
	class CapitalCallImport {
		public static List<KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception>> Errors = new List<KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception>>();
		public static List<KeyValuePair<CapitalCall, Exception>> ImportErrors = new List<KeyValuePair<CapitalCall, Exception>>();
		private static StringBuilder messageLog = new StringBuilder();

		public static int TotalConversionRecords = 0;
		public static int RecordsConvertedSuccessfully = 0;

		public static int TotalImportRecords = 0;
		public static int RecordsImportedSuccessfully = 0;

		public static void ImportCapitalCall(CookieCollection cookies) {
			ImportErrors = new List<KeyValuePair<CapitalCall, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;
			messageLog.AppendLine("<=========================BEGIN: Converting records Blue => DeepBlue=======================>");
			List<CapitalCall> capitalCalls = ConvertFromBlueToDeepBlue(cookies);
			messageLog.AppendLine("<=========================END: Converting records Blue => DeepBlue=======================>");
			LogErrors(Errors);
			foreach (CapitalCall capitalCall in capitalCalls) {
				TotalImportRecords++;
				try {
					string resp = string.Empty;
					string formData = string.Empty;
					// First make sure that this capital call doesnt already exist in the system
					bool? isCCAlreadyPresent = IsCapitalCallAlreadyPresent(capitalCall);
					if (isCCAlreadyPresent.HasValue && isCCAlreadyPresent.Value) {
						string msg = string.Format("Capital call already present correctponding to CapitalCallAmount: {0}, CapitalCallDate: {1}, CapitalCallDueDate: {2}", capitalCall.CapitalAmountCalled, capitalCall.CapitalCallDate, capitalCall.CapitalCallDueDate);
						Util.WriteWarning(msg);
						messageLog.AppendLine(msg);
					}
					else {
						int? callNo = CreateManualCapitalCall(cookies, capitalCall, out resp, out formData);
						messageLog.AppendLine(string.Format("#{0} Attempting to create capital call:", TotalImportRecords));
						messageLog.Append("Response from server: ").Append(resp).AppendLine();
						messageLog.Append("Form Data: ").AppendLine(formData);
						if (!callNo.HasValue) {
							Util.WriteError("Error creating Capital Call. " + Environment.NewLine
								 + "FundID:" + capitalCall.FundID + Environment.NewLine
								 + "CapitalAmountCalled:" + capitalCall.CapitalAmountCalled + Environment.NewLine
								 + "CapitalCallDate:" + capitalCall.CapitalCallDate + Environment.NewLine
								 + "CapitalCallDueDate:" + capitalCall.CapitalCallDueDate + Environment.NewLine 
								 );
							ImportErrors.Add(new KeyValuePair<CapitalCall, Exception>(capitalCall, new Exception("Error creating Capital Call. Error:" + resp)));
							string msg = "RESULT: FAIL";
							messageLog.AppendLine(msg);
							Util.WriteError(msg + " " + resp);
						}
						else {
							string msg = "RESULT: PASS";
							messageLog.AppendLine(msg);
							Util.WriteNewEntry(msg + " " + resp);
						}
					}
				}
				catch (Exception ex) {
					ImportErrors.Add(new KeyValuePair<CapitalCall, Exception>(capitalCall, ex));
					Util.WriteError(ex.Message);
				}
			}
			LogErrors(ImportErrors);
			LogMessages();
		}

		private static List<CapitalCall> ConvertFromBlueToDeepBlue(CookieCollection cookies) {
			Util.Log("Fetching Capital Calls from Blue............");
			List<CapitalCall> capitalCalls = new List<CapitalCall>();
			using (BlueEntities context = new BlueEntities()) {
				List<C3_10tblCallsandFeesFromAmberbrook> blueCapitalCalls = context.C3_10tblCallsandFeesFromAmberbrook.ToList();
				foreach (C3_10tblCallsandFeesFromAmberbrook blueCapitalCall in blueCapitalCalls) {
					try {
						bool success = true;
						string errorMsg = string.Empty;
						TotalImportRecords++;
						Util.Log("<======================Importing record#" + TotalImportRecords + "======================>");

						CapitalCall capCall = GetCapitalCallFromBlue(blueCapitalCall, context, cookies);
						string msg = string.Format("#{0} Getting capital call C3_10tblCallsandFeesFromAmberbrook => CapitalCall. C3_10tblCallsandFeesFromAmberbrook.TransactionID: {1}, AmbFund#: {2} => DeepBlueFundID: {3}, DueDate: {4}, NoticeDate: {5}, Total amount collected:{6}", TotalImportRecords, blueCapitalCall.TransactionID, blueCapitalCall.AmberbrookFundNo, capCall.FundID, blueCapitalCall.DueDate, blueCapitalCall.NoticeDate, blueCapitalCall.TotalAmountCollected);
						Util.Log(msg);
						messageLog.Append(msg);
						messageLog.Append(Environment.NewLine);

						List<C3_20tblCallsandFeesDistribution> blueCapitalCallLineItems = context.C3_20tblCallsandFeesDistribution.Where(x => x.CallID == blueCapitalCall.TransactionID).ToList();
						foreach (C3_20tblCallsandFeesDistribution blueCapitalCallLineItem in blueCapitalCallLineItems) {
							string resp = string.Empty;
							CapitalCallLineItem lineItem = GetCapitalCallLineItemFromBlue(blueCapitalCallLineItem, capCall.FundID, context, cookies, out resp);
							if (!string.IsNullOrEmpty(resp)) {
								success = false;
								errorMsg += resp;
							}
							else {
								messageLog.Append(string.Format("Line Item(C3_20tblCallsandFeesDistribution => CapitalCallLineItem) level: BlueCallID:{0}, BlueMember:{1} => DeepBlueInvestorId:{2}, BlueCalledCapital:{3} => DeepBlueCapitalAmountCalled:{4},  BlueCalledExpenses:{5} => DeepBlueFundExpenses:{6},  BlueCalledFees:{7} => DeepBlueManagementFees:{8}", blueCapitalCallLineItem.CallID, blueCapitalCallLineItem.Member, lineItem.InvestorID, blueCapitalCallLineItem.CalledCapital ?? 0, lineItem.CapitalAmountCalled, blueCapitalCallLineItem.CalledExpenses ?? 0, lineItem.FundExpenses ?? 0, blueCapitalCallLineItem.CalledFees ?? 0, lineItem.ManagementFees ?? 0));
								messageLog.Append(Environment.NewLine);
							}
							capCall.CapitalCallLineItems.Add(lineItem);
						}
						if (success) {
							RecordsImportedSuccessfully++;
							capitalCalls.Add(capCall);
						}
						else {
							Errors.Add(new KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception>(blueCapitalCall, new Exception(errorMsg)));
						}
					}
					catch (Exception ex) {
						Errors.Add(new KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception>(blueCapitalCall, ex));
						Util.WriteError("ConvertBlueToDeepBlue() " + ex);
					}
				}
			}
			Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalImportRecords, RecordsImportedSuccessfully));
			LogErrors(Errors);
			return capitalCalls;
		}

		private static CapitalCall GetCapitalCallFromBlue(C3_10tblCallsandFeesFromAmberbrook blueCapitalCall, BlueEntities context, CookieCollection cookies) {
			CapitalCall deepBlueCC = new CapitalCall();
			//deepBlueCC.FundID;
			deepBlueCC.CapitalCallDate = blueCapitalCall.NoticeDate;
			if (blueCapitalCall.DueDate.HasValue) {
				deepBlueCC.CapitalCallDueDate = blueCapitalCall.DueDate.Value;
			}
			// The CapitalAmountCalled is used for investing in new investments and existing investments
			// CapitalAmountCalled = NewInvestmentAmount + ExistingInvestmentAmount
			deepBlueCC.CapitalAmountCalled = (decimal)blueCapitalCall.TotalAmountCollected;
			// Capital Call Number
			deepBlueCC.CapitalCallTypeID = (int)DeepBlue.Models.CapitalCall.Enums.CapitalCallType.Manual;
			deepBlueCC.ManagementFees = (decimal)blueCapitalCall.ManagementFees;
			if (blueCapitalCall.Expenses.HasValue) {
				deepBlueCC.FundExpenses = (decimal)blueCapitalCall.Expenses.Value;
			}
			// NetCapital = TotalAmountCollected - ManagementFees - FundExpenses;

			// Initialize the values
			deepBlueCC.NewInvestmentAmount = deepBlueCC.CapitalAmountCalled;
			deepBlueCC.ExistingInvestmentAmount = 0;
			if (blueCapitalCall.NewInvestments.HasValue && blueCapitalCall.NewInvestments.Value > 0) {
				deepBlueCC.NewInvestmentAmount = (decimal)blueCapitalCall.NewInvestments.Value;
			}

			if (blueCapitalCall.OldInvestments.HasValue) {
				deepBlueCC.ExistingInvestmentAmount = (decimal)blueCapitalCall.OldInvestments.Value;
			}

			// InvestmentAmount = NewInvestmentAmount + ExistingInvestmentAmount
			// This is a required field right now, but the UI doesnt ask for it, and this should be calculated at the server.
			// This is a bug.. we should have to set it here, but until this is fixed, set it
			deepBlueCC.InvestmentAmount = deepBlueCC.NewInvestmentAmount.Value + deepBlueCC.ExistingInvestmentAmount.Value;

			int? fundId = GetFundID(blueCapitalCall.AmberbrookFundNo, context, cookies);
			if (fundId.HasValue) {
				deepBlueCC.FundID = fundId.Value;
			}
			return deepBlueCC;
		}

		private static CapitalCallLineItem GetCapitalCallLineItemFromBlue(C3_20tblCallsandFeesDistribution blueCapitalCallLineItem, int fundID, BlueEntities context, CookieCollection cookies, out string resp) {
			resp = string.Empty;
			CapitalCallLineItem deepBlueCCLineItem = new CapitalCallLineItem();
			// Investor ID
			int? investorId = GetInvestorID(blueCapitalCallLineItem.Member, fundID, context, cookies);
			if (investorId.HasValue) {
				deepBlueCCLineItem.InvestorID = investorId.Value;
			}
			else {
				resp = "Unable to find investor:" + blueCapitalCallLineItem.Member;
				Util.WriteError(resp);
			}
			// Capital call amount
			// Capital call amount includes management fees and expenses
			if (blueCapitalCallLineItem.CalledTotal.HasValue) {
				deepBlueCCLineItem.CapitalAmountCalled = (decimal)blueCapitalCallLineItem.CalledTotal.Value;
			}
			// management fees
			if (blueCapitalCallLineItem.CalledFees.HasValue) {
				deepBlueCCLineItem.ManagementFees = (decimal)blueCapitalCallLineItem.CalledFees.Value;
			}
			// Fund expenses
			if (blueCapitalCallLineItem.CalledExpenses.HasValue) {
				deepBlueCCLineItem.FundExpenses = (decimal)blueCapitalCallLineItem.CalledExpenses.Value;
			}

			if (blueCapitalCallLineItem.Paid.HasValue) {
				deepBlueCCLineItem.IsReconciled = blueCapitalCallLineItem.Paid.Value;
			}

			if (blueCapitalCallLineItem.PaymentDate.HasValue) {
				deepBlueCCLineItem.PaidON = blueCapitalCallLineItem.PaymentDate.Value;
			}

			// WARNING: Following fields are present in DeepBlue but not in blue
			// The following fields are present in UI and should be provided
			//deepBlueCCLineItem.ManagementFeeInterest;
			//deepBlueCCLineItem.InvestedAmountInterest;

			// Capital Call Reconciliation( actually it is Capital Call Line Item reconciliation)
			// When doing the reconciliation on the capital call line item, there is a Payment Date on the UI.
			// That Payment Date is actually the CapitalCall.ReceivedDate associated with the CapitalCallLineItem
			// Moreover, that field is editable, However, when you save the reconciliation, that changes on the PaymentDate will not have any effect, which the correct behavior
			// We should make the payment date non-editable

			// The following fields are not present in Blue
			//deepBlueCCLineItem.ChequeNumber;
			//deepBlueCCLineItem.ReconciliationMethod

			// The following fields are not asked from the manual capital call.
			// Should we add these fields to the UI?
			// Probably they shud be on the deepBlueCC level
			//deepBlueCCLineItem.InvestmentAmount; // This is actually sum of New + Existing Investment Amount
			//deepBlueCCLineItem.NewInvestmentAmount;
			//deepBlueCCLineItem.ExistingInvestmentAmount;


			// The following field is not being used
			// deepBlueCCLineItem.ReceivedDate;

			//Blue: Called Total in blue is sum of the following:. We dont store it seperately.
			//blueCapitalCallLineItem.CalledTotal = blueCapitalCallLineItem.CalledCapital + blueCapitalCallLineItem.CalledExpenses + blueCapitalCallLineItem.CalledFees;
			//The following 2 fields are just duplicated on the line item in blue..skip...
			//blueCapitalCallLineItem.Commitment;
			//blueCapitalCallLineItem.Designation;
			// The due date is on the Capital Call level, not on the line item level
			//blueCapitalCallLineItem.DueDate;
			return deepBlueCCLineItem;
		}

		private static int? GetFundID(string fundNumber, BlueEntities context, CookieCollection cookies) {
			int? fundId = null;
			C6_10AmberbrookFundInfo blueFund = GetBlueFund(fundNumber, context);
			if (blueFund != null) {
				DeepBlue.Models.Entity.Fund deepBlueFund = GetDeepBlueFund(blueFund.AmberbrookFundName, cookies);
				if (deepBlueFund != null) {
					fundId = deepBlueFund.FundID;
				}
				else {
				}
			}
			else {
			}
			return fundId;
		}

		private static Hashtable BlueFunds = new Hashtable();
		private static C6_10AmberbrookFundInfo GetBlueFund(string fundNumber, BlueEntities context) {
			if (BlueFunds.ContainsKey(fundNumber)) {
				return (C6_10AmberbrookFundInfo)BlueFunds[fundNumber];
			}
			else {
				C6_10AmberbrookFundInfo fund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(fundNumber)).FirstOrDefault();
				if (fund != null) {
					BlueFunds.Add(fundNumber, fund);
				}
				return fund;
			}
		}

		private static Hashtable DeepBlueFunds = new Hashtable();
		private static DeepBlue.Models.Entity.Fund GetDeepBlueFund(string fundName, CookieCollection cookies) {
			return FundImport.GetFund(fundName, cookies);
		}

		private static Hashtable Investors = new Hashtable();
		private static int? GetInvestorID(string limitedPartner, int fundID, BlueEntities context, CookieCollection cookies) {
			if (Investors.ContainsKey(limitedPartner)) {
				return Convert.ToInt32(Investors[limitedPartner]);
			}
			int? investor = null;
			List<Investor> investors = InvestorImport.GetInvestors(cookies, fundID, limitedPartner);
			investors = investors.Where(x => x.InvestorName.Equals(limitedPartner, StringComparison.OrdinalIgnoreCase)).ToList();
			if (investors.Count == 0) {
				Util.WriteError(string.Format("No investor found with name: {0} and fund: {1}", limitedPartner, fundID));
			}
			else if (investors.Count > 1) {
				Util.WriteError(string.Format("More than one investor found with name: {0} and fund: {1}", limitedPartner, fundID));
			}
			else if (investors.Count == 1) {
				investor = investors[0].InvestorID;
				Investors.Add(limitedPartner, investor.Value);
			}
			return investor;
		}

		private static int? CreateManualCapitalCall(CookieCollection cookies, CapitalCall capitalCall, out string resp, out string formdata) {
			int? capitalCallNumber = null;
			resp = string.Empty;
			formdata = string.Empty;
			//DeepBlue.Models.CapitalCall.CreateCapitalCallModel capitalCallModel = new Models.CapitalCall.CreateCapitalCallModel();
			//capitalCallModel.CapitalAmountCalled = capitalCall.CapitalAmountCalled;
			//capitalCall.capitalcall
			NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(capitalCall, string.Empty, string.Empty, new string[] { "CapitalCallLineItems" });
			// On the server side CapitalCallModel is used which is same as CapitalCall, except for the following mismatched names
			formValues["InvestedAmount"] = capitalCall.InvestmentAmount.ToString();

			if (capitalCall.CapitalCallLineItems.Count > 0) {
				formValues.Add("InvestorCount", capitalCall.CapitalCallLineItems.Count.ToString());
				int index = 0;
				foreach (CapitalCallLineItem li in capitalCall.CapitalCallLineItems.ToList()) {
					index++;
					formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(li, index + "_", string.Empty));
				}
			}

			// Send the request 
			//string url = HttpWebRequestUtil.GetUrl("CapitalCall/CreateManualCapitalCall");
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/Create");
			formdata = HttpWebRequestUtil.ToFormValue(formValues);
			byte[] postData = System.Text.Encoding.ASCII.GetBytes(formdata);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						capitalCallNumber = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
						response.Close();
						readStream.Close();
					}
				}

			}
			return capitalCallNumber;
		}

		private static void LogErrors(List<KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

				foreach (KeyValuePair<C3_10tblCallsandFeesFromAmberbrook, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.AmberbrookFundNo + ":" + kv.Value.Message + " Inner exception:" + kv.Value.InnerException.Message + " StackTrace: " + kv.Value.StackTrace);
					}
					catch (Exception ex) {
						Util.Log("Error logging exception: " + ex.Message);
					}
				}
				tw.Flush();
				tw.Close();
			}
		}

		private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.CapitalCall, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
				foreach (KeyValuePair<DeepBlue.Models.Entity.CapitalCall, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.FundID + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
					}
					catch (Exception ex) {
						Util.Log("Error logging exception: " + ex.Message);
					}
				}
				tw.Flush();
				tw.Close();
			}
		}

		private static void LogMessages() {
			using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
				tw.WriteLine(Environment.NewLine + messageLog.ToString());
				tw.Flush();
				tw.Close();
			}
		}

		private static bool? IsCapitalCallAlreadyPresent(CapitalCall capitalCall) {
			Models.CapitalCall.CapitalCallDetail detail = null;
			try {
				detail = FindCapitalCallDetail(Globals.CookieContainer, capitalCall.FundID, capitalCall.CapitalAmountCalled, capitalCall.CapitalCallDate, capitalCall.CapitalCallDueDate);
			}
			catch (Exception ex) {
				Util.Log("IsCapitalCallAlreadyPresent:" + ex.Message);
			}
			return detail != null;
		}

		public static CapitalCallDetail FindCapitalCallDetail(CookieCollection cookies, int fundID, decimal? capitalCallAmount, DateTime? capitalCallDate, DateTime? capitalCallDueDate) {
			CapitalCallDetail detail = null;
			// Send the request 
			string query = string.Empty;
			string resp = string.Empty;
			query = "&fundId=" + fundID + "&capitalCallAmount=" + capitalCallAmount + "&capitalCallDate=" + capitalCallDate + "&capitalCallDueDate=" + capitalCallDueDate;
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/FindCapitalCallDetail?" + query);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							detail = (CapitalCallDetail)js.Deserialize(resp, typeof(CapitalCallDetail));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return detail;
		}

		public static List<AutoCompleteList> GetCapitalCalls(CookieCollection cookies, int? fundID, out string resp, string capitalCallNumber = null) {
			List<AutoCompleteList> list = new List<AutoCompleteList>();
			// Send the request 
			string query = string.Empty;
			resp = string.Empty;
			if (fundID.HasValue) {
				query = "&fundId=" + fundID.Value;
			}
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/FindCapitalCalls?term=" + (string.IsNullOrEmpty(capitalCallNumber) ? string.Empty : System.Web.HttpUtility.UrlEncode(capitalCallNumber)) + query);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							list = (List<AutoCompleteList>)js.Deserialize(resp, typeof(List<AutoCompleteList>));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return list;
		}

		public static DeepBlue.Models.CapitalCall.DetailModel GetCapitalCallsWithDetails(CookieCollection cookies, int fundID, out string resp) {
			DeepBlue.Models.CapitalCall.DetailModel model = new DeepBlue.Models.CapitalCall.DetailModel();
			// Send the request 
			string query = string.Empty;
			resp = string.Empty;

			string url = HttpWebRequestUtil.GetUrl("CapitalCall/FindDetail?fundId=" + fundID);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							model = (DeepBlue.Models.CapitalCall.DetailModel)js.Deserialize(resp, typeof(DeepBlue.Models.CapitalCall.DetailModel));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return model;
		}
	}
}