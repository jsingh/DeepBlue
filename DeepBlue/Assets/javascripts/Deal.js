var deal={
	mvcValidation: false
	,onDealSuccess: null
	,init: function () {
		$(document).ready(function () {
			jHelper.waterMark();
			$("#Deal_Management").removeClass("subext").addClass("subext3");
			var FullDealList=$("#FullDealList");
			FullDealList.dialog({ title: "Deal List",autoOpen: false,width: 625,modal: true,position: 'top',autoResize: false,open: function () { $("#DealList").flexReload(); } });
			FullDealList.hide();
			var FullFundList=$("#FullFundList");
			FullFundList.dialog({ title: "Fund List",autoOpen: false,width: 625,modal: true,position: 'top',autoResize: false,open: function () { $("#FundList").flexReload(); } });
			FullFundList.hide();
		});
	}
	,initDealEvents: function () {
		$(".expandheader").click(function () {
			$(".expandtitle").hide();
			$(".expandimg").show();
			var bolexpandsel=$(this).hasClass("expandsel");
			$(".expandsel").removeClass("expandsel");
			$(".fieldbox").hide();
			$(".expandaddbtn").hide();
			if(!bolexpandsel) {
				$(this).addClass("expandsel");
				$(".rightuarrow").remove();
				var d=document.createElement("div");
				$(d).addClass("rightuarrow");
				$(this).append(d);
				$("#img",this).hide();
				$("#title",this).show();
				$("#title .expandtitle",this).show();
				$(".expandaddbtn",this).show();
				$(".makenew-header",this).show();
				$(".fieldbox",$(this).parent()).show();
				$(".expandaddbtn",$(this).parent()).show().addClass("addbtn-extend");
			} else {
				$(this).removeClass("expandsel");
			}
		});
	}
	,selectPartner: function (checked) {
		var divPartnerName=document.getElementById("divPartnerName");
		if(checked) { divPartnerName.style.display="none"; } else { divPartnerName.style.display="block"; }
	}
	/* Deal Detail */
	,getDealId: function () {
		return parseInt($("#DealId","#NewDeal").val());
	}
	,getFundId: function () {
		return parseInt($("#FundId","#NewDeal").val());
	}
	,setDealId: function (dealId) {
		$("#DealId","#NewDeal").val(dealId);
	}
	,loadDeal: function (dealId) {
		var dt=new Date();
		var url="/Deal/FindDeal/?dealId="+dealId+"&t="+dt.getTime();
		$("#NewDeal").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON(url,function (data) {
			$("#SaveDealBox").show();
			$("#SearchDealName").val(data.DealName);
			$("#btnDummySaveDeal","#SaveDealBox").attr("src","/Assets/images/mdeal.png");
			deal.loadTemplate(data);
		});
	}
	,loadTemplate: function (data) {
		try {
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
				$("#divPartnerName").show();
			} else {
				$("#divPartnerName").hide();
			}
			$.each(data.DealExpenses,function (index,item) { deal.loadDealExpenseData(item); });
			$.each(data.DealUnderlyingFunds,function (index,item) { deal.loadUnderlyingFundData(item); });
			$.each(data.DealUnderlyingDirects,function (index,item) { deal.loadUnderlyingDirectData(item); });

			var trUF=$("tr:first","#MakeNewDUFund");
			deal.applyUFAutocomplete(trUF);
			var trDirect=$("tr:first","#MakeNewDUDirect");
			deal.applyUDAutocomplete(trDirect);

			var dealMain=$("#DealMain");
			dealMain.show();
			deal.selectValue(dealMain);
			jHelper.applyDatePicker(dealMain);
			deal.setFundAutoComplete();
			deal.initDealEvents();
			deal.setIndex($("#tblUnderlyingFund"));
			deal.setIndex($("#tblUnderlyingDirect"));
			deal.calcDUF();
			deal.calcDUD();
			deal.documentSetUp();
			$("#DocumentDate").datepicker({ changeMonth: true,changeYear: true });
		} catch(e) { alert(e); }
	}
	,uploadDocument: function () {
		return false;
		try {
			$.ajaxFileUpload(
			{
				url: '/Deal/CreateDocument',
				secureuri: false,
				fileElementId: 'fileToUpload',
				formId: 'frmDocumentInfo',
				dataType: 'json',
				success: function (data,status) {
					if(typeof (data.error)!='undefined') {
						if(data.error!='') {
							alert(data.error);
						} else {
							alert(data.msg);
						}
					}
				},
				error: function (data,status,e) {
					alert(data.msg+","+status+","+e);
				}
			}
		);
		} catch(e) {
			alert(e);
		}
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

			} else {
				alert(UpdateTargetId.html())
			}
		}
	}
	,onDealSubmit: function (formId) {
		return deal.checkForm(document.getElementById(formId));
	}
	,saveDeal: function () {
		try {
			var frm=$("#AddNewDeal");
			var loading=$("#UpdateLoading");
			loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
			$.post("/Deal/Create",$(frm).serializeArray(),function (data) {
				loading.empty();
				var arr=data.split("||");
				if(arr[0]!="True") {
					alert(data);
				} else {
					deal.setDealId(arr[1]);
					if(deal.onDealSuccess) {
						alert(1);
						deal.onDealSuccess();
						deal.onDealSuccess=null;
					} else {
						alert("Deal Saved");
						$("#SearchDealName").val("");
						$("#M_Fund").val("");
						$("#DealMain").hide();
					}
				}
			});
		} catch(e) { alert(e); }
		return false;
	}
	,seeFullDeal: function () {
		var FullDealList=$("#FullDealList");
		FullDealList.dialog("open");
	}
	,onDealListSuccess: function () {
		var FullDealList=$("#FullDealList");
		$("tbody tr","#DealList").click(function () { deal.loadDeal($.trim($("td:eq(0) div",this).html()));FullDealList.dialog("close"); });
	}
	/* End Deal Detail */
	/* Common functions */
	,showMakeNewHeader: function (id) {
		var makeNew=$("#"+id);
		jHelper.setUpToolTip(makeNew);
		if(makeNew.css("display")=="none") {
			makeNew.css("display","");
			$(":input[type='text']",makeNew).val("");
			$("select",makeNew).val("0");
		} else {
			makeNew.css("display","none");
		}
	}
	/* End Common functions */
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
			deal.selectFund(fundId,fundName);
			FullFundList.dialog("close");
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
	,selectFund: function (fundId,fundName) {
		$(".sel","#DealFundList").removeClass("sel");
		var dt=new Date();
		var url="/Deal/FindFund/?fundId="+fundId+"&t="+dt.getTime();
		$("#NewDeal").html("<center><img src='/Assets/images/ajax.jpg'>&nbsp;Loading...</center>");
		$("#SaveDealBox").show();
		$("#btnDummySaveDeal","#SaveDealBox").attr("src","/Assets/images/cnewdeal.png");
		$.getJSON(url,function (data) {
			data.FundName=fundName;
			data.FundId=fundId;
			$("#M_Fund").val(fundName);
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
			deal.saveDeal();
		}
		return false;
	}
	/* End SellerInfo */
	,selectValue: function (target) {
		$("select",target).each(function () { var id=parseInt($(this).attr("val"));if(isNaN(id)) { id=0; } this.value=id; });
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
	,sellerInfoReset: function () {
		jHelper.resetFields($("#SellerInfo"));
	}
	,showElements: function (tr) {
		$(".hide",tr).css("display","block");
		$(".show",tr).css("display","none");
	}

}