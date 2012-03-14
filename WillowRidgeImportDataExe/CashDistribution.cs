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
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.ImportData {
	// /Deal/CreateUnderlyingFundCashDistribution

	class CashDistributionImport {
		public static List<KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>> Errors = new List<KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>>();
		public static List<KeyValuePair<UnderlyingFundCashDistribution, Exception>> ImportErrors = new List<KeyValuePair<UnderlyingFundCashDistribution, Exception>>();
		private static StringBuilder messageLog = new StringBuilder();

		public static int TotalConversionRecords = 0;
		public static int RecordsConvertedSuccessfully = 0;

		public static int TotalImportRecords = 0;
		public static int RecordsImportedSuccessfully = 0;

		public static void ImportPostRecordDateCashDistribution(CookieCollection cookies) {
			List<C1_30tblPostRecordDateTransactions> postRecordDateTransactions = new List<C1_30tblPostRecordDateTransactions>();
			using (BlueEntities context = new BlueEntities()) {
				postRecordDateTransactions = context.C1_30tblPostRecordDateTransactions.Where(x => x.TransactionType.Contains("cash")).ToList();
			}

			List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> prcdErrors = new List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>>();
			if (postRecordDateTransactions.Count > 0) {
				string resp = string.Empty;
				prcdErrors = CreatePostRecordCashDistribution(cookies, postRecordDateTransactions, out resp);
			}
			LogErrors(prcdErrors);
		}

		public static void ImportCashDistribution(CookieCollection cookies) {
			ImportErrors = new List<KeyValuePair<UnderlyingFundCashDistribution, Exception>>();
			TotalImportRecords = 0;
			RecordsImportedSuccessfully = 0;

			messageLog.AppendLine("<=========================BEGIN: Converting records Blue => DeepBlue=======================>");
			List<UnderlyingFundCashDistribution> cashDistributions = ConvertFromBlueToDeepBlue(cookies);
			messageLog.AppendLine("<=========================END: Converting records Blue => DeepBlue=======================>");
			LogErrors(Errors);
			foreach (UnderlyingFundCashDistribution cashDist in cashDistributions) {
				TotalImportRecords++;
				try {
					string resp = string.Empty;
					// Important: First make sure that the deak in this the UF is making a cash distribution is closed
					foreach (CashDistribution lineItem in cashDist.CashDistributions) {
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
					bool? alreadyExists = IsManualCashDistributionAlreadyCreated(cashDist, out resp);
					if (alreadyExists.HasValue && !alreadyExists.Value) {
						if (SanityCheck(cashDist, out resp)) {
							CreateManualCashDistribution(cookies, cashDist, out resp);
							if (!string.IsNullOrEmpty(resp)) {
								ImportErrors.Add(new KeyValuePair<UnderlyingFundCashDistribution, Exception>(cashDist, new Exception("Error creating Cash Distribution. Error:" + resp)));
							}
							else {
								string msg = string.Format("Created manual cash distribution. FundID:{0}, UFID:{1}, Amount:{2}, NoticeDate: {3}", cashDist.FundID, cashDist.UnderlyingFundID, cashDist.Amount, cashDist.NoticeDate);
								messageLog.AppendLine(msg);
								Util.WriteNewEntry(msg);
								foreach (CashDistribution cd in cashDist.CashDistributions) {
									messageLog.AppendLine(string.Format("CD(line item). Amount:{0}, DealID:{1}", cd.Amount, cd.DealID));
								}
							}
						}
						else {
							Util.WriteError(resp);
							ImportErrors.Add(new KeyValuePair<UnderlyingFundCashDistribution, Exception>(cashDist, new Exception(resp)));
						}
					}
					else {
						string warning = string.Format("Cash Distribution already created for Fund: {0}, UF:{1}, Amount: {2}, NoticeDate: {3}", cashDist.FundID, cashDist.UnderlyingFundID, cashDist.Amount, cashDist.NoticeDate);
						Util.WriteWarning(warning);
						messageLog.Append(warning);
						messageLog.Append(Environment.NewLine);
					}
				}
				catch (Exception ex) {
					ImportErrors.Add(new KeyValuePair<UnderlyingFundCashDistribution, Exception>(cashDist, ex));
					Util.WriteError(ex.Message);
				}
			}
			LogErrors(ImportErrors);
			LogMessages();
		}

		/// <summary>
		/// Make sure that the Capital Distribution you are trying to make is valid
		/// </summary>
		private static bool SanityCheck(UnderlyingFundCashDistribution ufcd, out string resp) {
			StringBuilder sb = new StringBuilder();
			resp = string.Empty;
			bool success = true;
			List<UnderlyingFundCashDistributionModel> ufs = GetUnderlyingFundCashDistributionList(ufcd.UnderlyingFundID);
			if (ufs.Count > 0) {
				// For Fund, UF
				UnderlyingFundCashDistributionModel ufInFund = ufs.Where(x => x.FundId == ufcd.FundID).FirstOrDefault();
				if (ufInFund != null) {
					// loop through the deals and make sure they are there
					// A UF may be present in a Fund in many deals, so get all those
					foreach (CashDistribution cd in ufcd.CashDistributions) {
						ActivityDealModel mod = ufInFund.Deals.Where(x => x.DealId == cd.DealID).FirstOrDefault();
						if (mod == null) {
							success = false;
							sb.Append(string.Format("could not find dealID: {0}, fundId: {1}, UFID: {2}", cd.DealID, ufcd.FundID, ufcd.UnderlyingFundID));
						}
					}
				}
				else {
					success = false;
					sb.Append(string.Format("No UnderlyingFundCashDistributionModel found for FundId: {0}, UFID: {1} ", ufcd.FundID, ufcd.UnderlyingFundID));
				}
				resp = sb.ToString();
				if (!string.IsNullOrEmpty(resp)) {
					resp = "Sanity check failed for UF: " + ufcd.UnderlyingFundID + " " + resp;
				}
			}
			else {
				success = false;
				resp = "Sanity check failed for UF: " + ufcd.UnderlyingFundID + " No UnderlyingFundCashDistributionModel found";
			}
			return success;
		}

		private static Hashtable _cashDistByUF = new Hashtable();
		private static List<UnderlyingFundCashDistributionModel> GetUnderlyingFundCashDistributionList(int underlyingFundId) {
			if (!_cashDistByUF.ContainsKey(underlyingFundId)) {
				string resp = string.Empty;
				List<UnderlyingFundCashDistributionModel> ufcdList = UnderlyingFundCashDistributionList(Globals.CookieContainer, underlyingFundId, out  resp);
				_cashDistByUF.Add(underlyingFundId, ufcdList);
			}
			return (List<UnderlyingFundCashDistributionModel>)_cashDistByUF[underlyingFundId];
		}


		private static List<int> ClosedDeals = new List<int>();


		private static List<UnderlyingFundCashDistribution> ConvertFromBlueToDeepBlue(CookieCollection cookies) {
			Util.Log("Fetching Underlying Fund Cash Distribution from Blue............");
			List<UnderlyingFundCashDistribution> cashDists = new List<UnderlyingFundCashDistribution>();
			using (BlueEntities context = new BlueEntities()) {
				// C1_20tblCallsToAmberbrook has deals also in the same table. We will group by (FundNo, Fund, NoticeDate) to get unique Capital Call (which should correspond to UnderlyingFundCapitalCall).
				var ufCashDists = from blueCC in context.C1_10tblDistToAmberbrookCash
								  group blueCC by
								  new { blueCC.AmberbrookFundNo, blueCC.Fund, blueCC.NoticeDate } into g
								  select g;
				TotalImportRecords = 0;
				RecordsImportedSuccessfully = 0;
				messageLog.Append("");
				foreach (var ufCD in ufCashDists) {
					TotalImportRecords++;
					List<C1_10tblDistToAmberbrookCash> blueCashDistsByDeal = context.C1_10tblDistToAmberbrookCash.Where(x => x.NoticeDate == ufCD.Key.NoticeDate).Where(x => x.Fund == ufCD.Key.Fund).Where(x => x.AmberbrookFundNo == ufCD.Key.AmberbrookFundNo).ToList();
					string msg = string.Format("#{0} Getting cash dist .AmbFund#: {1}, Fund: {2}, NoticeDate: {3}, Total deals in this distribution:{4} ", TotalImportRecords, ufCD.Key.AmberbrookFundNo, ufCD.Key.Fund, ufCD.Key.NoticeDate, blueCashDistsByDeal.Count);
					Util.Log(msg);
					messageLog.Append(msg);
					messageLog.Append(Environment.NewLine);
					C1_10tblDistToAmberbrookCash firstBlueCashDist = blueCashDistsByDeal.First();
					string cdResp = string.Empty;
					UnderlyingFundCashDistribution cd = GetCashDistributionFromBlue(firstBlueCashDist, (decimal)blueCashDistsByDeal.Sum(x => x.Proceeds), context, cookies, out cdResp);
					bool error = false;
					if (cd != null) {
						messageLog.Append(string.Format("UnderlyingFundCashDistribution level(C1_10tblDistToAmberbrookCash => UnderlyingFundCashDistribution): BlueTransactionID:{0}, BlueAMBFund#:{1} => DeepBlueFundId:{2}, BlueUF:{3} => DeepBlueUF:{4}", firstBlueCashDist.TransactionID, firstBlueCashDist.AmberbrookFundNo, cd.FundID, firstBlueCashDist.Fund, cd.UnderlyingFundID));
						messageLog.Append(Environment.NewLine);

						// UnderlyingFundCashDistributionLineItem
						foreach (C1_10tblDistToAmberbrookCash blueCashDist in blueCashDistsByDeal) {
							try {
								string resp = string.Empty;
								CashDistribution lineItem = GetCashDistributionLineItemFromBlue(blueCashDist, cd.UnderlyingFundID, cd.FundID, cookies, out resp);
								if (lineItem != null) {
									messageLog.Append(string.Format("Line Item(C1_10tblDistToAmberbrookCash => Cash Distribution) level: BlueTransactionID:{0}, BlueDeal#:{1} => DeepBlueDealId:{2}, BlueProceeds:{3} => DeepBlueAmount:{4},  BlueReceivedDate:{5} => DeepBlueDistributionDate:{6}", blueCashDist.TransactionID, blueCashDist.AmberbrookDealNo, lineItem.DealID, blueCashDist.Proceeds, lineItem.Amount, blueCashDist.Received, lineItem.DistributionDate));
									messageLog.Append(Environment.NewLine);
									cd.CashDistributions.Add(lineItem);
								}
								else {
									error = true;
									Errors.Add(new KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>(blueCashDist, new Exception(resp)));
									Util.WriteError(resp);
								}
							}
							catch (Exception ex) {
								error = true;
								Errors.Add(new KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>(blueCashDist, ex));
								Util.WriteError("ConvertBlueToDeepBlue() " + ex);
							}
						}
					}
					else {
						error = true;
						string errMsg = string.Format("ConvertBlueToDeepBlue() Failed to convert. AmberbrookFundNo: {0}, Fund: {1}, NoticeDate: {2}, Error:{3}", ufCD.Key.AmberbrookFundNo, ufCD.Key.Fund, ufCD.Key.NoticeDate, cdResp);
						Errors.Add(new KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>(firstBlueCashDist, new Exception(errMsg)));
						Util.WriteError("ConvertBlueToDeepBlue() " + errMsg);
					}
					if (!error) {
						cashDists.Add(cd);
						RecordsImportedSuccessfully++;
					}
				}
			}
			Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalImportRecords, RecordsImportedSuccessfully));
			return cashDists;
		}



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

		private static UnderlyingFundCashDistribution GetCashDistributionFromBlue(C1_10tblDistToAmberbrookCash blueCashDist, decimal amount, BlueEntities context, CookieCollection cookies, out string resp) {
			resp = string.Empty;
			UnderlyingFundCashDistribution deepBlueCD = new UnderlyingFundCashDistribution();
			C6_10AmberbrookFundInfo ambFundInfo = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo == blueCashDist.AmberbrookFundNo).FirstOrDefault();
			if (ambFundInfo != null) {
				Fund fund = FundImport.GetFund(ambFundInfo.AmberbrookFundName, cookies);
				if (fund != null) {
					deepBlueCD.FundID = fund.FundID;
				}
				else {
					resp = "Unable to find AMB Fund: " + ambFundInfo.AmberbrookFundName;
					return null;
				}
			}
			else {
				resp = "Unable to find AMB Fund#: " + blueCashDist.AmberbrookFundNo;
				return null;
			}

			List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = GetUnderlyingFunds(cookies);
			DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName == blueCashDist.Fund).FirstOrDefault();
			if (uf != null) {
				deepBlueCD.UnderlyingFundID = uf.UnderlyingFundId;
			}
			else {
				resp = "Unable to find Underlying fund: " + blueCashDist.Fund;
				Util.Log(resp);
				return null;
			}

			deepBlueCD.Amount = amount;
			// On the UI, this field is labelled Due Date
			if (blueCashDist.NoticeDate.HasValue)
				deepBlueCD.NoticeDate = blueCashDist.NoticeDate.Value.Date;

			// PRDCD is stored in the 1-30tblpostrecorddatetransactions table(Transaction type = cash distribution). so we assuming all the calls here are non-prdcc 
			deepBlueCD.IsPostRecordDateTransaction = false;

			// We dont need to provider the value for Received date, as it is assigned on the server side (to DateTime.Now)
			// deepBlueCD.ReceivedDate = DateTime.Now;
			if (blueCashDist.ReceivedDate.HasValue) {
				deepBlueCD.ReceivedDate = blueCashDist.ReceivedDate.Value.Date;
			}
			else {
				deepBlueCD.ReceivedDate = (DateTime.Now).Date;
			}

			// This should be handled in the reconciliation
			#region reconconciliation
			if (blueCashDist.Received.HasValue) {
				deepBlueCD.IsReconciled = blueCashDist.Received.Value;
			}
			deepBlueCD.PaidON = blueCashDist.ReceivedDate;
			// Paid Date is not required
			// TODO: Find out which of Paid Date/Paid On is used for reconciliation
			// deepBlueCD.PaidDate;
			// deepBlueCD.ReconciliationMethod;
			#endregion

			// WARNING: What should the Cash distribution type should be (Cash Distribution/Deemed Distribution/Netted Distribution)
			// Cash Distribution
			deepBlueCD.CashDistributionTypeID = 1;
			return deepBlueCD;
		}

		private static CashDistribution GetCashDistributionLineItemFromBlue(C1_10tblDistToAmberbrookCash blueCashDist, int underlyingFundID, int fundID, CookieCollection cookies, out string resp) {
			CashDistribution cashDistLineItem = new CashDistribution();
			resp = string.Empty;
			List<DeepBlue.Models.Deal.DealListModel> deals = DealImport.GetDeals(cookies, true, fundID).Where(x => x.DealNumber == blueCashDist.AmberbrookDealNo).ToList();
			if (deals.Count != 1) {
				if (deals.Count == 0) {
					resp = string.Format("Cannot find deal with deal#: {0}, for fund: {1}", blueCashDist.AmberbrookDealNo, fundID);
				}
				else if (deals.Count > 1) {
					resp = string.Format("Cannot uniquely identify deal with deal#: {0}, for fund: {1}. Expected 1 deal. Deals found: {2}", blueCashDist.AmberbrookDealNo, fundID, deals.Count);
				}
				return null;
			}
			DealListModel deal = deals.FirstOrDefault();
			cashDistLineItem.DealID = deal.DealId;
			cashDistLineItem.UnderlyingFundID = underlyingFundID;
			cashDistLineItem.Amount = (decimal)blueCashDist.Proceeds;
			// WARNING: Is this mapping ok?
			// Actually the UI doesnt ask for this field, so i think we are ok here.
			if (blueCashDist.ReceivedDate.HasValue)
				cashDistLineItem.DistributionDate = blueCashDist.ReceivedDate.Value.Date;

			return cashDistLineItem;
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

		private static void CreateManualCashDistribution(CookieCollection cookies, UnderlyingFundCashDistribution cashDist, out string resp) {
			resp = string.Empty;

			UnderlyingFundCashDistributionModel model = new UnderlyingFundCashDistributionModel();
			model.FundId = cashDist.FundID;
			model.UnderlyingFundId = cashDist.UnderlyingFundID;
			model.CashDistributionTypeId = cashDist.CashDistributionTypeID;
			model.Amount = cashDist.Amount;

			if (cashDist.NoticeDate.HasValue)
				model.NoticeDate = cashDist.NoticeDate.Value.Date;

			if (cashDist.ReceivedDate.HasValue)
				model.ReceivedDate = cashDist.ReceivedDate.Value.Date;

			NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, "0_", string.Empty);
			// This should be manual cash distribution
			formValues.Add("isManualCashDistribution", "true");
			formValues.Add("TotalRows", "1");

			if (cashDist.CashDistributions.Count > 0) {
				foreach (CashDistribution cd in cashDist.CashDistributions) {
					// underlyingFundCashDistribution.FundID.ToString() + "_" + dealUnderlyingFund.DealID.ToString() + "_" + "CallAmount"
					formValues.Add(string.Format("{0}_{1}_CallAmount", model.FundId, cd.DealID), cd.Amount.ToString());
				}
			}


			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundCashDistribution");
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

		private static bool? IsManualCashDistributionAlreadyCreated(UnderlyingFundCashDistribution cashDist, out string resp) {
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
			model.FundId = cashDist.FundID;
			model.UnderlyingFundId = cashDist.UnderlyingFundID;
			model.ReconcileType = (int)DeepBlue.Models.Deal.Enums.ReconcileType.UnderlyingFundCashDistribution;

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
							ReconcileResult reconcileResultModel = (ReconcileResult)js.Deserialize(resp, typeof(ReconcileResult));
							List<ReconcileReportModel> reconciles = reconcileResultModel.Results;
							alreadyExists = reconciles.Where(x => x.Amount == cashDist.Amount).FirstOrDefault() != null;
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

		public class ReconcileResult {
			public string Error { get; set; }
			public List<ReconcileReportModel> Results { get; set; }
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

		private static List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> CreatePostRecordCashDistribution(CookieCollection cookies, List<C1_30tblPostRecordDateTransactions> postRecordDateTransactions, out string resp) {
			resp = string.Empty;
			List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> errors = new List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>>();
			//int counter = 0;
			NameValueCollection formValues = new NameValueCollection();
			foreach (C1_30tblPostRecordDateTransactions postRecordDateTransaction in postRecordDateTransactions) {
				try {
					TotalConversionRecords++;
					TotalImportRecords++;
					RecordsConvertedSuccessfully++;
					UnderlyingFundPostRecordCashDistributionModel model = new UnderlyingFundPostRecordCashDistributionModel();

					List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = GetUnderlyingFunds(cookies);
					DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName == postRecordDateTransaction.Fund).FirstOrDefault();
					if (uf != null) {
						model.UnderlyingFundId = uf.UnderlyingFundId;

						// Post record items are for deals that are open.. so we need to first get the open deals
						List<UnderlyingFundPostRecordCashDistributionModel> prdcds = GetPostRecordCashDistribution(model.UnderlyingFundId);
						if (prdcds != null && prdcds.Count > 0) {
							UnderlyingFundPostRecordCashDistributionModel prdcd = prdcds.Where(x => x.DealName.Equals(GetDealName(postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.AmberbrookFundNo))).FirstOrDefault();
							if (prdcd != null) {
								model.DealId = prdcd.DealId;
								model.Amount = Math.Abs((decimal)postRecordDateTransaction.Proceeds);
								if (postRecordDateTransaction.EffectiveDate.HasValue) {
									model.DistributionDate = postRecordDateTransaction.EffectiveDate.Value.Date;
								}

								// Check already exist
								object prcdItem = FindUnderlyingFundPostRecordCashDistribution(cookies, model.UnderlyingFundId, model.DealId, model.Amount, model.DistributionDate, out resp);

								if (prcdItem != null) {
									string err = string.Format("Post Record Cash Distribution is already exist for AMBFund#: {0}, Deal# {1}, Fund: {2}, Response: {3}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund, resp);
									Util.WriteError(err);
									continue;
								}

								formValues = HttpWebRequestUtil.SetUpForm(model, "0_", string.Empty);
							}
							else {
								continue;
							}

							formValues.Add("TotalRows", "1");

							// Send the request 

							string url = HttpWebRequestUtil.GetUrl("Deal/CreateUnderlyingFundPostRecordCashDistribution");
							string data = HttpWebRequestUtil.ToFormValue(formValues);
							byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
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
											string err = string.Format("Error creating PRDCD for AMBFund#: {0}, Deal# {1}, Fund: {2}, Response: {3}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund, resp);
											Util.WriteError(err);
											errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception("Response from server: " + err)));
										}
										response.Close();
										readStream.Close();
									}
								}
							}
						}
						else {
							string err = string.Format("No open deal found for AMBFund#: {0}, Deal# {1}, Fund: {2}", postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund);
							Util.WriteError(err);
							errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
						}
					}
					else {
						string err = string.Format("when trying to  create post record cash distribution, could not find underlying fund: {0}, AMBFund#: {1}, Deal# {2}, Fund: {3}", postRecordDateTransaction.Fund, postRecordDateTransaction.AmberbrookFundNo, postRecordDateTransaction.AmberbrookDealNo, postRecordDateTransaction.Fund);
						Util.WriteError(err);
						errors.Add(new KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>(postRecordDateTransaction, new Exception(err)));
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

		private static object FindUnderlyingFundPostRecordCashDistribution(CookieCollection cookies, int underlyingFundId, int dealId, decimal? amount, DateTime? distributionDate, out string resp) {
			resp = string.Empty;
			object ufCD = null;
			// Send the request 
			if (amount.HasValue && distributionDate.HasValue) {
				string url = HttpWebRequestUtil.GetUrl("Deal/FindUnderlyingFundPostRecordCashDistribution?underlyingFundId=" + underlyingFundId + "&dealId=" + dealId + "&amount=" + amount + "&distributionDate=" + distributionDate);
				HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
				if (response.StatusCode == System.Net.HttpStatusCode.OK) {
					using (Stream receiveStream = response.GetResponseStream()) {
						// Pipes the stream to a higher level stream reader with the required encoding format. 
						using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
							resp = readStream.ReadToEnd();
							if (!string.IsNullOrEmpty(resp)) {
								JavaScriptSerializer js = new JavaScriptSerializer();
								ufCD = (object)js.Deserialize(resp, typeof(List<UnderlyingFundCapitalCallModel>));
							}
							else {
							}
							response.Close();
							readStream.Close();
						}
					}
				}
			}
			return ufCD;
		}

		private static Hashtable _ufIdToDeal = new Hashtable();
		private static List<UnderlyingFundPostRecordCashDistributionModel> GetPostRecordCashDistribution(int underlyingFundId) {
			if (!_ufIdToDeal.ContainsKey(underlyingFundId)) {
				string resp = string.Empty;
				List<UnderlyingFundPostRecordCashDistributionModel> openDeals = UnderlyingFundPostRecordCashDistributionList(Globals.CookieContainer, underlyingFundId, out  resp);
				if (openDeals != null && openDeals.Count > 0) {
					_ufIdToDeal.Add(underlyingFundId, openDeals);
				}
				else {
					return null;
				}
			}
			return (List<UnderlyingFundPostRecordCashDistributionModel>)_ufIdToDeal[underlyingFundId];
		}

		private static List<UnderlyingFundPostRecordCashDistributionModel> UnderlyingFundPostRecordCashDistributionList(CookieCollection cookies, int underlyingFundId, out string resp) {
			resp = string.Empty;
			List<UnderlyingFundPostRecordCashDistributionModel> ufPRCC = new List<UnderlyingFundPostRecordCashDistributionModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingFundPostRecordCashDistributionList?underlyingFundId=" + underlyingFundId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							ufPRCC = (List<UnderlyingFundPostRecordCashDistributionModel>)js.Deserialize(resp, typeof(List<UnderlyingFundPostRecordCashDistributionModel>));
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

		private static List<UnderlyingFundCashDistributionModel> UnderlyingFundCashDistributionList(CookieCollection cookies, int underlyingFundId, out string resp) {
			resp = string.Empty;
			List<UnderlyingFundCashDistributionModel> ufCD = new List<UnderlyingFundCashDistributionModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/UnderlyingFundCashDistributionList?underlyingFundId=" + underlyingFundId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							ufCD = (List<UnderlyingFundCashDistributionModel>)js.Deserialize(resp, typeof(List<UnderlyingFundCashDistributionModel>));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return ufCD;
		}

		private static void LogErrors(List<KeyValuePair<C1_30tblPostRecordDateTransactions, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalImportRecords, RecordsImportedSuccessfully, Errors.Count));

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

		private static void LogErrors(List<KeyValuePair<C1_10tblDistToAmberbrookCash, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

				foreach (KeyValuePair<C1_10tblDistToAmberbrookCash, Exception> kv in errors) {
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

		private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.UnderlyingFundCashDistribution, Exception>> errors) {
			using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
				tw.WriteLine(Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
				foreach (KeyValuePair<DeepBlue.Models.Entity.UnderlyingFundCashDistribution, Exception> kv in errors) {
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
	}
}


public class AmberBrookFundCollection {

	public string AmberBrookFundNo { get; set; }
}