﻿var dealActivity={
	newCDData: null
	,newPRCDData: null
	,newCCData: null
	,newPRCCData: null
	,newUFVData: null
	,init: function () {
		jHelper.resizeIframe();
		$(document).ready(function () {
			dealActivity.expand();
			jHelper.waterMark($("body"));
		});
	}
	,selectTab: function (type,lnk) {
		var UA=$("#UnderlyingActivity");
		var SA=$("#SecurityActivity");
		var SUD=$("#SearchUDirect");
		$("#UATab").removeClass("select");
		$("#SATab").removeClass("select");
		$(lnk).addClass("select");
		UA.hide();SA.hide();SUD.hide();
		$(".tablnk").removeClass("select");
		switch(type) {
			case "U": UA.show();break;
			case "S": SA.show();SUD.show();break;
		}
	}
	/* Common Functions */
	,applyAutoComplete: function (target) {
		var UnderlyingFundId=$("#UnderlyingFundId",target);
		var FundId=$("#FundId",target);
		var DealId=$("#DealId",target);
		var UnderlyingFundName=$("#UnderlyingFundName",target);
		var FundName=$("#FundName",target);
		var DealName=$("#DealName",target);
		var dealSearch="/Deal/FindFundDeals";
		var ufid=parseInt(UnderlyingFundId.val());
		if(isNaN(ufid)) { ufid=0; }
		var fundSearch="/Fund/FindDealFunds";

		UnderlyingFundName.blur(function () { if($.trim(this.value)=="") { UnderlyingFundId.val(0); } })
		.autocomplete({ source: "/Deal/FindUnderlyingFunds",minLength: 1
		,select: function (event,ui) {
			UnderlyingFundId.val(ui.item.id);
			FundId.val(0);FundName.val("");
			FundName.autocomplete("option","source",fundSearch+"?underlyingFundId="+UnderlyingFundId.val());
		},appendTo: "body",delay: 300
		});

		FundName.blur(function () { if($.trim(this.value)=="") { FundId.val(0);DealId.val(0);DealName.val(""); } })
		.autocomplete({ source: fundSearch+"?underlyingFundId="+ufid,minLength: 1
		,search: function (event,ui) {
			var ufid=parseInt(UnderlyingFundId.val());
			if(isNaN(ufid)) { ufid=0; }
			if(ufid==0) {
				alert("Underlying Fund is required");
				return false;
			}
		},select: function (event,ui) {
			FundId.val(ui.item.id);
			DealId.val(0);DealName.val("");
			DealName.autocomplete("option","source",dealSearch+"?fundId="+FundId.val());
		},appendTo: "body",delay: 300
		});

		DealName.blur(function () { if($.trim(this.value)=="") { DealId.val(0); } })
		.autocomplete({ source: dealSearch+"?fundId="+FundId.val(),minLength: 1
		,search: function (event,ui) {
			var fundId=parseInt(FundId.val());
			if(isNaN(fundId)) { fundId=0; }
			if(fundId==0) {
				alert("Fund is required");
				return false;
			}
		},select: function (event,ui) { DealId.val(ui.item.id); },appendTo: "body",delay: 300
		});
	}
	,setUpRow: function (tr) {
		jHelper.applyDatePicker(tr);
		jHelper.formatDateTxt(tr);
		jHelper.checkValAttr(tr);
		jHelper.formatDollar(tr);
		jHelper.formatDateHtml(tr);
		dealActivity.applyAutoComplete(tr);
	}
	,editRow: function (tr) {
		$(".show",tr).hide();
		$(".hide",tr).show();
		$("#add",tr).show();
	}
	,expand: function () {
		$(".headerbox").click(function () {
			$(this).hide();
			var parent=$(this).parent();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			if(display!="true") {
				detail.hide();
			} else {
				detail.show();
			}
		});
		$(".expandtitle",".expandheader").click(function () {
			var expandheader=$(this).parents(".expandheader:first");
			var parent=$(expandheader).parent();
			expandheader.hide();
			var detail=$(".detail",parent);
			detail.hide();
			$(".headerbox",parent).show();
		});
	}
	,processErrMsg: function (data,frm) {
		var arr=data.split("\n");
		var i;var msg='';var focus=false;
		for(i=0;i<arr.length;i++) {
			var v=arr[i];
			if(v!="") {
				var varr=v.split("||");
				if(focus==false) {
					$(":input[name='"+varr[0]+"']",frm).focus();
					focus=true;
				}
				msg+=varr[1]+"\n";
			}
		}
		alert(msg);
	}
	/* End Common Functions */
}