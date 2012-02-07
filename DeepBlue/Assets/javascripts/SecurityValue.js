﻿var securityValueReport={
	init: function () {
		jHelper.waterMark();
	}
	,selectFund: function (id) {
		$("#FundId").val(id);
	}
	,onSubmit: function (frm) {
		try {
			var loading=$("#SpnLoading",frm);
			loading.show();
			var target=$("#ReportDetail");
			target.show();
			var grid=$("#SecurityValueList");
			grid.flexRemoveSortClass();
			grid.flexOptions({ sortname: "" });
			securityValueReport.onGridInit();
			var param=$(frm).serializeForm();
			grid.flexOptions({ params: param });
			grid.flexReload();
		} catch(e) {
			jAlert(e);
		}
		return false;
	}
	,print: function () {
		$("#ReportDetail").printArea();
	}
	,onGridSuccess: function (t) {
		$("#SpnLoading").hide();
		var rowsLegnth=$("tbody tr",t).length;
		var reportDetail=$("#ReportDetail");
		reportDetail.hide();
		if(rowsLegnth>0)
			reportDetail.show();
	}
	,onGridInit: function () {
		var t=$("#SecurityValueList");
		$("thead th:eq(2)").addClass("sorted");
		$("thead th:eq(2) div span").addClass("sdesc");
		$("thead th:eq(3)").addClass("sorted");
		$("thead th:eq(3) div span").addClass("sdesc");
	}
	,onTemplate: function (tbody,data) {
		var error="";
		try {
			error=data.Error;
		} catch(e) { alert(e); }
		$("#SpnLoading").hide();
		if($.trim(error)!="")
			jAlert(error);
		else
			$("#GridTemplate").tmpl(data.Data).appendTo(tbody);
	}
	,exportData: function () {
		var exportTypeId=$("#ExportId").val();
		var url="";
		var width=300;var height=200;var left=(screen.availWidth/2)-(width/2);var top=(screen.availHeight/2)-(height/2);var features="width="+width+",height="+height+",left="+left+",top="+top+",location=no,menubar=no,toobar=no,scrollbars=yes,resizable=yes,status=yes";
		if(exportTypeId=="1"||exportTypeId=="4") {
			var frm=$("#frmFeeExpense");
			url="/Report/ExportSecurityValueDetail?FundId="+$("#FundId").val()+"&ExportTypeId="+exportTypeId;
			url+="&StartDate="+$("#StartDate").val()+"&EndDate="+$("#EndDate").val()
			window.open(deepBlue.rootUrl+url,"exportexcel",features);
		}
	}
}