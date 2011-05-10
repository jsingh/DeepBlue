var deal={
	mvcValidation: false
	,onDealSuccess: null
	,init: function () {
		$(document).ready(function () {
			deal.loadFundList();
			$("#Deal_Management").removeClass("subext").addClass("subext3");
			var FullDealList=$("#FullDealList");
			FullDealList.dialog({ title: "Deal List",autoOpen: false,width: 625,modal: true,position: 'top',autoResize: false,open: function () { $("#DealList").flexReload(); } });
			FullDealList.hide();
			var FullFundList=$("#FullFundList");
			FullFundList.dialog({ title: "Fund List",autoOpen: false,width: 625,modal: true,position: 'top',autoResize: false,open: function () { $("#FundList").flexReload(); } });
			FullFundList.hide();
			DeepBlue.layout();
		});
		var modifyDeal=$("#ModifyDealBox");
		var ul=$("#modifyDealUL");
		modifyDeal.empty();ul.show();
		modifyDeal.append(ul);
		var searchDealName=$("#SearchDealName",modifyDeal);
		searchDealName.autocomplete({ source: "/Deal/FindDeals",minLength: 1,select: function (event,ui) { deal.loadDeal(ui.item.id); },appendTo: "body",delay: 300 });
		menu.stopMenuClose=true;
		issuer.isCreateDealPage=true;
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
		$(".content","#DealMain").empty();

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
		$.each(data.DealExpenses,function (index,item) { deal.loadDealExpenseData(item); });
		$.each(data.DealUnderlyingFunds,function (index,item) { deal.loadUnderlyingFundData(item); });
		$.each(data.DealUnderlyingDirects,function (index,item) { deal.loadUnderlyingDirectData(item); });
		var dealMain=$("#DealMain");
		deal.selectValue(dealMain);
		deal.applyDatePicker(dealMain);
		deal.setFundAutoComplete();
		deal.initDealEvents();
		deal.initMVCValidation();
		deal.setIndex($("#tblUnderlyingFund"));
		deal.setIndex($("#tblUnderlyingDirect"));
	}
	,setIndex: function (target) {
		var index=0;
		$("tbody tr",target).each(function () { index=deal.putIndex(this,index); });
		$("tfoot tr",target).each(function () { index=deal.putIndex(this,index); });
	}
	,putIndex: function (tr,index) {
		var spnindex=$("#SpnIndex",tr).get(0);if(spnindex) { index++;spnindex.innerHTML=index+"."; } return index;
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
	,seeFullDeal: function () {
		var FullDealList=$("#FullDealList");
		FullDealList.dialog("open");
	}
	,onDealListSuccess: function () {
		var FullDealList=$("#FullDealList");$("tbody tr","#DealList").click(function () { deal.loadDeal($.trim($("td:eq(0) div",this).html()));FullDealList.dialog("close"); });
	}
	/* End Deal Detail */

	/* Fund Details */
	,seeFullFund: function () {
		var FullFundList=$("#FullFundList");
		FullFundList.dialog("open");
	}
	,onFundListSuccess: function () {
		var FullFundList=$("#FullFundList");
		$("tbody tr","#FundList").click(function () {
			var fundId=$.trim($("td:eq(0) div",this).html());
			var fundName=$.trim($("td:eq(1) div",this).html());
			deal.selectFund(null,fundId,fundName);
			FullFundList.dialog("close");
		});
	}
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
			DealFundList.empty();
			$("#FundListTemplate").tmpl(data).appendTo(DealFundList);
		});
	}
	,changeFund: function (fundId,fundName) {
		$("#FundId").val(fundId);
		$("#lnkFundName").html(fundName);
	}
	,setFundAutoComplete: function () {
		$("#FundName").autocomplete({ source: "/Fund/FindFunds",minLength: 1,select: function (event,ui) { deal.changeFund(ui.item.id,ui.item.label); },appendTo: "body",delay: 300 });
	}
	,checkFund: function (fundTxt) {
		if($.trim(fundTxt.value)=="") {
			$("#FundId").val(0);$("#lnkFundName").empty();
		}
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
		try {
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
		} catch(e) { alert(e); }
	}
	,showElements: function (tr) {
		$(".hide",tr).css("display","block");
		$(".show",tr).css("display","none");
	}
}