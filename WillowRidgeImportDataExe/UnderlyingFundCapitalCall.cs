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
	class UnderlyingFundCapitalCallImport {
		public static List<KeyValuePair<C1_20tblCallsToAmberbrook, Exception>> Errors = new List<KeyValuePair<C1_20tblCallsToAmberbrook, Exception>>();
		public static List<KeyValuePair<UnderlyingFundCapitalCall, Exception>> ImportErrors = new List<KeyValuePair<UnderlyingFundCapitalCall, Exception>>();
		private static StringBuilder messageLog = new StringBuilder();

		public static int TotalConversionRecords = 0;
		public static int RecordsConvertedSuccessfully = 0;

		public static int TotalImportRecords = 0;
		public static int RecordsImportedSuccessfully = 0;

		public static void ImportPostRecordCapitalCall(CookieCollection cookies) {
			ImportErrors = new List<KeyValuePair<UnderlyingFundCapitalCall, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;
			// Important: We assume that if we are importing post record transactions, then 
			// the deals are open, meaning the UF and Directs have not closed in the deals

			// Import the post record date capital calls
			List<C1_30tblPostRecordDateTransactions> postRecordDateTransactions = new List<C1_30tblPostRecordDateTransactions>();

			using (BlueEntities context = new BlueEntities()) {
				postRecordDateTransactions = context.C1_30tblPostRecordDateTransactions.Where(x => x.TransactionType.Contains("call")).ToList();
			}
			List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> prccErrors = new List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>>();
			if (postRecordDateTransactions.Count > 0) {
				string resp = string.Empty;
				prccErrors = CreatePostRecordCapitalCall(cookies, postRecordDateTransactions, out resp);
			}
			LogErrors(prccErrors);
		}

		public static void ImportCapitalCall(CookieCollection cookies) {
			ImportErrors = new List<KeyValuePair<UnderlyingFundCapitalCall, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;

			messageLog.AppendLine("<=========================BEGIN: Converting records Blue => DeepBlue=======================>");
			List<UnderlyingFundCapitalCall> capitalCalls = ConvertFromBlueToDeepBlue(cookies);
			messageLog.AppendLine("<=========================END: Converting records Blue => DeepBlue=======================>");
			LogErrors(Errors);
			foreach (UnderlyingFundCapitalCall capitalCall in capitalCalls) {
				TotalImportRecords++;
				try {
					string resp = string.Empty;
					// Important: First make sure that the deak in this the UF is making a cash distribution is closed
					foreach (UnderlyingFundCapitalCallLineItem lineItem in capitalCall.UnderlyingFundCapitalCallLineItems) {
						if (!ClosedDeals.Contains(lineItem.DealID)) {
							// Close the deal
							string res = string.Empty;
							int? dealClosingId = DealClosing.CreateDealClosing(lineItem.DealID, out res);
							messageLog.Append("trying to create/update deal closing for deal:").Append(lineItem.DealID.ToString()).Append("Result:").Append(res);
							if (dealClosingId.HasValue) {
								messageLog.Append("DealClosingID:").Append(dealClosingId.Value.ToString());
							}
							messageLog.Append(Environment.NewLine);
							ClosedDeals.Add(lineItem.DealID);
						}
					}

					// Do a Sanity Check
					bool? alreadyExists = IsManualCapitalCallAlreadyCreated(capitalCall, out resp);
					if (alreadyExists.HasValue && !alreadyExists.Value) {
						if (SanityCheck(capitalCall, out resp)) {
							CreateManualCapitalCall(cookies, capitalCall, out resp);
							if (!string.IsNullOrEmpty(resp)) {
								ImportErrors.Add(new KeyValuePair<UnderlyingFundCapitalCall, Exception>(capitalCall, new Exception("Error creating Capital Call. Error:" + resp)));
							}
							else {
								string msg = string.Format("Created manual cash distribution. FundID:{0}, UFID:{1}, Amount:{2}, NoticeDate: {3}", capitalCall.FundID, capitalCall.UnderlyingFundID, capitalCall.Amount, capitalCall.NoticeDate);
								messageLog.AppendLine(msg);
								Util.WriteNewEntry(msg);
								foreach (UnderlyingFundCapitalCallLineItem cc in capitalCall.UnderlyingFundCapitalCallLineItems) {
									messageLog.AppendLine(string.Format("CD(line item). Amount:{0}, DealID:{1}", cc.Amount, cc.DealID));
								}
							}
						}
						else {
							Util.WriteError(resp);
							ImportErrors.Add(new KeyValuePair<UnderlyingFundCapitalCall, Exception>(capitalCall, new Exception(resp)));
						}
					}
					else {
						string warning = string.Format("Capital call already created for Fund: {0}, UF:{1}, Amount: {2}, NoticeDate: {3}", capitalCall.FundID, capitalCall.UnderlyingFundID, capitalCall.Amount, capitalCall.NoticeDate);
						Util.WriteWarning(warning);
						messageLog.Append(warning);
						messageLog.Append(Environment.NewLine);
					}
				}
				catch (Exception ex) {
					ImportErrors.Add(new KeyValuePair<UnderlyingFundCapitalCall, Exception>(capitalCall, ex));
					Util.WriteError(ex.Message);
				}
			}
			LogErrors(ImportErrors);
			LogMessages();
		}

		/// <summary>
		/// Make sure that the Capital Distribution you are trying to make is valid
		/// </summary>
		private static bool SanityCheck(UnderlyingFundCapitalCall ufcc, out string resp) {
			StringBuilder sb = new StringBuilder();
			resp = string.Empty;
			bool success = true;
			List<UnderlyingFundCapitalCallModel> ufs = GetUnderlyingFundCapitalCallList(ufcc.UnderlyingFundID);
			if (ufs.Count > 0) {
				// For Fund, UF
				UnderlyingFundCapitalCallModel ufInFund = ufs.Where(x => x.FundId == ufcc.FundID).FirstOrDefault();
				if (ufInFund != null) {
					// loop through the deals and make sure they are there
					// A UF may be present in a Fund in many deals, so get all those
					foreach (UnderlyingFundCapitalCallLineItem cd in ufcc.UnderlyingFundCapitalCallLineItems) {
						ActivityDealModel mod = ufInFund.Deals.Where(x => x.DealId == cd.DealID).FirstOrDefault();
						if (mod == null) {
							success = false;
							sb.Append(string.Format("could not find dealID: {0}, fundId: {1}, UFID: {2}", cd.DealID, ufcc.FundID, ufcc.UnderlyingFundID));
						}
					}
				}
				else {
					success = false;
					sb.Append(string.Format("No UnderlyingFundCashDistributionModel found for FundId: {0}, UFID: {1} ", ufcc.FundID, ufcc.UnderlyingFundID));
				}
				resp = sb.ToString();
				if (!string.IsNullOrEmpty(resp)) {
					resp = "Sanity check failed for UF: " + ufcc.UnderlyingFundID + " " + resp;
				}
			}
			else {
				success = false;
				resp = "Sanity check failed for UF: " + ufcc.UnderlyingFundID + " No UnderlyingFundCashDistributionModel found";
			}
			return success;
		}

		private static Hashtable _capitalCallsByUF = new Hashtable();
		private static List<UnderlyingFundCapitalCallModel> GetUnderlyingFundCapitalCallList(int underlyingFundId) {
			if (!_capitalCallsByUF.ContainsKey(underlyingFundId)) {
				string resp = string.Empty;
				List<UnderlyingFundCapitalCallModel> ufcdList = UnderlyingFundCapitalCallList(Globals.CookieContainer, underlyingFundId, out  resp);
				_capitalCallsByUF.Add(underlyingFundId, ufcdList);
			}
			return (List<UnderlyingFundCapitalCallModel>)_capitalCallsByUF[underlyingFundId];
		}

		private static List<int> ClosedDeals = new List<int>();

		private static List<UnderlyingFundCapitalCall> ConvertFromBlueToDeepBlue(CookieCollection cookies) {
			Util.Log("Fetching Underlying Fund Capital Calls from Blue............");
			List<UnderlyingFundCapitalCall> capitalCalls = new List<UnderlyingFundCapitalCall>();
			using (BlueEntities context = new BlueEntities()) {
				// C1_20tblCallsToAmberbrook has deals also in the same table. We will group by (FundNo, Fund, NoticeDate) to get unique Capital Call (which should correspond to UnderlyingFundCapitalCall).
				var ufCapitalCalls = from blueCC in context.C1_20tblCallsToAmberbrook
									 group blueCC by
									 new { blueCC.AmberbrookFundNo, blueCC.Fund, blueCC.NoticeDate } into g
									 select g;

				TotalImportRecords = 0;
				RecordsImportedSuccessfully = 0;
				foreach (var ufCC in ufCapitalCalls) {
					TotalImportRecords++;
					List<C1_20tblCallsToAmberbrook> blueCapitalCallsByDeal = context.C1_20tblCallsToAmberbrook.Where(x => x.NoticeDate == ufCC.Key.NoticeDate).Where(x => x.Fund == ufCC.Key.Fund).Where(x => x.AmberbrookFundNo == ufCC.Key.AmberbrookFundNo).ToList();
					string msg = string.Format("#{0} Getting cash dist .AmbFund#: {1}, Fund: {2}, NoticeDate: {3}, Total deals in this distribution:{4} ", TotalImportRecords, ufCC.Key.AmberbrookFundNo, ufCC.Key.Fund, ufCC.Key.NoticeDate, blueCapitalCallsByDeal.Count);
					Util.Log(msg);
					messageLog.Append(msg);
					messageLog.Append(Environment.NewLine);
					C1_20tblCallsToAmberbrook firstBlueCapitalCall = blueCapitalCallsByDeal.First();
					string ccResp = string.Empty;
					UnderlyingFundCapitalCall cc = GetCapitalCallFromBlue(firstBlueCapitalCall, (decimal)blueCapitalCallsByDeal.Sum(x => x.Amount), context, cookies, out ccResp);
					bool error = false;
					if (cc != null) {
						messageLog.Append(string.Format("UnderlyingFundCashDistribution level(C1_20tblCallsToAmberbrook => UnderlyingFundCapitalCall): BlueTransactionID:{0}, BlueAMBFund#:{1} => DeepBlueFundId:{2}, BlueUF:{3} => DeepBlueUF:{4}", firstBlueCapitalCall.TransactionID, firstBlueCapitalCall.AmberbrookFundNo, cc.FundID, firstBlueCapitalCall.Fund, cc.UnderlyingFundID));
						messageLog.Append(Environment.NewLine);

						// UnderlyingFundCapitalCallLineItem
						foreach (C1_20tblCallsToAmberbrook blueCapitalCall in blueCapitalCallsByDeal) {
							try {
								string resp = string.Empty;
								UnderlyingFundCapitalCallLineItem lineItem = GetUnderlyingFundCapitalCallLineItemFromBlue(blueCapitalCall, cc.UnderlyingFundID, cc.FundID, cookies, out resp);
								if (lineItem != null) {
									messageLog.Append(string.Format("Line Item(C1_10tblDistToAmberbrookCash => Cash Distribution) level: BlueTransactionID:{0}, BlueDeal#:{1} => DeepBlueDealId:{2}, BlueProceeds:{3} => DeepBlueAmount:{4},  BluePaymentDate:{5} => DeepBlueCapitalCallDate:{6}", blueCapitalCall.TransactionID, blueCapitalCall.AmberbrookDealNo, lineItem.DealID, blueCapitalCall.Amount, lineItem.Amount, blueCapitalCall.PaymentDate, lineItem.CapitalCallDate));
									messageLog.Append(Environment.NewLine);
									cc.UnderlyingFundCapitalCallLineItems.Add(lineItem);
								}
								else {
									error = true;
									Errors.Add(new KeyValuePair<C1_20tblCallsToAmberbrook, Exception>(blueCapitalCall, new Exception(resp)));
									Util.WriteError(resp);
								}
													}
							catch (Exception ex) {
								error = true;
								Errors.Add(new KeyValuePair<C1_20tblCallsToAmberbrook, Exception>(blueCapitalCall, ex));
								Util.WriteError("ConvertBlueToDeepBlue() " + ex);
							}
						}
					}
					else {
						error = true;
						string errMsg = string.Format("ConvertBlueToDeepBlue() Failed to convert. AmberbrookFundNo: {0}, Fund: {1}, NoticeDate: {2}, Error:{3}", ufCC.Key.AmberbrookFundNo, ufCC.Key.Fund, ufCC.Key.NoticeDate, ccResp);
						Errors.Add(new KeyValuePair<C1_20tblCallsToAmberbrook, Exception>(firstBlueCapitalCall, new Exception(errMsg)));
						Util.WriteError("ConvertBlueToDeepBlue() " + errMsg);
					}
					if (!error) {
						capitalCalls.Add(cc);
						RecordsImportedSuccessfully++;
					}
				}
			}
			Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalImportRecords, RecordsImportedSuccessfully));
			LogErrors(Errors);
			return capitalCalls;
		}

		//private static List<DeepBlue.Models.Deal.DirectListModel> _underlyingFunds = null;
		//private static List<DeepBlue.Models.Deal.DirectListModel> GetUnderlyingFunds(CookieCollection cookies) {
		//    if (_underlyingFunds == null) {
		//        _underlyingFunds = IssuerImport.GetIssuersFromDeepBlue(cookies, true);
		//    }

		//    return _underlyingFunds;
		//}

		private static List<DeepBlue.Models.Deal.UnderlyingFundListModel> _underlyingFunds = null;
		private static List<DeepBlue.Models.Deal.UnderlyingFundListModel> GetUnderlyingFunds(CookieCollection cookies) {
			if (_underlyingFunds == null) {
				_underlyingFunds = UnderlyingFundImport.GetUnderlyingFunds(cookies);
			}

			return _underlyingFunds;
		}

		private static Hashtable _DealsByFund = new Hashtable();
		private static List<DeepBlue.Models.Deal.DealListModel> GetDeals(int fundID, CookieCollection cookies) {
			if (!_DealsByFund.ContainsKey(fundID)) {
				List<DeepBlue.Models.Deal.DealListModel> deals = DealImport.GetDeals(cookies, true);
				_DealsByFund.Add(fundID, deals);
			}

			return (List<DeepBlue.Models.Deal.DealListModel>)_DealsByFund[fundID];
		}

		private static UnderlyingFundCapitalCall GetCapitalCallFromBlue(C1_20tblCallsToAmberbrook blueCapitalCall, decimal amount, BlueEntities context, CookieCollection cookies, out string resp) {
			resp = string.Empty;
			UnderlyingFundCapitalCall deepBlueCC = new UnderlyingFundCapitalCall();
			C6_10AmberbrookFundInfo ambFundInfo = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo == blueCapitalCall.AmberbrookFundNo).FirstOrDefault();
			//if (ambFundInfo != null) {
			//    Fund fund = FundImport.GetFund(ambFundInfo.AmberbrookFundName, cookies);
			//    deepBlueCC.FundID = fund.FundID;
			//}
			if (ambFundInfo != null) {
				Fund fund = FundImport.GetFund(ambFundInfo.AmberbrookFundName, cookies);
				if (fund != null) {
					deepBlueCC.FundID = fund.FundID;
				}
				else {
					resp = "Unable to find AMB Fund: " + ambFundInfo.AmberbrookFundName;
					return null;
				}
			}
			else {
				resp = "Unable to find AMB Fund#: " + blueCapitalCall.AmberbrookFundNo;
				return null;
			}

			//List<DeepBlue.Models.Deal.DirectListModel> underlyingFunds = GetUnderlyingFunds(cookies);
			//DirectListModel uf = underlyingFunds.Where(x => x.DirectName == blueCapitalCall.Fund).FirstOrDefault();
			List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = GetUnderlyingFunds(cookies);
			DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName == blueCapitalCall.Fund).FirstOrDefault();
			if (uf != null) {
				deepBlueCC.UnderlyingFundID = uf.UnderlyingFundId;
			}
			else {
				resp = "Unable to find Underlying fund: " + blueCapitalCall.Fund;
				Util.Log(resp);
				return null;
			}
			deepBlueCC.Amount = amount;
			// On the UI, this field is labelled Due Date
			deepBlueCC.NoticeDate = blueCapitalCall.NoticeDate;


			if (blueCapitalCall.Paid.HasValue) {
				deepBlueCC.IsReconciled = blueCapitalCall.Paid.Value;
			}
			deepBlueCC.PaidON = blueCapitalCall.PaymentDate;
			// PRDCC is stored in the 1-30tblpostrecorddatetransactions table. so we assuming all the calls here are non-prdcc
			deepBlueCC.IsPostRecordDateTransaction = false;

		 

			// We dont use this anywhere currently
			//deepBlueCC.ReceivedDate;

			//WARNING: The following values are present on DeepBlue but are not present in Blue
			//deepBlueCC.IsDeemedCapitalCall 
			//deepBlueCC.ReconciliationMethod;

			//WARNING: The following fields are present in Blue but are not present in DeepBlue
			// There is actually a "Due Date" on the UI, but it is actually saved in Notice Date
			//blueCapitalCall.DueDate;
			//blueCapitalCall.Fees;
			//blueCapitalCall.Called;
			//blueCapitalCall.DateCalled;

			return deepBlueCC;
		}

		private static UnderlyingFundCapitalCallLineItem GetUnderlyingFundCapitalCallLineItemFromBlue(C1_20tblCallsToAmberbrook blueCapitalCall, int underlyingFundID, int fundID, CookieCollection cookies, out string resp) {
			UnderlyingFundCapitalCallLineItem deepBlueCCLineItem = new UnderlyingFundCapitalCallLineItem();
			resp = string.Empty;
			List<DeepBlue.Models.Deal.DealListModel> deals = DealImport.GetDeals(cookies, true, fundID).Where(x => x.DealNumber == blueCapitalCall.AmberbrookDealNo).ToList();
			if (deals.Count != 1) {
				if (deals.Count == 0) {
					resp = string.Format("Cannot find deal with deal#: {0}, for fund: {1}", blueCapitalCall.AmberbrookDealNo, fundID);
				}
				else if (deals.Count > 1) {
					resp = string.Format("Cannot uniquely identify deal with deal#: {0}, for fund: {1}. Expected 1 deal. Deals found: {2}", blueCapitalCall.AmberbrookDealNo, fundID, deals.Count);
				}
				return null;
			}
			DealListModel deal = deals.FirstOrDefault();
			deepBlueCCLineItem.DealID = deal.DealId;
			deepBlueCCLineItem.UnderlyingFundID = underlyingFundID;
			deepBlueCCLineItem.Amount = (decimal)blueCapitalCall.Amount;

			// WARNING: what are these fields used for?
			//deepBlueCCLineItem.CapitalCallDate;
			//deepBlueCCLineItem.ReceivedDate;
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

		private static void CreateManualCapitalCall(CookieCollection cookies, UnderlyingFundCapitalCall capitalCall, out string resp) {
			resp = string.Empty;

			UnderlyingFundCapitalCallModel model = new UnderlyingFundCapitalCallModel();
			model.FundId = capitalCall.FundID;
			model.Amount = capitalCall.Amount;
			model.NoticeDate = capitalCall.NoticeDate;
			model.UnderlyingFundId = capitalCall.UnderlyingFundID;

			if (model.Amount <= 0) {
				return;
			}

			// Call Amount
			// fundid_dealid_callamount

			NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, "0_", string.Empty);

			if (capitalCall.UnderlyingFundCapitalCallLineItems.Count > 0) {
				foreach (UnderlyingFundCapitalCallLineItem cc in capitalCall.UnderlyingFundCapitalCallLineItems) {
					formValues.Add(string.Format("{0}_{1}_CallAmount", model.FundId, cc.DealID), cc.Amount.ToString());
				}
			}

			formValues.Add("TotalRows", "1");
			formValues.Add("IsManualCapitalCall", "true");

			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundCapitalCall");
			byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
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
		}

		private static bool? IsManualCapitalCallAlreadyCreated(UnderlyingFundCapitalCall capitalCall, out string resp) {
			bool? alreadyExists = null;
			resp = string.Empty;
			ReconcileSearchModel model = new ReconcileSearchModel();
			// make the search criteria between start date and end date
			// currently we dont export the Received date, cos the server side code assigns the received date 
			// when we create the cash distribution. so the following code will not work
			//if(cashDist.ReceivedDate.HasValue){
			//    model.StartDate = cashDist.ReceivedDate.Value.AddDays(-1);
			//    model.EndDate = cashDist.ReceivedDate.Value.AddDays(1);
			//}
			model.FundId = capitalCall.FundID;
			model.UnderlyingFundId = capitalCall.UnderlyingFundID;
			model.ReconcileType = (int)DeepBlue.Models.Deal.Enums.ReconcileType.UnderlyingFundCapitalCall;

			NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
			// Send the request 

			string url = HttpWebRequestUtil.GetUrl("Deal/ReconcileList");

			byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, Globals.CookieContainer);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							//List<ReconcileReportModel> reconciles = (List<ReconcileReportModel>)js.Deserialize(resp, typeof(List<ReconcileReportModel>));
							//alreadyExists = reconciles.Where(x => x.Amount == cashDist.Amount).FirstOrDefault() != null;
							DeepBlue.ImportData.CashDistributionImport.ReconcileResult reconcileResultModel = (DeepBlue.ImportData.CashDistributionImport.ReconcileResult)js.Deserialize(resp, typeof(DeepBlue.ImportData.CashDistributionImport.ReconcileResult));
							List<ReconcileReportModel> reconciles = reconcileResultModel.Results;
							alreadyExists = reconciles.Where(x => x.Amount == capitalCall.Amount).FirstOrDefault() != null;
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return alreadyExists;
		}
		private static Hashtable _fundNoToFundName = new Hashtable();
		private static string GetFundName(string fundNo) {
			if (!_fundNoToFundName.ContainsKey(fundNo)) {
				using (BlueEntities context = new BlueEntities()) {
					C6_10AmberbrookFundInfo fund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(fundNo)).FirstOrDefault();
					if (fund != null) {
						_fundNoToFundName.Add(fundNo, fund.AmberbrookFundName);
					}
					else {
						return null;
					}
				}
			}
			return (string)_fundNoToFundName[fundNo];
		}

		private static Hashtable _dealNoToDealName = new Hashtable();
		private static string GetDealName(int dealNo, string fundNo) {
			string ident = string.Format("{0}:{1}", dealNo, fundNo);
			if (!_dealNoToDealName.ContainsKey(ident)) {
				using (BlueEntities context = new BlueEntities()) {
					C6_15tblAmberbrookDealInfo deal = context.C6_15tblAmberbrookDealInfo.Where(x => x.AmberbrookFundNo.Equals(fundNo)).Where(x => x.DealNo.Equals(dealNo)).FirstOrDefault();
					if (deal != null) {
						_dealNoToDealName.Add(ident, deal.DealName);
					}
					else {
						return null;
					}
				}
			}
			return (string)_dealNoToDealName[ident];
		}

		private static List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> CreatePostRecordCapitalCall(CookieCollection cookies, List<C1_30tblPostRecordDateTransactions> postRecordDateTransactions, out string resp) {
			resp = string.Empty;
			List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> errors = new List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>>();
			NameValueCollection formValues = new NameValueCollection();
			foreach (C1_30tblPostRecordDateTransactions postRecordDateTransaction in postRecordDateTransactions) {
				try {
					TotalConversionRecords++;
					TotalImportRecords++;
					RecordsConvertedSuccessfully++;
					UnderlyingFundPostRecordCapitalCallModel model = new UnderlyingFundPostRecordCapitalCallModel();

					//if (postRecordDateTransaction != null) {
					//    Fund fund = FundImport.GetFund(GetFundName(postRecordDateTransaction.AmberbrookFundNo), cookies);
					//    if (fund != null) {
					//        model.FundId = fund.FundID;
					//    } else {
					//        string err = string.Format("when trying to  create post record capital call, could not find AMBFund#: {0}", postRecordDateTransaction.AmberbrookFundNo);
					//        Util.WriteError(err);
					//        errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
					//        continue;
					//    }
					//}

					//List<DeepBlue.Models.Deal.DirectListModel> underlyingFunds = GetUnderlyingFunds(cookies);
					//DirectListModel uf = underlyingFunds.Where(x => x.DirectName == postRecordDateTransaction.Fund).FirstOrDefault();
					List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = GetUnderlyingFunds(cookies);
					DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName == postRecordDateTransaction.Fund).FirstOrDefault();
					if (uf != null) {
						model.UnderlyingFundId = uf.UnderlyingFundId;
					}
					else {
						string err = string.Format("when trying to create post record capital call, could not find AMBFund#: {0}", postRecordDateTransaction.AmberbrookFundNo);
						Util.WriteError(err);
						errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
						continue;
					}

					// Post record items are for deals that are open.. so we need to first get the open deals
					List<UnderlyingFundPostRecordCapitalCallModel> prdccs = GetPostRecordCapitalCall(model.UnderlyingFundId);
					if (prdccs != null && prdccs.Count > 0) {
						UnderlyingFundPostRecordCapitalCallModel prdcc = prdccs.Where(x => x.DealName.Equals(GetDealName(postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.AmberbrookFundNo))).FirstOrDefault();
						if (prdcc != null) {
							model.DealId = prdcc.DealId;
							model.Amount = Math.Abs((decimal)postRecordDateTransaction.Proceeds);
							if (postRecordDateTransaction.EffectiveDate.HasValue) {
								model.CapitalCallDate = postRecordDateTransaction.EffectiveDate;
							}
							model.FundId = prdcc.FundId;
							formValues = HttpWebRequestUtil.SetUpForm(model, "0_", string.Empty);
						}
						else {
							string err = string.Format("when trying to create post record capital call, could not find prdcc corresponding to AmberbrookDealNo: {0}, AmberbrookFundNo: {1}", postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.AmberbrookFundNo);
							Util.WriteError(err);
							errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
							continue;
						}
					}
					else {
						string err = string.Format("No open deal found for AMBFund#: {0}, Deal# {1}, Fund: {2}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund);
						Util.WriteError(err);
						errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
						continue;
					}
					formValues.Add("TotalRows", "1");

					// Check already exist
					object prccItem = FindUnderlyingFundCapitalCallLineItem(cookies, model.UnderlyingFundId, model.DealId, model.Amount, model.CapitalCallDate, out resp);

					if (prccItem != null) {
						string err = string.Format("Post Record Capital Call is already exist for AMBFund#: {0}, Deal# {1}, Fund: {2}, Response: {3}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund, resp);
						Util.WriteError(err);
						continue;
					}

					// Send the request 
					string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundPostRecordCapitalCall");
					byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
					HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
					if (response.StatusCode == System.Net.HttpStatusCode.OK) {
						using (Stream receiveStream = response.GetResponseStream()) {
							// Pipes the stream to a higher level stream reader with the required encoding format. 
							using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
								resp = readStream.ReadToEnd();
								if (string.IsNullOrEmpty(resp)) {
									RecordsImportedSuccessfully++;
								}
								else {
									string err = string.Format("Error creating PRDCC for AMBFund#: {0}, Deal# {1}, Fund: {2}, Response: {3}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund, resp);
									Util.WriteError(err);
									errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception("Response from server: " + err)));
								}
								response.Close();
								readStream.Close();
							}
						}
					}
				}
				catch (Exception ex) {
					string err = string.Format("Error creating post record cash distribution for AMBFund#: {0}, Deal# {1}, Fund: {2}, Error: {3}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund, ex.Message);
					Util.WriteError(err);
					errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err, ex)));
				}
			}
			return errors;
		}

		private static Hashtable _ufIdToDeal = new Hashtable();
		private static List<UnderlyingFundPostRecordCapitalCallModel> GetPostRecordCapitalCall(int underlyingFundId) {
			if (!_ufIdToDeal.ContainsKey(underlyingFundId)) {
				string resp = string.Empty;
				List<UnderlyingFundPostRecordCapitalCallModel> openDeals = GetUnderlyingFundPostRecordCapitalCallList(Globals.CookieContainer, underlyingFundId, out  resp);
				if (openDeals != null && openDeals.Count > 0) {
					_ufIdToDeal.Add(underlyingFundId, openDeals);
				}
				else {
					return null;
				}
			}
			return (List<UnderlyingFundPostRecordCapitalCallModel>)_ufIdToDeal[underlyingFundId];
		}

		private static List<UnderlyingFundPostRecordCapitalCallModel> GetUnderlyingFundPostRecordCapitalCallList(CookieCollection cookies, int underlyingFundId, out string resp) {
			resp = string.Empty;
			List<UnderlyingFundPostRecordCapitalCallModel> ufPRCC = new List<UnderlyingFundPostRecordCapitalCallModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingFundPostRecordCapitalCallList?underlyingFundId=" + underlyingFundId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							ufPRCC = (List<UnderlyingFundPostRecordCapitalCallModel>)js.Deserialize(resp, typeof(List<UnderlyingFundPostRecordCapitalCallModel>));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return ufPRCC;
		}

		private static object FindUnderlyingFundCapitalCallLineItem(CookieCollection cookies, int underlyingFundId, int dealId, decimal? amount, DateTime? capitalCallDate, out string resp) {
			resp = string.Empty;
			object ufCC = null;
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/FindUnderlyingFundPostRecordCapitalCall?underlyingFundId=" + underlyingFundId + "&dealId=" + dealId + "&amount=" + amount + "&capitalCallDate=" + capitalCallDate);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							ufCC = (object)js.Deserialize(resp, typeof(List<UnderlyingFundCapitalCallModel>));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return ufCC;
		}

		private static List<UnderlyingFundCapitalCallModel> UnderlyingFundCapitalCallList(CookieCollection cookies, int underlyingFundId, out string resp) {
			resp = string.Empty;
			List<UnderlyingFundCapitalCallModel> ufCC = new List<UnderlyingFundCapitalCallModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingFundCapitalCallList?underlyingFundId=" + underlyingFundId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							ufCC = (List<UnderlyingFundCapitalCallModel>)js.Deserialize(resp, typeof(List<UnderlyingFundCapitalCallModel>));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return ufCC;
		}


		private static void LogErrors(List<KeyValuePair<C1_20tblCallsToAmberbrook, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

				foreach (KeyValuePair<C1_20tblCallsToAmberbrook, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.AmberbrookFundNo + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + kv.Value.StackTrace);
					}
					catch (Exception ex) {
						Util.Log("Error logging exception: " + ex.Message);
					}
				}
				tw.Flush();
				tw.Close();
			}
		}

		private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFundCapitalCall, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
				foreach (KeyValuePair<DeepBlue.Models.Entity.UnderlyingFundCapitalCall, Exception> kv in errors) {
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

		private static void LogErrors(List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

				foreach (KeyValuePair<C1_30tblPostRecordDateTransactions, Exception> kv in errors) {
					try {
						tw.WriteLine(Environment.NewLine + kv.Key.AmberbrookFundNo + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + kv.Value.StackTrace);
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
	}
}
 