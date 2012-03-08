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
using DeepBlue.Models.Admin;

// Blue doesnt have a concept of Partner
// Deepblue doesnt provide a way for creating Contacts curently. So the following is put on hold for now
// => Contact
// Seller Name
// Seller Company
// Seller Phone
// Seller Fax
// Seller Email
// Seller Comments
// We need to create Seller Type in DeepBlue
// Seller Type DeepBlue has InvestorEntityType which has same values as SellerType. However, InvestorEntityType is on the Investor level

namespace DeepBlue.ImportData {
    class DealImport {
        private static Hashtable StateAbbr = new Hashtable();
        public static List<KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>> ImportErrors = new List<KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>>();
        private static StringBuilder messageLog = new StringBuilder();

        public static int TotalImportRecords = 0;
        public static int RecordsImportedSuccessfully = 0;

    //     private static NameValueCollection ContactsList = new NameValueCollection();

		private static List<DealContactList> ContactsList = new List<DealContactList>();

        private static List<AutoCompleteListExtend> _equitiesAndFIs = null;
        private static List<AutoCompleteListExtend> GetDeepBlueEquitiesAndFIs(CookieCollection cookies) {
            // Get all the equities and FIs in the system
            if (_equitiesAndFIs == null) {
                _equitiesAndFIs = DirectsImport.GetDirects(cookies);
            }
            return _equitiesAndFIs;
        }

        public static void ImportDeals(CookieCollection cookies) {
			ContactsList = GetDealContactsFromDeepBlue(cookies);
            Util.Log("Fetching Directs from Blue............");
            using (BlueEntities context = new BlueEntities()) {
                List<C6_15tblAmberbrookDealInfo> blueDeals = context.C6_15tblAmberbrookDealInfo.ToList();
                foreach (C6_15tblAmberbrookDealInfo blueDeal in blueDeals) {
                    try {
                        bool success = true;
                        string errorMsg = string.Empty;
                        TotalImportRecords++;
                        Util.Log("<======================Importing record#" + TotalImportRecords + "======================>");
                        string amberBrookFundNumber = blueDeal.AmberbrookFundNo;
                        C6_10AmberbrookFundInfo amberBrookFund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(amberBrookFundNumber)).FirstOrDefault();
                        if (amberBrookFund != null) {
                            // try to find the amber brook fund from DeepBlue
                            List<DeepBlue.Models.Entity.Fund> ufs = FundImport.GetFunds(cookies);
                            DeepBlue.Models.Entity.Fund fund = ufs.Where(x => x.FundName.Equals(amberBrookFund.AmberbrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                            //List<DeepBlue.Models.Deal.UnderlyingFundListModel> ufs = UnderlyingFundImport.GetUnderlyingFunds(cookies);
                            //UnderlyingFundListModel ufListModel = ufs.Where(x => x.FundName.Equals(amberBrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (fund != null) {
                                // Find a deal.. Add to the deal if it already exists, create new deal otherwise
                                // Get all the deals from DeepBlue. We need to do this every time, as if you add a new deal, you want it to be fetched back
                                List<DeepBlue.Models.Deal.DealListModel> deals = DealImport.GetDeals(cookies, null);

                                // Get the DeepBlue deal with the same Amberbrook Fund and DealName as from blue
                                // The following line is replaced:
                                //DeepBlue.Models.Deal.DealListModel deal = deals.Where(x => x.DealName.Equals(blueDeal.DealName)).Where(x=>x.FundName.Equals(amberBrookFund.AmberbrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                DeepBlue.Models.Deal.DealListModel deal = null;
                                // BY:
                                // Get the deals for AMB Fund
                                List<Deal> dls = GetDealsInFund(cookies, fund.FundID);
                                if (dls.Count > 0) {
                                    Deal targetDeal = dls.Where(x => x.DealName.Equals(blueDeal.DealName)).SingleOrDefault();
                                    if (targetDeal != null) {
                                        deal = new DealListModel() { DealId = targetDeal.DealID, DealName = targetDeal.DealName };
                                    }
                                }
                                int? dealId = null;
                                if (deal == null) {
                                    // Create a new deal
                                    int? purchaseType = null;
                                    PurchaseType pType = Globals.PurchaseTypes.Where(x => x.Name.Equals(blueDeal.PurchaseType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                                    if (pType != null) {
                                        purchaseType = pType.PurchaseTypeID;
                                    } else{
                                        string errMsg = "cannot find purchaseType: "+blueDeal.PurchaseType;
                                        Util.WriteError(errMsg);
                                        messageLog.AppendLine(errMsg);
                                    }
                                    string resp = string.Empty;
                                    dealId = CreateNewDeal(cookies, blueDeal, fund.FundID, purchaseType, out resp);
                                    if (dealId.HasValue && dealId.Value > 0) {
                                        deal = new DealListModel() { DealId = dealId.Value, DealName = blueDeal.DealName };
                                        string msg = "New Deal:" + dealId;
                                        Util.WriteNewEntry(msg);
                                        messageLog.AppendLine(msg);
                                    } else {
                                        string errMsg = "cannot create deal. Resp: " + resp;
                                        Util.WriteError(errMsg);
                                        messageLog.AppendLine(errMsg);
                                        ImportErrors.Add(new KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>(blueDeal, new Exception()));
                                        continue;
                                    }
                                } else {
                                    dealId = deal.DealId;
                                    Util.WriteWarning("Deal aready exists. DealID:" + dealId);
                                }

                                if (deal != null) {
									bool importAssets = true;
                                    bool importExpenses = true;
									bool importSellerInfo = true;
									bool importContact = true;

                                    string error = string.Empty;
                                    string err = string.Empty;
                                    string resp = string.Empty;

                                    string seperatorFormat = "---------------------------{0}--------------------------";
                                    if (importAssets) {
                                        // Now we have got the deal, lets import all the Directs and Underlying Funds
                                        Util.Log(string.Format(seperatorFormat, "importing assets in deal"));
                                        error = ImportAssetInDeal(cookies, context, blueDeal, amberBrookFund, deal, fund.FundID);
                                        if (!string.IsNullOrEmpty(error)) {
                                            success = false;
                                        }
                                    } else {
                                        Util.WriteWarning(string.Format(seperatorFormat, "Skipping importing assets in deal"));
                                    }

                                    DealDetailModel deepBlueDealDetail = GetDeal(cookies, deal.DealId);
                                    if (importExpenses) {
                                        Util.Log(string.Format(seperatorFormat, "importing expenses in deal"));
                                        err = ImportDealExpenses(cookies, context, blueDeal, deepBlueDealDetail);
                                        if (!string.IsNullOrEmpty(err)) {
                                            Util.WriteError(err);
                                        }
                                        error += err;
                                        if (!string.IsNullOrEmpty(error)) {
                                            success = false;
                                        }
                                    } else {
                                        Util.WriteWarning(string.Format(seperatorFormat, "skipping importing expenses in deal"));

                                    }

                                    if (importSellerInfo) {
                                        Util.Log(string.Format(seperatorFormat, "importing seller in deal"));
                                        int? dealSellerId = ImportDealSellerInfo(cookies, blueDeal, deepBlueDealDetail, out resp);
                                        // if (!dealSellerId.HasValue) {
                                        // TODO
                                        if (!string.IsNullOrEmpty(resp)) {// means there is error. Currently /Deal/CreateSelerInfo doesnt return the new ID. ASK Prasanna to correct this
                                            err = "Failed to import deal seller for amb fund#:" + blueDeal.AmberbrookFundNo + " ,deal#:" + blueDeal.DealNo + ", error:" + resp;
                                            Util.WriteError(err);
                                            error += err;
                                            success = false;
                                        } else {
                                            Util.WriteNewEntry("New DealSellerID:" + dealSellerId);
                                        }
                                    } else {
                                        Util.WriteWarning(string.Format(seperatorFormat, "skipping importing seller in deal"));
                                    }

                                    if (importContact) {
                                        resp = string.Empty;
                                        Util.Log(string.Format(seperatorFormat, "importing contact in deal"));
                                        int? dealContactId = ImportDealContact(cookies, blueDeal, deepBlueDealDetail, out resp);
                                        if (!dealContactId.HasValue) {
                                            err = "Failed to import deal contact for amb fund#:" + blueDeal.AmberbrookFundNo + " ,deal#:" + blueDeal.DealNo + ", error:" + resp;
                                            Util.WriteError(err);
                                            error += err;
                                            success = false;
                                        } else {
                                            Util.Log("Imported Contact for the deal....");
                                        }
                                    } else {
                                        Util.WriteWarning(string.Format(seperatorFormat, "skipping importing contact in deal"));
                                    }

                                    if (!success) {
                                        ImportErrors.Add(new KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>(blueDeal, new Exception(error)));
                                    }

                                } else {
                                    success = false;
                                    errorMsg = string.Format("Could not create/find DeepBlue deal with DealName:{0} and FundName:{1}", blueDeal.DealName, amberBrookFund.AmberbrookFundName);
                                    Util.WriteError(errorMsg);
                                }
                            } else {
                                // Log this.. Make sure the underlying fund is already created in DeepBlue
                                success = false;
                                errorMsg = "Could not find AMB Fund from DeepBlue:" + amberBrookFund.AmberbrookFundName;
                                Util.WriteError(errorMsg);
                                messageLog.AppendLine(errorMsg);
                            }
                        } else {
                            // Log it
                            success = false;
                            errorMsg = "Could not find AMB Fund from Blue with Fund#:" + amberBrookFundNumber;
                            Util.WriteError(errorMsg);
                            messageLog.AppendLine(errorMsg);
                        }
                        if (success) {
                            RecordsImportedSuccessfully++;
                        } else {
                            ImportErrors.Add(new KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>(blueDeal, new Exception(errorMsg)));
                        }
                    } catch (Exception ex) {
                        ImportErrors.Add(new KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>(blueDeal, ex));
                        Util.WriteError("ConvertBlueToDeepBlue() " + ex);
                    }
                }
            }
            Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalImportRecords, RecordsImportedSuccessfully));
            LogErrors(ImportErrors);
            LogMessages();
        }

