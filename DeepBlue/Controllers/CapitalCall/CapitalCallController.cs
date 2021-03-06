﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Investor;
using System.Text;
using DeepBlue.Models.CapitalCall.Enums;
using DeepBlue.Models.Admin;
using System.Configuration;
using System.Data;
using System.IO;
using DeepBlue.Controllers.Accounting;


namespace DeepBlue.Controllers.CapitalCall {

	[OtherEntityAuthorize]
	public class CapitalCallController:BaseController {

		#region Constants
		private const string EXCELCAPITALCALLERROR_BY_KEY="CapitalCall-Excel-{0}";
		#endregion

		public IFundRepository FundRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public IInvestorRepository InvestorRepository { get; set; }

		public DeepBlue.Controllers.Admin.IAdminRepository AdminRepository { get; set; }

		public CapitalCallController()
			: this(new FundRepository(),new CapitalCallRepository(),new InvestorRepository(),new DeepBlue.Controllers.Admin.AdminRepository()) {
		}

		public CapitalCallController(IFundRepository fundRepository,
			ICapitalCallRepository capitalCallRepository,
			IInvestorRepository investorRepository,
			DeepBlue.Controllers.Admin.IAdminRepository adminRepository) {
			FundRepository=fundRepository;
			CapitalCallRepository=capitalCallRepository;
			InvestorRepository=investorRepository;
			AdminRepository=adminRepository;
		}

		#region New Capital Call

		//
		// GET: /CapitalCall/New
		public ActionResult New() {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="CapitalCall";
			ViewData["PageName"]="CapitalCall";
			CreateCapitalCallModel model=new CreateCapitalCallModel();
			Models.Fund.FundDetail fundDetail=FundRepository.FindLastFundDetail();
			if(fundDetail!=null) {
				model.FundId=fundDetail.FundId;
			}
			return View(model);
		}

		//
		// POST: /CapitalCall/ImportCapitalCall
		[HttpPost]
		public ActionResult ImportCapitalCall(FormCollection collection) {
			Models.Entity.CapitalCall capitalCall=new Models.Entity.CapitalCall();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(capitalCall,collection);
			capitalCall.CreatedBy=Authentication.CurrentUser.UserID;
			capitalCall.CreatedDate=DateTime.Now;
			capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
			capitalCall.LastUpdatedDate=DateTime.Now;
			capitalCall.CapitalCallTypeID=(int)Models.CapitalCall.Enums.CapitalCallType.Manual;
			capitalCall.CapitalCallNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallNumber(capitalCall.FundID));
			IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
			if(errorInfo==null) {
				resultModel.Result+="True||"+capitalCall.CapitalCallID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}

			return View("Result",resultModel);
		}

