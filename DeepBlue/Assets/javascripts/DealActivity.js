﻿var dealActivity={
	newFLEData: null
	,templates: [
		 { id: "CashDistributionAddTemplate",url: "/Deal/CashDistributionAddTemplate" }
		,{ id: "PRCashDistributionAddTemplate",url: "/Deal/UnderlyingFundPostRecordCashDistribution" }
		,{ id: "CapitalCallAddTemplate",url: "/Deal/CapitalCallAddTemplate" }
		,{ id: "PRCapitalCallAddTemplate",url: "/Deal/UnderlyingFundPostRecordCapitalCall" }
		,{ id: "StockDistributionAddTemplate",url: "/Deal/StockDistributionAddTemplate" }
		,{ id: "StockDistributionDirectTemplate",url: "/Deal/ManualUnderlyingFundStockDistribution" }
		,{ id: "UFValuationAddTemplate",url: "/Deal/UnderlyingFundValuation" }
		]
	,init: function () {
		jHelper.resizeIframe();
		$(document).ready(function () {
			dealActivity.expand();
			var bdy=$("body");
			jHelper.waterMark(bdy);
			jHelper.jqComboBox(bdy);
			//dealActivity.loadTemplate(0);
		});
	}
	,loadTemplate: function (index) {
		if(index<dealActivity.templates.length) {
			var url=dealActivity.templates[index].url;
			var id=dealActivity.templates[index].id;
			$.get(url,function (data) {
				//var script=document.createElement("script");
				//script.id=id;
				//$(script).attr("type","text/x-jquery-tmpl").html(data);
				//$("body").append(script);
				$.tmpl(id, data);
				dealActivity.loadTemplate(index+1);
			});
		}
	}
	,selectTab: function (type,lnk) {
		$(".section-tab").removeClass("section-tab-sel");
		var UA=$("#UnderlyingActivity");
		var SA=$("#SecurityActivity");
		var RE=$("#Reconciliation");
		var SUD=$("#SearchUDirect");
		$(lnk).addClass("section-tab-sel");
		UA.hide();SA.hide();SUD.hide();RE.hide();
		$(".tablnk").removeClass("select");
		switch(type) {
			case "U": UA.show();break;
			case "S": SA.show();SUD.show();break;
			case "R": RE.show();SUD.show();break;
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
		var dealSearch="/Deal/FindDeals";
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
				jAlert("Underlying Fund is required");
				return false;
			}
		},select: function (event,ui) {
			FundId.val(ui.item.id);
			DealId.val(0);DealName.val("");
			DealName.autocomplete("option","source",
			function (request,response) {
				$.getJSON(dealSearch+"?term="+request.term+"&fundId="+FundId.val(),function (data) {
					response(data);
				});
			}
			);
		},appendTo: "body",delay: 300
		});

		DealName.blur(function () { if($.trim(this.value)=="") { DealId.val(0); } })
		.autocomplete({ source:
			function (request,response) {
				$.getJSON(dealSearch+"?term="+request.term+"&fundId="+FundId.val(),function (data) {
					response(data);
				});
			}
		,minLength: 1
		,search: function (event,ui) {
			var fundId=parseInt(FundId.val());
			if(isNaN(fundId)) { fundId=0; }
			if(fundId==0) {
				jAlert("Fund is required");
				return false;
			}
		},select: function (event,ui) { DealId.val(ui.item.id); },appendTo: "body",delay: 300
		});
	}
	,setUpRow: function (tr) {
		dealActivity.applyDatePicker(tr);
		jHelper.formatDateTxt(tr);
		jHelper.checkValAttr(tr);
		jHelper.formatDollar(tr);
		jHelper.formatDateHtml(tr);
		dealActivity.applyAutoComplete(tr);
	}
	,applyDatePicker: function (target) {
		$(".datefield",target).each(function () {
			var findid="";
			if(this.id.indexOf("CC_NoticeDate")>0)
				findid="CC_NoticeDate";
			if(this.id.indexOf("CD_NoticeDate")>0)
				findid="CD_NoticeDate";
			if(findid!="") {
				var curid=this.id;
				$(this).datepicker({ changeMonth: true,changeYear: true,onSelect: function (dateText,inst) {
					$(".datefield",target).each(function () {
						if((this.id.indexOf(findid)>0)&&(this.id!=curid)) {
							$(this).val(dateText);
						}
					});
				}
				});
			} else {
				$(this).datepicker({ changeMonth: true,changeYear: true });
			}
		});
	}
	,editRow: function (tr) {
		$(".show",tr).hide();
		$(".hide",tr).show();
		$("#add",tr).show();
	}
	,expand: function () {
		$(".headerbox").unbind('click').click(function () {
			$(".headerbox").show();
			$(".expandheader").hide();
			$(".detail").hide();
			$(".addbtn").hide();
			$(this).hide();
			var actGroup=$(this).parents(".act-group:first");
			var parent=$(this).parent();
			var actbox=$(parent).parents(".act-box:first");
			var line=$(actbox).next();
			var index=$(".act-box",actGroup).index(actbox);
			if(index>0) {
				var firstActBox=$(".act-box:first",actGroup);
				$(firstActBox).before(actbox);
				$(actbox).after(line);
			}
			$(".addbtn",parent).show();
			$(".expandheader",parent).show();
			var detail=$(".detail",parent);
			var display=detail.attr("issearch");
			if(display!="true") {
				detail.hide();
			} else {
				detail.show();
			}

		});
		$(".expandheader").unbind('click').click(function () {
			var expandheader=$(this);
			var parent=$(expandheader).parent();
			$(".addbtn",parent).hide();
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
		jAlert(msg);
	}
	/* End Common Functions */
}