        private static string ImportAssetInDeal(CookieCollection cookies, BlueEntities context, C6_15tblAmberbrookDealInfo blueDeal, C6_10AmberbrookFundInfo blueDealAmbFund, DeepBlue.Models.Deal.DealListModel deepBlueDeal, int fundID) {
            StringBuilder sb = new StringBuilder();
            List<C5_10tblDealOrigination> blueAssetsInDeal = context.C5_10tblDealOrigination.Where(x => x.AmberbrookFundNo == blueDeal.AmberbrookFundNo).Where(x => x.DealNo == blueDeal.DealNo).ToList();
            DealDetailModel deepBlueDealDetail = GetDeal(cookies, deepBlueDeal.DealId);
            foreach (C5_10tblDealOrigination blueAssetInDeal in blueAssetsInDeal) {
                // This can either be a direct or a UF
                C7_10tblGPPaymentInstructions blueAsset = context.C7_10tblGPPaymentInstructions.Where(x => x.Fund.Equals(blueAssetInDeal.Fund, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (blueAsset != null) {
                    // Find the deal with the same DealNumber and DealName, and that belongs to the same Amberbrook fund
                    if (blueAsset.FundType.Equals("Direct", StringComparison.OrdinalIgnoreCase)) {
                         List<KeyValuePair<C5_11tblDealOriginationDirects, string>> failedDirects = ImportUnderlyingDirect(cookies, context, blueAssetInDeal, deepBlueDeal.DealId, deepBlueDealDetail, fundID);
                        if (failedDirects.Count > 0) {
                            Util.WriteError("The following " + failedDirects.Count + " securities could not be imported:");
                        }
                        foreach (KeyValuePair<C5_11tblDealOriginationDirects, string> failedDirect in failedDirects) {
                            string errorMsg = "Cannot import direct:" + failedDirect.Key.Direct + " into the deal:" + deepBlueDeal.DealId + ", reason:" + failedDirect.Value;
                            Util.WriteError(errorMsg);
                            sb.Append(" " + errorMsg + " ");
                        }
                    } else {
                        string resp = string.Empty;
                        int? underlyingFundId = ImportUnderlyingFund(cookies, blueAssetInDeal, deepBlueDeal.DealId, fundID, deepBlueDealDetail, out resp);
                        if (!underlyingFundId.HasValue) {
                            string error = "Failed to import underlying fund:" + blueAssetInDeal.Fund + " into deal:" + deepBlueDeal.DealId + ", error:" + resp;
                            Util.WriteError(error);
                            sb.Append(error);
                        } else {
                            Util.Log("New DealUnderlyingFundID:" + underlyingFundId);
                        }
                    }
                } else {
                    string msg = "Unable to find record in C7_10tblGPPaymentInstructions with Fund:" + blueAssetInDeal.Fund;
                    Util.WriteError(msg);
                    sb.Append(msg);
                }
            }
            return sb.ToString();
        }

        private static string ImportDealExpenses(CookieCollection cookies, BlueEntities context, C6_15tblAmberbrookDealInfo blueDeal, DealDetailModel deepBlueDealDetail) {
            int dealId = deepBlueDealDetail.DealId;
            StringBuilder sb = new StringBuilder();
            string message = string.Empty;
            if (deepBlueDealDetail.DealExpenses == null || deepBlueDealDetail.DealExpenses.Count == 0) {
                List<C6_40tblDealExpenses> blueDealExpenses = context.C6_40tblDealExpenses.Where(x => x.AmberbrookFundNo == blueDeal.AmberbrookFundNo).Where(x => x.AmberbrookDealNo == blueDeal.DealNo).ToList();
                if (blueDealExpenses.Count > 0) {
                    foreach (C6_40tblDealExpenses blueDealExpense in blueDealExpenses) {
                        string resp = string.Empty;
                        int? dealExpenseId = ImportDealExpense(cookies, blueDealExpense, dealId, out resp);
                        if (!dealExpenseId.HasValue) {
                            string error = "Failed to import deal expense for amb fund#:" + blueDealExpense.AmberbrookFundNo + " ,deal#:" + blueDealExpense.AmberbrookDealNo + ", error:" + resp;
                            // Util.WriteError(error);
                            sb.Append(error);
                        } else {
                            message = "New DealExpenseID:" + dealExpenseId;
                            Util.WriteNewEntry(message);
                            messageLog.AppendLine(message);
                        }
                    }
                } else {
                    Util.Log("No deal expenses found to import..");
                }

            } else {
                message = "Deal:" + dealId + " already has deal expenses. Skipping Importing Deal Expenses";
                Util.WriteWarning(message);
                messageLog.AppendLine(message);
            }
            return sb.ToString();
        }

        private static int? ImportDealExpense(CookieCollection cookies, C6_40tblDealExpenses dealExpense, int dealId, out string resp) {
            int? dealExpenseId = null;
            resp = string.Empty;
            DealClosingCostModel model = new DealClosingCostModel();
            model.DealId = dealId;
            model.Amount = (decimal)dealExpense.Amount;
            model.Date = dealExpense.Date;

			if (model.Amount > 0) {


				DealClosingCostType defaultClosingCostType = Globals.DealClosingCostTypes.Where(x => x.Name.Equals(Globals.LegalFee)).FirstOrDefault();

				DealClosingCostType dclosingCostType = Globals.DealClosingCostTypes.Where(x => x.Name.Equals(dealExpense.Description)).FirstOrDefault();
				if (dclosingCostType != null) {
					model.DealClosingCostTypeId = Globals.DealClosingCostTypes.Where(x => x.Name.Equals(dealExpense.Description)).SingleOrDefault().DealClosingCostTypeID;
				}
				else {
					model.DealClosingCostTypeId = defaultClosingCostType.DealClosingCostTypeID;
				}

				NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

				// Send the request 
				string url = HttpWebRequestUtil.GetUrl("Deal/CreateDealExpense");
				string data = HttpWebRequestUtil.ToFormValue(formValues);
				messageLog.AppendLine("Form Data:" + data);
				byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
				HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
				if (response.StatusCode == System.Net.HttpStatusCode.OK) {
					using (Stream receiveStream = response.GetResponseStream()) {
						// Pipes the stream to a higher level stream reader with the required encoding format. 
						using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
							resp = readStream.ReadToEnd();
							messageLog.AppendLine("Response: " + resp);
							dealExpenseId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
							response.Close();
							readStream.Close();
						}
					}
				}
			}
            return dealExpenseId;
        }

        private static int? ImportDealSellerInfo(CookieCollection cookies, C6_15tblAmberbrookDealInfo blueDeal, DealDetailModel deepBlueDealDetail, out string resp) {
            int dealId = deepBlueDealDetail.DealId;
            int? sellerId = null;
            resp = string.Empty;
            string message = string.Empty;

            if (!deepBlueDealDetail.SellerContactId.HasValue) {
                if (!string.IsNullOrEmpty(blueDeal.SellerName)) {
                    DealSellerDetailModel model = new DealSellerDetailModel();
                    model.DealId = deepBlueDealDetail.DealId;
                    model.SellerName = blueDeal.SellerName;
                    model.ContactName = blueDeal.SellerCompany;
                    model.Phone = blueDeal.SellerPhone;
                    model.Email = blueDeal.SellerEmail;
                    model.Fax = blueDeal.SellerFax;
                    model.CompanyName = blueDeal.SellerCompany;
                    model.SellerTypeId = Globals.SellerTypes.Where(x => x.SellerType1.Equals(blueDeal.SellerType)).FirstOrDefault().SellerTypeID;


                    NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

                    // Send the request 
                    string url = HttpWebRequestUtil.GetUrl("/Deal/CreateSellerInfo");
                    string data = HttpWebRequestUtil.ToFormValue(formValues);
                    messageLog.AppendLine("Form data:"+data);
                    byte[] postData = System.Text.Encoding.ASCII.GetBytes(data);
                    HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        using (Stream receiveStream = response.GetResponseStream()) {
                            // Pipes the stream to a higher level stream reader with the required encoding format. 
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                                resp = readStream.ReadToEnd();
                                messageLog.AppendLine("Response: " + resp);
                                sellerId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                                response.Close();
                                readStream.Close();
                            }
                        }
                    }
                } else {
                    message = "Blue Deal: " + blueDeal.DealName + " doesnt have a seller";
                    Util.Log(message);
                    return 0;
                }
            } else {
                sellerId = deepBlueDealDetail.SellerContactId;
                message = "Seller Contact already exists:" + sellerId;
                Util.WriteWarning(message);
                messageLog.AppendLine(message);
            }
            return sellerId;
        }

/*        private static int? ImportDealContactOld(CookieCollection cookies, C6_15tblAmberbrookDealInfo blueDeal, DealDetailModel deepBlueDealDetail, out string resp) {
            int? contactId = null;
            resp = string.Empty;
            string message = string.Empty;
            if (!deepBlueDealDetail.ContactId.HasValue) {
                // First try to find id there is already a contact with the same name present
                // TODO: try fetching all the deal contacts first. Currently it is throwing an exception
                // List<DeepBlue.Models.Admin.DealContactList> dealContacts = GetDealContacts(cookies);
                List<DeepBlue.Models.Admin.DealContactList> dealContacts = new List<Models.Admin.DealContactList>();

                DeepBlue.Models.Admin.DealContactList dealContact = dealContacts.Where(x => x.ContactName.Equals(blueDeal.AmberContactName)).FirstOrDefault();
                if (dealContact != null) {
                    contactId = dealContact.ContactId;
                    Util.Log("Found contact..");
                } else {
                    contactId = CreateDealContact(cookies, blueDeal, deepBlueDealDetail, out resp);
                    if (contactId.HasValue) {
                        message = "New Contact created:" + contactId + " for deal:" + deepBlueDealDetail.DealId;
                        Util.WriteNewEntry(message);
                    }
                }
                if (contactId.HasValue) {
                    int? dealId = UpdateDealWithContact(cookies, contactId.Value, deepBlueDealDetail, out resp);
                    if (!dealId.HasValue) {
                        resp = string.Format("Unable to update deal: {0} with contactId: {1}, Reason: {2}", dealId, contactId, resp);
                    } else {
                        Util.WriteNewEntry(string.Format("deal: {0} updated with contactId: {1}, DealContactID: {2}", deepBlueDealDetail.DealId, contactId, dealId));
                    }
                }
                // 
            } else {
                contactId = deepBlueDealDetail.ContactId;
                message = "Contact already exists. ContactID:" + contactId;
                Console.Write(message);
            }
            return contactId;
        }
 * */