		//
		// POST: /CapitalCall/ImportCapitalCallLineItem
		[HttpPost]
		public ActionResult ImportCapitalCallLineItem(FormCollection collection) {
			CapitalCallLineItem item=new CapitalCallLineItem();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(item,collection);
			item.CreatedBy=Authentication.CurrentUser.UserID;
			item.CreatedDate=DateTime.Now;
			item.LastUpdatedBy=Authentication.CurrentUser.UserID;
			item.LastUpdatedDate=DateTime.Now;
			IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalCallLineItem(item);
			if(errorInfo==null) {
				resultModel.Result+="True||"+item.CapitalCallLineItemID;
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}


		//
		// POST: /CapitalCall/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateCapitalCallModel model=new CreateCapitalCallModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model,collection);
			if((model.AddManagementFees??false)==true) {
				DateTime fromDate=(model.FromDate??Convert.ToDateTime("01/01/1900"));
				DateTime toDate=(model.ToDate??Convert.ToDateTime("01/01/1900"));
				if(fromDate.Year<=1900)
					ModelState.AddModelError("FromDate","From Date is required");
				if(toDate.Year<=1900)
					ModelState.AddModelError("ToDate","To Date is required");
				if(fromDate.Year>1900&&toDate.Year>1900) {
					if(fromDate.Subtract(toDate).Days>=0) {
						ModelState.AddModelError("ToDate","To Date must be greater than From Date");
					}
				}
				if((model.ManagementFees??0)<=1) {
					ModelState.AddModelError("ManagementFees","Fee Amount is required");
				}
			}
			if(ModelState.IsValid) {

				// Attempt to create capital call.

				Models.Entity.CapitalCall capitalCall=new Models.Entity.CapitalCall();
				CapitalCallLineItem item;

				capitalCall.CapitalAmountCalled=model.CapitalAmountCalled;
				capitalCall.CapitalCallDate=model.CapitalCallDate;
				capitalCall.CapitalCallDueDate=model.CapitalCallDueDate;
				capitalCall.CapitalCallNumber=string.Empty;
				capitalCall.CapitalCallTypeID=(int)Models.CapitalCall.Enums.CapitalCallType.Reqular;
				capitalCall.CreatedBy=Authentication.CurrentUser.UserID;
				capitalCall.CreatedDate=DateTime.Now;
				capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
				capitalCall.LastUpdatedDate=DateTime.Now;
				capitalCall.ExistingInvestmentAmount=model.ExistingInvestmentAmount??0;
				capitalCall.NewInvestmentAmount=model.NewInvestmentAmount;
				capitalCall.FundID=model.FundId;
				capitalCall.CapitalCallNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallNumber(model.FundId));
				capitalCall.InvestmentAmount=(capitalCall.NewInvestmentAmount??0)+(capitalCall.ExistingInvestmentAmount??0);

				List<InvestorFund> investorFunds=CapitalCallRepository.GetAllInvestorFunds(capitalCall.FundID);

				if(investorFunds!=null) {

					// Find non managing total commitment.
					decimal nonManagingMemberTotalCommitment=investorFunds.Where(fund => fund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember).Sum(fund => fund.TotalCommitment);
					// Find managing total commitment.
					decimal managingMemberTotalCommitment=investorFunds.Where(fund => fund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember).Sum(fund => fund.TotalCommitment);
					// Calculate managing total commitment.
					decimal totalCommitment=nonManagingMemberTotalCommitment+managingMemberTotalCommitment;

					if((model.AddFundExpenses??false)==true) {
						capitalCall.FundExpenses=model.FundExpenseAmount??0;
					} else {
						capitalCall.FundExpenses=0;
					}

					if((model.AddManagementFees??false)==true) {
						capitalCall.ManagementFeeStartDate=model.FromDate;
						capitalCall.ManagementFeeEndDate=model.ToDate;
						capitalCall.ManagementFees=model.ManagementFees;
					} else {
						capitalCall.ManagementFees=0;
					}

					// Check new investment amount and existing investment amount.

					decimal investmentAmount=(capitalCall.NewInvestmentAmount??0)+(capitalCall.ExistingInvestmentAmount??0);
					decimal capitalAmount=capitalCall.CapitalAmountCalled-(capitalCall.ManagementFees??0)-(capitalCall.FundExpenses??0);

					if(((decimal.Round(investmentAmount)==decimal.Round(capitalAmount))==false)) {
						ModelState.AddModelError("NewInvestmentAmount","(New Investment Amount + Existing Investment Amount) should be equal to (Capital Amount - Management Fees - Fund Expenses).");
					} else {
						foreach(var investorFund in investorFunds) {

							// Attempt to create capital call line item for each investor fund.

							item=new CapitalCallLineItem();

							item.CreatedBy=Authentication.CurrentUser.UserID;
							item.CreatedDate=DateTime.Now;
							item.LastUpdatedBy=Authentication.CurrentUser.UserID;
							item.LastUpdatedDate=DateTime.Now;

							// Calculate Management Fees investor fund

							if((model.AddManagementFees??false)==true) {
								if(investorFund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
									item.ManagementFees=decimal.Multiply(
															decimal.Divide(investorFund.TotalCommitment,nonManagingMemberTotalCommitment)
															,(capitalCall.ManagementFees??0));
								}
							}

							// Calculate Fund Expense for investor fund

							if((model.AddFundExpenses??false)==true) {
								item.FundExpenses=decimal.Multiply(
													decimal.Divide(investorFund.TotalCommitment,totalCommitment)
													,(capitalCall.FundExpenses??0));
							}

							// Calculate Capital Amount for investor fund

							item.CapitalAmountCalled=CalculateCapitalAmountCalled(investorFund.TotalCommitment,
								totalCommitment,
								nonManagingMemberTotalCommitment,
								capitalCall.CapitalAmountCalled,
								(capitalCall.ManagementFees??0),
								(DeepBlue.Models.Investor.Enums.InvestorType)investorFund.InvestorTypeID);


							item.ExistingInvestmentAmount=(investorFund.TotalCommitment/totalCommitment)*capitalCall.ExistingInvestmentAmount;
							item.NewInvestmentAmount=(investorFund.TotalCommitment/totalCommitment)*capitalCall.NewInvestmentAmount;
							item.InvestmentAmount=(investorFund.TotalCommitment/totalCommitment)*capitalCall.InvestmentAmount;
							item.InvestorID=investorFund.InvestorID;

							// Reduce investor unfunded amount = investor unfunded amount - (capital call amount + management fees + fund expenses).

							investorFund.UnfundedAmount=decimal.Subtract((investorFund.UnfundedAmount??0)
																		   ,
																		   decimal.Add(item.CapitalAmountCalled,
																					   decimal.Add(
																									(item.ManagementFees??0)
																									,(item.FundExpenses??0))
																									)
																		   );

							capitalCall.CapitalCallLineItems.Add(item);
						}
						IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
						if(errorInfo!=null) {
							resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
						} else {
							foreach(var investorFund in investorFunds) {
								errorInfo=InvestorRepository.SaveInvestorFund(investorFund);
								resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
							}
						}
						if(string.IsNullOrEmpty(resultModel.Result)) {
							resultModel.Result+="True||"+(CapitalCallRepository.FindCapitalCallNumber(model.FundId))+"||"+capitalCall.CapitalCallID;
						}
					}
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		private static decimal CalculateCapitalAmountCalled(decimal memberCommitment,
		decimal totalCommitment,
		decimal nonManagingMemberCommitment,
		decimal totalCapitalCall,
		decimal managementFees,DeepBlue.Models.Investor.Enums.InvestorType investorType) {

			decimal memberCommitmentAmount=0;
			decimal capitalCallAmount=0;
			decimal result=0;

			if(investorType==DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {

				// Member commitment/Totalcommitment * (Total capital Call – ManagementFees)

				memberCommitmentAmount=decimal.Divide(memberCommitment,totalCommitment);

				capitalCallAmount=decimal.Subtract(totalCapitalCall,managementFees);

				result=decimal.Multiply(memberCommitmentAmount,capitalCallAmount);

			} else {

				// Member commitment / (sum of non managing member commitment ) * management fees

				memberCommitmentAmount=decimal.Divide(memberCommitment,totalCommitment);

				capitalCallAmount=decimal.Subtract(totalCapitalCall,managementFees);

				result=decimal.Multiply(memberCommitmentAmount,capitalCallAmount);

				memberCommitmentAmount=decimal.Divide(memberCommitment,nonManagingMemberCommitment);

				result=decimal.Add(result,decimal.Multiply(memberCommitmentAmount,managementFees));
			}

			return result;
		}

		//
		//GET : /CapitalCall/Result
		public ActionResult Result() {
			return View();
		}

		#endregion

		#region Modify Capital Call

		//
		// GET: /CapitalCall/ModifyCapitalCall
		public ActionResult ModifyCapitalCall(int? id) {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="ModifyCapitalCall";
			ViewData["PageName"]="ModifyCapitalCall";
			return View(new CreateCapitalCallModel { CapitalCallID=id });
		}

		//
		// POST: /CapitalCall/UpdateCapitalCall
		[HttpPost]
		public ActionResult UpdateCapitalCall(FormCollection collection) {
			CreateCapitalCallModel model=new CreateCapitalCallModel();
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(model,collection);
			if((model.CapitalCallID??0)==0) {
				ModelState.AddModelError("CapitalCallID","CapitalCallID is required");
			}
			if((model.AddManagementFees??false)==true) {
				DateTime fromDate=(model.FromDate??Convert.ToDateTime("01/01/1900"));
				DateTime toDate=(model.ToDate??Convert.ToDateTime("01/01/1900"));
				if(fromDate.Year<=1900)
					ModelState.AddModelError("FromDate","From Date is required");
				if(toDate.Year<=1900)
					ModelState.AddModelError("ToDate","To Date is required");
				if(fromDate.Year>1900&&toDate.Year>1900) {
					if(fromDate.Subtract(toDate).Days>=0) {
						ModelState.AddModelError("ToDate","To Date must be greater than From Date");
					}
				}
				if((model.ManagementFees??0)<=1) {
					ModelState.AddModelError("ManagementFees","Fee Amount is required");
				}
			}
			if(ModelState.IsValid) {

				// Attempt to update capital call.

				Models.Entity.CapitalCall capitalCall=CapitalCallRepository.FindCapitalCall((model.CapitalCallID??0));

				if(capitalCall!=null) {

					capitalCall.CapitalAmountCalled=model.CapitalAmountCalled;
					capitalCall.CapitalCallDate=model.CapitalCallDate;
					capitalCall.CapitalCallDueDate=model.CapitalCallDueDate;
					capitalCall.CapitalCallTypeID=(int)Models.CapitalCall.Enums.CapitalCallType.Reqular;
					capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
					capitalCall.LastUpdatedDate=DateTime.Now;
					capitalCall.NewInvestmentAmount=model.NewInvestmentAmount;
					capitalCall.FundID=model.FundId;

					if((model.AddFundExpenses??false)==true) {
						capitalCall.FundExpenses=model.FundExpenseAmount??0;
					} else {
						capitalCall.FundExpenses=0;
					}

					if((model.AddManagementFees??false)==true) {
						capitalCall.ManagementFeeStartDate=model.FromDate;
						capitalCall.ManagementFeeEndDate=model.ToDate;
						capitalCall.ManagementFees=model.ManagementFees;
					} else {
						capitalCall.ManagementFees=0;
					}

					// Check new investment amount and existing investment amount.

					decimal investmentAmount=(capitalCall.NewInvestmentAmount??0)+(capitalCall.ExistingInvestmentAmount??0);
					decimal capitalAmount=(capitalCall.CapitalAmountCalled)-(capitalCall.ManagementFees??0)-(capitalCall.FundExpenses??0);

					if(((decimal.Round(investmentAmount)==decimal.Round(capitalAmount))==false)) {
						ModelState.AddModelError("NewInvestmentAmount","(New Investment Amount + Existing Investment Amount) should be equal to (Capital Amount - Management Fees - Fund Expenses).");
					} else {

						errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
						resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);

						// Attempt to update capital call line item for each investor fund.
						int rowIndex;
						FormCollection rowCollection;
						CapitalCallLineItemModel itemModel=null;

						if(string.IsNullOrEmpty(resultModel.Result)) {
							for(rowIndex=0;rowIndex<model.CapitalCallLineItemsCount;rowIndex++) {
								rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(CapitalCallLineItemModel),"_");
								itemModel=new CapitalCallLineItemModel();
								this.TryUpdateModel(itemModel,rowCollection);
								errorInfo=ValidationHelper.Validate(itemModel);
								resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
								if(string.IsNullOrEmpty(resultModel.Result)) {
									CapitalCallLineItem capitalCallLineItem=CapitalCallRepository.FindCapitalCallLineItem(itemModel.CapitalCallLineItemID);
									if(capitalCallLineItem!=null) {
										capitalCallLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
										capitalCallLineItem.LastUpdatedDate=DateTime.Now;

										capitalCallLineItem.CapitalAmountCalled=itemModel.CapitalAmountCalled;
										capitalCallLineItem.FundExpenses=itemModel.FundExpenses;
										capitalCallLineItem.ManagementFees=itemModel.ManagementFees;
										capitalCallLineItem.NewInvestmentAmount=itemModel.NewInvestmentAmount;

										errorInfo=CapitalCallRepository.SaveCapitalCallLineItem(capitalCallLineItem);
									
										resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
									}
								} else {
									break;
								}
							}

						}

						if(string.IsNullOrEmpty(resultModel.Result)) {
							resultModel.Result+="True";
						}
					}
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /CapitalCall/FindCapitalCall
		public ActionResult FindCapitalCallModel(int id) {
			return Json(CapitalCallRepository.FindCapitalCallModel(id),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Manual Capital Call

		//
		// POST: /CapitalCall/CreateManualCapitalCall
		[HttpPost]
		public ActionResult CreateManualCapitalCall(FormCollection collection) {
			CreateCapitalCallModel model=new CreateCapitalCallModel();
			ResultModel resultModel=new ResultModel();
			List<InvestorFund> investorFunds=new List<InvestorFund>();
			this.TryUpdateModel(model,collection);
			if(ModelState.IsValid) {

				// Attempt to create manual capital call.

				Models.Entity.CapitalCall capitalCall=new Models.Entity.CapitalCall();
				CapitalCallLineItem item;

				capitalCall.CapitalAmountCalled=model.CapitalAmountCalled;
				capitalCall.CapitalCallDate=model.CapitalCallDate;
				capitalCall.CapitalCallDueDate=model.CapitalCallDueDate;
				capitalCall.CapitalCallTypeID=(int)Models.CapitalCall.Enums.CapitalCallType.Manual;
				capitalCall.CreatedBy=Authentication.CurrentUser.UserID;
				capitalCall.CreatedDate=DateTime.Now;
				capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
				capitalCall.LastUpdatedDate=DateTime.Now;
				capitalCall.ExistingInvestmentAmount=model.ExistingInvestmentAmount??0;
				capitalCall.NewInvestmentAmount=model.NewInvestmentAmount;
				capitalCall.FundID=model.FundId;
				capitalCall.InvestmentAmount=model.InvestedAmount??0;
				capitalCall.InvestedAmountInterest=model.InvestedAmountInterest??0;
				capitalCall.FundExpenses=model.FundExpenses??0;
				capitalCall.ManagementFees=model.ManagementFees??0;
				capitalCall.ManagementFeeInterest=model.ManagementFeeInterest??0;
				capitalCall.CapitalCallNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallNumber(model.FundId));
				int index;
				for(index=1;index<model.InvestorCount+1;index++) {
					item=new CapitalCallLineItem();
					item.CreatedBy=Authentication.CurrentUser.UserID;
					item.CreatedDate=DateTime.Now;
					item.LastUpdatedBy=Authentication.CurrentUser.UserID;
					item.LastUpdatedDate=DateTime.Now;
					item.CapitalAmountCalled=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"CapitalAmountCalled"]);
					item.ManagementFeeInterest=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"ManagementFeeInterest"]);
					item.InvestedAmountInterest=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"InvestedAmountInterest"]);
					item.ManagementFees=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"ManagementFees"]);
					item.FundExpenses=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"FundExpenses"]);
					item.InvestorID=DataTypeHelper.ToInt32(collection[index.ToString()+"_"+"InvestorId"]);
					if(item.InvestorID>0) {
						InvestorFund investorFund=InvestorRepository.FindInvestorFund(item.InvestorID,capitalCall.FundID);
						if(investorFund!=null) {
							// Reduce investor unfunded amount = investor unfunded amount – capital call amount for investor.
							investorFund.UnfundedAmount=investorFund.UnfundedAmount-item.CapitalAmountCalled;
							investorFunds.Add(investorFund);
						}
						capitalCall.CapitalCallLineItems.Add(item);
					}
				}
				if(capitalCall.CapitalCallLineItems.Count==0) {
					ModelState.AddModelError("InvestorCount","Select any one investor");
				} else {
					IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
					if(errorInfo!=null) {
						resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
					} else {
						foreach(var investorFund in investorFunds) {
							errorInfo=InvestorRepository.SaveInvestorFund(investorFund);
							resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
						}
					}
					if(string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result+="True||"+(CapitalCallRepository.FindCapitalCallNumber(model.FundId));
					}
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		#endregion

		#region Receive Capital Call

		//
		// GET: /CapitalCall/Receive
		[HttpGet]
		public ActionResult Receive(int? id,int? fundId) {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="Capital Call";
			ViewData["PageName"]="Receive Capital Call";
			CreateReceiveModel model=new CreateReceiveModel();
			model.CapitalCallId=id??0;
			model.FundId=fundId??0;
			model.CapitalCalls=new List<SelectListItem>();
			model.CapitalCalls.Add(new SelectListItem {
				Value="0",
				Text="--Select One--",
				Selected=false
			});
			model.Items=new List<CapitalCallLineItemDetail>();
			model.Items.Add(new CapitalCallLineItemDetail());
			return View(model);
		}

		//
		// POST: /CapitalCall/CreateReceiveCapitalCall
		[HttpPost]
		public ActionResult CreateReceiveCapitalCall(FormCollection collection) {
			CreateReceiveModel model=new CreateReceiveModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {

				// Attempt to create receive capital call.

				Models.Entity.CapitalCall capitalCall=CapitalCallRepository.FindCapitalCall(model.CapitalCallId);
				if(capitalCall!=null) {
					CapitalCallLineItem item;
					capitalCall.CapitalAmountCalled=model.CapitalAmountCalled;
					capitalCall.CapitalCallDate=model.CapitalCallDate;
					capitalCall.CapitalCallDueDate=model.CapitalCallDueDate;
					capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
					capitalCall.LastUpdatedDate=DateTime.Now;
					int index;
					for(index=1;index<model.ItemCount+1;index++) {
						item=capitalCall.CapitalCallLineItems.SingleOrDefault(capitalCallItem => capitalCallItem.CapitalCallLineItemID==Convert.ToInt32(collection[index.ToString()+"_"+"CapitalCallLineItemId"]));
						if(item!=null) {
							item.LastUpdatedBy=Authentication.CurrentUser.UserID;
							item.LastUpdatedDate=DateTime.Now;
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"CapitalAmountCalled"])==false) {
								item.CapitalAmountCalled=Convert.ToDecimal(collection[index.ToString()+"_"+"CapitalAmountCalled"]);
							}
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"ManagementFees"])==false) {
								item.ManagementFees=Convert.ToDecimal(collection[index.ToString()+"_"+"ManagementFees"]);
							}
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"InvestmentAmount"])==false) {
								item.InvestmentAmount=Convert.ToDecimal(collection[index.ToString()+"_"+"InvestmentAmount"]);
							}
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"InvestedAmountInterest"])==false) {
								item.InvestedAmountInterest=Convert.ToDecimal(collection[index.ToString()+"_"+"InvestedAmountInterest"]);
							}
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"ManagementFeeInterest"])==false) {
								item.ManagementFeeInterest=Convert.ToDecimal(collection[index.ToString()+"_"+"ManagementFeeInterest"]);
							}
							if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"Received"])==false) {
								if(collection[index.ToString()+"_"+"Received"].Contains("true")) {
									if(string.IsNullOrEmpty(collection[index.ToString()+"_"+"ReceivedDate"])==false) {
										item.ReceivedDate=Convert.ToDateTime(collection[index.ToString()+"_"+"ReceivedDate"]);
									}
								} else {
									item.ReceivedDate=null;
								}
							}
							if((item.ReceivedDate??Convert.ToDateTime("01/01/1900")).Year<=1900) {
								item.ReceivedDate=null;
							}
						}
					}
					IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		#endregion

		#region Manament Fee Calculation

		//GET : /CapitalCall/CalculateManagementFee
		[HttpGet]
		public JsonResult CalculateManagementFee(int fundId,DateTime startDate,DateTime endDate) {
			List<ManagementFeeRateScheduleTierDetail> tiers=null;
			List<ManagementFeeDetail> managementFeeDetails=GetManagementFeeDetails(fundId,startDate,endDate,ref tiers);
			return Json(new { ManagementFee=managementFeeDetails.Sum(m => m.ManagementFee),Tiers=tiers },JsonRequestBehavior.AllowGet);
		}

		private List<ManagementFeeDetail> GetManagementFeeDetails(int fundId,DateTime startDate,DateTime endDate,ref List<ManagementFeeRateScheduleTierDetail> tiers) {
			List<NonManagingInvestorFundDetail> investorFunds=CapitalCallRepository.GetAllNonManagingInvestorFunds(fundId);
			List<ManagementFeeDetail> managementFeeDetails=new List<ManagementFeeDetail>();
			tiers=CapitalCallRepository.GetAllManagementFeeRateScheduleTiers(fundId,startDate,endDate);
			if(tiers!=null&&investorFunds!=null) {
				ManagementFeeDetail managementFeeDetail=null;
				decimal fee;
				decimal multiplier;
				decimal totalCommitment=investorFunds.Sum(investorFund => investorFund.TotalCommitment);
				int tier1Days=0;
				foreach(var investorFund in investorFunds) {

					managementFeeDetail=new ManagementFeeDetail();
					managementFeeDetail.InvestorId=investorFund.InvestorId;
					managementFeeDetails.Add(managementFeeDetail);
					fee=0;
					multiplier=0;
					if(tiers.Count>0) {
						tier1Days=tiers[0].EndDate.Subtract(startDate).Days;
						switch(tiers.Count) {
						case 1:
							if(tiers[0].MultiplierTypeId==(int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
								multiplier=decimal.Divide(tiers[0].Multiplier,(decimal)100);
								managementFeeDetail.ManagementFee+=decimal.Multiply(multiplier,
									decimal.Multiply(
									investorFund.TotalCommitment,
									 decimal.Divide((decimal)tier1Days,(decimal)360)
									 ));
							} else {
								multiplier=(tiers[0].Multiplier);
								fee=decimal.Multiply(multiplier,decimal.Divide((decimal)tier1Days,(decimal)360));
								managementFeeDetail.ManagementFee+=decimal.Multiply(fee,decimal.Divide(investorFund.TotalCommitment,totalCommitment));
							}
							break;
						case 2:
							if(tiers[0].MultiplierTypeId==(int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
								multiplier=decimal.Divide(tiers[0].Multiplier,(decimal)100);
								managementFeeDetail.ManagementFee+=decimal.Multiply(multiplier,
									decimal.Multiply(
									investorFund.TotalCommitment,
									decimal.Divide((decimal)tier1Days,(decimal)360)
									));
							} else {
								multiplier=(tiers[0].Multiplier);
								fee=multiplier*decimal.Divide((decimal)tier1Days,(decimal)360);
								managementFeeDetail.ManagementFee+=decimal.Multiply(fee,decimal.Divide(investorFund.TotalCommitment,totalCommitment));
							}

							if(tiers[1].MultiplierTypeId==(int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
								multiplier=decimal.Divide(tiers[1].Multiplier,(decimal)100);
								managementFeeDetail.ManagementFee+=
								decimal.Multiply(multiplier,
								decimal.Multiply(
								investorFund.TotalCommitment,
								decimal.Divide((decimal)(90-tier1Days),(decimal)360))
								);
							} else {
								multiplier=(tiers[1].Multiplier);
								fee=decimal.Multiply(multiplier,decimal.Divide((decimal)(90-tier1Days),(decimal)360));
								managementFeeDetail.ManagementFee+=decimal.Multiply(fee,decimal.Divide(investorFund.TotalCommitment,totalCommitment));
							}
							break;
						}
					}
				}
			}
			return managementFeeDetails;
		}

		private decimal CalculateCapitalFee(decimal totalCommittedAmount,decimal multiplier,double days) {
			return (totalCommittedAmount*(multiplier/100)*decimal.Divide((decimal)days,(decimal)360));
		}

		private decimal CalculateFlatFee(decimal totalCommittedAmount,decimal multiplier,double days) {
			return (totalCommittedAmount*multiplier*decimal.Divide((decimal)days,(decimal)360));
		}

		#endregion

		#region Capital Distribution

		//
		// GET: /CapitalCall/NewCapitalDistribution
		[HttpGet]
		public ActionResult NewCapitalDistribution() {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="CapitalDistribution";
			ViewData["PageName"]="CapitalDistribution";
			CreateDistributionModel model=new CreateDistributionModel();
			Models.Fund.FundDetail fundDetail=FundRepository.FindLastFundDetail();
			if(fundDetail!=null) {
				model.FundId=fundDetail.FundId;
			}
			return View(model);
		}

		//
		// POST: /CapitalCall/CreateDistribution
		[HttpPost]
		public ActionResult CreateDistribution(FormCollection collection) {
			CreateDistributionModel model=new CreateDistributionModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model,collection);

			CheckDistributionAmount(model);

			if(ModelState.IsValid) {

				// Attempt to create cash distribution.

				CapitalDistribution distribution=new CapitalDistribution();
				CapitalDistributionLineItem item;
				distribution.CapitalDistributionDate=model.CapitalDistributionDate;
				distribution.CapitalDistributionDueDate=model.CapitalDistributionDueDate;

				distribution.CapitalReturn=model.CapitalReturn;
				distribution.PreferredReturn=model.PreferredReturn;
				distribution.ReturnManagementFees=model.ReturnManagementFees;
				distribution.ReturnFundExpenses=model.ReturnFundExpenses;
				distribution.PreferredCatchUp=model.PreferredCatchUp;
				distribution.Profits=model.GPProfits;
				distribution.LPProfits=model.LPProfits;

				distribution.CreatedBy=Authentication.CurrentUser.UserID;
				distribution.CreatedDate=DateTime.Now;
				distribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
				distribution.LastUpdatedDate=DateTime.Now;
				distribution.DistributionAmount=model.DistributionAmount;
				distribution.DistributionNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
				distribution.FundID=model.FundId;
				distribution.IsManual=false;

				// Get all investor funds.
				List<InvestorFund> investorFunds=CapitalCallRepository.GetAllInvestorFunds(distribution.FundID);

				if(investorFunds!=null) {

					// Find non managing member total commitment.
					decimal nonManagingMemberTotalCommitment=investorFunds.Where(fund => fund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember).Sum(fund => fund.TotalCommitment);
					// Find managing member total commitment.
					decimal managingMemberTotalCommitment=investorFunds.Where(fund => fund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember).Sum(fund => fund.TotalCommitment);
					// Find total commitment.
					decimal totalCommitment=nonManagingMemberTotalCommitment+managingMemberTotalCommitment;

					foreach(var investorFund in investorFunds) {

						// Attempt to create cash distribution of each investor.

						item=new CapitalDistributionLineItem();

						item.CreatedBy=Authentication.CurrentUser.UserID;
						item.CreatedDate=DateTime.Now;
						item.LastUpdatedBy=Authentication.CurrentUser.UserID;
						item.LastUpdatedDate=DateTime.Now;
						item.InvestorID=investorFund.InvestorID;

						item.CapitalReturn=decimal.Multiply((distribution.CapitalReturn??0),
																decimal.Divide(investorFund.TotalCommitment,totalCommitment)
																);

						item.PreferredReturn=decimal.Multiply((distribution.PreferredReturn??0),
																decimal.Divide(investorFund.TotalCommitment,totalCommitment)
																);

						// Non ManagingMember investor type only
						if(investorFund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
							item.ReturnManagementFees=decimal.Multiply((distribution.ReturnManagementFees??0),
																		 decimal.Divide(investorFund.TotalCommitment,nonManagingMemberTotalCommitment)
																		);
						}

						item.ReturnFundExpenses=decimal.Multiply((distribution.ReturnFundExpenses??0),
																decimal.Divide(investorFund.TotalCommitment,totalCommitment)
																);

						// ManagingMember investor type only
						if(investorFund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {
							item.PreferredCatchUp=distribution.PreferredCatchUp;
						}

						// ManagingMember investor type only
						if(investorFund.InvestorTypeID==(int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {
							item.Profits=distribution.Profits;
						}

						item.LPProfits=decimal.Multiply((distribution.LPProfits??0),
														decimal.Divide(investorFund.TotalCommitment,totalCommitment)
														);


						// Calculate distribution amount of each investor.
						item.DistributionAmount=(item.CapitalReturn??0)
													+(item.PreferredReturn??0)
													+(item.ReturnManagementFees??0)
													+(item.ReturnFundExpenses??0)
													+(item.PreferredCatchUp??0)
													+(item.Profits??0)
													+(item.LPProfits??0)
													;
						distribution.CapitalDistributionLineItems.Add(item);
					}
				}
				IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalDistribution(distribution);
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result+="True||"+(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId))+"||"+distribution.CapitalDistributionID;
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /CapitalCall/CapitalDistributionList
		[HttpGet]
		public ActionResult CapitalDistributionList(int? id) {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="Capital Call";
			ViewData["PageName"]="Capital Distribution List";
			CapitalDistributionListModel model=new CapitalDistributionListModel();
			model.FundId=id??0;
			if(model.FundId>0) {
				model.FundName=FundRepository.FindFundName(model.FundId);
			}
			return View(model);
		}

		//
		// GET: /CapitalCall/CapitalDistributionDetailList
		[HttpGet]
		public ActionResult CapitalDistributionDetailList(int pageIndex,int pageSize,string sortName,string sortOrder,int fundId) {
			int totalRows=0;
			List<Models.Entity.CapitalDistribution> capitalCalls=CapitalCallRepository.GetCapitalDistributions(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundId);
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(capitalCalls);
		}

		private void CheckDistributionAmount(CreateDistributionModel model) {
			if(model.DistributionAmount>0) {
				decimal distributionCheck=((model.CapitalReturn??0)+(model.PreferredReturn??0)+(model.PreferredCatchUp??0)+(model.ReturnFundExpenses??0)+(model.ReturnManagementFees??0)+(model.GPProfits??0)+(model.LPProfits??0));
				if((decimal.Round(model.DistributionAmount)==decimal.Round(distributionCheck))==false) {
					ModelState.AddModelError("DistributionAmount","Components of the distribution do not add up to total distribution amount");
				}
			}
		}

		#endregion

		#region Modify Capital Distribution
		//
		// GET: /CapitalCall/ModifyCapitalDistribution
		public ActionResult ModifyCapitalDistribution(int? id) {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="ModifyCapitalDistribution";
			ViewData["PageName"]="ModifyCapitalDistribution";
			return View(new CreateDistributionModel { CapitalDistributionID=id });
		}

		//
		// POST: /CapitalCall/UpdateDistribution
		[HttpPost]
		public ActionResult UpdateDistribution(FormCollection collection) {
			CreateDistributionModel model=new CreateDistributionModel();
			ResultModel resultModel=new ResultModel();
			IEnumerable<ErrorInfo> errorInfo=null;
			this.TryUpdateModel(model,collection);
			if((model.CapitalDistributionID??0)==0) {
				ModelState.AddModelError("CapitalDistributionID","CapitalDistributionID is required");
			}
			CheckDistributionAmount(model);
			if(ModelState.IsValid) {

				// Attempt to create cash distribution.

				CapitalDistribution distribution=CapitalCallRepository.FindCapitalDistribution((model.CapitalDistributionID??0));

				if(distribution!=null) {
					distribution.CapitalDistributionDate=model.CapitalDistributionDate;
					distribution.CapitalDistributionDueDate=model.CapitalDistributionDueDate;

					distribution.CapitalReturn=model.CapitalReturn;
					distribution.PreferredReturn=model.PreferredReturn;
					distribution.ReturnManagementFees=model.ReturnManagementFees;
					distribution.ReturnFundExpenses=model.ReturnFundExpenses;
					distribution.PreferredCatchUp=model.PreferredCatchUp;
					distribution.Profits=model.GPProfits;
					distribution.LPProfits=model.LPProfits;
					distribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
					distribution.LastUpdatedDate=DateTime.Now;
					distribution.DistributionAmount=model.DistributionAmount;

					errorInfo=CapitalCallRepository.SaveCapitalDistribution(distribution);
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);

					// Attempt to update capital call line item for each investor fund.
					int rowIndex;
					FormCollection rowCollection;
					CapitalDistributionLineItemModel itemModel=null;

					if(string.IsNullOrEmpty(resultModel.Result)) {
						for(rowIndex=0;rowIndex<model.CapitalDistributionLineItemsCount;rowIndex++) {
							rowCollection=FormCollectionHelper.GetFormCollection(collection,rowIndex,typeof(CapitalDistributionLineItemModel),"_");
							itemModel=new CapitalDistributionLineItemModel();
							this.TryUpdateModel(itemModel,rowCollection);
							errorInfo=ValidationHelper.Validate(itemModel);
							resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
							if(string.IsNullOrEmpty(resultModel.Result)) {

								// Attempt to create cash distribution of each investor.

								CapitalDistributionLineItem capitalDistributionLineItem=CapitalCallRepository.FindCapitalDistributionLineItem(itemModel.CapitalDistributionLineItemID);
								if(capitalDistributionLineItem!=null) {

									capitalDistributionLineItem.LastUpdatedBy=Authentication.CurrentUser.UserID;
									capitalDistributionLineItem.LastUpdatedDate=DateTime.Now;

									capitalDistributionLineItem.CapitalReturn=itemModel.CapitalReturn;
									capitalDistributionLineItem.PreferredReturn=itemModel.PreferredReturn;
									capitalDistributionLineItem.ReturnManagementFees=itemModel.ReturnManagementFees;
									capitalDistributionLineItem.ReturnFundExpenses=itemModel.ReturnFundExpenses;
									capitalDistributionLineItem.PreferredCatchUp=itemModel.PreferredCatchUp;
									capitalDistributionLineItem.Profits=itemModel.Profits;
									capitalDistributionLineItem.LPProfits=itemModel.LPProfits;
									capitalDistributionLineItem.DistributionAmount=itemModel.DistributionAmount;

									errorInfo=CapitalCallRepository.SaveCapitalDistributionLineItem(capitalDistributionLineItem);
									resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
								} else {
									break;
								}
							}

						}

						if(string.IsNullOrEmpty(resultModel.Result)) {
							resultModel.Result+="True";
						}
					}

				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// GET: /CapitalCall/FindCapitalDistributionModel
		public ActionResult FindCapitalDistributionModel(int id) {
			return Json(CapitalCallRepository.FindCapitalDistributionModel(id),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/FindCapitalDistributions
		[HttpGet]
		public JsonResult FindCapitalDistributions(string term,int fundId) {
			return Json(CapitalCallRepository.FindCapitalDistributions(term,fundId),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Manual Capital Distribution
		//
		// POST: /CapitalCall/CreateManualDistribution
		[HttpPost]
		public ActionResult CreateManualDistribution(FormCollection collection) {
			CreateDistributionModel model=new CreateDistributionModel();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(model,collection);

			CheckDistributionAmount(model);

			if(ModelState.IsValid) {

				// Attempt to create cash distribution.

				CapitalDistribution distribution=new CapitalDistribution();
				CapitalDistributionLineItem item;
				distribution.CapitalDistributionDate=model.CapitalDistributionDate;
				distribution.CapitalDistributionDueDate=model.CapitalDistributionDueDate;

				distribution.PreferredReturn=model.PreferredReturn;
				distribution.ReturnManagementFees=model.ReturnManagementFees;
				distribution.ReturnFundExpenses=model.ReturnFundExpenses;
				distribution.PreferredCatchUp=model.PreferredCatchUp;
				distribution.Profits=model.GPProfits;
				distribution.LPProfits=model.LPProfits;

				distribution.CreatedBy=Authentication.CurrentUser.UserID;
				distribution.CreatedDate=DateTime.Now;
				distribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
				distribution.LastUpdatedDate=DateTime.Now;
				distribution.DistributionAmount=model.DistributionAmount;
				distribution.DistributionNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
				distribution.FundID=model.FundId;
				distribution.CapitalReturn=model.CapitalReturn;
				distribution.IsManual=true;


				int index;

				for(index=1;index<model.InvestorCount+1;index++) {
					// Attempt to create cash distribution of each investor.

					item=new CapitalDistributionLineItem();
					item.CapitalReturn=0;
					item.CreatedBy=Authentication.CurrentUser.UserID;
					item.CreatedDate=DateTime.Now;
					item.LastUpdatedBy=Authentication.CurrentUser.UserID;
					item.LastUpdatedDate=DateTime.Now;
					item.InvestorID=DataTypeHelper.ToInt32(collection[index.ToString()+"_"+"InvestorId"]);
					item.DistributionAmount=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"DistributionAmount"]);
					item.ReturnManagementFees=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"ReturnManagementFees"]);
					item.ReturnFundExpenses=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"ReturnFundExpenses"]);
					item.Profits=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"GPProfits"]);
					item.PreferredReturn=DataTypeHelper.ToDecimal(collection[index.ToString()+"_"+"PreferredReturn"]);
					if(item.InvestorID>0) {
						distribution.CapitalDistributionLineItems.Add(item);
					}
				}
				if(distribution.CapitalDistributionLineItems.Count==0) {
					ModelState.AddModelError("InvestorCount","Select any one investor");
				} else {
					IEnumerable<ErrorInfo> errorInfo=CapitalCallRepository.SaveCapitalDistribution(distribution);
					resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
					if(string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result+="True||"+(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
					}
				}
			}
			if(ModelState.IsValid==false) {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return View("Result",resultModel);
		}

		//
		// POST: /CapitalCall/ImportManualDistribution
		[HttpPost]
		public ActionResult ImportManualDistribution(FormCollection collection) {
			CapitalDistribution distribution=new CapitalDistribution();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(distribution,collection);
			distribution.DistributionNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber(distribution.FundID));
			distribution.CreatedBy=Authentication.CurrentUser.UserID;
			distribution.CreatedDate=DateTime.Now;
			distribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
			distribution.LastUpdatedDate=DateTime.Now;
			IEnumerable<ErrorInfo> errorInfo=ValidationHelper.Validate(distribution);
			if(errorInfo.Any()==false) {
				// Attempt to create cash distribution.
				errorInfo=CapitalCallRepository.SaveCapitalDistribution(distribution);
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result+="True||"+distribution.CapitalDistributionID;
				}
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}

		[HttpPost]
		public ActionResult ImportManualDistributionLineItem(FormCollection collection) {
			CapitalDistributionLineItem item=new CapitalDistributionLineItem();
			ResultModel resultModel=new ResultModel();
			this.TryUpdateModel(item,collection);
			item.CreatedBy=Authentication.CurrentUser.UserID;
			item.CreatedDate=DateTime.Now;
			item.LastUpdatedBy=Authentication.CurrentUser.UserID;
			item.LastUpdatedDate=DateTime.Now;
			IEnumerable<ErrorInfo> errorInfo=ValidationHelper.Validate(item);
			if(errorInfo.Any()==false) {
				// Attempt to create cash distribution of each investor.
				errorInfo=CapitalCallRepository.SaveCapitalDistributionLineItem(item);
				resultModel.Result+=ValidationHelper.GetErrorInfo(errorInfo);
				if(string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result+="True||"+item.CapitalDistributionLineItemID;
				}
			} else {
				resultModel.Result=ValidationHelper.GetErrorInfo(errorInfo);
			}
			return View("Result",resultModel);
		}
		#endregion

		#region Capital Call Detail

		//
		//GET : /CapitalCall/Detail
		[HttpGet]
		public ActionResult Detail(int? fundId,int? typeId) {
			ViewData["MenuName"]="FundManagement";
			ViewData["SubmenuName"]="Detail";
			ViewData["PageName"]="Detail";
			if((typeId??0)==0) {
				typeId=(int)DetailType.CapitalCall;
			}
			DetailModel model=new DetailModel();
			model.FundId=fundId??0;
			model.DetailType=(DetailType)typeId;
			if(model.FundId==0) {
				Models.Fund.FundDetail fundDetail=FundRepository.FindLastFundDetail();
				if(fundDetail!=null) {
					model.FundId=fundDetail.FundId;
				}
			}
			return View("Detail",model);
		}

		//
		//GET : /CapitalCall/FindDetail
		[HttpGet]
		public JsonResult FindDetail(int fundId) {
			return Json(CapitalCallRepository.FindDetail(fundId),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult FindCapitalCallDetail(int fundId,decimal? capitalCallAmount,DateTime? capitalCallDate,DateTime? capitalCallDueDate) {
			return Json(CapitalCallRepository.FindCapitalCallDetail(fundId,capitalCallAmount,capitalCallDate,capitalCallDueDate),JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult FindCapitalDistributionDetail(int fundId,decimal? capitalDistributionAmount,DateTime? capitalDistributionDate,DateTime? capitalDistributionDueDate) {
			return Json(CapitalCallRepository.FindCapitalDistributionDetail(fundId,capitalDistributionAmount,capitalDistributionDate,capitalDistributionDueDate),JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/GetCapitalCallInvestors
		[HttpGet]
		public JsonResult GetCapitalCallInvestors(int capitalCallId) {
			return Json(new { Investors=CapitalCallRepository.GetCapitalCallInvestors(capitalCallId) },JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/GetCapitalDistributionInvestors
		[HttpGet]
		public JsonResult GetCapitalDistributionInvestors(int capitalDistributionId) {
			return Json(new { Investors=CapitalCallRepository.GetCapitalDistributionInvestors(capitalDistributionId) },JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/FundDetail
		[HttpGet]
		public JsonResult FundDetail(int id) {
			return Json(CapitalCallRepository.FindFundDetail(id),JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/CapitalCallList
		[HttpGet]
		public ActionResult CapitalCallList(int pageIndex,int pageSize,string sortName,string sortOrder,int fundId) {
			int totalRows=0;
			List<Models.Entity.CapitalCall> capitalCalls=CapitalCallRepository.GetCapitalCalls(pageIndex,pageSize,sortName,sortOrder,ref totalRows,fundId);
			ViewData["TotalRows"]=totalRows;
			ViewData["PageNo"]=pageIndex;
			return View(capitalCalls);
		}

		//
		// GET: /CapitalCall/GetCapitalCallList
		[HttpGet]
		public JsonResult GetCapitalCallList(int fundId) {
			List<Models.Entity.CapitalCall> capitalCalls=CapitalCallRepository.GetCapitalCalls(fundId);
			List<SelectListItem> selectLists=new List<SelectListItem>();
			foreach(var capitalCall in capitalCalls) {
				selectLists.Add(new SelectListItem { Selected=false,Text=capitalCall.CapitalCallNumber+"# ("+capitalCall.CapitalCallDate.ToString("MM/dd/yyyy")+")",Value=capitalCall.CapitalCallID.ToString() });
			}
			return Json(selectLists,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/FindCapitalCall
		[HttpGet]
		public JsonResult FindCapitalCall(int id) {
			CreateReceiveModel model=new CreateReceiveModel();
			Models.Entity.CapitalCall capitalCall=CapitalCallRepository.FindCapitalCall(id);
			if(capitalCall!=null) {
				model.FundId=capitalCall.Fund.FundID;
				model.FundName=capitalCall.Fund.FundName;
				model.CapitalCallNumber=capitalCall.CapitalCallNumber;
				model.CapitalAmountCalled=capitalCall.CapitalAmountCalled;
				model.CapitalCallDate=capitalCall.CapitalCallDate;
				model.CapitalCallDueDate=capitalCall.CapitalCallDueDate;
				model.CapitalCallId=capitalCall.CapitalCallID;
				model.Items=new List<CapitalCallLineItemDetail>();
				int index=0;
				foreach(var item in capitalCall.CapitalCallLineItems) {
					index++;
					model.Items.Add(new CapitalCallLineItemDetail {
						Index=index,
						InvestorName=item.Investor.InvestorName,
						CapitalAmountCalled=item.CapitalAmountCalled,
						InvestedAmountInterest=item.InvestedAmountInterest??0,
						CapitalCallLineItemId=item.CapitalCallLineItemID,
						InvestmentAmount=item.InvestedAmountInterest??0,
						ManagementFeeInterest=item.ManagementFeeInterest??0,
						ManagementFees=item.ManagementFees??0,
						Received=(item.ReceivedDate.HasValue?true:false),
						ReceivedDate=((item.ReceivedDate??Convert.ToDateTime("01/01/1900")).Year<=1900?string.Empty:(item.ReceivedDate??Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"))
					});
				}
			}
			return Json(model,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/FindCapitalCalls
		[HttpGet]
		public JsonResult FindCapitalCalls(string term,int fundId) {
			return Json(CapitalCallRepository.FindCapitalCalls(term,fundId),JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region Import CapitalCall

		[HttpPost]
		public ActionResult ImportExcel(FormCollection collection) {
			DeepBlue.Models.Deal.ImportExcelFileModel model=new DeepBlue.Models.Deal.ImportExcelFileModel();
			DeepBlue.Models.Excel.ImportExcelModel importExcelModel=new DeepBlue.Models.Excel.ImportExcelModel();
			importExcelModel.Tables=new List<DeepBlue.Models.Excel.ImportExcelTableModel>();
			this.TryUpdateModel(model);
			if(string.IsNullOrEmpty(model.FileName)) {
				ModelState.AddModelError("FileName","File Name is required");
			}
			if(ModelState.IsValid) {
				string rootPath=Server.MapPath("~/");
				string uploadFilePath=Path.Combine(model.FilePath,model.FileName);
				string errorMessage=string.Empty;
				string sessionKey=string.Empty;
				DataSet ds=ExcelConnection.GetDataSet(uploadFilePath,uploadFilePath,ref errorMessage,ref sessionKey);
				DeepBlue.Models.Excel.ImportExcelTableModel tableModel=null;
				if(string.IsNullOrEmpty(errorMessage)) {
					foreach(DataTable dt in ds.Tables) {
						tableModel=new DeepBlue.Models.Excel.ImportExcelTableModel();
						tableModel.TableName=dt.TableName;
						tableModel.TotalRows=dt.Rows.Count;
						tableModel.SessionKey=sessionKey;
						foreach(DataColumn column in dt.Columns) {
							tableModel.Columns.Add(column.ColumnName);
						}
						importExcelModel.Tables.Add(tableModel);
					}
				} else {
					importExcelModel.Result+=errorMessage;
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							importExcelModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return Json(importExcelModel);
		}

		#region ImportCapitalCallExcel

		[HttpPost]
		public ActionResult ImportCapitalCallExcel(FormCollection collection) {
			ImportManualCapitalCallExcelModel model=new ImportManualCapitalCallExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELCAPITALCALLERROR_BY_KEY,model.SessionKey);
				List<DeepBlue.Models.Deal.ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<DeepBlue.Models.Deal.ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.ManualCapitalCallTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.ManualCapitalCallTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}

						int rowNumber=0;

						string investorName=string.Empty;
						string fundName=string.Empty;
						decimal capitalCallAmount=0;
						decimal managementFeesInterest=0;
						decimal investedAmountInterest=0;
						decimal managementFees=0;
						decimal fundExpenses=0;
						DateTime capitalCallDate;
						DateTime capitalCallDueDate;

						DeepBlue.Models.Deal.ImportExcelError error;
						DeepBlue.Models.Entity.Investor investor;
						DeepBlue.Models.Entity.Fund fund;
						DeepBlue.Models.Entity.InvestorFund investorFund;

						EmailAttribute emailValidation=new EmailAttribute();
						ZipAttribute zipAttribute=new ZipAttribute();
						WebAddressAttribute webAttribute=new WebAddressAttribute();
						IEnumerable<ErrorInfo> errorInfo;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);

							error=new DeepBlue.Models.Deal.ImportExcelError { RowNumber=rowNumber };
							rowErrors=new StringBuilder();

							investorName=row.GetValue(model.InvestorName);
							fundName=row.GetValue(model.FundName);
							decimal.TryParse(row.GetValue(model.CapitalCallAmount),out capitalCallAmount);
							decimal.TryParse(row.GetValue(model.ManagementFeesInterest),out managementFeesInterest);
							decimal.TryParse(row.GetValue(model.InvestedAmountInterest),out investedAmountInterest);
							decimal.TryParse(row.GetValue(model.ManagementFees),out managementFees);
							decimal.TryParse(row.GetValue(model.FundExpenses),out fundExpenses);
							DateTime.TryParse(row.GetValue(model.CapitalCallDate),out capitalCallDate);
							DateTime.TryParse(row.GetValue(model.CapitalCallDueDate),out capitalCallDueDate);

							investor=null;
							fund=null;
							investorFund=null;

							if(string.IsNullOrEmpty(investorName)==false) {
								investor=InvestorRepository.FindInvestor(investorName);
							} else {
								error.Errors.Add(new ErrorInfo(model.InvestorName,"Investor is required"));
							}

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.InvestorName,"Fund is required"));
							}

							if(investor!=null&&fund!=null) {
								investorFund=InvestorRepository.FindInvestorFund(investor.InvestorID,fund.FundID);
							}

							if(error.Errors.Count()==0) {
								if(investorFund==null) {
									error.Errors.Add(new ErrorInfo(model.InvestorName,"Investror Commitment does not exist"));
								}
							}

							if(error.Errors.Count()==0) {

								Models.Entity.CapitalCall capitalCall=CapitalCallRepository.FindCapitalCall(
									(fund!=null?fund.FundID:0),
										   capitalCallDate,
										   capitalCallDueDate,
										   (int)Models.CapitalCall.Enums.CapitalCallType.Manual);

								if(capitalCall==null) {
									capitalCall=new Models.Entity.CapitalCall();
									capitalCall.CreatedBy=Authentication.CurrentUser.UserID;
									capitalCall.CreatedDate=DateTime.Now;
								}

								capitalCall.CapitalCallDate=capitalCallDate;
								capitalCall.CapitalCallDueDate=capitalCallDueDate;
								capitalCall.CapitalCallTypeID=(int)Models.CapitalCall.Enums.CapitalCallType.Manual;
								capitalCall.LastUpdatedBy=Authentication.CurrentUser.UserID;
								capitalCall.LastUpdatedDate=DateTime.Now;
								capitalCall.FundID=(fund!=null?fund.FundID:0);

								capitalCall.InvestedAmountInterest=0;
								capitalCall.CapitalAmountCalled+=capitalCallAmount;
								capitalCall.InvestedAmountInterest=(capitalCall.ExistingInvestmentAmount??0)+investedAmountInterest;
								capitalCall.FundExpenses=(capitalCall.FundExpenses??0)+fundExpenses;
								capitalCall.ManagementFees=(capitalCall.ManagementFees??0)+managementFees;
								capitalCall.ManagementFeeInterest=(capitalCall.ManagementFeeInterest??0)+managementFeesInterest;

								capitalCall.CapitalCallNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallNumber((fund!=null?fund.FundID:0)));

								if(error.Errors.Count()==0) {
									errorInfo=CapitalCallRepository.SaveCapitalCall(capitalCall);
									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.InvestorName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								}

								if(error.Errors.Count()==0) {
									bool isNewItem=false;
									CapitalCallLineItem item=CapitalCallRepository.FindCapitalCallLineItem(capitalCall.CapitalCallID,investor.InvestorID);
									if(item==null) {
										item=new CapitalCallLineItem();
										item.CreatedBy=Authentication.CurrentUser.UserID;
										item.CreatedDate=DateTime.Now;
										isNewItem=true;
									}
									item.LastUpdatedBy=Authentication.CurrentUser.UserID;
									item.LastUpdatedDate=DateTime.Now;
									item.CapitalAmountCalled=capitalCallAmount;
									item.ManagementFeeInterest=managementFeesInterest;
									item.InvestedAmountInterest=investedAmountInterest;
									item.ManagementFees=managementFees;
									item.FundExpenses=fundExpenses;
									item.InvestorID=investor.InvestorID;
									item.CapitalCallID=capitalCall.CapitalCallID;
									errorInfo=CapitalCallRepository.SaveCapitalCallLineItem(item);
									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.InvestorName,ValidationHelper.GetErrorInfo(errorInfo)));
									} else {
										if(isNewItem) {
											if(item.InvestorID>0) {
												if(investorFund!=null) {
													// Reduce investor unfunded amount = investor unfunded amount – capital call amount for investor.
													investorFund.UnfundedAmount=investorFund.UnfundedAmount-item.CapitalAmountCalled;
													errorInfo=InvestorRepository.SaveInvestorFund(investorFund);
													if(errorInfo!=null) {
														error.Errors.Add(new ErrorInfo(model.InvestorName,ValidationHelper.GetErrorInfo(errorInfo)));
													}
												}
											}
										}
									}
								}

							}

							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion

		#region ImportCapitalDistributionExcel

		[HttpPost]
		public ActionResult ImportCapitalDistributionExcel(FormCollection collection) {
			ImportManualCapitalDistributionExcelModel model=new ImportManualCapitalDistributionExcelModel();
			ResultModel resultModel=new ResultModel();
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			int totalPages=0;
			int totalRows=0;
			int completedRows=0;
			int? succssRows=0;
			int? errorRows=0;
			this.TryUpdateModel(model);
			if(ModelState.IsValid) {
				string key=string.Format(EXCELCAPITALCALLERROR_BY_KEY,model.SessionKey);
				List<DeepBlue.Models.Deal.ImportExcelError> errors=cacheManager.Get(key,() => {
					return new List<DeepBlue.Models.Deal.ImportExcelError>();
				});
				DataSet ds=ExcelConnection.ImportExcelDataset(model.SessionKey);
				if(ds!=null) {
					PagingDataTable importExcelTable=null;
					if(ds.Tables[model.ManualCapitalDistributionTableName]!=null) {
						importExcelTable=(PagingDataTable)ds.Tables[model.ManualCapitalDistributionTableName];
					}
					if(importExcelTable!=null) {
						importExcelTable.PageSize=model.PageSize;
						PagingDataTable table=importExcelTable.Skip(model.PageIndex);
						totalPages=importExcelTable.TotalPages;
						totalRows=importExcelTable.TotalRows;
						if(totalPages>model.PageIndex) {
							completedRows=(model.PageIndex*importExcelTable.PageSize);
						} else {
							completedRows=totalRows;
						}

						int rowNumber=0;

						string investorName=string.Empty;
						string fundName=string.Empty;
						decimal capitalDistributionAmount=0;
						decimal returnManagementFees=0;
						decimal returnFundExpenses=0;
						decimal costReturned=0;
						decimal profits=0;
						decimal profitsReturned=0;
						DateTime distributionDate;
						DateTime distributionDueDate;


						DeepBlue.Models.Deal.ImportExcelError error;
						DeepBlue.Models.Entity.Investor investor;
						DeepBlue.Models.Entity.Fund fund;
						DeepBlue.Models.Entity.InvestorFund investorFund;

						EmailAttribute emailValidation=new EmailAttribute();
						ZipAttribute zipAttribute=new ZipAttribute();
						WebAddressAttribute webAttribute=new WebAddressAttribute();
						IEnumerable<ErrorInfo> errorInfo;

						StringBuilder rowErrors;
						foreach(DataRow row in table.Rows) {
							int.TryParse(row.GetValue("RowNumber"),out rowNumber);

							error=new DeepBlue.Models.Deal.ImportExcelError { RowNumber=rowNumber };
							rowErrors=new StringBuilder();

							investorName=row.GetValue(model.InvestorName);
							fundName=row.GetValue(model.FundName);
							decimal.TryParse(row.GetValue(model.CapitalDistributionAmount),out capitalDistributionAmount);
							decimal.TryParse(row.GetValue(model.ReturnManagementFees),out returnManagementFees);
							decimal.TryParse(row.GetValue(model.ReturnFundExpenses),out returnFundExpenses);
							decimal.TryParse(row.GetValue(model.CostReturned),out costReturned);
							decimal.TryParse(row.GetValue(model.Profits),out profits);
							decimal.TryParse(row.GetValue(model.ProfitsReturned),out profitsReturned);
							DateTime.TryParse(row.GetValue(model.DistributionDate),out distributionDate);
							DateTime.TryParse(row.GetValue(model.DistributionDueDate),out distributionDueDate);

							investor=null;
							fund=null;
							investorFund=null;

							if(string.IsNullOrEmpty(investorName)==false) {
								investor=InvestorRepository.FindInvestor(investorName);
							} else {
								error.Errors.Add(new ErrorInfo(model.InvestorName,"Investor is required"));
							}

							if(string.IsNullOrEmpty(fundName)==false) {
								fund=FundRepository.FindFund(fundName);
							} else {
								error.Errors.Add(new ErrorInfo(model.InvestorName,"Fund is required"));
							}

							if(investor!=null&&fund!=null) {
								investorFund=InvestorRepository.FindInvestorFund(investor.InvestorID,fund.FundID);
							}

							if(error.Errors.Count()==0) {
								if(investorFund==null) {
									error.Errors.Add(new ErrorInfo(model.InvestorName,"Investror Commitment does not exist"));
								}
							}

							if(error.Errors.Count()==0) {

								Models.Entity.CapitalDistribution distribution=CapitalCallRepository.FindCapitalDistribution(
									(fund!=null?fund.FundID:0),
										   distributionDate,
										   distributionDueDate,
										   true);

								if(distribution==null) {
									distribution=new Models.Entity.CapitalDistribution();
									distribution.CreatedBy=Authentication.CurrentUser.UserID;
									distribution.CreatedDate=DateTime.Now;
								}

								distribution.CapitalDistributionDate=distributionDate;
								distribution.CapitalDistributionDueDate=distributionDueDate;

								distribution.PreferredReturn=(distribution.PreferredReturn??0)+profitsReturned;
								distribution.ReturnManagementFees=(distribution.ReturnManagementFees??0)+returnManagementFees;
								distribution.ReturnFundExpenses=(distribution.ReturnFundExpenses??0)+returnFundExpenses;
								distribution.Profits=(distribution.Profits??0)+profits;
								distribution.DistributionAmount=distribution.DistributionAmount+capitalDistributionAmount;

								distribution.LastUpdatedBy=Authentication.CurrentUser.UserID;
								distribution.LastUpdatedDate=DateTime.Now;

								distribution.DistributionNumber=Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber((fund!=null?fund.FundID:0)));
								distribution.FundID=(fund!=null?fund.FundID:0);
								distribution.CapitalReturn=(distribution.CapitalReturn??0)+costReturned;
								distribution.IsManual=true;

								if(error.Errors.Count()==0) {
									errorInfo=CapitalCallRepository.SaveCapitalDistribution(distribution);
									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.InvestorName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								}

								if(error.Errors.Count()==0) {
									CapitalDistributionLineItem item=CapitalCallRepository.FindCapitalDistributionLineItem(distribution.CapitalDistributionID,investor.InvestorID);
									if(item==null) {
										item=new CapitalDistributionLineItem();
										item.CreatedBy=Authentication.CurrentUser.UserID;
										item.CreatedDate=DateTime.Now;
									}
									item.CapitalReturn=0;
									item.CreatedBy=Authentication.CurrentUser.UserID;
									item.CreatedDate=DateTime.Now;
									item.LastUpdatedBy=Authentication.CurrentUser.UserID;
									item.LastUpdatedDate=DateTime.Now;
									item.InvestorID=(investor!=null?investor.InvestorID:0);
									item.DistributionAmount=capitalDistributionAmount;
									item.ReturnManagementFees=returnManagementFees;
									item.ReturnFundExpenses=returnFundExpenses;
									item.Profits=profits;
									item.PreferredReturn=profitsReturned;
									item.CapitalReturn=costReturned;
									item.CapitalDistributionID=distribution.CapitalDistributionID;
									errorInfo=CapitalCallRepository.SaveCapitalDistributionLineItem(item);
									if(errorInfo!=null) {
										error.Errors.Add(new ErrorInfo(model.InvestorName,ValidationHelper.GetErrorInfo(errorInfo)));
									}
								}

							}

							StringBuilder sberror=new StringBuilder();
							foreach(var e in error.Errors) {
								sberror.AppendFormat("{0},",e.ErrorMessage);
							}
							importExcelTable.AddError(rowNumber-1,sberror.ToString());
							errors.Add(error);
						}
					}
				}
				if(errors!=null) {
					succssRows=errors.Where(e => e.Errors.Count==0).Count();
					errorRows=errors.Where(e => e.Errors.Count>0).Count();
				}
			} else {
				foreach(var values in ModelState.Values.ToList()) {
					foreach(var err in values.Errors.ToList()) {
						if(string.IsNullOrEmpty(err.ErrorMessage)==false) {
							resultModel.Result+=err.ErrorMessage+"\n";
						}
					}
				}
			}
			return Json(new {
				Result=resultModel.Result,
				TotalRows=totalRows,
				CompletedRows=completedRows,
				TotalPages=totalPages,
				PageIndex=model.PageIndex,
				SuccessRows=succssRows,
				ErrorRows=errorRows
			});
		}

		#endregion


		#endregion

		public ActionResult GetImportErrorExcel(string sessionKey,string tableName) {
			MemoryCacheManager cacheManager=new MemoryCacheManager();
			ActionResult result=null;
			DataSet ds=ExcelConnection.ImportExcelDataset(sessionKey);
			if(ds!=null) {
				PagingDataTable importExcelTable=null;
				if(ds.Tables[tableName]!=null) {
					importExcelTable=(PagingDataTable)ds.Tables[tableName];
				}
				PagingDataTable errorTable=(PagingDataTable)importExcelTable.Clone();
				DataRow[] rows=importExcelTable.Select("ImportError<>''");
				foreach(var row in rows) {
					errorTable.ImportRow(row);
				}
				//if(importExcelTable!=null) {
					result=new ExportExcel { TableName=string.Format("{0}_{1}",tableName,sessionKey),GridData=errorTable };
				//}
			}
			return result;
		}

		private string AppendErrorInfo(int rowNumber,DataTable excelDataTable,List<DeepBlue.Models.Deal.ImportExcelError> errors) {
			StringBuilder errorInfo=new StringBuilder();
			errorInfo.Append("<table cellpadding=0 cellspacing=0 border=0>");
			errorInfo.Append("<thead>");
			errorInfo.Append("<tr>");
			errorInfo.AppendFormat("<th>{0}</th>","RowNumber");
			foreach(DataColumn col in excelDataTable.Columns) {
				errorInfo.AppendFormat("<th>{0}</th>",col.ColumnName);
			}
			errorInfo.Append("</tr>");
			errorInfo.Append("</thead>");
			errorInfo.Append("</tbody>");
			if(errors.Count()>0) {
				foreach(var err in errors) {
					if(err.Errors.Count()>0) {
						errorInfo.Append("<tr>");
						errorInfo.AppendFormat("<td>{0}</td>",err.RowNumber);
						foreach(DataColumn col in excelDataTable.Columns) {
							var errorColumn=err.Errors.Where(e => e.PropertyName==col.ColumnName).FirstOrDefault();
							if(errorColumn!=null) {
								errorInfo.AppendFormat("<td>{0}</td>",errorColumn.ErrorMessage);
							} else {
								errorInfo.AppendFormat("<td>{0}</td>",string.Empty);
							}
						}
						errorInfo.Append("</tr>");
					}
				}
			}
			errorInfo.Append("</tbody>");
			errorInfo.Append("</table>");
			return errorInfo.ToString();
		}

	}
}
