﻿var report={
	init: function () {
		jHelper.waterMark();
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
		var dt=new Date();
		var url="/Report/GetCapitalDistributions?fundId="+id+"&t="+dt.getTime();
		var txt=$("#jqCBSTextBox_CapitalDistributionId");
		txt.val("Loading...");
		var ddl=document.getElementById("CapitalDistributionId");
		ddl.options.length=null;
		var listItem;
		$.getJSON(url,function (data) {
			ddl.options.length=null;
			listItem=new Option("--Select One--","0",false,false);
			ddl.options[0]=listItem;
			for(i=0;i<data.length;i++) {
				listItem=new Option(data[i].Text,data[i].Value,false,false);
				ddl.options[ddl.options.length]=listItem;
			}
			txt.val("--Select One--");
		});
	}
	,onSubmit: function (frm) {
		try {
			var loading=$("#SpnLoading",frm);
			loading.show();
			var target=$("#ReportDetail");
			target.show();
			$.post("/Report/DistributionSummaryList",$(frm).serializeForm(),function (data) {
				loading.hide();
				if($.trim(data.Error)!="") {
					jAlert(data.Error);
				} else {
					target.empty();
					$("#ReportTemplate").tmpl(data.Data).appendTo(target);
				}
			});
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
	,exportDeal: function () {
		var exportTypeId=$("#ExportId").val();
		var url="";
		var features="width="+1+",height="+1;
		if(exportTypeId=="1"||exportTypeId=="4") {
			url="/Report/ExportCashDistributionDetail?CapitalDistributionId="+$("#CapitalDistributionId").val()+"&FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			window.open(url,"exportdeal",features);
		}
	}
}