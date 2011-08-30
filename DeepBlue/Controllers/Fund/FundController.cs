using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Fund;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Admin;
using System.Text;
using DeepBlue.Models.Admin.Enums;
using DeepBlue.Models.Fund.Enums;

namespace DeepBlue.Controllers.Fund {
	public class FundController : BaseController {

		public IFundRepository FundRepository { get; set; }

		public IAdminRepository AdminRepository { get; set; }

		public FundController()
			: this(new FundRepository(), new AdminRepository()) {
		}

		public FundController(IFundRepository fundRepository, IAdminRepository adminRepository) {
			FundRepository = fundRepository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Fund/
		public ActionResult Index() {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["PageName"] = "Fund Setup";
			CreateModel model = FindFundDetail(0);
			model.MultiplierTypes = SelectListFactory.GetMultiplierTypeList(FundRepository.GetAllMultiplierTypes());
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
			return View(model);
		}

		//
		// GET: /Fund/List
		[HttpGet]
		public ActionResult List(int pageIndex, int pageSize, string sortName, string sortOrder) {
			FlexigridData flexgridData = new FlexigridData();
			int totalRows = 0;
			List<FundListModel> funds = FundRepository.GetAllFunds(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			flexgridData.total = totalRows;
			flexgridData.page = pageIndex;
			foreach (var fund in funds) {
				flexgridData.rows.Add(new FlexigridRow {
					cell = new List<object> {
							fund.FundId,
							fund.FundName,
							fund.TaxId,
							fund.CommitmentAmount,
							fund.UnfundedAmount,
							(fund.InceptionDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"),
							(fund.InceptionDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"),
					}
				});
			}
			return Json(flexgridData, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Fund/List
		[HttpGet]
		public JsonResult GetAllFunds(int pageIndex, int pageSize) {
			return Json(FundRepository.GetAllFunds(pageIndex, pageSize), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Fund/FindFunds
		[HttpGet]
		public JsonResult FindFunds(string term) {
			return Json(FundRepository.FindFunds(term), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Fund/FindDealFunds
		[HttpGet]
		public JsonResult FindDealFunds(int underlyingFundId, string term) {
			if (underlyingFundId > 0)
				return Json(FundRepository.FindDealFunds(underlyingFundId, term), JsonRequestBehavior.AllowGet);
			else
				return FindFunds(term);
		}

		//
		// GET: /Fund/FindFund/1
		public JsonResult FindFund(int id) {
			return Json(FindFundDetail(id), JsonRequestBehavior.AllowGet);
		}

		private CreateModel FindFundDetail(int fundId) {
			CreateModel fundDetail = FundRepository.FindFundDetail(fundId);
			int index = 0;
			int total = 0;
			if (fundDetail == null) {
				fundDetail = new CreateModel();
				fundDetail.FundRateSchedules = new List<FundRateScheduleDetail>();
			}
			List<FundRateScheduleDetail> fundRateSchdules = (List<FundRateScheduleDetail>)fundDetail.FundRateSchedules;
			if (fundRateSchdules.Count == 0) {
				fundRateSchdules.Add(new FundRateScheduleDetail {
					FundId = fundId,
					InvestorTypeId = 0,
					RateScheduleId = 0,
					FundRateScheduleId = 0,
					RateScheduleTypeId = 0
				});
			}
			if (fundDetail.BankDetail == null)
				fundDetail.BankDetail = new List<FundBankDetail>();

			List<FundBankDetail> fundBankDetail;
			fundBankDetail = (List<FundBankDetail>)fundDetail.BankDetail;
			if (fundBankDetail.Count() == 0)
				fundBankDetail.Add(new FundBankDetail());

			/* Load Custom Fields */
			fundDetail.CustomField = new CustomFieldModel();
			fundDetail.CustomField.IsDisplayTwoColumn = true;
			IList<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(fundId);
			fundDetail.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund);
			fundDetail.CustomField.Values = new List<CustomFieldValueDetail>();
			foreach (var field in fundDetail.CustomField.Fields) {
				var value = customFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == field.CustomFieldId);
				if (value != null) {
					fundDetail.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = value.CustomFieldID,
						CustomFieldValueId = value.CustomFieldID,
						DataTypeId = value.CustomField.DataTypeID,
						BooleanValue = value.BooleanValue ?? false,
						CurrencyValue = value.CurrencyValue ?? 0,
						DateValue = ((value.DateValue ?? Convert.ToDateTime("01/01/1900")).Year == 1900 ? string.Empty : (value.DateValue ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")),
						IntegerValue = value.IntegerValue ?? 0,
						TextValue = value.TextValue,
						Key = fundId
					});
				}
				else {
					fundDetail.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = field.CustomFieldId,
						CustomFieldValueId = 0,
						DataTypeId = field.DataTypeId,
						BooleanValue = false,
						CurrencyValue = 0,
						DateValue = string.Empty,
						IntegerValue = 0,
						TextValue = string.Empty,
						Key = fundId
					});
				}
			}
			/* Load Fund Rate Schedule */
			foreach (var rateSchedule in fundDetail.FundRateSchedules) {
				ManagementFeeRateSchedule feeRateSchedule = FundRepository.FindManagementFeeRateSchedule(rateSchedule.RateScheduleId);
				if (feeRateSchedule != null) {
					foreach (var tier in feeRateSchedule.ManagementFeeRateScheduleTiers) {
						rateSchedule.FundRateScheduleTiers.Add(new FundRateScheduleTierDetail {
							Notes = tier.Notes,
							ManagementFeeRateScheduleId = tier.ManagementFeeRateScheduleID,
							ManagementFeeRateScheduleTierId = tier.ManagementFeeRateScheduleTierID,
							StartDate = tier.StartDate,
							EndDate = tier.EndDate,
							Rate = (tier.MultiplierTypeID == (int)MutiplierType.CapitalCommitted ? tier.Multiplier : 0),
							FlatFee = (tier.MultiplierTypeID == (int)MutiplierType.FlatFee ? tier.Multiplier : 0),
							MultiplierTypeId = tier.MultiplierTypeID
						});
					}
				}
				index = 0;
				total = (8 - rateSchedule.FundRateScheduleTiers.Count);
				for (index = 0; index < total; index++) {
					rateSchedule.FundRateScheduleTiers.Add(new FundRateScheduleTierDetail());
				}
			}
			return fundDetail;
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateModel model = new CreateModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo;
			ManagementFeeRateSchedule managementFeeRateSchedule = null;
			ManagementFeeRateScheduleTier rateTier = null;
			ResultModel resultModel = new ResultModel();
			string ErrorMessage = FundNameAvailable(model.FundName, model.FundId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("FundName", ErrorMessage);
			}
			ErrorMessage = TaxIdAvailable(model.TaxId, model.FundId);
			if (String.IsNullOrEmpty(ErrorMessage) == false) {
				ModelState.AddModelError("TaxId", ErrorMessage);
			}
			FundBankDetail bankDetailModel = new FundBankDetail();
			this.TryUpdateModel(bankDetailModel, collection);
			errorInfo = ValidationHelper.Validate(bankDetailModel);
			if (errorInfo.Any()) {
				foreach (var err in errorInfo) {
					ModelState.AddModelError(err.PropertyName, err.ErrorMessage);
				}
			}
			else {
				if ((bankDetailModel.ABANumber ?? 0) > 0 ||
				   string.IsNullOrEmpty(bankDetailModel.AccountNumberCash) == false ||
					string.IsNullOrEmpty(bankDetailModel.AccountOf) == false ||
					string.IsNullOrEmpty(bankDetailModel.Attention) == false ||
					string.IsNullOrEmpty(bankDetailModel.BankName) == false ||
					string.IsNullOrEmpty(bankDetailModel.Fax) == false ||
					string.IsNullOrEmpty(bankDetailModel.FFCNumber) == false ||
					string.IsNullOrEmpty(bankDetailModel.IBAN) == false ||
					string.IsNullOrEmpty(bankDetailModel.Reference) == false ||
					string.IsNullOrEmpty(bankDetailModel.Swift) == false ||
					string.IsNullOrEmpty(bankDetailModel.Telephone) == false
					) {
					if (string.IsNullOrEmpty(bankDetailModel.AccountNo)) {
						ModelState.AddModelError("AccountNo", "Account No is required");
					}
				}
			}
			if (ModelState.IsValid) {
				Models.Entity.Fund fund;
				if (model.FundId > 0) {
					fund = FundRepository.FindFund(model.FundId);
				}
				else {
					fund = new Models.Entity.Fund();
				}
				fund.Carry = model.Carry;
				fund.DateClawbackTriggered = model.DateClawbackTriggered;
				fund.EntityID = Authentication.CurrentEntity.EntityID;
				fund.FinalTerminationDate = model.FinalTerminationDate;
				fund.FundName = model.FundName;
				fund.InceptionDate = model.InceptionDate;
				fund.MgmtFeesCatchUpDate = model.MgmtFeesCatchUpDate;
				fund.NumofAutoExtensions = model.NumofAutoExtensions;
				fund.RecycleProvision = model.RecycleProvision;
				fund.ScheduleTerminationDate = model.ScheduleTerminationDate;
				fund.TaxID = model.TaxId;

				if (string.IsNullOrEmpty(bankDetailModel.AccountNo) == false) {
					//Add bank account
					FundAccount fundAccount;
					if (bankDetailModel.AccountId > 0) {
						fundAccount = fund.FundAccounts.SingleOrDefault(account => account.FundAccountID == bankDetailModel.AccountId);
					}
					else {
						fundAccount = new FundAccount();
						fund.FundAccounts.Add(fundAccount);
					}
					fundAccount.Account = bankDetailModel.AccountNo;
					fundAccount.AccountNumberCash = bankDetailModel.AccountNumberCash ?? string.Empty;
					fundAccount.AccountOf = bankDetailModel.AccountOf ?? string.Empty;
					fundAccount.Attention = bankDetailModel.Attention ?? string.Empty;
					fundAccount.BankName = bankDetailModel.BankName ?? string.Empty;
					fundAccount.CreatedBy = Authentication.CurrentUser.UserID;
					fundAccount.CreatedDate = DateTime.Now;
					fundAccount.EntityID = Authentication.CurrentEntity.EntityID;
					fundAccount.Fax = bankDetailModel.Fax ?? string.Empty;
					fundAccount.FFCNumber = bankDetailModel.FFCNumber ?? string.Empty;
					fundAccount.IBAN = bankDetailModel.IBAN ?? string.Empty;
					fundAccount.IsPrimary = false;
					fundAccount.LastUpdatedBy = Authentication.CurrentUser.UserID;
					fundAccount.LastUpdatedDate = DateTime.Now;
					fundAccount.Phone = bankDetailModel.Telephone ?? string.Empty;
					fundAccount.Reference = bankDetailModel.Reference ?? string.Empty;
					fundAccount.Routing = bankDetailModel.ABANumber;
					fundAccount.SWIFT = bankDetailModel.Swift ?? string.Empty;
				}

				/* Add new fund rate schedule */
				int FundRateSchedulesCount = DataTypeHelper.ToInt32(collection["FundRateSchedulesCount"]);
				if (FundRateSchedulesCount > 0) {
					int index = 0;
					int investorTypeId = 0;
					for (index = 0; index <= FundRateSchedulesCount; index++) {
						investorTypeId = DataTypeHelper.ToInt32(collection[index.ToString() + "_InvestorTypeId"]);
						if (investorTypeId > 0) {
							managementFeeRateSchedule = new ManagementFeeRateSchedule();
							managementFeeRateSchedule.CreatedBy = Authentication.CurrentUser.UserID;
							managementFeeRateSchedule.CreatedDate = DateTime.Now;
							managementFeeRateSchedule.Description = string.Empty;
							managementFeeRateSchedule.EntityID = Authentication.CurrentEntity.EntityID;
							managementFeeRateSchedule.LastUpdatedBy = Authentication.CurrentUser.UserID;
							managementFeeRateSchedule.LastUpdatedDate = DateTime.Now;
							managementFeeRateSchedule.Name = string.Empty;
							managementFeeRateSchedule.RateScheduleTypeID = (int)Models.Fund.Enums.RateScheduleType.TieredRateSchedule;

							int tiersCount = DataTypeHelper.ToInt32(collection[index.ToString() + "_Tiers"]) + 1;
							int tierIndex = 0;
							for (tierIndex = 1; tierIndex <= tiersCount; tierIndex++) {
								if (collection[index.ToString() + "_IsScheduleChange"] == "true" &&
									DataTypeHelper.ToDateTime(collection[index.ToString() + "_" + tierIndex.ToString() + "_StartDate"]).Year > 1900 &&
									DataTypeHelper.ToDateTime(collection[index.ToString() + "_" + tierIndex.ToString() + "_EndDate"]).Year > 1900) {

									rateTier = new ManagementFeeRateScheduleTier();
									rateTier.CreatedBy = Authentication.CurrentUser.UserID;
									rateTier.CreatedDate = DateTime.Now;

									rateTier.EndDate = DataTypeHelper.ToDateTime(collection[index.ToString() + "_" + tierIndex.ToString() + "_EndDate"]);
									rateTier.LastUpdatedBy = Authentication.CurrentUser.UserID;
									rateTier.LastUpdatedDate = DateTime.Now;
									rateTier.MultiplierTypeID = DataTypeHelper.ToInt32(collection[index.ToString() + "_" + tierIndex.ToString() + "_MultiplierTypeId"]);
									if (rateTier.MultiplierTypeID > 0) {

										if (rateTier.MultiplierTypeID == (int)DeepBlue.Models.Fund.Enums.MutiplierType.CapitalCommitted)
											rateTier.Multiplier = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + tierIndex.ToString() + "_Rate"]);
										else
											rateTier.Multiplier = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + tierIndex.ToString() + "_FlatFee"]);

										rateTier.StartDate = DataTypeHelper.ToDateTime(collection[index.ToString() + "_" + tierIndex.ToString() + "_StartDate"]);
										rateTier.Notes = collection[index.ToString() + "_" + tierIndex.ToString() + "_Notes"];

										managementFeeRateSchedule.ManagementFeeRateScheduleTiers.Add(rateTier);
									}
								}
							}

							if (managementFeeRateSchedule.ManagementFeeRateScheduleTiers.Count > 0) {
								errorInfo = FundRepository.SaveManagementFeeRateSchedule(managementFeeRateSchedule);
								if (errorInfo != null) {
									foreach (var err in errorInfo.ToList()) {
										resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
									}
								}
								else {
									FundRateSchedule fundRateSchedule = null;
									int fundRateScheduleId = DataTypeHelper.ToInt32(collection[index.ToString() + "_FundRateScheduleId"]);
									if (fundRateScheduleId > 0) {
										fundRateSchedule = fund.FundRateSchedules.SingleOrDefault(schedule => schedule.FundRateScheduleID == fundRateScheduleId);
									}
									if (fundRateSchedule == null) {
										fundRateSchedule = new FundRateSchedule();
										fundRateSchedule.CreatedBy = Authentication.CurrentUser.UserID;
										fundRateSchedule.CreatedDate = DateTime.Now;
										fund.FundRateSchedules.Add(fundRateSchedule);
									}
									fundRateSchedule.InvestorTypeID = investorTypeId;
									fundRateSchedule.LastUpdatedBy = Authentication.CurrentUser.UserID;
									fundRateSchedule.LastUpdatedDate = DateTime.Now;
									fundRateSchedule.RateScheduleTypeID = (int)Models.Fund.Enums.RateScheduleType.TieredRateSchedule;
									fundRateSchedule.RateScheduleID = managementFeeRateSchedule.ManagementFeeRateScheduleID;
								}
							}
						}
					}
				}

				if (string.IsNullOrEmpty(resultModel.Result)) {
					errorInfo = FundRepository.SaveFund(fund);
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
						}
					}
					else {
						// Update custom field Values
						resultModel.Result += SaveCustomValues(collection, fund.FundID);
					}
				}
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}

		private string SaveCustomValues(FormCollection collection, int key) {
			System.Text.StringBuilder result = new StringBuilder();
			IEnumerable<ErrorInfo> errorInfo;
			List<CustomFieldDetail> customFields = AdminRepository.GetAllCustomFields((int)Models.Admin.Enums.Module.Fund);
			foreach (var field in customFields) {
				var customFieldValue = collection["CustomField_" + field.CustomFieldId.ToString()];
				if (customFieldValue != null) {
					CustomFieldValue value = AdminRepository.FindCustomFieldValue(field.CustomFieldId, key);
					if (value == null) {
						value = new CustomFieldValue();
						value.CreatedBy = Authentication.CurrentUser.UserID;
						value.CreatedDate = DateTime.Now;
					}
					value.CustomFieldID = field.CustomFieldId;
					value.Key = key;
					value.LastUpdatedBy = Authentication.CurrentUser.UserID;
					value.LastUpdatedDate = DateTime.Now;
					switch ((CustomFieldDataType)field.DataTypeId) {
						case CustomFieldDataType.Integer:
							value.IntegerValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToInt32(customFieldValue));
							break;
						case CustomFieldDataType.DateTime:
							value.DateValue = (string.IsNullOrEmpty(customFieldValue) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(customFieldValue));
							break;
						case CustomFieldDataType.Text:
							value.TextValue = customFieldValue;
							break;
						case CustomFieldDataType.Currency:
							value.CurrencyValue = (string.IsNullOrEmpty(customFieldValue) ? 0 : Convert.ToDecimal(customFieldValue));
							break;
						case CustomFieldDataType.Boolean:
							value.BooleanValue = (customFieldValue.Contains("true") ? true : false);
							break;
					}
					errorInfo = value.Save();
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							result.Append(err.PropertyName + " : " + err.ErrorMessage + "\n");
						}
					}
				}
			}
			return result.ToString();
		}

		[HttpGet]
		public string TaxIdAvailable(string TaxId, int FundId) {
			if (FundRepository.TaxIdAvailable(TaxId, FundId))
				return "TaxId already exist";
			else
				return string.Empty;
		}

		[HttpGet]
		public string FundNameAvailable(string FundName, int FundId) {
			if (FundRepository.FundNameAvailable(FundName, FundId))
				return "Fund Name already exist";
			else
				return string.Empty;
		}

		[HttpGet]
		public void DeleteFundRateSchedule(int id) {
			FundRepository.DeleteFundRateSchedule(id);
		}

		[HttpGet]
		public void DeleteManagementFeeRateSchedule(int id) {
			FundRepository.DeleteManagementFeeRateSchedule(id);
		}

	}
}
