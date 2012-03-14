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
	public class DealClosing {
		public static int TotalConversionRecords = 0;
		public static int RecordsConvertedSuccessfully = 0;

		public static int TotalImportRecords = 0;
		public static int RecordsImportedSuccessfully = 0;



		public static int? CreateDealClosing(int dealId, out string resp) {
			// Get the first deal closing for the deal, or get template for a new deal close
			// the returned model will contain all the open funds/directs, so we can just add them to the closing
			resp = string.Empty;
			DeepBlue.Models.Deal.DealCloseListModel dealClosing = GetDealClosings(Globals.CookieContainer, dealId, out resp).FirstOrDefault();
			CreateDealCloseModel dealClosingDetail = null;
			if (dealClosing == null) {
				dealClosingDetail = GetDealClose(Globals.CookieContainer, dealId, null, out resp);
				resp = "attempting to create new deal close";
			}
			else {
				dealClosingDetail = GetDealClose(Globals.CookieContainer, dealId, dealClosing.DealClosingId, out resp);
				resp = "deal close already exists:" + dealClosing.DealClosingId;
			}

			if (dealClosingDetail != null) {
				// Add all the underlying funds and directs to the deal closing
				string str = string.Empty;
				int? dealClosingId = AddFundsAndDirectsToDealClose(Globals.CookieContainer, dealClosingDetail, out str);
				resp += str;
				return dealClosingId;
			}
			return null;
		}

		private static List<DeepBlue.Models.Deal.DealCloseListModel> GetDealClosings(CookieCollection cookies, int dealId, out string resp) {
			resp = string.Empty;
			List<DeepBlue.Models.Deal.DealCloseListModel> dealClosings = new List<DealCloseListModel>();
			List<UnderlyingFundPostRecordCashDistributionModel> ufPRCC = new List<UnderlyingFundPostRecordCashDistributionModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/DealClosingList?pageIndex=1&pageSize=5000&sortName=DealName&sortOrder=asc&dealId=" + dealId);
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
								DeepBlue.Models.Deal.DealCloseListModel dealClosingModel = new DeepBlue.Models.Deal.DealCloseListModel();
								dealClosingModel.DealClosingId = Convert.ToInt32(row.cell[0]);
								dealClosingModel.DealNumber = Convert.ToInt32(row.cell[1]);
								//dealClosingModel.DealCloseName = Convert.ToString(row.cell[2]);
								dealClosingModel.CloseDate = Convert.ToDateTime(row.cell[3]).Date;
								//dealClosingModel.TotalNetPurchasePrice = Convert.ToDecimal(row.cell[4]);
								dealClosings.Add(dealClosingModel);
							}
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return dealClosings;
		}

		private static CreateDealCloseModel GetDealClose(CookieCollection cookies, int dealId, int? dealClosingId, out string resp) {
			resp = string.Empty;
			CreateDealCloseModel newDealModel = null;
			List<DeepBlue.Models.Deal.DealCloseListModel> dealClosings = new List<DealCloseListModel>();
			List<UnderlyingFundPostRecordCashDistributionModel> ufPRCC = new List<UnderlyingFundPostRecordCashDistributionModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/GetDealCloseDetails?dealId=" + dealId + "&id=" + (dealClosingId.HasValue ? dealClosingId.ToString() : "0"));
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							newDealModel = (CreateDealCloseModel)js.Deserialize(resp, typeof(CreateDealCloseModel));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return newDealModel;
		}

		private static int? AddFundsAndDirectsToDealClose(CookieCollection cookies, CreateDealCloseModel dealClosingDetail, out string resp) {
			int? dealClosingId = null;
			resp = string.Empty;
			CreateDealCloseModel model = new CreateDealCloseModel();
			// Required fields
			model.DealId = dealClosingDetail.DealId;
			model.DealClosingId = dealClosingDetail.DealClosingId;

			model.FundId = dealClosingDetail.FundId;

			//model.CloseDate = dealClosingDetail.CloseDate;
			// Purchase date is used to close date

			List<Fund> funds = FundImport.GetFunds(cookies, string.Empty);
			Fund fund = funds.Where(f => f.FundID == model.FundId).FirstOrDefault();
			string fundName = string.Empty;

			if (fund != null) {
				fundName = fund.FundName;
			}

			CreateDealCloseModel dealDetail = GetDealDetail(cookies, dealClosingDetail.DealId, out resp);

			if (dealDetail == null) {
				resp = "Deal Detail does not exist : " + model.DealId;
				Util.Log(resp);
				return 0;
			}

			DateTime? purchaseDate = GetPurchaseDate(fundName, (dealDetail.DealNumber ?? 0));

			if (purchaseDate.HasValue == false) {
				resp = "Purchase date does not exist : " + model.DealId;
				Util.Log(resp);
				return 0;
			}

			model.CloseDate = (purchaseDate ?? DateTime.Now).Date;

			model.DealNumber = dealClosingDetail.DealNumber;

			NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty, new string[] { "DealUnderlyingFunds", "DealUnderlyingDirects" });

			int totalDirects = 0;
			foreach (DealUnderlyingDirectModel direct in dealClosingDetail.DealUnderlyingDirects) {
				if (!direct.DealClosingId.HasValue) {
					if (direct.RecordDate.HasValue) {
						direct.RecordDate = (direct.RecordDate ?? Convert.ToDateTime("01/01/1900")).ToLocalTime();
					}
					direct.IsClose = true;
					formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(direct, (++totalDirects) + "_", string.Empty));
				}
			}

			int totalUFs = 0;
			foreach (DealUnderlyingFundModel uf in dealClosingDetail.DealUnderlyingFunds) {
				if (!uf.DealClosingId.HasValue) {
					if (uf.RecordDate.HasValue) {
						uf.RecordDate = (uf.RecordDate ?? Convert.ToDateTime("01/01/1900")).ToLocalTime();
					}
					uf.IsClose = true;
					formValues = formValues.Combine(HttpWebRequestUtil.SetUpForm(uf, (++totalUFs) + "_", string.Empty));
				}
			}

			if (totalUFs > 0 || totalDirects > 0) {
				formValues.Add("TotalUnderlyingFunds", totalUFs.ToString());
				formValues.Add("TotalUnderlyingDirects", totalDirects.ToString());

				// Send the request 
				// dealClose.saveDealClose
				// Deal/UpdateDealClosing
				string url = HttpWebRequestUtil.GetUrl("Deal/UpdateDealClosing");
				byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
				HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, Globals.CookieContainer);
				if (response.StatusCode == System.Net.HttpStatusCode.OK) {
					using (Stream receiveStream = response.GetResponseStream()) {
						// Pipes the stream to a higher level stream reader with the required encoding format. 
						using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
							resp = readStream.ReadToEnd();
							dealClosingId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
							response.Close();
							readStream.Close();
						}
					}
				}
			}
			else {
				resp = "There were no open UF/Direts for this deal, probably all of them are already closed. DealId:" + model.DealId;
				Util.Log(resp);
				dealClosingId = model.DealClosingId;
			}
			return dealClosingId;
		}

		private static CreateDealCloseModel GetDealDetail(CookieCollection cookies, int dealId, out string resp) {
			resp = string.Empty;
			CreateDealCloseModel dealModel = null;
			List<DeepBlue.Models.Deal.DealCloseListModel> dealClosings = new List<DealCloseListModel>();
			List<UnderlyingFundPostRecordCashDistributionModel> ufPRCC = new List<UnderlyingFundPostRecordCashDistributionModel>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Deal/FindDealCloseModel?dealId=" + dealId);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							dealModel = (CreateDealCloseModel)js.Deserialize(resp, typeof(CreateDealCloseModel));
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return dealModel;
		}

		private static DateTime? GetPurchaseDate(string amberBrookFundName, int dealNo) {
			using (BlueEntities blueContext = new BlueEntities()) {
				string amberBrookFundNo = string.Empty;
				amberBrookFundNo = (from fund in blueContext.C6_10AmberbrookFundInfo
									where fund.AmberbrookFundName == amberBrookFundName
									select fund.AmberbrookFundNo).FirstOrDefault();
				return (from purchaseDetail in blueContext.C8_406qryDealPurchaseDate
						where purchaseDetail.AmberbrookFundNo == amberBrookFundNo && purchaseDetail.DealNo == dealNo
						select purchaseDetail.PurchaseDate).FirstOrDefault();
			}
		}
	}
}


