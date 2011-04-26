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
	public class FundController : Controller {

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
			return View();
		}

		//
		// GET: /Fund/List
		[HttpGet]
		public ActionResult List(int pageIndex, int pageSize, string sortName, string sortOrder) {
			int totalRows = 0;
			IList<FundListModel> fundLists = FundRepository.GetAllFunds(pageIndex, pageSize, sortName, sortOrder, ref totalRows);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(fundLists);
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
		// GET: /Fund/New
		public ActionResult New() {
			CreateModel model = new CreateModel();
			/* Load Custom Fields */
			model.CustomField = new CustomFieldModel();
			model.CustomField.DisplayTwoColumn = true;
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund);
			model.MultiplierTypes = SelectListFactory.GetMultiplierTypeList(FundRepository.GetAllMultiplierTypes());
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			foreach (var field in model.CustomField.Fields) {
				model.CustomField.Values.Add(new CustomFieldValueDetail {
					CustomFieldId = field.CustomFieldID,
					CustomFieldValueId = 0,
					DataTypeId = field.DataTypeID,
					BooleanValue = false,
					CurrencyValue = 0,
					DateValue = string.Empty,
					IntegerValue = 0,
					TextValue = string.Empty,
					Key = 0
				});
			}
			model.FundRateSchedules = new List<FundRateScheduleDetail>();
			FundRateScheduleDetail scheduleDetail = new FundRateScheduleDetail();
			scheduleDetail.FundRateScheduleTiers = new List<FundRateScheduleTier>();
			AddExcessTiers(ref scheduleDetail);
			model.FundRateSchedules.Add(scheduleDetail);
			return View(model);
		}

		//
		// GET: /Fund/Edit/1
		public ActionResult Edit(int id) {
			EditModel model = new EditModel();
			Models.Entity.Fund fund = FundRepository.FindFund(id);
			foreach (var fundAccount in fund.FundAccounts) {
				model.ABANumber = fundAccount.Routing;
				model.Account = fundAccount.Account;
				model.AccountNumberCash = fundAccount.AccountNumberCash;
				model.AccountOf = fundAccount.AccountOf;
				model.Attention = fundAccount.Attention;
				model.BankName = fundAccount.BankName;
				model.FFCNumber = fundAccount.FFCNumber;
				model.IBAN = fundAccount.IBAN;
				model.Swift = fundAccount.SWIFT;
				model.Fax = fundAccount.Fax;
				model.Reference = fundAccount.Reference;
				model.AccountId = fundAccount.FundAccountID;
				model.Telephone = fundAccount.Phone;
			}
			model.FundId = fund.FundID;
			model.FundName = fund.FundName;
			model.TaxId = fund.TaxID ?? string.Empty;
			model.InceptionDate = fund.InceptionDate ?? Convert.ToDateTime("01/01/1900");
			model.ScheduleTerminationDate = fund.ScheduleTerminationDate;
			model.FinalTerminationDate = fund.FinalTerminationDate;
			model.NumofAutoExtensions = fund.NumofAutoExtensions;
			model.RecycleProvision = fund.RecycleProvision;
			model.DateClawbackTriggered = fund.DateClawbackTriggered;
			model.MgmtFeesCatchUpDate = fund.MgmtFeesCatchUpDate;
			model.Carry = fund.Carry;
			/* Load Custom Fields */
			model.CustomField = new CustomFieldModel();
			model.CustomField.DisplayTwoColumn = true;
			IList<CustomFieldValue> customFieldValues = AdminRepository.GetAllCustomFieldValues(id);
			model.CustomField.Fields = AdminRepository.GetAllCustomFields((int)DeepBlue.Models.Admin.Enums.Module.Fund);
			model.CustomField.Values = new List<CustomFieldValueDetail>();
			foreach (var field in model.CustomField.Fields) {
				var value = customFieldValues.SingleOrDefault(fieldValue => fieldValue.CustomFieldID == field.CustomFieldID);
				if (value != null) {
					model.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = value.CustomFieldID,
						CustomFieldValueId = value.CustomFieldID,
						DataTypeId = value.CustomField.DataTypeID,
						BooleanValue = value.BooleanValue ?? false,
						CurrencyValue = value.CurrencyValue ?? 0,
						DateValue = ((value.DateValue ?? Convert.ToDateTime("01/01/1900")).Year == 1900 ? string.Empty : (value.DateValue ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")),
						IntegerValue = value.IntegerValue ?? 0,
						TextValue = value.TextValue,
						Key = id
					});
				}
				else {
					model.CustomField.Values.Add(new CustomFieldValueDetail {
						CustomFieldId = field.CustomFieldID,
						CustomFieldValueId = 0,
						DataTypeId = field.DataTypeID,
						BooleanValue = false,
						CurrencyValue = 0,
						DateValue = string.Empty,
						IntegerValue = 0,
						TextValue = string.Empty,
						Key = id
					});
				}
			}
			/* Load Fund Rate Schedules */
			model.MultiplierTypes = SelectListFactory.GetMultiplierTypeList(FundRepository.GetAllMultiplierTypes());
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
			model.FundRateSchedules = new List<FundRateScheduleDetail>();
			FundRateScheduleDetail schedule;
			if (fund.FundRateSchedules.Count > 0) {
				foreach (var rateSchedule in fund.FundRateSchedules) {
					schedule = new FundRateScheduleDetail {
						FundId = id,
						FundRateScheduleId = rateSchedule.FundRateScheduleID,
						InvestorTypeId = rateSchedule.InvestorTypeID,
						RateScheduleId = rateSchedule.RateScheduleID,
						RateScheduleTypeId = rateSchedule.RateScheduleTypeID
					};
					schedule.FundRateScheduleTiers = new List<FundRateScheduleTier>();
					ManagementFeeRateSchedule feeRateSchedule = FundRepository.FindManagementFeeRateSchedule(rateSchedule.RateScheduleID);
					if (feeRateSchedule != null) {
						foreach (var tier in feeRateSchedule.ManagementFeeRateScheduleTiers) {
							schedule.FundRateScheduleTiers.Add(new FundRateScheduleTier {
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
					AddExcessTiers(ref schedule);
					model.FundRateSchedules.Add(schedule);
				}
			}
			else {
				schedule = new FundRateScheduleDetail();
				schedule.FundRateScheduleTiers = new List<FundRateScheduleTier>();
				AddExcessTiers(ref schedule);
				model.FundRateSchedules.Add(schedule);
			}

			return View(model);
		}

		private void AddExcessTiers(ref FundRateScheduleDetail schedule) {
			int index = 0;
			int total = (8 - schedule.FundRateScheduleTiers.Count);
			for (index = 0; index < total; index++) {
				schedule.FundRateScheduleTiers.Add(new FundRateScheduleTier());
			}
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			return SaveFund(collection);
		}

		//
		// GET: /Fund/Create
		[HttpPost]
		public ActionResult Update(FormCollection collection) {
			return SaveFund(collection);
		}

		private ActionResult SaveFund(FormCollection collection) {
			FundDetail model = new FundDetail();
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
			if (ModelState.IsValid) {
				Models.Entity.Fund fund;
				if (model.FundId > 0) {
					fund = FundRepository.FindFund(model.FundId);
				}
				else {
					fund = new Models.Entity.Fund();
				}
				fund.Carry = model.Carry ?? 0;
				fund.DateClawbackTriggered = model.DateClawbackTriggered ?? Convert.ToDateTime("01/01/1900");
				fund.EntityID = (int)ConfigUtil.CurrentEntityID;
				fund.FinalTerminationDate = model.FinalTerminationDate ?? Convert.ToDateTime("01/01/1900");
				fund.FundName = model.FundName;
				fund.InceptionDate = model.InceptionDate;
				fund.MgmtFeesCatchUpDate = model.MgmtFeesCatchUpDate ?? Convert.ToDateTime("01/01/1900");
				fund.NumofAutoExtensions = model.NumofAutoExtensions ?? 0;
				fund.RecycleProvision = model.RecycleProvision ?? 0;
				fund.ScheduleTerminationDate = model.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900");
				fund.TaxID = model.TaxId;

				//Add bank account
				FundAccount fundAccount;
				if (model.AccountId > 0) {
					fundAccount = fund.FundAccounts.SingleOrDefault(account => account.FundAccountID == model.AccountId);
				}
				else {
					fundAccount = new FundAccount();
					fund.FundAccounts.Add(fundAccount);
				}
				fundAccount.Account = model.Account;
				fundAccount.AccountNumberCash = model.AccountNumberCash ?? string.Empty;
				fundAccount.AccountOf = model.AccountOf ?? string.Empty;
				fundAccount.Attention = model.Attention ?? string.Empty;
				fundAccount.BankName = model.BankName ?? string.Empty;
				fundAccount.CreatedBy = AppSettings.CreatedByUserId;
				fundAccount.CreatedDate = DateTime.Now;
				fundAccount.EntityID = (int)ConfigUtil.CurrentEntityID;
				fundAccount.Fax = model.Fax ?? string.Empty;
				fundAccount.FFCNumber = model.FFCNumber ?? string.Empty;
				fundAccount.IBAN = model.IBAN ?? string.Empty;
				fundAccount.IsPrimary = false;
				fundAccount.LastUpdatedBy = AppSettings.CreatedByUserId;
				fundAccount.LastUpdatedDate = DateTime.Now;
				fundAccount.Phone = model.Telephone ?? string.Empty;
				fundAccount.Reference = model.Reference ?? string.Empty;
				fundAccount.Routing = model.ABANumber;
				fundAccount.SWIFT = model.Swift ?? string.Empty;

				/* Add new fund rate schedule */
				int FundRateSchedulesCount = Convert.ToInt32(collection["FundRateSchedulesCount"]);
				if (FundRateSchedulesCount > 0) {
					int index = 0;
					for (index = 1; index < FundRateSchedulesCount + 1; index++) {
						if (Convert.ToInt32(Convert.ToInt32(collection[index.ToString() + "_InvestorTypeId"])) > 0) {

							managementFeeRateSchedule = new ManagementFeeRateSchedule();

							managementFeeRateSchedule.CreatedBy = AppSettings.CreatedByUserId;
							managementFeeRateSchedule.CreatedDate = DateTime.Now;
							managementFeeRateSchedule.Description = string.Empty;
							managementFeeRateSchedule.EntityID = (int)ConfigUtil.CurrentEntityID;
							managementFeeRateSchedule.LastUpdatedBy = AppSettings.CreatedByUserId;
							managementFeeRateSchedule.LastUpdatedDate = DateTime.Now;
							managementFeeRateSchedule.Name = string.Empty;
							managementFeeRateSchedule.RateScheduleTypeID = (int)Models.Fund.Enums.RateScheduleType.TieredRateSchedule;

							int tiersCount = Convert.ToInt32(collection[index.ToString() + "_Tiers"]);
							int tierIndex = 0;
							for (tierIndex = 1; tierIndex < tiersCount + 1; tierIndex++) {
								if (collection[index.ToString() + "_IsScheduleChange"] == "true" &&
									string.IsNullOrEmpty(collection[index.ToString() + "_$" + tierIndex.ToString() + "$StartDate"]) == false &&
									string.IsNullOrEmpty(collection[index.ToString() + "_$" + tierIndex.ToString() + "$EndDate"]) == false) {

									rateTier = new ManagementFeeRateScheduleTier();
									rateTier.CreatedBy = AppSettings.CreatedByUserId;
									rateTier.CreatedDate = DateTime.Now;

									rateTier.EndDate = Convert.ToDateTime(collection[index.ToString() + "_$" + tierIndex.ToString() + "$EndDate"]);
									rateTier.LastUpdatedBy = AppSettings.CreatedByUserId;
									rateTier.LastUpdatedDate = DateTime.Now;
									rateTier.MultiplierTypeID = Convert.ToInt32(collection[index.ToString() + "_$" + tierIndex.ToString() + "$MultiplierTypeId"]);
									if (rateTier.MultiplierTypeID > 0) {
										if (rateTier.MultiplierTypeID == (int)DeepBlue.Models.Fund.Enums.MutiplierType.CapitalCommitted) {
											if (string.IsNullOrEmpty(collection[index.ToString() + "_$" + tierIndex.ToString() + "$Rate"]) == false)
												rateTier.Multiplier = Convert.ToDecimal(collection[index.ToString() + "_$" + tierIndex.ToString() + "$Rate"]);
										}
										else {
											if (string.IsNullOrEmpty(collection[index.ToString() + "_$" + tierIndex.ToString() + "$FlatFee"]) == false)
												rateTier.Multiplier = Convert.ToDecimal(collection[index.ToString() + "_$" + tierIndex.ToString() + "$FlatFee"]);
										}
										rateTier.StartDate = Convert.ToDateTime(collection[index.ToString() + "_$" + tierIndex.ToString() + "$StartDate"]);
										rateTier.Notes = collection[index.ToString() + "_$" + tierIndex.ToString() + "$Notes"];
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
									if (Convert.ToInt32(collection[index.ToString() + "_FundRateScheduleId"]) > 0) {
										fundRateSchedule = fund.FundRateSchedules.SingleOrDefault(schedule => schedule.FundRateScheduleID == Convert.ToInt32(collection[index.ToString() + "_FundRateScheduleId"]));
									}
									if (fundRateSchedule == null) {
										fundRateSchedule = new FundRateSchedule();
										fundRateSchedule.CreatedBy = AppSettings.CreatedByUserId;
										fundRateSchedule.CreatedDate = DateTime.Now;
										fund.FundRateSchedules.Add(fundRateSchedule);
									}
									fundRateSchedule.InvestorTypeID = Convert.ToInt32(collection[index.ToString() + "_InvestorTypeId"]);
									fundRateSchedule.LastUpdatedBy = AppSettings.CreatedByUserId;
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
			IList<CustomField> customFields = AdminRepository.GetAllCustomFields((int)Models.Admin.Enums.Module.Fund);
			foreach (var field in customFields) {
				var customFieldValue = collection["CustomField_" + field.CustomFieldID.ToString()];
				if (customFieldValue != null) {
					CustomFieldValue value = AdminRepository.FindCustomFieldValue(field.CustomFieldID, key);
					if (value == null) {
						value = new CustomFieldValue();
					}
					value.CreatedBy = AppSettings.CreatedByUserId;
					value.CreatedDate = DateTime.Now;
					value.CustomFieldID = field.CustomFieldID;
					value.Key = key;
					value.LastUpdatedBy = AppSettings.CreatedByUserId;
					value.LastUpdatedDate = DateTime.Now;
					switch ((CustomFieldDataType)field.DataTypeID) {
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
