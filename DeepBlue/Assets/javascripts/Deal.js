var deal={
	mvcValidation: false
	,onDealSuccess: null
	,init: function () {
		$(document).ready(function () {
			deal.loadFundList();
			$("#Deal_Management").removeClass("subext").addClass("subext3");
			DeepBlue.layout();
		});
		var modifyDeal=$("#ModifyDealBox");
		modifyDeal.html("<ul><li class='searchdeal'><br/>Search Deal&nbsp;&nbsp;<input type='text' id='SearchDealName' style='width:200px'/></li></ul>");
		var searchDealName=$("#SearchDealName",modifyDeal);
		searchDealName.autocomplete({ source: "/Deal/FindDeals",minLength: 1,select: function (event,ui) { deal.loadDeal(ui.item.id); },appendTo: "body",delay: 300 });
		menu.stopMenuClose=true;
	}
	,initDealEvents: function () {
		$(".expandbtn").toggle(function () {
			var parent=$(this).parent().parent();
			var fname=this.src.substring(this.src.lastIndexOf('/')+1);
			var src=this.src.replace("/"+fname,"");
			fname="S_"+fname.replace("S_","");
			this.src=src+"/"+fname;
			$(".fieldbox",parent).show();
		},function () {
			var parent=$(this).parent().parent();
			var fname=this.src.substring(this.src.lastIndexOf('/')+1);
			var src=this.src.replace("/"+fname,"");
			fname=fname.replace("S_","");
			this.src=src+"/"+fname;
			$(".fieldbox",parent).hide();
		});
	}
	,selectPartner: function (checked) {
		var divPartnerName=document.getElementById("divPartnerName");
		if(checked)
			divPartnerName.style.display="none";
		else
			divPartnerName.style.display="";
	}

	/* Deal Detail */
	,getDealId: function () {
		return parseInt($("#DealId","#NewDeal").val());
	}
	,setDealId: function (dealId) {
		$("#DealId","#NewDeal").val(dealId);
	}
	,loadDeal: function (dealId) {
		var dt=new Date();
		var url="/Deal/FindDeal/?dealId="+dealId+"&t="+dt.getTime();
		$("#NewDeal").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			deal.loadTemplate(data);
		});
	}
	,loadTemplate: function (data) {
		$("#NewDeal").html("");
		$("#DealExpenses").html("");
		$("#DealDocuments").html("");
		$("#DealSellerInfo").html("");
		$("#DealUnderlyingFunds").html("");
		$("#DealUnderlyingDirects").html("");

		$("#DealTemplate").tmpl(data).appendTo("#NewDeal");
		$("#DealExpenseTemplate").tmpl(data).appendTo("#DealExpenses");
		$("#DealDocumentTemplate").tmpl(data).appendTo("#DealDocuments");
		$("#DealSellerInfoTemplate").tmpl(data.SellerInfo).appendTo("#DealSellerInfo");
		$("#DealUnderlyingFundTemplate").tmpl(data).appendTo("#DealUnderlyingFunds");
		$("#DealUnderlyingDirectTemplate").tmpl(data).appendTo("#DealUnderlyingDirects");

		var IsPartneredYes=document.getElementById("IsPartneredYes");
		var IsPartneredNo=document.getElementById("IsPartneredNo");
		IsPartneredYes.checked=data.IsPartnered;
		IsPartneredNo.checked=!data.IsPartnered;
		if(IsPartneredYes.checked) {
			$("#divPartnerName").css("display","");
		}
		$.each(data.DealExpenses,function (index,dealExpense) {
			deal.loadDealExpenseData(dealExpense);
		});
		var dealMain=$("#DealMain");
		deal.selectValue(dealMain);
		deal.applyDatePicker(dealMain);
		deal.initDealEvents();
		deal.initMVCValidation();
	}
	,onCreateDealBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateDealSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		var result=jQuery.trim(UpdateTargetId.html());
		if(result!="") {
			if(result.indexOf("True||")> -1) {
				deal.setDealId(result.split("||")[1]);
				if(deal.onDealSuccess) {
					deal.onDealSuccess();
					deal.onDealSuccess=null;
				}
			} else {
				alert(UpdateTargetId.html())
			}
		}
	}
	,onDealSubmit: function (formId) {
		return deal.checkForm(document.getElementById(formId));
	}
	/* End Deal Detail */
	/* Deal Expense */
	,deleteDealExpenses: function (id,img) {
		if(confirm("Are you sure you want to delete this deal expense?")) {
			var tr=$(img).parents("tr:first");
			var url="/Deal/DeleteDealClosingCost/"+id;
			$.get(url,function (data) {
				tr.prev().remove();
				tr.remove();
				deal.calcTotalExpense();
			});
		}
	}
	,editDealExpense: function (img) {
		var tr=$(img).parents("tr:first");
		if(img.src.indexOf('save.png')> -1) {
			deal.saveExpense(tr);
		} else {
			img.src="/Assets/images/save.png";
			deal.showElements(tr);
		}
	}
	,showElements: function (tr) {
		$(".hide",tr).css("display","block");
		$(".show",tr).css("display","none");
	}
	,addDealExpense: function (img) {
		var tr=$(img).parents("tr:first");
		deal.saveExpense(tr);
	}
	,saveExpense: function (tr) {
		var dealId=deal.getDealId();
		var spnAjax=$("#spnAjax",tr).show();
		spnAjax.show();
		if(dealId>0) {
			var param=[{ name: "DealClosingCostId",value: $(":input[name='DealClosingCostId']",tr).val() }
						,{ name: "DealClosingCostTypeId",value: $(":input[name='DealClosingCostTypeId']",tr).val() }
						,{ name: "Amount",value: $(":input[name='Amount']",tr).val() }
						,{ name: "Date",value: $(":input[name='Date']",tr).val() }
						,{ name: "DealId",value: dealId }
						];
			var url="/Deal/CreateDealExpense";
			$.post(url,param,function (data) {
				spnAjax.hide();
				if(data.indexOf("True||")> -1) {
					var id=data.split("||")[1];
					deal.loadDealExpense(id);
				} else {
					alert(data);
				}
			});
		} else {
			spnAjax.hide();
			deal.onDealSuccess=function () { deal.saveExpense(tr); }
			$("#btnSaveDeal").click();
		}
	}
	,loadDealExpense: function (id) {
		var dt=new Date();
		var url="/Deal/FindDealClosingCost?dealClosingCostId="+id+"&t="+dt.getTime();
		$.getJSON(url,function (data) {
			deal.loadDealExpenseData(data);
		});
	}
	,loadDealExpenseData: function (data) {
		var tbody=$("#tbodyDealExpense");
		var tr=$("#DealExpense_"+data.DealClosingCostId,tbody);
		if(!(tr.get(0))) {
			$("#DealExpensesRowTemplate").tmpl(data).appendTo("#tbodyDealExpense");
		} else {
			tr.prev().remove();
			$("#DealExpensesRowTemplate").tmpl(data).insertAfter(tr);
			tr.remove();
		}
		tr=$("#DealExpense_"+data.DealClosingCostId,tbody);
		$("#SpnAmount",tr).html(jHelper.dollarAmount(data.Amount.toString()));
		var date=jHelper.formatDate(jHelper.parseJSONDate(data.Date));
		$("#SpnDate",tr).html(date);
		$(":input[name='Date']",tr).val(date);
		deal.selectValue(tr);
		deal.applyDatePicker(tr);
		deal.calcTotalExpense();
	}
	,calcTotalExpense: function () {
		var total=0;
		$("tbody tr","#tblDealExpense").each(function () { var amt=parseFloat($("#Amount",this).val());if(isNaN(amt)) { amt=0; } total+=amt; });
		$("#SpnTotalExpenses").html(jHelper.dollarAmount(total.toString()));
	}
	/* End Deal Expense */
	/* Fund Details */
	,loadFundList: function () {
		var pageIndex=1;
		var pageSize=20;
		var sortName="FundName";
		var sortOrder="asc";
		var dt=new Date();
		var url="/Fund/GetAllFunds/?pageIndex="+pageIndex+"&pageSize="+pageSize+"&t="+dt.getTime();
		var FundList=document.getElementById("FundList");
		var DealFundList=$("#DealFundList");
		DealFundList.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			DealFundList.html("");
			var index=0;
			var ul=document.createElement("ul");
			$.each(data.FundDetails,function (index,item) {
				var li=document.createElement("li");
				li.innerHTML="<a href='#' style='cursor:pointer' onclick=\"javascript:deal.selectFund(this,"+item.FundId+",'"+item.FundName+"');\">"+item.FundName+"</a>";
				$(ul).append(li);
			});
			DealFundList.append(ul);
		});
	}
	,selectFund: function (lnk,fundId,fundName) {
		$(".sel","#DealFundList").removeClass("sel");
		$(lnk).addClass("sel");
		var dt=new Date();
		var url="/Deal/FindFund/?fundId="+fundId+"&t="+dt.getTime();
		$("#NewDeal").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			data.FundName=fundName;
			data.FundId=fundId;
			deal.loadTemplate(data);
		});
	}
	/* End Fund */
	/* SellerInfo */
	,saveSellerInfo: function (frm) {
		var dealId=deal.getDealId();
		if(dealId>0) {
			$("#DealId",frm).val(dealId);
			$("#SpnSellerUpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			var url="/Deal/CreateSellerInfo";
			var param=$(frm).serialize();
			$.post(url,param,function (data) {
				$("#SpnSellerUpdateLoading").html("");if(data!="") { alert(data); }
			});
		} else {
			deal.onDealSuccess=function () { deal.saveSellerInfo(frm); }
			$("#btnSaveDeal").click();
		}
		return false;
	}
	/* End SellerInfo */
	,selectValue: function (target) {
		$("select",target).each(function () { var id=parseInt($(this).attr("val"));if(isNaN(id)) { id=0; } this.value=id; });
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () {
			$(this).datepicker({ changeMonth: true,changeYear: true });
		});
	}
	,initMVCValidation: function () {
		if(deal.mvcValidation==false) {
			if(!window.mvcClientValidationMetadata) { window.mvcClientValidationMetadata=[]; }
			window.mvcClientValidationMetadata.push({ "Fields": [{ "FieldName": "FundId","ReplaceValidationMessageContents": true,"ValidationMessageId": "FundId_validationMessage","ValidationRules": [{ "ErrorMessage": "Fund is required","ValidationParameters": { "minimum": 1,"maximum": 2147483647 },"ValidationType": "range" },{ "ErrorMessage": "Fund is required","ValidationParameters": {},"ValidationType": "required" },{ "ErrorMessage": "The field FundId must be a number.","ValidationParameters": {},"ValidationType": "number"}] },{ "FieldName": "DealName","ReplaceValidationMessageContents": true,"ValidationMessageId": "DealName_validationMessage","ValidationRules": [{ "ErrorMessage": "Deal Name is required","ValidationParameters": {},"ValidationType": "required" },{ "ErrorMessage": "Deal Name must be under 50 characters.","ValidationParameters": { "minimumLength": 0,"maximumLength": 50 },"ValidationType": "stringLength"}] },{ "FieldName": "DealNumber","ReplaceValidationMessageContents": true,"ValidationMessageId": "DealNumber_validationMessage","ValidationRules": [{ "ErrorMessage": "Deal Number is required","ValidationParameters": { "minimum": 1,"maximum": 2147483647 },"ValidationType": "range" },{ "ErrorMessage": "Deal Number is required","ValidationParameters": {},"ValidationType": "required" },{ "ErrorMessage": "The field Deal No.- must be a number.","ValidationParameters": {},"ValidationType": "number"}] },{ "FieldName": "PurchaseTypeId","ReplaceValidationMessageContents": true,"ValidationMessageId": "PurchaseTypeId_validationMessage","ValidationRules": [{ "ErrorMessage": "Purchase Type is required","ValidationParameters": {},"ValidationType": "required" },{ "ErrorMessage": "Purchase Type is required","ValidationParameters": { "minimum": 1,"maximum": 2147483647 },"ValidationType": "range" },{ "ErrorMessage": "The field Purchase Type- must be a number.","ValidationParameters": {},"ValidationType": "number"}]}],"FormId": "AddNewDeal","ReplaceValidationSummary": false });
			Sys.Application.remove_load(arguments.callee);
			Sys.Mvc.FormContext._Application_Load();
			deal.mvcValidation==true;
		}
	}
	,checkForm: function (frm) {
		var message='';
		$(".field-validation-error",frm).html("");
		Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
		$(".field-validation-error",frm).each(function () {
			if(this.innerHTML!='') {
				message+=this.innerHTML+"\n";
			}
		});
		if($.trim(message)!="") {
			alert(message);
			return false;
		} else {
			return true;
		}
	}
}