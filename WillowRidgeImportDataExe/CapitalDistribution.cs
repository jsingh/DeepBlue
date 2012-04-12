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
    /// Activity=>Amberbrook funds=>Cash distribution
    /// AMB Fund => Investors
    /// </summary>
    class CapitalDistributionImport {
        public static List<KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception>> Errors = new List<KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception>>();
        public static List<KeyValuePair<CapitalDistribution, Exception>> ImportErrors = new List<KeyValuePair<CapitalDistribution, Exception>>();
        private static StringBuilder messageLog = new StringBuilder();

        public static int TotalConversionRecords = 0;
        public static int RecordsConvertedSuccessfully = 0;

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

        public static void ImportCapitalDistribution(CookieCollection cookies) {
            ImportErrors = new List<KeyValuePair<CapitalDistribution, Exception>>();
            TotalImportRecords = 0;
            RecordsImportedSuccessfully = 0;
            messageLog.AppendLine("<=========================BEGIN: Converting records Blue => DeepBlue=======================>");
            List<CapitalDistribution> capitalDistributions = ConvertFromBlueToDeepBlue(cookies);
            messageLog.AppendLine("<=========================END: Converting records Blue => DeepBlue=======================>");
            LogErrors(Errors);
            foreach (CapitalDistribution capitalDist in capitalDistributions) {
                TotalImportRecords++;
                try {
                    string resp = string.Empty;
                    string formData = string.Empty;
                    // First make sure that this capital call doesnt already exist in the system
                    bool? isCCAlreadyPresent = IsCapitalDistributionAlreadyPresent(capitalDist);
                    if (isCCAlreadyPresent.HasValue && isCCAlreadyPresent.Value) {
                        string msg = string.Format("Capital call already present correctponding to CapitalCallAmount: {0}, CapitalCallDate: {1}, CapitalCallDueDate: {2}", capitalDist.DistributionAmount, capitalDist.CapitalDistributionDate, capitalDist.CapitalDistributionDueDate);
                        Util.WriteWarning(msg);
                        messageLog.AppendLine(msg);
                    } else {
                        int? cDistID = CreateManualCapitalDistribution(cookies, capitalDist, out resp, out formData);

                        messageLog.AppendLine(string.Format("#{0} Attempting to create capital call:", TotalImportRecords));
                        messageLog.Append("Response from server: ").Append(resp).AppendLine();
                        messageLog.Append("Form Data: ").AppendLine(formData);
                        if (!cDistID.HasValue) {
                            ImportErrors.Add(new KeyValuePair<CapitalDistribution, Exception>(capitalDist, new Exception("Error creating Capital Distribition. Error:" + resp)));
                            string msg = "RESULT: FAIL";
                            messageLog.AppendLine(msg);
                            Util.WriteError(msg + " " + resp);
                        } else {
                            string msg = "RESULT: PASS";
                            messageLog.AppendLine(msg);
                            Util.WriteNewEntry(msg + " " + resp);
                            RecordsImportedSuccessfully++;
                        }
                    }
                } catch (Exception ex) {
                    ImportErrors.Add(new KeyValuePair<CapitalDistribution, Exception>(capitalDist, ex));
                }
            }
            LogErrors(ImportErrors);
        }

        private static List<CapitalDistribution> ConvertFromBlueToDeepBlue(CookieCollection cookies) {
            Util.Log("Fetching Capital Distribution from Blue............");
            List<CapitalDistribution> capitalDistributions = new List<CapitalDistribution>();
            TotalConversionRecords = 0;
            RecordsConvertedSuccessfully = 0;
            using (BlueEntities context = new BlueEntities()) {
                List<C2_10tblDistFromAmberbrookCash> blueCapitalDistributions = context.C2_10tblDistFromAmberbrookCash.ToList();
                foreach (C2_10tblDistFromAmberbrookCash blueCapitalDistribution in blueCapitalDistributions) {
                    try {
                        bool success = true;
                        string errorMsg = string.Empty;
                        TotalConversionRecords++;
                        Util.Log("<======================Importing record#" + TotalConversionRecords + "======================>");
                        CapitalDistribution capDist = GetCapitalDistributionFromBlue(blueCapitalDistribution, context, cookies, out errorMsg);
                        string msg = string.Format("#{0} Getting capital distribution C2_10tblDistFromAmberbrookCash => CapitalCall. C2_10tblDistFromAmberbrookCash.TransactionID: {1}, AmbFund#: {2} => DeepBlueFundID: {3}, NoticeDate: {4}, Total amount collected:{5}", TotalConversionRecords, blueCapitalDistribution.TransactionID, blueCapitalDistribution.AmberbrookFundNo, capDist.FundID, blueCapitalDistribution.NoticeDate, blueCapitalDistribution.TotalCashDistribution);
                        Util.Log(msg);
                        messageLog.Append(msg);
                        messageLog.Append(Environment.NewLine);
                        if (string.IsNullOrEmpty(errorMsg)) {

                            List<C6_30tblLPDistributions> blueCapitalDistLineItems = context.C6_30tblLPDistributions.Where(x => x.DistributionID == blueCapitalDistribution.TransactionID).ToList();
                            foreach (C6_30tblLPDistributions blueCapitalDistLineItem in blueCapitalDistLineItems) {
                                string resp = string.Empty;
                                CapitalDistributionLineItem lineItem = GetCapitalDistributionLineItemFromBlue(blueCapitalDistLineItem, capDist.FundID, context, cookies, out resp);
                                if (!string.IsNullOrEmpty(resp)) {
                                    success = false;
                                    errorMsg += resp;
                                } else {
                                    messageLog.Append(string.Format("Line Item(C2_10tblDistFromAmberbrookCash => CapitalDistributionLineItem) level: BlueCallID:{0}, BlueMember:{1} => DeepBlueInvestorId:{2}, BlueCalledCapital:{3} => DeepBlueCapitalAmountCalled:{4}", blueCapitalDistLineItem.DistributionID, blueCapitalDistLineItem.LimitedPartner, lineItem.InvestorID, blueCapitalDistLineItem.DistributionAmount ?? 0, lineItem.DistributionAmount));
                                    messageLog.Append(Environment.NewLine);
                                }
                                capDist.CapitalDistributionLineItems.Add(lineItem);
                            }
                        } else {
                            success = false;
                        }
                        if (success) {
                            RecordsConvertedSuccessfully++;
                            capitalDistributions.Add(capDist);
                        } else {
                            Util.WriteError(errorMsg);
                            Errors.Add(new KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception>(blueCapitalDistribution, new Exception(errorMsg)));
                        }
                    } catch (Exception ex) {
                        Errors.Add(new KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception>(blueCapitalDistribution, ex));
                        Util.WriteError("ConvertBlueToDeepBlue() " + ex);
                    }
                }
            }
            Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalImportRecords, RecordsImportedSuccessfully));
            return capitalDistributions;
        }

        private static CapitalDistribution GetCapitalDistributionFromBlue(C2_10tblDistFromAmberbrookCash blueCapitalDist, BlueEntities context, CookieCollection cookies, out string resp) {
            resp = string.Empty;
            CapitalDistribution deepBlueCD = new CapitalDistribution();
			deepBlueCD.CapitalDistributionDate = blueCapitalDist.NoticeDate.Date;
			deepBlueCD.CapitalDistributionDueDate = blueCapitalDist.EffectiveDate.Date;
            deepBlueCD.DistributionAmount = (decimal)blueCapitalDist.TotalCashDistribution;
            deepBlueCD.IsManual = true;
            if (blueCapitalDist.TotalCarry.HasValue) {
                deepBlueCD.PreferredReturn = (decimal)blueCapitalDist.TotalCarry.Value;
            }
            if (blueCapitalDist.TotalRepayment.HasValue) {
                deepBlueCD.ReturnManagementFees = (decimal)blueCapitalDist.TotalRepayment.Value;
            }


            // Distribution#.. The server automatically assign this.. so we dont need to provider this

            // WARNING: The following fields are present in DeepBlue but are absent from blue
            //deepBlueCD.CapitalDistributionProfit;
            //deepBlueCD.CapitalReturn;
            //deepBlueCD.LPProfits;
            //deepBlueCD.PreferredCatchUp;
            //deepBlueCD.Profits;
            //deepBlueCD.ReturnFundExpenses;
            
            // The following fields is present in blue but absent from DeepBlue
            //blueCapitalDist.TotalUnusedCapital;


            int? fundId = GetFundID(blueCapitalDist.AmberbrookFundNo, context, cookies);
            if (fundId.HasValue) {
                deepBlueCD.FundID = fundId.Value;
            } else {
                resp = "cannot find fund with AmberbrookFundNo: " + blueCapitalDist.AmberbrookFundNo;
            }
            return deepBlueCD;
        }

        private static CapitalDistributionLineItem GetCapitalDistributionLineItemFromBlue(C6_30tblLPDistributions blueCapitalDistLineItem, int fundID, BlueEntities context, CookieCollection cookies, out string resp) {
            resp = string.Empty;
            CapitalDistributionLineItem deepBlueCDLineItem = new CapitalDistributionLineItem();
            if (blueCapitalDistLineItem.DistributionAmount.HasValue) {
                deepBlueCDLineItem.DistributionAmount = (decimal)blueCapitalDistLineItem.DistributionAmount.Value;
            }
            // Investor ID
            int? investorId = GetInvestorID(blueCapitalDistLineItem.LimitedPartner, fundID, context, cookies);
            if (investorId.HasValue) {
                deepBlueCDLineItem.InvestorID = investorId.Value;
            } else {
                resp = string.Format("Unable to find investor: {0} in FundID: {1} (AmberbrookFund#: {2})" , blueCapitalDistLineItem.LimitedPartner, fundID, blueCapitalDistLineItem.AmberbrookFundNo);
                Util.WriteError(resp);
            }

            deepBlueCDLineItem.PaidON = blueCapitalDistLineItem.TransactionDate;
            deepBlueCDLineItem.IsReconciled = true;

            // WARNING: The following fields are present in DeepBlue but are not present in Blue
            //deepBlueCDLineItem.CapitalReturn
            //deepBlueCDLineItem.LPProfits;
            //deepBlueCDLineItem.PreferredCatchUp;
            //deepBlueCDLineItem.PreferredReturn;
            //deepBlueCDLineItem.Profits;
            //deepBlueCDLineItem.ReconciliationMethod;
            //deepBlueCDLineItem.ReturnFundExpenses;
            //deepBlueCDLineItem.ReturnManagementFees;

            // Preferred Return and Return Management Fees are available on the Capital Distribution level.
            // Distribute it pro-rate?
            // Get the commitment for investor in this fund
            return deepBlueCDLineItem;
        }

        private static int? GetFundID(string fundNumber, BlueEntities context, CookieCollection cookies) {
            int? fundId = null;
            C6_10AmberbrookFundInfo blueFund = GetBlueFund(fundNumber, context);
            if (blueFund != null) {
                DeepBlue.Models.Entity.Fund deepBlueFund = GetDeepBlueFund(blueFund.AmberbrookFundName, cookies);
                if (deepBlueFund != null) {
                    fundId = deepBlueFund.FundID;
                } else {
                }
            } else {
            }
            return fundId;
        }

        private static Hashtable BlueFunds = new Hashtable();
        private static C6_10AmberbrookFundInfo GetBlueFund(string fundNumber, BlueEntities context) {
            if (BlueFunds.ContainsKey(fundNumber)) {
                return (C6_10AmberbrookFundInfo)BlueFunds[fundNumber];
            } else {
                C6_10AmberbrookFundInfo fund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(fundNumber)).FirstOrDefault();
                if (fund != null) {
                    BlueFunds.Add(fundNumber, fund);
                }
                return fund;
            }
        }

        private static Hashtable DeepBlueFunds = new Hashtable();
        private static DeepBlue.Models.Entity.Fund GetDeepBlueFund(string fundName,  CookieCollection cookies) {
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
            } else if (investors.Count > 1) {
                Util.WriteError(string.Format("More than one investor found with name: {0} and fund: {1}", limitedPartner, fundID));
            } else if (investors.Count == 1) {
                investor = investors[0].InvestorID;
                Investors.Add(limitedPartner, investor.Value);
            }
            return investor;
        }

        private static int? CreateManualCapitalDistribution(CookieCollection cookies, CapitalDistribution capitalDistribution, out string resp, out string formdata) {
            int? capitalDistributionID = null;
            resp = string.Empty;
            formdata = string.Empty;
			//DeepBlue.Models.CapitalCall.CreateDistributionModel model = new DeepBlue.Models.CapitalCall.CreateDistributionModel();
			//model.CapitalDistributionDate = capitalDistribution.CapitalDistributionDate.Date;
			//model.CapitalDistributionDueDate = capitalDistribution.CapitalDistributionDueDate.Date;
			//model.FundId = capitalDistribution.FundID;

			//// initialize
			//model.CapitalReturn = model.PreferredReturn = model.PreferredCatchUp = model.ReturnFundExpenses = model.ReturnManagementFees = model.GPProfits = model.LPProfits = 0;
			//// Cost returned
			//if (capitalDistribution.CapitalReturn.HasValue) {
			//    model.CapitalReturn = capitalDistribution.CapitalReturn;
			//}
			//// 
			//model.DistributionAmount = capitalDistribution.DistributionAmount;
			//DeepBlue.Models.CapitalCall.FundDetail fundDetail = FundImport.GetFundDetail(cookies, capitalDistribution.FundID);
			//if (fundDetail != null) {
			//    model.DistributionNumber = fundDetail.DistributionNumber.Value.ToString();
			//}
            
			//// Profits returned
			//if (capitalDistribution.PreferredReturn.HasValue) {
			//    model.PreferredReturn = capitalDistribution.PreferredReturn;
			//}
			//if (capitalDistribution.ReturnFundExpenses.HasValue) {
			//    model.ReturnFundExpenses = capitalDistribution.ReturnFundExpenses;
			//}
			//if (capitalDistribution.ReturnManagementFees.HasValue) {
			//    model.ReturnManagementFees = capitalDistribution.ReturnManagementFees;
			//}
			//// the server does the distribution check. We need to make it pass

			//decimal distributionCheck = (model.CapitalReturn.Value + model.PreferredReturn.Value + model.PreferredCatchUp.Value + model.ReturnFundExpenses.Value + model.ReturnManagementFees.Value + model.GPProfits.Value + model.LPProfits.Value);
			//if (distributionCheck != model.DistributionAmount) {
			//    model.CapitalReturn = model.CapitalReturn + (model.DistributionAmount - distributionCheck);
			//}

			//// UI doesnt seem to ask for the following fields
			////model.FundId;
			////model.FundName;
			////model.GPProfits = capitalDistribution.Profits;
			////model.LPProfits;
			////model.PreferredCatchUp;
			////model.TotalDistribution;
			////model.TotalProfit;
			//model.InvestorCount = capitalDistribution.CapitalDistributionLineItems.Count;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(capitalDistribution, string.Empty, string.Empty, new string[] { "CapitalDistributionLineItems" });


			Util.Log("Capital Distribution FundID : " + capitalDistribution.FundID);

			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/ImportManualDistribution");
			//string url = HttpWebRequestUtil.GetUrl("CapitalCall/CreateDistribution");
			formdata = HttpWebRequestUtil.ToFormValue(formValues);
			byte[] postData = System.Text.Encoding.ASCII.GetBytes(formdata);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						capitalDistributionID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
						response.Close();
						readStream.Close();
					}
				}

			}
			if (capitalDistributionID > 0) {
				Util.Log("Capital Distribution ID : " + capitalDistributionID);
			}
			else {
				Util.Log("Capital Distribution Error : " + resp);
			}

			
            if (capitalDistribution.CapitalDistributionLineItems.Count > 0 && capitalDistributionID > 0) {
                int index = 0;
				int? id = 0;
                foreach (CapitalDistributionLineItem li in capitalDistribution.CapitalDistributionLineItems.ToList()) {
					li.CapitalDistributionID = (capitalDistributionID ?? 0);
                    index++;
					formValues = HttpWebRequestUtil.SetUpForm(li, string.Empty, string.Empty);
					url = HttpWebRequestUtil.GetUrl("CapitalCall/ImportManualDistributionLineItem");
					formdata = HttpWebRequestUtil.ToFormValue(formValues);
					postData = System.Text.Encoding.ASCII.GetBytes(formdata);
					response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
					if (response.StatusCode == System.Net.HttpStatusCode.OK) {
						using (Stream receiveStream = response.GetResponseStream()) {
							// Pipes the stream to a higher level stream reader with the required encoding format. 
							using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
								resp = readStream.ReadToEnd();
								id = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
								Util.Log("Capital Distribution Line Item ID : " + id);
								response.Close();
								readStream.Close();
							}
						}

					}

                }
            }

			return capitalDistributionID;
        }

        private static bool? IsCapitalDistributionAlreadyPresent(CapitalDistribution capitalDistribution) {
			CapitalDistributionDetail detail = null;
			try {
				detail = FindCapitalDistributionDetail(Globals.CookieContainer, capitalDistribution.FundID, capitalDistribution.DistributionAmount, capitalDistribution.CapitalDistributionDate, capitalDistribution.CapitalDistributionDueDate);
			}
			catch (Exception ex) {
				Util.Log("IsCapitalDistributionAlreadyPresent:" + ex.Message);
			}
			return detail != null;
        }


		public static CapitalDistributionDetail FindCapitalDistributionDetail(CookieCollection cookies, int fundID, decimal? capitalDistributionAmount, DateTime? capitalDistributionDate, DateTime? capitalDistributionDueDate) {
			CapitalDistributionDetail detail = null;
			// Send the request 
			string query = string.Empty;
			string resp = string.Empty;
			query = "&fundId=" + fundID + "&capitalDistributionAmount=" + capitalDistributionAmount + "&capitalDistributionDate=" + capitalDistributionDate + "&capitalDistributionDueDate=" + capitalDistributionDueDate;
			string url = HttpWebRequestUtil.GetUrl("CapitalCall/FindCapitalDistributionDetail?" + query);
			HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
			if (response.StatusCode == System.Net.HttpStatusCode.OK) {
				using (Stream receiveStream = response.GetResponseStream()) {
					// Pipes the stream to a higher level stream reader with the required encoding format. 
					using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
						resp = readStream.ReadToEnd();
						if (!string.IsNullOrEmpty(resp)) {
							JavaScriptSerializer js = new JavaScriptSerializer();
							detail = (CapitalDistributionDetail)js.Deserialize(resp, typeof(CapitalDistributionDetail));
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

        private static void LogErrors(List<KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

                foreach (KeyValuePair<C2_10tblDistFromAmberbrookCash, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.AmberbrookFundNo + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + kv.Value.StackTrace);
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<DeepBlue.Models.Entity.CapitalDistribution, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +Environment.NewLine + string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<DeepBlue.Models.Entity.CapitalDistribution, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.FundID + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }

    }
}