		public static List<DealContactList> GetDealContactsFromDeepBlue(CookieCollection cookies) {
			List<DealContactList> dealContacts = new List<DealContactList>();
			// Send the request 
			string url = HttpWebRequestUtil.GetUrl("Admin/DealContactList?pageIndex=1&pageSize=5000&sortName=ContactName&sortOrder=asc");
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
								DealContactList dealContact = new DealContactList();
								dealContact.ContactId = Convert.ToInt32(row.cell[0]);
								dealContact.ContactName = Convert.ToString(row.cell[1]);
								dealContacts.Add(dealContact);
							}
						}
						else {
						}
						response.Close();
						readStream.Close();
					}
				}
			}
			return dealContacts;
		}

        private static int? ImportDealContact(CookieCollection cookies, C6_15tblAmberbrookDealInfo blueDeal, DealDetailModel deepBlueDealDetail, out string resp) {
            int? contactId = null;
            resp = string.Empty;
            string message = string.Empty;
            if (!deepBlueDealDetail.ContactId.HasValue) {
				DealContactList dealContact = ContactsList.Where(c => c.ContactName == blueDeal.AmberContactName).FirstOrDefault();
				if (dealContact != null) {
					contactId = dealContact.ContactId;
                    Util.Log("Found contact..");
                } else {
                    contactId = CreateDealContact(cookies, blueDeal, deepBlueDealDetail, out resp);
                    if (contactId.HasValue) {
                        message = "New Contact created:" + contactId + " for deal:" + deepBlueDealDetail.DealId;
                        Util.WriteNewEntry(message);
                        messageLog.AppendLine(message);
						ContactsList.Add(new DealContactList { ContactName = blueDeal.AmberContactName, ContactId = contactId.Value });
                    }
                }
                if (contactId.HasValue) {
                    int? dealId = UpdateDealWithContact(cookies, contactId.Value, deepBlueDealDetail, out resp);
                    if (!dealId.HasValue) {
                        resp = string.Format("Unable to update deal: {0} with contactId: {1}, Reason: {2}", dealId, contactId, resp);
                    } else {
                        string success = string.Format("deal: {0} updated with contactId: {1}, DealContactID: {2}", deepBlueDealDetail.DealId, contactId, dealId);
                        Util.WriteNewEntry(success);
                        messageLog.AppendLine(success);
                    }
                }
                // 
            } else {
                contactId = deepBlueDealDetail.ContactId;
                message = "Contact already exists. ContactID:" + contactId;
                Console.Write(message);
            }
            return contactId;
        }

        private static int? CreateDealContact(CookieCollection cookies, C6_15tblAmberbrookDealInfo blueDeal, DealDetailModel deepBlueDealDetail, out string resp) {
            int? contactId = null;
            resp = string.Empty;

            DeepBlue.Models.Admin.EditDealContactModel model = new Models.Admin.EditDealContactModel();
            model.ContactName = blueDeal.AmberContactName;

            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("/Admin/UpdateDealContact");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        contactId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                    }
                }
            }

            return contactId;
        }

        private static int? UpdateDealWithContact(CookieCollection cookies, int contactID, DealDetailModel deepBlueDealDetail, out string resp) {
            int? dealId = null;
            resp = string.Empty;

            deepBlueDealDetail.ContactId = contactID;

            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(deepBlueDealDetail, string.Empty, string.Empty, new string[] { "SellerInfo", "DealExpenses", "DealUnderlyingFunds", "DealUnderlyingDirects" });

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("/Deal/Create");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        dealId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                    }
                }
            }

            return dealId;
        }

        private static List<DeepBlue.Models.Admin.DealContactList> GetDealContacts(CookieCollection cookies) {
            // Admin/DealContactList?pageIndex=1&pageSize=5000&sortName=ContactName&sortOrder=asc
            List<DeepBlue.Models.Admin.DealContactList> dealContacts = new List<DeepBlue.Models.Admin.DealContactList>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/DealContactList?pageIndex=1&pageSize=5000&sortName=ContactName&sortOrder=asc");
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
                                DeepBlue.Models.Admin.DealContactList dealContact = new DeepBlue.Models.Admin.DealContactList();
                                dealContact.ContactId = Convert.ToInt32(row.cell[0]);
                                dealContact.ContactName = Convert.ToString(row.cell[1]);
                                dealContact.ContactTitle = Convert.ToString(row.cell[2]);
                                dealContact.ContactNotes = Convert.ToString(row.cell[3]);
                                dealContact.Email = Convert.ToString(row.cell[4]);
                                dealContact.Phone = Convert.ToString(row.cell[5]);
                                dealContact.WebAddress = Convert.ToString(row.cell[6]);
                                dealContacts.Add(dealContact);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return dealContacts;
        }

        #region Underlying Direct
        private static List<KeyValuePair<C5_11tblDealOriginationDirects, string>> ImportUnderlyingDirect(CookieCollection cookies, BlueEntities context, C5_10tblDealOrigination blueAssetInDeal, int targetDealId, DealDetailModel deepBlueDealDetail, int fundID) {
            List<KeyValuePair<C5_11tblDealOriginationDirects, string>> failedDirects = new List<KeyValuePair<C5_11tblDealOriginationDirects, string>>();
            // This is a direct
            //string issuerName = blueAsset.Fund;
            List<C5_11tblDealOriginationDirects> blueSecurities = context.C5_11tblDealOriginationDirects.Where(x => x.Direct.Equals(blueAssetInDeal.Fund, StringComparison.OrdinalIgnoreCase)).Where(x => x.DealNo == blueAssetInDeal.DealNo).ToList();
            if (blueSecurities.Count > 0) {
                foreach (C5_11tblDealOriginationDirects blueSecurity in blueSecurities) {
                    // Equity from blue
                    C4_20tblStockTable blueStock = context.C4_20tblStockTable.Where(x => x.StockSymbol.Equals(blueSecurity.StockSymbol, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    // Find the direct corresponding to the security
                    List<AutoCompleteListExtend> equitiesAndFIs = GetDeepBlueEquitiesAndFIs(cookies);
                    string searchLabel = string.Format("{0}>>Equity>>{1}", blueStock.Company, blueStock.StockSymbol);
                    AutoCompleteListExtend equity = equitiesAndFIs.Where(x => x.otherid == (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity).Where(x => x.value.Equals(blueStock.Company, StringComparison.OrdinalIgnoreCase)).Where(x => x.label.Equals(searchLabel, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (equity == null) {
                        string errorMsg = "Unable to find an equity with issuer:" + blueStock.Company + ", and label:" + searchLabel;
                        Util.WriteError(errorMsg);
                        failedDirects.Add(new KeyValuePair<C5_11tblDealOriginationDirects, string>(blueSecurity, errorMsg));
                    } else {
                        // Create a DealUnderlyingDirect record
                        string resp = string.Empty;
                        // Make sure the direct/FI is already not part of the deal
                        DealUnderlyingDirectModel direct = deepBlueDealDetail.DealUnderlyingDirects.Where(x => x.SecurityId == equity.otherid2).Where(x => x.SecurityTypeId == (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity).FirstOrDefault();
                        if (direct == null) {
                            int? dealUnderlyingDirectId = CreateDealUnderlyingDirect(cookies, blueSecurity, blueAssetInDeal, targetDealId, equity.otherid2, equity.id, fundID, out resp);
                            if (!dealUnderlyingDirectId.HasValue || dealUnderlyingDirectId.Value <= 0) {
                                string errorMsg = string.Format("Unable to import underlying direct {0}, response from server:{1}", blueSecurity.Direct, resp);
                                Util.WriteError(errorMsg);
                                failedDirects.Add(new KeyValuePair<C5_11tblDealOriginationDirects, string>(blueSecurity, errorMsg));
                            } else {
                                string newEntry = "New DealUnderlyingDirectID:" + dealUnderlyingDirectId;
                                Util.WriteNewEntry(newEntry);
                                messageLog.AppendLine(newEntry);
                            }
                        } else {
                            Util.Log(direct.SecurityId + " is already part of this deal.");
                        }
                    }
                }
            } else {
                string warningMsg = string.Format("C5_10tblDealOrigination.Fund(which is direct): {0} doesnt have any securities (no records in C5_11tblDealOriginationDirects. C5_11tblDealOriginationDirects.Where(x => x.Direct.Equals(blueAssetInDeal.Fund)).Where(x => x.DealNo == blueAssetInDeal.DealNo) yielded NO results) ", blueAssetInDeal.Fund);
                Util.WriteWarning(warningMsg);
                messageLog.AppendLine(warningMsg);
            }
            return failedDirects;
        }

        private static int? CreateDealUnderlyingDirect(CookieCollection cookies, C5_11tblDealOriginationDirects security, C5_10tblDealOrigination direct, int dealId, int underlyingDirectId, int issuerId, int fundID, out string resp) {
            int? dealUnderlyingtDirectID = null;
            resp = string.Empty;
            DealUnderlyingDirectModel model = new DealUnderlyingDirectModel();
            // The UI asks for the following fields
            model.DealId = dealId;
            // model.DealUnderlyingDirectId = underlyingDirectId;
            model.SecurityId = underlyingDirectId;
            model.SecurityTypeId = (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity;
            // TODO: why is the required? We already have the SecurityID, and the issuer can be fetched from the security id
            model.IssuerId = issuerId;
            // TODO: why is this required? may be cos we are using the same screen for creating and updating deal. So probly FundId is required when creating
            model.FundId = fundID;
            // Number Of Shares
            model.NumberOfShares = (int)security.NShares;
            // Purchase Price
            if (security.PurchasePrice.HasValue && security.PurchasePrice.Value > 0) {
                model.PurchasePrice = (decimal)security.PurchasePrice;
            } else {
                model.PurchasePrice = 1.0m;
            }

            // FMV
            if (security.FairMarketValue.HasValue && security.FairMarketValue.Value > 0) {
                model.FMV = (decimal)security.FairMarketValue;
            } else {
                model.FMV = 1.0m;
            }

            // Tax cost basis per share
            if (security.TaxCostBasis.HasValue && security.TaxCostBasis.Value > 0) {
                model.TaxCostBase = (decimal)security.TaxCostBasis;
            } else {
                model.TaxCostBase = 1.0m;
            }
            // Tax cost date
            model.TaxCostDate = security.TaxCostDate;
            // Record Date
            model.RecordDate = direct.RecordDate;

            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/CreateDealUnderlyingDirect");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        dealUnderlyingtDirectID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return dealUnderlyingtDirectID;
        }
        #endregion

        #region Underlying Fund
        private static int? ImportUnderlyingFund(CookieCollection cookies, C5_10tblDealOrigination blueAssetInDeal, int targetDealId, int fundID, DealDetailModel deepBlueDealDetail, out string errorMsg) {
            errorMsg = string.Empty;
            int? underlyingFundId = null;
            // This is a UF
            // Find the corresponding Underlying Fund from DeepBlue
            List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = UnderlyingFundImport.GetUnderlyingFunds(cookies);
            DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName.Equals(blueAssetInDeal.Fund, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (uf == null) {
                // Try to create UF. Technically the code should not reach here, but i think when I imported the funds, the funds having an "&" were not encoded,
                // so they didnt get imported. So in case that happens, import those here
                underlyingFundId = UnderlyingFundImport.ImportUnderlyingFund(cookies, blueAssetInDeal.Fund);
                if (!underlyingFundId.HasValue) {
                    // This should not happen, as the assumption is that we have already imported all the underlying funds
                    errorMsg = "Unable to find AMB fund from DeepBlue:" + blueAssetInDeal.Fund;
                    Util.WriteError(errorMsg);
                    messageLog.AppendLine(errorMsg);
                } else {
                    uf = new UnderlyingFundListModel() { UnderlyingFundId = underlyingFundId.Value };
                }
            }

            if (uf != null) {
                DealUnderlyingFundModel existingUF = deepBlueDealDetail.DealUnderlyingFunds.Where(x => x.UnderlyingFundId == uf.UnderlyingFundId).FirstOrDefault();
                if (existingUF == null) {
                    // Find the corresponding deal from deepBlue
                    string resp = string.Empty;
                    underlyingFundId = CreateDealUnderlyingFund(cookies, blueAssetInDeal, targetDealId, uf.UnderlyingFundId, fundID, out resp);
                    if (underlyingFundId.HasValue) {
                        string newEntry = "New UF in Deal:" + underlyingFundId;
                        Util.WriteNewEntry(newEntry);
                        messageLog.AppendLine(newEntry);
                    }
                    errorMsg = resp;
                } else {
                    string msg = existingUF.UnderlyingFundId + " already exists for Deal:" + targetDealId;
                    Util.WriteWarning(msg);
                    messageLog.AppendLine(msg);
                    underlyingFundId = 0;
                }
            }
            return underlyingFundId;
        }

        private static int? CreateDealUnderlyingFund(CookieCollection cookies, C5_10tblDealOrigination blueAsset, int dealId, int underlyingFundId, int fundID, out string resp) {
            int? dealUnderlyingtFundID = null;
            resp = string.Empty;
            DealUnderlyingFundModel model = new DealUnderlyingFundModel();
            // The UI asks for the following fields
            model.DealId = dealId;
            model.UnderlyingFundId = underlyingFundId;
            model.FundId = fundID;
            if (blueAsset.FundNAV <= 0) {
                model.FundNAV = 1.0m;
            } else {
                model.FundNAV = blueAsset.FundNAV;
            }
            if (blueAsset.CapitalCommitment <= 0) {
                model.CommittedAmount = 1.0m;
            } else {
                model.CommittedAmount = blueAsset.CapitalCommitment;
            }
            model.RecordDate = blueAsset.RecordDate;
            // optional fields
            model.GrossPurchasePrice = blueAsset.GrossPurchasePrice;
            model.UnfundedAmount = blueAsset.AmountUnfunded;
            
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/CreateDealUnderlyingFund");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        dealUnderlyingtFundID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                    }
                }

            }
            return dealUnderlyingtFundID;
        }
        #endregion

        private static int? CreateNewDeal(CookieCollection cookies, C6_15tblAmberbrookDealInfo blueDeal, int fundID, int? purchaseTypeId, out string resp) {
            int? dealID = null;
            resp = string.Empty;
            // Deal/Create
            DealDetailModel model = new DealDetailModel();

            model.FundId = fundID;
            model.DealName = blueDeal.DealName;
            model.DealNumber = blueDeal.DealNo;
            if (purchaseTypeId.HasValue) {
                model.PurchaseTypeId = purchaseTypeId.Value;
            }
            model.IsPartnered = false;
            // GPP
            // NPP/
            // Total Costs
            // Syn
            // Syndicate
            // AmberBrook Contact Name
            // Seller Type DeepBlue has InvestorEntityType which has same values as SellerType. However, InvestorEntityType is on the Investor level
            // => Contact
            // Seller Name
            // Seller Company
            // Seller Phone
            // Seller Fax
            // Seller Email
            // Seller Comments
            // Comments
            // blueDeal.NAVAllocation

            // model.ContactId;
            // model.IsPartnered; 
            // model.PartnerName;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            formValues.Remove("SellerInfo");
            formValues.Remove("DealExpenses");
            formValues.Remove("DealUnderlyingFunds");
            formValues.Remove("DealUnderlyingDirects");

            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/Create");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        dealID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        response.Close();
                        readStream.Close();
                    }
                }

            }
            return dealID;
        }

        public static List<DeepBlue.Models.Deal.DealListModel> GetDeals(CookieCollection cookies, bool? isNotClose, int?fundId = null) {
            List<DeepBlue.Models.Deal.DealListModel> deals = new List<DeepBlue.Models.Deal.DealListModel>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/DealList?pageIndex=1&pageSize=5000&sortName=DealName&sortOrder=asc" + (isNotClose != null ? "&isNotClose=" + isNotClose.Value : string.Empty) + (fundId != null ? "&fundId=" + fundId.Value : string.Empty));
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
                                DeepBlue.Models.Deal.DealListModel dealListModel = new DeepBlue.Models.Deal.DealListModel();
                                dealListModel.DealId = Convert.ToInt32(row.cell[0]);
                                dealListModel.DealName = Convert.ToString(row.cell[1]);
                                dealListModel.DealNumber = Convert.ToInt32(row.cell[2]);
                                deals.Add(dealListModel);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return deals;
        }

        public static List<DeepBlue.Models.Entity.Deal> GetDealsInFund(CookieCollection cookies, int fundId) {
            // Deal/FindDeals?fundId=
            List<DeepBlue.Models.Entity.Deal> deals = new List<DeepBlue.Models.Entity.Deal>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/FindDeals?term=&fundId=" + fundId);
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
                                DeepBlue.Models.Entity.Deal deal = new Deal();
                                deal.DealID = alist.id;
                                deal.DealName = alist.value;
                                deals.Add(deal);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return deals;
        }

        public static DealDetailModel GetDeal(CookieCollection cookies, int dealId) {
            DealDetailModel model = null;
                // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Deal/FindDeal/?dealId=" + dealId);
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        string resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            model = (DealDetailModel)js.Deserialize(resp, typeof(DealDetailModel));
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return model;
        }

        private static int? GetDealContact(CookieCollection cookies, string contactName) {
            // Admin/FindDealContacts?term=
            return null;
        }

        private static void LogMessages() {
            using (TextWriter tw = new StreamWriter(Globals.MessageFile, true)) {
                tw.WriteLine(Environment.NewLine +messageLog.ToString());
                tw.Flush();
                tw.Close();
            }
        }

        private static void LogErrors(List<KeyValuePair<C6_15tblAmberbrookDealInfo, Exception>> errors) {
            using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
                tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
                foreach (KeyValuePair<C6_15tblAmberbrookDealInfo, Exception> kv in errors) {
                    try {
                        tw.WriteLine(Environment.NewLine +kv.Key.DealName + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
                    } catch (Exception ex) {
                        Util.Log("Error logging exception: " + ex.Message);
                    }
                }
                tw.Flush();
                tw.Close();
            }
        }
    }

    //// Imports UnderlyingFunds and Directs in the deal
    //class DealUnderlyingFundImport {
    //    private static Hashtable StateAbbr = new Hashtable();
    //    public static List<KeyValuePair<C4_20tblStockTable, Exception>> Errors = new List<KeyValuePair<C4_20tblStockTable, Exception>>();
    //    public static List<KeyValuePair<Equity, Exception>> ImportErrors = new List<KeyValuePair<Equity, Exception>>();

    //    public static int TotalConversionRecords = 0;
    //    public static int RecordsConvertedSuccessfully = 0;

    //    public static int TotalImportRecords = 0;
    //    public static int RecordsImportedSuccessfully = 0;

    //    public static void ImportUnderlyingFunds2(CookieCollection cookies) {
    //        Errors = new List<KeyValuePair<C4_20tblStockTable, Exception>>();
    //        TotalConversionRecords = 0;
    //        RecordsConvertedSuccessfully = 0;
    //        Util.Log("Fetching Directs from Blue............");
    //        using (BlueEntities context = new BlueEntities()) {
    //            List<C6_15tblAmberbrookDealInfo> blueDeals = context.C6_15tblAmberbrookDealInfo.ToList();
    //            // Get all the deauls in the system
    //            List<DealListModel> deepBlueDeals = DealImport.GetDeals(cookies, null);
    //            // Get all the equities and FIs in the system
    //            List<AutoCompleteListExtend> equitiesAndFIs = DirectsImport.GetDirects(cookies);

    //            foreach (C6_15tblAmberbrookDealInfo blueDeal in blueDeals) {
    //                try {

    //                    // Find which Amberbrook fund this Deal belongs to
    //                    C6_10AmberbrookFundInfo blueAmbFund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(blueDeal.AmberbrookFundNo)).FirstOrDefault();
    //                    if (blueAmbFund != null) {
    //                        // Find the UF/Directs in the deal
    //                        List<C5_10tblDealOrigination> blueAssetsInDeal = context.C5_10tblDealOrigination.Where(x => x.AmberbrookFundNo == blueDeal.AmberbrookFundNo).Where(x => x.DealNo == blueDeal.DealNo).ToList();
    //                        foreach (C5_10tblDealOrigination blueAssetInDeal in blueAssetsInDeal) {
    //                            // This can either be a direct or a UF
    //                            string assetName = blueAssetInDeal.Fund;
    //                            C7_10tblGPPaymentInstructions blueAsset = context.C7_10tblGPPaymentInstructions.Where(x => x.Fund.Equals(assetName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                            // Find the deal with the same DealNumber and DealName, and that belongs to the same Amberbrook fund
    //                            DealListModel deepBlueDeal = deepBlueDeals.Where(x => x.DealName.Equals(blueDeal.DealName, StringComparison.OrdinalIgnoreCase)).Where(x => x.FundName.Equals(blueAmbFund.AmberbrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                            ////////////
    //                            if (blueAsset.FundType.Equals("Direct", StringComparison.OrdinalIgnoreCase)) {
    //                                // This is a direct
    //                                //string issuerName = blueAsset.Fund;
    //                                List<C5_11tblDealOriginationDirects> blueSecurities = context.C5_11tblDealOriginationDirects.Where(x => x.Direct.Equals(blueAssetInDeal.Fund, StringComparison.OrdinalIgnoreCase)).Where(x => x.DealNo == blueAssetInDeal.DealNo).ToList();
    //                                foreach (C5_11tblDealOriginationDirects blueSecurity in blueSecurities) {
    //                                    // Equity from blue
    //                                    C4_20tblStockTable blueStock = context.C4_20tblStockTable.Where(x => x.StockSymbol.Equals(blueSecurity.StockSymbol, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                                    // Find the direct corresponding to the security
    //                                    AutoCompleteListExtend equity = equitiesAndFIs.Where(x => x.otherid == (int)DeepBlue.Models.Deal.Enums.SecurityType.Equity).Where(x => x.value.Equals(blueStock.Company, StringComparison.OrdinalIgnoreCase)).Where(x => x.label.Equals(string.Format("{0}>>Equity>>(1)", blueStock.Company, blueStock.StockSymbol), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                                    // Create a DealUnderlyingDirect record
    //                                    string resp = string.Empty;
    //                                    int? dealUnderlyingDirectId = CreateDealUnderlyingDirect(cookies, blueSecurity, blueAssetInDeal, deepBlueDeal.DealId, equity.otherid2, out resp);
    //                                }
    //                            } else {
    //                                // This is a UF
    //                                // Find the corresponding Underlying Fund from DeepBlue
    //                                List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = UnderlyingFundImport.GetUnderlyingFunds(cookies);
    //                                DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName.Equals(blueAsset.Fund, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                                if (uf != null) {
    //                                    // Find the corresponding deal from deepBlue
    //                                    DealListModel deepBlueDeal = deepBlueDeals.Where(x => x.DealName.Equals(blueDeal.DealName, StringComparison.OrdinalIgnoreCase)).Where(x => x.FundName.Equals(blueAmberbrookFund.AmberbrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                                    // We are assuming that all the deals have already been imported using the DealImport class
    //                                    if (deepBlueDeal != null) {
    //                                        string resp = string.Empty;
    //                                        CreateDealUnderlyingFund(cookies, blueAssetInDeal, deepBlueDeal.DealId, uf.UnderlyingFundId, out resp);
    //                                    } else {
    //                                        // Log Error
    //                                    }
    //                                } else {
    //                                    // This should not happen, as the assumption is that we have already imported all the underlying funds
    //                                }
    //                            }
    //                            ////////////
    //                        }
    //                    } else {
    //                        // Log Error
    //                    }

    //                } catch {
    //                    // Log the error
    //                }
    //            }
    //        }
    //    }

    //    public static void ImportUnderlyingFunds(CookieCollection cookies) {
    //        Errors = new List<KeyValuePair<C4_20tblStockTable, Exception>>();
    //        TotalConversionRecords = 0;
    //        RecordsConvertedSuccessfully = 0;
    //        Util.Log("Fetching Directs from Blue............");
    //        using (BlueEntities context = new BlueEntities()) {
    //            List<C5_10tblDealOrigination> assetsInDeals = context.C5_10tblDealOrigination.ToList();
    //            foreach (C5_10tblDealOrigination assetInDeal in assetsInDeals) {
    //                try {
    //                    // This can either be a direct or a UF
    //                    string assetName = assetInDeal.Fund;
    //                    C7_10tblGPPaymentInstructions blueAsset = context.C7_10tblGPPaymentInstructions.Where(x => x.Fund.Equals(assetName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

    //                    C6_15tblAmberbrookDealInfo blueDeal = context.C6_15tblAmberbrookDealInfo.Where(x => x.AmberbrookFundNo.Equals(assetInDeal.AmberbrookFundNo, StringComparison.OrdinalIgnoreCase)).Where(x => x.DealNo == assetInDeal.DealNo).FirstOrDefault();
    //                    C6_10AmberbrookFundInfo blueAmberbrookFund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(blueDeal.AmberbrookFundNo, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

    //                    // Find the corresponding deal from DeepBlue
    //                    List<DealListModel> deepBlueDeals = DealImport.GetDeals(cookies, null);

    //                    if (blueAsset.FundType.Equals("Direct", StringComparison.OrdinalIgnoreCase)) {
    //                        // This is a direct
    //                        string issuerName = blueAsset.Fund;

    //                    } else {
    //                        // This is a UF

    //                        // Find the corresponding Underlying Fund from DeepBlue
    //                        List<DeepBlue.Models.Deal.UnderlyingFundListModel> underlyingFunds = UnderlyingFundImport.GetUnderlyingFunds(cookies);
    //                        DeepBlue.Models.Deal.UnderlyingFundListModel uf = underlyingFunds.Where(x => x.FundName.Equals(blueAsset.Fund, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                        if (uf != null) {
    //                            // Find the corresponding deal from deepBlue
    //                            DealListModel deepBlueDeal = deepBlueDeals.Where(x => x.DealName.Equals(blueDeal.DealName, StringComparison.OrdinalIgnoreCase)).Where(x => x.FundName.Equals(blueAmberbrookFund.AmberbrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                            string resp = string.Empty;
    //                            CreateDealUnderlyingFund(cookies, assetInDeal, deepBlueDeal.DealId, uf.UnderlyingFundId, out resp);
    //                        } else {
    //                            // This should not happen, as the assumption is that we have already imported all the underlying funds
    //                        }
    //                    }

    //                    C6_10AmberbrookFundInfo amberBrookFund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(amberBrookFundNumber)).FirstOrDefault();
    //                    if (amberBrookFund != null) {
    //                        string amberBrookFundName = amberBrookFund.AmberbrookFundName;
    //                        // try to find the amber brook fund from DeepBlue
    //                        List<DeepBlue.Models.Entity.Fund> ufs = FundImport.GetFunds(cookies);
    //                        DeepBlue.Models.Entity.Fund fund = ufs.Where(x => x.FundName.Equals(amberBrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

    //                        //List<DeepBlue.Models.Deal.UnderlyingFundListModel> ufs = UnderlyingFundImport.GetUnderlyingFunds(cookies);
    //                        //UnderlyingFundListModel ufListModel = ufs.Where(x => x.FundName.Equals(amberBrookFundName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    //                        if (fund != null) {
    //                            // Find a deal.. Add to the deal if it already exists, create new deal otherwise
    //                            List<DeepBlue.Models.Deal.DealListModel> deals = DealImport.GetDeals(cookies, null);
    //                            DeepBlue.Models.Deal.DealListModel deal = deals.Where(x => x.DealName.Equals(blueDeal.DealName)).FirstOrDefault();
    //                            if (deal != null) {
    //                                // We found the deal..make sure that this deal is for the same fund.. if not, then we need to create a new deal
    //                                C6_10AmberbrookFundInfo blueFund = context.C6_10AmberbrookFundInfo.Where(x => x.AmberbrookFundNo.Equals(blueDeal.AmberbrookFundNo)).FirstOrDefault();
    //                                if (!deal.FundName.Equals(blueFund.AmberbrookFundName)) {
    //                                    deal = null;
    //                                }
    //                            }
    //                            if (deal == null) {
    //                                // Create a new deal
    //                                int purchaseType = Globals.PurchaseTypes.Where(x => x.Name.Equals(blueDeal.PurchaseType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().PurchaseTypeID;
    //                                string resp = string.Empty;
    //                                int? newDeal = CreateNewDeal(cookies, blueDeal, fund.FundID, purchaseType, out resp);
    //                            }

    //                        } else {
    //                            // Log this.. Make sure the underlying fund is already created in DeepBlue

    //                        }
    //                    } else {
    //                        // Log it
    //                    }
    //                } catch (Exception ex) {
    //                    Errors.Add(new KeyValuePair<C4_20tblStockTable, Exception>(equity, ex));
    //                    Util.Log("ConvertBlueToDeepBlue() " + ex);
    //                }
    //            }
    //        }
    //        Util.Log(string.Format("End fetching records from blue. Total Records: {0}, Records Converted successfully: {1}", TotalConversionRecords, RecordsConvertedSuccessfully));
    //    }

    //    private static int? CreateDealUnderlyingFund(CookieCollection cookies, C5_10tblDealOrigination blueAsset, int dealId, int underlyingFundId, out string resp) {
    //        int? dealUnderlyingtFundID = null;
    //        resp = string.Empty;
    //        DealUnderlyingFundModel model = new DealUnderlyingFundModel();
    //        // The UI asks for the following fields
    //        model.DealId = dealId;
    //        model.UnderlyingFundId = underlyingFundId;
    //        model.GrossPurchasePrice = blueAsset.GrossPurchasePrice;
    //        model.FundNAV = blueAsset.FundNAV;
    //        model.CommittedAmount = blueAsset.CapitalCommitment;
    //        model.UnfundedAmount = blueAsset.AmountUnfunded;
    //        model.RecordDate = blueAsset.RecordDate;

    //        NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

    //        // Send the request 
    //        string url = HttpWebRequestUtil.GetUrl("Deal/CreateDealUnderlyingFund");
    //        byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
    //        HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
    //        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
    //            using (Stream receiveStream = response.GetResponseStream()) {
    //                // Pipes the stream to a higher level stream reader with the required encoding format. 
    //                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
    //                    resp = readStream.ReadToEnd();
    //                    dealUnderlyingtFundID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
    //                    response.Close();
    //                    readStream.Close();
    //                }
    //            }

    //        }
    //        return dealUnderlyingtFundID;
    //    }

    //    private static int? CreateDealUnderlyingDirect(CookieCollection cookies, C5_11tblDealOriginationDirects security, C5_10tblDealOrigination direct, int dealId, int underlyingDirectId, out string resp) {
    //        int? dealUnderlyingtDirectID = null;
    //        resp = string.Empty;
    //        DealUnderlyingDirectModel model = new DealUnderlyingDirectModel();
    //        // The UI asks for the following fields
    //        model.DealId = dealId;
    //        model.DealUnderlyingDirectId = underlyingDirectId;
    //        // Number Of Shares
    //        model.NumberOfShares = (int)security.NShares;
    //        // Purchase Price
    //        model.PurchasePrice = (decimal)security.PurchasePrice;
    //        // FMV
    //        model.FMV = (decimal)security.FairMarketValue;
    //        // Tax cost basis per share
    //        model.TaxCostBase = (decimal)security.TaxCostBasis;
    //        // Tax cost date
    //        model.TaxCostDate = security.TaxCostDate;
    //        // Record Date
    //        model.RecordDate = direct.RecordDate;

    //        NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);

    //        // Send the request 
    //        string url = HttpWebRequestUtil.GetUrl("Deal/CreateDealUnderlyingDirect");
    //        byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
    //        HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
    //        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
    //            using (Stream receiveStream = response.GetResponseStream()) {
    //                // Pipes the stream to a higher level stream reader with the required encoding format. 
    //                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
    //                    resp = readStream.ReadToEnd();
    //                    dealUnderlyingtDirectID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
    //                    response.Close();
    //                    readStream.Close();
    //                }
    //            }

    //        }
    //        return dealUnderlyingtDirectID;
    //    }

    //    public static List<DeepBlue.Models.Deal.DealListModel> GetDeals(CookieCollection cookies, bool? isNotClose) {
    //        List<DeepBlue.Models.Deal.DealListModel> deals = new List<DeepBlue.Models.Deal.DealListModel>();
    //        // Send the request 
    //        string url = HttpWebRequestUtil.GetUrl("Deal/DealList?pageIndex=1&pageSize=5000&sortName=DealName&sortOrder=asc" + (isNotClose != null ? "&isNotClose=" + isNotClose.Value : string.Empty));
    //        HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, null, false, cookies, false, HttpWebRequestUtil.JsonContentType);
    //        if (response.StatusCode == System.Net.HttpStatusCode.OK) {
    //            using (Stream receiveStream = response.GetResponseStream()) {
    //                // Pipes the stream to a higher level stream reader with the required encoding format. 
    //                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
    //                    string resp = readStream.ReadToEnd();
    //                    if (!string.IsNullOrEmpty(resp)) {
    //                        JavaScriptSerializer js = new JavaScriptSerializer();
    //                        FlexigridData flexiGrid = (FlexigridData)js.Deserialize(resp, typeof(FlexigridData));
    //                        foreach (Helpers.FlexigridRow row in flexiGrid.rows) {
    //                            DeepBlue.Models.Deal.DealListModel dealListModel = new DeepBlue.Models.Deal.DealListModel();
    //                            dealListModel.DealId = Convert.ToInt32(row.cell[0]);
    //                            dealListModel.DealName = Convert.ToString(row.cell[1]);
    //                            dealListModel.FundName = Convert.ToString(row.cell[2]);
    //                            deals.Add(dealListModel);
    //                        }
    //                    } else {
    //                    }
    //                    response.Close();
    //                    readStream.Close();
    //                }
    //            }
    //        }
    //        return deals;
    //    }

    //    private static int? GetDealContact(CookieCollection cookies, string contactName) {
    //        // Admin/FindDealContacts?term=
    //        return null;
    //    }

    //    private static void LogErrors(List<KeyValuePair<C4_20tblStockTable, Exception>> errors) {
    //        using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
    //            tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Converted:{1}, Failed Conversion:{2}", TotalConversionRecords, RecordsConvertedSuccessfully, Errors.Count));

    //            foreach (KeyValuePair<C4_20tblStockTable, Exception> kv in errors) {
    //                try {
    //                    tw.WriteLine(Environment.NewLine +kv.Key.Company + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
    //                } catch (Exception ex) {
    //                    Util.Log("Error logging exception: " + ex.Message);
    //                }
    //            }
    //            tw.Flush();
    //            tw.Close();
    //        }
    //    }

    //    private static void LogErrors(List<KeyValuePair<Equity, Exception>> errors) {
    //        using (TextWriter tw = new StreamWriter(Globals.LogFile, true)) {
    //            tw.WriteLine(Environment.NewLine +string.Format("Total Records:{0}, Records Successfully Imported:{1}, Failed Import:{2}", TotalImportRecords, RecordsImportedSuccessfully, ImportErrors.Count));
    //            foreach (KeyValuePair<Equity, Exception> kv in errors) {
    //                try {
    //                    tw.WriteLine(Environment.NewLine +kv.Key.Symbol + ":" + kv.Value.Message + " Inner exception:" + (kv.Value.InnerException != null ? kv.Value.InnerException.Message : string.Empty) + " StackTrace: " + (kv.Value.StackTrace != null ? kv.Value.StackTrace : string.Empty));
    //                } catch (Exception ex) {
    //                    Util.Log("Error logging exception: " + ex.Message);
    //                }
    //            }
    //            tw.Flush();
    //            tw.Close();
    //        }
    //    }
    //}

    /// <summary>
    /// Distinct 4-20tblStockTable.Company => Issuer (in DeepBlue)
    /// </summary>

    public class PurchaseTypeImport {
        public static void SyncPurchaseTypes(CookieCollection cookies) {
            List<string> bluePurchaseTypes = GetPurchaseTypeFromBlue();
            List<DeepBlue.Models.Entity.PurchaseType> deepbluePurchaseTypes = GetPurchaseTypesFromDeepBlue(cookies);

            Util.Log("Issuer import. Total Records to be imported: " + bluePurchaseTypes.Count);
            int totalFailures = 0;

            foreach (string bluePurchaseType in bluePurchaseTypes) {
                DeepBlue.Models.Entity.PurchaseType deepBluePurchaseType = deepbluePurchaseTypes.Where(x => x.Name == bluePurchaseType).FirstOrDefault();
                if (deepBluePurchaseType == null) {
                    // Add the new issuer
                    string resp = string.Empty;
                    int? issuer = CreatePurchaseType(cookies, bluePurchaseType, out resp);
                    if (!issuer.HasValue) {
                        totalFailures++;
                    }
                }
            }
            Util.Log("Total import failures: " + totalFailures);
        }

        public static List<string> GetPurchaseTypeFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C6_15tblAmberbrookDealInfo.Select(x => x.PurchaseType).Distinct().ToList();
            }
        }

        public static List<DeepBlue.Models.Entity.PurchaseType> GetPurchaseTypesFromDeepBlue(CookieCollection cookies) {
            // 
            List<DeepBlue.Models.Entity.PurchaseType> purchaseTypes = new List<DeepBlue.Models.Entity.PurchaseType>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/PurchaseTypeList?pageIndex=1&pageSize=5000&sortName=Name&sortOrder=asc");
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
                                DeepBlue.Models.Entity.PurchaseType purchaseType = new DeepBlue.Models.Entity.PurchaseType();
                                purchaseType.PurchaseTypeID = Convert.ToInt32(row.cell[0]);
                                purchaseType.Name = Convert.ToString(row.cell[1]);
                                purchaseTypes.Add(purchaseType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return purchaseTypes;
        }

        // /Admin/UpdatePurchaseType
        private static int? CreatePurchaseType(CookieCollection cookies, string purchaseType, out string resp) {
            resp = string.Empty;
            int? purchaseTypeId = null;
            DeepBlue.Models.Admin.EditPurchaseTypeModel model = new DeepBlue.Models.Admin.EditPurchaseTypeModel();
            model.Name = purchaseType;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdatePurchaseType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            purchaseTypeId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return purchaseTypeId;
        }
    }

    public class DealClosingCostTypeImport {
        public static void SyncDealClosingCostTypes(CookieCollection cookies) {
            List<string> blueDealClosingCostTypes = GetDealClosingCostTypesFromBlue();
            List<DeepBlue.Models.Entity.DealClosingCostType> deepblueDealClosingCostTypes = GetDealClosingCostTypesFromDeepBlue(cookies);

            Util.Log("DealClosingCostTypes import. Total Records to be imported: " + blueDealClosingCostTypes.Count);
            int totalFailures = 0;
            // Create a Legal Fees closing cost type
            blueDealClosingCostTypes.Insert(0, Globals.LegalFee);
            bool first = true;
            foreach (string blueDealClosingCostType in blueDealClosingCostTypes) {
                if (first || !blueDealClosingCostType.ToLower().Contains(Globals.LegalFee.ToLower())) {
                    DeepBlue.Models.Entity.DealClosingCostType deepBlueDealClosingCostType = deepblueDealClosingCostTypes.Where(x => x.Name == blueDealClosingCostType).FirstOrDefault();
                    if (deepBlueDealClosingCostType == null) {
                        // Add the new issuer
                        string resp = string.Empty;
                         string closingCostType = blueDealClosingCostType.Length > 50 ? blueDealClosingCostType.Substring(0, 50) : blueDealClosingCostType;
                         int? issuer = CreateDealClosingCostType(cookies, closingCostType, out resp);
                         if (!issuer.HasValue) {
                             totalFailures++;
                         }
                    }
                }
                first = false;
            }
            Util.Log("Total import failures: " + totalFailures);
        }

        public static List<string> GetDealClosingCostTypesFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C6_40tblDealExpenses.Select(x => x.Description).Distinct().ToList();
            }
        }

        public static List<DeepBlue.Models.Entity.DealClosingCostType> GetDealClosingCostTypesFromDeepBlue(CookieCollection cookies) {
            // Admin/DealClosingCostTypeList?pageIndex=1&pageSize=5000&sortName=Name&sortOrder=asc
            List<DeepBlue.Models.Entity.DealClosingCostType> dealClosingCostTypes = new List<DeepBlue.Models.Entity.DealClosingCostType>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/DealClosingCostTypeList?pageIndex=1&pageSize=5000&sortName=Name&sortOrder=asc");
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
                                DeepBlue.Models.Entity.DealClosingCostType dealClosingType = new DeepBlue.Models.Entity.DealClosingCostType();
                                dealClosingType.DealClosingCostTypeID = Convert.ToInt32(row.cell[0]);
                                dealClosingType.Name = Convert.ToString(row.cell[1]);
                                dealClosingCostTypes.Add(dealClosingType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return dealClosingCostTypes;
        }

        // /Admin/UpdatePurchaseType
        private static int? CreateDealClosingCostType(CookieCollection cookies, string dealClosingCostType, out string resp) {
            resp = string.Empty;
            int? dealClosingCostTypeID = null;
            DeepBlue.Models.Admin.EditDealClosingCostTypeModel model = new DeepBlue.Models.Admin.EditDealClosingCostTypeModel();
            model.Name = dealClosingCostType;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateDealClosingCostType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            dealClosingCostTypeID = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return dealClosingCostTypeID;
        }
    }

    // /Admin/UpdateSellerType
    public class SellerTypeImport {
        public static void SynSellerTypes(CookieCollection cookies) {
            List<string> blueSellerTypes = GetSellerTypesFromBlue();
            List<DeepBlue.Models.Entity.SellerType> deepblueSellerTypes = GetSellerTypesFromDeepBlue(cookies);

            Util.Log("SellerTypes import. Total Records to be imported: " + blueSellerTypes.Count);
            int totalFailures = 0;

            foreach (string blueSellerType in blueSellerTypes) {
                DeepBlue.Models.Entity.SellerType deepBlueSellerType = deepblueSellerTypes.Where(x => x.SellerType1 == blueSellerType).FirstOrDefault();
                if (deepBlueSellerType == null) {
                    // Add the new issuer
                    string resp = string.Empty;
                    int? issuer = CreateSellerType(cookies, blueSellerType, out resp);
                    if (!issuer.HasValue) {
                        totalFailures++;
                    }
                }
            }
            Util.Log("Total import failures: " + totalFailures);
        }

        public static List<string> GetSellerTypesFromBlue() {
            using (BlueEntities context = new BlueEntities()) {
                return context.C8_60tblSellerType.Select(x => x.SellerType).Distinct().ToList();
            }
        }

       public static List<DeepBlue.Models.Entity.SellerType> GetSellerTypesFromDeepBlue(CookieCollection cookies) {
            // Admin/DealClosingCostTypeList?pageIndex=1&pageSize=5000&sortName=Name&sortOrder=asc
           List<DeepBlue.Models.Entity.SellerType> sellerTypes = new List<DeepBlue.Models.Entity.SellerType>();
            // Send the request 
            string url = HttpWebRequestUtil.GetUrl("Admin/SellerTypeList?pageIndex=1&pageSize=5000&sortName=SellerType1&sortOrder=asc");
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
                                DeepBlue.Models.Entity.SellerType sellerType = new DeepBlue.Models.Entity.SellerType();
                                sellerType.SellerTypeID = Convert.ToInt32(row.cell[0]);
                                sellerType.SellerType1 = Convert.ToString(row.cell[1]);
                                sellerTypes.Add(sellerType);
                            }
                        } else {
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return sellerTypes;
        }

        // /Admin/UpdatePurchaseType
        private static int? CreateSellerType(CookieCollection cookies, string sellerType, out string resp) {
            resp = string.Empty;
            int? sellerTypeId = null;
            DeepBlue.Models.Admin.EditSellerTypeModel model = new DeepBlue.Models.Admin.EditSellerTypeModel();
            model.SellerType= sellerType;
            NameValueCollection formValues = HttpWebRequestUtil.SetUpForm(model, string.Empty, string.Empty);
            string url = HttpWebRequestUtil.GetUrl("Admin/UpdateSellerType");
            byte[] postData = System.Text.Encoding.ASCII.GetBytes(HttpWebRequestUtil.ToFormValue(formValues));
            HttpWebResponse response = HttpWebRequestUtil.SendRequest(url, postData, true, cookies);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                using (Stream receiveStream = response.GetResponseStream()) {
                    // Pipes the stream to a higher level stream reader with the required encoding format. 
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
                        resp = readStream.ReadToEnd();
                        if (!string.IsNullOrEmpty(resp)) {
                            sellerTypeId = HttpWebRequestUtil.GetNewKeyFromResponse(resp);
                        }
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return sellerTypeId;
        }
    }
}
